using System.Collections.Generic;
using UnityEngine;

partial class StudioDirectorPlugin
{
    public static void BeginDesignForSubsidiary(publisherScript subsidiary)
    {
        mainScript main = FindMain();
        GUI_Main gui = FindGui();
        if (main == null || gui == null)
        {
            Log?.LogWarning("Could not find main/gui objects for subsidiary project design.");
            return;
        }

        if (main.multiplayer)
        {
            gui.MessageBox("Studio Director is disabled in multiplayer for now.", closeMenu: false);
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
            gui.MessageBox(
                "This subsidiary is already developing a game. " +
                "Use the trashcan to discard the current project first, then try again.",
                closeMenu: false);
            return;
        }

        roomScript room = CreateTemporaryDesignRoom();
        if (room == null)
        {
            gui.MessageBox("Could not create a temporary design context for this subsidiary project.", closeMenu: false);
            return;
        }

        ActiveSubsidiary = subsidiary;
        BorrowedRoom = room;
        BorrowedRoomOriginalTaskId = room.taskID;
        BorrowedRoomOriginalTaskObject = room.taskGameObject;
        PendingStartSnapshot = null;

        CloseAllBackgroundMenus(gui);
        gui.OpenMenu(hideChars: false);
        gui.ActivateMenu(gui.uiObjects[57]);
        gui.uiObjects[57].transform.SetAsLastSibling();
        gui.uiObjects[57].GetComponent<Menu_DevGameMain>().Init(room);

        Log?.LogInfo("Opened directed project menu for subsidiary " + subsidiary.GetName() + " using room " + room.myID + ".");
    }

    public static void CancelDesignContext()
    {
        if (ActiveSubsidiary != null)
            Log?.LogInfo("Cancelled directed project context for " + ActiveSubsidiary.GetName() + ".");

        ActiveSubsidiary = null;
        CleanupTemporaryDesignRoom();
        BorrowedRoom = null;
        BorrowedRoomOriginalTaskId = -1;
        BorrowedRoomOriginalTaskObject = null;
        PendingStartSnapshot = null;
    }

    internal static void CleanupStaleDesignContext()
    {
        if (ActiveSubsidiary == null)
        {
            if (TemporaryDesignRoomObject != null)
                CleanupTemporaryDesignRoom();
            return;
        }

        if (PendingStartSnapshot != null) return;

        GUI_Main gui = FindGui();
        if (gui == null || gui.uiObjects == null) return;

        if (!IsAnyDirectorDesignMenuActive(gui))
            CancelDesignContext();
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

    private static roomScript CreateTemporaryDesignRoom()
    {
        CleanupTemporaryDesignRoom();

        TemporaryDesignRoomObject = new GameObject("SD_TemporaryDesignRoom");
        TemporaryDesignRoomObject.hideFlags = HideFlags.HideAndDontSave;
        roomScript room = TemporaryDesignRoomObject.AddComponent<roomScript>();
        room.myID = -87654321;
        room.typ = 1;
        room.taskID = -1;
        room.taskGameObject = null;
        room.leitenderGamedesigner = -1;
        room.myName = "Studio Director Context";
        room.uiPos = Vector3.zero;
        TemporaryDesignRoomObject.SetActive(false);
        return room;
    }

    private static void CleanupTemporaryDesignRoom()
    {
        if (TemporaryDesignRoomObject != null)
        {
            UnityEngine.Object.Destroy(TemporaryDesignRoomObject);
            TemporaryDesignRoomObject = null;
        }
    }

    internal static void CaptureStartSnapshot()
    {
        if (!HasActiveSubsidiaryContext())
        {
            PendingStartSnapshot = null;
            return;
        }

        games games = FindGames();
        HashSet<int> knownGameIds = new HashSet<int>();
        if (games != null && games.arrayGamesScripts != null)
        {
            for (int i = 0; i < games.arrayGamesScripts.Length; i++)
            {
                gameScript g = games.arrayGamesScripts[i];
                if (g != null) knownGameIds.Add(g.myID);
            }
        }

        PendingStartSnapshot = new StartSnapshot
        {
            KnownGameIds = knownGameIds,
            Room = BorrowedRoom,
            OriginalTaskId = BorrowedRoom != null ? BorrowedRoom.taskID : BorrowedRoomOriginalTaskId,
            OriginalTaskObject = BorrowedRoom != null ? BorrowedRoom.taskGameObject : BorrowedRoomOriginalTaskObject
        };
    }

    internal sealed class StartSnapshot
    {
        internal HashSet<int> KnownGameIds;
        internal roomScript Room;
        internal int OriginalTaskId;
        internal GameObject OriginalTaskObject;
    }
}
