using HarmonyLib;
using System;
using UnityEngine;
using UnityEngine.UI;

// ════════════════════════════════════════════════════════
//  Item_Stats_Tochterfirma.SetData + Main UI Patches
// ════════════════════════════════════════════════════════

[HarmonyPatch(typeof(Item_Stats_Tochterfirma), "SetData")]
public static class Item_Stats_Tochterfirma_SetData_PrefixPatch
{
    [HarmonyPrefix]
    static bool Prefix(Item_Stats_Tochterfirma __instance)
    {
        try
        {
            DynamicSubsidiaryTimelinePlugin.SafeSetData(__instance);
            return false;
        }
        catch (Exception ex)
        {
            if (DynamicSubsidiaryTimelinePlugin.log != null)
                DynamicSubsidiaryTimelinePlugin.log.LogError("[DynamicTimeline] Item SetData error: " + ex);
            return true;
        }
    }
}

public partial class DynamicSubsidiaryTimelinePlugin
{
    internal static void SafeSetData(Item_Stats_Tochterfirma item)
    {
        if (item == null || item.pS_ == null) return;
        var pS = item.pS_;

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

        SafeSetItemText(item, 0, pS.GetName());
        SafeSetItemImage(item, 1, pS.GetLogo());

        if (item.uiObjects != null && item.uiObjects.Length > 3 && item.uiObjects[3] != null && item.guiMain_ != null)
            item.guiMain_.DrawStars(item.uiObjects[3], Mathf.RoundToInt(pS.stars / 20f));

        SafeSetItemText(item, 2, pS.GetAmountGames().ToString());

        if (item.uiObjects != null && item.uiObjects.Length > 4 && item.uiObjects[4] != null)
        {
            var textComp = item.uiObjects[4].GetComponent<Text>();
            if (textComp != null)
            {
                long customValue = SafeGetFirmenwert(pS);
                textComp.text = item.mS_ != null ? item.mS_.GetMoney(customValue, showDollar: true) : $"${customValue:N0}";
            }
        }

        if (item.uiObjects != null && item.uiObjects.Length > 11 && item.uiObjects[11] != null)
        {
            var textComp = item.uiObjects[11].GetComponent<Text>();
            if (textComp != null)
            {
                long adminCost = CalculateDynamicVerwaltungskosten(pS);
                textComp.text = item.mS_ != null ? item.mS_.GetMoney(adminCost, showDollar: true) : $"${adminCost:N0}";
            }
        }

        SafeSetItemImage(item, 12, item.genres_?.GetPic(pS.fanGenre));
        SafeSetItemText(item, 13, item.mS_ != null ? item.mS_.GetMoney(pS.tf_umsatz_allTime, showDollar: true) : $"${pS.tf_umsatz_allTime:N0}");
        SafeSetItemText(item, 14, item.mS_ != null ? item.mS_.GetMoney(pS.GetTochterfirmaUmsatz24Monate(), showDollar: true) : $"${pS.GetTochterfirmaUmsatz24Monate():N0}");
        SafeSetItemText(item, 7, pS.GetDeveloperPublisherString());

        if (item.tooltip_ != null)
        {
            try { item.tooltip_.c = pS.GetTooltip(); }
            catch { item.tooltip_.c = "<b><size=18>" + pS.GetName() + "</size></b>"; }
        }

        if (pS.Geschlossen())
        {
            var imgColor = item.GetComponent<Image>();
            if (imgColor != null && item.guiMain_ != null && item.guiMain_.colors != null && item.guiMain_.colors.Length > 25)
                imgColor.color = item.guiMain_.colors[25];
        }

        if (item.uiObjects != null && item.uiObjects.Length > 5 && item.uiObjects[5] != null)
        {
            if (pS.Geschlossen() != item.uiObjects[5].activeSelf)
                item.uiObjects[5].SetActive(pS.Geschlossen());
        }

        if (pS.developer)
        {
            float remainingWeeks = 999999f;
            float progressFraction = 0f;
            var game = FindSlotGameClosestToRelease(pS, out remainingWeeks, out progressFraction);

            SafeSetItemFill(item, 8, progressFraction);

            if (game != null)
            {
                SafeSetItemText(item, 10, game.GetNameWithTag());
                if (item.uiObjects != null && item.uiObjects.Length > 9 && item.uiObjects[9] != null)
                {
                    var textComp = item.uiObjects[9].GetComponent<Text>();
                    if (textComp != null)
                    {
                        int weeksInt = Mathf.RoundToInt(remainingWeeks);
                        if (weeksInt > 0)
                            textComp.text = (item.tS_ != null ? item.tS_.GetText(1944) : "Progress") + ": " + Mathf.RoundToInt(progressFraction * 100f) + "%";
                        else
                            textComp.text = game.HasUnreleasedPlattform()
                                ? (item.tS_ != null ? item.tS_.GetText(2316) : "Wait for Release")
                                : (item.tS_ != null ? item.tS_.GetText(1947) : "Finished");
                    }
                }
            }
            else
            {
                SafeSetItemText(item, 10, item.tS_ != null ? item.tS_.GetText(1949) : "No game in development");
                SafeSetItemText(item, 9, "0%");
                SafeSetItemFill(item, 8, 0f);
            }
        }
        else
        {
            SafeSetItemText(item, 9, item.tS_ != null ? item.tS_.GetText(1949) : "No game in development");
            SafeSetItemFill(item, 8, 0f);
        }
    }

