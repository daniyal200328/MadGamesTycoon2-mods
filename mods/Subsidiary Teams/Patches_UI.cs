using HarmonyLib;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

[HarmonyPatch(typeof(Menu_Stats_Tochterfirma_Main), "UpdateData")]
public static class Menu_Stats_Tochterfirma_Main_UpdateData_Patch
{
    public static void Prefix(Menu_Stats_Tochterfirma_Main __instance)
    {
        if (__instance == null || !__instance) return;
        try
        {
            if (__instance.uiObjects == null || __instance.uiObjects.Length <= 31) return;

            publisherScript studio = SubsidiaryTeamsPlugin.f_pS.GetValue(__instance) as publisherScript;
            if (studio != null && studio.developer)
            {
                var data = SubsidiaryTeamsPlugin.GetStudioSlotData(studio.myID);
                if (data == null || data.slots == null || data.slots.Length == 0) return;

                games gamesScript = studio.games_ ?? SubsidiaryTeamsPlugin.GetGamesScript();
                gameScript slot1Game = SubsidiaryTeamsPlugin.FindGameByID(gamesScript, data.slots[0].gameID);

                if (SubsidiaryTeamsPlugin.f_nextGame != null)
                    SubsidiaryTeamsPlugin.f_nextGame.SetValue(__instance, slot1Game);

                if (slot1Game != null)
                {
                    float acceleration = SubsidiaryTeamsPlugin.GetAccelerationFactor(studio, 0);
                    int minFloor = SubsidiaryTeamsPlugin.GetMinimumFloorWeeks(slot1Game.gameSize);
                    int calculatedWeeks = Mathf.RoundToInt(data.slots[0].remainingWeeks / acceleration);
                    studio.newGameInWeeks = Mathf.Max(1, calculatedWeeks);
                    int calculatedTotalWeeks = Mathf.RoundToInt(data.slots[0].totalWeeks / acceleration);
                    studio.newGameInWeeksORG = Mathf.Max(minFloor, calculatedTotalWeeks);
                }
                else
                {
                    studio.newGameInWeeks = 0;
                    studio.newGameInWeeksORG = 0;
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error in SubsidiaryTeams UpdateData Prefix patch: " + ex);
        }
    }

    public static void Postfix(Menu_Stats_Tochterfirma_Main __instance)
    {
        if (__instance == null || !__instance) return;
        try
        {
            publisherScript studio = SubsidiaryTeamsPlugin.f_pS.GetValue(__instance) as publisherScript;
            SubsidiaryTeamsPlugin.RefreshSlotUI(__instance, studio);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error in SubsidiaryTeams UpdateData Postfix patch: " + ex);
        }
    }
}

[HarmonyPatch(typeof(Menu_Stats_Tochterfirma_Main), "Update")]
public static class Menu_Stats_Tochterfirma_Main_Update_Patch
{
    private static int frameInterval;

    public static void Postfix(Menu_Stats_Tochterfirma_Main __instance)
    {
        frameInterval++;
        if (frameInterval % 10 != 0) return; // update every 10 frames

        if (__instance == null || !__instance) return;
        try
        {
            publisherScript studio = SubsidiaryTeamsPlugin.f_pS.GetValue(__instance) as publisherScript;
            if (studio != null && studio.developer && __instance.uiObjects != null && __instance.uiObjects.Length > 29)
            {
                SubsidiaryTeamsPlugin.RefreshSupportAssignments(studio);
                SubsidiaryTeamsPlugin.RefreshSlotUI(__instance, studio);

                var data = SubsidiaryTeamsPlugin.GetStudioSlotData(studio.myID);
                if (data == null || data.slots == null || data.slots.Length == 0) return;

                var slot1 = data.slots[0];
                if (slot1 == null) return;

                if (slot1.gameID == -1)
                {
                    var txt = __instance.uiObjects[29]?.GetComponent<Text>();
                    if (txt != null) txt.text = "";
                    var fill = __instance.uiObjects[19]?.GetComponent<Image>();
                    if (fill != null) fill.fillAmount = 0f;
                    var prgTxt = __instance.uiObjects[20]?.GetComponent<Text>();
                    if (prgTxt != null) prgTxt.text = "";
                }
                else
                {
                    float acceleration = SubsidiaryTeamsPlugin.GetAccelerationFactor(studio, 0);
                    games gamesScript = studio.games_ ?? SubsidiaryTeamsPlugin.GetGamesScript();
                    gameScript slot1Game = SubsidiaryTeamsPlugin.FindGameByID(gamesScript, slot1.gameID);
                    int minFloor = (slot1Game != null) ? SubsidiaryTeamsPlugin.GetMinimumFloorWeeks(slot1Game.gameSize) : 1;

                    float progress = (slot1.totalWeeks > 0f) ? (1f - (slot1.remainingWeeks / slot1.totalWeeks)) : 0f;
                    progress = Mathf.Clamp01(progress);

                    var fill = __instance.uiObjects[19]?.GetComponent<Image>();
                    if (fill != null) fill.fillAmount = progress;

                    var prgTxt = __instance.uiObjects[20]?.GetComponent<Text>();
                    if (prgTxt != null) prgTxt.text = $"Progress: {Mathf.RoundToInt(progress * 100f)}%";

                    int calculatedWeeks = Mathf.CeilToInt(slot1.remainingWeeks / acceleration);
                    int weeks = Mathf.Max(1, calculatedWeeks);
                    var txt = __instance.uiObjects[29]?.GetComponent<Text>();
                    if (txt != null) txt.text = weeks.ToString();

                    var clockIcon = __instance.uiObjects[28];
                    if (clockIcon != null)
                    {
                        tooltip clockTooltip = clockIcon.GetComponent<tooltip>();
                        textScript tS = SubsidiaryTeamsPlugin.f_tS.GetValue(__instance) as textScript;
                        if (clockTooltip != null && tS != null)
                        {
                            string text = tS.GetText(1948);
                            text = text.Replace("<NUM>", "<color=blue><b>" + weeks + "</b></color>");
                            clockTooltip.c = text;
                        }
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error in SubsidiaryTeams Update Postfix patch: " + ex);
        }
    }
}

[HarmonyPatch(typeof(Menu_Stats_Tochterfirma_Main), "ResetGame")]
public static class Menu_Stats_Tochterfirma_Main_ResetGame_Patch
{
    public static bool Prefix(Menu_Stats_Tochterfirma_Main __instance)
    {
        try
        {
            publisherScript studio = SubsidiaryTeamsPlugin.f_pS.GetValue(__instance) as publisherScript;
            if (studio != null)
            {
                var data = SubsidiaryTeamsPlugin.GetStudioSlotData(studio.myID);
                int slotIdx = SubsidiaryTeamsPlugin.slotToDiscard >= 0 ? SubsidiaryTeamsPlugin.slotToDiscard : 0;

                if (slotIdx < 3)
                {
                    data.slots[slotIdx].gameID = -1;
                    data.slots[slotIdx].remainingWeeks = 0f;
                    data.slots[slotIdx].totalWeeks = 0f;
                    data.slots[slotIdx].isPlayerAssigned = false;
                }

                __instance.UpdateData();
                return false;
            }
        }
        finally
        {
            SubsidiaryTeamsPlugin.slotToDiscard = -1;
            SubsidiaryTeamsPlugin.SaveState(SubsidiaryTeamsPlugin.currentSaveSlot);
        }
        return true;
    }
}

[HarmonyPatch(typeof(StudioDirectorPlugin), "BeginDesignForSubsidiary")]
public static class SPD_BeginDesignForSubsidiary_Patch
{
    public static bool Prefix(publisherScript subsidiary)
    {
        if (subsidiary == null) return true;

        var data = SubsidiaryTeamsPlugin.GetStudioSlotData(subsidiary.myID);
        bool hasIdle = false;
        for (int i = 0; i < data.unlockedSlots; i++)
        {
            if (data.slots[i].gameID == -1) { hasIdle = true; break; }
        }

        if (!hasIdle)
        {
            GUI_Main gui = SubsidiaryTeamsPlugin.GetGuiMain();
            if (gui != null) gui.MessageBox("All development teams are currently busy! Discard or wait for a project to finish first.", closeMenu: false);
            return false;
        }

        if (data.unlockedSlots <= 1)
        {
            SubsidiaryTeamsPlugin.selectedSlotIndex = 0;
            SubsidiaryTeamsPlugin.isDesigningForSlot = true;
            return true;
        }

        if (SubsidiaryTeamsPlugin.isDesigningForSlot) return true;

        SubsidiaryTeamsPlugin.showSlotPicker = true;
        SubsidiaryTeamsPlugin.pickerTarget = subsidiary;
        return false;
    }
}

[HarmonyPatch(typeof(StudioDirectorPlugin), "FindActiveSubsidiaryProject")]
public static class SPD_FindActiveSubsidiaryProject_Patch
{
    public static bool Prefix(ref gameScript __result)
    {
        if (SubsidiaryTeamsPlugin.isDesigningForSlot)
        {
            __result = null;
            return false;
        }
        return true;
    }
}

[HarmonyPatch(typeof(StudioDirectorPlugin), "ConvertCreatedGameToSubsidiaryProject")]
public static class SPD_ConvertCreatedGame_Patch
{
    public static void Prefix()
    {
        SubsidiaryTeamsPlugin.isCreatingPlayerSlotProject = true;
        SubsidiaryTeamsPlugin.pendingPlayerSlotDuration = -1f;
    }

    public static void Postfix()
    {
        SubsidiaryTeamsPlugin.isCreatingPlayerSlotProject = false;
        SubsidiaryTeamsPlugin.isDesigningForSlot = false;
        SubsidiaryTeamsPlugin.SaveState(SubsidiaryTeamsPlugin.currentSaveSlot);
    }
}

[HarmonyPatch(typeof(StudioDirectorPlugin), "CancelDesignContext")]
public static class SPD_CancelDesignContext_Patch
{
    public static void Postfix()
    {
        SubsidiaryTeamsPlugin.isDesigningForSlot = false;
        SubsidiaryTeamsPlugin.selectedSlotIndex = -1;
    }
}

[HarmonyPatch(typeof(Object), "Destroy", new System.Type[] { typeof(Object) })]
public static class UnityObjectDestroy_Patch
{
    public static bool Prefix(Object obj)
    {
        if (!SubsidiaryTeamsPlugin.isCreatingPlayerSlotProject) return true;

        GameObject go = obj as GameObject;
        if (go == null) return true;

        gameScript game = go.GetComponent<gameScript>();
        if (game == null) return true;

        if (SubsidiaryTeamsPlugin.State?.studios != null)
        {
            foreach (var studio in SubsidiaryTeamsPlugin.State.studios)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (studio.slots[i].gameID == game.myID)
                    {
                        go.tag = "Game";
                        return false;
                    }
                }
            }
        }
        return true;
    }
}

[HarmonyPatch(typeof(publisherScript), "ReleaseGameInDevelopment")]
public static class PublisherScript_ReleaseGameInDevelopment_Patch
{
    public static bool Prefix(publisherScript __instance)
    {
        if (__instance == null || !__instance) return true;
        try
        {
            if (__instance.IsTochterfirma() && __instance.IsMyTochterfirma()) return false;
        }
        catch { }
        return true;
    }
}

[HarmonyPatch(typeof(Menu_Stats_Tochterfirma_GameEntwicklungsbericht), "Init")]
public static class GameEntwicklungsbericht_Init_Patch
{
    public static void Postfix(Menu_Stats_Tochterfirma_GameEntwicklungsbericht __instance, gameScript game_)
    {
        if (game_ == null) return;
        publisherScript studio = SubsidiaryTeamsPlugin.FindStudioByID(game_.developerID);
        if (studio == null) return;

        var data = SubsidiaryTeamsPlugin.GetStudioSlotData(studio.myID);
        int slotIdx = -1;
        for (int i = 0; i < 3; i++)
        {
            if (data.slots[i].gameID == game_.myID) { slotIdx = i; break; }
        }

        if (slotIdx < 0) return;

        var slot = data.slots[slotIdx];
        float acceleration = SubsidiaryTeamsPlugin.GetAccelerationFactor(studio, slotIdx);
        int minFloor = SubsidiaryTeamsPlugin.GetMinimumFloorWeeks(game_.gameSize);

        float progress = (slot.totalWeeks > 0f) ? (1f - (slot.remainingWeeks / slot.totalWeeks)) : 0f;
        progress = Mathf.Clamp01(progress);
        int calculatedWeeks = Mathf.CeilToInt(slot.remainingWeeks / acceleration);
        int weeks = Mathf.Max(1, calculatedWeeks);

        if (__instance.uiObjects != null)
        {
            if (__instance.uiObjects.Length > 2 && __instance.uiObjects[2] != null)
            {
                Image img = __instance.uiObjects[2].GetComponent<Image>();
                if (img != null) img.fillAmount = progress;
            }

            textScript tS = null;
            mainScript mS = Object.FindObjectOfType<mainScript>();
            if (mS != null) tS = mS.tS_;

            if (tS != null)
            {
                if (__instance.uiObjects.Length > 3 && __instance.uiObjects[3] != null)
                {
                    Text txt = __instance.uiObjects[3].GetComponent<Text>();
                    if (txt != null)
                        txt.text = slot.remainingWeeks > 2f
                            ? tS.GetText(1944) + ": " + Mathf.RoundToInt(progress * 100f) + "%"
                            : tS.GetText(1947);
                }

                if (__instance.uiObjects.Length > 31 && __instance.uiObjects[31] != null)
                {
                    Text txt = __instance.uiObjects[31].GetComponent<Text>();
                    if (txt != null)
                    {
                        string text = tS.GetText(1948);
                        text = text.Replace("<NUM>", "<color=blue><b>" + weeks + "</b></color>");
                        txt.text = text;
                    }
                }

                if (__instance.uiObjects.Length > 5 && __instance.uiObjects[5] != null)
                {
                    Text txt = __instance.uiObjects[5].GetComponent<Text>();
                    if (txt != null)
                    {
                        game_.CalcReview(entwicklungsbericht: true);
                        float f = (float)game_.reviewTotal * progress;
                        int num2 = Mathf.RoundToInt(f) - 10;
                        int num3 = Mathf.RoundToInt(f) + 10;
                        num2 = num2 / 10 * 10;
                        num3 = num3 / 10 * 10;
                        if (num2 < 1) num2 = 1;
                        if (num3 > 100) num3 = 100;
                        txt.text = tS.GetText(452) + "<color=blue>" + " " + num2 + "% - " + num3 + "%" + "</color>";
                        game_.ClearReview();
                    }
                }
            }

            if (__instance.uiObjects.Length > 6 && __instance.uiObjects[6] != null)
            {
                genres genComp = Object.FindObjectOfType<genres>();
                Image img = __instance.uiObjects[6].GetComponent<Image>();
                if (genComp != null && img != null)
                    img.sprite = genComp.GetScreenshot(game_.maingenre, Mathf.RoundToInt(game_.points_grafik * progress));
            }

            if (__instance.uiObjects.Length > 7 && __instance.uiObjects[7] != null)
            {
                Text txt = __instance.uiObjects[7].GetComponent<Text>();
                if (txt != null) txt.text = Mathf.RoundToInt(game_.points_gameplay * progress).ToString();
            }
            if (__instance.uiObjects.Length > 8 && __instance.uiObjects[8] != null)
            {
                Text txt = __instance.uiObjects[8].GetComponent<Text>();
                if (txt != null) txt.text = Mathf.RoundToInt(game_.points_grafik * progress).ToString();
            }
            if (__instance.uiObjects.Length > 9 && __instance.uiObjects[9] != null)
            {
                Text txt = __instance.uiObjects[9].GetComponent<Text>();
                if (txt != null) txt.text = Mathf.RoundToInt(game_.points_sound * progress).ToString();
            }
            if (__instance.uiObjects.Length > 10 && __instance.uiObjects[10] != null)
            {
                Text txt = __instance.uiObjects[10].GetComponent<Text>();
                if (txt != null) txt.text = Mathf.RoundToInt(game_.points_technik * progress).ToString();
            }

            if (__instance.uiObjects.Length > 17 && __instance.uiObjects[17] != null)
            {
                Component_Aufwertungen comp = __instance.uiObjects[17].GetComponent<Component_Aufwertungen>();
                if (comp != null) comp.InitNpcGame(game_, progress);
            }
        }
    }
}

[HarmonyPatch(typeof(Menu_Stats_Tochterfirma_Main), "BUTTON_FirmaAufwerten")]
public static class Menu_Stats_Tochterfirma_Main_BUTTON_FirmaAufwerten_Patch
{
    public static bool Prefix(Menu_Stats_Tochterfirma_Main __instance, publisherScript ___pS_, GUI_Main ___guiMain_, sfxScript ___sfx_)
    {
        if (___pS_ == null) return true;
        if (!___pS_.IsTochterfirma() || !___pS_.IsMyTochterfirma()) return true;

        var data = SubsidiaryTeamsPlugin.GetStudioSlotData(___pS_.myID);
        if (data == null) return true;

        var slot = data.slots[0];
        if (slot.stars >= 5)
        {
            if (___guiMain_ != null)
            {
                ___guiMain_.MessageBox("Studio is already at maximum star rating!", closeMenu: false);
            }
            if (___sfx_ != null) ___sfx_.PlaySound(5, force: true);
            return false;
        }

        int requiredXP = SubsidiaryTeamsPlugin.GetStudioXPRequired(slot.stars);
        if (data.studioXP < requiredXP)
        {
            if (___guiMain_ != null)
            {
                ___guiMain_.MessageBox(
                    $"Cannot upgrade studio! You must complete the XP challenge first.\n" +
                    $"Required: {requiredXP} XP • Current: {data.studioXP} XP.",
                    closeMenu: false);
            }
            if (___sfx_ != null) ___sfx_.PlaySound(5, force: true);
            return false; // Blocks the upgrade confirmation window from opening!
        }

        return true; // Let the confirmation window open
    }

    public static void Postfix(Menu_Stats_Tochterfirma_Main __instance, publisherScript ___pS_, GUI_Main ___guiMain_)
    {
        if (___pS_ == null) return;
        if (!___pS_.IsTochterfirma() || !___pS_.IsMyTochterfirma()) return;

        var data = SubsidiaryTeamsPlugin.GetStudioSlotData(___pS_.myID);
        if (data == null) return;

        var slot = data.slots[0];
        bool shouldBlock = slot.stars >= 5 ||
            data.studioXP < SubsidiaryTeamsPlugin.GetStudioXPRequired(slot.stars);

        if (shouldBlock && ___guiMain_ != null && ___guiMain_.uiObjects.Length > 388)
        {
            var popup = ___guiMain_.uiObjects[388];
            if (popup != null) popup.SetActive(false);
        }
    }
}

[HarmonyPatch(typeof(Menu_W_FirmaAufwerten), "BUTTON_Yes")]
public static class Menu_W_FirmaAufwerten_BUTTON_Yes_Patch
{
    public static void Postfix(Menu_W_FirmaAufwerten __instance, publisherScript ___pS_)
    {
        if (___pS_ == null) return;
        if (!___pS_.IsTochterfirma() || !___pS_.IsMyTochterfirma()) return;

        var data = SubsidiaryTeamsPlugin.GetStudioSlotData(___pS_.myID);
        if (data == null) return;

        var slot = data.slots[0];
        int vanillaStars = ___pS_.GetStarsAmount();
        if (vanillaStars > slot.stars)
        {
            int preStars = slot.stars;
            slot.stars = vanillaStars;
            SubsidiaryTeamsPlugin.SaveState(SubsidiaryTeamsPlugin.currentSaveSlot);
            SubsidiaryTeamsPlugin.Log?.LogInfo($"Studio slot 0 stars synced to vanilla stars: {slot.stars}");

            // Recalculate speeds
            SubsidiaryTeamsPlugin.SanitizeSlotSpeeds(___pS_, data);

            // Refresh the main UI
            Menu_Stats_Tochterfirma_Main menu = UnityEngine.Object.FindObjectOfType<Menu_Stats_Tochterfirma_Main>();
            if (menu != null && menu.gameObject.activeInHierarchy)
            {
                menu.UpdateData();
            }
        }
    }
}

[HarmonyPatch(typeof(Menu_W_FirmaAufwerten), "Init")]
public static class Menu_W_FirmaAufwerten_Init_PrintCosts_Patch
{
    public static void Postfix(Menu_W_FirmaAufwerten __instance)
    {
        if (__instance.costs != null)
        {
            for (int i = 0; i < __instance.costs.Length; i++)
            {
                SubsidiaryTeamsPlugin.Log?.LogInfo($"[Vanilla Studio Upgrade Cost] Stars {i}★ -> {i+1}★: {__instance.costs[i]}");
            }
        }
    }
}


