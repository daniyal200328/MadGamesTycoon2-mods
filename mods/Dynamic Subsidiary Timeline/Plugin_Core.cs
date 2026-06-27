using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

[BepInPlugin("org.bepinex.plugins.dynamicsubsidiarytimeline", "Dynamic Subsidiary Timeline", "1.0.0")]
[BepInDependency("org.bepinex.plugins.subsidiaryteams", BepInDependency.DependencyFlags.SoftDependency)]
public partial class DynamicSubsidiaryTimelinePlugin : BaseUnityPlugin
{
    // ── Config Entries ──────────────────────────────────
    internal static ConfigEntry<bool> cfgEnable;
    internal static ConfigEntry<bool> cfgApplyOrganic;
    internal static ConfigEntry<bool> cfgApplyAcquired;
    internal static ConfigEntry<bool> cfgTrendIntegration;
    internal static ConfigEntry<bool> cfgRandomVariance;
    internal static ConfigEntry<bool> cfgLogCalc;

    internal static ConfigEntry<bool> cfgCostsEnable;
    internal static ConfigEntry<float> cfgCostsIdleDrop;
    internal static ConfigEntry<float> cfgCostsAAAAMultiplier;

    internal static ConfigEntry<bool> cfgMultiTeamPenaltyEnabled;
    internal static ConfigEntry<float> cfgMultiTeamPenaltyPercent;

    internal static ConfigEntry<float> cfgDevDurationShort;
    internal static ConfigEntry<float> cfgDevDurationBalanced;
    internal static ConfigEntry<float> cfgDevDurationGenerous;

    internal static ConfigEntry<long> cfgMaxUpkeepCap;
    internal static ConfigEntry<float> cfgBaseInflationRate;
    internal static readonly ConfigEntry<float>[] cfgBaseMidpointWeeks = new ConfigEntry<float>[6];
    internal static readonly ConfigEntry<int>[] cfgHardFloorWeeks = new ConfigEntry<int>[6];
    internal static readonly ConfigEntry<int>[] cfgHardCeilWeeks = new ConfigEntry<int>[6];

    internal static readonly string[] SizeLabels = { "B", "B+", "A", "AA", "AAA", "AAAA" };

    internal static ManualLogSource log;

    // ── Reflection Caches ──────────────────────────────
    private static bool goodwillModChecked = false;
    private static FieldInfo goodwillTrendDictField = null;
    private static Type goodwillPluginType = null;

    private static bool organicModChecked = false;
    private static MethodInfo organicSaleValueMethod = null;
    private static MethodInfo organicGetStudioDataMethod = null;
    private static Type organicPluginType = null;

    private static bool teamSlotsAvailable = false;
    private static bool teamSlotsChecked = false;
    private static Type teamSlotsType = null;

    // ── Menu Reflection Fields ─────────────────────────
    internal static readonly FieldInfo menuPSField = AccessTools.Field(typeof(Menu_Stats_Tochterfirma_Main), "pS_");
    internal static readonly FieldInfo menuMSField = AccessTools.Field(typeof(Menu_Stats_Tochterfirma_Main), "mS_");
    internal static readonly FieldInfo menuTSField = AccessTools.Field(typeof(Menu_Stats_Tochterfirma_Main), "tS_");
    internal static readonly FieldInfo menuGenresField = AccessTools.Field(typeof(Menu_Stats_Tochterfirma_Main), "genres_");
    internal static readonly FieldInfo menuGuiMainField = AccessTools.Field(typeof(Menu_Stats_Tochterfirma_Main), "guiMain_");
    internal static readonly FieldInfo menuNextGameField = AccessTools.Field(typeof(Menu_Stats_Tochterfirma_Main), "nextGame_");
    internal static readonly FieldInfo menuSfxField = AccessTools.Field(typeof(Menu_Stats_Tochterfirma_Main), "sfx_");

    internal static readonly FieldInfo setMSField = AccessTools.Field(typeof(Menu_Stats_TochterfirmaSettings), "mS_");
    internal static readonly FieldInfo setTSField = AccessTools.Field(typeof(Menu_Stats_TochterfirmaSettings), "tS_");
    internal static readonly FieldInfo setGenresField = AccessTools.Field(typeof(Menu_Stats_TochterfirmaSettings), "genres_");
    internal static readonly FieldInfo setGamesField = AccessTools.Field(typeof(Menu_Stats_TochterfirmaSettings), "games_");
    internal static readonly FieldInfo setGuiMainField = AccessTools.Field(typeof(Menu_Stats_TochterfirmaSettings), "guiMain_");
    internal static readonly FieldInfo setSfxField = AccessTools.Field(typeof(Menu_Stats_TochterfirmaSettings), "sfx_");