    private static void SafeSetItemText(Item_Stats_Tochterfirma item, int idx, string val)
    {
        if (item.uiObjects != null && item.uiObjects.Length > idx && item.uiObjects[idx] != null)
        {
            var t = item.uiObjects[idx].GetComponent<Text>();
            if (t != null) t.text = val;
        }
    }

    private static void SafeSetItemImage(Item_Stats_Tochterfirma item, int idx, Sprite sprite)
    {
        if (item.uiObjects != null && item.uiObjects.Length > idx && item.uiObjects[idx] != null)
        {
            var img = item.uiObjects[idx].GetComponent<Image>();
            if (img != null) img.sprite = sprite;
        }
    }

    private static void SafeSetItemFill(Item_Stats_Tochterfirma item, int idx, float fill)
    {
        if (item.uiObjects != null && item.uiObjects.Length > idx && item.uiObjects[idx] != null)
        {
            var img = item.uiObjects[idx].GetComponent<Image>();
            if (img != null) img.fillAmount = fill;
        }
    }

    // ══════════════════════════════════════════════════
    //  Safe UI Helper Methods (Main UI)
    // ══════════════════════════════════════════════════
    internal static void SafeFindScripts(Menu_Stats_Tochterfirma_Main menu)
    {
        if (menu == null) return;
        var mainGO = GameObject.FindWithTag("Main") ?? GameObject.Find("Main");
        if (mainGO != null)
        {
            var mS = mainGO.GetComponent<mainScript>();
            if (mS != null)
            {
                menuMSField?.SetValue(menu, mS);
                if (mS.tS_ != null) menuTSField?.SetValue(menu, mS.tS_);
            }
            var genComp = mainGO.GetComponent<genres>();
            if (genComp != null) menuGenresField?.SetValue(menu, genComp);
        }

        var sfxObj = GameObject.Find("SFX") ?? GameObject.FindWithTag("SFX");
        if (sfxObj != null)
        {
            var sfx = sfxObj.GetComponent<sfxScript>();
            if (sfx != null) menuSfxField?.SetValue(menu, sfx);
        }

        var guiMainObj = GameObject.Find("CanvasInGameMenu");
        if (guiMainObj != null)
        {
            var guiComp = guiMainObj.GetComponent<GUI_Main>();
            if (guiComp != null) menuGuiMainField?.SetValue(menu, guiComp);
        }

        var canvas = GameObject.Find("CanvasInGameMenu");
        if (canvas != null)
        {
            var comps = canvas.GetComponentsInChildren<Menu_Stats_Tochterfirma_Main>(true);
            foreach (var c in comps)
            {
                if (c == menu) continue;
                if (menuPSField?.GetValue(c) is publisherScript ps && ps != null)
                {
                    if (menuMSField?.GetValue(menu) == null) menuMSField?.SetValue(menu, menuMSField?.GetValue(c));
                    if (menuTSField?.GetValue(menu) == null) menuTSField?.SetValue(menu, menuTSField?.GetValue(c));
                    if (menuGenresField?.GetValue(menu) == null) menuGenresField?.SetValue(menu, menuGenresField?.GetValue(c));
                    if (menuGuiMainField?.GetValue(menu) == null) menuGuiMainField?.SetValue(menu, menuGuiMainField?.GetValue(c));
                    break;
                }
            }
        }
    }
}

