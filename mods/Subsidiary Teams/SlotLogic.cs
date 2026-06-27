using HarmonyLib;
using System.Reflection;
using UnityEngine;

public partial class SubsidiaryTeamsPlugin
{
    internal static long GetTeamUpkeep(publisherScript studio, int slotIdx)
    {
        if (studio == null) return 0L;
        StudioSlotData data = GetStudioSlotData(studio.myID);
        if (slotIdx >= data.unlockedSlots) return 0L;
        SlotData slot = data.slots[slotIdx];
        if (!slot.isUnlocked) return 0L;

        long upkeep;
        try
        {
            var dst = GetDSTPluginType();
            if (dst != null)
            {
                var method = AccessTools.Method(dst, "CalculateSingleSlotUpkeep");
                if (method != null) upkeep = (long)method.Invoke(null, new object[] { studio, slotIdx });
                else upkeep = studio.GetVerwaltungskosten() / data.unlockedSlots;
            }
            else
            {
                upkeep = studio.GetVerwaltungskosten() / data.unlockedSlots;
            }
        }
        catch (System.Exception ex)
        {
            Log?.LogWarning("Failed to call CalculateSingleSlotUpkeep: " + ex);
            upkeep = studio.GetVerwaltungskosten() / data.unlockedSlots;
        }

        return ApplyTagUpkeepModifiers(upkeep, slot);
    }

    internal static float GetAccelerationFactor(publisherScript studio, int slotIdx)
    {
        if (studio == null) return 1f;
        try
        {
            var dst = GetDSTPluginType();
            if (dst != null)
            {
                var method = AccessTools.Method(dst, "GetAccelerationFactor");
                if (method != null) return (float)method.Invoke(null, new object[] { studio, slotIdx });
            }
        }
        catch (System.Exception ex)
        {
            Log?.LogWarning("Failed to call GetAccelerationFactor: " + ex);
        }
        return 1f;
    }

    internal static float ApplyTagSpeedModifiers(float acceleration, SlotData slot, publisherScript studio, int slotIdx, games gamesScript)
    {
        if (slot == null || slot.traitID < 0) return acceleration;

        switch (slot.traitID)
        {
            case 0: acceleration *= 1.25f; break;
            case 8:
                if (slot.helpingSlotIndex == -1)
                    acceleration *= 1.20f;
                break;
            case 9: acceleration *= 0.80f; break;
            case 12:
            {
                gameScript game = FindGameByID(gamesScript, slot.gameID);
                if (game != null)
                {
                    if (game.typ_nachfolger || game.typ_remaster || game.typ_spinoff)
                        acceleration *= 1.30f;
                    else if (game.typ_standard)
                        acceleration *= 0.85f;
                }
                break;
            }
            case 13: acceleration *= 0.80f; break;
            case 19: acceleration *= 1.66f; break;
        }
        return acceleration;
    }

    public static long ApplyTagUpkeepModifiers(long upkeep, SlotData slot)
    {
        if (slot == null || slot.traitID < 0) return upkeep;

        switch (slot.traitID)
        {
            case 3: upkeep = (long)(upkeep * 1.25f); break;
            case 10: upkeep = (long)(upkeep * 1.15f); break;
            case 11: upkeep = (long)(upkeep * 0.60f); break;
            case 16: upkeep = (long)(upkeep * 1.30f); break;
        }
        return upkeep;
    }

    public static float ApplyTagHelperSpeedModifiers(float speedMult, SlotData helperSlot)
    {
        if (helperSlot == null || helperSlot.traitID < 0) return speedMult;
        if (helperSlot.traitID == 8) // Lone Wolves: -20% Contribution Speed
        {
            speedMult *= 0.80f;
        }
        return speedMult;
    }

