using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

// ════════════════════════════════════════════════════════
//  Menu_Stats_TochterfirmaSettings Patches + Upgrade Buttons
// ════════════════════════════════════════════════════════

public partial class DynamicSubsidiaryTimelinePlugin
{
    // ── Settings Safe Helpers ──────────────────────────

    internal static void SafeFindScriptsSettings(Menu_Stats_TochterfirmaSettings menu)
    {
        if (menu == null) return;
        var mainGO = GameObject.FindWithTag("Main") ?? GameObject.Find("Main");
        if (mainGO != null)
        {
            var mS = mainGO.GetComponent<mainScript>();
            if (mS != null)
            {
                setMSField?.SetValue(menu, mS);
                if (mS.tS_ != null) setTSField?.SetValue(menu, mS.tS_);
                var genComp = mainGO.GetComponent<genres>();
                if (genComp != null) setGenresField?.SetValue(menu, genComp);
            }
            var gamesComp = mainGO.GetComponent<games>();
            if (gamesComp != null) setGamesField?.SetValue(menu, gamesComp);
        }

        var sfxObj = GameObject.Find("SFX") ?? GameObject.FindWithTag("SFX");
        if (sfxObj != null)
        {
            var sfx = sfxObj.GetComponent<sfxScript>();
            if (sfx != null) setSfxField?.SetValue(menu, sfx);
        }

        var guiMainObj = GameObject.Find("CanvasInGameMenu");
        if (guiMainObj != null)
        {
            var guiComp = guiMainObj.GetComponent<GUI_Main>();
            if (guiComp != null) setGuiMainField?.SetValue(menu, guiComp);
        }
    }

    internal static void SafeClearAndAddOptions(GameObject[] uiObjects, int idx, List<string> options)
    {
        if (uiObjects == null || uiObjects.Length <= idx || uiObjects[idx] == null) return;
        var dropdown = uiObjects[idx].GetComponent<Dropdown>();
        if (dropdown == null) return;
        dropdown.ClearOptions();
        dropdown.AddOptions(options);
    }

    internal static void SafeSetDropdownValue(GameObject[] uiObjects, int idx, int val)
    {
        if (uiObjects == null || uiObjects.Length <= idx || uiObjects[idx] == null) return;
        var dropdown = uiObjects[idx].GetComponent<Dropdown>();
        if (dropdown == null) return;
        dropdown.value = val;
    }

    internal static void SafeSetToggleIsOn(GameObject[] uiObjects, int idx, bool val)
    {
        if (uiObjects == null || uiObjects.Length <= idx || uiObjects[idx] == null) return;
        var toggle = uiObjects[idx].GetComponent<Toggle>();
        if (toggle == null) return;
        toggle.isOn = val;
    }

    internal static void SafeSetToggleInteractable(GameObject[] uiObjects, int idx, bool val)
    {
        if (uiObjects == null || uiObjects.Length <= idx || uiObjects[idx] == null) return;
        var toggle = uiObjects[idx].GetComponent<Toggle>();
        if (toggle == null) return;
        toggle.interactable = val;
    }

    internal static void SafeSetButtonInteractable(GameObject[] uiObjects, int idx, bool val)
    {
        if (uiObjects == null || uiObjects.Length <= idx || uiObjects[idx] == null) return;
        var btn = uiObjects[idx].GetComponent<Button>();
        if (btn == null) return;
        btn.interactable = val;
    }

    internal static void SafeSetDropdownInteractable(GameObject[] uiObjects, int idx, bool val)
    {
        if (uiObjects == null || uiObjects.Length <= idx || uiObjects[idx] == null) return;
        var dropdown = uiObjects[idx].GetComponent<Dropdown>();
        if (dropdown == null) return;
        dropdown.interactable = val;
    }

    internal static void SafeSetText(GameObject[] uiObjects, int idx, string val)
    {
        if (uiObjects == null || uiObjects.Length <= idx || uiObjects[idx] == null) return;
        var text = uiObjects[idx].GetComponent<Text>();
        if (text == null) return;
        text.text = val;
    }
}

// ── Settings FindScripts ──────────────────────────────

