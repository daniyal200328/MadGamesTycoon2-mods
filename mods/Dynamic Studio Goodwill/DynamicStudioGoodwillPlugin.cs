using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

[BepInPlugin("org.bepinex.plugins.dynamicstudiogoodwill", "Dynamic Studio Goodwill", "1.0.0")]
public class DynamicStudioGoodwillPlugin : BaseUnityPlugin
{
    private const int LaunchJudgmentWeek = 4;
    private const int LongTailJudgmentWeek = 24;
    private const long VanillaFirmValueCap = 30000000L;

    private static readonly HashSet<int> processedLaunchGames = new HashSet<int>();
    private static readonly HashSet<int> processedLongTailGames = new HashSet<int>();
    private static readonly HashSet<string> processedRivalries = new HashSet<string>();
    private static readonly Dictionary<int, StudioState> states = new Dictionary<int, StudioState>();

    private static ManualLogSource log;
    private static string statePath;
    private static ConfigEntry<float> globalStrength;
    private static ConfigEntry<bool> showMajorNews;
    private static ConfigEntry<bool> adjustPlayerOwnedSubsidiaries;

    private void Awake()
    {
        log = Logger;
        statePath = Path.Combine(Paths.ConfigPath, "DynamicStudioGoodwill_State.tsv");
        globalStrength = Config.Bind("Balance", "GlobalStrength", 1.0f, "Scales all goodwill changes.");
        showMajorNews = Config.Bind("Feedback", "ShowMajorNews", false, "Shows short top-news messages for major goodwill swings.");
        adjustPlayerOwnedSubsidiaries = Config.Bind("Balance", "AdjustPlayerOwnedSubsidiaries", true, "Allows player-owned subsidiaries to gain/lose goodwill.");

        LoadState();
        new Harmony("org.bepinex.plugins.dynamicstudiogoodwill").PatchAll();
        log.LogInfo("Dynamic Studio Goodwill loaded. Studios will be judged at weeks 4 and 24 after release.");
    }

    private mainScript cachedMainScript;
    private int lastCheckedMonth = -1;
    private void Update()
    {
        if (cachedMainScript == null)
        {
            cachedMainScript = FindObjectOfType<mainScript>();
        }
        if (cachedMainScript != null && cachedMainScript.officeLoaded && cachedMainScript.month != lastCheckedMonth)
        {
            lastCheckedMonth = cachedMainScript.month;
            OnMonthChanged(cachedMainScript);
        }
    }

    private void OnMonthChanged(mainScript mS)
    {
        try
        {
            bool stateChanged = false;
            foreach (var kvp in states.ToList())
            {
                StudioState state = kvp.Value;
                if (state.Label == "In Crisis")
                {
                    state.MonthsInCrisis++;
                    stateChanged = true;
                    if (state.MonthsInCrisis >= 6)
                    {
                        publisherScript studio = FindStudioByID(mS, state.StudioId);
                        if (studio != null)
                        {
                            float oldStars = studio.stars;
                            studio.stars = Mathf.Clamp(studio.stars - 5f, 20f, 100f);
                            if (studio.stars < oldStars)
                            {
                                GUI_Main gui = FindObjectOfType<GUI_Main>();
                                if (gui != null)
                                {
                                    gui.CreateTopNewsInfo(studio.GetName() + " talent departs after 6 months in crisis! Stars decreased.");
                                }
                                log.LogInfo("[Dynamic Goodwill] " + studio.GetName() + " stars reduced from " + oldStars + " to " + studio.stars + " due to prolonged crisis.");
                            }
                        }
                    }
                }
                else
                {
                    if (state.MonthsInCrisis > 0)
                    {
                        state.MonthsInCrisis = 0;
                        stateChanged = true;
                    }
                }
            }
            if (stateChanged)
            {
                SaveState();
            }
        }
        catch (Exception ex)
        {
            log.LogWarning("Error in OnMonthChanged: " + ex);
        }
    }

    private static publisherScript FindStudioByID(mainScript mainScript, int id)
    {
        if (id < 0) return null;
        if (mainScript != null && mainScript.arrayPublisherScripts != null)
        {
            for (int i = 0; i < mainScript.arrayPublisherScripts.Length; i++)
            {
                publisherScript studio = mainScript.arrayPublisherScripts[i];
                if (studio != null && studio.myID == id)
                {
                    return studio;
                }
            }
        }
        return UnityEngine.Object.FindObjectsOfType<publisherScript>().FirstOrDefault(studio => studio != null && studio.myID == id);
    }

