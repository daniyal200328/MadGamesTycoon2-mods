public partial class SubsidiaryTeamsPlugin
{
    public const int ROLE_GENERAL = 0;
    public const int ROLE_SUPPORT = 1;
    public const int ROLE_SEQUEL = 2;
    public const int ROLE_REMASTER = 3;
    public const int ROLE_NEWIP = 4;
    public const int ROLE_ADDON = 5;

    public static int currentForcedSpecialtyNum = -1;

    private static readonly string[] RoleDisplayNames = new string[]
    {
        "General Dev",
        "Support",
        "Sequel Team",
        "Remaster/Remake",
        "Incubation New IP",
        "Addon/DLC"
    };

    public static string GetRoleDisplayName(int role)
    {
        if (role >= 0 && role < RoleDisplayNames.Length)
            return RoleDisplayNames[role];
        return "General Dev";
    }

    public static int MapRoleToProjectNum(int role)
    {
        switch (role)
        {
            case ROLE_SEQUEL: return 0;
            case ROLE_REMASTER: return 1;
            case ROLE_ADDON: return 2;
            case ROLE_NEWIP: return 6;
            default: return -1;
        }
    }

    public static int OverrideProjectNum(int originalProjectNum)
    {
        if (currentForcedSpecialtyNum >= 0) return currentForcedSpecialtyNum;
        return originalProjectNum;
    }

    internal static void RefreshSupportAssignments(publisherScript studio)
    {
        if (studio == null) return;
        StudioSlotData data = GetStudioSlotData(studio.myID);
        if (data == null || data.slots == null) return;

        int bestTarget = -1;
        float bestRemaining = float.MaxValue;
        int activeCount = 0;

        for (int i = 0; i < data.unlockedSlots && i < 3; i++)
        {
            if (!data.slots[i].isUnlocked) continue;
            if (data.slots[i].gameID != -1)
            {
                activeCount++;
                if (data.slots[i].remainingWeeks < bestRemaining)
                {
                    bestRemaining = data.slots[i].remainingWeeks;
                    bestTarget = i;
                }
            }
        }

        bool changed = false;
        for (int i = 0; i < data.unlockedSlots && i < 3; i++)
        {
            if (!data.slots[i].isUnlocked) continue;
            if (data.slots[i].role != ROLE_SUPPORT) continue;
            if (data.slots[i].gameID != -1) continue;

            int newTarget = -1;
            int targetMode = data.slots[i].supportTarget;

            if (targetMode == -1)
            {
                // Auto (Closest to release)
                if (activeCount > 0 && bestTarget != i)
                    newTarget = bestTarget;
            }
            else
            {
                // Manual Forced Target (must be unlocked and developing a game)
                if (targetMode >= 0 && targetMode < 3 && targetMode != i)
                {
                    SlotData targetSlot = data.slots[targetMode];
                    if (targetSlot.isUnlocked && targetSlot.gameID != -1)
                    {
                        newTarget = targetMode;
                    }
                }
            }

            if (data.slots[i].helpingSlotIndex != newTarget)
            {
                data.slots[i].helpingSlotIndex = newTarget;
                changed = true;
            }
        }

        if (changed)
        {
            SaveState(currentSaveSlot);
            RecalculateDSTTimeline(studio);
        }
    }
}