[HarmonyPatch(typeof(Menu_Stats_TochterfirmaSettings), "FindScripts")]
public static class Menu_Stats_TochterfirmaSettings_FindScripts_Patch
{
    [HarmonyPrefix]
    static bool Prefix(Menu_Stats_TochterfirmaSettings __instance)
    {
        try
        {
            DynamicSubsidiaryTimelinePlugin.SafeFindScriptsSettings(__instance);
        }
        catch (Exception ex)
        {
            if (DynamicSubsidiaryTimelinePlugin.log != null)
                DynamicSubsidiaryTimelinePlugin.log.LogError("[DynamicTimeline] Settings FindScripts error: " + ex);
        }
        return false;
    }
}

// ── Settings InitDropdowns ────────────────────────────

[HarmonyPatch(typeof(Menu_Stats_TochterfirmaSettings), "InitDropdowns")]
public static class Menu_Stats_TochterfirmaSettings_InitDropdowns_Patch
{
    [HarmonyPrefix]
    static bool Prefix(Menu_Stats_TochterfirmaSettings __instance)
    {
        try
        {
            DynamicSubsidiaryTimelinePlugin.SafeFindScriptsSettings(__instance);
            var pS = __instance.pS_;
            var tS = DynamicSubsidiaryTimelinePlugin.setTSField?.GetValue(__instance) as textScript;
            var genres = DynamicSubsidiaryTimelinePlugin.setGenresField?.GetValue(__instance) as genres;
            if (pS == null || tS == null || genres == null) return false;

            var list = new List<string>();
            list.Add(pS.tf_publisher ? tS.GetText(432) : "<color=red>" + tS.GetText(432) + "</color>");
            list.Add(pS.tf_developer ? tS.GetText(274) : "<color=red>" + tS.GetText(274) + "</color>");
            list.Add(pS.tf_publisher && pS.tf_developer
                ? tS.GetText(432) + " & " + tS.GetText(274)
                : "<color=red>" + tS.GetText(432) + " & " + tS.GetText(274) + "</color>");
            DynamicSubsidiaryTimelinePlugin.SafeClearAndAddOptions(__instance.uiObjects, 5, list);

            list = new List<string> { tS.GetText(1963), tS.GetText(1964), tS.GetText(1965) };
            DynamicSubsidiaryTimelinePlugin.SafeClearAndAddOptions(__instance.uiObjects, 6, list);

            list = new List<string> { tS.GetText(1966), tS.GetText(329), tS.GetText(330), tS.GetText(331), tS.GetText(332), tS.GetText(333), tS.GetText(2193) };
            DynamicSubsidiaryTimelinePlugin.SafeClearAndAddOptions(__instance.uiObjects, 7, list);

            list = new List<string> { tS.GetText(1966) };
            if (genres.genres_LEVEL != null)
            {
                for (int i = 0; i < genres.genres_LEVEL.Length; i++)
                {
                    bool unlocked = genres.genres_UNLOCK != null && i < genres.genres_UNLOCK.Length && genres.genres_UNLOCK[i];
                    list.Add(unlocked ? genres.GetName(i) : "<color=red>" + genres.GetName(i) + "</color>");
                }
            }
            DynamicSubsidiaryTimelinePlugin.SafeClearAndAddOptions(__instance.uiObjects, 8, list);

            list = new List<string> { tS.GetText(1966), "< 10%", "< 20%", "< 30%", "< 40%", "< 50%", "< 60%", "< 70%", "< 80%", "< 90%" };
            DynamicSubsidiaryTimelinePlugin.SafeClearAndAddOptions(__instance.uiObjects, 32, list);
        }
        catch (Exception ex)
        {
            if (DynamicSubsidiaryTimelinePlugin.log != null)
                DynamicSubsidiaryTimelinePlugin.log.LogError("[DynamicTimeline] Settings InitDropdowns error: " + ex);
        }
        return false;
    }
}

// ── Settings SetData ─────────────────────────────────

