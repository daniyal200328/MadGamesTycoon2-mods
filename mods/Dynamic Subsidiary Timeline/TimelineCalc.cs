using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public partial class DynamicSubsidiaryTimelinePlugin
{
    // ══════════════════════════════════════════════════
    //  Shared Timeline Calculation
    // ══════════════════════════════════════════════════
    internal static int CalcTimeline(publisherScript studio, gameScript game, out string debugInfo, bool applyVariance = true)
    {
        debugInfo = null;
        if (studio == null || game == null) return 1;

        bool isOrganic = IsOrganicStudio(studio);
        bool isAcquired = IsAcquiredSubsidiary(studio);
        if (!isOrganic && !isAcquired) return Mathf.RoundToInt(studio.newGameInWeeksORG > 0 ? studio.newGameInWeeksORG : 1);

        int gameSize = Mathf.Clamp(game.gameSize, 0, 5);
        int platformCount = CountPlatforms(game);
        int starsCount = studio.GetStarsAmount();
        int speedLevel = studio.developmentSpeed;

        GetSlotSpecificStats(studio, game, ref starsCount, ref speedLevel);

        string trendState = GetTrendState(studio);

        float starMult = CalcStarMultiplier(starsCount, gameSize);
        float speedMult = CalcSpeedMultiplier(speedLevel, isOrganic);
        float platformMult = CalcPlatformMultiplier(platformCount);
        float typeMult = CalcProjectTypeMultiplier(game);
        float trendMult = CalcTrendMultiplier(trendState);

        float baseWeeks = cfgBaseMidpointWeeks[gameSize].Value;
        float finalWeeks = baseWeeks * starMult * speedMult * platformMult * typeMult * trendMult;

        int weeksInt = Mathf.Clamp(
            Mathf.RoundToInt(finalWeeks),
            cfgHardFloorWeeks[gameSize].Value,
            cfgHardCeilWeeks[gameSize].Value);

        if (applyVariance && cfgRandomVariance.Value)
        {
            float variancePercent = UnityEngine.Random.Range(-7f, 7f);
            int varianceWeeks = Mathf.RoundToInt(weeksInt * (variancePercent / 100f));
            weeksInt += varianceWeeks;
            if (weeksInt < 1) weeksInt = 1;
            weeksInt = Mathf.Clamp(weeksInt, cfgHardFloorWeeks[gameSize].Value, cfgHardCeilWeeks[gameSize].Value);
        }

        float mtMult = CalcMultiTeamPenaltyMultiplier(studio, game);
        if (mtMult > 1.0001f)
        {
            int mtAdjusted = Mathf.RoundToInt(weeksInt * mtMult);
            weeksInt = mtAdjusted;
            if (weeksInt < 1) weeksInt = 1;
        }

        if (cfgLogCalc.Value && log != null)
        {
            string projectTypeLabel = GetProjectTypeLabel(game);
            debugInfo = string.Format(
                "Studio: '{0}' | Game: '{1}' (Size={2}, Type={3}) | " +
                "Stars={4}/5 Speed={5} Plats={6} Trend={7} | " +
                "Base={8:F1}w x*={9:F2} s*={10:F2} p*={11:F2} t*={12:F2} g*={13:F2} => Final={14}w",
                studio.GetName(), game.GetNameWithTag(), SizeLabels[gameSize], projectTypeLabel,
                starsCount, speedLevel, platformCount, trendState,
                baseWeeks, starMult, speedMult, platformMult, typeMult, trendMult,
                weeksInt);
        }

        return weeksInt;
    }

    internal static void ScaleSubsidiaryTimeline(publisherScript studio, gameScript game, int weeksInt)
    {
        if (studio == null || game == null) return;
        float devMult = GetDevDurationMultiplier(studio);
        weeksInt = Mathf.RoundToInt(weeksInt * devMult);
        if (weeksInt < 1) weeksInt = 1;

        GetGameWeeks(studio, game, out float wRemainingPrev, out float wOriginalPrev);

        if (wOriginalPrev > 0f && wRemainingPrev > 0f && wOriginalPrev < 9990f && wRemainingPrev < 9990f)
        {
            float ratio = Mathf.Clamp01(wRemainingPrev / wOriginalPrev);
            float wRemainingNew = (float)(ratio * weeksInt);
            if (wRemainingNew < 1f && ratio > 0.0) wRemainingNew = 1f;
            SetGameWeeks(studio, game, wRemainingNew, weeksInt);
        }
        else
        {
            SetGameWeeks(studio, game, weeksInt, weeksInt);
        }
    }

    internal static gameScript FindSlotGameClosestToRelease(publisherScript studio, out float remainingWeeks, out float progressFraction)
    {
        remainingWeeks = 999999f;
        progressFraction = 0f;
        gameScript closestGame = null;
        if (studio == null) return null;

        if (IsTeamSlotsLoaded())
        {
            try
            {
                var slotData = SubsidiaryTeamsPlugin.GetStudioSlotData(studio.myID);
                if (slotData != null && slotData.slots != null)
                {
                    var gamesScript = studio.games_ ?? GetGamesScript();
                    for (int i = 0; i < slotData.slots.Length; i++)
                    {
                        var slot = slotData.slots[i];
                        if (slot != null && slot.gameID != -1)
                        {
                            float acceleration = GetAccelerationFactor(studio, i);
                            float calendarWeeks = slot.remainingWeeks / acceleration;
                            if (calendarWeeks < remainingWeeks)
                            {
                                var game = FindGameByIDInGlobal(gamesScript, slot.gameID);
                                if (game != null)
                                {
                                    remainingWeeks = calendarWeeks;
                                    closestGame = game;
                                    progressFraction = (slot.totalWeeks > 0f) ? Mathf.Clamp01(1f - (slot.remainingWeeks / slot.totalWeeks)) : 0f;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (log != null) log.LogWarning("[DynamicTimeline] FindSlotGameClosestToRelease failed: " + ex.Message);
            }
        }

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

    public static float GetAccelerationFactor(publisherScript studio, int slotIdx)
    {
        if (studio == null || !IsTeamSlotsLoaded()) return 1f;
        try
        {
            var slotData = SubsidiaryTeamsPlugin.GetStudioSlotData(studio.myID);
            if (slotData == null || slotData.slots == null || slotIdx < 0 || slotIdx >= slotData.slots.Length) return 1f;
            var primarySlot = slotData.slots[slotIdx];
            if (primarySlot == null || primarySlot.gameID == -1) return 1f;

            int gameSize = 2;
            try
            {
                var gs = studio.games_ ?? UnityEngine.Object.FindObjectOfType<games>();
                if (gs != null)
                {
                    var g = FindGameByIDInGlobal(gs, primarySlot.gameID);
                    if (g != null) gameSize = Mathf.Clamp(g.gameSize, 0, 5);
                }
            }
            catch { }

            bool isOrganic = IsOrganicStudio(studio);
            int primaryStars = (slotIdx == 0) ? studio.GetStarsAmount() : primarySlot.stars;
            int primarySpeed = (slotIdx == 0) ? studio.developmentSpeed : primarySlot.speed;
            float pStarMult = CalcStarMultiplier(primaryStars, gameSize);
            float pSpeedMult = CalcSpeedMultiplier(primarySpeed, isOrganic);
            float wPrimary = 1.0f / Mathf.Max(0.001f, pStarMult * pSpeedMult);

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
                    if (IsTeamSlotsLoaded())
                    {
                        hSpeedMult = SubsidiaryTeamsPlugin.ApplyTagHelperSpeedModifiers(hSpeedMult, helperSlot);
                    }
                    wHelpersSum += 1.0f / Mathf.Max(0.001f, hStarMult * hSpeedMult);
                }
            }
            return 1.0f + 0.75f * (wHelpersSum / wPrimary);
        }
        catch (Exception ex)
        {
            if (log != null) log.LogWarning("[DynamicTimeline] GetAccelerationFactor failed: " + ex);
            return 1f;
        }
    }

    public static void RecalculateActiveProjectTimeline(publisherScript studio)
    {
        if (studio == null || !cfgEnable.Value) return;
        bool isOrganic = IsOrganicStudio(studio);
        bool isAcquired = IsAcquiredSubsidiary(studio);
        if (!isOrganic && !isAcquired) return;
        if (isOrganic && !cfgApplyOrganic.Value) return;
        if (isAcquired && !cfgApplyAcquired.Value) return;

        try
        {
            var activeGames = new List<gameScript>();
            try
            {
                if (IsTeamSlotsLoaded())
                {
                    var slotData = SubsidiaryTeamsPlugin.GetStudioSlotData(studio.myID);
                    if (slotData != null && slotData.slots != null)
                    {
                        var gamesScript = studio.games_ ?? UnityEngine.Object.FindObjectOfType<games>();
                        for (int si = 0; si < slotData.slots.Length; si++)
                        {
                            var slot = slotData.slots[si];
                            if (slot != null && slot.gameID != -1)
                            {
                                var slotGame = FindGameByIDInGlobal(gamesScript, slot.gameID);
                                if (slotGame != null && slotGame.inDevelopment)
                                    activeGames.Add(slotGame);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (log != null) log.LogWarning("[DynamicTimeline] RecalculateActiveProjectTimeline slot lookup: " + ex.Message);
            }

            if (activeGames.Count == 0)
            {
                var game = SafeFindGameInDevelopment(studio);
                if (game != null) activeGames.Add(game);
            }

            foreach (var game in activeGames)
            {
                if (game == null || !game.inDevelopment) continue;
                GetGameWeeks(studio, game, out float wRemainingPrev, out float wTotalPrev);
                if (wTotalPrev <= 0f) continue;

                bool slotDataWasSentinel = (wTotalPrev >= 9990f || wRemainingPrev >= 9990f);
                if (slotDataWasSentinel)
                {
                    wRemainingPrev = 0f;
                    wTotalPrev = 0f;
                }

                int gameSize = Mathf.Clamp(game.gameSize, 0, 5);
                int platformCount = CountPlatforms(game);
                int slotIdx = GetGameSlotIndex(studio, game);

                if (preUpgradeStars >= 0)
                {
                    if (preUpgradeIsStudioLevel)
                    {
                        if (slotIdx != 0)
                            continue; // Studio upgrades only affect slot 0 in real-time
                    }
                    else
                    {
                        if (preUpgradeSlotIndex >= 0 && slotIdx != preUpgradeSlotIndex)
                            continue; // Team upgrades only affect that specific slot in real-time
                    }
                }

                int preStars;
                int preSpeed;
                bool callerProvidedPreValues = (preUpgradeStars >= 0);

                if (callerProvidedPreValues)
                {
                    if (preUpgradeIsStudioLevel && slotIdx > 0)
                    {
                        preStars = preUpgradeStars;
                        preSpeed = preUpgradeSpeed >= 0 ? preUpgradeSpeed : studio.developmentSpeed;
                        GetSlotSpecificStats(studio, game, ref preStars, ref preSpeed);
                    }
                    else
                    {
                        preStars = preUpgradeStars;
                        preSpeed = preUpgradeSpeed >= 0 ? preUpgradeSpeed : studio.developmentSpeed;
                    }
                }
                else
                {
                    preStars = studio.GetStarsAmount();
                    preSpeed = studio.developmentSpeed;
                    GetSlotSpecificStats(studio, game, ref preStars, ref preSpeed);
                }

                string trendState = GetTrendState(studio);
                float preStarMult = CalcStarMultiplier(preStars, gameSize);
                float preSpeedMult = CalcSpeedMultiplier(preSpeed, isOrganic);
                float platformMult = CalcPlatformMultiplier(platformCount);
                float typeMult = CalcProjectTypeMultiplier(game);
                float trendMult = CalcTrendMultiplier(trendState);

                float baseWeeks = cfgBaseMidpointWeeks[gameSize].Value;
                float preFinalWeeks = baseWeeks * preStarMult * preSpeedMult * platformMult * typeMult * trendMult;

                int preUpgradeValue = Mathf.Clamp(
                    Mathf.RoundToInt(preFinalWeeks),
                    cfgHardFloorWeeks[gameSize].Value,
                    cfgHardCeilWeeks[gameSize].Value);

                float mtMult = CalcMultiTeamPenaltyMultiplier(studio, game);
                if (mtMult > 1.0001f)
                {
                    preUpgradeValue = Mathf.RoundToInt(preUpgradeValue * mtMult);
                    if (preUpgradeValue < 1) preUpgradeValue = 1;
                }

                float devMult = GetDevDurationMultiplier(studio);
                preUpgradeValue = Mathf.RoundToInt(preUpgradeValue * devMult);
                if (preUpgradeValue < 1) preUpgradeValue = 1;

                int currentStars = studio.GetStarsAmount();
                int currentSpeed = studio.developmentSpeed;
                if (slotIdx != 0)
                    GetSlotSpecificStats(studio, game, ref currentStars, ref currentSpeed);

                float currentStarMult = CalcStarMultiplier(currentStars, gameSize);
                float currentSpeedMult = CalcSpeedMultiplier(currentSpeed, isOrganic);
                float newFinalWeeks = baseWeeks * currentStarMult * currentSpeedMult * platformMult * typeMult * trendMult;

                int newClampedWeeks = Mathf.Clamp(
                    Mathf.RoundToInt(newFinalWeeks),
                    cfgHardFloorWeeks[gameSize].Value,
                    cfgHardCeilWeeks[gameSize].Value);

                if (mtMult > 1.0001f)
                {
                    newClampedWeeks = Mathf.RoundToInt(newClampedWeeks * mtMult);
                    if (newClampedWeeks < 1) newClampedWeeks = 1;
                }

                newClampedWeeks = Mathf.RoundToInt(newClampedWeeks * devMult);
                if (newClampedWeeks < 1) newClampedWeeks = 1;

                int newTotalWeeks;
                float doneFraction;

                if (slotDataWasSentinel || wTotalPrev <= 0f)
                {
                    newTotalWeeks = newClampedWeeks;
                    doneFraction = 0f;
                }
                else
                {
                    float originalVarianceRatio = wTotalPrev / (float)preUpgradeValue;
                    newTotalWeeks = Mathf.RoundToInt(newClampedWeeks * originalVarianceRatio);
                    if (newTotalWeeks < 1) newTotalWeeks = 1;
                    doneFraction = 1.0f - Mathf.Clamp01(wRemainingPrev / wTotalPrev);
                }

                float newRemaining = newTotalWeeks * (1.0f - doneFraction);
                if (newRemaining < 1f && doneFraction < 1.0f) newRemaining = 1f;

                SetGameWeeks(studio, game, newRemaining, (float)newTotalWeeks);
                RecalculateGameDevPoints(game, newTotalWeeks, doneFraction);

                if (log != null)
                {
                    log.LogInfo(string.Format(
                        "[DynamicTimeline] Studio '{0}' | Game '{1}' | Stars {2}->{3} Speed {4}->{5}. " +
                        "New: total={6}w -> rem={7:F1}w ({8:P0} done)",
                        studio.GetName(), game.GetNameWithTag(),
                        preStars, currentStars, preSpeed, currentSpeed,
                        newTotalWeeks, newRemaining, doneFraction));
                }
            }
        }
        catch (Exception ex)
        {
            if (log != null) log.LogError("[DynamicTimeline] RecalculateActiveProjectTimeline error: " + ex.Message);
        }
        finally
        {
            preUpgradeStars = -1;
            preUpgradeSpeed = -1;
            preUpgradeIsStudioLevel = false;
            preUpgradeSlotIndex = -1;
        }
    }

    // ══════════════════════════════════════════════════
    //  Multiplier Formulas
    // ══════════════════════════════════════════════════

    internal static float CalcStarMultiplier(int starsCount, int gameSize)
    {
        int starsClamped = Mathf.Clamp(starsCount, 0, 5);
        int gap = 5 - starsClamped;
        float[] starWeights = { 0.20f, 0.30f, 0.40f, 0.50f, 0.60f, 0.70f };
        float weight = starWeights[Mathf.Clamp(gameSize, 0, 5)];
        return 1.0f + gap * weight;
    }

    internal static float CalcSpeedMultiplier(int speedLevel, bool isOrganic)
    {
        if (isOrganic)
        {
            int level = Mathf.Clamp(speedLevel, 0, 10);
            return 1.0556f - 0.0556f * level;
        }
        else
        {
            int level = Mathf.Clamp(speedLevel, 0, 4);
            return 1.1667f - 0.1667f * level;
        }
    }

    internal static float CalcPlatformMultiplier(int platformCount)
    {
        int extra = Mathf.Max(0, platformCount - 1);
        return 1.0f + extra * 0.10f;
    }

    internal static float CalcProjectTypeMultiplier(gameScript game)
    {
        if (game == null) return 1.15f;
        float baseMult = 1.15f;

        if (game.typ_bundle || game.typ_budget || game.typ_bundleAddon || game.typ_goty)
            baseMult = 0.10f;
        else if (game.portID != -1)
            baseMult = 0.25f;
        else if (game.typ_addon)
            baseMult = 0.30f;
        else if (game.typ_mmoaddon)
            baseMult = 0.35f;
        else if (game.typ_addonStandalone)
            baseMult = 0.40f;
        else if (game.typ_remaster)
            baseMult = 0.45f;
        else if (game.typ_contractGame)
            baseMult = 0.70f;
        else if (game.typ_nachfolger)
            baseMult = 0.85f;
        else if (game.typ_spinoff)
            baseMult = 0.90f;
        else if (game.typ_standard)
            baseMult = 1.15f;

        if (!game.typ_bundle && !game.typ_budget && !game.typ_bundleAddon && !game.typ_goty && game.portID == -1 && !game.typ_contractGame)
        {
            if (game.gameTyp == 1) return baseMult * 1.35f;
            if (game.gameTyp == 2) return baseMult * 1.25f;
        }
        return baseMult;
    }

    internal static string GetProjectTypeLabel(gameScript game)
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

    internal static float CalcTrendMultiplier(string trendState)
    {
        if (!cfgTrendIntegration.Value) return 1.0f;
        switch (trendState)
        {
            case "In Crisis": return 1.35f;
            case "Declining": return 1.15f;
            case "Stable": return 1.00f;
            case "Rising": return 0.92f;
            case "Commercial Powerhouse": return 0.85f;
            default: return 1.00f;
        }
    }

    internal static int CountPlatforms(gameScript game)
    {
        if (game.gamePlatform == null || game.gamePlatform.Length == 0) return 1;
        int count = 0;
        foreach (int p in game.gamePlatform)
            if (p >= 0) count++;
        return Mathf.Max(1, count);
    }
}