    public static void EvaluateGameIfReady(gameScript game)
    {
        if (game == null || !game.isOnMarket || game.releaseDate > 0 || game.reviewTotal <= 0 || game.schublade || game.auftragsspiel || game.pubAngebot)
        {
            return;
        }

        if (game.weeksOnMarket == LaunchJudgmentWeek && processedLaunchGames.Add(game.myID))
        {
            EvaluateGame(game, "launch", 1.0f);
        }
        else if (game.weeksOnMarket == LongTailJudgmentWeek && processedLongTailGames.Add(game.myID))
        {
            EvaluateGame(game, "long-tail", 0.45f);
        }
    }

    private static void EvaluateGame(gameScript game, string stage, float stageMultiplier)
    {
        publisherScript developer = FindStudio(game, game.developerID, game.devS_);
        publisherScript publisher = FindStudio(game, game.publisherID, game.pS_);

        if (developer != null)
        {
            ApplyImpact(game, developer, stage, stageMultiplier, 1.0f, "developer");
        }

        if (publisher != null && (developer == null || publisher.myID != developer.myID))
        {
            ApplyImpact(game, publisher, stage, stageMultiplier, 0.6f, "publisher");
        }
    }

    private static void ApplyImpact(gameScript game, publisherScript studio, string stage, float stageMultiplier, float roleMultiplier, string role)
    {
        if (!CanAdjustStudio(studio))
        {
            return;
        }

        StudioState state = EnsureState(studio);
        long currentBaseValue = Math.Max(100000L, studio.firmenwert);
        float stars = Mathf.Clamp(studio.stars, 0f, 100f);
        float expectedReview = 42f + stars * 0.43f;
        float reviewDelta = game.reviewTotal - expectedReview;
        float reviewImpact = Mathf.Clamp(reviewDelta * 0.13f, -5.5f, 5.5f);

        double expectedSales = GetExpectedSales(game, studio);
        double actualSales = Math.Max(0L, game.sellsTotal + game.abonnements);
        double salesRatio = actualSales / Math.Max(1.0, expectedSales);
        float salesImpact = Mathf.Clamp((float)(Math.Log(Math.Max(0.05, salesRatio), 2.0) * 1.6), -4.5f, 4.5f);

        bool commercialHit = game.commercialHit || salesRatio >= 2.25;
        bool commercialFlop = game.commercialFlop || (salesRatio <= 0.35 && game.reviewTotal < expectedReview - 5f);
        bool overhyped = IsOverhyped(studio, game, reviewDelta, salesRatio);
        bool cult = IsCultBreakout(studio, game, salesRatio);
        bool comeback = state.TrendScore <= -2.5f && reviewDelta >= 12f && salesRatio >= 0.85;

        float eventImpact = 0f;
        if (commercialHit)
        {
            eventImpact += 2.5f;
        }
        if (commercialFlop)
        {
            eventImpact -= 3.0f;
        }
        if (overhyped)
        {
            eventImpact -= 2.5f;
        }
        if (cult)
        {
            eventImpact += 2.0f;
        }
        if (comeback)
        {
            eventImpact += 2.5f;
        }

        float rivalryImpact = 0f;
        if (stage == "launch")
        {
            rivalryImpact = EvaluateRivalry(game, studio);
        }

        float typeMultiplier = GetGameTypeMultiplier(game);
        float totalPercent = (reviewImpact + salesImpact + eventImpact + rivalryImpact) * stageMultiplier * roleMultiplier * typeMultiplier * Mathf.Max(0f, globalStrength.Value);
        totalPercent = Mathf.Clamp(totalPercent, -12f, 12f);

        long newBaseValue = ApplyPercentChange(studio, state, currentBaseValue, totalPercent);
        UpdateTrend(state, totalPercent, commercialHit, commercialFlop, overhyped, cult, comeback);

        long delta = newBaseValue - currentBaseValue;
        if (delta != 0L)
        {
            studio.firmenwert = newBaseValue;
            SaveState();
            LogImpact(game, studio, stage, role, totalPercent, delta, state.Label, salesRatio, expectedReview);
            MaybeShowNews(game, studio, delta, state.Label);
        }
    }