[HarmonyPatch(typeof(Menu_Stats_TochterfirmaSettings), "SetData")]
public static class Menu_Stats_TochterfirmaSettings_SetData_Patch
{
    [HarmonyPrefix]
    static bool Prefix(Menu_Stats_TochterfirmaSettings __instance)
    {
        try
        {
            var pS = __instance.pS_;
            if (pS == null) return false;
            var ui = __instance.uiObjects;
            if (ui == null) return false;

            if (pS.publisher && !pS.developer) DynamicSubsidiaryTimelinePlugin.SafeSetDropdownValue(ui, 5, 0);
            if (!pS.publisher && pS.developer) DynamicSubsidiaryTimelinePlugin.SafeSetDropdownValue(ui, 5, 1);
            if (pS.publisher && pS.developer) DynamicSubsidiaryTimelinePlugin.SafeSetDropdownValue(ui, 5, 2);

            DynamicSubsidiaryTimelinePlugin.SafeSetDropdownValue(ui, 6, pS.tf_entwicklungsdauer);
            DynamicSubsidiaryTimelinePlugin.SafeSetDropdownValue(ui, 7, pS.tf_gameSize);
            DynamicSubsidiaryTimelinePlugin.SafeSetDropdownValue(ui, 8, pS.tf_gameGenre);

            DynamicSubsidiaryTimelinePlugin.SafeSetToggleIsOn(ui, 10, pS.tf_autoRelease);
            DynamicSubsidiaryTimelinePlugin.SafeSetToggleIsOn(ui, 11, pS.tf_onlyPlayerConsole);
            DynamicSubsidiaryTimelinePlugin.SafeSetToggleIsOn(ui, 12, pS.tf_allowMMO);
            DynamicSubsidiaryTimelinePlugin.SafeSetToggleIsOn(ui, 13, pS.tf_allowF2P);
            DynamicSubsidiaryTimelinePlugin.SafeSetToggleIsOn(ui, 14, pS.tf_allowAddon);
            DynamicSubsidiaryTimelinePlugin.SafeSetToggleIsOn(ui, 15, pS.tf_noArcade);
            DynamicSubsidiaryTimelinePlugin.SafeSetToggleIsOn(ui, 16, pS.tf_noHandy);
            DynamicSubsidiaryTimelinePlugin.SafeSetToggleIsOn(ui, 17, pS.tf_noRetro);
            DynamicSubsidiaryTimelinePlugin.SafeSetToggleIsOn(ui, 18, pS.tf_noPorts);
            DynamicSubsidiaryTimelinePlugin.SafeSetToggleIsOn(ui, 19, pS.tf_noBudget);
            DynamicSubsidiaryTimelinePlugin.SafeSetToggleIsOn(ui, 20, pS.tf_noGOTY);
            DynamicSubsidiaryTimelinePlugin.SafeSetToggleIsOn(ui, 21, pS.tf_noRemaster);
            DynamicSubsidiaryTimelinePlugin.SafeSetToggleIsOn(ui, 22, pS.tf_noSpinoffs);
            DynamicSubsidiaryTimelinePlugin.SafeSetToggleIsOn(ui, 23, pS.tf_ownPublisher);
            DynamicSubsidiaryTimelinePlugin.SafeSetToggleIsOn(ui, 43, pS.tf_autoGamePass);
            DynamicSubsidiaryTimelinePlugin.SafeSetDropdownValue(ui, 32, pS.tf_autoReleaseVal);
            DynamicSubsidiaryTimelinePlugin.SafeSetToggleIsOn(ui, 41, pS.tf_noBundles);
            DynamicSubsidiaryTimelinePlugin.SafeSetToggleIsOn(ui, 42, pS.tf_noAddonBundles);

            __instance.UpdateData();
        }
        catch (Exception ex)
        {
            if (DynamicSubsidiaryTimelinePlugin.log != null)
                DynamicSubsidiaryTimelinePlugin.log.LogError("[DynamicTimeline] Settings SetData error: " + ex);
        }
        return false;
    }
}

// ── Settings UpdateData ──────────────────────────────

