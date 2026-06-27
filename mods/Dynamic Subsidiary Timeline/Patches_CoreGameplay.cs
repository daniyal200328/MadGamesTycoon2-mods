using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;

// ════════════════════════════════════════════════════════
//  Core Gameplay Patches
// ════════════════════════════════════════════════════════

[HarmonyPatch(typeof(gameScript), "SetAsGameInDevelopmentNPC")]
public static class GameScript_SetAsGameInDevelopmentNPC_Patch
{
    [HarmonyPostfix]
    static void Postfix(gameScript __instance)
    {
        if (!DynamicSubsidiaryTimelinePlugin.cfgEnable.Value) return;
        var game = __instance;
        if (game == null) return;

        var studio = DynamicSubsidiaryTimelinePlugin.FindStudioByID(game.developerID);
        if (studio == null) return;

        bool isOrganic = DynamicSubsidiaryTimelinePlugin.IsOrganicStudio(studio);
        bool isAcquired = DynamicSubsidiaryTimelinePlugin.IsAcquiredSubsidiary(studio);
        if (!isOrganic && !isAcquired) return;
        if (isOrganic && !DynamicSubsidiaryTimelinePlugin.cfgApplyOrganic.Value) return;
        if (isAcquired && !DynamicSubsidiaryTimelinePlugin.cfgApplyAcquired.Value) return;

        int weeksInt = DynamicSubsidiaryTimelinePlugin.CalcTimeline(studio, game, out string debugInfo);

        DynamicSubsidiaryTimelinePlugin.ScaleSubsidiaryTimeline(studio, game, weeksInt);

        if (debugInfo != null && DynamicSubsidiaryTimelinePlugin.log != null)
            DynamicSubsidiaryTimelinePlugin.log.LogInfo("[DynamicTimeline NPC] " + debugInfo);
    }
}

[HarmonyPatch(typeof(publisherScript), "SetNewGameInWeeks")]
public static class PublisherScript_SetNewGameInWeeks_Patch
{
    [HarmonyPostfix]
    static void Postfix(publisherScript __instance, int force)
    {
        if (!DynamicSubsidiaryTimelinePlugin.cfgEnable.Value) return;
        var studio = __instance;
        if (studio == null) return;

        bool isOrganic = DynamicSubsidiaryTimelinePlugin.IsOrganicStudio(studio);
        bool isAcquired = DynamicSubsidiaryTimelinePlugin.IsAcquiredSubsidiary(studio);
        if (!isOrganic && !isAcquired) return;
        if (isOrganic && !DynamicSubsidiaryTimelinePlugin.cfgApplyOrganic.Value) return;
        if (isAcquired && !DynamicSubsidiaryTimelinePlugin.cfgApplyAcquired.Value) return;

        var game = DynamicSubsidiaryTimelinePlugin.SafeFindGameInDevelopment(studio);
        if (game == null) return;

        DynamicSubsidiaryTimelinePlugin.GetGameWeeks(studio, game, out float remaining, out float total);
        if (total > 0f && remaining < total - 0.01f) return;

        int weeksInt = DynamicSubsidiaryTimelinePlugin.CalcTimeline(studio, game, out string debugInfo);

        DynamicSubsidiaryTimelinePlugin.ScaleSubsidiaryTimeline(studio, game, weeksInt);

        if (debugInfo != null && DynamicSubsidiaryTimelinePlugin.log != null)
            DynamicSubsidiaryTimelinePlugin.log.LogInfo("[DynamicTimeline] " + debugInfo);
    }
}