    private static float EvaluateRivalry(gameScript game, publisherScript studio)
    {
        try
        {
            games gamesScript = UnityEngine.Object.FindObjectOfType<games>();
            if (gamesScript == null || gamesScript.arrayGamesScripts == null)
            {
                return 0f;
            }

            gameScript rivalGame = null;
            for (int i = 0; i < gamesScript.arrayGamesScripts.Length; i++)
            {
                gameScript g = gamesScript.arrayGamesScripts[i];
                if (g != null && g.myID != game.myID && g.maingenre == game.maingenre &&
                    g.date_month == game.date_month && g.date_year == game.date_year &&
                    g.developerID != game.developerID && g.isOnMarket && !g.schublade &&
                    !g.auftragsspiel && !g.pubAngebot)
                {
                    rivalGame = g;
                    break;
                }
            }

            if (rivalGame == null)
            {
                return 0f;
            }

            int minID = Math.Min(game.myID, rivalGame.myID);
            int maxID = Math.Max(game.myID, rivalGame.myID);
            string key = minID + "_" + maxID;

            bool isNew = processedRivalries.Add(key);
            
            if (game.reviewTotal > rivalGame.reviewTotal)
            {
                if (isNew)
                {
                    GUI_Main gui = UnityEngine.Object.FindObjectOfType<GUI_Main>();
                    if (gui != null)
                    {
                        gui.CreateTopNewsInfo("Rivalry! " + studio.GetName() + "'s new game outshines rival release from " + GetStudioNameByID(rivalGame.developerID) + "! Goodwill rises.");
                    }
                }
                return 4.0f;
            }
            else if (game.reviewTotal < rivalGame.reviewTotal)
            {
                if (isNew)
                {
                    GUI_Main gui = UnityEngine.Object.FindObjectOfType<GUI_Main>();
                    if (gui != null)
                    {
                        gui.CreateTopNewsInfo("Rivalry! " + studio.GetName() + "'s new game was outshone by rival release from " + GetStudioNameByID(rivalGame.developerID) + "! Goodwill falls.");
                    }
                }
                return -5.0f;
            }
        }
        catch (Exception ex)
        {
            log.LogWarning("Error in EvaluateRivalry: " + ex);
        }
        return 0f;
    }

    private static string GetStudioNameByID(int id)
    {
        mainScript mainScript = UnityEngine.Object.FindObjectOfType<mainScript>();
        if (mainScript != null && mainScript.arrayPublisherScripts != null)
        {
            for (int i = 0; i < mainScript.arrayPublisherScripts.Length; i++)
            {
                publisherScript studio = mainScript.arrayPublisherScripts[i];
                if (studio != null && studio.myID == id)
                {
                    return studio.GetName();
                }
            }
        }
        return "Unknown Studio";
    }

    private static bool CanAdjustStudio(publisherScript studio)
    {
        if (studio == null || studio.isPlayer)
        {
            return false;
        }

        if (studio.myID >= 100000)
        {
            return false;
        }

        if (studio.IsMyTochterfirma() && !adjustPlayerOwnedSubsidiaries.Value)
        {
            return false;
        }

        if (!studio.developer && !studio.publisher)
        {
            return false;
        }

        return true;
    }

    private static publisherScript FindStudio(gameScript game, int id, publisherScript cached)
    {
        if (id < 0)
        {
            return null;
        }

        if (cached != null && cached.myID == id)
        {
            return cached;
        }

        mainScript mainScript = game.mS_ != null ? game.mS_ : UnityEngine.Object.FindObjectOfType<mainScript>();
        if (mainScript != null && mainScript.arrayPublisherScripts != null)
        {
            for (int i = 0; i < mainScript.arrayPublisherScripts.Length; i++)
            {
                publisherScript studio = mainScript.arrayPublisherScripts[i];
                if (studio != null && studio.myID == id)
                {
                    return studio;
                }
            }
        }

        return UnityEngine.Object.FindObjectsOfType<publisherScript>().FirstOrDefault(studio => studio != null && studio.myID == id);
    }

    private static StudioState EnsureState(publisherScript studio)
    {
        StudioState state;
        if (states.TryGetValue(studio.myID, out state))
        {
            if (state.BaseValue <= 0L)
            {
                state.BaseValue = Math.Max(100000L, studio.firmenwert);
            }
            return state;
        }

        state = new StudioState
        {
            StudioId = studio.myID,
            BaseValue = Math.Max(100000L, studio.firmenwert),
            TrendScore = 0f,
            Label = "Stable"
        };
        states[studio.myID] = state;
        return state;
    }