[HarmonyPatch(typeof(Menu_Stats_TochterfirmaSettings), "UpdateData")]
public static class Menu_Stats_TochterfirmaSettings_UpdateData_Patch
{
    [HarmonyPrefix]
    static bool Prefix(Menu_Stats_TochterfirmaSettings __instance)
    {
        try
        {
            var pS = __instance.pS_;
            var tS = DynamicSubsidiaryTimelinePlugin.setTSField?.GetValue(__instance) as textScript;
            if (pS == null || tS == null) return false;
            var ui = __instance.uiObjects;
            if (ui == null) return false;

            DynamicSubsidiaryTimelinePlugin.SafeSetText(ui, 24,
                pS.tf_gameTopic != -1 ? "<b>" + tS.GetThemes(pS.tf_gameTopic) + "</b>" : tS.GetText(1966));

            if (pS.tf_ipFocus != null)
            {
                for (int i = 0; i < pS.tf_ipFocus.Length; i++)
                {
                    int index = 47 + i;
                    if (pS.tf_ipFocus[i] != -1)
                    {
                        var gameObj = GameObject.Find("GAME_" + pS.tf_ipFocus[i]);
                        if (gameObj != null)
                        {
                            var gameComp = gameObj.GetComponent<gameScript>();
                            if (gameComp != null)
                            {
                                DynamicSubsidiaryTimelinePlugin.SafeSetText(ui, index, "<b>" + gameComp.GetIpName() + "</b>");
                                continue;
                            }
                        }
                        pS.tf_ipFocus[i] = -1;
                    }
                    DynamicSubsidiaryTimelinePlugin.SafeSetText(ui, index, tS.GetText(1966));
                }
            }

            if (pS.tf_engine != -1 && pS.tf_engine != 0)
            {
                var engObj = GameObject.Find("ENGINE_" + pS.tf_engine);
                if (engObj != null)
                {
                    var engComp = engObj.GetComponent<engineScript>();
                    if (engComp != null)
                        DynamicSubsidiaryTimelinePlugin.SafeSetText(ui, 31, "<b>" + engComp.GetName() + "</b>");
                }
            }
            else
                DynamicSubsidiaryTimelinePlugin.SafeSetText(ui, 31, tS.GetText(1966));

            if (pS.tf_platformFocus != null)
            {
                for (int j = 0; j < pS.tf_platformFocus.Length; j++)
                {
                    int index = 37 + j;
                    if (pS.tf_platformFocus[j] != -1)
                    {
                        var platObj = GameObject.Find("PLATFORM_" + pS.tf_platformFocus[j]);
                        if (platObj != null)
                        {
                            var platComp = platObj.GetComponent<platformScript>();
                            if (platComp != null)
                            {
                                DynamicSubsidiaryTimelinePlugin.SafeSetText(ui, index,
                                    platComp.vomMarktGenommen
                                        ? "<b><color=red>" + platComp.GetName() + "</color></b>"
                                        : "<b>" + platComp.GetName() + "</b>");
                                continue;
                            }
                        }
                        pS.tf_platformFocus[j] = -1;
                    }
                    DynamicSubsidiaryTimelinePlugin.SafeSetText(ui, index, tS.GetText(1966));
                }
            }

            bool prioritySet = false;
            if (pS.tf_ownPublisherPriority != -1)
            {
                var pubObj = GameObject.Find("PUB_" + pS.tf_ownPublisherPriority);
                if (pubObj != null)
                {
                    var pubComp = pubObj.GetComponent<publisherScript>();
                    if (pubComp != null && pubComp.IsMyTochterfirma())
                    {
                        DynamicSubsidiaryTimelinePlugin.SafeSetText(ui, 45, "<b>" + pubComp.GetName() + "</b>");
                        prioritySet = true;
                    }
                }
            }
            if (!prioritySet)
            {
                pS.tf_ownPublisherPriority = -1;
                DynamicSubsidiaryTimelinePlugin.SafeSetText(ui, 45, tS.GetText(1966));
            }
        }
        catch (Exception ex)
        {
            if (DynamicSubsidiaryTimelinePlugin.log != null)
                DynamicSubsidiaryTimelinePlugin.log.LogError("[DynamicTimeline] Settings UpdateData error: " + ex);
        }
        return false;
    }
}

// ── Settings LoadCopyToggles ─────────────────────────

