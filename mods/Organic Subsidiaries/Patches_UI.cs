using HarmonyLib;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public partial class Subsidiary2Plugin
{
    private static publisherScript[] _cachedPublishers;
    private static int _cachedPublishersFrame = -1;

    private static publisherScript[] GetAllPublishersCached()
    {
        int frame = Time.frameCount;
        if (_cachedPublishersFrame != frame)
        {
            _cachedPublishers = Object.FindObjectsOfType<publisherScript>();
            _cachedPublishersFrame = frame;
        }
        return _cachedPublishers;
    }

    private static GameObject[] _cachedGameObjects;
    private static int _cachedGameObjectsFrame = -1;

    private static GameObject[] GetAllGamesCached()
    {
        int frame = Time.frameCount;
        if (_cachedGameObjectsFrame != frame)
        {
            _cachedGameObjects = GameObject.FindGameObjectsWithTag("Game");
            _cachedGameObjectsFrame = frame;
        }
        return _cachedGameObjects;
    }

    internal static bool IsSubsidiaryGame(gameScript game)
    {
        if (game == null) return false;
        try
        {
            mainScript main = GetMainScript();
            if (main == null) return false;

            publisherScript[] publishers = GetAllPublishersCached();
            int[] ids = new int[] { game.ownerID, game.developerID, game.publisherID };
            for (int i = 0; i < ids.Length; i++)
            {
                int id = ids[i];
                if (id <= 0 || id == main.myID) continue;
                for (int j = 0; j < publishers.Length; j++)
                {
                    publisherScript pub = publishers[j];
                    if (pub != null && (bool)pub && pub.myID == id && pub.IsMyTochterfirma())
                        return true;
                }
            }
        }
        catch (System.Exception ex)
        {
            if (log != null) log.LogError("Error in IsSubsidiaryGame: " + ex);
        }
        return false;
    }

    internal static gameScript FindFirstSubsidiaryGame(bool includeOnMarketOrDev, bool includeDrawer)
    {
        try
        {
            mainScript main = GetMainScript();
            if (main == null) return null;

            GameObject[] gameObjects = GetAllGamesCached();
            for (int i = 0; i < gameObjects.Length; i++)
            {
                gameScript game = gameObjects[i]?.GetComponent<gameScript>();
                if (game == null || game.typ_contractGame) continue;

                bool statusMatch = false;
                if (includeOnMarketOrDev && (game.isOnMarket || game.inDevelopment)) statusMatch = true;
                if (includeDrawer && game.schublade) statusMatch = true;
                if (!statusMatch) continue;

                if (IsSubsidiaryGame(game)) return game;
            }
        }
        catch (System.Exception ex)
        {
            if (log != null) log.LogError("Error in FindFirstSubsidiaryGame: " + ex);
        }
        return null;
    }
}

[HarmonyPatch(typeof(Menu_W_FirmaVerkaufen), "Init")]
public static class OrganicSubsidiarySellWindowPatch
{
    public static bool Prefix(Menu_W_FirmaVerkaufen __instance, publisherScript script_)
    {
        try
        {
            if (Subsidiary2Plugin.BlockLockedOrganicSale(__instance, script_))
                return false;
        }
        catch { }
        return true;
    }
}

