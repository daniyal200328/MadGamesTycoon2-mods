using System.Collections.Generic;

public partial class SubsidiaryTeamsPlugin
{
    private static readonly int[] StudioXPRequirements = new int[] { 0, 100, 250, 500, 1000 };
    private static readonly long[] StudioUpgradeFees = new long[] { 0L, 5000000L, 10000000L, 25000000L, 50000000L };

    private static readonly int[] TeamXPRequirements = new int[] { 0, 100, 250, 500, 1000 };
    private static readonly long[] TeamUpgradeFees = new long[] { 0L, 5000000L, 10000000L, 25000000L, 50000000L };

    private class PendingAwardData
    {
        public int studioID;
        public int slotIndex;
        public int reviewTotal;
    }

    private static Dictionary<int, PendingAwardData> pendingAwards = new Dictionary<int, PendingAwardData>();

    internal static int GetStudioXPRequired(int currentStars)
    {
        if (currentStars < 1 || currentStars >= 5) return int.MaxValue;
        return StudioXPRequirements[currentStars];
    }

    internal static int GetTeamXPRequired(int currentStars)
    {
        if (currentStars < 1 || currentStars >= 5) return int.MaxValue;
        return TeamXPRequirements[currentStars];
    }

    internal static long GetStudioUpgradeFee(int currentStars)
    {
        if (currentStars < 1 || currentStars >= 5) return long.MaxValue;
        return StudioUpgradeFees[currentStars];
    }

    internal static long GetTeamUpgradeFee(int currentStars)
    {
        if (currentStars < 1 || currentStars >= 5) return long.MaxValue;
        return TeamUpgradeFees[currentStars];
    }

    internal static void RegisterPendingAward(int gameID, int studioID, int slotIndex, int reviewTotal)
    {
        pendingAwards[gameID] = new PendingAwardData
        {
            studioID = studioID,
            slotIndex = slotIndex,
            reviewTotal = reviewTotal
        };
    }

    internal static void TryFlushPendingAward(gameScript game)
    {
        if (game == null) return;
        PendingAwardData data;
        if (!pendingAwards.TryGetValue(game.myID, out data)) return;
        pendingAwards.Remove(game.myID);

        publisherScript studio = FindStudioByID(data.studioID);
        if (studio == null) return;
        if (studio.mS_ == null) return;
        AwardXPForRelease(studio, data.slotIndex, data.reviewTotal, game);
    }

    internal static void AwardXPForRelease(publisherScript studio, int slotIdx, int reviewTotal, gameScript game)
    {
        if (studio == null) return;
        StudioSlotData studioData = GetStudioSlotData(studio.myID);
        if (studioData == null) return;
        if (slotIdx < 0 || slotIdx >= studioData.slots.Length) return;
        SlotData slot = studioData.slots[slotIdx];
        if (slot == null || !slot.isUnlocked) return;

        // Base XP formula: 15 + RoundToInt((reviewTotal + 1) / 10)
        int xpGain = 15 + UnityEngine.Mathf.RoundToInt((reviewTotal + 1f) / 10f);

        // Add game size bonus
        if (game != null)
        {
            int sizeBonus = 0;
            switch (game.gameSize)
            {
                case 0: sizeBonus = 0; break;
                case 1: sizeBonus = 2; break;
                case 2: sizeBonus = 5; break;
                case 3: sizeBonus = 7; break;
                case 4: sizeBonus = 10; break;
                case 5: sizeBonus = 15; break;
            }
            xpGain += sizeBonus;
        }

        // Bargain Bin Devs (11): -25% XP earned
        if (slot.traitID == 11)
            xpGain = UnityEngine.Mathf.RoundToInt(xpGain * 0.75f);

        if (slotIdx == 0)
        {
            studioData.studioXP += xpGain;
        }
        else
        {
            slot.teamXP += xpGain;
        }

        // Track game for future sales milestones
        if (game != null)
        {
            if (State.trackedGames == null) State.trackedGames = new List<TrackedGameSales>();
            if (!State.trackedGames.Exists(tg => tg.gameID == game.myID))
            {
                State.trackedGames.Add(new TrackedGameSales
                {
                    gameID = game.myID,
                    studioID = studio.myID,
                    slotIndex = slotIdx,
                    milestonesMask = 0
                });
            }
        }

        SaveState(currentSaveSlot);
    }

    internal static void CheckSalesMilestones(gameScript game)
    {
        if (game == null) return;
        if (State == null || State.trackedGames == null || State.trackedGames.Count == 0) return;

        // Find if this game is tracked
        TrackedGameSales tracked = null;
        for (int i = 0; i < State.trackedGames.Count; i++)
        {
            if (State.trackedGames[i].gameID == game.myID)
            {
                tracked = State.trackedGames[i];
                break;
            }
        }

        if (tracked == null) return;

        long sales = game.sellsTotal;
        int mask = tracked.milestonesMask;
        int newMask = mask;
        int totalXPEarned = 0;

        // Milestones: 1M (bit 0), 5M (bit 1), 10M (bit 2), 25M (bit 3), 50M (bit 4), 100M (bit 5)
        if (sales >= 1000000L && (mask & 1) == 0)
        {
            newMask |= 1;
            totalXPEarned += 5;
        }
        if (sales >= 5000000L && (mask & 2) == 0)
        {
            newMask |= 2;
            totalXPEarned += 20;
        }
        if (sales >= 10000000L && (mask & 4) == 0)
        {
            newMask |= 4;
            totalXPEarned += 35;
        }
        if (sales >= 25000000L && (mask & 8) == 0)
        {
            newMask |= 8;
            totalXPEarned += 50;
        }
        if (sales >= 50000000L && (mask & 16) == 0)
        {
            newMask |= 16;
            totalXPEarned += 75;
        }
        if (sales >= 100000000L && (mask & 32) == 0)
        {
            newMask |= 32;
            totalXPEarned += 150;
        }

        if (newMask != mask)
        {
            tracked.milestonesMask = newMask;
            
            // Award XP
            StudioSlotData studioData = GetStudioSlotData(tracked.studioID);
            if (studioData != null && tracked.slotIndex >= 0 && tracked.slotIndex < studioData.slots.Length)
            {
                SlotData slot = studioData.slots[tracked.slotIndex];
                if (slot != null)
                {
                    if (tracked.slotIndex == 0)
                    {
                        studioData.studioXP += totalXPEarned;
                    }
                    else
                    {
                        slot.teamXP += totalXPEarned;
                    }

                    // Display notification in news ticker
                    GUI_Main gui = GetGuiMain();
                    if (gui != null)
                    {
                        string teamName = (tracked.slotIndex == 0) ? "Main Studio" : slot.teamName;
                        string gameName = game.GetNameWithTag();
                        gui.CreateTopNewsInfo($"★ {teamName} earned {totalXPEarned} XP! {gameName} reached new sales milestone.");
                    }
                }
            }
            SaveState(currentSaveSlot);
        }
    }
}
