using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;

[BepInPlugin("org.bepinex.plugins.studiodirector", "Studio Director", "1.0.0")]
public partial class StudioDirectorPlugin : BaseUnityPlugin
{
    internal static ManualLogSource Log;
    internal static publisherScript ActiveSubsidiary;
    internal static roomScript BorrowedRoom;
    internal static int BorrowedRoomOriginalTaskId = -1;
    internal static GameObject BorrowedRoomOriginalTaskObject;
    internal static GameObject TemporaryDesignRoomObject;
    internal static StartSnapshot PendingStartSnapshot;

    internal static readonly FieldInfo TochterfirmaMenuPublisherField =
        AccessTools.Field(typeof(Menu_Stats_Tochterfirma_Main), "pS_");
    private static readonly MethodInfo SetPointsMethod =
        AccessTools.Method(typeof(publisherScript), "SetPoints");
    private static readonly MethodInfo SetStudioAufwertungenMethod =
        AccessTools.Method(typeof(publisherScript), "SetStudioAufwertungen");

    private static mainScript _cachedMain;
    private static GUI_Main _cachedGui;
    private static games _cachedGames;

    private void Awake()
    {
        Log = Logger;
        new Harmony("org.bepinex.plugins.studiodirector").PatchAll();
        Log.LogInfo("Studio Director loaded. Open a subsidiary, then Ctrl+Shift+M to design its next game.");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && IsShortcutHeld())
            TryOpenFromHotkey();

        CleanupStaleDesignContext();
    }

    internal static bool HasActiveSubsidiaryContext()
    {
        return ActiveSubsidiary != null && IsValidSubsidiary(ActiveSubsidiary);
    }

    internal static int GetActiveSubsidiaryId()
    {
        return HasActiveSubsidiaryContext() ? ActiveSubsidiary.myID : -1;
    }

    private static bool IsShortcutHeld()
    {
        return (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) &&
               (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
    }

    private static void TryOpenFromHotkey()
    {
        GUI_Main gui = FindGui();
        if (gui == null || gui.uiObjects == null || gui.uiObjects.Length <= 387 ||
            gui.uiObjects[387] == null || !gui.uiObjects[387].activeSelf)
            return;

        Menu_Stats_Tochterfirma_Main menu = gui.uiObjects[387].GetComponent<Menu_Stats_Tochterfirma_Main>();
        publisherScript subsidiary = GetMenuPublisher(menu);
        if (subsidiary != null)
            BeginDesignForSubsidiary(subsidiary);
    }

    internal static mainScript FindMain()
    {
        if (_cachedMain == null)
        {
            GameObject go = GameObject.FindGameObjectWithTag("Main");
            _cachedMain = go != null ? go.GetComponent<mainScript>() : null;
        }
        return _cachedMain;
    }

    internal static GUI_Main FindGui()
    {
        if (_cachedGui == null)
        {
            GameObject go = GameObject.Find("CanvasInGameMenu");
            _cachedGui = go != null ? go.GetComponent<GUI_Main>() : null;
        }
        return _cachedGui;
    }

    internal static games FindGames()
    {
        if (_cachedGames == null)
        {
            mainScript main = FindMain();
            _cachedGames = main != null ? main.GetComponent<games>() : null;
        }
        return _cachedGames;
    }

    internal static void ResetCachedObjects()
    {
        _cachedMain = null;
        _cachedGui = null;
        _cachedGames = null;
    }

    internal static void ApplySubsidiaryQuality(publisherScript subsidiary, gameScript game)
    {
        if (subsidiary == null || game == null) return;

        ResetPointTotals(game);

        try
        {
            SetStudioAufwertungenMethod?.Invoke(subsidiary, new object[] { game });
            SetPointsMethod?.Invoke(subsidiary, new object[] { game });
        }
        catch
        {
            Log?.LogWarning("Could not apply private subsidiary point logic. Using fallback quality.");
            ApplyFallbackPoints(subsidiary, game);
        }

        if (game.points_gameplay < 1f || game.points_grafik < 1f ||
            game.points_sound < 1f || game.points_technik < 1f)
            ApplyFallbackPoints(subsidiary, game);
    }
}