[HarmonyPatch(typeof(Menu_Stats_TochterfirmaSettings), "LoadCopyToggles")]
public static class Menu_Stats_TochterfirmaSettings_LoadCopyToggles_Patch
{
    [HarmonyPrefix]
    static bool Prefix(Menu_Stats_TochterfirmaSettings __instance)
    {
        try
        {
            var copyToggles = __instance.copyToggles;
            if (copyToggles == null) return false;
            for (int i = 0; i < copyToggles.Length; i++)
            {
                if (copyToggles[i] == null) continue;
                if (PlayerPrefs.HasKey(copyToggles[i].name))
                    copyToggles[i].isOn = PlayerPrefs.GetInt(copyToggles[i].name) != 0;
            }
        }
        catch (Exception ex)
        {
            if (DynamicSubsidiaryTimelinePlugin.log != null)
                DynamicSubsidiaryTimelinePlugin.log.LogError("[DynamicTimeline] Settings LoadCopyToggles error: " + ex);
        }
        return false;
    }
}

// ── Settings Update ──────────────────────────────────

[HarmonyPatch(typeof(Menu_Stats_TochterfirmaSettings), "Update")]
public static class Menu_Stats_TochterfirmaSettings_Update_Patch
{
    [HarmonyPrefix]
    static bool Prefix(Menu_Stats_TochterfirmaSettings __instance)
    {
        try
        {
            var ui = __instance.uiObjects;
            if (ui == null) return false;

            if (ui.Length > 2 && ui[2] != null && ui.Length > 3 && ui[3] != null)
            {
                var anim = ui[2].GetComponent<Animation>();
                var scroll = ui[3].GetComponent<Scrollbar>();
                if (anim != null && scroll != null && anim.IsPlaying("openMenu"))
                    scroll.value = 1f;
            }

            bool toggle11IsOn = false;
            if (ui.Length > 11 && ui[11] != null)
            {
                var t11 = ui[11].GetComponent<Toggle>();
                if (t11 != null) toggle11IsOn = t11.isOn;
            }

            if (toggle11IsOn)
            {
                DynamicSubsidiaryTimelinePlugin.SafeSetToggleIsOn(ui, 15, true);
                DynamicSubsidiaryTimelinePlugin.SafeSetToggleIsOn(ui, 16, true);
                DynamicSubsidiaryTimelinePlugin.SafeSetToggleIsOn(ui, 17, true);
                DynamicSubsidiaryTimelinePlugin.SafeSetToggleIsOn(ui, 18, true);
                DynamicSubsidiaryTimelinePlugin.SafeSetToggleInteractable(ui, 15, false);
                DynamicSubsidiaryTimelinePlugin.SafeSetToggleInteractable(ui, 16, false);
                DynamicSubsidiaryTimelinePlugin.SafeSetToggleInteractable(ui, 17, false);
                DynamicSubsidiaryTimelinePlugin.SafeSetToggleInteractable(ui, 18, false);
                DynamicSubsidiaryTimelinePlugin.SafeSetButtonInteractable(ui, 33, false);
                DynamicSubsidiaryTimelinePlugin.SafeSetButtonInteractable(ui, 34, false);
                DynamicSubsidiaryTimelinePlugin.SafeSetButtonInteractable(ui, 35, false);
                DynamicSubsidiaryTimelinePlugin.SafeSetButtonInteractable(ui, 36, false);
            }
            else
            {
                DynamicSubsidiaryTimelinePlugin.SafeSetToggleInteractable(ui, 15, true);
                DynamicSubsidiaryTimelinePlugin.SafeSetToggleInteractable(ui, 16, true);
                DynamicSubsidiaryTimelinePlugin.SafeSetToggleInteractable(ui, 17, true);
                DynamicSubsidiaryTimelinePlugin.SafeSetToggleInteractable(ui, 18, true);
                DynamicSubsidiaryTimelinePlugin.SafeSetButtonInteractable(ui, 33, true);
                DynamicSubsidiaryTimelinePlugin.SafeSetButtonInteractable(ui, 34, true);
                DynamicSubsidiaryTimelinePlugin.SafeSetButtonInteractable(ui, 35, true);
                DynamicSubsidiaryTimelinePlugin.SafeSetButtonInteractable(ui, 36, true);
            }

            bool toggle10IsOn = false;
            if (ui.Length > 10 && ui[10] != null)
            {
                var t10 = ui[10].GetComponent<Toggle>();
                if (t10 != null) toggle10IsOn = t10.isOn;
            }
            DynamicSubsidiaryTimelinePlugin.SafeSetDropdownInteractable(ui, 32, toggle10IsOn);

            bool toggle23IsOn = false;
            if (ui.Length > 23 && ui[23] != null)
            {
                var t23 = ui[23].GetComponent<Toggle>();
                if (t23 != null) toggle23IsOn = t23.isOn;
            }
            DynamicSubsidiaryTimelinePlugin.SafeSetButtonInteractable(ui, 44, toggle23IsOn);
        }
        catch { }
        return false;
    }
}

