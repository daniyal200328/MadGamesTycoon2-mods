using HarmonyLib;
using System;
using UnityEngine;
using UnityEngine.UI;

// ════════════════════════════════════════════════════════
//  Menu_Statistics_Tochterfirmen Patches
// ════════════════════════════════════════════════════════

[HarmonyPatch(typeof(Menu_Statistics_Tochterfirmen), "SetData")]
public static class Menu_Statistics_Tochterfirmen_SetData_Patch
{
    [HarmonyPrefix]
    static bool Prefix(Menu_Statistics_Tochterfirmen __instance)
    {
        try
        {
            DynamicSubsidiaryTimelinePlugin.SafeMenuSetData(__instance);
            return false;
        }
        catch (Exception ex)
        {
            if (DynamicSubsidiaryTimelinePlugin.log != null)
                DynamicSubsidiaryTimelinePlugin.log.LogError("[DynamicTimeline] List SetData error: " + ex);
            return true;
        }
    }
}

[HarmonyPatch(typeof(Menu_Statistics_Tochterfirmen), "DROPDOWN_Sort")]
public static class Menu_Statistics_Tochterfirmen_DROPDOWN_Sort_Patch
{
    [HarmonyPrefix]
    static bool Prefix(Menu_Statistics_Tochterfirmen __instance)
    {
        try
        {
            DynamicSubsidiaryTimelinePlugin.SafeMenuDropdownSort(__instance);
            return false;
        }
        catch (Exception ex)
        {
            if (DynamicSubsidiaryTimelinePlugin.log != null)
                DynamicSubsidiaryTimelinePlugin.log.LogError("[DynamicTimeline] List sort error: " + ex);
            return true;
        }
    }
}

public partial class DynamicSubsidiaryTimelinePlugin
{
    internal static void SafeMenuSetData(Menu_Statistics_Tochterfirmen menu)
    {
        if (menu == null || menu.uiObjects == null || menu.uiObjects.Length < 8 || menu.uiPrefabs == null || menu.uiPrefabs.Length < 1) return;

        var mS = listMSField?.GetValue(menu) as mainScript;
        var guiMain = listGuiMainField?.GetValue(menu) as GUI_Main;
        var sfx = listSfxField?.GetValue(menu) as sfxScript;
        var tS = listTSField?.GetValue(menu) as textScript;
        var genres = listGenresField?.GetValue(menu) as genres;
        string searchStringA = listSearchStringAField?.GetValue(menu) as string ?? "";

        long totalAdminCost = 0L;
        var publishers = GameObject.FindGameObjectsWithTag("Publisher");

        var contentPanel = menu.uiObjects[0];
        var searchInput = menu.uiObjects[6];
        var emptyBanner = menu.uiObjects[4];
        var totalLabel = menu.uiObjects[7];
        if (contentPanel == null) return;

        string searchInputText = "";
        if (searchInput != null)
        {
            var inputComp = searchInput.GetComponent<InputField>();
            if (inputComp != null) searchInputText = inputComp.text;
        }
        searchInputText = searchInputText.ToLower();

        for (int i = 0; i < publishers.Length; i++)
        {
            var pubObj = publishers[i];
            if (pubObj == null) continue;
            var component = pubObj.GetComponent<publisherScript>();
            if (component != null && component.isUnlocked && component.IsMyTochterfirma())
            {
                string text = component.GetName().ToLower();
                if ((searchInputText.Length <= 0 || text.Contains(searchInputText)) && !SafeExists(contentPanel, component.myID))
                {
                    try
                    {
                        var spawned = UnityEngine.Object.Instantiate(menu.uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, contentPanel.transform);
                        if (spawned != null)
                        {
                            var component2 = spawned.GetComponent<Item_Stats_Tochterfirma>();
                            if (component2 != null)
                            {
                                component2.pS_ = component;
                                component2.playerID = -1;
                                component2.mS_ = mS;
                                component2.tS_ = tS;
                                component2.sfx_ = sfx;
                                component2.guiMain_ = guiMain;
                                component2.genres_ = genres;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (log != null) log.LogError("[DynamicTimeline] Error instantiating item: " + ex);
                    }
                }

                if (IsOrganicStudio(component) || IsAcquiredSubsidiary(component))
                    totalAdminCost += CalculateDynamicVerwaltungskosten(component);
                else
                    totalAdminCost += component.GetVerwaltungskosten();
            }
        }

        try { SafeMenuDropdownSort(menu); }
        catch (Exception ex) { if (log != null) log.LogError("[DynamicTimeline] Sort error: " + ex); }

        if (guiMain != null && emptyBanner != null)
            guiMain.KeinEintrag(contentPanel, emptyBanner);

        if (totalLabel != null && tS != null && mS != null)
        {
            var textLabel = totalLabel.GetComponent<Text>();
            if (textLabel != null)
                textLabel.text = tS.GetText(1934) + ": <b><color=red>" + mS.GetMoney(totalAdminCost, showDollar: true) + "</color></b>";
        }
    }

    internal static void SafeMenuDropdownSort(Menu_Statistics_Tochterfirmen menu)
    {
        if (menu == null || menu.uiObjects == null || menu.uiObjects.Length < 6) return;

        var dropdown = menu.uiObjects[5]?.GetComponent<Dropdown>();
        if (dropdown == null) return;

        int value = dropdown.value;
        PlayerPrefs.SetInt(menu.uiObjects[5].name, value);

        var contentPanel = menu.uiObjects[0];
        if (contentPanel == null) return;

        int childCount = contentPanel.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            var child = contentPanel.transform.GetChild(i);
            if (child == null) continue;
            var component = child.GetComponent<Item_Stats_Tochterfirma>();
            if (component != null && component.pS_ != null)
            {
                var pS = component.pS_;
                switch (value)
                {
                    case 0: child.gameObject.name = pS.GetName(); break;
                    case 1: child.gameObject.name = pS.stars.ToString(); break;
                    case 2: child.gameObject.name = SafeGetFirmenwert(pS).ToString(); break;
                    case 3: child.gameObject.name = pS.GetAmountGames().ToString(); break;
                    case 4: child.gameObject.name = pS.GetEntwicklungsFortschritt().ToString(); break;
                    case 5: child.gameObject.name = CalculateDynamicVerwaltungskosten(pS).ToString(); break;
                    case 6: child.gameObject.name = pS.tf_umsatz_allTime.ToString(); break;
                    case 7: child.gameObject.name = pS.GetTochterfirmaUmsatz24Monate().ToString(); break;
                    default: child.gameObject.name = pS.GetName(); break;
                }
            }
        }

        var mS = listMSField?.GetValue(menu) as mainScript;
        if (mS != null)
        {
            if (value == 0) mS.SortChildrenByName(contentPanel);
            else mS.SortChildrenByFloat(contentPanel);
        }
    }

    private static bool SafeExists(GameObject parent, int id)
    {
        if (parent == null) return false;
        int childCount = parent.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            var child = parent.transform.GetChild(i);
            if (child == null || !child.gameObject.activeSelf) continue;
            var item = child.GetComponent<Item_Stats_Tochterfirma>();
            if (item != null && item.pS_ != null && item.pS_.myID == id)
                return true;
        }
        return false;
    }
}