[HarmonyPatch(typeof(Menu_Stats_Tochterfirma_Main), "FindScripts")]
public static class Menu_Stats_Tochterfirma_Main_FindScripts_Patch
{
    [HarmonyPrefix]
    static bool Prefix(Menu_Stats_Tochterfirma_Main __instance)
    {
        try
        {
            DynamicSubsidiaryTimelinePlugin.SafeFindScripts(__instance);
            return false;
        }
        catch (Exception ex)
        {
            if (DynamicSubsidiaryTimelinePlugin.log != null)
                DynamicSubsidiaryTimelinePlugin.log.LogError("[DynamicTimeline] FindScripts error: " + ex);
            return true;
        }
    }
}

[HarmonyPatch(typeof(Menu_Stats_Tochterfirma_Main), "Init")]
public static class Menu_Stats_Tochterfirma_Main_Init_Patch
{
    [HarmonyPrefix]
    static bool Prefix(Menu_Stats_Tochterfirma_Main __instance, publisherScript script_)
    {
        try
        {
            if (script_ == null) return false;
            DynamicSubsidiaryTimelinePlugin.SafeFindScripts(__instance);
            DynamicSubsidiaryTimelinePlugin.menuPSField?.SetValue(__instance, script_);
            var nextGame = script_.FindGameInDevelopment();
            DynamicSubsidiaryTimelinePlugin.menuNextGameField?.SetValue(__instance, nextGame);
            __instance.UpdateData();
            return false;
        }
        catch (Exception ex)
        {
            if (DynamicSubsidiaryTimelinePlugin.log != null)
                DynamicSubsidiaryTimelinePlugin.log.LogError("[DynamicTimeline] Init error: " + ex);
            return true;
        }
    }
}

[HarmonyPatch(typeof(Menu_Stats_Tochterfirma_Main), "Update")]
public static class Menu_Stats_Tochterfirma_Main_Update_PostfixPatch
{
    [HarmonyPostfix]
    static void Postfix(Menu_Stats_Tochterfirma_Main __instance)
    {
        try
        {
            var pS = DynamicSubsidiaryTimelinePlugin.menuPSField?.GetValue(__instance) as publisherScript;
            if (pS == null) return;
            var mS = DynamicSubsidiaryTimelinePlugin.menuMSField?.GetValue(__instance) as mainScript;
            var tS = DynamicSubsidiaryTimelinePlugin.menuTSField?.GetValue(__instance) as textScript;
            if (mS == null || tS == null || __instance.uiObjects == null) return;

            SafeSetMainText(__instance, 14,
                tS.GetText(1934) + ": <b>" + mS.GetMoney(DynamicSubsidiaryTimelinePlugin.CalculateDynamicVerwaltungskosten(pS), showDollar: true) + "</b>");
        }
        catch (Exception ex)
        {
            if (DynamicSubsidiaryTimelinePlugin.log != null)
                DynamicSubsidiaryTimelinePlugin.log.LogWarning("[DynamicTimeline] Update error: " + ex);
        }
    }

