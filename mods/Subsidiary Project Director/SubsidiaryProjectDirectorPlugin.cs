using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

[BepInPlugin("org.bepinex.plugins.subsidiaryprojectdirector", "Subsidiary Project Director", "0.1.0")]
public class SubsidiaryProjectDirectorPlugin : BaseUnityPlugin
{
    private const string InjectedButtonName = "Button_SubsidiaryProjectDirector";

    private static ManualLogSource log;
    private static publisherScript activeSubsidiary;
    private static roomScript borrowedRoom;
    private static int borrowedRoomOriginalTaskId = -1;
    private static GameObject borrowedRoomOriginalTaskObject;
    private static GameObject temporaryDesignRoomObject;
    private static StartSnapshot pendingStartSnapshot;

    private static readonly FieldInfo TochterfirmaMenuPublisherField = AccessTools.Field(typeof(Menu_Stats_Tochterfirma_Main), "pS_");
    private static readonly MethodInfo SetPointsMethod = AccessTools.Method(typeof(publisherScript), "SetPoints");
    private static readonly MethodInfo SetStudioAufwertungenMethod = AccessTools.Method(typeof(publisherScript), "SetStudioAufwertungen");

    private void Awake()
    {
        log = Logger;
        new Harmony("org.bepinex.plugins.subsidiaryprojectdirector").PatchAll();
        log.LogInfo("Subsidiary Project Director loaded. Press Ctrl+Shift+M on a subsidiary page to open the design menu.");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && IsShortcutHeld())
        {
            TryOpenFromHotkey();
        }