    internal static readonly FieldInfo listMSField = AccessTools.Field(typeof(Menu_Statistics_Tochterfirmen), "mS_");
    internal static readonly FieldInfo listGuiMainField = AccessTools.Field(typeof(Menu_Statistics_Tochterfirmen), "guiMain_");
    internal static readonly FieldInfo listSfxField = AccessTools.Field(typeof(Menu_Statistics_Tochterfirmen), "sfx_");
    internal static readonly FieldInfo listTSField = AccessTools.Field(typeof(Menu_Statistics_Tochterfirmen), "tS_");
    internal static readonly FieldInfo listGenresField = AccessTools.Field(typeof(Menu_Statistics_Tochterfirmen), "genres_");
    internal static readonly FieldInfo listSearchStringAField = AccessTools.Field(typeof(Menu_Statistics_Tochterfirmen), "searchStringA");

    // ── GUI State ──────────────────────────────────────
    private static bool showConfigWindow = false;
    private static Rect configWindowRect = new Rect(100, 100, 480, 560);
    private static Vector2 configScrollPos;
    private static string upkeepCapString = "";
    private static string inflationRateString = "";
    private static string multiTeamPenaltyPercentString = "";
    private static string devDurationShortString = "";
    private static string devDurationBalancedString = "";
    private static string devDurationGenerousString = "";
    private static string[][] timelineStrings = null;

    // ── Upgrade Tracking State ─────────────────────────
    internal static int preUpgradeStars = -1;
    internal static int preUpgradeSpeed = -1;
    internal static bool preUpgradeIsStudioLevel = false;
    internal static int preUpgradeSlotIndex = -1;

    // ── Singleton Caches ───────────────────────────────
    private static mainScript cachedMainScript;
    private static games cachedGames;

