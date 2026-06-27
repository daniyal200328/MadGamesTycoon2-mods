using System;
using System.Collections.Generic;
using UnityEngine;

partial class StudioDirectorPlugin
{
    public static void ConvertCreatedGameToSubsidiaryProject()
    {
        if (!HasActiveSubsidiaryContext() || PendingStartSnapshot == null) return;

        publisherScript subsidiary = ActiveSubsidiary;
        StartSnapshot snapshot = PendingStartSnapshot;
        PendingStartSnapshot = null;

        games games = FindGames();
        mainScript main = FindMain();
        GUI_Main gui = FindGui();
        if (games == null || main == null || subsidiary == null)
        {
            CancelDesignContext();
            return;
        }

        games.FindGames();
        gameScript createdGame = FindNewlyCreatedPlayerGame(games, main, snapshot.KnownGameIds);
        if (createdGame == null)
        {
            PendingStartSnapshot = null;
            return;
        }

        DestroyCreatedTaskForGame(createdGame, snapshot);
        RestoreBorrowedRoom(snapshot);
        RefundPlayerDevelopmentCost(main, createdGame);
        CloseVanillaHypeWindows(gui);

        CleanupParallelDevelopmentGames(games, subsidiary, createdGame);

        createdGame.ownerID = subsidiary.myID;
        createdGame.developerID = subsidiary.myID;
        createdGame.publisherID = -1;
        createdGame.ownerS_ = subsidiary;
        createdGame.devS_ = subsidiary;
        createdGame.pS_ = null;
        createdGame.inDevelopment = true;
        createdGame.schublade = false;
        createdGame.pubOffer = false;
        createdGame.pubAngebot = false;
        createdGame.auftragsspiel = false;
        createdGame.angekuendigt = false;

        createdGame.newGenreCombination = IsNewGenreCombinationForOwner(games, subsidiary.myID, createdGame.maingenre, createdGame.subgenre, createdGame.myID);
        createdGame.newTopicCombination = IsNewTopicCombinationForOwner(games, subsidiary.myID, createdGame.gameMainTheme, createdGame.gameSubTheme, createdGame.myID);

        ApplySubsidiaryQuality(subsidiary, createdGame);
        createdGame.devPoints = 0f;
        createdGame.devPointsStart = 0f;
        createdGame.devPoints_Gesamt = 0f;
        createdGame.devPointsStart_Gesamt = 0f;

        games.FindGames();
        createdGame.SetAsGameInDevelopmentNPC();
        subsidiary.SetNewGameInWeeks(0);
        games.FindGames();
        RefreshOpenSubsidiaryScreen(subsidiary);

        string message = "Assigned " + createdGame.GetNameWithTag() + " to " + subsidiary.GetName() +
                         ". It is now this subsidiary's current project.";
        if (gui != null)
            gui.MessageBox(message, closeMenu: false);

        Log?.LogInfo("Converted game " + createdGame.myID + " to subsidiary project for " + subsidiary.GetName() + ".");
        CancelDesignContext();
    }

    private static gameScript FindNewlyCreatedPlayerGame(games games, mainScript main, HashSet<int> knownGameIds)
    {
        gameScript best = null;
        if (games == null || main == null || games.arrayGamesScripts == null) return null;

        for (int i = 0; i < games.arrayGamesScripts.Length; i++)
        {
            gameScript game = games.arrayGamesScripts[i];
            if (game == null) continue;

            bool known = knownGameIds != null && knownGameIds.Contains(game.myID);
            if (!known && game.inDevelopment && game.developerID == main.myID && game.ownerID == main.myID)
            {
                if (best == null || game.myID > best.myID)
                    best = game;
            }
        }

        return best;
    }

    private static void DestroyCreatedTaskForGame(gameScript game, StartSnapshot snapshot)
    {
        if (game == null) return;

        taskGame[] tasks = UnityEngine.Object.FindObjectsOfType<taskGame>();
        for (int i = 0; i < tasks.Length; i++)
        {
            taskGame task = tasks[i];
            if (task != null && task.gameID == game.myID)
                UnityEngine.Object.Destroy(task.gameObject);
        }

        if (snapshot != null && snapshot.Room != null && snapshot.Room.taskID != snapshot.OriginalTaskId)
        {
            GameObject taskObject = GameObject.Find("Task_" + snapshot.Room.taskID);
            if (taskObject != null)
                UnityEngine.Object.Destroy(taskObject);
        }
    }

    private static void RestoreBorrowedRoom(StartSnapshot snapshot)
    {
        if (snapshot == null || snapshot.Room == null) return;
        snapshot.Room.taskID = snapshot.OriginalTaskId;
        snapshot.Room.taskGameObject = snapshot.OriginalTaskObject;
    }

    private static void RefundPlayerDevelopmentCost(mainScript main, gameScript game)
    {
        if (main == null || game == null || game.costs_entwicklung <= 0) return;
        main.Pay(-game.costs_entwicklung, 10);
    }

