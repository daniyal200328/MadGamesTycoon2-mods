using System;
using UnityEngine;

public partial class DynamicSubsidiaryTimelinePlugin
{
    public static long CalculateSingleSlotUpkeep(publisherScript studio, int slotIdx)
    {
        if (studio == null) return 0L;
        bool isOrganic = IsOrganicStudio(studio);
        long studioValue = isOrganic ? GetOrganicUpkeepBaseValue(studio) : SafeGetFirmenwert(studio);
        if (studioValue < 0) studioValue = 0;
        if (studioValue > 100000000L) studioValue = 100000000L;

        var main = GetMainScript();
        int year = main != null ? main.year : 2020;
        double baseInflation = cfgBaseInflationRate?.Value ?? 2.0f;
        double inflationRate = (baseInflation / 100.0) * (1.0 + 1.5 * (studioValue / 100000000.0));
        double yearMult = Math.Max(1.0, 1.0 + inflationRate * (year - 1976));
        double vBase = 5000.0 * yearMult;
        double baseUpkeep = (studioValue * 0.001) + vBase;

        double diffMult = main != null ? (0.6 + main.difficulty * 0.20) : 1.0;
        double adjustedBase = baseUpkeep * diffMult;

        try
        {
            if (IsTeamSlotsLoaded())
            {
                var slotData = SubsidiaryTeamsPlugin.GetStudioSlotData(studio.myID);
                if (slotData != null && slotData.slots != null && slotIdx >= 0 && slotIdx < slotData.slots.Length)
                {
                    var slot = slotData.slots[slotIdx];
                    if (slot != null && slot.isUnlocked)
                    {
                        var gamesScript = studio.games_ ?? GetGamesScript();
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
                                var targetGame = FindGameByIDInGlobal(gamesScript, targetSlot.gameID);
                                if (targetGame != null)
                                {
                                    int gs = Mathf.Clamp(targetGame.gameSize, 0, 5);
                                    sizeMult = 1.0 + 0.5 * gs + 0.2 * (gs * gs);
                                }
                            }
                            helperDiscount = 0.85;
                        }
                        else if (slot.gameID != -1)
                        {
                            var game = FindGameByIDInGlobal(gamesScript, slot.gameID);
                            if (game != null)
                            {
                                int gs = Mathf.Clamp(game.gameSize, 0, 5);
                                sizeMult = 1.0 + 0.5 * gs + 0.2 * (gs * gs);
                            }
                        }
                        else
                        {
                            sizeMult = 0.4;
                        }

                        int unlockedCount = 0;
                        for (int i = 0; i < slotData.slots.Length; i++)
                        {
                            if (slotData.slots[i] != null && slotData.slots[i].isUnlocked) unlockedCount++;
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
            if (log != null) log.LogWarning("[DynamicTimeline] CalculateSingleSlotUpkeep failed: " + ex);
        }
        return 0L;
    }

    public static long CalculateDynamicVerwaltungskosten(publisherScript studio)
    {
        if (studio == null || studio.tf_geschlossen) return 0L;

        bool isOrganic = IsOrganicStudio(studio);
        var main = GetMainScript();
        int year = main != null ? main.year : 2020;
        double baseInflation = cfgBaseInflationRate?.Value ?? 2.0f;
        long upkeepCap = cfgMaxUpkeepCap?.Value ?? 17500000L;

        long studioValue = isOrganic ? GetOrganicUpkeepBaseValue(studio) : SafeGetFirmenwert(studio);
        if (studioValue < 0) studioValue = 0;
        if (studioValue > 100000000L) studioValue = 100000000L;

        double inflationRate = (baseInflation / 100.0) * (1.0 + 1.5 * (studioValue / 100000000.0));
        double yearMult = Math.Max(1.0, 1.0 + inflationRate * (year - 1976));
        double vBase = 5000.0 * yearMult;
        double baseUpkeep = (studioValue * 0.001) + vBase;

        double diffMult = main != null ? (0.6 + main.difficulty * 0.20) : 1.0;
        double adjustedBase = baseUpkeep * diffMult;

        long totalCost = 0L;

        if (IsTeamSlotsLoaded())
        {
            try
            {
                var slotData = SubsidiaryTeamsPlugin.GetStudioSlotData(studio.myID);
                if (slotData != null && slotData.slots != null)
                {
                    var gamesScript = studio.games_ ?? GetGamesScript();
                    int unlockedCount = 0;
                    for (int i = 0; i < slotData.slots.Length; i++)
                    {
                        if (slotData.slots[i] != null && slotData.slots[i].isUnlocked) unlockedCount++;
                    }

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
                        double speedMultVal = isOrganic ? (1.0 + speedLevel / 10.0) : (1.0 + speedLevel / 4.0);
                        double sizeMult = 1.0;
                        double helperDiscount = 1.0;

                        if (slot.helpingSlotIndex >= 0 && slot.helpingSlotIndex < slotData.slots.Length)
                        {
                            int targetSlotIdx = slot.helpingSlotIndex;
                            var targetSlot = slotData.slots[targetSlotIdx];
                            if (targetSlot != null && targetSlot.gameID != -1)
                            {
                                var targetGame = FindGameByIDInGlobal(gamesScript, targetSlot.gameID);
                                if (targetGame != null)
                                {
                                    int gs = Mathf.Clamp(targetGame.gameSize, 0, 5);
                                    sizeMult = 1.0 + 0.5 * gs + 0.2 * (gs * gs);
                                }
                            }
                            helperDiscount = 0.85;
                        }
                        else if (slot.gameID != -1)
                        {
                            var game = FindGameByIDInGlobal(gamesScript, slot.gameID);
                            if (game != null)
                            {
                                int gs = Mathf.Clamp(game.gameSize, 0, 5);
                                sizeMult = 1.0 + 0.5 * gs + 0.2 * (gs * gs);
                            }
                        }
                        else
                        {
                            sizeMult = 0.4;
                        }

                        double teamUpkeep = adjustedBase * sizeMult * ratingMult * speedMultVal * helperDiscount;
                        long teamUpkeepCost = (long)Math.Round(teamUpkeep * complexityFactor);
                        if (IsTeamSlotsLoaded())
                        {
                            teamUpkeepCost = SubsidiaryTeamsPlugin.ApplyTagUpkeepModifiers(teamUpkeepCost, slot);
                        }
                        if (teamUpkeepCost > upkeepCap) teamUpkeepCost = upkeepCap;
                        totalCost += teamUpkeepCost;
                    }
                }
            }
            catch (Exception ex)
            {
                if (log != null) log.LogWarning("[DynamicTimeline] CalculateDynamicVerwaltungskosten slots failed: " + ex);
            }
        }

        if (totalCost == 0L)
        {
            var game = SafeFindGameInDevelopment(studio);
            bool isIdle = (game == null);
            double sizeMult = isIdle ? 0.4 : (1.0 + 0.5 * game.gameSize + 0.2 * (game.gameSize * game.gameSize));
            int starsCount = studio.GetStarsAmount();
            double ratingMult = 1.0 + 0.2 * starsCount + 0.1 * (starsCount * starsCount);
            int speedLevel = studio.developmentSpeed;
            double speedMultVal = isOrganic ? (1.0 + speedLevel / 10.0) : (1.0 + speedLevel / 4.0);
            totalCost = (long)Math.Round(adjustedBase * sizeMult * ratingMult * speedMultVal);
            if (totalCost > upkeepCap) totalCost = upkeepCap;
        }

        return totalCost;
    }
}