        CleanupStaleDesignContext();
    }

    public static bool HasActiveSubsidiaryContext()
    {
        return activeSubsidiary != null && IsValidSubsidiary(activeSubsidiary);
    }

    public static void InjectButton(Menu_Stats_Tochterfirma_Main menu, publisherScript subsidiary)
    {
        #region debug-point B:inject-button-start
        SubsidiaryProjectDirectorPlugin.log?.LogInfo("[DEBUG] InjectButton - Entering");
        #endregion
        if (menu == null || subsidiary == null || menu.uiObjects == null || menu.uiObjects.Length <= 31)
        {
            #region debug-point B:inject-button-early-return
            SubsidiaryProjectDirectorPlugin.log?.LogInfo("[DEBUG] InjectButton - Early return: invalid params");
            #endregion
            return;
        }

        GameObject template = menu.uiObjects[31];
        if (template == null)
        {
            return;
        }

        // Adjust parent layout group spacing to fit 5 circular buttons nicely
        if (template.transform.parent != null)
        {
            HorizontalLayoutGroup hlg = template.transform.parent.GetComponent<HorizontalLayoutGroup>();
            if (hlg != null)
            {
                hlg.spacing = 10f; // Squeeze layout spacing
                hlg.childAlignment = TextAnchor.MiddleCenter; // Center the row
            }
            else
            {
                GridLayoutGroup glg = template.transform.parent.GetComponent<GridLayoutGroup>();
                if (glg != null)
                {
                    glg.spacing = new Vector2(10f, glg.spacing.y);
                    glg.childAlignment = TextAnchor.MiddleCenter;
                }
            }
        }

        GameObject buttonObject = FindInjectedButton(menu);
        if (buttonObject != null && buttonObject.GetComponent<SubsidiaryDesignButton>() == null)
        {
            UnityEngine.Object.Destroy(buttonObject);
            buttonObject = null;
        }

        if (buttonObject == null)
        {
            buttonObject = CreateFreshCircleButton(template);

            RectTransform templateRect = template.GetComponent<RectTransform>();
            RectTransform buttonRect = buttonObject.GetComponent<RectTransform>();
            if (templateRect != null && buttonRect != null)
            {
                buttonRect.anchorMin = templateRect.anchorMin;
                buttonRect.anchorMax = templateRect.anchorMax;
                buttonRect.pivot = templateRect.pivot;
                buttonRect.sizeDelta = templateRect.sizeDelta;
                buttonRect.anchoredPosition = templateRect.anchoredPosition + new Vector2(0f, 68f);
            }
        }

        SubsidiaryDesignButton marker = buttonObject.GetComponent<SubsidiaryDesignButton>();
        if (marker == null)
        {
            marker = buttonObject.AddComponent<SubsidiaryDesignButton>();
        }
        marker.Target = subsidiary;

        ConfigureButtonVisual(buttonObject);

        Button button = buttonObject.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(marker.HandleClick);
            button.interactable = IsValidSubsidiary(subsidiary);
        }

        buttonObject.SetActive(IsValidSubsidiary(subsidiary));
    }

    public static void RefreshButton(Menu_Stats_Tochterfirma_Main menu)
    {
        if (menu == null)
        {
            return;
        }

        GameObject buttonObject = FindInjectedButton(menu);
        if (buttonObject == null)
        {
            return;
        }

        publisherScript subsidiary = GetMenuPublisher(menu);
        SubsidiaryDesignButton marker = buttonObject.GetComponent<SubsidiaryDesignButton>();
        if (marker != null)
        {
            marker.Target = subsidiary;
        }

        bool valid = IsValidSubsidiary(subsidiary);
        buttonObject.SetActive(valid);
        Button button = buttonObject.GetComponent<Button>();
        if (button != null)
        {
            button.interactable = valid;
        }
    }

    public static void BeginDesignForSubsidiary(publisherScript subsidiary)
    {
        mainScript main = FindMain();
        GUI_Main gui = FindGui();
        if (main == null || gui == null)
        {
            log?.LogWarning("Could not find main/gui objects for subsidiary project design.");
            return;
        }

        if (main.multiplayer)
        {
            gui.MessageBox("Subsidiary Project Director is disabled in multiplayer for now.", closeMenu: false);
            return;
        }

        if (!IsValidSubsidiary(subsidiary))
        {
            gui.MessageBox("This company is not an active owned developer subsidiary.", closeMenu: false);
            return;
        }

        gameScript currentProject = FindActiveSubsidiaryProject(subsidiary);
        if (currentProject != null)
        {
            gui.MessageBox("This subsidiary is already developing a game. Use the trashcan to discard the current project first, then press the design button again.", closeMenu: false);
            return;
        }

        roomScript room = CreateTemporaryDesignRoom();
        if (room == null)
        {
            gui.MessageBox("Could not create a temporary design context for this subsidiary project.", closeMenu: false);
            return;
        }

        activeSubsidiary = subsidiary;
        borrowedRoom = room;
        borrowedRoomOriginalTaskId = room.taskID;
        borrowedRoomOriginalTaskObject = room.taskGameObject;
        pendingStartSnapshot = null;

        CloseAllBackgroundMenus(gui);
        gui.OpenMenu(hideChars: false);
        gui.ActivateMenu(gui.uiObjects[57]);
        gui.uiObjects[57].transform.SetAsLastSibling();
        gui.uiObjects[57].GetComponent<Menu_DevGameMain>().Init(room);

        log?.LogInfo("Opened directed project menu for subsidiary " + subsidiary.GetName() + " using room " + room.myID + ".");
    }

    public static void CancelDesignContext()
    {
        if (activeSubsidiary != null)
        {
            log?.LogInfo("Cancelled directed project context for " + activeSubsidiary.GetName() + ".");
        }

        activeSubsidiary = null;
        CleanupTemporaryDesignRoom();
        borrowedRoom = null;
        borrowedRoomOriginalTaskId = -1;
        borrowedRoomOriginalTaskObject = null;
        pendingStartSnapshot = null;
    }

    public static void CaptureStartSnapshot()
    {
        if (!HasActiveSubsidiaryContext())
        {
            pendingStartSnapshot = null;
            return;
        }

        games games = FindGames();
        HashSet<int> knownGameIds = new HashSet<int>();
        if (games != null && games.arrayGamesScripts != null)
        {
            for (int i = 0; i < games.arrayGamesScripts.Length; i++)
            {
                gameScript game = games.arrayGamesScripts[i];
                if (game != null)
                {
                    knownGameIds.Add(game.myID);
                }
            }
        }

        pendingStartSnapshot = new StartSnapshot
        {
            KnownGameIds = knownGameIds,
            Room = borrowedRoom,
            OriginalTaskId = borrowedRoom != null ? borrowedRoom.taskID : borrowedRoomOriginalTaskId,
            OriginalTaskObject = borrowedRoom != null ? borrowedRoom.taskGameObject : borrowedRoomOriginalTaskObject
        };
    }

    public static void ConvertCreatedGameToSubsidiaryProject()
    {
        if (!HasActiveSubsidiaryContext() || pendingStartSnapshot == null)
        {
            return;
        }

        publisherScript subsidiary = activeSubsidiary;
        StartSnapshot snapshot = pendingStartSnapshot;
        pendingStartSnapshot = null;

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
            pendingStartSnapshot = null;
            return;
        }

        DestroyCreatedTaskForGame(createdGame, snapshot);
        RestoreBorrowedRoom(snapshot);
        RefundPlayerDevelopmentCost(main, createdGame);
        CloseVanillaHypeWindows(gui);

        // Clean up any other active games in development for this subsidiary to avoid parallel development bugs
        if (games != null && games.arrayGamesScripts != null)
        {
            for (int i = 0; i < games.arrayGamesScripts.Length; i++)
            {
                gameScript oldGame = games.arrayGamesScripts[i];
                if (oldGame != null && oldGame.inDevelopment && oldGame.developerID == subsidiary.myID && oldGame.myID != createdGame.myID)
                {
                    if (IsGameTrackedInTeamSlots(oldGame.myID))
                    {
                        log?.LogInfo($"[ProjectDirector] Skipping cleanup of slot-tracked game '{oldGame.myName}' (ID={oldGame.myID})");
                        continue;
                    }
                    oldGame.gameObject.tag = "GameRemoved";
                    UnityEngine.Object.Destroy(oldGame.gameObject);
                }
            }
            games.FindGames();
        }

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

        string message = "Assigned " + createdGame.GetNameWithTag() + " to " + subsidiary.GetName() + ". It is now this subsidiary's current project.";
        if (gui != null)
        {
            gui.MessageBox(message, closeMenu: false);
        }

        log?.LogInfo("Converted game " + createdGame.myID + " to subsidiary project for " + subsidiary.GetName() + ".");
        CancelDesignContext();
    }

    private static bool IsGameTrackedInTeamSlots(int gameId)
    {
        try
        {
            System.Type slotsType = System.Type.GetType("SubsidiaryTeamSlotsPlugin, SubsidiaryTeamSlots");
            if (slotsType == null)
            {
                foreach (var asm in System.AppDomain.CurrentDomain.GetAssemblies())
                {
                    slotsType = asm.GetType("SubsidiaryTeamSlotsPlugin");
                    if (slotsType != null) break;
                }
            }

            if (slotsType != null)
            {
                var stateField = slotsType.GetField("State", BindingFlags.Public | BindingFlags.Static);
                if (stateField != null)
                {
                    object state = stateField.GetValue(null);
                    if (state != null)
                    {
                        var studiosField = state.GetType().GetField("studios", BindingFlags.Public | BindingFlags.Instance);
                        if (studiosField != null)
                        {
                            System.Collections.IEnumerable studios = studiosField.GetValue(state) as System.Collections.IEnumerable;
                            if (studios != null)
                            {
                                foreach (object studio in studios)
                                {
                                    var slotsField = studio.GetType().GetField("slots", BindingFlags.Public | BindingFlags.Instance);
                                    if (slotsField != null)
                                    {
                                        System.Array slots = slotsField.GetValue(studio) as System.Array;
                                        if (slots != null)
                                        {
                                            for (int i = 0; i < slots.Length; i++)
                                            {
                                                object slot = slots.GetValue(i);
                                                if (slot != null)
                                                {
                                                    var gameIdField = slot.GetType().GetField("gameID", BindingFlags.Public | BindingFlags.Instance);
                                                    if (gameIdField != null)
                                                    {
                                                        int slotGameId = (int)gameIdField.GetValue(slot);
                                                        if (slotGameId == gameId)
                                                        {
                                                            return true;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            log?.LogWarning($"Error in IsGameTrackedInTeamSlots: {ex}");
        }
        return false;
    }

    public static bool CheckNachfolgerForSubsidiary(gameScript script)
    {
        int ownerId = GetActiveSubsidiaryId();
        if (ownerId == -1 || script == null)
        {
            return false;
        }

        return script.ownerID == ownerId &&
               !script.inDevelopment &&
               !script.schublade &&
               !script.typ_budget &&
               !script.typ_goty &&
               script.portID == -1 &&
               !script.auftragsspiel &&
               !script.typ_bundle &&
               !script.typ_bundleAddon &&
               !script.f2pConverted &&
               (script.typ_standard || script.typ_nachfolger || script.typ_spinoff) &&
               (!script.pubOffer || script.ownerID == ownerId) &&
               !script.nachfolger_created &&
               !script.GetIpIsInArchiv();
    }

    public static bool CheckRemasterForSubsidiary(gameScript script)
    {
        int ownerId = GetActiveSubsidiaryId();
        if (ownerId == -1 || script == null)
        {
            return false;
        }

        return script.ownerID == ownerId &&
               !script.inDevelopment &&
               !script.schublade &&
               script.gameTyp == 0 &&
               script.portID == -1 &&
               !script.auftragsspiel &&
               (script.typ_standard || script.typ_nachfolger || script.typ_spinoff) &&
               !script.remaster_created &&
               !script.isOnMarket &&
               !script.typ_budget &&
               !script.typ_goty &&
               !script.typ_remaster &&
               !script.typ_mmoaddon &&
               !script.typ_bundleAddon &&
               !script.typ_bundle &&
               (!script.pubOffer || script.ownerID == ownerId) &&
               !script.GetIpIsInArchiv();
    }

    public static bool CheckSpinoffForSubsidiary(gameScript script)
    {
        int ownerId = GetActiveSubsidiaryId();
        if (ownerId == -1 || script == null)
        {
            return false;
        }

        return script.ownerID == ownerId &&
               !script.inDevelopment &&
               script.mainIP == script.myID &&
               (!script.pubOffer || script.ownerID == ownerId) &&
               !script.typ_nachfolger &&
               !script.auftragsspiel &&
               !script.GetIpIsInArchiv();
    }

    public static bool CheckPortForSubsidiary(gameScript script)
    {
        int ownerId = GetActiveSubsidiaryId();
        if (ownerId == -1 || script == null || script.ownerID != ownerId || script.portID != -1)
        {
            return false;
        }

        int existingPorts = 0;
        for (int i = 0; i < script.portExist.Length; i++)
        {
            if (script.portExist[i])
            {
                existingPorts++;
            }
        }

        mainScript main = FindMain();
        if (main != null && !main.unlock_.Get(65))
        {
            existingPorts++;
        }
        if (script.gameTyp == 1 || script.gameTyp == 2)
        {
            existingPorts++;
        }

        return existingPorts < 2 &&
               !script.typ_contractGame &&
               !script.typ_addon &&
               !script.typ_addonStandalone &&
               !script.typ_mmoaddon &&
               !script.typ_bundle &&
               !script.typ_budget &&
               !script.typ_bundleAddon &&
               !script.typ_goty &&
               !script.auftragsspiel &&
               !script.f2pConverted &&
               (!script.pubOffer || script.ownerID == ownerId) &&
               !script.GetIpIsInArchiv();
    }

    private static bool IsValidSubsidiary(publisherScript subsidiary)
    {
        if (subsidiary == null || !subsidiary) return false;
        try
        {
            return subsidiary.isUnlocked &&
                   subsidiary.developer &&
                   subsidiary.IsMyTochterfirma() &&
                   !subsidiary.Geschlossen() &&
                   !subsidiary.TochterfirmaGeschlossen();
        }
        catch
        {
            return false;
        }
    }

    private static bool IsShortcutHeld()
    {
        return (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) &&
               (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
    }

    private static void TryOpenFromHotkey()
    {
        GUI_Main gui = FindGui();
        if (gui == null || gui.uiObjects == null || gui.uiObjects.Length <= 387 || gui.uiObjects[387] == null || !gui.uiObjects[387].activeSelf)
        {
            return;
        }

        Menu_Stats_Tochterfirma_Main menu = gui.uiObjects[387].GetComponent<Menu_Stats_Tochterfirma_Main>();
        publisherScript subsidiary = GetMenuPublisher(menu);
        if (subsidiary != null)
        {
            BeginDesignForSubsidiary(subsidiary);
        }
    }

    private static gameScript FindActiveSubsidiaryProject(publisherScript subsidiary)
    {
        if (subsidiary == null)
        {
            return null;
        }

        gameScript game = subsidiary.FindGameInDevelopment();
        if (game != null)
        {
            return game;
        }

        games games = FindGames();
        if (games == null || games.arrayGamesScripts == null)
        {
            return null;
        }

        for (int i = 0; i < games.arrayGamesScripts.Length; i++)
        {
            gameScript candidate = games.arrayGamesScripts[i];
            if (candidate != null && candidate.inDevelopment && !candidate.isOnMarket && candidate.developerID == subsidiary.myID)
            {
                return candidate;
            }
        }

        return null;
    }

    private static int GetActiveSubsidiaryId()
    {
        return HasActiveSubsidiaryContext() ? activeSubsidiary.myID : -1;
    }

    private static GameObject FindInjectedButton(Menu_Stats_Tochterfirma_Main menu)
    {
        if (menu == null)
        {
            return null;
        }

        if (menu.uiObjects != null && menu.uiObjects.Length > 31 && menu.uiObjects[31] != null)
        {
            Transform parentTrans = menu.uiObjects[31].transform.parent;
            if (parentTrans != null)
            {
                Transform found = parentTrans.Find(InjectedButtonName);
                if (found != null)
                {
                    return found.gameObject;
                }
            }
        }

        SubsidiaryDesignButton[] markers = menu.GetComponentsInChildren<SubsidiaryDesignButton>(true);
        for (int i = 0; i < markers.Length; i++)
        {
            if (markers[i] != null && markers[i].gameObject.name == InjectedButtonName)
            {
                return markers[i].gameObject;
            }
        }

        return null;
    }

    private static void ConfigureButtonVisual(GameObject buttonObject)
    {
        if (buttonObject == null)
        {
            return;
        }

        tooltip tooltip = buttonObject.GetComponent<tooltip>();
        if (tooltip == null)
        {
            tooltip = buttonObject.AddComponent<tooltip>();
        }
        tooltip.c = "Design the next game for this subsidiary.";

        Text[] texts = buttonObject.GetComponentsInChildren<Text>(true);
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].text = "";
        }

        Sprite icon = null;
        games games = FindGames();
        if (games != null && games.gameTypSprites != null && games.gameTypSprites.Length > 0)
        {
            icon = games.gameTypSprites[0];
        }

        if (icon == null)
        {
            return;
        }

        Image[] images = buttonObject.GetComponentsInChildren<Image>(true);
        for (int i = 0; i < images.Length; i++)
        {
            if (images[i] != null && images[i].gameObject != buttonObject)
            {
                images[i].sprite = icon;
                images[i].color = Color.white;
                images[i].enabled = true;
                break;
            }
        }
    }

    private static GameObject CreateFreshCircleButton(GameObject template)
    {
        GameObject buttonObject = new GameObject(InjectedButtonName, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button), typeof(tooltip), typeof(SubsidiaryDesignButton));
        buttonObject.transform.SetParent(template.transform.parent, false);
        buttonObject.transform.SetAsLastSibling();

        Image templateImage = template.GetComponent<Image>();
        Image image = buttonObject.GetComponent<Image>();
        if (templateImage != null)
        {
            image.sprite = templateImage.sprite;
            image.type = templateImage.type;
            image.color = templateImage.color;
            image.material = templateImage.material;
        }

        Button button = buttonObject.GetComponent<Button>();
        button.targetGraphic = image;
        button.transition = Selectable.Transition.ColorTint;

        GameObject iconObject = new GameObject("Icon", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        iconObject.transform.SetParent(buttonObject.transform, false);
        RectTransform iconRect = iconObject.GetComponent<RectTransform>();
        iconRect.anchorMin = new Vector2(0.18f, 0.18f);
        iconRect.anchorMax = new Vector2(0.82f, 0.82f);
        iconRect.offsetMin = Vector2.zero;
        iconRect.offsetMax = Vector2.zero;

        Image iconImage = iconObject.GetComponent<Image>();
        iconImage.preserveAspect = true;
        iconImage.raycastTarget = false;

        return buttonObject;
    }

    private static publisherScript GetMenuPublisher(Menu_Stats_Tochterfirma_Main menu)
    {
        if (menu == null || TochterfirmaMenuPublisherField == null)
        {
            return null;
        }

        return TochterfirmaMenuPublisherField.GetValue(menu) as publisherScript;
    }

    private static roomScript CreateTemporaryDesignRoom()
    {
        CleanupTemporaryDesignRoom();

        temporaryDesignRoomObject = new GameObject("SPD_TemporaryDesignRoom");
        temporaryDesignRoomObject.hideFlags = HideFlags.HideAndDontSave;
        roomScript room = temporaryDesignRoomObject.AddComponent<roomScript>();
        room.myID = -87654321;
        room.typ = 1;
        room.taskID = -1;
        room.taskGameObject = null;
        room.leitenderGamedesigner = -1;
        room.myName = "Subsidiary Design Context";
        room.uiPos = Vector3.zero;
        temporaryDesignRoomObject.SetActive(false);
        return room;
    }

    private static void CleanupTemporaryDesignRoom()
    {
        if (temporaryDesignRoomObject != null)
        {
            UnityEngine.Object.Destroy(temporaryDesignRoomObject);
            temporaryDesignRoomObject = null;
        }
    }

    private static void CleanupStaleDesignContext()
    {
        if (activeSubsidiary == null)
        {
            if (temporaryDesignRoomObject != null)
            {
                CleanupTemporaryDesignRoom();
            }
            return;
        }

        if (pendingStartSnapshot != null)
        {
            return;
        }

        GUI_Main gui = FindGui();
        if (gui == null || gui.uiObjects == null)
        {
            return;
        }

        if (!IsAnyDirectorDesignMenuActive(gui))
        {
            CancelDesignContext();
        }
    }

    private static bool IsAnyDirectorDesignMenuActive(GUI_Main gui)
    {
        return IsUiActive(gui, 56) ||
               IsUiActive(gui, 57) ||
               IsUiActive(gui, 97) ||
               IsUiActive(gui, 98) ||
               IsUiActive(gui, 310) ||
               IsUiActive(gui, 312) ||
               IsUiActive(gui, 313) ||
               IsUiActive(gui, 369) ||
               IsUiActive(gui, 370) ||
               IsUiActive(gui, 441);
    }

    private static bool IsUiActive(GUI_Main gui, int index)
    {
        return gui != null &&
               gui.uiObjects != null &&
               gui.uiObjects.Length > index &&
               gui.uiObjects[index] != null &&
               gui.uiObjects[index].activeSelf;
    }

    private static gameScript FindNewlyCreatedPlayerGame(games games, mainScript main, HashSet<int> knownGameIds)
    {
        gameScript best = null;
        if (games == null || main == null || games.arrayGamesScripts == null)
        {
            return null;
        }

        for (int i = 0; i < games.arrayGamesScripts.Length; i++)
        {
            gameScript game = games.arrayGamesScripts[i];
            if (game == null)
            {
                continue;
            }

            bool known = knownGameIds != null && knownGameIds.Contains(game.myID);
            if (!known && game.inDevelopment && game.developerID == main.myID && game.ownerID == main.myID)
            {
                if (best == null || game.myID > best.myID)
                {
                    best = game;
                }
            }
        }

        return best;
    }

    private static void DestroyCreatedTaskForGame(gameScript game, StartSnapshot snapshot)
    {
        if (game == null)
        {
            return;
        }

        taskGame[] tasks = UnityEngine.Object.FindObjectsOfType<taskGame>();
        for (int i = 0; i < tasks.Length; i++)
        {
            taskGame task = tasks[i];
            if (task != null && task.gameID == game.myID)
            {
                UnityEngine.Object.Destroy(task.gameObject);
            }
        }

        if (snapshot != null && snapshot.Room != null && snapshot.Room.taskID != snapshot.OriginalTaskId)
        {
            GameObject taskObject = GameObject.Find("Task_" + snapshot.Room.taskID);
            if (taskObject != null)
            {
                UnityEngine.Object.Destroy(taskObject);
            }
        }
    }

    private static void RestoreBorrowedRoom(StartSnapshot snapshot)
    {
        if (snapshot == null || snapshot.Room == null)
        {
            return;
        }

        snapshot.Room.taskID = snapshot.OriginalTaskId;
        snapshot.Room.taskGameObject = snapshot.OriginalTaskObject;
    }

    private static void RefundPlayerDevelopmentCost(mainScript main, gameScript game)
    {
        if (main == null || game == null || game.costs_entwicklung <= 0)
        {
            return;
        }

        main.Pay(-game.costs_entwicklung, 10);
    }

    private static void ApplySubsidiaryQuality(publisherScript subsidiary, gameScript game)
    {
        if (subsidiary == null || game == null)
        {
            return;
        }

        ResetPointTotals(game);

        try
        {
            SetStudioAufwertungenMethod?.Invoke(subsidiary, new object[] { game });
            SetPointsMethod?.Invoke(subsidiary, new object[] { game });
        }
        catch (System.Exception ex)
        {
            log?.LogWarning("Could not apply private subsidiary point logic. Using fallback quality. " + ex);
            ApplyFallbackPoints(subsidiary, game);
        }

        if (game.points_gameplay < 1f || game.points_grafik < 1f || game.points_sound < 1f || game.points_technik < 1f)
        {
            ApplyFallbackPoints(subsidiary, game);
        }
    }

    private static void ResetPointTotals(gameScript game)
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
        if (games == null || games.arrayGamesScripts == null)
        {
            return false;
        }

        for (int i = 0; i < games.arrayGamesScripts.Length; i++)
        {
            gameScript game = games.arrayGamesScripts[i];
            if (game != null && game.myID != ignoreGameId && game.ownerID == ownerId && !game.pubOffer && game.maingenre == maingenre && game.subgenre == subgenre)
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsNewTopicCombinationForOwner(games games, int ownerId, int maintopic, int subtopic, int ignoreGameId)
    {
        if (games == null || games.arrayGamesScripts == null)
        {
            return false;
        }

        for (int i = 0; i < games.arrayGamesScripts.Length; i++)
        {
            gameScript game = games.arrayGamesScripts[i];
            if (game != null && game.myID != ignoreGameId && game.ownerID == ownerId && !game.pubOffer && game.gameMainTheme == maintopic && game.gameSubTheme == subtopic)
            {
                return false;
            }
        }

        return true;
    }

    private static void CloseVanillaHypeWindows(GUI_Main gui)
    {
        if (gui == null || gui.uiObjects == null)
        {
            return;
        }

        SetInactiveIfPresent(gui, 233);
        SetInactiveIfPresent(gui, 311);
        SetInactiveIfPresent(gui, 314);
    }

    private static void SetInactiveIfPresent(GUI_Main gui, int index)
    {
        if (gui.uiObjects.Length > index && gui.uiObjects[index] != null && gui.uiObjects[index].activeSelf)
        {
            gui.uiObjects[index].SetActive(false);
        }
    }

    private static void RefreshOpenSubsidiaryScreen(publisherScript subsidiary)
    {
        GUI_Main gui = FindGui();
        if (gui == null || gui.uiObjects == null || gui.uiObjects.Length <= 387 || gui.uiObjects[387] == null || !gui.uiObjects[387].activeSelf)
        {
            return;
        }

        Menu_Stats_Tochterfirma_Main menu = gui.uiObjects[387].GetComponent<Menu_Stats_Tochterfirma_Main>();
        if (menu != null)
        {
            menu.Init(subsidiary);
        }
    }

    private static void CloseAllBackgroundMenus(GUI_Main gui)
    {
        if (gui == null || gui.uiObjects == null)
        {
            return;
        }

        // Close all subsidiary-related menus and any other popup windows
        // that might be blocking the view when opening the game design window
        int[] menuIndicesToClose = new int[]
        {
            // Main subsidiary menus
            385,  // Subsidiary overview list
            387,  // Subsidiary main stats/info window
            393,  // Subsidiary settings
            396,  // Subsidiary revenue/Umsatz
            398,  // Subsidiary Abrechnung
            399,  // Subsidiary topic selection
            400,  // Subsidiary IP focus
            401,  // Subsidiary engine selection
            402,  // Subsidiary platform focus
            403,  // Subsidiary IP trading
            406,  // Subsidiary game development report
            431,  // Subsidiary own publisher settings
        };

        foreach (int index in menuIndicesToClose)
        {
            if (gui.uiObjects.Length > index && gui.uiObjects[index] != null && gui.uiObjects[index].activeSelf)
            {
                gui.DeactivateMenu(gui.uiObjects[index]);
                log?.LogDebug($"Closed UI object at index {index}");
            }
        }

        log?.LogInfo("Closed all subsidiary-related menus before opening project design window.");
    }

    private static mainScript cachedMain;
    private static GUI_Main cachedGui;
    private static games cachedGames;

    private static mainScript FindMain()
    {
        if (cachedMain == null)
        {
            GameObject main = GameObject.FindGameObjectWithTag("Main");
            cachedMain = main != null ? main.GetComponent<mainScript>() : null;
        }
        return cachedMain;
    }

    private static GUI_Main FindGui()
    {
        if (cachedGui == null)
        {
            GameObject gui = GameObject.Find("CanvasInGameMenu");
            cachedGui = gui != null ? gui.GetComponent<GUI_Main>() : null;
        }
        return cachedGui;
    }

    private static games FindGames()
    {
        if (cachedGames == null)
        {
            mainScript main = FindMain();
            cachedGames = main != null ? main.GetComponent<games>() : null;
        }
        return cachedGames;
    }

    private sealed class StartSnapshot
    {
        public HashSet<int> KnownGameIds;
        public roomScript Room;
        public int OriginalTaskId;
        public GameObject OriginalTaskObject;
    }

    private sealed class SubsidiaryDesignButton : MonoBehaviour
    {
        public publisherScript Target;

        public void HandleClick()
        {
            BeginDesignForSubsidiary(Target);
        }
    }

    [HarmonyPatch(typeof(Menu_Stats_Tochterfirma_Main), "Init")]
    private static class MenuStatsTochterfirmaInitPatch
    {
        private static void Postfix(Menu_Stats_Tochterfirma_Main __instance, publisherScript script_)
        {
            InjectButton(__instance, script_);
        }
    }

    [HarmonyPatch(typeof(Menu_Stats_Tochterfirma_Main), "UpdateData")]
    private static class MenuStatsTochterfirmaUpdateDataPatch
    {
        private static void Postfix(Menu_Stats_Tochterfirma_Main __instance)
        {
            RefreshButton(__instance);
        }
    }

    [HarmonyPatch(typeof(Menu_DevGameMain), "BUTTON_Abbrechen")]
    private static class DevGameMainCancelPatch
    {
        private static void Postfix()
        {
            CancelDesignContext();
        }
    }

    [HarmonyPatch(typeof(Menu_DevGame), "BUTTON_Start")]
    private static class DevGameStartPatch
    {
        private static void Prefix()
        {
            CaptureStartSnapshot();
        }

        private static void Postfix()
        {
            ConvertCreatedGameToSubsidiaryProject();
        }
    }

    [HarmonyPatch(typeof(Menu_Dev_NachfolgerSelect), "CheckGameData")]
    private static class NachfolgerCheckPatch
    {
        private static bool Prefix(gameScript script_, ref bool __result)
        {
            if (!HasActiveSubsidiaryContext())
            {
                return true;
            }

            __result = CheckNachfolgerForSubsidiary(script_);
            return false;
        }
    }

    [HarmonyPatch(typeof(Menu_Dev_RemasterSelect), "CheckGameData")]
    private static class RemasterCheckPatch
    {
        private static bool Prefix(gameScript script_, ref bool __result)
        {
            if (!HasActiveSubsidiaryContext())
            {
                return true;
            }

            __result = CheckRemasterForSubsidiary(script_);
            return false;
        }
    }

    [HarmonyPatch(typeof(Menu_Dev_SpinoffSelect), "CheckGameData")]
    private static class SpinoffCheckPatch
    {
        private static bool Prefix(gameScript script_, ref bool __result)
        {
            if (!HasActiveSubsidiaryContext())
            {
                return true;
            }

            __result = CheckSpinoffForSubsidiary(script_);
            return false;
        }
    }

    [HarmonyPatch(typeof(Menu_Dev_PortSelect), "CheckGameData")]
    private static class PortCheckPatch
    {
        private static bool Prefix(gameScript script_, ref bool __result)
        {
            if (!HasActiveSubsidiaryContext())
            {
                return true;
            }

            __result = CheckPortForSubsidiary(script_);
            return false;
        }
    }
}