    private static void SafeSetMainText(Menu_Stats_Tochterfirma_Main menu, int idx, string val)
    {
        if (menu.uiObjects.Length > idx && menu.uiObjects[idx] != null)
        {
            var t = menu.uiObjects[idx].GetComponent<Text>();
            if (t != null) t.text = val;
        }
    }
}

[HarmonyPatch(typeof(Menu_Stats_Tochterfirma_Main), "UpdateData")]
public static class Menu_Stats_Tochterfirma_Main_UpdateData_PostfixPatch
{
    [HarmonyPostfix]
    static void Postfix(Menu_Stats_Tochterfirma_Main __instance)
    {
        try
        {
            var pS = DynamicSubsidiaryTimelinePlugin.menuPSField?.GetValue(__instance) as publisherScript;
            if (pS == null) return;

            var mS = DynamicSubsidiaryTimelinePlugin.menuMSField?.GetValue(__instance) as mainScript;
            var tS = DynamicSubsidiaryTimelinePlugin.menuTSField?.GetValue(__instance) as textScript;
            var genres = DynamicSubsidiaryTimelinePlugin.menuGenresField?.GetValue(__instance) as genres;
            var guiMain = DynamicSubsidiaryTimelinePlugin.menuGuiMainField?.GetValue(__instance) as GUI_Main;

            if (mS == null || tS == null || genres == null || guiMain == null)
            {
                DynamicSubsidiaryTimelinePlugin.SafeFindScripts(__instance);
                mS = DynamicSubsidiaryTimelinePlugin.menuMSField?.GetValue(__instance) as mainScript;
                tS = DynamicSubsidiaryTimelinePlugin.menuTSField?.GetValue(__instance) as textScript;
                genres = DynamicSubsidiaryTimelinePlugin.menuGenresField?.GetValue(__instance) as genres;
                guiMain = DynamicSubsidiaryTimelinePlugin.menuGuiMainField?.GetValue(__instance) as GUI_Main;
            }

            if (mS == null || tS == null || genres == null || guiMain == null || __instance.uiObjects == null) return;

            Action<int, string> safeText = (idx, val) => {
                if (__instance.uiObjects.Length > idx && __instance.uiObjects[idx] != null)
                {
                    var txt = __instance.uiObjects[idx].GetComponent<Text>();
                    if (txt != null) txt.text = val;
                }
            };
            Action<int, Sprite> safeSprite = (idx, sprite) => {
                if (__instance.uiObjects.Length > idx && __instance.uiObjects[idx] != null)
                {
                    var img = __instance.uiObjects[idx].GetComponent<Image>();
                    if (img != null) img.sprite = sprite;
                }
            };

            // Override firm value with organic-aware value
            safeText(7, tS.GetText(685) + ": <b>" + mS.GetMoney(DynamicSubsidiaryTimelinePlugin.SafeGetFirmenwert(pS), showDollar: true) + "</b>");

            // Override admin cost with dynamic calculation
            safeText(14, tS.GetText(1934) + ": <b>" + mS.GetMoney(DynamicSubsidiaryTimelinePlugin.CalculateDynamicVerwaltungskosten(pS), showDollar: true) + "</b>");

            if (pS.developer)
            {
                float closestRemainingWeeks, closestProgress;
                var closestGame = DynamicSubsidiaryTimelinePlugin.FindSlotGameClosestToRelease(pS, out closestRemainingWeeks, out closestProgress);

                // Closest-game progress display (overrides vanilla nextGame_ progress)
                if (closestGame != null)
                {
                    int displayWeeks = Mathf.Max(1, Mathf.RoundToInt(closestRemainingWeeks));

                    if (__instance.uiObjects.Length > 28 && __instance.uiObjects[28] != null)
                    {
                        var tooltip = __instance.uiObjects[28].GetComponent<tooltip>();
                        if (tooltip != null) tooltip.c = tS.GetText(1948).Replace("<NUM>", "<color=blue><b>" + displayWeeks + "</b></color>");
                    }

                    safeText(29, displayWeeks.ToString());

                    if (__instance.uiObjects.Length > 19 && __instance.uiObjects[19] != null)
                    {
                        var img = __instance.uiObjects[19].GetComponent<Image>();
                        if (img != null) img.fillAmount = closestProgress;
                    }

                    if (closestRemainingWeeks > 0.5f)
                        safeText(20, tS.GetText(1944) + ": " + Mathf.RoundToInt(closestProgress * 100f) + "%");
                    else
                        safeText(20, closestGame.HasUnreleasedPlattform() ? tS.GetText(2316) : tS.GetText(1947));

                    // Override game info to show closest-to-release game
                    safeText(23, closestGame.GetNameWithTag());
                    safeText(26, mS.Round(closestGame.GetIpBekanntheit(), 1).ToString());
                    safeSprite(24, closestGame.GetTypSprite());
                    safeSprite(25, closestGame.GetSizeSprite());

                    string genreString = closestGame.GetGenreString();
                    if (closestGame.subgenre != -1) genreString += " / " + closestGame.GetSubGenreString();
                    safeText(27, genreString);

                    SafeSetButtonInteractable(__instance, 30, true);
                    SafeSetButtonInteractable(__instance, 31, true);
                }
                else
                {
                    // No active game — clear closest-game fields
                    safeText(23, "");
                    safeText(26, "");
                    safeSprite(24, guiMain.uiSprites?.Length > 19 ? guiMain.uiSprites[19] : null);
                    safeSprite(25, guiMain.uiSprites?.Length > 19 ? guiMain.uiSprites[19] : null);
                    safeText(27, "");
                    safeText(29, "");
                    SafeSetButtonInteractable(__instance, 30, false);
                    SafeSetButtonInteractable(__instance, 31, false);
                    safeText(20, tS.GetText(1949));
                    if (__instance.uiObjects.Length > 19 && __instance.uiObjects[19] != null)
                    {
                        var img = __instance.uiObjects[19].GetComponent<Image>();
                        if (img != null) img.fillAmount = 0f;
                    }
                    if (__instance.uiObjects.Length > 28 && __instance.uiObjects[28] != null)
                    {
                        var tooltip = __instance.uiObjects[28].GetComponent<tooltip>();
                        if (tooltip != null) tooltip.c = "";
                    }
                }
            }
            else
            {
                // Non-developer subsidiary — no game info to show
                safeText(23, "<i>" + tS.GetText(2029) + "</i>");
                safeText(26, "");
                safeSprite(24, guiMain.uiSprites?.Length > 19 ? guiMain.uiSprites[19] : null);
                safeSprite(25, guiMain.uiSprites?.Length > 19 ? guiMain.uiSprites[19] : null);
                safeText(27, "");
                safeText(29, "");
                SafeSetButtonInteractable(__instance, 30, false);
                SafeSetButtonInteractable(__instance, 31, false);
                safeText(21, "");
                safeText(20, tS.GetText(1949));
                if (__instance.uiObjects.Length > 19 && __instance.uiObjects[19] != null)
                {
                    var img = __instance.uiObjects[19].GetComponent<Image>();
                    if (img != null) img.fillAmount = 0f;
                }
                if (__instance.uiObjects.Length > 28 && __instance.uiObjects[28] != null)
                {
                    var tooltip = __instance.uiObjects[28].GetComponent<tooltip>();
                    if (tooltip != null) tooltip.c = "";
                }
            }
        }
        catch (Exception ex)
        {
            if (DynamicSubsidiaryTimelinePlugin.log != null)
                DynamicSubsidiaryTimelinePlugin.log.LogWarning("[DynamicTimeline] UpdateData postfix error: " + ex);
        }
    }

    private static void SafeSetButtonInteractable(Menu_Stats_Tochterfirma_Main menu, int idx, bool interactable)
    {
        if (menu.uiObjects.Length > idx && menu.uiObjects[idx] != null)
        {
            var btn = menu.uiObjects[idx].GetComponent<Button>();
            if (btn != null) btn.interactable = interactable;
        }
    }
}
