using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

[BepInPlugin("org.bepinex.plugins.dynamicsubsidiarytimeline", "Dynamic Subsidiary Timeline", "1.0.0")]
[BepInDependency("org.bepinex.plugins.subsidiaryteamslots", BepInDependency.DependencyFlags.SoftDependency)]
public class DynamicSubsidiaryTimelinePlugin : BaseUnityPlugin
{
    // ───────────────────────────────────────────────────────
    //  Config Entries
    // ───────────────────────────────────────────────────────
    private static ConfigEntry<bool> cfgEnable;
    private static ConfigEntry<bool> cfgApplyOrganic;
    private static ConfigEntry<bool> cfgApplyAcquired;
    private static ConfigEntry<bool> cfgTrendIntegration;
    private static ConfigEntry<bool> cfgRandomVariance;
    private static ConfigEntry<bool> cfgLogCalc;

    private static ConfigEntry<bool> cfgCostsEnable;
    private static ConfigEntry<float> cfgCostsIdleDrop;
    private static ConfigEntry<float> cfgCostsAAAAMultiplier;

    // Multi-Team Penalty (resources stretched when running multiple projects simultaneously)
    private static ConfigEntry<bool> cfgMultiTeamPenaltyEnabled;
    private static ConfigEntry<float> cfgMultiTeamPenaltyPercent; // % added to weeks per extra active project (16-22% recommended)

    // Development Duration Multipliers (tf_entwicklungsdauer)
    private static ConfigEntry<float> cfgDevDurationShort;
    private static ConfigEntry<float> cfgDevDurationBalanced;
    private static ConfigEntry<float> cfgDevDurationGenerous;

    internal static ManualLogSource log;

    // ───────────────────────────────────────────────────────
    //  Goodwill Mod Reflection Cache
    // ───────────────────────────────────────────────────────
    private static bool      goodwillModChecked     = false;
    private static FieldInfo goodwillTrendDictField  = null;
    private static Type      goodwillPluginType      = null;

    // ───────────────────────────────────────────────────────
    //  Organic Sale Value Reflection Cache
    // ───────────────────────────────────────────────────────
    private static MethodInfo organicSaleValueMethod  = null;
    private static bool       organicSaleValueChecked = false;

    // ───────────────────────────────────────────────────────
    //  Menu_Statistics_Tochterfirmen Reflection Cache
    // ───────────────────────────────────────────────────────
    private static readonly FieldInfo menuMSField = AccessTools.Field(typeof(Menu_Statistics_Tochterfirmen), "mS_");
    private static readonly FieldInfo menuGuiMainField = AccessTools.Field(typeof(Menu_Statistics_Tochterfirmen), "guiMain_");
    private static readonly FieldInfo menuSfxField = AccessTools.Field(typeof(Menu_Statistics_Tochterfirmen), "sfx_");
    private static readonly FieldInfo menuTSField = AccessTools.Field(typeof(Menu_Statistics_Tochterfirmen), "tS_");
    private static readonly FieldInfo menuGenresField = AccessTools.Field(typeof(Menu_Statistics_Tochterfirmen), "genres_");
    private static readonly FieldInfo menuSearchStringAField = AccessTools.Field(typeof(Menu_Statistics_Tochterfirmen), "searchStringA");

    // ───────────────────────────────────────────────────────
    //  Menu_Stats_Tochterfirma_Main Reflection Cache
    // ───────────────────────────────────────────────────────
    private static readonly FieldInfo mainPSField = AccessTools.Field(typeof(Menu_Stats_Tochterfirma_Main), "pS_");
    private static readonly FieldInfo mainMSField = AccessTools.Field(typeof(Menu_Stats_Tochterfirma_Main), "mS_");
    private static readonly FieldInfo mainTSField = AccessTools.Field(typeof(Menu_Stats_Tochterfirma_Main), "tS_");
    private static readonly FieldInfo mainGenresField = AccessTools.Field(typeof(Menu_Stats_Tochterfirma_Main), "genres_");
    private static readonly FieldInfo mainGuiMainField = AccessTools.Field(typeof(Menu_Stats_Tochterfirma_Main), "guiMain_");
    private static readonly FieldInfo mainNextGameField = AccessTools.Field(typeof(Menu_Stats_Tochterfirma_Main), "nextGame_");
    private static readonly FieldInfo mainSfxField = AccessTools.Field(typeof(Menu_Stats_Tochterfirma_Main), "sfx_");

    // ───────────────────────────────────────────────────────
    //  Menu_Stats_TochterfirmaSettings Reflection Cache
    // ───────────────────────────────────────────────────────
    private static readonly FieldInfo mainGameObjectSettingsField = AccessTools.Field(typeof(Menu_Stats_TochterfirmaSettings), "main_");
    private static readonly FieldInfo mainMSSettingsField = AccessTools.Field(typeof(Menu_Stats_TochterfirmaSettings), "mS_");
    private static readonly FieldInfo mainTSSettingsField = AccessTools.Field(typeof(Menu_Stats_TochterfirmaSettings), "tS_");
    private static readonly FieldInfo mainGenresSettingsField = AccessTools.Field(typeof(Menu_Stats_TochterfirmaSettings), "genres_");
    private static readonly FieldInfo mainGamesSettingsField = AccessTools.Field(typeof(Menu_Stats_TochterfirmaSettings), "games_");
    private static readonly FieldInfo mainGuiMainSettingsField = AccessTools.Field(typeof(Menu_Stats_TochterfirmaSettings), "guiMain_");
    private static readonly FieldInfo mainSfxSettingsField = AccessTools.Field(typeof(Menu_Stats_TochterfirmaSettings), "sfx_");



    // ───────────────────────────────────────────────────────
    //  Game Size Tables (Index 0=B, 1=B+, 2=A, 3=AA, 4=AAA, 5=AAAA)
    // ───────────────────────────────────────────────────────
    private static readonly string[] SizeLabels       = { "B", "B+", "A", "AA", "AAA", "AAAA" };
    private static ConfigEntry<long> cfgMaxUpkeepCap;
    private static ConfigEntry<float> cfgBaseInflationRate;
    private static readonly ConfigEntry<float>[] cfgBaseMidpointWeeks = new ConfigEntry<float>[6];
    private static readonly ConfigEntry<int>[]   cfgHardFloorWeeks    = new ConfigEntry<int>[6];
    private static readonly ConfigEntry<int>[]   cfgHardCeilWeeks     = new ConfigEntry<int>[6];

    // GUI Overlay State
    private static bool showConfigWindow = false;
    private static Rect configWindowRect = new Rect(100, 100, 480, 560);
    private static string upkeepCapString = "";
    private static string inflationRateString = "";
    private static string multiTeamPenaltyPercentString = "";
    private static string devDurationShortString = "";
    private static string devDurationBalancedString = "";
    private static string devDurationGenerousString = "";
    private static string[][] timelineStrings = null;

    // Upgrade tracking state
    private static int preUpgradeStars = -1;
    private static int preUpgradeSpeed = -1;
    private static bool preUpgradeIsStudioLevel = false;
    private static int preUpgradeSlotIndex = -1;

    // Team Slots Mod Integration (direct API — no reflection needed; types are public)
    private static bool teamSlotsAvailable = false;
    private static bool teamSlotsChecked = false;

    private static bool IsTeamSlotsLoaded()
    {
        if (teamSlotsChecked) return teamSlotsAvailable;
        teamSlotsChecked = true;
        try
        {
            // Calling GetStudioSlotData with a dummy ID just to probe availability
            var _ = SubsidiaryTeamSlotsPlugin.GetStudioSlotData(-999);
            teamSlotsAvailable = true;
            if (log != null) log.LogInfo("[Timeline] Team Slots mod detected — using direct API.");
        }
        catch (Exception)
        {
            teamSlotsAvailable = false;
            if (log != null) log.LogInfo("[Timeline] Team Slots mod NOT detected — using vanilla timeline only.");
        }
        return teamSlotsAvailable;
    }