    private static long ApplyPercentChange(publisherScript studio, StudioState state, long currentBaseValue, float percent)
    {
        if (Mathf.Abs(percent) < 0.05f)
        {
            return currentBaseValue;
        }

        long delta = (long)Math.Round(currentBaseValue * ((double)percent / 100.0));
        if (delta == 0L)
        {
            delta = percent > 0f ? 50000L : -50000L;
        }

        long minValue = GetMinimumBaseValue(studio, state);
        long maxValue = GetMaximumBaseValue(studio, state);
        long value = currentBaseValue + delta;
        return ClampLong(value, minValue, maxValue);
    }

    private static long GetMinimumBaseValue(publisherScript studio, StudioState state)
    {
        float floor = IsOrganicStudio(studio) || studio.IsMyTochterfirma() ? 0.5f : 0.4f;
        if (studio.stars >= 80f)
        {
            floor += 0.1f;
        }

        return Math.Max(100000L, (long)(state.BaseValue * floor));
    }

    private static long GetMaximumBaseValue(publisherScript studio, StudioState state)
    {
        if (IsOrganicStudio(studio))
        {
            return Math.Max(state.BaseValue * 4L, state.BaseValue + 50000000L);
        }

        return VanillaFirmValueCap;
    }

    private static double GetExpectedSales(gameScript game, publisherScript studio)
    {
        mainScript mainScript = game.mS_ != null ? game.mS_ : UnityEngine.Object.FindObjectOfType<mainScript>();
        int year = mainScript != null ? mainScript.year : Math.Max(1976, game.date_year);
        float yearFactor = Mathf.Clamp((year - 1975) / 20f, 0.25f, 2.5f);
        float starFactor = 0.65f + Mathf.Clamp(studio.stars, 0f, 100f) / 100f;
        float reviewFactor = Mathf.Clamp(game.reviewTotal / 70f, 0.35f, 1.5f);
        double baseValueMillions = Math.Max(1.0, GetUnpatchedBaseValue(studio) / 1000000.0);
        double sizeFactor = Math.Sqrt(baseValueMillions);

        double expected = 25000.0 * sizeFactor * yearFactor * starFactor * reviewFactor;

        if (game.handy)
        {
            expected *= 1.5;
        }
        if (game.arcade)
        {
            expected *= 0.08;
        }
        if (game.gameTyp == 1 || game.gameTyp == 2)
        {
            expected *= 0.75;
        }
        if (game.typ_addon || game.typ_addonStandalone || game.typ_mmoaddon)
        {
            expected *= 0.4;
        }
        if (game.typ_budget || game.typ_bundle || game.typ_bundleAddon || game.typ_goty)
        {
            expected *= 0.3;
        }

        return Math.Max(1000.0, expected);
    }

    private static bool IsOverhyped(publisherScript studio, gameScript game, float reviewDelta, double salesRatio)
    {
        bool largeStudio = studio.stars >= 80f || GetApproximateDisplayedValue(studio) >= 500000000L;
        return largeStudio && (reviewDelta <= -12f || (game.reviewTotal < 65 && salesRatio < 0.85));
    }

    private static bool IsCultBreakout(publisherScript studio, gameScript game, double salesRatio)
    {
        bool smallOrMidStudio = GetApproximateDisplayedValue(studio) <= 150000000L && studio.stars <= 70f;
        bool prestigeGame = game.reviewTotal >= 84 && !game.typ_bundle && !game.typ_bundleAddon && !game.typ_goty;
        return smallOrMidStudio && prestigeGame && salesRatio >= 0.35 && salesRatio <= 1.35;
    }

    private static float GetGameTypeMultiplier(gameScript game)
    {
        if (game.typ_bundle || game.typ_bundleAddon || game.typ_goty)
        {
            return 0.25f;
        }
        if (game.typ_budget)
        {
            return 0.35f;
        }
        if (game.typ_addon || game.typ_addonStandalone || game.typ_mmoaddon)
        {
            return 0.45f;
        }
        if (game.typ_remaster)
        {
            return 0.75f;
        }
        return 1.0f;
    }