// ════════════════════════════════════════════════════════
//  Upgrade & Action Button Patches (Unified)
// ════════════════════════════════════════════════════════

// ── Helper: Open an upgrade/settings menu ────────────

public partial class DynamicSubsidiaryTimelinePlugin
{
    internal static bool OpenUpgradeMenu(Menu_Stats_Tochterfirma_Main __instance, int guiObjectIndex)
    {
        try
        {
            var pS = menuPSField?.GetValue(__instance) as publisherScript;
            if (pS == null) return false;

            var sfx = menuSfxField?.GetValue(__instance) as sfxScript;
            if (sfx != null) sfx.PlaySound(3, force: true);

            var guiMain = menuGuiMainField?.GetValue(__instance) as GUI_Main;
            if (guiMain == null) guiMain = UnityEngine.Object.FindObjectOfType<GUI_Main>();
            if (guiMain == null) return false;

            GameObject menuObj = null;
            if (guiMain.uiObjects != null && guiMain.uiObjects.Length > guiObjectIndex)
                menuObj = guiMain.uiObjects[guiObjectIndex];

            if (menuObj == null)
            {
                switch (guiObjectIndex)
                {
                    case 388:
                        var m1 = UnityEngine.Object.FindObjectOfType<Menu_W_FirmaAufwerten>(true);
                        if (m1 != null) menuObj = m1.gameObject;
                        break;
                    case 389:
                        var m2 = UnityEngine.Object.FindObjectOfType<Menu_W_FirmaAufwertenPublisher>(true);
                        if (m2 != null) menuObj = m2.gameObject;
                        break;
                    case 390:
                        var m3 = UnityEngine.Object.FindObjectOfType<Menu_W_FirmaAufwertenDeveloper>(true);
                        if (m3 != null) menuObj = m3.gameObject;
                        break;
                }
            }

            if (menuObj != null)
            {
                guiMain.ActivateMenu(menuObj);
                switch (guiObjectIndex)
                {
                    case 388:
                        var c1 = menuObj.GetComponent<Menu_W_FirmaAufwerten>();
                        if (c1 != null) c1.Init(pS);
                        break;
                    case 389:
                        var c2 = menuObj.GetComponent<Menu_W_FirmaAufwertenPublisher>();
                        if (c2 != null) c2.Init(pS);
                        break;
                    case 390:
                        var c3 = menuObj.GetComponent<Menu_W_FirmaAufwertenDeveloper>();
                        if (c3 != null) c3.Init(pS);
                        break;
                }
            }
            else if (log != null)
                log.LogError($"[DynamicTimeline] Could not find upgrade menu GameObject at index {guiObjectIndex}");
        }
        catch (Exception ex)
        {
            if (log != null) log.LogError($"[DynamicTimeline] OpenUpgradeMenu error: {ex}");
        }
        return false;
    }
}

// ── 3 Upgrade Button Patches → 1 shared helper ──────

[HarmonyPatch(typeof(Menu_Stats_Tochterfirma_Main), "BUTTON_FirmaAufwerten")]
public static class Menu_Stats_Tochterfirma_Main_BUTTON_FirmaAufwerten_Patch
{
    [HarmonyPrefix]
    static bool Prefix(Menu_Stats_Tochterfirma_Main __instance) =>
        DynamicSubsidiaryTimelinePlugin.OpenUpgradeMenu(__instance, 388);
}

[HarmonyPatch(typeof(Menu_Stats_Tochterfirma_Main), "BUTTON_FirmaAufwertenPublisher")]
public static class Menu_Stats_Tochterfirma_Main_BUTTON_FirmaAufwertenPublisher_Patch
{
    [HarmonyPrefix]
    static bool Prefix(Menu_Stats_Tochterfirma_Main __instance) =>
        DynamicSubsidiaryTimelinePlugin.OpenUpgradeMenu(__instance, 389);
}

