using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using UnityEngine;

[BepInPlugin("org.bepinex.plugins.dynamicstudiogoodwill", "Dynamic Studio Goodwill", "1.0.0")]
public partial class DynamicStudioGoodwillPlugin : BaseUnityPlugin
{
    private static ManualLogSource log;

    private void Awake()
    {
        log = Logger;
        new Harmony("org.bepinex.plugins.dynamicstudiogoodwill").PatchAll();
        log.LogInfo("Dynamic Studio Goodwill (Division Tags Only) loaded.");
    }

    [HarmonyPatch(typeof(publisherScript), "GetTooltip")]
    public static class DivisionTooltipPatch
    {
        public static void Postfix(publisherScript __instance, ref string __result)
        {
            if (__instance == null || !__instance) return;
            try
            {
                int subdiv = GetSubdivision(__instance);
                string name = GetSubdivisionName(subdiv);
                string color = GetSubdivisionColor(subdiv);
                if (__result == null) __result = "";
                __result += $"\n<color={color}><b>{name}</b></color>";
            }
            catch (Exception ex)
            {
                log.LogWarning("[Dynamic Goodwill] Tooltip hook failed: " + ex);
            }
        }
    }
}