    private static void UpdateTrend(StudioState state, float impact, bool hit, bool flop, bool overhyped, bool cult, bool comeback)
    {
        float trendDelta = Mathf.Clamp(impact / 2.5f, -3f, 3f);
        state.TrendScore = Mathf.Clamp(state.TrendScore * 0.82f + trendDelta, -10f, 10f);

        if (comeback)
        {
            state.Label = "Comeback";
        }
        else if (cult)
        {
            state.Label = "Cult Favorite";
        }
        else if (overhyped)
        {
            state.Label = "Overhyped";
        }
        else if (hit)
        {
            state.Label = "Commercial Powerhouse";
        }
        else if (flop && state.TrendScore <= -4f)
        {
            state.Label = "In Crisis";
        }
        else if (state.TrendScore >= 3f)
        {
            state.Label = "Rising";
        }
        else if (state.TrendScore <= -4f)
        {
            state.Label = "In Crisis";
        }
        else if (state.TrendScore <= -1.5f)
        {
            state.Label = "Declining";
        }
        else
        {
            state.Label = "Stable";
        }
    }

    private static void MaybeShowNews(gameScript game, publisherScript studio, long delta, string label)
    {
        if (!showMajorNews.Value || Math.Abs(delta) < Math.Max(1000000L, studio.firmenwert / 20L))
        {
            return;
        }

        GUI_Main gui = UnityEngine.Object.FindObjectOfType<GUI_Main>();
        if (gui == null)
        {
            return;
        }

        string direction = delta > 0L ? "rises" : "falls";
        gui.CreateTopNewsInfo(studio.GetName() + " goodwill " + direction + " after " + game.GetNameWithTag() + ". Trend: " + label + ".");
    }

    private static void LogImpact(gameScript game, publisherScript studio, string stage, string role, float percent, long delta, string label, double salesRatio, float expectedReview)
    {
        string sign = delta >= 0L ? "+" : "";
        log.LogInfo(
            "[Dynamic Goodwill] " + studio.GetName() +
            " (" + role + ", " + stage + ") " +
            sign + delta.ToString("N0", CultureInfo.InvariantCulture) +
            " base goodwill (" + percent.ToString("0.00", CultureInfo.InvariantCulture) + "%)" +
            " from '" + game.GetNameWithTag() + "'" +
            " review=" + game.reviewTotal +
            " expectedReview=" + expectedReview.ToString("0.0", CultureInfo.InvariantCulture) +
            " salesRatio=" + salesRatio.ToString("0.00", CultureInfo.InvariantCulture) +
            " trend=" + label);
    }

    private static bool IsOrganicStudio(publisherScript studio)
    {
        if (studio == null || !studio) return false;
        try
        {
            return (studio.myID >= 9000 && studio.myID < 10000) || (studio.myID >= 90000 && studio.myID < 100000);
        }
        catch
        {
            return false;
        }
    }

    private static long GetUnpatchedBaseValue(publisherScript studio)
    {
        if (studio == null)
        {
            return 100000L;
        }

        return Math.Max(100000L, studio.firmenwert);
    }

    private static long GetApproximateDisplayedValue(publisherScript studio)
    {
        if (studio == null)
        {
            return 0L;
        }

        if (IsOrganicStudio(studio))
        {
            return GetUnpatchedBaseValue(studio);
        }

        mainScript mainScript = studio.mS_ != null ? studio.mS_ : UnityEngine.Object.FindObjectOfType<mainScript>();
        long yearMultiplier = mainScript != null ? Math.Max(1L, Math.Min(50L, mainScript.year - 1975)) : 1L;
        long value = Math.Min(VanillaFirmValueCap, GetUnpatchedBaseValue(studio)) * yearMultiplier;
        if (studio.ownPlatform)
        {
            value *= 3L;
        }

        if (mainScript != null)
        {
            switch (mainScript.difficulty)
            {
                case 0:
                    value /= 5L;
                    break;
                case 1:
                    value /= 4L;
                    break;
                case 2:
                    value /= 3L;
                    break;
                case 3:
                    value /= 2L;
                    break;
                case 5:
                    value *= 2L;
                    break;
            }
        }

        return Math.Max(0L, value);
    }

    private static long ClampLong(long value, long min, long max)
    {
        if (value < min)
        {
            return min;
        }
        if (value > max)
        {
            return max;
        }
        return value;
    }

