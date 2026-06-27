using HarmonyLib;

[HarmonyPatch(typeof(Menu_Stats_Tochterfirma_Main), "Init")]
static class MenuStatsTochterfirmaInitPatch
{
    static void Postfix(Menu_Stats_Tochterfirma_Main __instance, publisherScript script_)
    {
        StudioDirectorPlugin.InjectButton(__instance, script_);
    }
}

[HarmonyPatch(typeof(Menu_Stats_Tochterfirma_Main), "UpdateData")]
static class MenuStatsTochterfirmaUpdateDataPatch
{
    static void Postfix(Menu_Stats_Tochterfirma_Main __instance)
    {
        StudioDirectorPlugin.RefreshButton(__instance);
    }
}

[HarmonyPatch(typeof(Menu_DevGameMain), "BUTTON_Abbrechen")]
static class DevGameMainCancelPatch
{
    static void Postfix()
    {
        StudioDirectorPlugin.CancelDesignContext();
    }
}

[HarmonyPatch(typeof(Menu_DevGame), "BUTTON_Start")]
static class DevGameStartPatch
{
    static void Prefix()
    {
        StudioDirectorPlugin.CaptureStartSnapshot();
    }

    static void Postfix()
    {
        StudioDirectorPlugin.ConvertCreatedGameToSubsidiaryProject();
    }
}

[HarmonyPatch(typeof(Menu_Dev_NachfolgerSelect), "CheckGameData")]
static class NachfolgerCheckPatch
{
    static bool Prefix(gameScript script_, ref bool __result)
    {
        if (!StudioDirectorPlugin.HasActiveSubsidiaryContext()) return true;
        __result = StudioDirectorPlugin.CheckNachfolgerForSubsidiary(script_);
        return false;
    }
}

[HarmonyPatch(typeof(Menu_Dev_RemasterSelect), "CheckGameData")]
static class RemasterCheckPatch
{
    static bool Prefix(gameScript script_, ref bool __result)
    {
        if (!StudioDirectorPlugin.HasActiveSubsidiaryContext()) return true;
        __result = StudioDirectorPlugin.CheckRemasterForSubsidiary(script_);
        return false;
    }
}

[HarmonyPatch(typeof(Menu_Dev_SpinoffSelect), "CheckGameData")]
static class SpinoffCheckPatch
{
    static bool Prefix(gameScript script_, ref bool __result)
    {
        if (!StudioDirectorPlugin.HasActiveSubsidiaryContext()) return true;
        __result = StudioDirectorPlugin.CheckSpinoffForSubsidiary(script_);
        return false;
    }
}

[HarmonyPatch(typeof(Menu_Dev_PortSelect), "CheckGameData")]
static class PortCheckPatch
{
    static bool Prefix(gameScript script_, ref bool __result)
    {
        if (!StudioDirectorPlugin.HasActiveSubsidiaryContext()) return true;
        __result = StudioDirectorPlugin.CheckPortForSubsidiary(script_);
        return false;
    }
}
