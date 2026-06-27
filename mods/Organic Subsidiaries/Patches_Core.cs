using HarmonyLib;
using UnityEngine;

[HarmonyPatch(typeof(publisherScript), "IsMyTochterfirma")]
public static class OrganicSubsidiaryIsMyTochterfirmaPatch
{
    public static bool Prefix(publisherScript __instance, ref bool __result)
    {
        if (__instance == null || !__instance)
        {
            __result = false;
            return false;
        }

        try
        {
            mainScript mainScript = __instance.mS_ ?? Subsidiary2Plugin.GetMainScript();
            if (mainScript == null) { __result = false; return false; }
            if (__instance.isPlayer) { __result = false; return false; }
            if (__instance.myID >= 100000) { __result = false; return false; }
            __result = (__instance.ownerID == mainScript.myID);
        }
        catch { __result = false; }
        return false;
    }
}

[HarmonyPatch(typeof(publisherScript), "GetName")]
public static class OrganicSubsidiaryGetNamePatch
{
    public static bool Prefix(publisherScript __instance, ref string __result)
    {
        try
        {
            if (__instance != null && Subsidiary2Plugin.IsOrganicStudio(__instance))
            {
                __result = !string.IsNullOrEmpty(__instance.name_EN) ? __instance.name_EN : "Organic Studio";
                return false;
            }
        }
        catch
        {
            if (__instance != null && Subsidiary2Plugin.IsOrganicStudio(__instance))
            {
                __result = "Organic Studio";
                return false;
            }
        }
        return true;
    }
}

[HarmonyPatch(typeof(publisherScript), "GetLogo")]
public static class OrganicSubsidiaryGetLogoPatch
{
    public static bool Prefix(publisherScript __instance, ref Sprite __result)
    {
        try
        {
            if (__instance != null && Subsidiary2Plugin.IsOrganicStudio(__instance))
            {
                GUI_Main guiMain = __instance.guiMain_ ?? Subsidiary2Plugin.GetGuiMain();
                if (guiMain != null && guiMain.logoSprites != null && __instance.logoID >= 0 && __instance.logoID < guiMain.logoSprites.Length)
                {
                    __result = guiMain.logoSprites[__instance.logoID];
                    return false;
                }
            }
        }
        catch
        {
            if (__instance != null && Subsidiary2Plugin.IsOrganicStudio(__instance))
            {
                __result = null;
                return false;
            }
        }
        return true;
    }
}

[HarmonyPatch(typeof(publisherScript), "GetTooltip")]
public static class OrganicSubsidiaryGetTooltipPatch
{
    public static bool Prefix(publisherScript __instance, ref string __result)
    {
        try
        {
            if (__instance != null && Subsidiary2Plugin.IsOrganicStudio(__instance))
            {
                mainScript mS = __instance.mS_ ?? Subsidiary2Plugin.GetMainScript();
                textScript tS = __instance.tS_ ?? (mS != null ? mS.tS_ : Subsidiary2Plugin.GetTextScript());
                __result = Subsidiary2Plugin.GetSafeOrganicTooltip(__instance, mS, tS);
                return false;
            }
        }
        catch
        {
            if (__instance != null && Subsidiary2Plugin.IsOrganicStudio(__instance))
            {
                __result = "";
                return false;
            }
        }
        return true;
    }
}

[HarmonyPatch(typeof(publisherScript), "GetDeveloperPublisherString")]
public static class OrganicSubsidiaryDevPubStringPatch
{
    public static bool Prefix(publisherScript __instance, ref string __result)
    {
        try
        {
            if (__instance != null && Subsidiary2Plugin.IsOrganicStudio(__instance))
            {
                textScript tS = __instance.tS_ ?? Subsidiary2Plugin.GetTextScript();
                if (__instance.developer && !__instance.publisher)
                    __result = tS != null ? tS.GetText(274) : "Developer";
                else if (!__instance.developer && __instance.publisher)
                    __result = tS != null ? tS.GetText(432) : "Publisher";
                else
                    __result = (tS != null ? tS.GetText(432) : "Publisher") + " & " + (tS != null ? tS.GetText(274) : "Developer");
                return false;
            }
        }
        catch
        {
            if (__instance != null && Subsidiary2Plugin.IsOrganicStudio(__instance))
            {
                __result = "Developer";
                return false;
            }
        }
        return true;
    }
}