[HarmonyPatch(typeof(Menu_Stats_Tochterfirma_Main), "BUTTON_FirmaAufwertenDeveloper")]
public static class Menu_Stats_Tochterfirma_Main_BUTTON_FirmaAufwertenDeveloper_Patch
{
    [HarmonyPrefix]
    static bool Prefix(Menu_Stats_Tochterfirma_Main __instance) =>
        DynamicSubsidiaryTimelinePlugin.OpenUpgradeMenu(__instance, 390);
}

// ── Settings Button ──────────────────────────────────

[HarmonyPatch(typeof(Menu_Stats_Tochterfirma_Main), "BUTTON_Settings")]
public static class Menu_Stats_Tochterfirma_Main_BUTTON_Settings_Patch
{
    [HarmonyPrefix]
    static bool Prefix(Menu_Stats_Tochterfirma_Main __instance)
    {
        try
        {
            var pS = DynamicSubsidiaryTimelinePlugin.menuPSField?.GetValue(__instance) as publisherScript;
            if (pS == null) return false;

            var sfx = DynamicSubsidiaryTimelinePlugin.menuSfxField?.GetValue(__instance) as sfxScript;
            if (sfx != null) sfx.PlaySound(3, force: true);

            var guiMain = DynamicSubsidiaryTimelinePlugin.menuGuiMainField?.GetValue(__instance) as GUI_Main;
            if (guiMain == null) guiMain = UnityEngine.Object.FindObjectOfType<GUI_Main>();
            if (guiMain == null) return false;

            GameObject menuObj = null;
            if (guiMain.uiObjects != null && guiMain.uiObjects.Length > 393)
                menuObj = guiMain.uiObjects[393];

            if (menuObj == null)
            {
                var menuComp = UnityEngine.Object.FindObjectOfType<Menu_Stats_TochterfirmaSettings>(true);
                if (menuComp != null) menuObj = menuComp.gameObject;
            }

            if (menuObj != null)
            {
                guiMain.ActivateMenu(menuObj);
                var menuComp = menuObj.GetComponent<Menu_Stats_TochterfirmaSettings>();
                if (menuComp != null) menuComp.Init(pS);
            }
            else if (DynamicSubsidiaryTimelinePlugin.log != null)
                DynamicSubsidiaryTimelinePlugin.log.LogError("[DynamicTimeline] Could not find Settings GameObject!");
        }
        catch (Exception ex)
        {
            if (DynamicSubsidiaryTimelinePlugin.log != null)
                DynamicSubsidiaryTimelinePlugin.log.LogError("[DynamicTimeline] BUTTON_Settings error: " + ex);
        }
        return false;
    }
}

// ── 3 BUTTON_Yes Patches → 1 shared helper ──────────

public partial class DynamicSubsidiaryTimelinePlugin
{
    internal static void OnUpgradeYesPrefix(publisherScript studio)
    {
        if (studio == null) return;
        preUpgradeStars = studio.GetStarsAmount();
        preUpgradeSpeed = studio.developmentSpeed;
        preUpgradeIsStudioLevel = true;
    }

    internal static void OnUpgradeYesPostfix(publisherScript studio)
    {
        if (studio == null) return;
        RecalculateActiveProjectTimeline(studio);
        RefreshSubsidiaryDevUI(studio);
    }
}

// ── Menu_W_FirmaAufwerten ────────────────────────────

[HarmonyPatch(typeof(Menu_W_FirmaAufwerten), "BUTTON_Yes")]
public static class Menu_W_FirmaAufwerten_BUTTON_Yes_Patch
{
    [HarmonyPrefix]
    static void Prefix(Menu_W_FirmaAufwerten __instance)
    {
        try
        {
            var psField = AccessTools.Field(typeof(Menu_W_FirmaAufwerten), "pS_");
            var studio = psField?.GetValue(__instance) as publisherScript;
            DynamicSubsidiaryTimelinePlugin.OnUpgradeYesPrefix(studio);
        }
        catch (Exception ex)
        {
            if (DynamicSubsidiaryTimelinePlugin.log != null)
                DynamicSubsidiaryTimelinePlugin.log.LogError("[DynamicTimeline] Aufwerten Yes Prefix: " + ex);
        }
    }