    // ══════════════════════════════════════════════════
    //  Awake
    // ══════════════════════════════════════════════════
    private void Awake()
    {
        log = Logger;

        cfgEnable = Config.Bind("General", "EnableDynamicTimeline", true,
            "Master toggle for the dynamic subsidiary development timeline system.");
        cfgApplyOrganic = Config.Bind("General", "ApplyToOrganicStudios", true,
            "Apply dynamic timeline to player-created (organic) subsidiaries.");
        cfgApplyAcquired = Config.Bind("General", "ApplyToAcquiredStudios", true,
            "Apply dynamic timeline to acquired (bought) subsidiaries.");
        cfgTrendIntegration = Config.Bind("General", "TrendIntegrationEnabled", true,
            "Read trend state from Dynamic Studio Goodwill mod (if loaded).");
        cfgRandomVariance = Config.Bind("General", "RandomVarianceEnabled", true,
            "Add +/-7 percent random variance to development duration AFTER floor/ceiling caps.");
        cfgLogCalc = Config.Bind("Debug", "LogCalculations", false,
            "Log every timeline calculation to the BepInEx console.");

        cfgCostsEnable = Config.Bind("Costs", "EnableDynamicCosts", true,
            "Enable dynamic monthly administration costs for subsidiaries.");
        cfgCostsIdleDrop = Config.Bind("Costs", "IdleCostMultiplier", 0.5f,
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

        cfgMultiTeamPenaltyEnabled = Config.Bind("MultiTeamPenalty", "Enabled", true,
            "Enable the multi-team simultaneous-development penalty.");
        cfgMultiTeamPenaltyPercent = Config.Bind("MultiTeamPenalty", "PercentPerExtraProject", 18.0f,
            "Percentage added to development duration for each extra active project (16-22 recommended).");

        cfgDevDurationShort = Config.Bind("DevDuration", "ShortMultiplier", 0.65f, "Multiplier when tf_entwicklungsdauer = 0 (Short)");
        cfgDevDurationBalanced = Config.Bind("DevDuration", "BalancedMultiplier", 0.85f, "Multiplier when tf_entwicklungsdauer = 1 (Balanced)");
        cfgDevDurationGenerous = Config.Bind("DevDuration", "GenerousMultiplier", 1.00f, "Multiplier when tf_entwicklungsdauer = 2 (Generous)");

        new Harmony("org.bepinex.plugins.dynamicsubsidiarytimeline").PatchAll();
        log.LogInfo("Dynamic Subsidiary Timeline Plugin loaded successfully.");
    }

    private void Update()
    {
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) &&
            (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) &&
            Input.GetKeyDown(KeyCode.T))
        {
            showConfigWindow = !showConfigWindow;
            if (showConfigWindow)
                InitializeUIStrings();
        }
    }

    private void OnGUI()
    {
        if (!showConfigWindow) return;
        configWindowRect = GUILayout.Window(9999, configWindowRect, DrawConfigWindow, "Dynamic Subsidiary Timeline Config");
    }

    private static void InitializeUIStrings()
    {
        upkeepCapString = cfgMaxUpkeepCap.Value.ToString();
        inflationRateString = cfgBaseInflationRate.Value.ToString(CultureInfo.InvariantCulture);
        multiTeamPenaltyPercentString = (cfgMultiTeamPenaltyPercent?.Value ?? 18f).ToString("F1", CultureInfo.InvariantCulture);
        devDurationShortString = (cfgDevDurationShort?.Value ?? 0.65f).ToString("F2", CultureInfo.InvariantCulture);
        devDurationBalancedString = (cfgDevDurationBalanced?.Value ?? 0.85f).ToString("F2", CultureInfo.InvariantCulture);
        devDurationGenerousString = (cfgDevDurationGenerous?.Value ?? 1.00f).ToString("F2", CultureInfo.InvariantCulture);
        if (timelineStrings == null)
            timelineStrings = new string[6][];
        for (int i = 0; i < 6; i++)
        {
            timelineStrings[i] = new string[3];
            timelineStrings[i][0] = cfgBaseMidpointWeeks[i].Value.ToString(CultureInfo.InvariantCulture);
            timelineStrings[i][1] = cfgHardFloorWeeks[i].Value.ToString();
            timelineStrings[i][2] = cfgHardCeilWeeks[i].Value.ToString();
        }
    }

    private void DrawConfigWindow(int windowID)
    {
        configScrollPos = GUILayout.BeginScrollView(configScrollPos, GUILayout.Width(460), GUILayout.Height(540));
        GUILayout.BeginVertical();

        GUILayout.Label("<b>Timeline & Upkeep Settings</b>", GUILayout.ExpandWidth(true));
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Max Upkeep Cap ($):", GUILayout.Width(170));
        upkeepCapString = GUILayout.TextField(upkeepCapString, GUILayout.Width(130));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Base Inflation Rate (%):", GUILayout.Width(170));
        inflationRateString = GUILayout.TextField(inflationRateString, GUILayout.Width(130));
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.Label("<b>Multi-Team Penalty</b> (overhead when multiple teams run projects in parallel)");
        GUILayout.BeginHorizontal();
        bool mtEnabled = cfgMultiTeamPenaltyEnabled?.Value ?? true;
        bool newMtEnabled = GUILayout.Toggle(mtEnabled, "  Enable Penalty", GUILayout.Width(140));
        if (newMtEnabled != mtEnabled && cfgMultiTeamPenaltyEnabled != null)
            cfgMultiTeamPenaltyEnabled.Value = newMtEnabled;
        GUILayout.Label("Percent per extra project (%):", GUILayout.Width(200));
        multiTeamPenaltyPercentString = GUILayout.TextField(multiTeamPenaltyPercentString, GUILayout.Width(80));
        GUILayout.EndHorizontal();
        if (cfgMultiTeamPenaltyEnabled != null && cfgMultiTeamPenaltyEnabled.Value && cfgMultiTeamPenaltyPercent != null)
        {
            float pct = cfgMultiTeamPenaltyPercent.Value;
            GUILayout.Label(string.Format("  <color=#cccccc>1 active = 1.00x | 2 active = {0:F2}x | 3 active = {1:F2}x</color>",
                1f + (pct / 100f), 1f + 2f * (pct / 100f)));
        }
        else
            GUILayout.Label("  <color=#cccccc>(disabled - no penalty applied)</color>");
        GUILayout.Space(10);

        GUILayout.Label("<b>Dev Duration Multipliers</b> (applied after DST formula)");
        GUILayout.BeginHorizontal();
        GUILayout.Label("Short (x):", GUILayout.Width(170));
        devDurationShortString = GUILayout.TextField(devDurationShortString, GUILayout.Width(80));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Balanced (x):", GUILayout.Width(170));
        devDurationBalancedString = GUILayout.TextField(devDurationBalancedString, GUILayout.Width(80));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Generous (x):", GUILayout.Width(170));
        devDurationGenerousString = GUILayout.TextField(devDurationGenerousString, GUILayout.Width(80));
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

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
                timelineStrings[i][1] = GUILayout.TextField(timelineStrings[i][1], GUILayout.Width(100));
                timelineStrings[i][0] = GUILayout.TextField(timelineStrings[i][0], GUILayout.Width(100));
                timelineStrings[i][2] = GUILayout.TextField(timelineStrings[i][2], GUILayout.Width(100));
                GUILayout.EndHorizontal();
            }
        }

        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save & Apply"))
        {
            ApplyAndSaveSettings();
            showConfigWindow = false;
        }
        if (GUILayout.Button("Cancel"))
            showConfigWindow = false;
        if (GUILayout.Button("Reset Defaults"))
        {
            ResetToDefaults();
            InitializeUIStrings();
        }
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
        GUILayout.EndScrollView();
        GUI.DragWindow();
    }

    private void ApplyAndSaveSettings()
    {
        if (long.TryParse(upkeepCapString, out long upkeepVal))
            cfgMaxUpkeepCap.Value = upkeepVal;
        if (float.TryParse(inflationRateString, NumberStyles.Float, CultureInfo.InvariantCulture, out float inflationVal))
            cfgBaseInflationRate.Value = inflationVal;
        if (cfgMultiTeamPenaltyPercent != null &&
            float.TryParse(multiTeamPenaltyPercentString, NumberStyles.Float, CultureInfo.InvariantCulture, out float mtPct))
        {
            if (mtPct < 0f) mtPct = 0f;
            if (mtPct > 200f) mtPct = 200f;
            cfgMultiTeamPenaltyPercent.Value = mtPct;
        }
        if (cfgDevDurationShort != null &&
            float.TryParse(devDurationShortString, NumberStyles.Float, CultureInfo.InvariantCulture, out float ddShort))
        {
            if (ddShort < 0.1f) ddShort = 0.1f;
            if (ddShort > 5f) ddShort = 5f;
            cfgDevDurationShort.Value = ddShort;
        }
        if (cfgDevDurationBalanced != null &&
            float.TryParse(devDurationBalancedString, NumberStyles.Float, CultureInfo.InvariantCulture, out float ddBal))
        {
            if (ddBal < 0.1f) ddBal = 0.1f;
            if (ddBal > 5f) ddBal = 5f;
            cfgDevDurationBalanced.Value = ddBal;
        }
        if (cfgDevDurationGenerous != null &&
            float.TryParse(devDurationGenerousString, NumberStyles.Float, CultureInfo.InvariantCulture, out float ddGen))
        {
            if (ddGen < 0.1f) ddGen = 0.1f;
            if (ddGen > 5f) ddGen = 5f;
            cfgDevDurationGenerous.Value = ddGen;
        }
        if (timelineStrings != null)
        {
            for (int i = 0; i < 6; i++)
            {
                if (float.TryParse(timelineStrings[i][0], NumberStyles.Float, CultureInfo.InvariantCulture, out float midpoint))
                    cfgBaseMidpointWeeks[i].Value = midpoint;
                if (int.TryParse(timelineStrings[i][1], out int floor))
                    cfgHardFloorWeeks[i].Value = floor;
                if (int.TryParse(timelineStrings[i][2], out int ceil))
                    cfgHardCeilWeeks[i].Value = ceil;
            }
        }
        Config.Save();
        if (log != null) log.LogInfo("[DynamicTimeline] Config settings applied and saved.");
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
        if (cfgDevDurationShort != null) cfgDevDurationShort.Value = 0.65f;
        if (cfgDevDurationBalanced != null) cfgDevDurationBalanced.Value = 0.85f;
        if (cfgDevDurationGenerous != null) cfgDevDurationGenerous.Value = 1.00f;
        Config.Save();
        if (log != null) log.LogInfo("[DynamicTimeline] Config settings reset to defaults.");
    }

    // ══════════════════════════════════════════════════
    //  TeamSlots Integration
    // ══════════════════════════════════════════════════
    internal static bool IsTeamSlotsLoaded()
    {
        if (teamSlotsChecked) return teamSlotsAvailable;
        teamSlotsChecked = true;
        try
        {
            teamSlotsType = Type.GetType("SubsidiaryTeamsPlugin, SubsidiaryTeams");
            teamSlotsAvailable = teamSlotsType != null;
            if (teamSlotsAvailable && log != null)
                log.LogInfo("[DynamicTimeline] Subsidiary Teams mod detected.");
            else if (log != null)
                log.LogInfo("[DynamicTimeline] Subsidiary Teams mod NOT detected.");
        }
        catch (Exception)
        {
            teamSlotsAvailable = false;
        }
        return teamSlotsAvailable;
    }

    internal static Type GetTeamSlotsType()
    {
        if (teamSlotsType == null && !teamSlotsChecked)
            IsTeamSlotsLoaded();
        return teamSlotsType;
    }

    internal static int GetGameSlotIndex(publisherScript studio, gameScript game)
    {
        if (studio == null || game == null || !IsTeamSlotsLoaded()) return -1;
        try
        {
            var slotData = SubsidiaryTeamsPlugin.GetStudioSlotData(studio.myID);
            if (slotData == null || slotData.slots == null) return -1;
            for (int i = 0; i < slotData.slots.Length; i++)
            {
                if (slotData.slots[i] != null && slotData.slots[i].gameID == game.myID)
                    return i;
            }
        }
        catch (Exception ex)
        {
            if (log != null) log.LogWarning("[DynamicTimeline] GetGameSlotIndex failed: " + ex.Message);
        }
        return -1;
    }

    internal static void GetSlotSpecificStats(publisherScript studio, gameScript game, ref int starsCount, ref int speedLevel)
    {
        if (studio == null || !IsTeamSlotsLoaded()) return;
        try
        {
            int slotIdx = -1;
            if (SubsidiaryTeamsPlugin.isCreatingAutonomousSlot)
                slotIdx = SubsidiaryTeamsPlugin.targetSlotIndex;
            else if (SubsidiaryTeamsPlugin.isCreatingPlayerSlotProject)
                slotIdx = SubsidiaryTeamsPlugin.selectedSlotIndex;
            else if (game != null)
                slotIdx = GetGameSlotIndex(studio, game);

            if (slotIdx > 0 && slotIdx < 3)
            {
                var slotData = SubsidiaryTeamsPlugin.GetStudioSlotData(studio.myID);
                if (slotData != null && slotData.slots != null && slotData.slots[slotIdx] != null)
                {
                    starsCount = slotData.slots[slotIdx].stars;
                    speedLevel = slotData.slots[slotIdx].speed;
                }
            }
        }
        catch (Exception ex)
        {
            if (log != null) log.LogWarning("[DynamicTimeline] GetSlotSpecificStats failed: " + ex.Message);
        }
    }

    internal static void RefreshSubsidiaryDevUI(publisherScript studio)
    {
        if (studio == null) return;
        if (IsTeamSlotsLoaded())
        {
            SubsidiaryTeamsPlugin.InvalidateStudioDevUI();
            if (SubsidiaryTeamsPlugin.currentSaveSlot >= 0)
                SubsidiaryTeamsPlugin.SaveState(SubsidiaryTeamsPlugin.currentSaveSlot);
        }
        try
        {
            var menu = UnityEngine.Object.FindObjectOfType<Menu_Stats_Tochterfirma_Main>();
            if (menu == null || !menu.gameObject.activeInHierarchy) return;
            var menuStudio = menuPSField?.GetValue(menu) as publisherScript;
            if (menuStudio != null && menuStudio.myID == studio.myID)
                menu.UpdateData();
        }
        catch (Exception ex)
        {
            if (log != null) log.LogWarning("[DynamicTimeline] RefreshSubsidiaryDevUI failed: " + ex.Message);
        }
    }

    internal static void GetGameWeeks(publisherScript studio, gameScript game, out float remaining, out float total)
    {
        if (studio == null)
        {
            remaining = 0f; total = 0f;
            return;
        }
        remaining = studio.newGameInWeeks;
        total = studio.newGameInWeeksORG;
        if (game == null || !IsTeamSlotsLoaded()) return;
        try
        {
            var slotData = SubsidiaryTeamsPlugin.GetStudioSlotData(studio.myID);
            if (slotData == null || slotData.slots == null)
            {
                remaining = 0f; total = 0f;
                return;
            }
            for (int i = 0; i < slotData.slots.Length; i++)
            {
                if (slotData.slots[i] != null && slotData.slots[i].gameID == game.myID)
                {
                    remaining = slotData.slots[i].remainingWeeks;
                    total = slotData.slots[i].totalWeeks;
                    return;
                }
            }
            remaining = 0f; total = 0f;
        }
        catch (Exception ex)
        {
            if (log != null) log.LogWarning("[DynamicTimeline] GetGameWeeks failed: " + ex.Message);
            remaining = 0f; total = 0f;
        }
    }

    internal static void SetGameWeeks(publisherScript studio, gameScript game, float remaining, float total)
    {
        if (studio == null) return;
        if (game != null && IsTeamSlotsLoaded())
        {
            try
            {
                var slotData = SubsidiaryTeamsPlugin.GetStudioSlotData(studio.myID);
                if (slotData != null && slotData.slots != null)
                {
                    for (int i = 0; i < slotData.slots.Length; i++)
                    {
                        if (slotData.slots[i] != null && slotData.slots[i].gameID == game.myID)
                        {
                            slotData.slots[i].remainingWeeks = remaining;
                            slotData.slots[i].totalWeeks = total;
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (log != null) log.LogWarning("[DynamicTimeline] SetGameWeeks slot write failed: " + ex.Message);
            }
        }
        studio.newGameInWeeks = Mathf.RoundToInt(remaining);
        studio.newGameInWeeksORG = Mathf.RoundToInt(total);
    }

    internal static bool IsSlotManagedByTeamSlots(publisherScript studio, gameScript game)
    {
        if (game == null || !IsTeamSlotsLoaded()) return false;
        try
        {
            var slotData = SubsidiaryTeamsPlugin.GetStudioSlotData(studio.myID);
            if (slotData == null || slotData.slots == null) return false;
            for (int i = 0; i < slotData.slots.Length; i++)
            {
                if (slotData.slots[i] != null && slotData.slots[i].gameID == game.myID)
                    return true;
            }
        }
        catch { }
        return false;
    }

    // ══════════════════════════════════════════════════
    //  Helper Methods
    // ══════════════════════════════════════════════════
    internal static gameScript FindGameByIDInGlobal(games gamesScript, int gameID)
    {
        if (gamesScript == null || gamesScript.arrayGamesScripts == null) return null;
        for (int i = 0; i < gamesScript.arrayGamesScripts.Length; i++)
        {
            if (gamesScript.arrayGamesScripts[i] != null && gamesScript.arrayGamesScripts[i].myID == gameID)
                return gamesScript.arrayGamesScripts[i];
        }
        return null;
    }

    internal static mainScript GetMainScript()
    {
        if (cachedMainScript == null)
            cachedMainScript = UnityEngine.Object.FindObjectOfType<mainScript>();
        return cachedMainScript;
    }

    internal static games GetGamesScript()
    {
        if (cachedGames == null)
            cachedGames = UnityEngine.Object.FindObjectOfType<games>();
        return cachedGames;
    }

    internal static publisherScript FindStudioByID(int studioID)
    {
        var main = GetMainScript();
        if (main != null && main.arrayPublisherScripts != null)
        {
            foreach (var p in main.arrayPublisherScripts)
            {
                if (p != null && p.myID == studioID)
                    return p;
            }
        }
        var allStudios = UnityEngine.Object.FindObjectsOfType<publisherScript>();
        if (allStudios != null)
        {
            foreach (var p in allStudios)
            {
                if (p != null && p.myID == studioID)
                    return p;
            }
        }
        return null;
    }

    internal static bool IsOrganicStudio(publisherScript studio)
    {
        if (studio == null) return false;
        try
        {
            return (studio.myID >= 9000 && studio.myID < 10000)
                || (studio.myID >= 90000 && studio.myID < 100000);
        }
        catch { return false; }
    }

    internal static bool IsAcquiredSubsidiary(publisherScript studio)
    {
        if (studio == null) return false;
        try
        {
            if (IsOrganicStudio(studio)) return false;
            return studio.IsMyTochterfirma();
        }
        catch { return false; }
    }

    internal static int CountOtherActiveProjects(publisherScript studio, gameScript game)
    {
        if (studio == null || !IsTeamSlotsLoaded()) return 0;
        try
        {
            var slotData = SubsidiaryTeamsPlugin.GetStudioSlotData(studio.myID);
            if (slotData == null || slotData.slots == null) return 0;
            int count = 0;
            for (int i = 0; i < slotData.slots.Length; i++)
            {
                var slot = slotData.slots[i];
                if (slot != null && slot.gameID != -1 && slot.isUnlocked && slot.remainingWeeks > 0f)
                {
                    if (game != null && slot.gameID == game.myID)
                        continue;
                    count++;
                }
            }
            return count;
        }
        catch (Exception ex)
        {
            if (log != null) log.LogWarning("[DynamicTimeline] CountOtherActiveProjects failed: " + ex.Message);
            return 0;
        }
    }

    internal static float GetSlotPenaltyMultiplier(publisherScript studio, gameScript game)
    {
        if (studio == null || game == null || !IsTeamSlotsLoaded()) return 1f;
        try
        {
            var slotData = SubsidiaryTeamsPlugin.GetStudioSlotData(studio.myID);
            if (slotData != null && slotData.slots != null)
            {
                for (int i = 0; i < slotData.slots.Length; i++)
                {
                    var slot = slotData.slots[i];
                    if (slot != null && slot.gameID == game.myID)
                    {
                        return slot.penaltyMultiplier > 0.01f ? slot.penaltyMultiplier : 1f;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            if (log != null) log.LogWarning("[DynamicTimeline] GetSlotPenaltyMultiplier failed: " + ex.Message);
        }
        return 1f;
    }

    internal static float CalcMultiTeamPenaltyMultiplier(publisherScript studio)
    {
        return CalcMultiTeamPenaltyMultiplier(studio, null);
    }

    internal static float CalcMultiTeamPenaltyMultiplier(publisherScript studio, gameScript game)
    {
        if (studio == null) return 1f;
        if (cfgMultiTeamPenaltyEnabled == null || !cfgMultiTeamPenaltyEnabled.Value) return 1f;
        if (cfgMultiTeamPenaltyPercent == null) return 1f;

        if (game != null)
        {
            return GetSlotPenaltyMultiplier(studio, game);
        }

        int activeCount = CountOtherActiveProjects(studio, null);
        float pct = Mathf.Clamp(cfgMultiTeamPenaltyPercent.Value, 0f, 200f);
        return 1.0f + activeCount * (pct / 100f);
    }

    internal static long SafeGetFirmenwert(publisherScript studio)
    {
        if (studio == null) return 0L;
        if (IsOrganicStudio(studio))
            return GetOrganicSaleValue(studio);
        long fVal = studio.firmenwert;
        if (fVal > 30000000) fVal = 30000000L;
        var main = GetMainScript();
        long years = main != null ? (main.year - 1975) : 0L;
        if (years < 0) years = 0;
        if (years > 50) years = 50L;
        long num2 = years * fVal;
        if (studio.ownPlatform) num2 *= 3;
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
        if (studio.games_ != null && studio.games_.arrayGamesScripts != null)
        {
            try
            {
                for (int i = 0; i < studio.games_.arrayGamesScripts.Length; i++)
                {
                    var g = studio.games_.arrayGamesScripts[i];
                    if (g != null && g.sellsTotal > 0 && !g.inDevelopment && !g.schublade && !g.pubAngebot && !g.auftragsspiel && g.IsMyIP(studio) && g.mainIP == g.myID)
                        num2 += g.GetIpWert();
                }
            }
            catch { }
        }
        return num2;
    }

    internal static gameScript SafeFindGameInDevelopment(publisherScript studio)
    {
        if (studio == null) return null;
        if (studio.games_ != null && studio.games_.arrayGamesScripts != null)
        {
            try
            {
                for (int i = 0; i < studio.games_.arrayGamesScripts.Length; i++)
                {
                    var g = studio.games_.arrayGamesScripts[i];
                    if (g != null && g.developerID == studio.myID && g.inDevelopment && !g.isOnMarket)
                        return g;
                }
            }
            catch { }
        }
        try
        {
            var gamesManager = GetGamesScript();
            if (gamesManager != null && gamesManager.arrayGamesScripts != null)
            {
                for (int i = 0; i < gamesManager.arrayGamesScripts.Length; i++)
                {
                    var g = gamesManager.arrayGamesScripts[i];
                    if (g != null && g.developerID == studio.myID && g.inDevelopment && !g.isOnMarket)
                        return g;
                }
            }
        }
        catch { }
        return null;
    }

    internal static float GetDevDurationMultiplier(publisherScript studio)
    {
        if (studio == null) return 1f;
        return studio.tf_entwicklungsdauer switch
        {
            0 => cfgDevDurationShort?.Value ?? 0.65f,
            1 => cfgDevDurationBalanced?.Value ?? 0.85f,
            2 => cfgDevDurationGenerous?.Value ?? 1.00f,
            _ => 1f,
        };
    }

    internal static long GetOrganicSaleValue(publisherScript studio)
    {
        try
        {
            if (!organicModChecked)
            {
                organicModChecked = true;
                foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    var t = asm.GetType("Subsidiary2Plugin") ?? asm.GetType("OrganicSubsidiaries.OrganicSubsidiariesPlugin");
                    if (t != null)
                    {
                        organicPluginType = t;
                        organicSaleValueMethod = t.GetMethod("GetOrganicSaleValue",
                            BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                        organicGetStudioDataMethod = t.GetMethod("GetStudioData",
                            BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                        break;
                    }
                }
            }
            if (organicSaleValueMethod != null)
                return (long)organicSaleValueMethod.Invoke(null, new object[] { studio });
        }
        catch { }
        return studio != null ? studio.firmenwert : 2000000L;
    }

    internal static object InvokeGetStudioData(int studioID)
    {
        try
        {
            if (organicPluginType == null && !organicModChecked)
                GetOrganicSaleValue(null);
            if (organicGetStudioDataMethod != null)
                return organicGetStudioDataMethod.Invoke(null, new object[] { studioID });
        }
        catch { }
        return null;
    }

    internal static long GetOrganicUpkeepBaseValue(publisherScript studio)
    {
        if (studio == null) return 2000000L;
        object data = InvokeGetStudioData(studio.myID);
        if (data != null)
        {
            try
            {
                var creationCostField = data.GetType().GetField("creationCost");
                var upgradeInvestmentsField = data.GetType().GetField("upgradeInvestments");
                if (creationCostField != null && upgradeInvestmentsField != null)
                {
                    long creationCost = (long)creationCostField.GetValue(data);
                    long upgradeInvestments = (long)upgradeInvestmentsField.GetValue(data);
                    return creationCost + upgradeInvestments;
                }
            }
            catch { }
        }
        return studio.firmenwert > 0 ? studio.firmenwert : 2000000L;
    }

    internal static string GetTrendState(publisherScript studio)
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
                var rawDict = goodwillTrendDictField.GetValue(null);
                var trends = rawDict as Dictionary<int, string>;
                if (trends != null && trends.TryGetValue(studio.myID, out string label))
                    return label;
            }
        }
        catch { }
        return "Stable";
    }

    internal static void RecalculateActiveProjectsAfterUpgrade(publisherScript studio, string upgradeType)
    {
        if (studio == null) return;
        try
        {
            bool recalculatedAny = false;
            if (IsTeamSlotsLoaded())
            {
                var tType = GetTeamSlotsType();
                if (tType != null)
                {
                    var getSlotDataMethod = AccessTools.Method(tType, "GetStudioSlotData");
                    if (getSlotDataMethod != null)
                    {
                        object slotData = getSlotDataMethod.Invoke(null, new object[] { studio.myID });
                        if (slotData != null)
                        {
                            var slotsField = AccessTools.Field(slotData.GetType(), "slots");
                            if (slotsField != null)
                            {
                                var slots = slotsField.GetValue(slotData) as Array;
                                if (slots != null)
                                {
                                    var gamesScript = studio.games_ ?? GetGamesScript();
                                    if (gamesScript != null)
                                    {
                                        for (int i = 0; i < slots.Length; i++)
                                        {
                                            object slot = slots.GetValue(i);
                                            if (slot == null) continue;
                                            var gameIDField = AccessTools.Field(slot.GetType(), "gameID");
                                            var remainingField = AccessTools.Field(slot.GetType(), "remainingWeeks");
                                            var totalField = AccessTools.Field(slot.GetType(), "totalWeeks");
                                            var unlockedField = AccessTools.Field(slot.GetType(), "isUnlocked");
                                            if (gameIDField == null || remainingField == null || totalField == null || unlockedField == null) continue;
                                            int gameID = (int)gameIDField.GetValue(slot);
                                            bool isUnlocked = (bool)unlockedField.GetValue(slot);
                                            if (gameID == -1 || !isUnlocked) continue;
                                            var game = FindGameByIDInGlobal(gamesScript, gameID);
                                            if (game == null || !game.inDevelopment) continue;
                                            float currentRemaining = (float)remainingField.GetValue(slot);
                                            float currentTotal = (float)totalField.GetValue(slot);
                                            float progress = (currentTotal > 0f) ? Mathf.Clamp01(1f - (currentRemaining / currentTotal)) : 0f;
                                            int newTotalWeeks = RecalculateGameDuration(studio, game);
                                            float newRemaining = newTotalWeeks * (1f - progress);
                                            if (newRemaining < 1f) newRemaining = 1f;
                                            remainingField.SetValue(slot, newRemaining);
                                            totalField.SetValue(slot, (float)newTotalWeeks);
                                            recalculatedAny = true;
                                            if (cfgLogCalc.Value && log != null)
                                                log.LogInfo($"[DynamicTimeline] Slot {i}: '{game.myName}' after {upgradeType} | {currentRemaining:F1}w -> {newRemaining:F1}w");
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
                var game = SafeFindGameInDevelopment(studio);
                if (game != null && game.inDevelopment)
                {
                    float currentRemaining = studio.newGameInWeeks;
                    float currentTotal = studio.newGameInWeeksORG;
                    float progress = (currentTotal > 0f) ? Mathf.Clamp01(1f - (currentRemaining / currentTotal)) : 0f;
                    int newTotalWeeks = RecalculateGameDuration(studio, game);
                    float newRemaining = newTotalWeeks * (1f - progress);
                    if (newRemaining < 1f) newRemaining = 1f;
                    studio.newGameInWeeks = Mathf.RoundToInt(newRemaining);
                    studio.newGameInWeeksORG = newTotalWeeks;
                    recalculatedAny = true;
                    if (cfgLogCalc.Value && log != null)
                        log.LogInfo($"[DynamicTimeline] Vanilla: '{game.myName}' after {upgradeType} | {currentRemaining}w -> {newRemaining:F1}w");
                }
            }
            if (recalculatedAny && cfgLogCalc.Value && log != null)
                log.LogInfo($"[DynamicTimeline] Recalculated projects for studio '{studio.GetName()}' after {upgradeType}");
        }
        catch (Exception ex)
        {
            if (log != null) log.LogError($"[DynamicTimeline] RecalculateActiveProjectsAfterUpgrade error: {ex}");
        }
    }

    internal static int RecalculateGameDuration(publisherScript studio, gameScript game)
    {
        if (studio == null || game == null) return 1;
        int weeks = CalcTimeline(studio, game, out _, applyVariance: false);
        float devMult = GetDevDurationMultiplier(studio);
        weeks = Mathf.RoundToInt(weeks * devMult);
        return Mathf.Max(1, weeks);
    }

    internal static void RecalculateGameDevPoints(gameScript game, float newTotalWeeks, float progressFraction)
    {
        if (game == null) return;
        try
        {
            float originalDevPointsStart = game.devPointsStart_Gesamt;
            if (originalDevPointsStart <= 0f) return;
            float oldTotalWeeks = 0f;
            if (IsTeamSlotsLoaded())
            {
                try
                {
                    var gamesManager = UnityEngine.Object.FindObjectOfType<games>();
                    if (gamesManager != null && gamesManager.arrayGamesScripts != null)
                    {
                        for (int i = 0; i < gamesManager.arrayGamesScripts.Length; i++)
                        {
                            if (gamesManager.arrayGamesScripts[i] == game)
                            {
                                int developerID = game.developerID;
                                if (developerID != -1)
                                {
                                    var slotData = SubsidiaryTeamsPlugin.GetStudioSlotData(developerID);
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
            if (oldTotalWeeks <= 0f)
                oldTotalWeeks = Mathf.Max(1f, originalDevPointsStart / 4f);
            float weekScaling = newTotalWeeks / Mathf.Max(1f, oldTotalWeeks);
            float newTotalDevPoints = originalDevPointsStart * weekScaling;
            game.devPointsStart_Gesamt = newTotalDevPoints;
            float newRemainingDevPoints = newTotalDevPoints * (1f - progressFraction);
            game.devPoints_Gesamt = Mathf.Max(1f, newRemainingDevPoints);
        }
        catch (Exception ex)
        {
            if (log != null) log.LogError($"[DynamicTimeline] RecalculateGameDevPoints error: {ex}");
        }
    }

    internal static float GetUnclampedWeeksForParams(publisherScript studio, gameScript game, int starsCount, int speedLevel)
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
        return cfgBaseMidpointWeeks[gameSize].Value * starMult * speedMult * platformMult * typeMult * trendMult;
    }
}