    internal static int GetMinimumFloorWeeks(int gameSize)
    {
        if (gameSize < 0 || gameSize > 5) return 1;

        try
        {
            var dst = GetDSTPluginType();
            if (dst != null)
            {
                var cfgField = AccessTools.Field(dst, "cfgHardFloorWeeks");
                if (cfgField != null)
                {
                    var cfgArray = cfgField.GetValue(null) as System.Array;
                    if (cfgArray != null && gameSize < cfgArray.Length)
                    {
                        var configEntry = cfgArray.GetValue(gameSize);
                        if (configEntry != null)
                        {
                            var valueProp = configEntry.GetType().GetProperty("Value");
                            if (valueProp != null && valueProp.GetValue(configEntry) is int floorValue)
                                return floorValue;
                        }
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            Log?.LogWarning($"Failed to get minimum floor weeks for game size {gameSize}: {ex}");
        }

        int[] defaultFloors = { 6, 12, 26, 78, 146, 188 };
        return (gameSize >= 0 && gameSize < defaultFloors.Length) ? defaultFloors[gameSize] : 1;
    }

    internal static void TickActiveProjects(publisherScript studio)
    {
        if (studio == null || studio.Geschlossen() || studio.TochterfirmaGeschlossen()) return;
        if (!studio.developer) return;

        StudioSlotData data = GetStudioSlotData(studio.myID);
        games gamesScript = studio.games_ ?? GetGamesScript();
        if (gamesScript == null) return;

        for (int i = 0; i < 3; i++)
        {
            if (i >= data.unlockedSlots)
            {
                data.slots[i].gameID = -1;
                data.slots[i].helpingSlotIndex = -1;
                continue;
            }
            if (data.slots[i].gameID != -1 && FindGameByID(gamesScript, data.slots[i].gameID) == null)
            {
                data.slots[i].gameID = -1;
                data.slots[i].remainingWeeks = 0f;
                data.slots[i].totalWeeks = 0f;
            }
        }

        RefreshSupportAssignments(studio);

        for (int i = 0; i < data.unlockedSlots; i++)
        {
            if (!data.slots[i].isUnlocked) continue;
            if (data.slots[i].role == ROLE_SUPPORT) continue;
            if (data.slots[i].gameID == -1 && data.slots[i].helpingSlotIndex == -1 && !data.slots[i].isPlayerAssigned)
            {
                StartAutonomousProject(studio, i);
            }
        }

        for (int i = 0; i < data.unlockedSlots; i++)
        {
            SlotData slot = data.slots[i];
            if (!slot.isUnlocked || slot.gameID == -1) continue;

            gameScript game = FindGameByID(gamesScript, slot.gameID);
            if (game == null) { slot.gameID = -1; continue; }

            float acceleration = GetAccelerationFactor(studio, i);
            acceleration = ApplyTagSpeedModifiers(acceleration, slot, studio, i, gamesScript);
            slot.remainingWeeks -= acceleration;

            // Early Access Addicts (15): Generate sales during development
            if (slot.traitID == 15 && studio.IsMyTochterfirma() && studio.mS_ != null)
            {
                long weeklyRevenue = Mathf.RoundToInt(game.hype * (game.gameSize + 1) * 200f);
                if (weeklyRevenue > 0L)
                {
                    studio.mS_.Earn(weeklyRevenue, 3);
                }
            }

            if (slot.remainingWeeks <= 0f)
                ReleaseSlotProject(studio, game, slot);
        }
    }

    private static void StartAutonomousProject(publisherScript studio, int slotIndex)
    {
        try
        {
            isBypassingPrefix = true;
            isCreatingAutonomousSlot = true;
            targetSlotIndex = slotIndex;

            StudioSlotData data = GetStudioSlotData(studio.myID);
            currentForcedSpecialtyNum = MapRoleToProjectNum(data.slots[slotIndex].role);

            studio.newGameInWeeks = 0;
            studio.newGameInWeeksORG = 0;

            studio.CreateNewGame2(false, true);
        }
        catch (System.Exception ex)
        {
            Log?.LogError($"Failed to start autonomous project for slot {slotIndex}: {ex}");
        }
        finally
        {
            currentForcedSpecialtyNum = -1;
            isBypassingPrefix = false;
            isCreatingAutonomousSlot = false;
            targetSlotIndex = -1;

            studio.newGameInWeeks = 9999;
            studio.newGameInWeeksORG = 9999;

            SaveState(currentSaveSlot);
        }
    }

    private static void ReleaseSlotProject(publisherScript studio, gameScript game, SlotData slot)
    {
        int releasedSlotIdx = -1;
        StudioSlotData data = GetStudioSlotData(studio.myID);
        for (int i = 0; i < 3; i++)
        {
            if (data.slots[i] == slot) { releasedSlotIdx = i; break; }
        }

        slot.gameID = -1;
        slot.remainingWeeks = 0f;
        slot.totalWeeks = 0f;
        slot.isPlayerAssigned = false;

        if (releasedSlotIdx != -1)
        {
            for (int i = 0; i < 3; i++)
            {
                if (data.slots[i].helpingSlotIndex == releasedSlotIdx)
                    data.slots[i].helpingSlotIndex = -1;
            }
        }

        game.inDevelopment = false;

        // Tag hype boosts on release
        if (slot.traitID == 10) // Modding Darlings
            game.hype += 30f;
        else if (slot.traitID == 14) // Hype Beasts (simplified: flat boost)
            game.hype += 50f;

        if (studio.IsTochterfirma() && studio.IsMyTochterfirma())
        {
            int reviewTotal = game.reviewTotal;
            if (game.reviewTotal <= 0)
            {
                game.CalcReview(entwicklungsbericht: true);
                reviewTotal = game.reviewTotal;
                game.ClearReview();
            }

            if (releasedSlotIdx != -1)
            {
                RegisterPendingAward(game.myID, studio.myID, releasedSlotIdx, reviewTotal);
            }

            if (!studio.tf_autoRelease || (studio.tf_autoRelease && studio.tf_autoReleaseVal != 0 && reviewTotal >= studio.tf_autoReleaseVal * 10))
            {
                MethodInfo waitCorMethod = AccessTools.Method(typeof(publisherScript), "iWaitTochterfirmaReleaseGame");
                if (waitCorMethod != null)
                {
                    var cor = waitCorMethod.Invoke(studio, new object[] { game, studio }) as System.Collections.IEnumerator;
                    studio.StartCoroutine(cor);
                }
            }
            else
            {
                if (game.reviewTotal <= 0)
                {
                    game.CalcReview(entwicklungsbericht: false);
                    studio.reviewText_.GetReviewText(game);
                }
                else
                {
                    game.date_year = studio.mS_.year;
                    game.date_month = studio.mS_.month;
                    studio.reviewText_.GetReviewText(game);
                }

                studio.ReleaseTheGame(game, forceContractGame: false, ignoreTochterfirma: true);
                studio.SetGameOnMarket(game);

                if (studio.tf_autoGamePass && studio.gpS_.gamePass_aktiv && game.CanBeInGamePass())
                    studio.gpS_.GAMEPASS_AddGame(game, updateGamesAmount: true);
            }
        }
    }

    internal static void TryPurchaseSlot(publisherScript studio, int slotIdx)
    {
        long cost = (slotIdx == 1) ? Slot2UnlockCost.Value : Slot3UnlockCost.Value;
        int reqStars = (slotIdx == 1) ? 3 : 5;

        mainScript main = studio.mS_ ?? GetMainScript();
        GUI_Main gui = GetGuiMain();
        if (main == null || gui == null) return;

        StudioSlotData data = GetStudioSlotData(studio.myID);

        // Gate on MOD studio star level (slot 0 stars from our XP system),
        // NOT the vanilla studio.GetStarsAmount().
        int modStudioStars = data.slots[0].stars;
        if (modStudioStars < reqStars)
        {
            gui.MessageBox(
                $"Requires Studio ★{reqStars} in the Development Teams progression to unlock this team slot!\n" +
                $"Current studio level: ★{modStudioStars}. Earn more Studio XP to upgrade.",
                closeMenu: false);
            return;
        }

        if (data.closedYear != -1 && data.closedMonth != -1)
        {
            int monthsSinceClose = (main.year - data.closedYear) * 12 + (main.month - data.closedMonth);
            if (monthsSinceClose < 12)
            {
                gui.MessageBox($"Cannot open any team! Global cooldown active: a team was closed {monthsSinceClose} months ago (requires 12 months).", closeMenu: false);
                return;
            }
        }

        if (main.money < cost) { gui.ShowNoMoney(); return; }

        main.Pay(cost, 29);
        data.slots[slotIdx].isUnlocked = true;
        data.slots[slotIdx].unlockedYear = main.year;
        data.slots[slotIdx].unlockedMonth = main.month;
        data.slots[slotIdx].stars = 1;
        data.slots[slotIdx].speed = 0;
        data.unlockedSlots = Mathf.Max(data.unlockedSlots, slotIdx + 1);
        SaveState(currentSaveSlot);

        var sfx = GetSfxScript();
        if (sfx != null) sfx.PlaySound(22, true);

        gui.MessageBox($"Successfully unlocked Team Slot {slotIdx + 1}!", closeMenu: false);
        lastBuiltForStudio = -1;
        Menu_Stats_Tochterfirma_Main menu = UnityEngine.Object.FindObjectOfType<Menu_Stats_Tochterfirma_Main>();
        if (menu != null && menu.gameObject.activeInHierarchy) menu.UpdateData();
    }

    internal static void TryCloseSlot(publisherScript studio, int slotIdx)
    {
        if (slotIdx < 1 || slotIdx > 2) return;

        StudioSlotData data = GetStudioSlotData(studio.myID);
        SlotData slot = data.slots[slotIdx];
        if (!slot.isUnlocked) return;

        mainScript main = studio.mS_ ?? GetMainScript();
        GUI_Main gui = GetGuiMain();
        if (main == null || gui == null) return;

        int monthsSinceOpen = (main.year - slot.unlockedYear) * 12 + (main.month - slot.unlockedMonth);
        if (monthsSinceOpen < 12 && slot.unlockedYear != -1)
        {
            gui.MessageBox($"Cannot close this team yet! It must remain open for at least 12 months (currently open for {monthsSinceOpen} months).", closeMenu: false);
            return;
        }

        if (slot.gameID != -1)
        {
            games gamesScript = studio.games_ ?? GetGamesScript();
            gameScript game = FindGameByID(gamesScript, slot.gameID);
            if (game != null)
            {
                game.gameObject.tag = "GameRemoved";
                UnityEngine.Object.Destroy(game.gameObject);
            }
        }

        slot.isUnlocked = false;
        slot.stars = 1;
        slot.speed = 0;
        slot.gameID = -1;
        slot.remainingWeeks = 0f;
        slot.totalWeeks = 0f;
        slot.isPlayerAssigned = false;

        // Clean up helpers helping this closed slot
        for (int i = 0; i < 3; i++)
        {
            if (data.slots[i].helpingSlotIndex == slotIdx)
                data.slots[i].helpingSlotIndex = -1;
        }
        slot.helpingSlotIndex = -1;

        data.closedYear = main.year;
        data.closedMonth = main.month;
        SaveState(currentSaveSlot);

        var sfx = GetSfxScript();
        if (sfx != null) sfx.PlaySound(22, true);

        gui.MessageBox($"Successfully closed {slot.teamName}! Global 12-month cooldown for reopening has started.", closeMenu: false);
        lastBuiltForStudio = -1;
        Menu_Stats_Tochterfirma_Main menu = UnityEngine.Object.FindObjectOfType<Menu_Stats_Tochterfirma_Main>();
        if (menu != null && menu.gameObject.activeInHierarchy) menu.UpdateData();
    }

    internal static void TryUpgradeTeam(publisherScript studio, int slotIdx)
    {
        if (slotIdx < 0 || slotIdx > 2) return;

        mainScript main = studio.mS_ ?? GetMainScript();
        GUI_Main gui = GetGuiMain();
        if (main == null || gui == null) return;

        StudioSlotData data = GetStudioSlotData(studio.myID);
        if (data == null) return;

        if (slotIdx == 0)
        {
            SlotData slot = data.slots[0];
            if (slot == null) return;
            if (slot.stars >= 5)
            {
                gui.MessageBox("Studio is already at maximum star rating!", closeMenu: false);
                return;
            }

            int requiredXP = GetStudioXPRequired(slot.stars);
            long fee = GetStudioUpgradeFee(slot.stars);

            if (data.studioXP < requiredXP)
            {
                gui.MessageBox($"Need {requiredXP - data.studioXP} more Studio XP to upgrade to ★{slot.stars + 1}!", closeMenu: false);
                return;
            }
            if (main.money < fee) { gui.ShowNoMoney(); return; }

            main.Pay(fee, 29);
            slot.stars++;
            SaveState(currentSaveSlot);

            var sfx = GetSfxScript();
            if (sfx != null) sfx.PlaySound(22, true);

            gui.MessageBox($"Studio upgraded to ★{slot.stars}!", closeMenu: false);
        }
        else
        {
            SlotData slot = data.slots[slotIdx];
            if (slot == null || !slot.isUnlocked) return;
            if (slot.stars >= 5)
            {
                gui.MessageBox("Team is already at maximum star rating!", closeMenu: false);
                return;
            }
            int studioStars = data.slots[0].stars;
            if (slot.stars >= studioStars)
            {
                gui.MessageBox($"Cannot upgrade beyond the studio's star level (★{studioStars})!", closeMenu: false);
                return;
            }

            int requiredXP = GetTeamXPRequired(slot.stars);
            long fee = GetTeamUpgradeFee(slot.stars);

            if (slot.teamXP < requiredXP)
            {
                gui.MessageBox($"Need {requiredXP - slot.teamXP} more Team XP to upgrade to ★{slot.stars + 1}!", closeMenu: false);
                return;
            }
            if (main.money < fee) { gui.ShowNoMoney(); return; }

            int preStars = slot.stars;
            int preSpeed = slot.speed;

            main.Pay(fee, 29);
            slot.stars++;

            SanitizeSlotSpeeds(studio, data);
            SaveState(currentSaveSlot);

            var sfx = GetSfxScript();
            if (sfx != null) sfx.PlaySound(22, true);

            NotifyDSTUpgrade(studio, preStars, preSpeed, slotIdx);

            if (slot.gameID != -1)
            {
                gui.MessageBox($"Upgraded {slot.teamName} to ★{slot.stars}! This team is currently in active development. The upgrade will take effect starting with the next project.", closeMenu: false);
            }
            else
            {
                gui.MessageBox($"Upgraded {slot.teamName} to ★{slot.stars}!", closeMenu: false);
            }
        }

        lastBuiltForStudio = -1;
        Menu_Stats_Tochterfirma_Main menu = UnityEngine.Object.FindObjectOfType<Menu_Stats_Tochterfirma_Main>();
        if (menu != null && menu.gameObject.activeInHierarchy) menu.UpdateData();
    }



    private static void NotifyDSTUpgrade(publisherScript studio, int preStars, int preSpeed, int slotIdx)
    {
        try
        {
            var dst = GetDSTPluginType();
            if (dst != null)
            {
                var preStarsField = AccessTools.Field(dst, "preUpgradeStars");
                var preSpeedField = AccessTools.Field(dst, "preUpgradeSpeed");
                var preStudioField = AccessTools.Field(dst, "preUpgradeIsStudioLevel");
                var preSlotField = AccessTools.Field(dst, "preUpgradeSlotIndex");
                if (preStarsField != null) preStarsField.SetValue(null, preStars);
                if (preSpeedField != null) preSpeedField.SetValue(null, preSpeed);
                if (preStudioField != null) preStudioField.SetValue(null, false);
                if (preSlotField != null) preSlotField.SetValue(null, slotIdx);

                var method = AccessTools.Method(dst, "RecalculateActiveProjectTimeline");
                if (method != null) method.Invoke(null, new object[] { studio });
            }
        }
        catch (System.Exception ex)
        {
            Log?.LogWarning("Failed to trigger RecalculateActiveProjectTimeline: " + ex);
        }
    }

    internal static void TryStartHelping(publisherScript studio, int helperSlotIdx, int targetSlotIdx)
    {
        if (studio == null) return;
        if (helperSlotIdx < 0 || helperSlotIdx > 2 || targetSlotIdx < 0 || targetSlotIdx > 2) return;
        if (helperSlotIdx == targetSlotIdx) return;

        StudioSlotData data = GetStudioSlotData(studio.myID);
        if (data == null || data.slots == null) return;

        SlotData helperSlot = data.slots[helperSlotIdx];
        SlotData targetSlot = data.slots[targetSlotIdx];

        if (helperSlot == null || !helperSlot.isUnlocked || helperSlot.gameID != -1) return;
        if (targetSlot == null || !targetSlot.isUnlocked || targetSlot.gameID == -1) return;

        int existingHelpers = 0;
        for (int i = 0; i < data.slots.Length; i++)
        {
            if (i != targetSlotIdx && data.slots[i].isUnlocked && data.slots[i].helpingSlotIndex == targetSlotIdx)
                existingHelpers++;
        }
        if (existingHelpers >= 2)
        {
            GUI_Main gui = GetGuiMain();
            if (gui != null) gui.MessageBox("Cannot assign more than 2 helper teams to a single project!", closeMenu: false);
            return;
        }

        helperSlot.helpingSlotIndex = targetSlotIdx;
        SaveState(currentSaveSlot);

        RecalculateDSTTimeline(studio);

        var sfx = GetSfxScript();
        if (sfx != null) sfx.PlaySound(22, true);

        lastBuiltForStudio = -1;
        Menu_Stats_Tochterfirma_Main menu = UnityEngine.Object.FindObjectOfType<Menu_Stats_Tochterfirma_Main>();
        if (menu != null && menu.gameObject.activeInHierarchy) menu.UpdateData();
    }

    internal static void TryStopHelping(publisherScript studio, int helperSlotIdx)
    {
        if (studio == null) return;
        if (helperSlotIdx < 0 || helperSlotIdx > 2) return;

        StudioSlotData data = GetStudioSlotData(studio.myID);
        if (data == null || data.slots == null) return;

        SlotData helperSlot = data.slots[helperSlotIdx];
        if (helperSlot == null || helperSlot.helpingSlotIndex < 0) return;

        helperSlot.helpingSlotIndex = -1;
        SaveState(currentSaveSlot);

        RecalculateDSTTimeline(studio);

        var sfx = GetSfxScript();
        if (sfx != null) sfx.PlaySound(22, true);

        lastBuiltForStudio = -1;
        Menu_Stats_Tochterfirma_Main menu = UnityEngine.Object.FindObjectOfType<Menu_Stats_Tochterfirma_Main>();
        if (menu != null && menu.gameObject.activeInHierarchy) menu.UpdateData();
    }

    private static void RecalculateDSTTimeline(publisherScript studio)
    {
        try
        {
            var dst = GetDSTPluginType();
            if (dst != null)
            {
                var method = AccessTools.Method(dst, "RecalculateActiveProjectTimeline");
                if (method != null) method.Invoke(null, new object[] { studio });
            }
        }
        catch (System.Exception ex)
        {
            Log?.LogWarning("Failed to trigger RecalculateActiveProjectTimeline: " + ex);
        }
    }

    internal static void SanitizeSlotSpeeds(publisherScript studio, StudioSlotData data)
    {
        if (studio == null || data == null) return;
        bool isOrganic = IsOrganicStudio(studio);
        int maxSpeed = studio.developmentSpeed;
        bool changed = false;

        // Synchronize Slot 0 (Main Studio) star rating with vanilla studio rating
        int vanillaStars = studio.GetStarsAmount();
        if (data.slots[0].stars != vanillaStars)
        {
            data.slots[0].stars = vanillaStars;
            changed = true;
        }

        for (int i = 1; i < 3; i++)
        {
            if (data.slots[i].isUnlocked)
            {
                int expectedSpeed = Mathf.RoundToInt(((float)(data.slots[i].stars - 1) / 4f) * maxSpeed);
                if (data.slots[i].speed != expectedSpeed)
                {
                    data.slots[i].speed = expectedSpeed;
                    changed = true;
                }
            }
        }
        if (changed)
        {
            SaveState(currentSaveSlot);
        }
    }

    internal static float CalculateInitialPenaltyMultiplier(publisherScript studio, int slotIdx)
    {
        if (studio == null) return 1f;
        StudioSlotData data = GetStudioSlotData(studio.myID);
        if (data == null) return 1f;

        if (data.slots[slotIdx].traitID == 3) // Crunch Monsters: Immune to multi-team speed penalty
        {
            return 1.0f;
        }

        int activeOtherSlots = 0;
        for (int j = 0; j < 3; j++)
        {
            if (j != slotIdx && data.slots[j].isUnlocked && data.slots[j].gameID != -1)
            {
                activeOtherSlots++;
            }
        }

        float penaltyPct = 0f;
        try
        {
            var dst = GetDSTPluginType();
            if (dst != null)
            {
                var enabledField = AccessTools.Field(dst, "cfgMultiTeamPenaltyEnabled");
                var percentField = AccessTools.Field(dst, "cfgMultiTeamPenaltyPercent");
                if (enabledField != null && percentField != null)
                {
                    var enabledVal = enabledField.GetValue(null);
                    var percentVal = percentField.GetValue(null);
                    if (enabledVal != null && percentVal != null)
                    {
                        bool enabled = (bool)enabledVal.GetType().GetProperty("Value").GetValue(enabledVal);
                        if (enabled)
                        {
                            float pct = (float)percentVal.GetType().GetProperty("Value").GetValue(percentVal);
                            penaltyPct = pct;
                        }
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            Log?.LogWarning("Failed to get penalty config from DST: " + ex);
        }

        return 1.0f + activeOtherSlots * (penaltyPct / 100f);
    }
}