    [HarmonyPostfix]
    static void Postfix(Menu_W_FirmaAufwerten __instance)
    {
        try
        {
            var psField = AccessTools.Field(typeof(Menu_W_FirmaAufwerten), "pS_");
            var studio = psField?.GetValue(__instance) as publisherScript;
            DynamicSubsidiaryTimelinePlugin.OnUpgradeYesPostfix(studio);
        }
        catch (Exception ex)
        {
            if (DynamicSubsidiaryTimelinePlugin.log != null)
                DynamicSubsidiaryTimelinePlugin.log.LogError("[DynamicTimeline] Aufwerten Yes Postfix: " + ex);
        }
    }
}

// ── Menu_W_FirmaAufwertenPublisher ───────────────────

[HarmonyPatch(typeof(Menu_W_FirmaAufwertenPublisher), "BUTTON_Yes")]
public static class Menu_W_FirmaAufwertenPublisher_BUTTON_Yes_Patch
{
    [HarmonyPrefix]
    static void Prefix(Menu_W_FirmaAufwertenPublisher __instance)
    {
        try
        {
            var psField = AccessTools.Field(typeof(Menu_W_FirmaAufwertenPublisher), "pS_");
            var studio = psField?.GetValue(__instance) as publisherScript;
            DynamicSubsidiaryTimelinePlugin.OnUpgradeYesPrefix(studio);
        }
        catch (Exception ex)
        {
            if (DynamicSubsidiaryTimelinePlugin.log != null)
                DynamicSubsidiaryTimelinePlugin.log.LogError("[DynamicTimeline] Publisher Yes Prefix: " + ex);
        }
    }

    [HarmonyPostfix]
    static void Postfix(Menu_W_FirmaAufwertenPublisher __instance)
    {
        try
        {
            var psField = AccessTools.Field(typeof(Menu_W_FirmaAufwertenPublisher), "pS_");
            var studio = psField?.GetValue(__instance) as publisherScript;
            DynamicSubsidiaryTimelinePlugin.OnUpgradeYesPostfix(studio);
        }
        catch (Exception ex)
        {
            if (DynamicSubsidiaryTimelinePlugin.log != null)
                DynamicSubsidiaryTimelinePlugin.log.LogError("[DynamicTimeline] Publisher Yes Postfix: " + ex);
        }
    }
}

// ── Menu_W_FirmaAufwertenDeveloper ───────────────────

[HarmonyPatch(typeof(Menu_W_FirmaAufwertenDeveloper), "BUTTON_Yes")]
public static class Menu_W_FirmaAufwertenDeveloper_BUTTON_Yes_Patch
{
    [HarmonyPrefix]
    static void Prefix(Menu_W_FirmaAufwertenDeveloper __instance)
    {
        try
        {
            var psField = AccessTools.Field(typeof(Menu_W_FirmaAufwertenDeveloper), "pS_");
            var studio = psField?.GetValue(__instance) as publisherScript;
            DynamicSubsidiaryTimelinePlugin.OnUpgradeYesPrefix(studio);
        }
        catch (Exception ex)
        {
            if (DynamicSubsidiaryTimelinePlugin.log != null)
                DynamicSubsidiaryTimelinePlugin.log.LogError("[DynamicTimeline] Developer Yes Prefix: " + ex);
        }
    }

    [HarmonyPostfix]
    static void Postfix(Menu_W_FirmaAufwertenDeveloper __instance)
    {
        try
        {
            var psField = AccessTools.Field(typeof(Menu_W_FirmaAufwertenDeveloper), "pS_");
            var studio = psField?.GetValue(__instance) as publisherScript;
            DynamicSubsidiaryTimelinePlugin.OnUpgradeYesPostfix(studio);
        }
        catch (Exception ex)
        {
            if (DynamicSubsidiaryTimelinePlugin.log != null)
                DynamicSubsidiaryTimelinePlugin.log.LogError("[DynamicTimeline] Developer Yes Postfix: " + ex);
        }
    }
}