[HarmonyPatch(typeof(publisherScript), "CreateNewGame2")]
public static class PublisherScript_CreateNewGame2_Patch
{
    [HarmonyPostfix]
    static void Postfix(publisherScript __instance)
    {
        if (__instance == null || !DynamicSubsidiaryTimelinePlugin.cfgEnable.Value) return;
        var studio = __instance;

        bool isOrganic = DynamicSubsidiaryTimelinePlugin.IsOrganicStudio(studio);
        bool isAcquired = DynamicSubsidiaryTimelinePlugin.IsAcquiredSubsidiary(studio);
        if (!isOrganic && !isAcquired) return;
        if (isOrganic && !DynamicSubsidiaryTimelinePlugin.cfgApplyOrganic.Value) return;
        if (isAcquired && !DynamicSubsidiaryTimelinePlugin.cfgApplyAcquired.Value) return;

        var game = DynamicSubsidiaryTimelinePlugin.SafeFindGameInDevelopment(studio);
        if (game == null) return;

        if (DynamicSubsidiaryTimelinePlugin.IsSlotManagedByTeamSlots(studio, game)) return;

        DynamicSubsidiaryTimelinePlugin.GetGameWeeks(studio, game, out float remaining, out float total);
        if (total > 0f && remaining < total - 0.01f) return;

        int weeksInt = DynamicSubsidiaryTimelinePlugin.CalcTimeline(studio, game, out string debugInfo);

        DynamicSubsidiaryTimelinePlugin.ScaleSubsidiaryTimeline(studio, game, weeksInt);

        try
        {
            var fieldAddon = typeof(publisherScript).GetField("nextGameAddon", BindingFlags.Instance | BindingFlags.NonPublic);
            var fieldMmoAddon = typeof(publisherScript).GetField("nextGameMMOAddon", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fieldAddon != null) fieldAddon.SetValue(studio, false);
            if (fieldMmoAddon != null) fieldMmoAddon.SetValue(studio, false);
        }
        catch (Exception ex)
        {
            if (DynamicSubsidiaryTimelinePlugin.log != null)
                DynamicSubsidiaryTimelinePlugin.log.LogError("[DynamicTimeline] Failed to reset nextGameAddon fields: " + ex);
        }

        if (debugInfo != null && DynamicSubsidiaryTimelinePlugin.log != null)
            DynamicSubsidiaryTimelinePlugin.log.LogInfo("[DynamicTimeline CNew2] " + debugInfo);
    }
}

[HarmonyPatch(typeof(publisherScript), "VerwaltungskostenBezahlen")]
public static class PublisherScript_VerwaltungskostenBezahlen_Patch
{
    [HarmonyPrefix]
    static bool Prefix(publisherScript __instance)
    {
        try
        {
            if (__instance == null) return true;
            if (__instance.mS_ == null)
                __instance.mS_ = UnityEngine.Object.FindObjectOfType<mainScript>();
            if (__instance.mS_ != null && __instance.IsMyTochterfirma() && !__instance.tf_geschlossen)
            {
                long cost = DynamicSubsidiaryTimelinePlugin.CalculateDynamicVerwaltungskosten(__instance);
                __instance.mS_.Pay(cost, 30);
            }
            return false;
        }
        catch (Exception ex)
        {
            if (DynamicSubsidiaryTimelinePlugin.log != null)
                DynamicSubsidiaryTimelinePlugin.log.LogError("[DynamicTimeline] VerwaltungskostenBezahlen error: " + ex);
            return true;
        }
    }
}

[HarmonyPatch(typeof(publisherScript), "GetTooltip")]
public static class PublisherScript_GetTooltip_Patch
{
    [HarmonyPostfix]
    static void Postfix(publisherScript __instance, ref string __result)
    {
        try
        {
            if (__instance == null || string.IsNullOrEmpty(__result)) return;
            bool isOrganic = DynamicSubsidiaryTimelinePlugin.IsOrganicStudio(__instance);
            bool isAcquired = DynamicSubsidiaryTimelinePlugin.IsAcquiredSubsidiary(__instance);
            if (!isOrganic && !isAcquired) return;

            var mS = __instance.mS_ ? __instance.mS_ : UnityEngine.Object.FindObjectOfType<mainScript>();
            var tS = __instance.tS_ ? __instance.tS_ : UnityEngine.Object.FindObjectOfType<textScript>();
            if (mS == null || tS == null) return;

            string searchPrefix = tS.GetText(1934) + ": <color=red><b>";
            int index = __result.IndexOf(searchPrefix);
            if (index >= 0)
            {
                int startIdx = index + searchPrefix.Length;
                int endIdx = __result.IndexOf("</b></color>", startIdx);
                if (endIdx >= 0)
                {
                    long cost = DynamicSubsidiaryTimelinePlugin.CalculateDynamicVerwaltungskosten(__instance);
                    string costStr = mS.GetMoney(cost, showDollar: true);
                    __result = __result.Substring(0, startIdx) + costStr + __result.Substring(endIdx);
                }
            }
        }
        catch (Exception ex)
        {
            if (DynamicSubsidiaryTimelinePlugin.log != null)
                DynamicSubsidiaryTimelinePlugin.log.LogError("[DynamicTimeline] GetTooltip error: " + ex);
        }
    }
}