[HarmonyPatch(typeof(tooltip), "Start")]
public static class SafeTooltipStartPatch
{
    public static bool Prefix(tooltip __instance)
    {
        try
        {
            GameObject main = AccessTools.Field(typeof(tooltip), "main_").GetValue(__instance) as GameObject;
            if (main == null)
            {
                main = GameObject.FindWithTag("Main");
                AccessTools.Field(typeof(tooltip), "main_").SetValue(__instance, main);
            }
            if (main == null) return false;

            mainScript mS = AccessTools.Field(typeof(tooltip), "mS_").GetValue(__instance) as mainScript;
            if (mS == null)
            {
                mS = main.GetComponent<mainScript>();
                AccessTools.Field(typeof(tooltip), "mS_").SetValue(__instance, mS);
            }
            if (mS == null) return false;

            if (mS != null && !mS.guiMain_) mS.FindScripts();

            textScript tS = AccessTools.Field(typeof(tooltip), "tS_").GetValue(__instance) as textScript;
            if (tS == null && mS != null)
            {
                tS = mS.tS_;
                AccessTools.Field(typeof(tooltip), "tS_").SetValue(__instance, tS);
            }

            settingsScript settings = AccessTools.Field(typeof(tooltip), "settings_").GetValue(__instance) as settingsScript;
            if (settings == null && mS != null)
            {
                settings = mS.settings_;
                AccessTools.Field(typeof(tooltip), "settings_").SetValue(__instance, settings);
            }

            GUI_Main guiMain = AccessTools.Field(typeof(tooltip), "guiMain_").GetValue(__instance) as GUI_Main;
            if (guiMain == null && mS != null)
            {
                guiMain = mS.guiMain_;
                AccessTools.Field(typeof(tooltip), "guiMain_").SetValue(__instance, guiMain);
            }
        }
        catch { }
        return true;
    }
}

[HarmonyPatch(typeof(setFont), "OnEnable")]
public static class SafeSetFontOnEnablePatch
{
    public static bool Prefix(setFont __instance)
    {
        try
        {
            GameObject main = AccessTools.Field(typeof(setFont), "main_").GetValue(__instance) as GameObject;
            if (main == null)
            {
                main = GameObject.FindGameObjectWithTag("Main");
                AccessTools.Field(typeof(setFont), "main_").SetValue(__instance, main);
            }
            if (main == null) return false;

            mainScript mS = AccessTools.Field(typeof(setFont), "mS_").GetValue(__instance) as mainScript;
            if (mS == null)
            {
                mS = main.GetComponent<mainScript>();
                AccessTools.Field(typeof(setFont), "mS_").SetValue(__instance, mS);
            }
            if (mS == null) return false;

            settingsScript settings = AccessTools.Field(typeof(setFont), "settings_").GetValue(__instance) as settingsScript;
            if (settings == null)
            {
                settings = main.GetComponent<settingsScript>();
                AccessTools.Field(typeof(setFont), "settings_").SetValue(__instance, settings);
            }
            if (settings == null) return false;
        }
        catch { }
        return true;
    }
}

[HarmonyPatch(typeof(setText), "OnEnable")]
public static class SafeSetTextOnEnablePatch
{
    public static bool Prefix(setText __instance)
    {
        try
        {
            GameObject main = AccessTools.Field(typeof(setText), "main_").GetValue(__instance) as GameObject;
            if (main == null)
            {
                main = GameObject.Find("Main");
                if (main == null) main = GameObject.FindGameObjectWithTag("Main");
                AccessTools.Field(typeof(setText), "main_").SetValue(__instance, main);
            }
            if (main == null) return false;

            textScript tS = AccessTools.Field(typeof(setText), "tS_").GetValue(__instance) as textScript;
            if (tS == null)
            {
                tS = main.GetComponent<textScript>();
                AccessTools.Field(typeof(setText), "tS_").SetValue(__instance, tS);
            }
            if (tS == null) return false;

            settingsScript settings = AccessTools.Field(typeof(setText), "settings_").GetValue(__instance) as settingsScript;
            if (settings == null)
            {
                settings = main.GetComponent<settingsScript>();
                AccessTools.Field(typeof(setText), "settings_").SetValue(__instance, settings);
            }
        }
        catch { }
        return true;
    }
}

[HarmonyPatch(typeof(platformScript), "SellPlayerKonsoleToNPC")]
public static class SafeSellPlayerKonsoleToNPCPatch
{
    public static void Prefix(platformScript __instance, publisherScript pS_)
    {
        try
        {
            if (__instance != null && pS_ != null)
                Subsidiary2Plugin.SafeCheckPublisherBuyedSize(__instance, pS_.myID);
        }
        catch { }
    }
}

[HarmonyPatch(typeof(publisherScript), "SelectPlayerPlatform")]
public static class SafeSelectPlayerPlatformPatch
{
    public static void Prefix(publisherScript __instance, platformScript script_)
    {
        try
        {
            if (__instance != null && script_ != null)
                Subsidiary2Plugin.SafeCheckPublisherBuyedSize(script_, __instance.myID);
        }
        catch { }
    }
}