    private static void LoadState()
    {
        states.Clear();
        if (!File.Exists(statePath))
        {
            return;
        }

        string[] lines = File.ReadAllLines(statePath);
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
            {
                continue;
            }

            string[] parts = line.Split('\t');
            if (parts.Length < 4)
            {
                continue;
            }

            int id;
            long baseValue;
            float trend;
            if (!int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out id) ||
                !long.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out baseValue) ||
                !float.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out trend))
            {
                continue;
            }

            int monthsInCrisis = 0;
            if (parts.Length >= 5)
            {
                int.TryParse(parts[4], NumberStyles.Integer, CultureInfo.InvariantCulture, out monthsInCrisis);
            }

            states[id] = new StudioState
            {
                StudioId = id,
                BaseValue = Math.Max(100000L, baseValue),
                TrendScore = Mathf.Clamp(trend, -10f, 10f),
                Label = string.IsNullOrEmpty(parts[3]) ? "Stable" : parts[3],
                MonthsInCrisis = monthsInCrisis
            };
        }
    }

    private static void SaveState()
    {
        try
        {
            string dir = Path.GetDirectoryName(statePath);
            if (!string.IsNullOrEmpty(dir))
            {
                Directory.CreateDirectory(dir);
            }

            List<string> lines = new List<string>();
            lines.Add("# studioId\tbaseValue\ttrendScore\tlabel\tmonthsInCrisis");
            foreach (StudioState state in states.Values.OrderBy(s => s.StudioId))
            {
                lines.Add(
                    state.StudioId.ToString(CultureInfo.InvariantCulture) + "\t" +
                    state.BaseValue.ToString(CultureInfo.InvariantCulture) + "\t" +
                    state.TrendScore.ToString("0.###", CultureInfo.InvariantCulture) + "\t" +
                    (state.Label ?? "Stable") + "\t" +
                    state.MonthsInCrisis.ToString(CultureInfo.InvariantCulture));
            }
            File.WriteAllLines(statePath, lines.ToArray());
        }
        catch (Exception ex)
        {
            log.LogWarning("Could not save Dynamic Studio Goodwill state: " + ex.Message);
        }
    }

    private class StudioState
    {
        public int StudioId;
        public long BaseValue;
        public float TrendScore;
        public string Label;
        public int MonthsInCrisis;
    }

    [HarmonyPatch(typeof(publisherScript), "GetTooltip")]
    public static class DynamicStudioGoodwillTooltipPatch
    {
        public static void Postfix(publisherScript __instance, ref string __result)
        {
            if (__instance == null || !__instance) return;
            try
            {
                if (__instance.isPlayer)
                {
                    return;
                }

                StudioState state = EnsureState(__instance);
                if (state != null && !string.IsNullOrEmpty(state.Label))
                {
                    string color;
                    switch (state.Label)
                    {
                        case "Rising":
                            color = "#32CD32"; // Lime green
                            break;
                        case "Commercial Powerhouse":
                            color = "#00FF00"; // Bright green
                            break;
                        case "Cult Favorite":
                            color = "#00FFFF"; // Cyan
                            break;
                        case "Comeback":
                            color = "#FFD700"; // Gold
                            break;
                        case "Declining":
                            color = "#FFA500"; // Orange
                            break;
                        case "In Crisis":
                            color = "#FF0000"; // Red
                            break;
                        case "Overhyped":
                            color = "#FF4500"; // Orange-Red
                            break;
                        default:
                            color = "#808080"; // Gray for Stable
                            break;
                    }
                    if (__result == null)
                    {
                        __result = "";
                    }
                    __result += "\nTrend: <color=" + color + "><b>" + state.Label + "</b></color>";
                }
            }
            catch (Exception ex)
            {
                log.LogWarning("[Dynamic Studio Goodwill] Tooltip hook failed safely: " + ex);
            }
        }
    }
}

[HarmonyPatch(typeof(gameScript), "SellGame")]
public static class DynamicStudioGoodwillSellGamePatch
{
    public static void Postfix(gameScript __instance)
    {
        try
        {
            DynamicStudioGoodwillPlugin.EvaluateGameIfReady(__instance);
        }
        catch (Exception ex)
        {
            Debug.LogWarning("[Dynamic Studio Goodwill] SellGame hook failed safely: " + ex);
        }
    }
}