[HarmonyPatch(typeof(Menu_W_FirmaVerkaufen), "Init")]
public static class Menu_W_FirmaVerkaufen_Init_Patch
{
    public static void Postfix(Menu_W_FirmaVerkaufen __instance, publisherScript script_)
    {
        try
        {
            if (script_ != null && Subsidiary2Plugin.IsOrganicStudio(script_))
            {
                long customValue = Subsidiary2Plugin.GetOrganicSaleValue(script_);
                textScript tS = AccessTools.Field(typeof(Menu_W_FirmaVerkaufen), "tS_").GetValue(__instance) as textScript;
                mainScript mS = AccessTools.Field(typeof(Menu_W_FirmaVerkaufen), "mS_").GetValue(__instance) as mainScript;

                if (tS != null && mS != null)
                {
                    string text = tS.GetText(1974);
                    text = text.Replace("<NAME>", "<color=blue>" + script_.GetName() + "</color>");
                    text = text.Replace("<NUM>", "<color=blue>" + mS.GetMoney(customValue, showDollar: true) + "</color>");

                    string trend = Subsidiary2Plugin.GetGoodwillTrendLabel(script_.myID);
                    if (!string.IsNullOrEmpty(trend))
                    {
                        if (trend == "Rising")
                            text += "\n\n<color=green>Negotiation: buyers are eager due to their <b>" + trend + "</b> status (+15% value)!</color>";
                        else if (trend == "Commercial Powerhouse")
                            text += "\n\n<color=green>Negotiation: buyers are extremely eager due to their <b>" + trend + "</b> status (+30% value)!</color>";
                        else if (trend == "Declining" || trend == "In Crisis")
                            text += "\n\n<color=red>Negotiation: buyers are lowballing due to their <b>" + trend + "</b> status (-20% value)!</color>";
                    }

                    if (__instance.uiObjects != null && __instance.uiObjects.Length > 0 && __instance.uiObjects[0] != null)
                    {
                        Text textComp = __instance.uiObjects[0].GetComponent<Text>();
                        if (textComp != null) textComp.text = text;
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            if (Subsidiary2Plugin.log != null) Subsidiary2Plugin.log.LogError("Error in Menu_W_FirmaVerkaufen_Init_Patch: " + ex);
        }
    }
}

[HarmonyPatch(typeof(Menu_W_FirmaVerkaufen), "BUTTON_Yes")]
public static class Menu_W_FirmaVerkaufen_BUTTON_Yes_Patch
{
    public static bool Prefix(Menu_W_FirmaVerkaufen __instance)
    {
        try
        {
            publisherScript pS = AccessTools.Field(typeof(Menu_W_FirmaVerkaufen), "pS_").GetValue(__instance) as publisherScript;
            if (Subsidiary2Plugin.BlockLockedOrganicSale(__instance, pS))
                return false;

            if (pS != null && Subsidiary2Plugin.IsOrganicStudio(pS))
            {
                mainScript mS = AccessTools.Field(typeof(Menu_W_FirmaVerkaufen), "mS_").GetValue(__instance) as mainScript;
                textScript tS = AccessTools.Field(typeof(Menu_W_FirmaVerkaufen), "tS_").GetValue(__instance) as textScript;
                games gamesScript = AccessTools.Field(typeof(Menu_W_FirmaVerkaufen), "games_").GetValue(__instance) as games;
                gamepassScript gpS = AccessTools.Field(typeof(Menu_W_FirmaVerkaufen), "gpS_").GetValue(__instance) as gamepassScript;
                GUI_Main guiMain = AccessTools.Field(typeof(Menu_W_FirmaVerkaufen), "guiMain_").GetValue(__instance) as GUI_Main;

                if (mS != null && tS != null && gamesScript != null && gpS != null && guiMain != null)
                {
                    long customValue = Subsidiary2Plugin.GetOrganicSaleValue(pS);
                    mS.Earn(customValue, 12);
                    pS.RemoveTochterfirma();
                    pS.ResetTochterfirmaSettings();

                    if (mS.multiplayer)
                    {
                        if (mS.mpCalls_.isServer)
                        {
                            mS.mpCalls_.SERVER_Send_PublisherTochterfirmaSettings(pS);
                            mS.mpCalls_.SERVER_Send_Publisher(pS);
                        }
                        else
                        {
                            mS.mpCalls_.CLIENT_Send_PublisherTochterfirmaSettings(pS);
                            mS.mpCalls_.CLIENT_Send_Publisher(pS);
                        }
                    }

                    int num = 0;
                    for (int i = 0; i < gamesScript.arrayGamesScripts.Length; i++)
                    {
                        if ((bool)gamesScript.arrayGamesScripts[i] && gamesScript.arrayGamesScripts[i].IsMyIP(pS) && gamesScript.arrayGamesScripts[i].inGamePass)
                        {
                            gpS.GAMEPASS_RemoveGame(gamesScript.arrayGamesScripts[i], updateGamesAmount: false);
                            num++;
                        }
                    }
                    gpS.GetAmountGamePassGames();

                    if (num > 0)
                    {
                        string text = tS.GetText(2120);
                        text = text.Replace("<NUM>", num.ToString());
                        guiMain.MessageBox(text, closeMenu: false);
                    }

                    if (guiMain.uiObjects != null && guiMain.uiObjects.Length > 387 && guiMain.uiObjects[387] != null && guiMain.uiObjects[387].activeSelf)
                        guiMain.uiObjects[387].SetActive(false);

                    if (guiMain.uiObjects != null && guiMain.uiObjects.Length > 385 && guiMain.uiObjects[385] != null && guiMain.uiObjects[385].activeSelf)
                    {
                        Menu_Statistics_Tochterfirmen statsMenu = guiMain.uiObjects[385].GetComponent<Menu_Statistics_Tochterfirmen>();
                        if (statsMenu != null) statsMenu.BUTTON_Search();
                    }

                    __instance.BUTTON_Abbrechen();
                    return false;
                }
            }
        }
        catch (System.Exception ex)
        {
            if (Subsidiary2Plugin.log != null) Subsidiary2Plugin.log.LogError("Error in Menu_W_FirmaVerkaufen_BUTTON_Yes_Patch: " + ex);
        }
        return true;
    }
}

[HarmonyPatch(typeof(Item_Stats_Tochterfirma), "SetData")]
public static class Item_Stats_Tochterfirma_SetData_Patch
{
    public static void Postfix(Item_Stats_Tochterfirma __instance)
    {
        try
        {
            if (__instance.pS_ != null && Subsidiary2Plugin.IsOrganicStudio(__instance.pS_))
            {
                long customValue = Subsidiary2Plugin.GetOrganicSaleValue(__instance.pS_);
                if (__instance.uiObjects != null && __instance.uiObjects.Length > 4 && __instance.uiObjects[4] != null)
                {
                    Text textComp = __instance.uiObjects[4].GetComponent<Text>();
                    if (textComp != null) textComp.text = __instance.mS_.GetMoney(customValue, showDollar: true);
                }
            }
        }
        catch (System.Exception ex)
        {
            if (Subsidiary2Plugin.log != null) Subsidiary2Plugin.log.LogError("Error in Item_Stats_Tochterfirma_SetData_Patch: " + ex);
        }
    }
}

[HarmonyPatch(typeof(Menu_Stats_Tochterfirma_Main), "Update")]
public static class Menu_Stats_Tochterfirma_Main_Update_Patch
{
    private static readonly FieldInfo pSField = AccessTools.Field(typeof(Menu_Stats_Tochterfirma_Main), "pS_");
    private static readonly FieldInfo mSField = AccessTools.Field(typeof(Menu_Stats_Tochterfirma_Main), "mS_");
    private static readonly FieldInfo tSField = AccessTools.Field(typeof(Menu_Stats_Tochterfirma_Main), "tS_");

    public static void Postfix(Menu_Stats_Tochterfirma_Main __instance)
    {
        try
        {
            publisherScript pS = pSField?.GetValue(__instance) as publisherScript;
            if (pS != null && Subsidiary2Plugin.IsOrganicStudio(pS))
            {
                mainScript mS = mSField?.GetValue(__instance) as mainScript;
                textScript tS = tSField?.GetValue(__instance) as textScript;
                if (mS != null && tS != null)
                {
                    long customValue = Subsidiary2Plugin.GetOrganicSaleValue(pS);
                    if (__instance.uiObjects != null && __instance.uiObjects.Length > 7 && __instance.uiObjects[7] != null)
                    {
                        Text textComp = __instance.uiObjects[7].GetComponent<Text>();
                        if (textComp != null) textComp.text = tS.GetText(685) + ": <b>" + mS.GetMoney(customValue, showDollar: true) + "</b>";
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            if (Subsidiary2Plugin.log != null) Subsidiary2Plugin.log.LogError("Error in Menu_Stats_Tochterfirma_Main_Update_Patch: " + ex);
        }
    }
}

[HarmonyPatch(typeof(Menu_Stats_Tochterfirma_Main), "UpdateData")]
public static class Menu_Stats_Tochterfirma_Main_UpdateData_Patch
{
    private static readonly FieldInfo pSField = AccessTools.Field(typeof(Menu_Stats_Tochterfirma_Main), "pS_");
    private static readonly FieldInfo mSField = AccessTools.Field(typeof(Menu_Stats_Tochterfirma_Main), "mS_");
    private static readonly FieldInfo tSField = AccessTools.Field(typeof(Menu_Stats_Tochterfirma_Main), "tS_");

    public static void Postfix(Menu_Stats_Tochterfirma_Main __instance)
    {
        try
        {
            publisherScript pS = pSField?.GetValue(__instance) as publisherScript;
            if (pS != null && Subsidiary2Plugin.IsOrganicStudio(pS))
            {
                mainScript mS = mSField?.GetValue(__instance) as mainScript;
                textScript tS = tSField?.GetValue(__instance) as textScript;
                if (mS != null && tS != null)
                {
                    long customValue = Subsidiary2Plugin.GetOrganicSaleValue(pS);
                    if (__instance.uiObjects != null && __instance.uiObjects.Length > 7 && __instance.uiObjects[7] != null)
                    {
                        Text textComp = __instance.uiObjects[7].GetComponent<Text>();
                        if (textComp != null) textComp.text = tS.GetText(685) + ": <b>" + mS.GetMoney(customValue, showDollar: true) + "</b>";
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            if (Subsidiary2Plugin.log != null) Subsidiary2Plugin.log.LogError("Error in Menu_Stats_Tochterfirma_Main_UpdateData_Patch: " + ex);
        }
    }
}

[HarmonyPatch(typeof(Menu_W_FirmaAufwerten), "BUTTON_Yes")]
public static class OrganicSubsidiaryUpgradePatch
{
    public static void Prefix(Menu_W_FirmaAufwerten __instance, out (int preStars, int preSpeed)? __state)
    {
        __state = null;
        try
        {
            FieldInfo publisherField = AccessTools.Field(typeof(Menu_W_FirmaAufwerten), "pS_");
            FieldInfo mainField = AccessTools.Field(typeof(Menu_W_FirmaAufwerten), "mS_");
            publisherScript studio = publisherField?.GetValue(__instance) as publisherScript;
            mainScript mainScript = mainField?.GetValue(__instance) as mainScript;

            if (studio == null || !Subsidiary2Plugin.IsOrganicStudio(studio) || mainScript == null || __instance.costs == null)
                return;

            int starsAmount = studio.GetStarsAmount();
            if (starsAmount < 0 || starsAmount >= __instance.costs.Length) return;

            long upgradeCost = __instance.costs[starsAmount];
            if (mainScript.money >= upgradeCost)
            {
                __state = (starsAmount, studio.developmentSpeed);
                Subsidiary2Plugin.AddOrganicUpgradeInvestment(studio, upgradeCost);
            }
        }
        catch (System.Exception ex)
        {
            if (Subsidiary2Plugin.log != null) Subsidiary2Plugin.log.LogError("Error in OrganicSubsidiaryUpgradePatch Prefix: " + ex);
        }
    }

    public static void Postfix(Menu_W_FirmaAufwerten __instance, (int preStars, int preSpeed)? __state)
    {
        try
        {
            if (!__state.HasValue) return;
            FieldInfo publisherField = AccessTools.Field(typeof(Menu_W_FirmaAufwerten), "pS_");
            publisherScript studio = publisherField?.GetValue(__instance) as publisherScript;
            if (studio == null) return;

            System.Type timelineClass = System.Type.GetType("DynamicSubsidiaryTimelinePlugin, DynamicSubsidiaryTimeline");
            if (timelineClass == null)
            {
                foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
                {
                    timelineClass = assembly.GetType("DynamicSubsidiaryTimelinePlugin");
                    if (timelineClass != null) break;
                }
            }
            if (timelineClass != null)
            {
                var preStarsField = AccessTools.Field(timelineClass, "preUpgradeStars");
                var preSpeedField = AccessTools.Field(timelineClass, "preUpgradeSpeed");
                var preStudioField = AccessTools.Field(timelineClass, "preUpgradeIsStudioLevel");
                if (preStarsField != null) preStarsField.SetValue(null, __state.Value.preStars);
                if (preSpeedField != null) preSpeedField.SetValue(null, __state.Value.preSpeed);
                if (preStudioField != null) preStudioField.SetValue(null, true);

                var method = AccessTools.Method(timelineClass, "RecalculateActiveProjectTimeline");
                if (method != null) method.Invoke(null, new object[] { studio });
            }

            Menu_Stats_Tochterfirma_Main menu = Object.FindObjectOfType<Menu_Stats_Tochterfirma_Main>();
            if (menu != null && menu.gameObject.activeInHierarchy)
                menu.UpdateData();
        }
        catch (System.Exception ex)
        {
            if (Subsidiary2Plugin.log != null) Subsidiary2Plugin.log.LogError("Error in OrganicSubsidiaryUpgradePatch Postfix: " + ex);
        }
    }
}

[HarmonyPatch(typeof(Menu_MesseSelectGame), "CheckGameData")]
public static class MesseSelectGame_CheckGameData_Patch
{
    [HarmonyPrefix]
    private static bool Prefix(gameScript script_, ref bool __result)
    {
        try
        {
            if (script_ != null && !script_.typ_contractGame && (script_.inDevelopment || script_.isOnMarket || script_.schublade) && Subsidiary2Plugin.IsSubsidiaryGame(script_))
            {
                __result = true;
                return false;
            }
        }
        catch (System.Exception ex)
        {
            if (Subsidiary2Plugin.log != null) Subsidiary2Plugin.log.LogError("Error in MesseSelectGame_CheckGameData: " + ex);
        }
        return true;
    }
}

[HarmonyPatch(typeof(Menu_Marketing_SelectGame), "CheckGameData")]
public static class MarketingSelectGame_CheckGameData_Patch
{
    [HarmonyPrefix]
    private static bool Prefix(gameScript script_, ref bool __result)
    {
        try
        {
            if (script_ != null && !script_.typ_contractGame && (script_.inDevelopment || script_.isOnMarket || script_.schublade) && Subsidiary2Plugin.IsSubsidiaryGame(script_))
            {
                __result = true;
                return false;
            }
        }
        catch (System.Exception ex)
        {
            if (Subsidiary2Plugin.log != null) Subsidiary2Plugin.log.LogError("Error in MarketingSelectGame_CheckGameData: " + ex);
        }
        return true;
    }
}

[HarmonyPatch(typeof(Menu_Marketing_SpezialSelectGame), "CheckGameData")]
public static class MarketingSpezialSelectGame_CheckGameData_Patch
{
    [HarmonyPrefix]
    private static bool Prefix(gameScript script_, ref bool __result)
    {
        try
        {
            if (script_ != null && !script_.typ_contractGame && (script_.inDevelopment || script_.isOnMarket || script_.schublade) && Subsidiary2Plugin.IsSubsidiaryGame(script_))
            {
                __result = true;
                return false;
            }
        }
        catch (System.Exception ex)
        {
            if (Subsidiary2Plugin.log != null) Subsidiary2Plugin.log.LogError("Error in MarketingSpezialSelectGame_CheckGameData: " + ex);
        }
        return true;
    }
}

[HarmonyPatch(typeof(Menu_Marketing_GameKampagne), "FindGame")]
public static class MarketingGameKampagne_FindGame_Patch
{
    [HarmonyPostfix]
    private static void Postfix(ref gameScript __result)
    {
        try
        {
            if (__result != null) return;

            gameScript found = Subsidiary2Plugin.FindFirstSubsidiaryGame(true, false);
            if (found != null) { __result = found; return; }

            found = Subsidiary2Plugin.FindFirstSubsidiaryGame(false, true);
            if (found != null) __result = found;
        }
        catch (System.Exception ex)
        {
            if (Subsidiary2Plugin.log != null) Subsidiary2Plugin.log.LogError("Error in MarketingGameKampagne_FindGame: " + ex);
        }
    }
}