    private static void CleanupParallelDevelopmentGames(games games, publisherScript subsidiary, gameScript createdGame)
    {
        if (games == null || games.arrayGamesScripts == null) return;

        for (int i = 0; i < games.arrayGamesScripts.Length; i++)
        {
            gameScript oldGame = games.arrayGamesScripts[i];
            if (oldGame == null || !oldGame.inDevelopment ||
                oldGame.developerID != subsidiary.myID || oldGame.myID == createdGame.myID)
                continue;

            if (IsGameTrackedInTeamSlots(oldGame.myID))
            {
                Log?.LogInfo($"[StudioDirector] Skipping cleanup of slot-tracked game '{oldGame.myName}' (ID={oldGame.myID})");
                continue;
            }

            oldGame.gameObject.tag = "GameRemoved";
            UnityEngine.Object.Destroy(oldGame.gameObject);
        }
    }

    internal static void ResetPointTotals(gameScript game)
    {
        game.points_gameplay = 0f;
        game.points_grafik = 0f;
        game.points_sound = 0f;
        game.points_technik = 0f;
        game.points_bugs = 0f;
        game.points_bugsInvis = 0f;
    }

    private static void ApplyFallbackPoints(publisherScript subsidiary, gameScript game)
    {
        float basePoints = Mathf.Lerp(2500f, 22000f, Mathf.Clamp01(subsidiary.stars / 100f));
        float sizeMultiplier = 1f + Mathf.Clamp(game.gameSize, 0, 5) * 0.18f;
        float randomMultiplier = UnityEngine.Random.Range(0.85f, 1.15f);
        float total = basePoints * sizeMultiplier * randomMultiplier;

        game.points_gameplay = Mathf.Clamp(total, 1f, 99999f);
        game.points_grafik = Mathf.Clamp(total, 1f, 99999f);
        game.points_sound = Mathf.Clamp(total, 1f, 99999f);
        game.points_technik = Mathf.Clamp(total, 1f, 99999f);
        game.points_bugs = 0f;
        game.points_bugsInvis = 0f;
    }

    private static bool IsNewGenreCombinationForOwner(games games, int ownerId, int maingenre, int subgenre, int ignoreGameId)
    {
        if (games == null || games.arrayGamesScripts == null) return false;

        for (int i = 0; i < games.arrayGamesScripts.Length; i++)
        {
            gameScript g = games.arrayGamesScripts[i];
            if (g != null && g.myID != ignoreGameId && g.ownerID == ownerId &&
                !g.pubOffer && g.maingenre == maingenre && g.subgenre == subgenre)
                return false;
        }

        return true;
    }

    private static bool IsNewTopicCombinationForOwner(games games, int ownerId, int maintopic, int subtopic, int ignoreGameId)
    {
        if (games == null || games.arrayGamesScripts == null) return false;

        for (int i = 0; i < games.arrayGamesScripts.Length; i++)
        {
            gameScript g = games.arrayGamesScripts[i];
            if (g != null && g.myID != ignoreGameId && g.ownerID == ownerId &&
                !g.pubOffer && g.gameMainTheme == maintopic && g.gameSubTheme == subtopic)
                return false;
        }

        return true;
    }

    internal static void CloseVanillaHypeWindows(GUI_Main gui)
    {
        if (gui == null || gui.uiObjects == null) return;
        SetInactiveIfPresent(gui, 233);
        SetInactiveIfPresent(gui, 311);
        SetInactiveIfPresent(gui, 314);
    }

    private static void SetInactiveIfPresent(GUI_Main gui, int index)
    {
        if (gui.uiObjects.Length > index && gui.uiObjects[index] != null && gui.uiObjects[index].activeSelf)
            gui.uiObjects[index].SetActive(false);
    }

    internal static void RefreshOpenSubsidiaryScreen(publisherScript subsidiary)
    {
        GUI_Main gui = FindGui();
        if (gui == null || gui.uiObjects == null || gui.uiObjects.Length <= 387 ||
            gui.uiObjects[387] == null || !gui.uiObjects[387].activeSelf)
            return;

        Menu_Stats_Tochterfirma_Main menu = gui.uiObjects[387].GetComponent<Menu_Stats_Tochterfirma_Main>();
        if (menu != null)
            menu.Init(subsidiary);
    }

    internal static void CloseAllBackgroundMenus(GUI_Main gui)
    {
        if (gui == null || gui.uiObjects == null) return;

        int[] menuIndicesToClose =
        {
            385, 387, 393, 396, 398, 399, 400, 401, 402, 403, 406, 431
        };

        foreach (int index in menuIndicesToClose)
        {
            if (gui.uiObjects.Length > index && gui.uiObjects[index] != null && gui.uiObjects[index].activeSelf)
                gui.DeactivateMenu(gui.uiObjects[index]);
        }
    }
}
