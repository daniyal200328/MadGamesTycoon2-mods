using HarmonyLib;

// Hook publisherScript.SetGameOnMarket — this is called both for auto-releases and
// player-triggered releases via iWaitTochterfirmaReleaseGame coroutine.
[HarmonyPatch(typeof(publisherScript), "SetGameOnMarket")]
public static class PublisherScript_SetGameOnMarket_XP_Patch
{
    public static void Postfix(publisherScript __instance, gameScript script_)
    {
        if (__instance == null || script_ == null) return;
        if (!__instance.IsTochterfirma() || !__instance.IsMyTochterfirma()) return;
        try
        {
            SubsidiaryTeamsPlugin.TryFlushPendingAward(script_);
        }
        catch (System.Exception ex)
        {
            SubsidiaryTeamsPlugin.Log?.LogError($"Error awarding XP on SetGameOnMarket for game {script_.myID}: {ex}");
        }
    }
}

// Hook gameScript.SellGame — this is called weekly for active games on the market.
// We use this to check and trigger sales milestone XP rewards.
[HarmonyPatch(typeof(gameScript), "SellGame")]
public static class GameScript_SellGame_XP_Patch
{
    public static void Postfix(gameScript __instance)
    {
        if (__instance == null) return;
        try
        {
            SubsidiaryTeamsPlugin.CheckSalesMilestones(__instance);
        }
        catch (System.Exception ex)
        {
            SubsidiaryTeamsPlugin.Log?.LogError($"Error checking sales milestones on SellGame for game {__instance.myID}: {ex}");
        }
    }
}