    private static System.Type GetTeamSlotsType()
    {
        System.Type teamSlotsType = System.Type.GetType("SubsidiaryTeamSlotsPlugin, SubsidiaryTeamSlots");
        if (teamSlotsType == null)
        {
            foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                teamSlotsType = assembly.GetType("SubsidiaryTeamSlotsPlugin");
                if (teamSlotsType != null) break;
            }
        }
        return teamSlotsType;
    }

    private static int GetGameSlotIndex(publisherScript studio, gameScript game)
    {
        if (studio == null || game == null || !IsTeamSlotsLoaded()) return -1;
        try
        {
            SubsidiaryTeamSlotsPlugin.StudioSlotData slotData = SubsidiaryTeamSlotsPlugin.GetStudioSlotData(studio.myID);
            if (slotData == null || slotData.slots == null) return -1;
            for (int i = 0; i < slotData.slots.Length; i++)
            {
                if (slotData.slots[i] != null && slotData.slots[i].gameID == game.myID)
                    return i;
            }
        }
        catch (Exception ex)
        {
            if (log != null) log.LogWarning("[Timeline] GetGameSlotIndex failed: " + ex.Message);
        }
        return -1;
    }

    private static void GetSlotSpecificStats(publisherScript studio, gameScript game, ref int starsCount, ref int speedLevel)
    {
        if (studio == null || !IsTeamSlotsLoaded()) return;
        try
        {
            int slotIdx = -1;
            if (SubsidiaryTeamSlotsPlugin.isCreatingAutonomousSlot)
            {
                slotIdx = SubsidiaryTeamSlotsPlugin.targetSlotIndex;
            }
            else if (SubsidiaryTeamSlotsPlugin.isCreatingPlayerSlotProject)
            {
                slotIdx = SubsidiaryTeamSlotsPlugin.selectedSlotIndex;
            }
            else if (game != null)
            {
                slotIdx = GetGameSlotIndex(studio, game);
            }

            if (slotIdx > 0 && slotIdx < 3)
            {
                SubsidiaryTeamSlotsPlugin.StudioSlotData slotData = SubsidiaryTeamSlotsPlugin.GetStudioSlotData(studio.myID);
                if (slotData != null && slotData.slots != null && slotData.slots[slotIdx] != null)
                {
                    starsCount = slotData.slots[slotIdx].stars;
                    speedLevel = slotData.slots[slotIdx].speed;
                }
            }
        }
        catch (Exception ex)
        {
            if (log != null) log.LogWarning("[Timeline] GetSlotSpecificStats failed: " + ex.Message);
        }
    }

    /// <summary>
    /// After timeline recalc, persist slot weeks and refresh the subsidiary detail UI.
    /// </summary>
    private static void RefreshSubsidiaryDevUI(publisherScript studio)
    {
        if (studio == null) return;

        if (IsTeamSlotsLoaded())
        {
            SubsidiaryTeamSlotsPlugin.InvalidateStudioDevUI();
            if (SubsidiaryTeamSlotsPlugin.currentSaveSlot >= 0)
            {
                SubsidiaryTeamSlotsPlugin.SaveState(SubsidiaryTeamSlotsPlugin.currentSaveSlot);
            }
        }

        try
        {
            Menu_Stats_Tochterfirma_Main menu = UnityEngine.Object.FindObjectOfType<Menu_Stats_Tochterfirma_Main>();
            if (menu == null || !menu.gameObject.activeInHierarchy) return;

            publisherScript menuStudio = mainPSField?.GetValue(menu) as publisherScript;
            if (menuStudio != null && menuStudio.myID == studio.myID)
            {
                menu.UpdateData();
            }
        }
        catch (Exception ex)
        {
            if (log != null) log.LogWarning("[Timeline] RefreshSubsidiaryDevUI failed: " + ex.Message);
        }
    }

    /// <summary>
    /// Reads the remaining/total weeks for a specific game from the Team Slots mod if loaded,
    /// otherwise falls back to vanilla publisherScript fields.
    /// </summary>
    private static void GetGameWeeks(publisherScript studio, gameScript game, out float remaining, out float total)
    {
        remaining = studio.newGameInWeeks;
        total = studio.newGameInWeeksORG;

        if (game == null) return;

        if (!IsTeamSlotsLoaded()) return;

        try
        {
            SubsidiaryTeamSlotsPlugin.StudioSlotData slotData = SubsidiaryTeamSlotsPlugin.GetStudioSlotData(studio.myID);
            if (slotData == null)
            {
                remaining = 0f;
                total = 0f;
                return;
            }
            SubsidiaryTeamSlotsPlugin.SlotData[] slots = slotData.slots;
            if (slots == null)
            {
                remaining = 0f;
                total = 0f;
                return;
            }
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] != null && slots[i].gameID == game.myID)
                {
                    remaining = slots[i].remainingWeeks;
                    total = slots[i].totalWeeks;
                    return;
                }
            }
            // Game not found in slots - return 0f to indicate newly initializing project
            remaining = 0f;
            total = 0f;
        }
        catch (Exception ex)
        {
            if (log != null) log.LogWarning("[Timeline] GetGameWeeks failed: " + ex.Message);
            remaining = 0f;
            total = 0f;
        }
    }

    /// <summary>
    /// Writes remaining/total weeks for a specific game into the Team Slots mod if loaded,
    /// otherwise falls back to vanilla publisherScript fields.
    /// </summary>
    private static void SetGameWeeks(publisherScript studio, gameScript game, float remaining, float total)
    {
        if (game != null && IsTeamSlotsLoaded())
        {
            // Team Slots path: write only into slot data.
            // Team Slots manages newGameInWeeks itself (sets to 9999 to suppress vanilla,
            // then re-syncs from slot data for UI display). Writing here causes a 1-frame 9999 flicker.
            try
            {
                SubsidiaryTeamSlotsPlugin.StudioSlotData slotData = SubsidiaryTeamSlotsPlugin.GetStudioSlotData(studio.myID);
                if (slotData != null && slotData.slots != null)
                {
                    for (int i = 0; i < slotData.slots.Length; i++)
                    {
                        if (slotData.slots[i] != null && slotData.slots[i].gameID == game.myID)
                        {
                            slotData.slots[i].remainingWeeks = remaining;
                            slotData.slots[i].totalWeeks     = total;
                            if (log != null && cfgLogCalc.Value)
                                log.LogInfo(string.Format("[Timeline] SetGameWeeks slot[{0}] game={1} rem={2:F1} tot={3:F1}",
                                    i, game.myID, remaining, total));
                            return;
                        }
                    }
                    // Game not yet assigned to a slot (race: slot not created yet).
                    // Fall through to vanilla write so Team Slots' hook can read it.
                    if (log != null && cfgLogCalc.Value)
                        log.LogInfo(string.Format("[Timeline] SetGameWeeks: game {0} not in any slot yet for studio {1}, writing vanilla fields as handoff.", game.myID, studio.myID));
                }
            }
            catch (Exception ex)
            {
                if (log != null) log.LogWarning("[Timeline] SetGameWeeks slot write failed: " + ex.Message);
            }
        }

        // Vanilla path (no Team Slots, or game not yet in a slot — write vanilla fields as handoff)
        studio.newGameInWeeks       = Mathf.RoundToInt(remaining);
        studio.newGameInWeeksORG    = Mathf.RoundToInt(total);
    }

    /// <summary>
    /// Recalculates game development points (devPoints_Gesamt) based on the new total week count.
    /// Preserves the progress fraction so that the same percentage of work remains.
    /// This makes the game complete faster when studio speed/stars increase.
    /// </summary>
    private static void RecalculateGameDevPoints(gameScript game, float newTotalWeeks, float progressFraction)
    {
        if (game == null) return;

        try
        {
            // Scale devPoints proportionally to the new week count
            // Get the original devPoints start value
            float originalDevPointsStart = game.devPointsStart_Gesamt;
            
            if (originalDevPointsStart <= 0f) return; // Safety check

            // Find the slot for this game to get the actual previous total weeks
            float oldTotalWeeks = 0f;
            try
            {
                if (IsTeamSlotsLoaded())
                {
                    try
                    {
                        // Use reflection to find the studio that owns this game
                        // (we don't have studio reference here, search globally)
                        games gamesManager = UnityEngine.Object.FindObjectOfType<games>();
                        if (gamesManager != null && gamesManager.arrayGamesScripts != null)
                        {
                            for (int i = 0; i < gamesManager.arrayGamesScripts.Length; i++)
                            {
                                if (gamesManager.arrayGamesScripts[i] == game)
                                {
                                    // Try to find the studio via developerID
                                    int developerID = game.developerID;
                                    if (developerID != -1)
                                    {
                                        SubsidiaryTeamSlotsPlugin.StudioSlotData slotData = SubsidiaryTeamSlotsPlugin.GetStudioSlotData(developerID);
                                        if (slotData != null && slotData.slots != null)
                                        {
                                            for (int s = 0; s < slotData.slots.Length; s++)
                                            {
                                                if (slotData.slots[s] != null && slotData.slots[s].gameID == game.myID)
                                                {
                                                    oldTotalWeeks = slotData.slots[s].totalWeeks;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    catch { }
                }
            }
            catch { }

            // Fall back to heuristic if we couldn't find the previous total weeks
            if (oldTotalWeeks <= 0f)
            {
                // Estimate points per week (rough heuristic: typically 3-5 points per week)
                oldTotalWeeks = Mathf.Max(1f, originalDevPointsStart / 4f);
            }
            
            // Calculate the scaling factor based on actual previous total weeks vs new total weeks
            float weekScaling = newTotalWeeks / Mathf.Max(1f, oldTotalWeeks);
            
            // Recalculate total devPoints based on new week count
            float newTotalDevPoints = originalDevPointsStart * weekScaling;
            game.devPointsStart_Gesamt = newTotalDevPoints;
            
            // Update remaining devPoints to match progress (preserve % complete)
            float newRemainingDevPoints = newTotalDevPoints * (1f - progressFraction);
            game.devPoints_Gesamt = Mathf.Max(1f, newRemainingDevPoints);

            if (log != null && cfgLogCalc.Value)
            {
                log.LogInfo(string.Format(
                    "[Upgrade DevPoints] Game '{0}' | Progress: {1:P0} | " +
                    "OldTotalWeeks: {2:F1} -> NewTotalWeeks: {3:F1} | " +
                    "DevPoints: {4:F0} -> {5:F0} (total), Remaining: {6:F0}",
                    game.GetNameWithTag(),
                    progressFraction,
                    oldTotalWeeks, newTotalWeeks,
                    originalDevPointsStart,
                    game.devPointsStart_Gesamt,
                    newRemainingDevPoints));
            }
        }
        catch (Exception ex)
        {
            if (log != null) log.LogError($"Error in RecalculateGameDevPoints: {ex}");
        }
    }

    /// <summary>
    /// Returns true if any Team Slots slot for this studio contains the given gameID and remaining >= 9990
    /// (the sentinel value the Team Slots mod uses to indicate it is managing that slot).
    /// </summary>
    private static bool IsSlotManagedByTeamSlots(publisherScript studio, gameScript game)
    {
        if (game == null) return false;
        if (!IsTeamSlotsLoaded()) return false;
        try
        {
            SubsidiaryTeamSlotsPlugin.StudioSlotData slotData = SubsidiaryTeamSlotsPlugin.GetStudioSlotData(studio.myID);
            if (slotData == null) return false;
            SubsidiaryTeamSlotsPlugin.SlotData[] slots = slotData.slots;
            if (slots == null) return false;
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] != null && slots[i].gameID == game.myID)
                    return true; // Slot exists — it is managed by Team Slots
            }
        }
        catch { }
        return false;
    }

    private static gameScript FindGameByIDInGlobal(games gamesScript, int gameID)
    {
        if (gamesScript == null || gamesScript.arrayGamesScripts == null) return null;
        for (int i = 0; i < gamesScript.arrayGamesScripts.Length; i++)
        {
            if (gamesScript.arrayGamesScripts[i] != null && gamesScript.arrayGamesScripts[i].myID == gameID)
            {
                return gamesScript.arrayGamesScripts[i];
            }
        }
        return null;
    }

    // ───────────────────────────────────────────────────────
    //  Awake
    // ───────────────────────────────────────────────────────
    private void Awake()
    {
        log = Logger;

        cfgEnable           = Config.Bind("General", "EnableDynamicTimeline",   true,
            "Master toggle for the dynamic subsidiary development timeline system.");
        cfgApplyOrganic     = Config.Bind("General", "ApplyToOrganicStudios",   true,
            "Apply dynamic timeline to player-created (organic) subsidiaries.");
        cfgApplyAcquired    = Config.Bind("General", "ApplyToAcquiredStudios",  true,
            "Apply dynamic timeline to acquired (bought) subsidiaries.");
        cfgTrendIntegration = Config.Bind("General", "TrendIntegrationEnabled", true,
            "Read trend state from Dynamic Studio Goodwill mod (if loaded).");
        cfgRandomVariance   = Config.Bind("General", "RandomVarianceEnabled",   true,
            "Add +/-7 percent random variance to development duration AFTER floor/ceiling caps (can break caps).");
        cfgLogCalc          = Config.Bind("Debug",   "LogCalculations",         true,
            "Log every timeline calculation to the BepInEx console.");

        cfgCostsEnable      = Config.Bind("Costs", "EnableDynamicCosts", true,
            "Enable dynamic monthly administration costs for subsidiaries.");
        cfgCostsIdleDrop    = Config.Bind("Costs", "IdleCostMultiplier", 0.5f,
            "Multiplier applied to monthly costs when a studio has no active game (Idle).");
        cfgCostsAAAAMultiplier = Config.Bind("Costs", "AAAAMultiplier", 5.0f,
            "Max multiplier applied to monthly costs for a AAAA game. (B game is 1.0x).");

        cfgMaxUpkeepCap = Config.Bind("Costs", "MaxUpkeepCap", 17500000L,
            "Maximum monthly administration cost cap for subsidiaries.");

        cfgBaseInflationRate = Config.Bind("Costs", "BaseInflationRate", 2.0f,
            "Base yearly inflation rate in percent.");

        float[] defaultMidpoints = { 12f, 26f, 48f, 102f, 202f, 314f };
        int[] defaultFloors = { 6, 12, 26, 78, 146, 188 };
        int[] defaultCeils = { 32, 52, 102, 198, 384, 720 };

        for (int i = 0; i < 6; i++)
        {
            string label = SizeLabels[i];
            cfgBaseMidpointWeeks[i] = Config.Bind("Timelines", label + "_MidpointWeeks", defaultMidpoints[i], $"Base midpoint weeks for {label} games.");
            cfgHardFloorWeeks[i] = Config.Bind("Timelines", label + "_FloorWeeks", defaultFloors[i], $"Hard floor weeks for {label} games.");
            cfgHardCeilWeeks[i] = Config.Bind("Timelines", label + "_CeilWeeks", defaultCeils[i], $"Hard ceiling weeks for {label} games.");
        }

        // Multi-Team Penalty config: when multiple slots have an active project simultaneously,
        // each additional active project (beyond the first) adds this % overhead to the
        // development duration.  This represents stretched resources and coordination costs.
        //   1 active project  → 0% penalty (no overhead)
        //   2 active projects → cfgMultiTeamPenaltyPercent extra weeks on EACH project
        //   3 active projects → 2 × cfgMultiTeamPenaltyPercent extra weeks on EACH project
        // Default 18% is in the recommended 16-22% range.
        cfgMultiTeamPenaltyEnabled = Config.Bind("MultiTeamPenalty", "Enabled", true,
            "Enable the multi-team simultaneous-development penalty. Disable for vanilla-style 3 parallel AAAA games.");
        cfgMultiTeamPenaltyPercent = Config.Bind("MultiTeamPenalty", "PercentPerExtraProject", 18.0f,
            "Percentage added to development duration for each extra active project (16-22 recommended). Example: 18.0 → 2 projects = +18% weeks, 3 projects = +36% weeks.");

        cfgDevDurationShort    = Config.Bind("DevDuration", "ShortMultiplier",     0.65f, "Multiplier when tf_entwicklungsdauer = 0 (Short)");
        cfgDevDurationBalanced = Config.Bind("DevDuration", "BalancedMultiplier",  0.85f, "Multiplier when tf_entwicklungsdauer = 1 (Balanced)");
        cfgDevDurationGenerous = Config.Bind("DevDuration", "GenerousMultiplier",  1.00f, "Multiplier when tf_entwicklungsdauer = 2 (Generous)");

        new Harmony("org.bepinex.plugins.dynamicsubsidiarytimeline").PatchAll();
        log.LogInfo("Dynamic Subsidiary Timeline Plugin loaded successfully.");
    }

    // ───────────────────────────────────────────────────────
    //  GUI Event Handlers
    // ───────────────────────────────────────────────────────
    private void Update()
    {
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) &&
            (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) &&
            Input.GetKeyDown(KeyCode.T))
        {
            showConfigWindow = !showConfigWindow;
            if (showConfigWindow)
            {
                InitializeUIStrings();
            }
        }
    }

    private void OnGUI()
    {
        if (!showConfigWindow) return;

        configWindowRect = GUILayout.Window(9999, configWindowRect, DrawConfigWindow, "Subsidiary Mod Config Panel");
    }

    private static void InitializeUIStrings()
    {
        upkeepCapString = cfgMaxUpkeepCap.Value.ToString();
        inflationRateString = cfgBaseInflationRate.Value.ToString();
        multiTeamPenaltyPercentString = cfgMultiTeamPenaltyPercent != null ? cfgMultiTeamPenaltyPercent.Value.ToString("F1") : "18.0";
        devDurationShortString     = cfgDevDurationShort    != null ? cfgDevDurationShort.Value.ToString("F2")    : "0.65";
        devDurationBalancedString  = cfgDevDurationBalanced != null ? cfgDevDurationBalanced.Value.ToString("F2") : "0.85";
        devDurationGenerousString  = cfgDevDurationGenerous != null ? cfgDevDurationGenerous.Value.ToString("F2") : "1.00";
        if (timelineStrings == null)
        {
            timelineStrings = new string[6][];
        }
        for (int i = 0; i < 6; i++)
        {
            timelineStrings[i] = new string[3];
            timelineStrings[i][0] = cfgBaseMidpointWeeks[i].Value.ToString();
            timelineStrings[i][1] = cfgHardFloorWeeks[i].Value.ToString();
            timelineStrings[i][2] = cfgHardCeilWeeks[i].Value.ToString();
        }
    }

    private void DrawConfigWindow(int windowID)
    {
        GUILayout.BeginVertical();

        GUILayout.Label("<b>Subsidiary Timeline & Upkeep Settings</b>", GUILayout.ExpandWidth(true));
        GUILayout.Space(10);

        // Upkeep settings
        GUILayout.BeginHorizontal();
        GUILayout.Label("Max Upkeep Cap ($):", GUILayout.Width(170));
        upkeepCapString = GUILayout.TextField(upkeepCapString, GUILayout.Width(130));
        GUILayout.EndHorizontal();

        // Inflation settings
        GUILayout.BeginHorizontal();
        GUILayout.Label("Base Inflation Rate (%):", GUILayout.Width(170));
        inflationRateString = GUILayout.TextField(inflationRateString, GUILayout.Width(130));
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        // ── Multi-Team Penalty (simultaneous development overhead) ──
        GUILayout.Label("<b>Multi-Team Penalty</b> (overhead when multiple teams run projects in parallel)");
        GUILayout.BeginHorizontal();
        bool mtEnabled = cfgMultiTeamPenaltyEnabled != null && cfgMultiTeamPenaltyEnabled.Value;
        bool newMtEnabled = GUILayout.Toggle(mtEnabled, "  Enable Penalty", GUILayout.Width(140));
        if (newMtEnabled != mtEnabled && cfgMultiTeamPenaltyEnabled != null)
        {
            cfgMultiTeamPenaltyEnabled.Value = newMtEnabled;
        }
        GUILayout.Label("Percent per extra project (%):", GUILayout.Width(200));
        multiTeamPenaltyPercentString = GUILayout.TextField(multiTeamPenaltyPercentString, GUILayout.Width(80));
        GUILayout.EndHorizontal();
        if (cfgMultiTeamPenaltyEnabled != null && cfgMultiTeamPenaltyEnabled.Value
            && cfgMultiTeamPenaltyPercent != null)
        {
            float pct = cfgMultiTeamPenaltyPercent.Value;
            GUILayout.Label(string.Format(
                "  <color=#cccccc>1 active = 1.00x | 2 active = {0:F2}x | 3 active = {1:F2}x</color>",
                1f + (pct / 100f),
                1f + 2f * (pct / 100f)));
        }
        else
        {
            GUILayout.Label("  <color=#cccccc>(disabled - no penalty applied)</color>");
        }
        GUILayout.Space(10);

        // ── Development Duration Multipliers (tf_entwicklungsdauer) ──
        GUILayout.Label("<b>Dev Duration Multipliers</b> (applied after DST formula)");
        GUILayout.BeginHorizontal();
        GUILayout.Label("Short (×):", GUILayout.Width(170));
        devDurationShortString = GUILayout.TextField(devDurationShortString, GUILayout.Width(80));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Balanced (×):", GUILayout.Width(170));
        devDurationBalancedString = GUILayout.TextField(devDurationBalancedString, GUILayout.Width(80));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Generous (×):", GUILayout.Width(170));
        devDurationGenerousString = GUILayout.TextField(devDurationGenerousString, GUILayout.Width(80));
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        // Timeline settings table header
        GUILayout.BeginHorizontal();
        GUILayout.Label("<b>Size</b>", GUILayout.Width(60));
        GUILayout.Label("<b>Min (Floor)</b>", GUILayout.Width(100));
        GUILayout.Label("<b>Midpoint</b>", GUILayout.Width(100));
        GUILayout.Label("<b>Max (Ceil)</b>", GUILayout.Width(100));
        GUILayout.EndHorizontal();

        if (timelineStrings != null)
        {
            for (int i = 0; i < 6; i++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(SizeLabels[i], GUILayout.Width(60));
                // Floor
                timelineStrings[i][1] = GUILayout.TextField(timelineStrings[i][1], GUILayout.Width(100));
                // Midpoint
                timelineStrings[i][0] = GUILayout.TextField(timelineStrings[i][0], GUILayout.Width(100));
                // Ceiling
                timelineStrings[i][2] = GUILayout.TextField(timelineStrings[i][2], GUILayout.Width(100));
                GUILayout.EndHorizontal();
            }
        }

        GUILayout.Space(20);

        // Action buttons
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save & Apply"))
        {
            ApplyAndSaveSettings();
            showConfigWindow = false;
        }
        if (GUILayout.Button("Cancel"))
        {
            showConfigWindow = false;
        }
        if (GUILayout.Button("Reset Defaults"))
        {
            ResetToDefaults();
            InitializeUIStrings();
        }
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
        GUI.DragWindow();
    }

    private void ApplyAndSaveSettings()
    {
        if (long.TryParse(upkeepCapString, out long upkeepVal))
        {
            cfgMaxUpkeepCap.Value = upkeepVal;
        }

        if (float.TryParse(inflationRateString, out float inflationVal))
        {
            cfgBaseInflationRate.Value = inflationVal;
        }

        if (cfgMultiTeamPenaltyPercent != null
            && float.TryParse(multiTeamPenaltyPercentString, out float mtPct))
        {
            if (mtPct < 0f) mtPct = 0f;
            if (mtPct > 200f) mtPct = 200f;
            cfgMultiTeamPenaltyPercent.Value = mtPct;
        }

        if (cfgDevDurationShort != null && float.TryParse(devDurationShortString, out float ddShort))
        {
            if (ddShort < 0.1f) ddShort = 0.1f;
            if (ddShort > 5f) ddShort = 5f;
            cfgDevDurationShort.Value = ddShort;
        }
        if (cfgDevDurationBalanced != null && float.TryParse(devDurationBalancedString, out float ddBal))
        {
            if (ddBal < 0.1f) ddBal = 0.1f;
            if (ddBal > 5f) ddBal = 5f;
            cfgDevDurationBalanced.Value = ddBal;
        }
        if (cfgDevDurationGenerous != null && float.TryParse(devDurationGenerousString, out float ddGen))
        {
            if (ddGen < 0.1f) ddGen = 0.1f;
            if (ddGen > 5f) ddGen = 5f;
            cfgDevDurationGenerous.Value = ddGen;
        }

        if (timelineStrings != null)
        {
            for (int i = 0; i < 6; i++)
            {
                if (float.TryParse(timelineStrings[i][0], out float midpoint))
                {
                    cfgBaseMidpointWeeks[i].Value = midpoint;
                }
                if (int.TryParse(timelineStrings[i][1], out int floor))
                {
                    cfgHardFloorWeeks[i].Value = floor;
                }
                if (int.TryParse(timelineStrings[i][2], out int ceil))
                {
                    cfgHardCeilWeeks[i].Value = ceil;
                }
            }
        }

        Config.Save();
        if (log != null) log.LogInfo("Subsidiary Mod config settings successfully applied and saved.");
    }

    private void ResetToDefaults()
    {
        cfgMaxUpkeepCap.Value = 17500000L;
        cfgBaseInflationRate.Value = 2.0f;

        float[] defaultMidpoints = { 12f, 26f, 48f, 102f, 202f, 314f };
        int[] defaultFloors = { 6, 12, 26, 78, 146, 188 };
        int[] defaultCeils = { 32, 52, 102, 198, 384, 720 };

        for (int i = 0; i < 6; i++)
        {
            cfgBaseMidpointWeeks[i].Value = defaultMidpoints[i];
            cfgHardFloorWeeks[i].Value = defaultFloors[i];
            cfgHardCeilWeeks[i].Value = defaultCeils[i];
        }

        if (cfgMultiTeamPenaltyEnabled != null) cfgMultiTeamPenaltyEnabled.Value = true;
        if (cfgMultiTeamPenaltyPercent != null) cfgMultiTeamPenaltyPercent.Value = 18.0f;

        if (cfgDevDurationShort != null)    cfgDevDurationShort.Value    = 0.65f;
        if (cfgDevDurationBalanced != null) cfgDevDurationBalanced.Value = 0.85f;
        if (cfgDevDurationGenerous != null) cfgDevDurationGenerous.Value = 1.00f;

        Config.Save();
        if (log != null) log.LogInfo("Subsidiary Mod config settings reset to default values.");
    }

    // ───────────────────────────────────────────────────────
    //  PATCH: gameScript.SetAsGameInDevelopmentNPC (Postfix)
    // ───────────────────────────────────────────────────────
    [HarmonyPatch(typeof(gameScript), "SetAsGameInDevelopmentNPC")]
    private static class GameScript_SetAsGameInDevelopmentNPC_Patch
    {
        [HarmonyPostfix]
        static void Postfix(gameScript __instance)
        {
            if (log != null) log.LogInfo("[TimelineDebug] SetAsGameInDevelopmentNPC Hook Triggered!");

            if (!cfgEnable.Value) return;

            gameScript game = __instance;
            if (game == null) return;

            // Find the developer studio
            publisherScript studio = FindStudioByID(game.developerID);
            if (studio == null)
            {
                if (log != null) log.LogWarning(string.Format("[TimelineDebug] SetAsGameInDevelopmentNPC: Could not find studio for developerID={0}", game.developerID));
                return;
            }

            bool isOrganic  = IsOrganicStudio(studio);
            bool isAcquired = IsAcquiredSubsidiary(studio);

            if (log != null)
            {
                log.LogInfo(string.Format("[TimelineDebug] SetAsGameInDevelopmentNPC: Studio '{0}' found. isOrganic={1}, isAcquired={2}", studio.GetName(), isOrganic, isAcquired));
            }

            if (!isOrganic && !isAcquired) return;
            if (isOrganic  && !cfgApplyOrganic.Value)  return;
            if (isAcquired && !cfgApplyAcquired.Value) return;

            // ── Gather inputs ────────────────────────────────────
            int   gameSize      = Mathf.Clamp(game.gameSize, 0, 5);
            int   platformCount = CountPlatforms(game);
            int   starsCount    = studio.GetStarsAmount(); // 0 to 5
            int   speedLevel    = studio.developmentSpeed; // 0 to 10 (organic), 0 to 4 (AI)
            
            GetSlotSpecificStats(studio, game, ref starsCount, ref speedLevel);
            
            string trendState   = GetTrendState(studio);

            // ── Compute multipliers ──────────────────────────────
            float starMult     = CalcStarMultiplier(starsCount, gameSize);
            float speedMult    = CalcSpeedMultiplier(speedLevel, isOrganic);
            float platformMult = CalcPlatformMultiplier(platformCount);
            float typeMult     = CalcProjectTypeMultiplier(game);
            float trendMult    = CalcTrendMultiplier(trendState);

            // ── Apply formula (without random variance first) ────
            float baseWeeks  = cfgBaseMidpointWeeks[gameSize].Value;
            float finalWeeks = baseWeeks * starMult * speedMult * platformMult * typeMult * trendMult;

            int weeksInt = Mathf.Clamp(
                Mathf.RoundToInt(finalWeeks),
                cfgHardFloorWeeks[gameSize].Value,
                cfgHardCeilWeeks[gameSize].Value);

            // ── Apply random variance AFTER clamping (can break floor/ceiling) ──
            if (cfgRandomVariance.Value)
            {
                float variancePercent = UnityEngine.Random.Range(-7f, 7f); // -7% to +7%
                int varianceWeeks = Mathf.RoundToInt(weeksInt * (variancePercent / 100f));
                weeksInt += varianceWeeks;
                
                // Ensure at least 1 week minimum
                if (weeksInt < 1) weeksInt = 1;
            }

            // ── Apply Multi-Team Penalty (BEFORE ScaleSubsidiaryTimeline) ──
            // Penalty adds overhead to the timeline when multiple projects run in parallel.
            float mtMultNPC = CalcMultiTeamPenaltyMultiplier(studio);
            if (mtMultNPC > 1.0001f)
            {
                int mtAdjusted = Mathf.RoundToInt(weeksInt * mtMultNPC);
                if (log != null && cfgLogCalc.Value)
                {
                    log.LogInfo(string.Format("[Timeline NPC] Multi-Team Penalty: '{0}' active projects, mult={1:F2} -> {2}w -> {3}w",
                        CountActiveProjects(studio), mtMultNPC, weeksInt, mtAdjusted));
                }
                weeksInt = mtAdjusted;
                if (weeksInt < 1) weeksInt = 1;
            }

            // ── Override values ──────────────────────────────────
            ScaleSubsidiaryTimeline(studio, game, weeksInt);

            // ── Debug Logging ────────────────────────────────────
            if (cfgLogCalc.Value && log != null)
            {
                string projectTypeLabel = GetProjectTypeLabel(game);
                log.LogInfo(string.Format(
                    "[Timeline NPC] Studio: '{0}' | Game: '{1}' (Size={2}, Type={3}) | " +
                    "Stars={4}/5 Speed={5} Plats={6} Trend={7} | " +
                    "Base={8:F1}w x*={9:F2} s*={10:F2} p*={11:F2} t*={12:F2} g*={13:F2} => Final={14}w",
                    studio.GetName(), game.GetNameWithTag(), SizeLabels[gameSize], projectTypeLabel,
                    starsCount, speedLevel, platformCount, trendState,
                    baseWeeks, starMult, speedMult, platformMult, typeMult, trendMult,
                    weeksInt));
            }
        }
    }

    // ───────────────────────────────────────────────────────
    //  PATCH: publisherScript.SetNewGameInWeeks (Fallback Postfix)
    // ───────────────────────────────────────────────────────
    [HarmonyPatch(typeof(publisherScript), "SetNewGameInWeeks")]
    private static class PublisherScript_SetNewGameInWeeks_Patch
    {
        [HarmonyPostfix]
        static void Postfix(publisherScript __instance, int force)
        {
            if (log != null)
            {
                log.LogInfo(string.Format("[TimelineDebug] SetNewGameInWeeks called for studio='{0}' (ID={1}), force={2}",
                    __instance != null ? __instance.GetName() : "null",
                    __instance != null ? __instance.myID : -1,
                    force));
            }

            if (!cfgEnable.Value) return;

            // Override vanilla forced value to ensure our custom timeline calculations always take priority

            publisherScript studio = __instance;
            if (studio == null) return;

            bool isOrganic  = IsOrganicStudio(studio);
            bool isAcquired = IsAcquiredSubsidiary(studio);

            if (!isOrganic && !isAcquired) return;
            if (isOrganic  && !cfgApplyOrganic.Value)  return;
            if (isAcquired && !cfgApplyAcquired.Value) return;

            // Only calculate if a game is actively in development at this studio
            gameScript game = SafeFindGameInDevelopment(studio);
            if (game == null)
            {
                if (log != null) log.LogInfo("[TimelineDebug] SetNewGameInWeeks: No game in development found.");
                return;
            }

            // Get actual remaining/total weeks (vanilla path only)
            GetGameWeeks(studio, game, out float remaining, out float total);

            // Only initialize at the start of development
            if (total > 0f && remaining < total - 0.01f)
            {
                return; // Already in progress, let vanilla decrement naturally!
            }

            // ── Gather inputs ────────────────────────────────────
            int   gameSize      = Mathf.Clamp(game.gameSize, 0, 5);
            int   platformCount = CountPlatforms(game);
            int   starsCount    = studio.GetStarsAmount(); // 0 to 5
            int   speedLevel    = studio.developmentSpeed; // 0 to 10 (organic), 0 to 4 (AI)
            
            GetSlotSpecificStats(studio, game, ref starsCount, ref speedLevel);
            
            string trendState   = GetTrendState(studio);

            // ── Compute multipliers ──────────────────────────────
            float starMult     = CalcStarMultiplier(starsCount, gameSize);
            float speedMult    = CalcSpeedMultiplier(speedLevel, isOrganic);
            float platformMult = CalcPlatformMultiplier(platformCount);
            float typeMult     = CalcProjectTypeMultiplier(game);
            float trendMult    = CalcTrendMultiplier(trendState);

            // ── Apply formula (without random variance first) ────
            float baseWeeks  = cfgBaseMidpointWeeks[gameSize].Value;
            float finalWeeks = baseWeeks * starMult * speedMult * platformMult * typeMult * trendMult;

            int weeksInt = Mathf.Clamp(
                Mathf.RoundToInt(finalWeeks),
                cfgHardFloorWeeks[gameSize].Value,
                cfgHardCeilWeeks[gameSize].Value);

            // ── Apply random variance AFTER clamping (can break floor/ceiling) ──
            if (cfgRandomVariance.Value)
            {
                float variancePercent = UnityEngine.Random.Range(-7f, 7f); // -7% to +7%
                int varianceWeeks = Mathf.RoundToInt(weeksInt * (variancePercent / 100f));
                weeksInt += varianceWeeks;
                
                // Ensure at least 1 week minimum
                if (weeksInt < 1) weeksInt = 1;
            }

            // ── Apply Multi-Team Penalty (BEFORE ScaleSubsidiaryTimeline) ──
            float mtMultFB = CalcMultiTeamPenaltyMultiplier(studio);
            if (mtMultFB > 1.0001f)
            {
                int mtAdjusted = Mathf.RoundToInt(weeksInt * mtMultFB);
                if (log != null && cfgLogCalc.Value)
                {
                    log.LogInfo(string.Format("[Timeline Fallback] Multi-Team Penalty: '{0}' active projects, mult={1:F2} -> {2}w -> {3}w",
                        CountActiveProjects(studio), mtMultFB, weeksInt, mtAdjusted));
                }
                weeksInt = mtAdjusted;
                if (weeksInt < 1) weeksInt = 1;
            }

            // ── Override values ──────────────────────────────────
            ScaleSubsidiaryTimeline(studio, game, weeksInt);

            // ── Debug Logging ────────────────────────────────────
            if (cfgLogCalc.Value && log != null)
            {
                string projectTypeLabel = GetProjectTypeLabel(game);
                log.LogInfo(string.Format(
                    "[Timeline Fallback] Studio: '{0}' | Game: '{1}' (Size={2}, Type={3}) | " +
                    "Stars={4}/5 Speed={5} Plats={6} Trend={7} | " +
                    "Base={8:F1}w x*={9:F2} s*={10:F2} p*={11:F2} t*={12:F2} g*={13:F2} => Final={14}w",
                    studio.GetName(), game.GetNameWithTag(), SizeLabels[gameSize], projectTypeLabel,
                    starsCount, speedLevel, platformCount, trendState,
                    baseWeeks, starMult, speedMult, platformMult, typeMult, trendMult,
                    weeksInt));
            }
        }
    }

    // ───────────────────────────────────────────────────────
    //  PATCH: publisherScript.CreateNewGame2 (Postfix)
    // ───────────────────────────────────────────────────────
    [HarmonyPatch(typeof(publisherScript), "CreateNewGame2")]
    private static class PublisherScript_CreateNewGame2_Patch
    {
        [HarmonyPostfix]
        static void Postfix(publisherScript __instance)
        {
            if (__instance == null || !cfgEnable.Value) return;

            publisherScript studio = __instance;
            bool isOrganic  = IsOrganicStudio(studio);
            bool isAcquired = IsAcquiredSubsidiary(studio);

            if (!isOrganic && !isAcquired) return;
            if (isOrganic  && !cfgApplyOrganic.Value)  return;
            if (isAcquired && !cfgApplyAcquired.Value) return;

            gameScript game = SafeFindGameInDevelopment(studio);
            if (game == null) return;

            // If Team Slots mod is managing this game, skip — it sets timing on slot assignment.
            if (IsSlotManagedByTeamSlots(studio, game))
            {
                if (log != null && cfgLogCalc.Value) log.LogInfo("[Timeline] CreateNewGame2: game is in a Team Slot, skipping vanilla override.");
                return;
            }

            // Get actual remaining/total weeks (vanilla path only)
            GetGameWeeks(studio, game, out float remaining, out float total);

            // Only initialize at the start of development
            if (total > 0f && remaining < total - 0.01f)
            {
                return; // Already in progress, let vanilla decrement naturally!
            }

            int   gameSize      = Mathf.Clamp(game.gameSize, 0, 5);
            int   platformCount = CountPlatforms(game);
            int   starsCount    = studio.GetStarsAmount();
            int   speedLevel    = studio.developmentSpeed;
            
            GetSlotSpecificStats(studio, game, ref starsCount, ref speedLevel);
            
            string trendState   = GetTrendState(studio);

            float starMult     = CalcStarMultiplier(starsCount, gameSize);
            float speedMult    = CalcSpeedMultiplier(speedLevel, isOrganic);
            float platformMult = CalcPlatformMultiplier(platformCount);
            float typeMult     = CalcProjectTypeMultiplier(game);
            float trendMult    = CalcTrendMultiplier(trendState);
            float randomMult   = 1f; // Use 1.0x to prevent weekly random walk timeline fluctuation during active development

            float baseWeeks  = cfgBaseMidpointWeeks[gameSize].Value;
            float finalWeeks = baseWeeks * starMult * speedMult * platformMult * typeMult * trendMult * randomMult;

            int weeksInt = Mathf.Clamp(
                Mathf.RoundToInt(finalWeeks),
                cfgHardFloorWeeks[gameSize].Value,
                cfgHardCeilWeeks[gameSize].Value);

            // ── Override and Dynamically Scale values ─────────────
            ScaleSubsidiaryTimeline(studio, game, weeksInt);

            // Neutralize vanilla quick addon scheduling
            try
            {
                var fieldAddon = typeof(publisherScript).GetField("nextGameAddon", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                var fieldMmoAddon = typeof(publisherScript).GetField("nextGameMMOAddon", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                if (fieldAddon != null) fieldAddon.SetValue(studio, false);
                if (fieldMmoAddon != null) fieldMmoAddon.SetValue(studio, false);
            }
            catch (Exception ex)
            {
                if (log != null) log.LogError("Failed to reset vanilla nextGameAddon fields: " + ex);
            }

            if (cfgLogCalc.Value && log != null)
            {
                string projectTypeLabel = GetProjectTypeLabel(game);
                log.LogInfo(string.Format(
                    "[Timeline CreateNewGame2 Postfix] Studio: '{0}' | Game: '{1}' (Size={2}, Type={3}) | " +
                    "Stars={4}/5 Speed={5} Plats={6} Trend={7} | " +
                    "Base={8:F1}w x*={9:F2} s*={10:F2} p*={11:F2} t*={12:F2} g*={13:F2} r*={14:F2} => Final={15}w | Remaining={16}w",
                    studio.GetName(), game.GetNameWithTag(), SizeLabels[gameSize], projectTypeLabel,
                    starsCount, speedLevel, platformCount, trendState,
                    baseWeeks, starMult, speedMult, platformMult, typeMult, trendMult, randomMult,
                    weeksInt, studio.newGameInWeeks));
            }
        }
    }

    // ───────────────────────────────────────────────────────
    //  PATCH: publisherScript.VerwaltungskostenBezahlen (Prefix)
    // ───────────────────────────────────────────────────────
    [HarmonyPatch(typeof(publisherScript), "VerwaltungskostenBezahlen")]
    private static class PublisherScript_VerwaltungskostenBezahlen_Patch
    {
        [HarmonyPrefix]
        static bool Prefix(publisherScript __instance)
        {
            try
            {
                if (__instance == null) return true;

                if (__instance.mS_ == null)
                {
                    __instance.mS_ = UnityEngine.Object.FindObjectOfType<mainScript>();
                }

                if (__instance.mS_ != null && __instance.IsMyTochterfirma() && !__instance.tf_geschlossen)
                {
                    long cost = CalculateDynamicVerwaltungskosten(__instance);
                    __instance.mS_.Pay(cost, 30);
                }
                return false; // Skip vanilla
            }
            catch (Exception ex)
            {
                if (log != null) log.LogError("Error in VerwaltungskostenBezahlen Prefix: " + ex);
                return true; // Fallback to vanilla
            }
        }
    }

    // ───────────────────────────────────────────────────────
    //  PATCH: publisherScript.GetTooltip (Postfix)
    // ───────────────────────────────────────────────────────
    [HarmonyPatch(typeof(publisherScript), "GetTooltip")]
    private static class PublisherScript_GetTooltip_Patch
    {
        [HarmonyPostfix]
        static void Postfix(publisherScript __instance, ref string __result)
        {
            try
            {
                if (__instance == null || string.IsNullOrEmpty(__result)) return;

                bool isOrganic = IsOrganicStudio(__instance);
                bool isAcquired = IsAcquiredSubsidiary(__instance);

                if (isOrganic || isAcquired)
                {
                    mainScript mS = __instance.mS_ ? __instance.mS_ : UnityEngine.Object.FindObjectOfType<mainScript>();
                    textScript tS = __instance.tS_ ? __instance.tS_ : UnityEngine.Object.FindObjectOfType<textScript>();
                    if (mS != null && tS != null)
                    {
                        string searchPrefix = tS.GetText(1934) + ": <color=red><b>";
                        int index = __result.IndexOf(searchPrefix);
                        if (index >= 0)
                        {
                            int startIdx = index + searchPrefix.Length;
                            int endIdx = __result.IndexOf("</b></color>", startIdx);
                            if (endIdx >= 0)
                            {
                                long cost = CalculateDynamicVerwaltungskosten(__instance);
                                string costStr = mS.GetMoney(cost, showDollar: true);
                                __result = __result.Substring(0, startIdx) + costStr + __result.Substring(endIdx);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (log != null) log.LogError("Error in GetTooltip Postfix: " + ex);
            }
        }
    }

    // ───────────────────────────────────────────────────────
    //  PATCH: publisherScript.GetVerwaltungskosten (Prefix)
    // ───────────────────────────────────────────────────────
    [HarmonyPatch(typeof(publisherScript), "GetVerwaltungskosten")]
    private static class PublisherScript_GetVerwaltungskosten_Patch
    {
        [HarmonyPrefix]
        static bool Prefix(publisherScript __instance, ref long __result)
        {
            try
            {
                if (__instance != null && __instance.IsTochterfirma() && __instance.IsMyTochterfirma() && !__instance.tf_geschlossen)
                {
                    __result = CalculateDynamicVerwaltungskosten(__instance);
                    return false; // Skip vanilla
                }
            }
            catch (Exception ex)
            {
                if (log != null) log.LogError("Error in PublisherScript.GetVerwaltungskosten Prefix: " + ex);
            }
            return true; // Fallback to vanilla
        }
    }

    // ───────────────────────────────────────────────────────
    //  PATCH: Item_Stats_Tochterfirma.SetData (Prefix)
    // ───────────────────────────────────────────────────────
    [HarmonyPatch(typeof(Item_Stats_Tochterfirma), "SetData")]
    private static class Item_Stats_Tochterfirma_SetData_PrefixPatch
    {
        [HarmonyPrefix]
        static bool Prefix(Item_Stats_Tochterfirma __instance)
        {
            try
            {
                SafeSetData(__instance);
                return false; // Skip vanilla buggy SetData
            }
            catch (Exception ex)
            {
                if (log != null) log.LogError("Error in Item_Stats_Tochterfirma.SetData Prefix: " + ex.ToString());
                return true; // Fallback to vanilla
            }
        }
    }

    private static gameScript FindSlotGameClosestToRelease(publisherScript studio, out float remainingWeeks, out float progressFraction)
    {
        remainingWeeks = 999999f;
        progressFraction = 0f;
        gameScript closestGame = null;

        if (studio == null) return null;

        if (IsTeamSlotsLoaded())
        {
            try
            {
                SubsidiaryTeamSlotsPlugin.StudioSlotData slotData = SubsidiaryTeamSlotsPlugin.GetStudioSlotData(studio.myID);
                if (slotData != null && slotData.slots != null)
                {
                    games gamesScript = studio.games_ != null ? studio.games_ : GetGamesScript();
                    for (int i = 0; i < slotData.slots.Length; i++)
                    {
                        SubsidiaryTeamSlotsPlugin.SlotData slot = slotData.slots[i];
                        if (slot != null && slot.gameID != -1)
                        {
                            float acceleration = GetAccelerationFactor(studio, i);
                            float calendarWeeks = slot.remainingWeeks / acceleration;
                            if (calendarWeeks < remainingWeeks)
                            {
                                gameScript game = FindGameByIDInGlobal(gamesScript, slot.gameID);
                                if (game != null)
                                {
                                    remainingWeeks = calendarWeeks;
                                    closestGame = game;
                                    progressFraction = (slot.totalWeeks > 0f) ? (1f - (slot.remainingWeeks / slot.totalWeeks)) : 0f;
                                    progressFraction = Mathf.Clamp01(progressFraction);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (log != null) log.LogWarning("[Timeline] FindSlotGameClosestToRelease failed: " + ex.Message);
            }
        }

        // Fallback to vanilla
        if (closestGame == null)
        {
            closestGame = SafeFindGameInDevelopment(studio);
            if (closestGame != null)
            {
                remainingWeeks = studio.newGameInWeeks;
                progressFraction = studio.GetEntwicklungsFortschritt();
            }
        }

        return closestGame;
    }

    // ───────────────────────────────────────────────────────
    //  Safe UI Card Renderer Helper
    // ───────────────────────────────────────────────────────
    private static void SafeSetData(Item_Stats_Tochterfirma item)
    {
        if (item == null || item.pS_ == null) return;

        publisherScript pS = item.pS_;

        // ── Force-Initialize all reference fields on the publisher script ──
        if (pS.mS_ == null) pS.mS_ = item.mS_;
        if (pS.tS_ == null) pS.tS_ = item.tS_;
        if (pS.games_ == null)
        {
            if (item.mS_ != null) pS.games_ = item.mS_.GetComponent<games>();
            if (pS.games_ == null && item.genres_ != null) pS.games_ = item.genres_.gameObject.GetComponent<games>();
            if (pS.games_ == null) pS.games_ = UnityEngine.Object.FindObjectOfType<games>();
        }
        if (pS.settings_ == null && item.mS_ != null) pS.settings_ = item.mS_.settings_;

        if (pS.tf_umsatz == null) pS.tf_umsatz = new long[24];
        if (pS.awards == null) pS.awards = new int[30];
        if (pS.tf_ipFocus == null)
        {
            pS.tf_ipFocus = new int[6];
            for (int i = 0; i < pS.tf_ipFocus.Length; i++) pS.tf_ipFocus[i] = -1;
        }
        if (pS.tf_platformFocus == null)
        {
            pS.tf_platformFocus = new int[4];
            for (int i = 0; i < pS.tf_platformFocus.Length; i++) pS.tf_platformFocus[i] = -1;
        }

        // 0. Name
        if (item.uiObjects != null && item.uiObjects.Length > 0 && item.uiObjects[0] != null)
        {
            Text textComp = item.uiObjects[0].GetComponent<Text>();
            if (textComp != null) textComp.text = pS.GetName();
        }

        // 1. Logo
        if (item.uiObjects != null && item.uiObjects.Length > 1 && item.uiObjects[1] != null)
        {
            Image imgComp = item.uiObjects[1].GetComponent<Image>();
            if (imgComp != null) imgComp.sprite = pS.GetLogo();
        }

        // 3. Stars
        if (item.uiObjects != null && item.uiObjects.Length > 3 && item.uiObjects[3] != null && item.guiMain_ != null)
        {
            item.guiMain_.DrawStars(item.uiObjects[3], Mathf.RoundToInt(pS.stars / 20f));
        }

        // 2. Games count
        if (item.uiObjects != null && item.uiObjects.Length > 2 && item.uiObjects[2] != null)
        {
            Text textComp = item.uiObjects[2].GetComponent<Text>();
            if (textComp != null) textComp.text = pS.GetAmountGames().ToString();
        }

        // 4. Goodwill (Firmenwert)
        if (item.uiObjects != null && item.uiObjects.Length > 4 && item.uiObjects[4] != null)
        {
            Text textComp = item.uiObjects[4].GetComponent<Text>();
            if (textComp != null)
            {
                long customValue = SafeGetFirmenwert(pS);
                textComp.text = item.mS_ != null ? item.mS_.GetMoney(customValue, showDollar: true) : $"${customValue:N0}";
            }
        }

        // 11. Monthly costs (Verwaltungskosten)
        if (item.uiObjects != null && item.uiObjects.Length > 11 && item.uiObjects[11] != null)
        {
            Text textComp = item.uiObjects[11].GetComponent<Text>();
            if (textComp != null)
            {
                long adminCost = CalculateDynamicVerwaltungskosten(pS);
                textComp.text = item.mS_ != null ? item.mS_.GetMoney(adminCost, showDollar: true) : $"${adminCost:N0}";
            }
        }

        // 12. Fan genre
        if (item.uiObjects != null && item.uiObjects.Length > 12 && item.uiObjects[12] != null && item.genres_ != null)
        {
            Image imgComp = item.uiObjects[12].GetComponent<Image>();
            if (imgComp != null) imgComp.sprite = item.genres_.GetPic(pS.fanGenre);
        }

        // 13. Total revenue
        if (item.uiObjects != null && item.uiObjects.Length > 13 && item.uiObjects[13] != null)
        {
            Text textComp = item.uiObjects[13].GetComponent<Text>();
            if (textComp != null)
            {
                textComp.text = item.mS_ != null ? item.mS_.GetMoney(pS.tf_umsatz_allTime, showDollar: true) : $"${pS.tf_umsatz_allTime:N0}";
            }
        }

        // 14. Last 24 months
        if (item.uiObjects != null && item.uiObjects.Length > 14 && item.uiObjects[14] != null)
        {
            Text textComp = item.uiObjects[14].GetComponent<Text>();
            if (textComp != null)
            {
                long rev24 = pS.GetTochterfirmaUmsatz24Monate();
                textComp.text = item.mS_ != null ? item.mS_.GetMoney(rev24, showDollar: true) : $"${rev24:N0}";
            }
        }

        // 7. Developer/Publisher string
        if (item.uiObjects != null && item.uiObjects.Length > 7 && item.uiObjects[7] != null)
        {
            Text textComp = item.uiObjects[7].GetComponent<Text>();
            if (textComp != null) textComp.text = pS.GetDeveloperPublisherString();
        }

        // Tooltip
        if (item.tooltip_ != null)
        {
            try
            {
                item.tooltip_.c = pS.GetTooltip();
            }
            catch
            {
                item.tooltip_.c = "<b><size=18>" + pS.GetName() + "</size></b>";
            }
        }

        // Closed status coloring
        if (pS.Geschlossen())
        {
            Image imgColor = item.GetComponent<Image>();
            if (imgColor != null && item.guiMain_ != null && item.guiMain_.colors != null && item.guiMain_.colors.Length > 25)
            {
                imgColor.color = item.guiMain_.colors[25];
            }
        }

        // Closed active status
        if (item.uiObjects != null && item.uiObjects.Length > 5 && item.uiObjects[5] != null)
        {
            if (pS.Geschlossen())
            {
                if (!item.uiObjects[5].activeSelf) item.uiObjects[5].SetActive(true);
            }
            else
            {
                if (item.uiObjects[5].activeSelf) item.uiObjects[5].SetActive(false);
            }
        }

        // Development stats for developer studios
        if (pS.developer)
        {
            float remainingWeeks = 999999f;
            float progressFraction = 0f;
            gameScript game = FindSlotGameClosestToRelease(pS, out remainingWeeks, out progressFraction);

            if (item.uiObjects != null && item.uiObjects.Length > 8 && item.uiObjects[8] != null)
            {
                Image imgComp = item.uiObjects[8].GetComponent<Image>();
                if (imgComp != null) imgComp.fillAmount = progressFraction;
            }

            if (game != null)
            {
                if (item.uiObjects != null && item.uiObjects.Length > 10 && item.uiObjects[10] != null)
                {
                    Text textComp = item.uiObjects[10].GetComponent<Text>();
                    if (textComp != null) textComp.text = game.GetNameWithTag();
                }

                if (item.uiObjects != null && item.uiObjects.Length > 9 && item.uiObjects[9] != null)
                {
                    Text textComp = item.uiObjects[9].GetComponent<Text>();
                    if (textComp != null)
                    {
                        int weeksInt = Mathf.RoundToInt(remainingWeeks);
                        if (weeksInt > 2)
                        {
                            textComp.text = (item.tS_ != null ? item.tS_.GetText(1944) : "Progress") + ": " + Mathf.RoundToInt(progressFraction * 100f) + "%";
                        }
                        else
                        {
                            textComp.text = item.tS_ != null ? item.tS_.GetText(1947) : "Finished";
                            if (game.HasUnreleasedPlattform())
                            {
                                textComp.text = item.tS_ != null ? item.tS_.GetText(2316) : "Wait for Release";
                            }
                        }
                    }
                }
            }
            else
            {
                if (item.uiObjects != null && item.uiObjects.Length > 10 && item.uiObjects[10] != null)
                {
                    Text textComp = item.uiObjects[10].GetComponent<Text>();
                    if (textComp != null) textComp.text = item.tS_ != null ? item.tS_.GetText(1949) : "No game in development";
                }

                if (item.uiObjects != null && item.uiObjects.Length > 9 && item.uiObjects[9] != null)
                {
                    Text textComp = item.uiObjects[9].GetComponent<Text>();
                    if (textComp != null) textComp.text = "0%";
                }

                if (item.uiObjects != null && item.uiObjects.Length > 8 && item.uiObjects[8] != null)
                {
                    Image imgComp = item.uiObjects[8].GetComponent<Image>();
                    if (imgComp != null) imgComp.fillAmount = 0f;
                }
            }
        }
        else
        {
            if (item.uiObjects != null && item.uiObjects.Length > 9 && item.uiObjects[9] != null)
            {
                Text textComp = item.uiObjects[9].GetComponent<Text>();
                if (textComp != null) textComp.text = item.tS_ != null ? item.tS_.GetText(1949) : "No game in development";
            }

            if (item.uiObjects != null && item.uiObjects.Length > 8 && item.uiObjects[8] != null)
            {
                Image imgComp = item.uiObjects[8].GetComponent<Image>();
                if (imgComp != null) imgComp.fillAmount = 0f;
            }
        }
    }

    // ───────────────────────────────────────────────────────
    //  Star Multiplier: Size-weighted linear penalty
    //  Formula: 1.0 + (5 - StarsCount) * starWeights[GameSize]
    //  Small games (B) get a mild penalty, large games (AAAA) get a harsh penalty.
    //  Each star upgrade reduces the multiplier by weight.
    // ───────────────────────────────────────────────────────
    private static float CalcStarMultiplier(int starsCount, int gameSize)
    {
        int starsClamped = Mathf.Clamp(starsCount, 0, 5);
        int gap = 5 - starsClamped;
        float[] starWeights = { 0.20f, 0.30f, 0.40f, 0.50f, 0.60f, 0.70f };
        float weight = starWeights[Mathf.Clamp(gameSize, 0, 5)];
        return 1.0f + gap * weight;
    }

    // ───────────────────────────────────────────────────────
    //  Speed Multiplier: speed 1 = ×1.0 baseline, speed 10 = ×0.50 max boost
    //  Formula (organic): 1.0556 - 0.0556 * level
    //  Formula (acquired): 1.1667 - 0.1667 * level
    // ───────────────────────────────────────────────────────
    private static float CalcSpeedMultiplier(int speedLevel, bool isOrganic)
    {
        if (isOrganic)
        {
            int level = Mathf.Clamp(speedLevel, 1, 10);
            return 1.0556f - 0.0556f * level;
        }
        else
        {
            int level = Mathf.Clamp(speedLevel, 1, 4);
            return 1.1667f - 0.1667f * level;
        }
    }

    // ───────────────────────────────────────────────────────
    //  Platform Multiplier: +10% per platform beyond the first
    //  Formula: 1.0 + 0.10 * (PlatformCount - 1)
    // ───────────────────────────────────────────────────────
    private static float CalcPlatformMultiplier(int platformCount)
    {
        int extra = Mathf.Max(0, platformCount - 1);
        return 1.0f + extra * 0.10f;
    }

    // ───────────────────────────────────────────────────────
    //  Project Type Multiplier
    // ───────────────────────────────────────────────────────
    private static float CalcProjectTypeMultiplier(gameScript game)
    {
        if (game == null) return 1.15f;

        float baseMult = 1.15f;

        // Compilation packaging
        if (game.typ_bundle || game.typ_budget || game.typ_bundleAddon || game.typ_goty)
            baseMult = 0.10f;
        // Porting code to target platforms
        else if (game.portID != -1)
            baseMult = 0.25f;
        // Add-ons
        else if (game.typ_addon)
            baseMult = 0.30f;
        else if (game.typ_mmoaddon)
            baseMult = 0.35f;
        else if (game.typ_addonStandalone)
            baseMult = 0.40f;
        // Remasters (graphics/engine tweaks only)
        else if (game.typ_remaster)
            baseMult = 0.45f;
        // Contract games (predefined specification, fast execution)
        else if (game.typ_contractGame)
            baseMult = 0.70f;
        // Sequel (Nachfolger) - established workflow and lore
        else if (game.typ_nachfolger)
            baseMult = 0.85f;
        // Spin-off - lore/graphics templates
        else if (game.typ_spinoff)
            baseMult = 0.90f;
        // New IP (Standard game) - requires full design, pre-production, artwork, and code frameworks
        else if (game.typ_standard)
            baseMult = 1.15f;

        // Apply MMO and F2P modifiers on top of base games (New IP, Sequel, Spin-off, Remasters)
        if (!game.typ_bundle && !game.typ_budget && !game.typ_bundleAddon && !game.typ_goty && game.portID == -1 && !game.typ_contractGame)
        {
            if (game.gameTyp == 1) // MMO
                return baseMult * 1.35f;
            if (game.gameTyp == 2) // Free-to-play
                return baseMult * 1.25f;
        }

        return baseMult;
    }

    private static string GetProjectTypeLabel(gameScript game)
    {
        if (game == null) return "Standard";
        if (game.typ_bundle) return "Bundle";
        if (game.typ_budget) return "Budget";
        if (game.typ_bundleAddon) return "BundleAddon";
        if (game.typ_goty) return "GOTY";
        if (game.portID != -1) return "Port";
        if (game.typ_addon) return "Addon";
        if (game.typ_mmoaddon) return "MMOAddon";
        if (game.typ_addonStandalone) return "StandaloneAddon";
        if (game.typ_remaster) return "Remaster";
        if (game.typ_contractGame) return "ContractGame";
        if (game.typ_nachfolger) return "Sequel";
        if (game.typ_spinoff) return "Spinoff";
        if (game.typ_standard) return "NewIP";
        return "Standard";
    }

    // ───────────────────────────────────────────────────────
    //  Trend Multiplier
    // ───────────────────────────────────────────────────────
    private static float CalcTrendMultiplier(string trendState)
    {
        if (!cfgTrendIntegration.Value) return 1.0f;

        switch (trendState)
        {
            case "In Crisis":             return 1.35f;
            case "Declining":             return 1.15f;
            case "Stable":                return 1.00f;
            case "Rising":                return 0.92f;
            case "Commercial Powerhouse": return 0.85f;
            default:                      return 1.00f;
        }
    }

    // ───────────────────────────────────────────────────────
    //  Late-bound trend getter from Dynamic Studio Goodwill Mod
    // ───────────────────────────────────────────────────────
    private static string GetTrendState(publisherScript studio)
    {
        if (!cfgTrendIntegration.Value) return "Stable";
        try
        {
            if (!goodwillModChecked)
            {
                goodwillModChecked = true;
                foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    goodwillPluginType = asm.GetType("DynamicStudioGoodwillPlugin");
                    if (goodwillPluginType != null)
                    {
                        goodwillTrendDictField = goodwillPluginType.GetField("studioTrends",
                            BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                        if (goodwillTrendDictField == null)
                            goodwillTrendDictField = goodwillPluginType.GetField("trendLabels",
                                BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                        break;
                    }
                }
            }

            if (goodwillTrendDictField != null)
            {
                object rawDict = goodwillTrendDictField.GetValue(null);
                Dictionary<int, string> trends = rawDict as Dictionary<int, string>;
                if (trends != null)
                {
                    string label;
                    if (trends.TryGetValue(studio.myID, out label))
                        return label;
                }
            }
        }
        catch { /* Fallback on reflection errors */ }

        return "Stable";
    }

    // ───────────────────────────────────────────────────────
    //  Helper Methods
    // ───────────────────────────────────────────────────────

    private static int CountPlatforms(gameScript game)
    {
        if (game.gamePlatform == null || game.gamePlatform.Length == 0) return 1;
        int count = 0;
        foreach (int p in game.gamePlatform)
            if (p >= 0) count++;
        return Mathf.Max(1, count);
    }

    private static mainScript cachedMainScript;
    private static games cachedGames;

    public static mainScript GetMainScript()
    {
        if (cachedMainScript == null)
        {
            cachedMainScript = UnityEngine.Object.FindObjectOfType<mainScript>();
        }
        return cachedMainScript;
    }

    public static games GetGamesScript()
    {
        if (cachedGames == null)
        {
            cachedGames = UnityEngine.Object.FindObjectOfType<games>();
        }
        return cachedGames;
    }

    private static publisherScript FindStudioByID(int studioID)
    {
        mainScript main = GetMainScript();
        if (main != null && main.arrayPublisherScripts != null)
        {
            foreach (publisherScript p in main.arrayPublisherScripts)
            {
                if (p != null && p.myID == studioID)
                    return p;
            }
        }

        publisherScript[] allStudios = UnityEngine.Object.FindObjectsOfType<publisherScript>();
        if (allStudios != null)
        {
            foreach (publisherScript p in allStudios)
            {
                if (p != null && p.myID == studioID)
                    return p;
            }
        }
        return null;
    }

    private static bool IsOrganicStudio(publisherScript studio)
    {
        if (studio == null || !studio) return false;
        try
        {
            return (studio.myID >= 9000 && studio.myID < 10000)
                || (studio.myID >= 90000 && studio.myID < 100000);
        }
        catch
        {
            return false;
        }
    }

    private static bool IsAcquiredSubsidiary(publisherScript studio)
    {
        if (studio == null || !studio) return false;
        try
        {
            if (IsOrganicStudio(studio)) return false;
            return studio.IsMyTochterfirma();
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Counts how many team-slots in this studio currently have an ACTIVE in-development project
    /// (not helping another slot, and not idle).
    /// Returns 0 if Team Slots is not loaded, or if the studio has no slot data.
    /// </summary>
    private static int CountActiveProjects(publisherScript studio)
    {
        if (studio == null || !IsTeamSlotsLoaded()) return 0;
        try
        {
            SubsidiaryTeamSlotsPlugin.StudioSlotData slotData = SubsidiaryTeamSlotsPlugin.GetStudioSlotData(studio.myID);
            if (slotData == null || slotData.slots == null) return 0;
            int count = 0;
            for (int i = 0; i < slotData.slots.Length; i++)
            {
                var slot = slotData.slots[i];
                if (slot != null && slot.gameID != -1 && slot.isUnlocked)
                {
                    count++;
                }
            }
            return count;
        }
        catch (Exception ex)
        {
            if (log != null) log.LogWarning("CountActiveProjects failed: " + ex.Message);
            return 0;
        }
    }

    /// <summary>
    /// Returns the multi-team penalty multiplier for a given studio.
    /// Multiplier = 1.0 + (activeCount - 1) * (cfgMultiTeamPenaltyPercent / 100).
    /// Examples (default 18%):
    ///   1 active project  → 1.00 (no penalty)
    ///   2 active projects → 1.18 (+18% weeks)
    ///   3 active projects → 1.36 (+36% weeks)
    /// Returns 1.0 if penalty disabled, Team Slots not loaded, or 0-1 active projects.
    /// </summary>
    private static float CalcMultiTeamPenaltyMultiplier(publisherScript studio)
    {
        if (studio == null) return 1f;
        if (cfgMultiTeamPenaltyEnabled == null || !cfgMultiTeamPenaltyEnabled.Value) return 1f;
        if (cfgMultiTeamPenaltyPercent == null) return 1f;

        int activeCount = CountActiveProjects(studio);
        if (activeCount <= 1) return 1f;

        float pct = cfgMultiTeamPenaltyPercent.Value;
        if (pct < 0f) pct = 0f;
        if (pct > 200f) pct = 200f; // hard cap to avoid runaway

        float mult = 1.0f + (activeCount - 1) * (pct / 100f);
        return mult;
    }

    private static long GetOrganicSaleValue(publisherScript studio)
    {
        try
        {
            if (!organicSaleValueChecked)
            {
                organicSaleValueChecked = true;
                foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    Type t = asm.GetType("Subsidiary2Plugin");
                    if (t != null)
                    {
                        organicSaleValueMethod = t.GetMethod("GetOrganicSaleValue",
                            BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                        break;
                    }
                }
            }
            if (organicSaleValueMethod != null)
            {
                return (long)organicSaleValueMethod.Invoke(null, new object[] { studio });
            }
        }
        catch { }
        return studio != null ? studio.firmenwert : 2000000L;
    }

    private static object InvokeGetStudioData(int studioID)
    {
        try
        {
            Type pluginType = null;
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                pluginType = asm.GetType("Subsidiary2Plugin");
                if (pluginType != null) break;
            }
            if (pluginType != null)
            {
                MethodInfo getStudioDataMethod = pluginType.GetMethod("GetStudioData",
                    BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                if (getStudioDataMethod != null)
                {
                    return getStudioDataMethod.Invoke(null, new object[] { studioID });
                }
            }
        }
        catch { }
        return null;
    }

    private static long GetOrganicUpkeepBaseValue(publisherScript studio)
    {
        if (studio == null) return 2000000L;
        object data = InvokeGetStudioData(studio.myID);
        if (data != null)
        {
            try
            {
                FieldInfo creationCostField = data.GetType().GetField("creationCost");
                FieldInfo upgradeInvestmentsField = data.GetType().GetField("upgradeInvestments");
                long creationCost = (long)creationCostField.GetValue(data);
                long upgradeInvestments = (long)upgradeInvestmentsField.GetValue(data);
                return creationCost + upgradeInvestments;
            }
            catch {}
        }
        return studio.firmenwert > 0 ? studio.firmenwert : 2000000L;
    }

    public static float GetAccelerationFactor(publisherScript studio, int slotIdx)
    {
        if (studio == null || !IsTeamSlotsLoaded()) return 1f;
        try
        {
            var slotData = SubsidiaryTeamSlotsPlugin.GetStudioSlotData(studio.myID);
            if (slotData == null || slotData.slots == null || slotIdx < 0 || slotIdx >= slotData.slots.Length) return 1f;
            
            var primarySlot = slotData.slots[slotIdx];
            if (primarySlot == null || primarySlot.gameID == -1) return 1f;
            
            // Determine game size for size-weighted star multiplier
            int gameSize = 2; // default to A if lookup fails
            try
            {
                games gs = studio.games_ ?? UnityEngine.Object.FindObjectOfType<games>();
                if (gs != null)
                {
                    gameScript g = FindGameByIDInGlobal(gs, primarySlot.gameID);
                    if (g != null) gameSize = Mathf.Clamp(g.gameSize, 0, 5);
                }
            }
            catch { }
            
            bool isOrganic = IsOrganicStudio(studio);
            int primaryStars = (slotIdx == 0) ? studio.GetStarsAmount() : primarySlot.stars;
            int primarySpeed = (slotIdx == 0) ? studio.developmentSpeed : primarySlot.speed;
            
            float pStarMult = CalcStarMultiplier(primaryStars, gameSize);
            float pSpeedMult = CalcSpeedMultiplier(primarySpeed, isOrganic);
            float wPrimary = 1.0f / (pStarMult * pSpeedMult);
            if (wPrimary <= 0.001f) wPrimary = 0.001f;
            
            float wHelpersSum = 0f;
            for (int i = 0; i < slotData.slots.Length; i++)
            {
                if (i == slotIdx) continue;
                var helperSlot = slotData.slots[i];
                if (helperSlot != null && helperSlot.isUnlocked && helperSlot.helpingSlotIndex == slotIdx)
                {
                    int hStars = (i == 0) ? studio.GetStarsAmount() : helperSlot.stars;
                    int hSpeed = (i == 0) ? studio.developmentSpeed : helperSlot.speed;
                    float hStarMult = CalcStarMultiplier(hStars, gameSize);
                    float hSpeedMult = CalcSpeedMultiplier(hSpeed, isOrganic);
                    float wHelper = 1.0f / (hStarMult * hSpeedMult);
                    wHelpersSum += wHelper;
                }
            }
            
            return 1.0f + 0.75f * (wHelpersSum / wPrimary);
        }
        catch (Exception ex)
        {
            if (log != null) log.LogWarning("GetAccelerationFactor failed: " + ex);
            return 1f;
        }
    }

    public static long CalculateSingleSlotUpkeep(publisherScript studio, int slotIdx)
    {
        if (studio == null) return 0L;
        bool isOrganic = IsOrganicStudio(studio);
        
        long studioValue = isOrganic ? GetOrganicUpkeepBaseValue(studio) : SafeGetFirmenwert(studio);
        if (studioValue < 0) studioValue = 0;
        if (studioValue > 100000000L) studioValue = 100000000L;
        
        mainScript main = GetMainScript();
        int year = main != null ? main.year : 2020;
        double baseInflation = cfgBaseInflationRate != null ? cfgBaseInflationRate.Value : 2.0f;
        double inflationRate = (baseInflation / 100.0) * (1.0 + 1.5 * (studioValue / 100000000.0));
        double yearMult = 1.0 + inflationRate * (year - 1976);
        if (yearMult < 1.0) yearMult = 1.0;
        double vBase = 5000.0 * yearMult;
        double baseUpkeep = (studioValue * 0.001) + vBase;
        
        double diffMult = 1.0;
        if (main != null)
        {
            diffMult = 0.6 + (main.difficulty * 0.20);
        }
        double adjustedBase = baseUpkeep * diffMult;
        
        try
        {
            if (IsTeamSlotsLoaded())
            {
                var slotData = SubsidiaryTeamSlotsPlugin.GetStudioSlotData(studio.myID);
                if (slotData != null && slotData.slots != null && slotIdx >= 0 && slotIdx < slotData.slots.Length)
                {
                    var slot = slotData.slots[slotIdx];
                    if (slot != null && slot.isUnlocked)
                    {
                        games gamesScript = studio.games_ != null ? studio.games_ : GetGamesScript();
                        int starsCount = (slotIdx == 0) ? studio.GetStarsAmount() : slot.stars;
                        int speedLevel = (slotIdx == 0) ? studio.developmentSpeed : slot.speed;
                        
                        double ratingMult = 1.0 + 0.2 * starsCount + 0.1 * (starsCount * starsCount);
                        double speedMultVal = isOrganic ? (1.0 + speedLevel / 10.0) : (1.0 + speedLevel / 4.0);
                        
                        double sizeMult = 1.0;
                        double helperDiscount = 1.0;
                        
                        if (slot.helpingSlotIndex >= 0 && slot.helpingSlotIndex < slotData.slots.Length)
                        {
                            int targetSlotIdx = slot.helpingSlotIndex;
                            var targetSlot = slotData.slots[targetSlotIdx];
                            if (targetSlot != null && targetSlot.gameID != -1)
                            {
                                gameScript targetGame = FindGameByIDInGlobal(gamesScript, targetSlot.gameID);
                                if (targetGame != null)
                                {
                                    int gameSize = Mathf.Clamp(targetGame.gameSize, 0, 5);
                                    sizeMult = 1.0 + 0.5 * gameSize + 0.2 * (gameSize * gameSize);
                                }
                            }
                            helperDiscount = 0.85; // 15% discount
                        }
                        else if (slot.gameID != -1)
                        {
                            gameScript game = FindGameByIDInGlobal(gamesScript, slot.gameID);
                            if (game != null)
                            {
                                int gameSize = Mathf.Clamp(game.gameSize, 0, 5);
                                sizeMult = 1.0 + 0.5 * gameSize + 0.2 * (gameSize * gameSize);
                            }
                        }
                        else
                        {
                            sizeMult = 0.4;
                        }
                        
                        int unlockedCount = 0;
                        for (int i = 0; i < slotData.slots.Length; i++)
                        {
                            if (slotData.slots[i].isUnlocked) unlockedCount++;
                        }
                        double complexityFactor = 1.00;
                        if (unlockedCount == 2) complexityFactor = 1.15;
                        else if (unlockedCount == 3) complexityFactor = 1.30;
                        
                        return (long)Math.Round(adjustedBase * sizeMult * ratingMult * speedMultVal * helperDiscount * complexityFactor);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            if (log != null) log.LogWarning("CalculateSingleSlotUpkeep failed: " + ex);
        }
        
        return 0L;
    }

    public static long CalculateDynamicVerwaltungskosten(publisherScript studio)
    {
        if (studio == null) return 0L;
        if (studio.tf_geschlossen) return 0L;

        bool isOrganic  = IsOrganicStudio(studio);
        mainScript main = GetMainScript();
        int year = main != null ? main.year : 2020;
        double baseInflation = cfgBaseInflationRate != null ? cfgBaseInflationRate.Value : 2.0f;
        long upkeepCap = cfgMaxUpkeepCap != null ? cfgMaxUpkeepCap.Value : 17500000L;

        // Base Goodwill value
        long studioValue = isOrganic ? GetOrganicUpkeepBaseValue(studio) : SafeGetFirmenwert(studio);
        if (studioValue < 0) studioValue = 0;
        if (studioValue > 100000000L) studioValue = 100000000L;

        double inflationRate = (baseInflation / 100.0) * (1.0 + 1.5 * (studioValue / 100000000.0));
        double yearMult = 1.0 + inflationRate * (year - 1976);
        if (yearMult < 1.0) yearMult = 1.0;
        double vBase = 5000.0 * yearMult;
        double baseUpkeep = (studioValue * 0.001) + vBase;

        double diffMult = 1.0;
        if (main != null)
        {
            diffMult = 0.6 + (main.difficulty * 0.20);
        }
        double adjustedBase = baseUpkeep * diffMult;

        long totalCost = 0L;

        if (IsTeamSlotsLoaded())
        {
            try
            {
                var slotData = SubsidiaryTeamSlotsPlugin.GetStudioSlotData(studio.myID);
                if (slotData != null && slotData.slots != null)
                {
                    games gamesScript = studio.games_ != null ? studio.games_ : GetGamesScript();
                    int unlockedCount = 0;
                    for (int i = 0; i < slotData.slots.Length; i++)
                    {
                        if (slotData.slots[i].isUnlocked) unlockedCount++;
                    }

                    // Complexity factor based on unlocked slots count
                    double complexityFactor = 1.00;
                    if (unlockedCount == 2) complexityFactor = 1.15;
                    else if (unlockedCount == 3) complexityFactor = 1.30;

                    for (int i = 0; i < slotData.slots.Length; i++)
                    {
                        var slot = slotData.slots[i];
                        if (slot == null || !slot.isUnlocked) continue;

                        int starsCount = (i == 0) ? studio.GetStarsAmount() : slot.stars;
                        int speedLevel = (i == 0) ? studio.developmentSpeed : slot.speed;

                        double ratingMult = 1.0 + 0.2 * starsCount + 0.1 * (starsCount * starsCount);
                        double speedMultVal = 1.0;
                        if (isOrganic)
                        {
                            speedMultVal = 1.0 + (speedLevel / 10.0);
                        }
                        else
                        {
                            speedMultVal = 1.0 + (speedLevel / 4.0);
                        }

                        double sizeMult = 1.0;
                        double helperDiscount = 1.0;

                        if (slot.helpingSlotIndex >= 0 && slot.helpingSlotIndex < slotData.slots.Length)
                        {
                            // Helper slot: upkeep is based on the target game's size with 15% discount
                            int targetSlotIdx = slot.helpingSlotIndex;
                            var targetSlot = slotData.slots[targetSlotIdx];
                            if (targetSlot != null && targetSlot.gameID != -1)
                            {
                                gameScript targetGame = FindGameByIDInGlobal(gamesScript, targetSlot.gameID);
                                if (targetGame != null)
                                {
                                    int gameSize = Mathf.Clamp(targetGame.gameSize, 0, 5);
                                    sizeMult = 1.0 + 0.5 * gameSize + 0.2 * (gameSize * gameSize);
                                }
                            }
                            helperDiscount = 0.85; // 15% discount
                        }
                        else if (slot.gameID != -1)
                        {
                            // Active slot
                            gameScript game = FindGameByIDInGlobal(gamesScript, slot.gameID);
                            if (game != null)
                            {
                                int gameSize = Mathf.Clamp(game.gameSize, 0, 5);
                                sizeMult = 1.0 + 0.5 * gameSize + 0.2 * (gameSize * gameSize);
                            }
                        }
                        else
                        {
                            // Idle slot
                            sizeMult = 0.4;
                        }

                        double teamUpkeep = adjustedBase * sizeMult * ratingMult * speedMultVal * helperDiscount;
                        long teamUpkeepCost = (long)Math.Round(teamUpkeep * complexityFactor);
                        
                        // Cap INDIVIDUAL team upkeep at the configured maximum
                        if (teamUpkeepCost > upkeepCap) teamUpkeepCost = upkeepCap;
                        
                        totalCost += teamUpkeepCost;
                    }
                }
            }
            catch (Exception ex)
            {
                if (log != null) log.LogWarning("CalculateDynamicVerwaltungskosten slots logic failed: " + ex);
            }
        }

        if (totalCost == 0L)
        {
            // Fallback for single team (non-slots or error)
            gameScript game = SafeFindGameInDevelopment(studio);
            bool isIdle = (game == null);
            double sizeMult = isIdle ? 0.4 : (1.0 + 0.5 * game.gameSize + 0.2 * (game.gameSize * game.gameSize));
            int starsCount = studio.GetStarsAmount();
            double ratingMult = 1.0 + 0.2 * starsCount + 0.1 * (starsCount * starsCount);
            int speedLevel = studio.developmentSpeed;
            double speedMultVal = isOrganic ? (1.0 + speedLevel / 10.0) : (1.0 + speedLevel / 4.0);

            totalCost = (long)Math.Round(adjustedBase * sizeMult * ratingMult * speedMultVal);
            
            // Cap single team upkeep at the configured maximum
            if (totalCost > upkeepCap) totalCost = upkeepCap;
        }

        // Note: When using multiple team slots, totalCost is the sum of individually-capped team upkeeps.
        // There is NO overall studio cap - only individual team caps. This allows studios with 3 teams
        // to potentially have upkeep exceeding the individual cap (e.g., 3 teams @ 17.5M each = 52.5M total).
        
        return totalCost;
    }

    /// <summary>
    /// Recalculates development weeks for all active projects when a studio is upgraded.
    /// This ensures that games in progress benefit immediately from the star/speed upgrade.
    /// </summary>
    public static void RecalculateActiveProjectsAfterUpgrade(publisherScript studio, string upgradeType)
    {
        if (studio == null) return;

        try
        {
            bool recalculatedAny = false;

            // Handle Team Slots projects if the mod is loaded
            if (IsTeamSlotsLoaded())
            {
                var teamSlotsType = GetTeamSlotsType();
                if (teamSlotsType != null)
                {
                    // Get studio slot data
                    var getStudioSlotDataMethod = AccessTools.Method(teamSlotsType, "GetStudioSlotData");
                    if (getStudioSlotDataMethod != null)
                    {
                        object slotData = getStudioSlotDataMethod.Invoke(null, new object[] { studio.myID });
                        if (slotData != null)
                        {
                            var slotsField = AccessTools.Field(slotData.GetType(), "slots");
                            if (slotsField != null)
                            {
                                System.Array slots = slotsField.GetValue(slotData) as System.Array;
                                if (slots != null)
                                {
                                    games gamesScript = studio.games_ != null ? studio.games_ : GetGamesScript();
                                    if (gamesScript != null)
                                    {
                                        // Recalculate each active slot
                                        for (int i = 0; i < slots.Length; i++)
                                        {
                                            object slot = slots.GetValue(i);
                                            if (slot != null)
                                            {
                                                var gameIDField = AccessTools.Field(slot.GetType(), "gameID");
                                                var remainingWeeksField = AccessTools.Field(slot.GetType(), "remainingWeeks");
                                                var totalWeeksField = AccessTools.Field(slot.GetType(), "totalWeeks");
                                                var isUnlockedField = AccessTools.Field(slot.GetType(), "isUnlocked");

                                                if (gameIDField != null && remainingWeeksField != null && totalWeeksField != null && isUnlockedField != null)
                                                {
                                                    int gameID = (int)gameIDField.GetValue(slot);
                                                    bool isUnlocked = (bool)isUnlockedField.GetValue(slot);

                                                    if (gameID != -1 && isUnlocked)
                                                    {
                                                        // Find the game
                                                        gameScript game = FindGameByIDInGlobal(gamesScript, gameID);
                                                        if (game != null && game.inDevelopment)
                                                        {
                                                            float currentRemaining = (float)remainingWeeksField.GetValue(slot);
                                                            float currentTotal = (float)totalWeeksField.GetValue(slot);
                                                            
                                                            // Calculate what percentage of development is complete
                                                            float progress = (currentTotal > 0f) ? (1f - (currentRemaining / currentTotal)) : 0f;
                                                            progress = Mathf.Clamp01(progress);

                                                            // Recalculate the total duration with new star/speed levels
                                                            int newTotalWeeks = RecalculateGameDuration(studio, game);
                                                            
                                                            // Apply the same progress to the new duration
                                                            float newRemainingWeeks = newTotalWeeks * (1f - progress);
                                                            
                                                            // Ensure at least 1 week remaining
                                                            if (newRemainingWeeks < 1f) newRemainingWeeks = 1f;

                                                            // Update the slot data
                                                            remainingWeeksField.SetValue(slot, newRemainingWeeks);
                                                            totalWeeksField.SetValue(slot, (float)newTotalWeeks);

                                                            recalculatedAny = true;
                                                            
                                                            if (log != null && cfgLogCalc.Value)
                                                            {
                                                                log.LogInfo($"[Upgrade Recalc] Slot {i}: '{game.myName}' after {upgradeType} | Progress: {progress:F1}% | {currentRemaining:F1}w -> {newRemainingWeeks:F1}w (total: {currentTotal:F1}w -> {newTotalWeeks}w)");
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
            else
            {
                // Handle vanilla single project
                gameScript game = SafeFindGameInDevelopment(studio);
                if (game != null && game.inDevelopment)
                {
                    // Get current weeks and calculate progress
                    float currentRemaining = studio.newGameInWeeks;
                    float currentTotal = studio.newGameInWeeksORG;
                    float progress = (currentTotal > 0f) ? (1f - (currentRemaining / currentTotal)) : 0f;
                    progress = Mathf.Clamp01(progress);

                    // Recalculate with new star/speed levels
                    int newTotalWeeks = RecalculateGameDuration(studio, game);
                    float newRemainingWeeks = newTotalWeeks * (1f - progress);
                    
                    // Ensure at least 1 week remaining
                    if (newRemainingWeeks < 1f) newRemainingWeeks = 1f;

                    // Update vanilla fields
                    studio.newGameInWeeks = Mathf.RoundToInt(newRemainingWeeks);
                    studio.newGameInWeeksORG = newTotalWeeks;

                    recalculatedAny = true;

                    if (log != null && cfgLogCalc.Value)
                    {
                        log.LogInfo($"[Upgrade Recalc] Vanilla: '{game.myName}' after {upgradeType} | Progress: {progress:F1}% | {currentRemaining}w -> {newRemainingWeeks:F1}w (total: {currentTotal}w -> {newTotalWeeks}w)");
                    }
                }
            }

            if (recalculatedAny && log != null)
            {
                log.LogInfo($"[Upgrade Recalc] Recalculated active projects for studio '{studio.GetName()}' after {upgradeType}");
            }
        }
        catch (Exception ex)
        {
            if (log != null) log.LogError($"Error in RecalculateActiveProjectsAfterUpgrade: {ex}");
        }
    }

    /// <summary>
    /// Recalculates the total duration for a game with current studio stats.
    /// </summary>
    private static int RecalculateGameDuration(publisherScript studio, gameScript game)
    {
        if (studio == null || game == null) return 1;

        bool isOrganic = IsOrganicStudio(studio);
        int gameSize = Mathf.Clamp(game.gameSize, 0, 5);
        int platformCount = CountPlatforms(game);
        int starsCount = studio.GetStarsAmount();
        int speedLevel = studio.developmentSpeed;
        
        string trendState = GetTrendState(studio);

        // Calculate multipliers
        float starMult = CalcStarMultiplier(starsCount, gameSize);
        float speedMult = CalcSpeedMultiplier(speedLevel, isOrganic);
        float platformMult = CalcPlatformMultiplier(platformCount);
        float typeMult = CalcProjectTypeMultiplier(game);
        float trendMult = CalcTrendMultiplier(trendState);

        // Calculate base duration (without random variance for recalculation)
        float baseWeeks = cfgBaseMidpointWeeks[gameSize].Value;
        float finalWeeks = baseWeeks * starMult * speedMult * platformMult * typeMult * trendMult;

        // Apply floor/ceiling clamps
        int weeksInt = Mathf.Clamp(
            Mathf.RoundToInt(finalWeeks),
            cfgHardFloorWeeks[gameSize].Value,
            cfgHardCeilWeeks[gameSize].Value);

        // Note: We don't apply random variance here since this is a recalculation
        // Random variance should only be applied once at project start

        float devMult = GetDevDurationMultiplier(studio);
        weeksInt = Mathf.RoundToInt(weeksInt * devMult);
        if (weeksInt < 1) weeksInt = 1;

        return Mathf.Max(1, weeksInt);
    }

    public static long SafeGetFirmenwert(publisherScript studio)
    {
        if (studio == null) return 0L;
        if (IsOrganicStudio(studio))
        {
            return GetOrganicSaleValue(studio);
        }

        long fVal = studio.firmenwert;
        if (fVal > 30000000)
        {
            fVal = 30000000L;
        }

        mainScript main = GetMainScript();
        long years = main != null ? (main.year - 1975) : 0L;
        if (years < 0) years = 0;
        if (years > 50) years = 50L;

        long num2 = years * fVal;
        if (studio.ownPlatform)
        {
            num2 *= 3;
        }

        if (main != null)
        {
            num2 = main.difficulty switch
            {
                0 => num2 / 5,
                1 => num2 / 4,
                2 => num2 / 3,
                3 => num2 / 2,
                4 => num2,
                5 => num2 * 2,
                _ => num2,
            };
        }

        // Add IP values safely
        if (studio.games_ != null && studio.games_.arrayGamesScripts != null)
        {
            try
            {
                for (int i = 0; i < studio.games_.arrayGamesScripts.Length; i++)
                {
                    gameScript g = studio.games_.arrayGamesScripts[i];
                    if (g != null && g.sellsTotal > 0 && !g.inDevelopment && !g.schublade && !g.pubAngebot && !g.auftragsspiel && g.IsMyIP(studio) && g.mainIP == g.myID)
                    {
                        num2 += g.GetIpWert();
                    }
                }
            }
            catch { }
        }

        return num2;
    }

    private static gameScript SafeFindGameInDevelopment(publisherScript studio)
    {
        if (studio == null) return null;

        if (studio.games_ != null && studio.games_.arrayGamesScripts != null)
        {
            try
            {
                for (int i = 0; i < studio.games_.arrayGamesScripts.Length; i++)
                {
                    gameScript g = studio.games_.arrayGamesScripts[i];
                    if (g != null && g.developerID == studio.myID && g.inDevelopment && !g.isOnMarket)
                    {
                        return g;
                    }
                }
            }
            catch { }
        }

        try
        {
            games gamesManager = GetGamesScript();
            if (gamesManager != null && gamesManager.arrayGamesScripts != null)
            {
                for (int i = 0; i < gamesManager.arrayGamesScripts.Length; i++)
                {
                    gameScript g = gamesManager.arrayGamesScripts[i];
                    if (g != null && g.developerID == studio.myID && g.inDevelopment && !g.isOnMarket)
                    {
                        return g;
                    }
                }
            }
        }
        catch { }

        return null;
    }

    private static float GetDevDurationMultiplier(publisherScript studio)
    {
        if (studio == null) return 1f;
        int val = studio.tf_entwicklungsdauer;
        if (val == 0) return cfgDevDurationShort    != null ? cfgDevDurationShort.Value    : 0.65f;
        if (val == 1) return cfgDevDurationBalanced != null ? cfgDevDurationBalanced.Value : 0.85f;
        if (val == 2) return cfgDevDurationGenerous != null ? cfgDevDurationGenerous.Value : 1.00f;
        return 1f;
    }

    private static void ScaleSubsidiaryTimeline(publisherScript studio, gameScript game, int weeksInt)
    {
        if (studio == null || game == null) return;

        float devMult = GetDevDurationMultiplier(studio);
        weeksInt = Mathf.RoundToInt(weeksInt * devMult);
        if (weeksInt < 1) weeksInt = 1;

        GetGameWeeks(studio, game, out float wRemainingPrev, out float wOriginalPrev);

        if (wOriginalPrev > 0f && wRemainingPrev > 0f && wRemainingPrev < 9990f)
        {
            double ratio = (double)wRemainingPrev / wOriginalPrev;
            if (ratio < 0.0) ratio = 0.0;
            if (ratio > 1.0) ratio = 1.0;

            float wRemainingNew = (float)(ratio * weeksInt);
            if (wRemainingNew < 1f && ratio > 0.0) wRemainingNew = 1f;

            SetGameWeeks(studio, game, wRemainingNew, weeksInt);
        }
        else
        {
            SetGameWeeks(studio, game, weeksInt, weeksInt);
        }
    }

    // ───────────────────────────────────────────────────────
    //  Safe Menu UI Helper Methods for Menu_Stats_Tochterfirma_Main
    // ───────────────────────────────────────────────────────
    private static void SafeFindScripts(Menu_Stats_Tochterfirma_Main menu)
    {
        if (menu == null) return;

        GameObject main = GameObject.FindWithTag("Main");
        if (main == null) main = GameObject.Find("Main");

        mainScript mS = null;
        if (main != null)
        {
            mainPSField?.SetValue(menu, null); // reset to allow fresh fetch
            mS = main.GetComponent<mainScript>();
            if (mS != null)
            {
                mainMSField?.SetValue(menu, mS);
                if (mS.tS_ != null)
                {
                    mainTSField?.SetValue(menu, mS.tS_);
                }
            }

            genres genComp = main.GetComponent<genres>();
            if (genComp != null)
            {
                mainGenresField?.SetValue(menu, genComp);
            }
        }
        else
        {
            // fallback: direct singleton lookup
            mS = UnityEngine.Object.FindObjectOfType<mainScript>();
            if (mS != null)
            {
                mainMSField?.SetValue(menu, mS);
                if (mS.tS_ != null)
                {
                    mainTSField?.SetValue(menu, mS.tS_);
                }
            }
            genres genComp = UnityEngine.Object.FindObjectOfType<genres>();
            if (genComp != null)
            {
                mainGenresField?.SetValue(menu, genComp);
            }
        }

        GameObject sfxObj = GameObject.Find("SFX");
        if (sfxObj != null)
        {
            sfxScript sfx = sfxObj.GetComponent<sfxScript>();
            if (sfx != null)
            {
                mainSfxField?.SetValue(menu, sfx);
            }
        }
        else
        {
            sfxScript sfx = UnityEngine.Object.FindObjectOfType<sfxScript>();
            if (sfx != null)
            {
                mainSfxField?.SetValue(menu, sfx);
            }
        }

        GameObject canvas = GameObject.Find("CanvasInGameMenu");
        if (canvas != null)
        {
            GUI_Main guiMain = canvas.GetComponent<GUI_Main>();
            if (guiMain != null)
            {
                mainGuiMainField?.SetValue(menu, guiMain);
            }
        }
        else
        {
            GUI_Main guiMain = UnityEngine.Object.FindObjectOfType<GUI_Main>();
            if (guiMain != null)
            {
                mainGuiMainField?.SetValue(menu, guiMain);
            }
        }
    }

    private static void SafeFindScriptsSettings(Menu_Stats_TochterfirmaSettings menu)
    {
        if (menu == null) return;
        GameObject main = GameObject.FindWithTag("Main");
        if (main == null) main = GameObject.Find("Main");

        if (main != null)
        {
            mainGameObjectSettingsField?.SetValue(menu, main);
            mainMSSettingsField?.SetValue(menu, main.GetComponent<mainScript>());
            mainTSSettingsField?.SetValue(menu, main.GetComponent<textScript>());
            mainGenresSettingsField?.SetValue(menu, main.GetComponent<genres>());
            mainGamesSettingsField?.SetValue(menu, main.GetComponent<games>());
        }
        else
        {
            mainScript mS = UnityEngine.Object.FindObjectOfType<mainScript>();
            if (mS != null)
            {
                mainMSSettingsField?.SetValue(menu, mS);
                if (mS.tS_ != null) mainTSSettingsField?.SetValue(menu, mS.tS_);
            }
            genres genComp = UnityEngine.Object.FindObjectOfType<genres>();
            if (genComp != null) mainGenresSettingsField?.SetValue(menu, genComp);
            games gamesComp = UnityEngine.Object.FindObjectOfType<games>();
            if (gamesComp != null) mainGamesSettingsField?.SetValue(menu, gamesComp);
        }

        GameObject sfxObj = GameObject.Find("SFX");
        if (sfxObj != null)
        {
            sfxScript sfx = sfxObj.GetComponent<sfxScript>();
            if (sfx != null) mainSfxSettingsField?.SetValue(menu, sfx);
        }
        else
        {
            sfxScript sfx = UnityEngine.Object.FindObjectOfType<sfxScript>();
            if (sfx != null) mainSfxSettingsField?.SetValue(menu, sfx);
        }

        GameObject canvas = GameObject.Find("CanvasInGameMenu");
        if (canvas != null)
        {
            GUI_Main guiMain = canvas.GetComponent<GUI_Main>();
            if (guiMain != null) mainGuiMainSettingsField?.SetValue(menu, guiMain);
        }
        else
        {
            GUI_Main guiMain = UnityEngine.Object.FindObjectOfType<GUI_Main>();
            if (guiMain != null) mainGuiMainSettingsField?.SetValue(menu, guiMain);
        }
    }

    private static void SafeSetText(GameObject[] uiObjects, int index, string text)
    {
        if (uiObjects != null && index >= 0 && index < uiObjects.Length && uiObjects[index] != null)
        {
            Text t = uiObjects[index].GetComponent<Text>();
            if (t != null) t.text = text;
        }
    }

    private static void SafeSetDropdownValue(GameObject[] uiObjects, int index, int value)
    {
        if (uiObjects != null && index >= 0 && index < uiObjects.Length && uiObjects[index] != null)
        {
            Dropdown d = uiObjects[index].GetComponent<Dropdown>();
            if (d != null) d.value = value;
        }
    }

    private static void SafeSetToggleIsOn(GameObject[] uiObjects, int index, bool isOn)
    {
        if (uiObjects != null && index >= 0 && index < uiObjects.Length && uiObjects[index] != null)
        {
            Toggle t = uiObjects[index].GetComponent<Toggle>();
            if (t != null) t.isOn = isOn;
        }
    }

    private static void SafeClearAndAddOptions(GameObject[] uiObjects, int index, List<string> options)
    {
        if (uiObjects != null && index >= 0 && index < uiObjects.Length && uiObjects[index] != null)
        {
            Dropdown d = uiObjects[index].GetComponent<Dropdown>();
            if (d != null)
            {
                d.ClearOptions();
                d.AddOptions(options);
            }
        }
    }

    private static void SafeSetToggleInteractable(GameObject[] uiObjects, int index, bool interactable)
    {
        if (uiObjects != null && index >= 0 && index < uiObjects.Length && uiObjects[index] != null)
        {
            Toggle t = uiObjects[index].GetComponent<Toggle>();
            if (t != null) t.interactable = interactable;
        }
    }

    private static void SafeSetButtonInteractable(GameObject[] uiObjects, int index, bool interactable)
    {
        if (uiObjects != null && index >= 0 && index < uiObjects.Length && uiObjects[index] != null)
        {
            Button b = uiObjects[index].GetComponent<Button>();
            if (b != null) b.interactable = interactable;
        }
    }

    private static void SafeSetDropdownInteractable(GameObject[] uiObjects, int index, bool interactable)
    {
        if (uiObjects != null && index >= 0 && index < uiObjects.Length && uiObjects[index] != null)
        {
            Dropdown d = uiObjects[index].GetComponent<Dropdown>();
            if (d != null) d.interactable = interactable;
        }
    }

    [HarmonyPatch(typeof(Menu_Stats_Tochterfirma_Main), "FindScripts")]
    private static class Menu_Stats_Tochterfirma_Main_FindScripts_Patch
    {
        [HarmonyPrefix]
        static bool Prefix(Menu_Stats_Tochterfirma_Main __instance)
        {
            try
            {
                SafeFindScripts(__instance);
                return false; // Skip vanilla
            }
            catch (Exception ex)
            {
                if (log != null) log.LogError("Error in Menu_Stats_Tochterfirma_Main.FindScripts: " + ex);
                return true; // Fallback
            }
        }
    }

    [HarmonyPatch(typeof(Menu_Stats_Tochterfirma_Main), "Init")]
    private static class Menu_Stats_Tochterfirma_Main_Init_PrefixPatch
    {
        [HarmonyPrefix]
        static bool Prefix(Menu_Stats_Tochterfirma_Main __instance, publisherScript script_)
        {
            try
            {
                if (script_ == null) return false;
                
                if (log != null) log.LogInfo("[DEBUG] Init Prefix: Before SafeFindScripts");
                SafeFindScripts(__instance);
                if (log != null) log.LogInfo("[DEBUG] Init Prefix: After SafeFindScripts");
                
                mainPSField?.SetValue(__instance, script_);
                if (log != null) log.LogInfo("[DEBUG] Init Prefix: Set pS_");
                
                if (log != null) log.LogInfo("[DEBUG] Init Prefix: Before FindGameInDevelopment");
                gameScript nextGame = script_.FindGameInDevelopment();
                if (log != null) log.LogInfo("[DEBUG] Init Prefix: After FindGameInDevelopment, nextGame=" + (nextGame != null ? nextGame.myName : "null"));
                
                mainNextGameField?.SetValue(__instance, nextGame);
                if (log != null) log.LogInfo("[DEBUG] Init Prefix: Set nextGame_");
                
                if (log != null) log.LogInfo("[DEBUG] Init Prefix: Before UpdateData");
                __instance.UpdateData();
                if (log != null) log.LogInfo("[DEBUG] Init Prefix: After UpdateData");
                
                return false; // Skip vanilla
            }
            catch (Exception ex)
            {
                if (log != null) log.LogError("Error in Menu_Stats_Tochterfirma_Main.Init Prefix: " + ex);
                return true; // Fallback
            }
        }
    }

    [HarmonyPatch(typeof(Menu_Stats_Tochterfirma_Main), "Update")]
    private static class Menu_Stats_Tochterfirma_Main_Update_PrefixPatch
    {
        [HarmonyPrefix]
        static bool Prefix(Menu_Stats_Tochterfirma_Main __instance)
        {
            try
            {
                publisherScript pS = mainPSField?.GetValue(__instance) as publisherScript;
                if (pS == null) return false;

                mainScript mS = mainMSField?.GetValue(__instance) as mainScript;
                textScript tS = mainTSField?.GetValue(__instance) as textScript;

                if (mS == null || tS == null)
                {
                    SafeFindScripts(__instance);
                    mS = mainMSField?.GetValue(__instance) as mainScript;
                    tS = mainTSField?.GetValue(__instance) as textScript;
                }

                if (mS == null || tS == null || __instance.uiObjects == null) return false;

                if (__instance.uiObjects.Length > 7 && __instance.uiObjects[7] != null)
                {
                    Text textComp = __instance.uiObjects[7].GetComponent<Text>();
                    if (textComp != null)
                    {
                        textComp.text = tS.GetText(685) + ": <b>" + pS.GetFirmenwertString() + "</b>";
                    }
                }

                if (__instance.uiObjects.Length > 14 && __instance.uiObjects[14] != null)
                {
                    Text textComp = __instance.uiObjects[14].GetComponent<Text>();
                    if (textComp != null)
                    {
                        textComp.text = tS.GetText(1934) + ": <b>" + mS.GetMoney(CalculateDynamicVerwaltungskosten(pS), showDollar: true) + "</b>";
                    }
                }
            }
            catch (Exception ex)
            {
                if (log != null) log.LogWarning("Safe Update prefix failed: " + ex.ToString());
            }
            return false; // Skip vanilla Update
        }
    }

    [HarmonyPatch(typeof(Menu_Stats_Tochterfirma_Main), "UpdateData")]
    private static class Menu_Stats_Tochterfirma_Main_UpdateData_PrefixPatch
    {
        [HarmonyPrefix]
        static bool Prefix(Menu_Stats_Tochterfirma_Main __instance)
        {
            try
            {
                publisherScript pS = mainPSField?.GetValue(__instance) as publisherScript;
                if (pS == null) return false;

                mainScript mS = mainMSField?.GetValue(__instance) as mainScript;
                textScript tS = mainTSField?.GetValue(__instance) as textScript;
                genres genres = mainGenresField?.GetValue(__instance) as genres;
                GUI_Main guiMain = mainGuiMainField?.GetValue(__instance) as GUI_Main;
                gameScript nextGame = mainNextGameField?.GetValue(__instance) as gameScript;

                if (mS == null || tS == null || genres == null || guiMain == null)
                {
                    SafeFindScripts(__instance);
                    mS = mainMSField?.GetValue(__instance) as mainScript;
                    tS = mainTSField?.GetValue(__instance) as textScript;
                    genres = mainGenresField?.GetValue(__instance) as genres;
                    guiMain = mainGuiMainField?.GetValue(__instance) as GUI_Main;
                }

                if (mS == null || tS == null || genres == null || guiMain == null || __instance.uiObjects == null) return false;

                // Safe helper to set text
                System.Action<int, string> safeSetText = (index, val) => {
                    if (__instance.uiObjects.Length > index && __instance.uiObjects[index] != null) {
                        Text txt = __instance.uiObjects[index].GetComponent<Text>();
                        if (txt != null) txt.text = val;
                    }
                };

                // Safe helper to set sprite
                System.Action<int, Sprite> safeSetSprite = (index, sprite) => {
                    if (__instance.uiObjects.Length > index && __instance.uiObjects[index] != null) {
                        Image img = __instance.uiObjects[index].GetComponent<Image>();
                        if (img != null) img.sprite = sprite;
                    }
                };

                // Safe helper to set active
                System.Action<int, bool> safeSetActive = (index, active) => {
                    if (__instance.uiObjects.Length > index && __instance.uiObjects[index] != null) {
                        __instance.uiObjects[index].SetActive(active);
                    }
                };

                safeSetSprite(1, pS.GetLogo());

                if (__instance.uiObjects.Length > 2 && __instance.uiObjects[2] != null)
                {
                    guiMain.DrawStarsColor(__instance.uiObjects[2], Mathf.RoundToInt(pS.stars / 20f), Color.white);
                }

                safeSetText(5, pS.GetDateString());
                safeSetSprite(6, genres.GetPic(pS.fanGenre));

                if (__instance.uiObjects.Length > 6 && __instance.uiObjects[6] != null)
                {
                    tooltip tooltip = __instance.uiObjects[6].GetComponent<tooltip>();
                    if (tooltip != null)
                    {
                        tooltip.c = tS.GetText(437) + ": <b>" + genres.GetName(pS.fanGenre) + "</b>";
                    }
                }

                safeSetText(7, tS.GetText(685) + ": <b>" + pS.GetFirmenwertString() + "</b>");
                safeSetText(14, tS.GetText(1934) + ": <b>" + mS.GetMoney(CalculateDynamicVerwaltungskosten(pS), showDollar: true) + "</b>");

                if (pS.developer)
                {
                    // Use the game with the least weeks remaining across all slots
                    float closestRemainingWeeks;
                    float closestProgress;
                    gameScript closestGame = FindSlotGameClosestToRelease(pS, out closestRemainingWeeks, out closestProgress);

                    int displayWeeks = Mathf.Max(2, Mathf.RoundToInt(closestRemainingWeeks));
                    string text = tS.GetText(1948);
                    text = text.Replace("<NUM>", "<color=blue><b>" + displayWeeks + "</b></color>");

                    if (__instance.uiObjects.Length > 28 && __instance.uiObjects[28] != null)
                    {
                        tooltip tooltip = __instance.uiObjects[28].GetComponent<tooltip>();
                        if (tooltip != null) tooltip.c = text;
                    }

                    safeSetText(29, displayWeeks.ToString());

                    if (__instance.uiObjects.Length > 19 && __instance.uiObjects[19] != null)
                    {
                        Image img = __instance.uiObjects[19].GetComponent<Image>();
                        if (img != null) img.fillAmount = closestProgress;
                    }

                    if (closestGame != null)
                    {
                        if (closestRemainingWeeks > 2f)
                        {
                            safeSetText(20, tS.GetText(1944) + ": " + Mathf.RoundToInt(closestProgress * 100f) + "%");
                        }
                        else
                        {
                            string statusText = tS.GetText(1947);
                            if (closestGame.HasUnreleasedPlattform())
                            {
                                statusText = tS.GetText(2316);
                            }
                            safeSetText(20, statusText);
                        }

                        safeSetText(23, closestGame.GetNameWithTag());
                        safeSetText(26, mS.Round(closestGame.GetIpBekanntheit(), 1).ToString());
                        safeSetSprite(24, closestGame.GetTypSprite());
                        safeSetSprite(25, closestGame.GetSizeSprite());

                        string genreString = closestGame.GetGenreString();
                        if (closestGame.subgenre != -1)
                        {
                            genreString += " / " + closestGame.GetSubGenreString();
                        }
                        safeSetText(27, genreString);

                        if (__instance.uiObjects.Length > 30 && __instance.uiObjects[30] != null)
                        {
                            Button btn = __instance.uiObjects[30].GetComponent<Button>();
                            if (btn != null) btn.interactable = true;
                        }
                        if (__instance.uiObjects.Length > 31 && __instance.uiObjects[31] != null)
                        {
                            Button btn = __instance.uiObjects[31].GetComponent<Button>();
                            if (btn != null) btn.interactable = true;
                        }
                    }
                    else
                    {
                        safeSetText(23, "");
                        safeSetText(26, "");
                        safeSetSprite(24, guiMain.uiSprites != null && guiMain.uiSprites.Length > 19 ? guiMain.uiSprites[19] : null);
                        safeSetSprite(25, guiMain.uiSprites != null && guiMain.uiSprites.Length > 19 ? guiMain.uiSprites[19] : null);
                        safeSetText(27, "");
                        safeSetText(29, "");
                        if (__instance.uiObjects.Length > 30 && __instance.uiObjects[30] != null)
                        {
                            Button btn = __instance.uiObjects[30].GetComponent<Button>();
                            if (btn != null) btn.interactable = false;
                        }
                        if (__instance.uiObjects.Length > 31 && __instance.uiObjects[31] != null)
                        {
                            Button btn = __instance.uiObjects[31].GetComponent<Button>();
                            if (btn != null) btn.interactable = false;
                        }
                        safeSetText(20, tS.GetText(1949));
                        if (__instance.uiObjects.Length > 19 && __instance.uiObjects[19] != null)
                        {
                            Image img = __instance.uiObjects[19].GetComponent<Image>();
                            if (img != null) img.fillAmount = 0f;
                        }
                        if (__instance.uiObjects.Length > 28 && __instance.uiObjects[28] != null)
                        {
                            tooltip tooltip = __instance.uiObjects[28].GetComponent<tooltip>();
                            if (tooltip != null) tooltip.c = "";
                        }
                    }
                }
                else
                {
                    safeSetText(23, "<i>" + tS.GetText(2029) + "</i>");
                    safeSetText(26, "");
                    safeSetSprite(24, guiMain.uiSprites != null && guiMain.uiSprites.Length > 19 ? guiMain.uiSprites[19] : null);
                    safeSetSprite(25, guiMain.uiSprites != null && guiMain.uiSprites.Length > 19 ? guiMain.uiSprites[19] : null);
                    safeSetText(27, "");
                    safeSetText(29, "");
                    if (__instance.uiObjects.Length > 30 && __instance.uiObjects[30] != null)
                    {
                        Button btn = __instance.uiObjects[30].GetComponent<Button>();
                        if (btn != null) btn.interactable = false;
                    }
                    if (__instance.uiObjects.Length > 31 && __instance.uiObjects[31] != null)
                    {
                        Button btn = __instance.uiObjects[31].GetComponent<Button>();
                        if (btn != null) btn.interactable = false;
                    }
                    safeSetText(21, "");
                    safeSetText(20, tS.GetText(1949));
                    if (__instance.uiObjects.Length > 19 && __instance.uiObjects[19] != null)
                    {
                        Image img = __instance.uiObjects[19].GetComponent<Image>();
                        if (img != null) img.fillAmount = 0f;
                    }
                    if (__instance.uiObjects.Length > 28 && __instance.uiObjects[28] != null)
                    {
                        tooltip tooltip = __instance.uiObjects[28].GetComponent<tooltip>();
                        if (tooltip != null) tooltip.c = "";
                    }
                }

                if (pS.Geschlossen())
                {
                    safeSetActive(12, true);
                    safeSetActive(18, true);
                    safeSetText(0, "<color=red>" + pS.GetName() + "</color>");
                    safeSetText(10, pS.GetDeveloperPublisherString());
                }
                else
                {
                    safeSetActive(12, false);
                    safeSetActive(18, false);
                    safeSetText(0, pS.GetName());
                    safeSetText(10, pS.GetDeveloperPublisherString());
                }

                if (!pS.publisher)
                {
                    safeSetActive(13, true);
                    safeSetText(4, tS.GetText(436) + ": <b>$ -</b>");
                }
                else
                {
                    safeSetActive(13, false);
                    safeSetText(4, tS.GetText(436) + ": <b>$" + mS.Round(pS.share, 1) + "</b>");
                }

                if (!pS.tf_publisher)
                {
                    if (__instance.uiObjects.Length > 8 && __instance.uiObjects[8] != null)
                    {
                        Button btn = __instance.uiObjects[8].GetComponent<Button>();
                        if (btn != null) btn.interactable = false;
                    }
                    if (__instance.uiObjects.Length > 16 && __instance.uiObjects[16] != null)
                    {
                        Button btn = __instance.uiObjects[16].GetComponent<Button>();
                        if (btn != null) btn.interactable = true;
                    }
                    if (__instance.uiObjects.Length > 32 && __instance.uiObjects[32] != null)
                    {
                        Button btn = __instance.uiObjects[32].GetComponent<Button>();
                        if (btn != null) btn.interactable = false;
                    }
                }
                else
                {
                    if (__instance.uiObjects.Length > 8 && __instance.uiObjects[8] != null)
                    {
                        Button btn = __instance.uiObjects[8].GetComponent<Button>();
                        if (btn != null) btn.interactable = true;
                    }
                    if (__instance.uiObjects.Length > 16 && __instance.uiObjects[16] != null)
                    {
                        Button btn = __instance.uiObjects[16].GetComponent<Button>();
                        if (btn != null) btn.interactable = false;
                    }
                    if (__instance.uiObjects.Length > 32 && __instance.uiObjects[32] != null)
                    {
                        Button btn = __instance.uiObjects[32].GetComponent<Button>();
                        if (btn != null) btn.interactable = true;
                    }
                }

                if (!pS.tf_developer)
                {
                    if (__instance.uiObjects.Length > 9 && __instance.uiObjects[9] != null)
                    {
                        Button btn = __instance.uiObjects[9].GetComponent<Button>();
                        if (btn != null) btn.interactable = false;
                    }
                    if (__instance.uiObjects.Length > 17 && __instance.uiObjects[17] != null)
                    {
                        Button btn = __instance.uiObjects[17].GetComponent<Button>();
                        if (btn != null) btn.interactable = true;
                    }
                }
                else
                {
                    if (__instance.uiObjects.Length > 9 && __instance.uiObjects[9] != null)
                    {
                        Button btn = __instance.uiObjects[9].GetComponent<Button>();
                        if (btn != null) btn.interactable = true;
                    }
                    if (__instance.uiObjects.Length > 17 && __instance.uiObjects[17] != null)
                    {
                        Button btn = __instance.uiObjects[17].GetComponent<Button>();
                        if (btn != null) btn.interactable = false;
                    }
                }

                if (pS.stars >= 100f || pS.GetStarsAmount() >= 5)
                {
                    if (__instance.uiObjects.Length > 15 && __instance.uiObjects[15] != null)
                    {
                        Button btn = __instance.uiObjects[15].GetComponent<Button>();
                        if (btn != null) btn.interactable = false;
                    }
                }
                else
                {
                    if (__instance.uiObjects.Length > 15 && __instance.uiObjects[15] != null)
                    {
                        Button btn = __instance.uiObjects[15].GetComponent<Button>();
                        if (btn != null) btn.interactable = true;
                    }
                }

                // Call UpdateGewinnanteilTooltip safely
                if (__instance.uiObjects.Length > 22 && __instance.uiObjects[22] != null)
                {
                    string tooltipText = tS.GetText(1991);
                    tooltipText = tooltipText.Replace("<NUM>", "<color=blue><b>" + mS.GetMoney(Mathf.RoundToInt(pS.share), showDollar: true) + "</b></color>");
                    tooltip tooltip = __instance.uiObjects[22].GetComponent<tooltip>();
                    if (tooltip != null) tooltip.c = tooltipText;
                }
            }
            catch (Exception ex)
            {
                if (log != null) log.LogWarning("Safe UpdateData prefix failed: " + ex.ToString());
            }
            return false; // Skip vanilla UpdateData
        }
    }

    // ───────────────────────────────────────────────────────
    //  PATCH: Menu_Statistics_Tochterfirmen.SetData (Prefix)
    // ───────────────────────────────────────────────────────
    [HarmonyPatch(typeof(Menu_Statistics_Tochterfirmen), "SetData")]
    private static class Menu_Statistics_Tochterfirmen_SetData_Patch
    {
        [HarmonyPrefix]
        static bool Prefix(Menu_Statistics_Tochterfirmen __instance)
        {
            try
            {
                SafeMenuSetData(__instance);
                return false; // Skip vanilla
            }
            catch (Exception ex)
            {
                if (log != null) log.LogError("Error in Menu_Statistics_Tochterfirmen.SetData Prefix: " + ex);
                return true; // Fallback to vanilla
            }
        }
    }

    // ───────────────────────────────────────────────────────
    //  PATCH: Menu_Statistics_Tochterfirmen.DROPDOWN_Sort (Prefix)
    // ───────────────────────────────────────────────────────
    [HarmonyPatch(typeof(Menu_Statistics_Tochterfirmen), "DROPDOWN_Sort")]
    private static class Menu_Statistics_Tochterfirmen_DROPDOWN_Sort_Patch
    {
        [HarmonyPrefix]
        static bool Prefix(Menu_Statistics_Tochterfirmen __instance)
        {
            try
            {
                SafeMenuDropdownSort(__instance);
                return false; // Skip vanilla
            }
            catch (Exception ex)
            {
                if (log != null) log.LogError("Error in Menu_Statistics_Tochterfirmen.DROPDOWN_Sort Prefix: " + ex);
                return true; // Fallback to vanilla
            }
        }
    }

    // ───────────────────────────────────────────────────────
    //  Safe Menu UI Helper Methods
    // ───────────────────────────────────────────────────────
    private static bool SafeExists(GameObject parent, int id)
    {
        if (parent == null) return false;
        int childCount = parent.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = parent.transform.GetChild(i);
            if (child == null || !child.gameObject.activeSelf) continue;
            Item_Stats_Tochterfirma item = child.GetComponent<Item_Stats_Tochterfirma>();
            if (item != null && item.pS_ != null && item.pS_.myID == id)
            {
                return true;
            }
        }
        return false;
    }

    private static void SafeMenuDropdownSort(Menu_Statistics_Tochterfirmen menu)
    {
        if (menu == null || menu.uiObjects == null || menu.uiObjects.Length < 6) return;

        Dropdown dropdown = menu.uiObjects[5]?.GetComponent<Dropdown>();
        if (dropdown == null) return;

        int value = dropdown.value;
        PlayerPrefs.SetInt(menu.uiObjects[5].name, value);

        GameObject contentPanel = menu.uiObjects[0];
        if (contentPanel == null) return;

        int childCount = contentPanel.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = contentPanel.transform.GetChild(i);
            if (child == null) continue;

            Item_Stats_Tochterfirma component = child.GetComponent<Item_Stats_Tochterfirma>();
            if (component != null && component.pS_ != null)
            {
                publisherScript pS = component.pS_;
                switch (value)
                {
                    case 0:
                        child.gameObject.name = pS.GetName();
                        break;
                    case 1:
                        child.gameObject.name = pS.stars.ToString();
                        break;
                    case 2:
                        child.gameObject.name = SafeGetFirmenwert(pS).ToString();
                        break;
                    case 3:
                        child.gameObject.name = pS.GetAmountGames().ToString();
                        break;
                    case 4:
                        child.gameObject.name = pS.GetEntwicklungsFortschritt().ToString();
                        break;
                    case 5:
                        child.gameObject.name = CalculateDynamicVerwaltungskosten(pS).ToString();
                        break;
                    case 6:
                        child.gameObject.name = pS.tf_umsatz_allTime.ToString();
                        break;
                    case 7:
                        child.gameObject.name = pS.GetTochterfirmaUmsatz24Monate().ToString();
                        break;
                    default:
                        child.gameObject.name = pS.GetName();
                        break;
                }
            }
        }

        mainScript mS = menuMSField?.GetValue(menu) as mainScript;
        if (mS != null)
        {
            if (value == 0)
            {
                mS.SortChildrenByName(contentPanel);
            }
            else
            {
                mS.SortChildrenByFloat(contentPanel);
            }
        }
    }

    private static void SafeMenuSetData(Menu_Statistics_Tochterfirmen menu)
    {
        if (menu == null || menu.uiObjects == null || menu.uiObjects.Length < 8 || menu.uiPrefabs == null || menu.uiPrefabs.Length < 1) return;

        mainScript mS = menuMSField?.GetValue(menu) as mainScript;
        GUI_Main guiMain = menuGuiMainField?.GetValue(menu) as GUI_Main;
        sfxScript sfx = menuSfxField?.GetValue(menu) as sfxScript;
        textScript tS = menuTSField?.GetValue(menu) as textScript;
        genres genres = menuGenresField?.GetValue(menu) as genres;
        string searchStringA = menuSearchStringAField?.GetValue(menu) as string ?? "";

        long totalAdminCost = 0L;
        GameObject[] publishers = GameObject.FindGameObjectsWithTag("Publisher");

        GameObject contentPanel = menu.uiObjects[0];
        GameObject searchInput = menu.uiObjects[6];
        GameObject emptyBanner = menu.uiObjects[4];
        GameObject totalLabel = menu.uiObjects[7];

        if (contentPanel == null) return;

        string searchInputText = "";
        if (searchInput != null)
        {
            InputField inputComp = searchInput.GetComponent<InputField>();
            if (inputComp != null)
            {
                searchInputText = inputComp.text;
            }
        }

        searchStringA = searchStringA.ToLower();

        for (int i = 0; i < publishers.Length; i++)
        {
            GameObject pubObj = publishers[i];
            if (pubObj == null) continue;

            publisherScript component = pubObj.GetComponent<publisherScript>();
            if (component != null && component.isUnlocked && component.IsMyTochterfirma())
            {
                string text = component.GetName();
                text = text.ToLower();

                if ((searchInputText.Length <= 0 || text.Contains(searchStringA)) && !SafeExists(contentPanel, component.myID))
                {
                    try
                    {
                        GameObject spawned = UnityEngine.Object.Instantiate(menu.uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, contentPanel.transform);
                        if (spawned != null)
                        {
                            Item_Stats_Tochterfirma component2 = spawned.GetComponent<Item_Stats_Tochterfirma>();
                            if (component2 != null)
                            {
                                component2.pS_ = component;
                                component2.playerID = -1;
                                component2.mS_ = mS;
                                component2.tS_ = tS;
                                component2.sfx_ = sfx;
                                component2.guiMain_ = guiMain;
                                component2.genres_ = genres;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (log != null) log.LogError("Error instantiating daughter studio item: " + ex);
                    }
                }

                if (IsOrganicStudio(component) || IsAcquiredSubsidiary(component))
                {
                    totalAdminCost += CalculateDynamicVerwaltungskosten(component);
                }
                else
                {
                    totalAdminCost += component.GetVerwaltungskosten();
                }
            }
        }

        try
        {
            SafeMenuDropdownSort(menu);
        }
        catch (Exception ex)
        {
            if (log != null) log.LogError("Error in SafeMenuDropdownSort: " + ex);
        }

        if (guiMain != null && emptyBanner != null)
        {
            guiMain.KeinEintrag(contentPanel, emptyBanner);
        }

        if (totalLabel != null && tS != null && mS != null)
        {
            Text textLabel = totalLabel.GetComponent<Text>();
            if (textLabel != null)
            {
                textLabel.text = tS.GetText(1934) + ": <b><color=red>" + mS.GetMoney(totalAdminCost, showDollar: true) + "</color></b>";
            }
        }
    }

    // ───────────────────────────────────────────────────────
    //  PATCHES: Menu_Stats_TochterfirmaSettings Safety & Fallback
    // ───────────────────────────────────────────────────────
    [HarmonyPatch(typeof(Menu_Stats_TochterfirmaSettings), "FindScripts")]
    private static class Menu_Stats_TochterfirmaSettings_FindScripts_Patch
    {
        [HarmonyPrefix]
        static bool Prefix(Menu_Stats_TochterfirmaSettings __instance)
        {
            try
            {
                SafeFindScriptsSettings(__instance);
            }
            catch (Exception ex)
            {
                if (log != null) log.LogError("Error in Menu_Stats_TochterfirmaSettings.FindScripts: " + ex);
            }
            return false; // Skip vanilla
        }
    }

    [HarmonyPatch(typeof(Menu_Stats_TochterfirmaSettings), "InitDropdowns")]
    private static class Menu_Stats_TochterfirmaSettings_InitDropdowns_Patch
    {
        [HarmonyPrefix]
        static bool Prefix(Menu_Stats_TochterfirmaSettings __instance)
        {
            try
            {
                SafeFindScriptsSettings(__instance);
                publisherScript pS = __instance.pS_;
                textScript tS = mainTSSettingsField?.GetValue(__instance) as textScript;
                genres genres = mainGenresSettingsField?.GetValue(__instance) as genres;

                if (pS == null || tS == null || genres == null) return false;

                List<string> list = new List<string>();
                if (pS.tf_publisher)
                {
                    list.Add(tS.GetText(432));
                }
                else
                {
                    list.Add("<color=red>" + tS.GetText(432) + "</color>");
                }
                
                if (pS.tf_developer)
                {
                    list.Add(tS.GetText(274));
                }
                else
                {
                    list.Add("<color=red>" + tS.GetText(274) + "</color>");
                }

                if (pS.tf_publisher && pS.tf_developer)
                {
                    list.Add(tS.GetText(432) + " & " + tS.GetText(274));
                }
                else
                {
                    list.Add("<color=red>" + tS.GetText(432) + " & " + tS.GetText(274) + "</color>");
                }

                SafeClearAndAddOptions(__instance.uiObjects, 5, list);

                list = new List<string>();
                list.Add(tS.GetText(1963));
                list.Add(tS.GetText(1964));
                list.Add(tS.GetText(1965));
                SafeClearAndAddOptions(__instance.uiObjects, 6, list);

                list = new List<string>();
                list.Add(tS.GetText(1966));
                list.Add(tS.GetText(329));
                list.Add(tS.GetText(330));
                list.Add(tS.GetText(331));
                list.Add(tS.GetText(332));
                list.Add(tS.GetText(333));
                list.Add(tS.GetText(2193));
                SafeClearAndAddOptions(__instance.uiObjects, 7, list);

                list = new List<string>();
                list.Add(tS.GetText(1966));
                if (genres.genres_LEVEL != null)
                {
                    for (int i = 0; i < genres.genres_LEVEL.Length; i++)
                    {
                        bool unlocked = genres.genres_UNLOCK != null && i < genres.genres_UNLOCK.Length && genres.genres_UNLOCK[i];
                        if (unlocked)
                        {
                            list.Add(genres.GetName(i));
                        }
                        else
                        {
                            list.Add("<color=red>" + genres.GetName(i) + "</color>");
                        }
                    }
                }
                SafeClearAndAddOptions(__instance.uiObjects, 8, list);

                list = new List<string>();
                list.Add(tS.GetText(1966));
                list.Add("< 10%");
                list.Add("< 20%");
                list.Add("< 30%");
                list.Add("< 40%");
                list.Add("< 50%");
                list.Add("< 60%");
                list.Add("< 70%");
                list.Add("< 80%");
                list.Add("< 90%");
                SafeClearAndAddOptions(__instance.uiObjects, 32, list);
            }
            catch (Exception ex)
            {
                if (log != null) log.LogError("Error in Menu_Stats_TochterfirmaSettings.InitDropdowns: " + ex);
            }
            return false; // Skip vanilla
        }
    }

    [HarmonyPatch(typeof(Menu_Stats_TochterfirmaSettings), "SetData")]
    private static class Menu_Stats_TochterfirmaSettings_SetData_Patch
    {
        [HarmonyPrefix]
        static bool Prefix(Menu_Stats_TochterfirmaSettings __instance)
        {
            try
            {
                publisherScript pS = __instance.pS_;
                if (pS == null) return false;

                GameObject[] uiObjects = __instance.uiObjects;
                if (uiObjects == null) return false;

                if (pS.publisher && !pS.developer)
                {
                    SafeSetDropdownValue(uiObjects, 5, 0);
                }
                if (!pS.publisher && pS.developer)
                {
                    SafeSetDropdownValue(uiObjects, 5, 1);
                }
                if (pS.publisher && pS.developer)
                {
                    SafeSetDropdownValue(uiObjects, 5, 2);
                }

                SafeSetDropdownValue(uiObjects, 6, pS.tf_entwicklungsdauer);
                SafeSetDropdownValue(uiObjects, 7, pS.tf_gameSize);
                SafeSetDropdownValue(uiObjects, 8, pS.tf_gameGenre);

                SafeSetToggleIsOn(uiObjects, 10, pS.tf_autoRelease);
                SafeSetToggleIsOn(uiObjects, 11, pS.tf_onlyPlayerConsole);
                SafeSetToggleIsOn(uiObjects, 12, pS.tf_allowMMO);
                SafeSetToggleIsOn(uiObjects, 13, pS.tf_allowF2P);
                SafeSetToggleIsOn(uiObjects, 14, pS.tf_allowAddon);
                SafeSetToggleIsOn(uiObjects, 15, pS.tf_noArcade);
                SafeSetToggleIsOn(uiObjects, 16, pS.tf_noHandy);
                SafeSetToggleIsOn(uiObjects, 17, pS.tf_noRetro);
                SafeSetToggleIsOn(uiObjects, 18, pS.tf_noPorts);
                SafeSetToggleIsOn(uiObjects, 19, pS.tf_noBudget);
                SafeSetToggleIsOn(uiObjects, 20, pS.tf_noGOTY);
                SafeSetToggleIsOn(uiObjects, 21, pS.tf_noRemaster);
                SafeSetToggleIsOn(uiObjects, 22, pS.tf_noSpinoffs);
                SafeSetToggleIsOn(uiObjects, 23, pS.tf_ownPublisher);

                SafeSetToggleIsOn(uiObjects, 43, pS.tf_autoGamePass);
                SafeSetDropdownValue(uiObjects, 32, pS.tf_autoReleaseVal);
                SafeSetToggleIsOn(uiObjects, 41, pS.tf_noBundles);
                SafeSetToggleIsOn(uiObjects, 42, pS.tf_noAddonBundles);

                __instance.UpdateData();
            }
            catch (Exception ex)
            {
                if (log != null) log.LogError("Error in Menu_Stats_TochterfirmaSettings.SetData: " + ex);
            }
            return false; // Skip vanilla
        }
    }

    [HarmonyPatch(typeof(Menu_Stats_TochterfirmaSettings), "UpdateData")]
    private static class Menu_Stats_TochterfirmaSettings_UpdateData_Patch
    {
        [HarmonyPrefix]
        static bool Prefix(Menu_Stats_TochterfirmaSettings __instance)
        {
            try
            {
                publisherScript pS = __instance.pS_;
                textScript tS = mainTSSettingsField?.GetValue(__instance) as textScript;
                if (pS == null || tS == null) return false;

                GameObject[] uiObjects = __instance.uiObjects;
                if (uiObjects == null) return false;

                if (pS.tf_gameTopic != -1)
                {
                    SafeSetText(uiObjects, 24, "<b>" + tS.GetThemes(pS.tf_gameTopic) + "</b>");
                }
                else
                {
                    SafeSetText(uiObjects, 24, tS.GetText(1966));
                }

                if (pS.tf_ipFocus != null)
                {
                    for (int i = 0; i < pS.tf_ipFocus.Length; i++)
                    {
                        int index = 47 + i;
                        if (pS.tf_ipFocus[i] != -1)
                        {
                            GameObject gameObj = GameObject.Find("GAME_" + pS.tf_ipFocus[i]);
                            if (gameObj != null)
                            {
                                gameScript gameComp = gameObj.GetComponent<gameScript>();
                                if (gameComp != null)
                                {
                                    SafeSetText(uiObjects, index, "<b>" + gameComp.GetIpName() + "</b>");
                                    continue;
                                }
                            }
                            pS.tf_ipFocus[i] = -1;
                            SafeSetText(uiObjects, index, tS.GetText(1966));
                        }
                        else
                        {
                            SafeSetText(uiObjects, index, tS.GetText(1966));
                        }
                    }
                }

                if (pS.tf_engine != -1 && pS.tf_engine != 0)
                {
                    GameObject engObj = GameObject.Find("ENGINE_" + pS.tf_engine);
                    if (engObj != null)
                    {
                        engineScript engComp = engObj.GetComponent<engineScript>();
                        if (engComp != null)
                        {
                            SafeSetText(uiObjects, 31, "<b>" + engComp.GetName() + "</b>");
                        }
                    }
                }
                else
                {
                    SafeSetText(uiObjects, 31, tS.GetText(1966));
                }

                if (pS.tf_platformFocus != null)
                {
                    for (int j = 0; j < pS.tf_platformFocus.Length; j++)
                    {
                        int index = 37 + j;
                        if (pS.tf_platformFocus[j] != -1)
                        {
                            GameObject platObj = GameObject.Find("PLATFORM_" + pS.tf_platformFocus[j]);
                            if (platObj != null)
                            {
                                platformScript platComp = platObj.GetComponent<platformScript>();
                                if (platComp != null)
                                {
                                    if (!platComp.vomMarktGenommen)
                                    {
                                        SafeSetText(uiObjects, index, "<b>" + platComp.GetName() + "</b>");
                                    }
                                    else
                                    {
                                        SafeSetText(uiObjects, index, "<b><color=red>" + platComp.GetName() + "</color></b>");
                                    }
                                }
                            }
                            else
                            {
                                pS.tf_platformFocus[j] = -1;
                                SafeSetText(uiObjects, index, tS.GetText(1966));
                            }
                        }
                        else
                        {
                            SafeSetText(uiObjects, index, tS.GetText(1966));
                        }
                    }
                }

                bool prioritySet = false;
                if (pS.tf_ownPublisherPriority != -1)
                {
                    GameObject pubObj = GameObject.Find("PUB_" + pS.tf_ownPublisherPriority);
                    if (pubObj != null)
                    {
                        publisherScript pubComp = pubObj.GetComponent<publisherScript>();
                        if (pubComp != null && pubComp.IsMyTochterfirma())
                        {
                            SafeSetText(uiObjects, 45, "<b>" + pubComp.GetName() + "</b>");
                            prioritySet = true;
                        }
                    }
                }
                if (!prioritySet)
                {
                    pS.tf_ownPublisherPriority = -1;
                    SafeSetText(uiObjects, 45, tS.GetText(1966));
                }
            }
            catch (Exception ex)
            {
                if (log != null) log.LogError("Error in Menu_Stats_TochterfirmaSettings.UpdateData: " + ex);
            }
            return false; // Skip vanilla
        }
    }

    [HarmonyPatch(typeof(Menu_Stats_TochterfirmaSettings), "LoadCopyToggles")]
    private static class Menu_Stats_TochterfirmaSettings_LoadCopyToggles_Patch
    {
        [HarmonyPrefix]
        static bool Prefix(Menu_Stats_TochterfirmaSettings __instance)
        {
            try
            {
                Toggle[] copyToggles = __instance.copyToggles;
                if (copyToggles == null) return false;

                for (int i = 0; i < copyToggles.Length; i++)
                {
                    if (copyToggles[i] == null) continue;
                    if (PlayerPrefs.HasKey(copyToggles[i].name))
                    {
                        copyToggles[i].isOn = PlayerPrefs.GetInt(copyToggles[i].name) != 0;
                    }
                }
            }
            catch (Exception ex)
            {
                if (log != null) log.LogError("Error in Menu_Stats_TochterfirmaSettings.LoadCopyToggles: " + ex);
            }
            return false; // Skip vanilla
        }
    }

    [HarmonyPatch(typeof(Menu_Stats_TochterfirmaSettings), "Update")]
    private static class Menu_Stats_TochterfirmaSettings_Update_Patch
    {
        [HarmonyPrefix]
        static bool Prefix(Menu_Stats_TochterfirmaSettings __instance)
        {
            try
            {
                GameObject[] uiObjects = __instance.uiObjects;
                if (uiObjects == null) return false;

                if (uiObjects.Length > 2 && uiObjects[2] != null && uiObjects.Length > 3 && uiObjects[3] != null)
                {
                    Animation anim = uiObjects[2].GetComponent<Animation>();
                    Scrollbar scroll = uiObjects[3].GetComponent<Scrollbar>();
                    if (anim != null && scroll != null && anim.IsPlaying("openMenu"))
                    {
                        scroll.value = 1f;
                    }
                }

                bool toggle11IsOn = false;
                if (uiObjects.Length > 11 && uiObjects[11] != null)
                {
                    Toggle t11 = uiObjects[11].GetComponent<Toggle>();
                    if (t11 != null) toggle11IsOn = t11.isOn;
                }

                if (toggle11IsOn)
                {
                    SafeSetToggleIsOn(uiObjects, 15, true);
                    SafeSetToggleIsOn(uiObjects, 16, true);
                    SafeSetToggleIsOn(uiObjects, 17, true);
                    SafeSetToggleIsOn(uiObjects, 18, true);

                    SafeSetToggleInteractable(uiObjects, 15, false);
                    SafeSetToggleInteractable(uiObjects, 16, false);
                    SafeSetToggleInteractable(uiObjects, 17, false);
                    SafeSetToggleInteractable(uiObjects, 18, false);

                    SafeSetButtonInteractable(uiObjects, 33, false);
                    SafeSetButtonInteractable(uiObjects, 34, false);
                    SafeSetButtonInteractable(uiObjects, 35, false);
                    SafeSetButtonInteractable(uiObjects, 36, false);
                }
                else
                {
                    SafeSetToggleInteractable(uiObjects, 15, true);
                    SafeSetToggleInteractable(uiObjects, 16, true);
                    SafeSetToggleInteractable(uiObjects, 17, true);
                    SafeSetToggleInteractable(uiObjects, 18, true);

                    SafeSetButtonInteractable(uiObjects, 33, true);
                    SafeSetButtonInteractable(uiObjects, 34, true);
                    SafeSetButtonInteractable(uiObjects, 35, true);
                    SafeSetButtonInteractable(uiObjects, 36, true);
                }

                bool toggle10IsOn = false;
                if (uiObjects.Length > 10 && uiObjects[10] != null)
                {
                    Toggle t10 = uiObjects[10].GetComponent<Toggle>();
                    if (t10 != null) toggle10IsOn = t10.isOn;
                }
                SafeSetDropdownInteractable(uiObjects, 32, toggle10IsOn);

                bool toggle23IsOn = false;
                if (uiObjects.Length > 23 && uiObjects[23] != null)
                {
                    Toggle t23 = uiObjects[23].GetComponent<Toggle>();
                    if (t23 != null) toggle23IsOn = t23.isOn;
                }
                SafeSetButtonInteractable(uiObjects, 44, toggle23IsOn);
            }
            catch (Exception)
            {
                // Ignore per-frame update exceptions
            }
            return false; // Skip vanilla Update
        }
    }

    // ───────────────────────────────────────────────────────
    //  PATCHES: Menu_Stats_Tochterfirma_Main Upgrade & Action Buttons
    // ───────────────────────────────────────────────────────
    [HarmonyPatch(typeof(Menu_Stats_Tochterfirma_Main), "BUTTON_FirmaAufwerten")]
    private static class Menu_Stats_Tochterfirma_Main_BUTTON_FirmaAufwerten_Patch
    {
        [HarmonyPrefix]
        static bool Prefix(Menu_Stats_Tochterfirma_Main __instance)
        {
            try
            {
                publisherScript pS = mainPSField?.GetValue(__instance) as publisherScript;
                if (pS == null) return false;

                sfxScript sfx = mainSfxField?.GetValue(__instance) as sfxScript;
                if (sfx != null) sfx.PlaySound(3, force: true);

                GUI_Main guiMain = mainGuiMainField?.GetValue(__instance) as GUI_Main;
                if (guiMain == null) guiMain = UnityEngine.Object.FindObjectOfType<GUI_Main>();
                if (guiMain == null) return false;

                GameObject menuObj = null;
                if (guiMain.uiObjects != null && guiMain.uiObjects.Length > 388)
                {
                    menuObj = guiMain.uiObjects[388];
                }

                if (menuObj == null)
                {
                    Menu_W_FirmaAufwerten menuComp = UnityEngine.Object.FindObjectOfType<Menu_W_FirmaAufwerten>(true);
                    if (menuComp != null) menuObj = menuComp.gameObject;
                }

                if (menuObj != null)
                {
                    guiMain.ActivateMenu(menuObj);
                    Menu_W_FirmaAufwerten menuComp = menuObj.GetComponent<Menu_W_FirmaAufwerten>();
                    if (menuComp != null) menuComp.Init(pS);
                }
                else
                {
                    if (log != null) log.LogError("Could not find Menu_W_FirmaAufwerten GameObject!");
                }
            }
            catch (Exception ex)
            {
                if (log != null) log.LogError("Error in Menu_Stats_Tochterfirma_Main.BUTTON_FirmaAufwerten: " + ex);
            }
            return false; // Skip vanilla
        }
    }

    [HarmonyPatch(typeof(Menu_Stats_Tochterfirma_Main), "BUTTON_FirmaAufwertenPublisher")]
    private static class Menu_Stats_Tochterfirma_Main_BUTTON_FirmaAufwertenPublisher_Patch
    {
        [HarmonyPrefix]
        static bool Prefix(Menu_Stats_Tochterfirma_Main __instance)
        {
            try
            {
                publisherScript pS = mainPSField?.GetValue(__instance) as publisherScript;
                if (pS == null) return false;

                sfxScript sfx = mainSfxField?.GetValue(__instance) as sfxScript;
                if (sfx != null) sfx.PlaySound(3, force: true);

                GUI_Main guiMain = mainGuiMainField?.GetValue(__instance) as GUI_Main;
                if (guiMain == null) guiMain = UnityEngine.Object.FindObjectOfType<GUI_Main>();
                if (guiMain == null) return false;

                GameObject menuObj = null;
                if (guiMain.uiObjects != null && guiMain.uiObjects.Length > 389)
                {
                    menuObj = guiMain.uiObjects[389];
                }

                if (menuObj == null)
                {
                    Menu_W_FirmaAufwertenPublisher menuComp = UnityEngine.Object.FindObjectOfType<Menu_W_FirmaAufwertenPublisher>(true);
                    if (menuComp != null) menuObj = menuComp.gameObject;
                }

                if (menuObj != null)
                {
                    guiMain.ActivateMenu(menuObj);
                    Menu_W_FirmaAufwertenPublisher menuComp = menuObj.GetComponent<Menu_W_FirmaAufwertenPublisher>();
                    if (menuComp != null) menuComp.Init(pS);
                }
                else
                {
                    if (log != null) log.LogError("Could not find Menu_W_FirmaAufwertenPublisher GameObject!");
                }
            }
            catch (Exception ex)
            {
                if (log != null) log.LogError("Error in Menu_Stats_Tochterfirma_Main.BUTTON_FirmaAufwertenPublisher: " + ex);
            }
            return false; // Skip vanilla
        }
    }

    [HarmonyPatch(typeof(Menu_Stats_Tochterfirma_Main), "BUTTON_FirmaAufwertenDeveloper")]
    private static class Menu_Stats_Tochterfirma_Main_BUTTON_FirmaAufwertenDeveloper_Patch
    {
        [HarmonyPrefix]
        static bool Prefix(Menu_Stats_Tochterfirma_Main __instance)
        {
            try
            {
                publisherScript pS = mainPSField?.GetValue(__instance) as publisherScript;
                if (pS == null) return false;

                sfxScript sfx = mainSfxField?.GetValue(__instance) as sfxScript;
                if (sfx != null) sfx.PlaySound(3, force: true);

                GUI_Main guiMain = mainGuiMainField?.GetValue(__instance) as GUI_Main;
                if (guiMain == null) guiMain = UnityEngine.Object.FindObjectOfType<GUI_Main>();
                if (guiMain == null) return false;

                GameObject menuObj = null;
                if (guiMain.uiObjects != null && guiMain.uiObjects.Length > 390)
                {
                    menuObj = guiMain.uiObjects[390];
                }

                if (menuObj == null)
                {
                    Menu_W_FirmaAufwertenDeveloper menuComp = UnityEngine.Object.FindObjectOfType<Menu_W_FirmaAufwertenDeveloper>(true);
                    if (menuComp != null) menuObj = menuComp.gameObject;
                }

                if (menuObj != null)
                {
                    guiMain.ActivateMenu(menuObj);
                    Menu_W_FirmaAufwertenDeveloper menuComp = menuObj.GetComponent<Menu_W_FirmaAufwertenDeveloper>();
                    if (menuComp != null) menuComp.Init(pS);
                }
                else
                {
                    if (log != null) log.LogError("Could not find Menu_W_FirmaAufwertenDeveloper GameObject!");
                }
            }
            catch (Exception ex)
            {
                if (log != null) log.LogError("Error in Menu_Stats_Tochterfirma_Main.BUTTON_FirmaAufwertenDeveloper: " + ex);
            }
            return false; // Skip vanilla
        }
    }

    [HarmonyPatch(typeof(Menu_Stats_Tochterfirma_Main), "BUTTON_Settings")]
    private static class Menu_Stats_Tochterfirma_Main_BUTTON_Settings_Patch
    {
        [HarmonyPrefix]
        static bool Prefix(Menu_Stats_Tochterfirma_Main __instance)
        {
            try
            {
                publisherScript pS = mainPSField?.GetValue(__instance) as publisherScript;
                if (pS == null) return false;

                sfxScript sfx = mainSfxField?.GetValue(__instance) as sfxScript;
                if (sfx != null) sfx.PlaySound(3, force: true);

                GUI_Main guiMain = mainGuiMainField?.GetValue(__instance) as GUI_Main;
                if (guiMain == null) guiMain = UnityEngine.Object.FindObjectOfType<GUI_Main>();
                if (guiMain == null) return false;

                GameObject menuObj = null;
                if (guiMain.uiObjects != null && guiMain.uiObjects.Length > 393)
                {
                    menuObj = guiMain.uiObjects[393];
                }

                if (menuObj == null)
                {
                    Menu_Stats_TochterfirmaSettings menuComp = UnityEngine.Object.FindObjectOfType<Menu_Stats_TochterfirmaSettings>(true);
                    if (menuComp != null) menuObj = menuComp.gameObject;
                }

                if (menuObj != null)
                {
                    guiMain.ActivateMenu(menuObj);
                    Menu_Stats_TochterfirmaSettings menuComp = menuObj.GetComponent<Menu_Stats_TochterfirmaSettings>();
                    if (menuComp != null) menuComp.Init(pS);
                }
                else
                {
                    if (log != null) log.LogError("Could not find Menu_Stats_TochterfirmaSettings GameObject!");
                }
            }
            catch (Exception ex)
            {
                if (log != null) log.LogError("Error in Menu_Stats_Tochterfirma_Main.BUTTON_Settings: " + ex);
            }
            return false; // Skip vanilla
        }
    }

    private static float GetUnclampedWeeksForParams(publisherScript studio, gameScript game, int starsCount, int speedLevel)
    {
        if (studio == null || game == null) return 0f;

        bool isOrganic = IsOrganicStudio(studio);
        int gameSize = Mathf.Clamp(game.gameSize, 0, 5);
        int platformCount = CountPlatforms(game);
        string trendState = GetTrendState(studio);

        float starMult = CalcStarMultiplier(starsCount, gameSize);
        float speedMult = CalcSpeedMultiplier(speedLevel, isOrganic);
        float platformMult = CalcPlatformMultiplier(platformCount);
        float typeMult = CalcProjectTypeMultiplier(game);
        float trendMult = CalcTrendMultiplier(trendState);

        float baseWeeks = cfgBaseMidpointWeeks[gameSize].Value;
        return baseWeeks * starMult * speedMult * platformMult * typeMult * trendMult;
    }

    public static void RecalculateActiveProjectTimeline(publisherScript studio)
    {
        if (studio == null || !cfgEnable.Value) return;

        bool isOrganic = IsOrganicStudio(studio);
        bool isAcquired = IsAcquiredSubsidiary(studio);
        if (!isOrganic && !isAcquired) return;
        if (isOrganic && !cfgApplyOrganic.Value) return;
        if (isAcquired && !cfgApplyAcquired.Value) return;

        // Support multiple active slots if Team Slots mod is loaded
        List<gameScript> activeGames = new List<gameScript>();
        try
        {
            if (IsTeamSlotsLoaded())
            {
                SubsidiaryTeamSlotsPlugin.StudioSlotData slotData = SubsidiaryTeamSlotsPlugin.GetStudioSlotData(studio.myID);
                if (slotData != null && slotData.slots != null)
                {
                    games gamesScript = studio.games_ != null ? studio.games_ : UnityEngine.Object.FindObjectOfType<games>();
                    for (int si = 0; si < slotData.slots.Length; si++)
                    {
                        SubsidiaryTeamSlotsPlugin.SlotData slot = slotData.slots[si];
                        if (slot != null && slot.gameID != -1)
                        {
                            gameScript slotGame = FindGameByIDInGlobal(gamesScript, slot.gameID);
                            if (slotGame != null && slotGame.inDevelopment)
                                activeGames.Add(slotGame);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            if (log != null) log.LogWarning("[Timeline] RecalculateActiveProjectTimeline slot lookup failed: " + ex.Message);
        }

        // Fallback to single active game if list is empty (vanilla or no slots)
        if (activeGames.Count == 0)
        {
            gameScript game = SafeFindGameInDevelopment(studio);
            if (game != null)
                activeGames.Add(game);
        }

        foreach (gameScript game in activeGames)
        {
            if (game == null || !game.inDevelopment) continue;

            // ── Read current slot state ──────────────────────────
            GetGameWeeks(studio, game, out float wRemainingPrev, out float wTotalPrev);

            // Guard: skip if no meaningful state to scale from
            if (wTotalPrev <= 0f) continue;

            // Guard: if Team Slots stored the 9999 sentinel as the slot duration (race condition
            // where the autonomous-start hook read studio.newGameInWeeks before the timeline mod
            // had a chance to write real values), treat this slot as 0% complete and let the
            // recalculation stamp in the correct week count from scratch.
            bool slotDataWasSentinel = (wTotalPrev >= 9990f || wRemainingPrev >= 9990f);
            if (slotDataWasSentinel)
            {
                // We can't derive progress from bad data — treat as game just started (0% done).
                // The new totalWeeks will be calculated below and stamped in full.
                wRemainingPrev = 0f;
                wTotalPrev     = 0f; // force sentinel path: skip variance ratio, use raw newClampedWeeks
            }

            // ── Calculate what the original base total would have been BEFORE upgrade ──
            int gameSize = Mathf.Clamp(game.gameSize, 0, 5);
            int platformCount = CountPlatforms(game);
            int slotIdx = GetGameSlotIndex(studio, game);
            
            // Use PRE-upgrade values for original calculation.
            int preStars;
            int preSpeed;
            bool callerProvidedPreValues = (preUpgradeStars >= 0);

            // Guard: for Team Slots slot-specific upgrades, only recalculate the upgraded slot
            if (callerProvidedPreValues && !preUpgradeIsStudioLevel && preUpgradeSlotIndex >= 0 && slotIdx != preUpgradeSlotIndex)
            {
                continue;
            }

            if (callerProvidedPreValues)
            {
                if (preUpgradeIsStudioLevel && slotIdx > 0)
                {
                    // Studio market-star upgrade on a game in an auxiliary team slot.
                    // Auxiliary slots have their OWN independent star level (not the studio-level stars),
                    // so a studio upgrade does NOT change their effective stars.
                    // Seed with preUpgradeStars (not post-upgrade studio.GetStarsAmount()), then let
                    // GetSlotSpecificStats override with the slot's own stars.  Pre == post for
                    // auxiliary slots → variance ratio = 1.0 → no change (correct).
                    preStars = preUpgradeStars;
                    preSpeed = preUpgradeSpeed >= 0 ? preUpgradeSpeed : studio.developmentSpeed;
                    GetSlotSpecificStats(studio, game, ref preStars, ref preSpeed);
                }
                else
                {
                    // Slot 0 studio upgrade, or team slot star/speed upgrade with injected values.
                    preStars = preUpgradeStars;
                    preSpeed = preUpgradeSpeed >= 0 ? preUpgradeSpeed : studio.developmentSpeed;
                }
            }
            else
            {
                // No explicit pre-upgrade values: derive from studio stats and slot overrides.
                preStars = studio.GetStarsAmount();
                preSpeed = studio.developmentSpeed;
                GetSlotSpecificStats(studio, game, ref preStars, ref preSpeed);
            }
            
            string trendState = GetTrendState(studio);

            float preStarMult     = CalcStarMultiplier(preStars, gameSize);
            float preSpeedMult    = CalcSpeedMultiplier(preSpeed, isOrganic);
            float platformMult    = CalcPlatformMultiplier(platformCount);
            float typeMult        = CalcProjectTypeMultiplier(game);
            float trendMult       = CalcTrendMultiplier(trendState);

            float baseWeeks = cfgBaseMidpointWeeks[gameSize].Value;
            float preFinalWeeks = baseWeeks * preStarMult * preSpeedMult * platformMult * typeMult * trendMult;

            int preUpgradeValue = Mathf.Clamp(
                Mathf.RoundToInt(preFinalWeeks),
                cfgHardFloorWeeks[gameSize].Value,
                cfgHardCeilWeeks[gameSize].Value);

            // ── Compute new target total weeks with CURRENT (post-upgrade) stats ──
            int currentStars = studio.GetStarsAmount();
            int currentSpeed = studio.developmentSpeed;

            if (slotIdx != 0)
            {
                GetSlotSpecificStats(studio, game, ref currentStars, ref currentSpeed);
            }

            float currentStarMult  = CalcStarMultiplier(currentStars, gameSize);
            float currentSpeedMult = CalcSpeedMultiplier(currentSpeed, isOrganic);

            float newFinalWeeks = baseWeeks * currentStarMult * currentSpeedMult * platformMult * typeMult * trendMult;

            int newClampedWeeks = Mathf.Clamp(
                Mathf.RoundToInt(newFinalWeeks),
                cfgHardFloorWeeks[gameSize].Value,
                cfgHardCeilWeeks[gameSize].Value);

            float devMult = GetDevDurationMultiplier(studio);
            newClampedWeeks = Mathf.RoundToInt(newClampedWeeks * devMult);
            if (newClampedWeeks < 1) newClampedWeeks = 1;

            int newTotalWeeks;
            float doneFraction;

            if (slotDataWasSentinel || wTotalPrev <= 0f)
            {
                // Slot data was uninitialized (9999 sentinel) or zero — we cannot derive a
                // meaningful progress fraction.  Apply the new total as-is (0% done assumed),
                // which correctly stamps the post-upgrade week count on first real data write.
                newTotalWeeks = newClampedWeeks;
                doneFraction  = 0f;
            }
            else
            {
                // Calculate the variance ratio that was applied to the original game:
                // varianceRatio = actualTotal / baseCalculatedTotal
                // This preserves any random variance the game received at start.
                float originalVarianceRatio = wTotalPrev / (float)preUpgradeValue;

                // Apply the same variance ratio to the new base so variance is preserved
                newTotalWeeks = Mathf.RoundToInt((float)newClampedWeeks * originalVarianceRatio);
                if (newTotalWeeks < 1) newTotalWeeks = 1;

                // ── Preserve the progress fraction ────────────────────
                doneFraction = 1.0f - Mathf.Clamp01(wRemainingPrev / wTotalPrev);
            }

            float newRemaining = newTotalWeeks * (1.0f - doneFraction);
            if (newRemaining < 1f && doneFraction < 1.0f) newRemaining = 1f;

            SetGameWeeks(studio, game, newRemaining, (float)newTotalWeeks);
            
            // NEW: Recalculate dev points based on new week count (preserves progress %)
            RecalculateGameDevPoints(game, newTotalWeeks, doneFraction);

            if (log != null)
            {
                log.LogInfo(string.Format(
                    "[Timeline Upgrade] Studio '{0}' | Game '{1}' | Stars {2}->{3} Speed {4}->{5}. " +
                    "{6}" +
                    "New: total={7}w -> rem={8:F1}w ({9:P0} done)",
                    studio.GetName(), game.GetNameWithTag(),
                    preStars, currentStars, preSpeed, currentSpeed,
                    slotDataWasSentinel
                        ? "[sentinel slot data \u2014 applied fresh total] "
                        : string.Format("Prev: rem={0:F1}w / tot={1:F1}w ({2:P0} done). ",
                            wRemainingPrev, wTotalPrev, doneFraction),
                    newTotalWeeks, newRemaining, doneFraction));
            }
        }

        // Reset tracking state
        preUpgradeStars = -1;
        preUpgradeSpeed = -1;
        preUpgradeIsStudioLevel = false;
        preUpgradeSlotIndex = -1;
    }

    [HarmonyPatch(typeof(Menu_W_FirmaAufwerten), "BUTTON_Yes")]
    private static class Menu_W_FirmaAufwerten_BUTTON_Yes_Patch
    {
        [HarmonyPrefix]
        static void Prefix(Menu_W_FirmaAufwerten __instance)
        {
            try
            {
                FieldInfo psField = AccessTools.Field(typeof(Menu_W_FirmaAufwerten), "pS_");
                publisherScript studio = psField?.GetValue(__instance) as publisherScript;
                if (studio != null)
                {
                    preUpgradeStars = studio.GetStarsAmount();
                    preUpgradeSpeed = studio.developmentSpeed;
                    preUpgradeIsStudioLevel = true;
                }
            }
            catch (Exception ex)
            {
                if (log != null) log.LogError("Error in Menu_W_FirmaAufwerten.BUTTON_Yes Prefix: " + ex);
            }
        }

        [HarmonyPostfix]
        static void Postfix(Menu_W_FirmaAufwerten __instance)
        {
            try
            {
                FieldInfo psField = AccessTools.Field(typeof(Menu_W_FirmaAufwerten), "pS_");
                publisherScript studio = psField?.GetValue(__instance) as publisherScript;
                if (studio != null)
                {
                    RecalculateActiveProjectTimeline(studio);
                    RefreshSubsidiaryDevUI(studio);
                }
            }
            catch (Exception ex)
            {
                if (log != null) log.LogError("Error in Menu_W_FirmaAufwerten.BUTTON_Yes Postfix: " + ex);
            }
        }
    }

    [HarmonyPatch(typeof(Menu_W_FirmaAufwertenPublisher), "BUTTON_Yes")]
    private static class Menu_W_FirmaAufwertenPublisher_BUTTON_Yes_Patch
    {
        [HarmonyPrefix]
        static void Prefix(Menu_W_FirmaAufwertenPublisher __instance)
        {
            try
            {
                FieldInfo psField = AccessTools.Field(typeof(Menu_W_FirmaAufwertenPublisher), "pS_");
                publisherScript studio = psField?.GetValue(__instance) as publisherScript;
                if (studio != null)
                {
                    preUpgradeStars = studio.GetStarsAmount();
                    preUpgradeSpeed = studio.developmentSpeed;
                    preUpgradeIsStudioLevel = true;
                }
            }
            catch (Exception ex)
            {
                if (log != null) log.LogError("Error in Menu_W_FirmaAufwertenPublisher.BUTTON_Yes Prefix: " + ex);
            }
        }

        [HarmonyPostfix]
        static void Postfix(Menu_W_FirmaAufwertenPublisher __instance)
        {
            try
            {
                FieldInfo psField = AccessTools.Field(typeof(Menu_W_FirmaAufwertenPublisher), "pS_");
                publisherScript studio = psField?.GetValue(__instance) as publisherScript;
                if (studio != null)
                {
                    RecalculateActiveProjectTimeline(studio);
                    RefreshSubsidiaryDevUI(studio);
                }
            }
            catch (Exception ex)
            {
                if (log != null) log.LogError("Error in Menu_W_FirmaAufwertenPublisher.BUTTON_Yes Postfix: " + ex);
            }
        }
    }

    [HarmonyPatch(typeof(Menu_W_FirmaAufwertenDeveloper), "BUTTON_Yes")]
    private static class Menu_W_FirmaAufwertenDeveloper_BUTTON_Yes_Patch
    {
        [HarmonyPrefix]
        static void Prefix(Menu_W_FirmaAufwertenDeveloper __instance)
        {
            try
            {
                FieldInfo psField = AccessTools.Field(typeof(Menu_W_FirmaAufwertenDeveloper), "pS_");
                publisherScript studio = psField?.GetValue(__instance) as publisherScript;
                if (studio != null)
                {
                    preUpgradeStars = studio.GetStarsAmount();
                    preUpgradeSpeed = studio.developmentSpeed;
                    preUpgradeIsStudioLevel = true;
                }
            }
            catch (Exception ex)
            {
                if (log != null) log.LogError("Error in Menu_W_FirmaAufwertenDeveloper.BUTTON_Yes Prefix: " + ex);
            }
        }

        [HarmonyPostfix]
        static void Postfix(Menu_W_FirmaAufwertenDeveloper __instance)
        {
            try
            {
                FieldInfo psField = AccessTools.Field(typeof(Menu_W_FirmaAufwertenDeveloper), "pS_");
                publisherScript studio = psField?.GetValue(__instance) as publisherScript;
                if (studio != null)
                {
                    RecalculateActiveProjectTimeline(studio);
                    RefreshSubsidiaryDevUI(studio);
                }
            }
            catch (Exception ex)
            {
                if (log != null) log.LogError("Error in Menu_W_FirmaAufwertenDeveloper.BUTTON_Yes Postfix: " + ex);
            }
        }
    }
}

