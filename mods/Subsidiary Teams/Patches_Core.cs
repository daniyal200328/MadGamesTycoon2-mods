using HarmonyLib;
using System.Reflection;
using UnityEngine;

[HarmonyPatch(typeof(savegameScript), "Save")]
public static class SavegameScript_Save_Patch
{
    public static void Postfix(int i)
    {
        SubsidiaryTeamsPlugin.SaveState(i);
    }
}

[HarmonyPatch(typeof(savegameScript), "Load")]
public static class SavegameScript_Load_Patch
{
    public static void Postfix(int i)
    {
        SubsidiaryTeamsPlugin.LoadState(i);
    }
}

[HarmonyPatch(typeof(mainScript), "Start")]
public static class MainScript_Start_Patch
{
    public static void Postfix()
    {
        SubsidiaryTeamsPlugin.currentSaveSlot = -1;
        SubsidiaryTeamsPlugin.State = new SubsidiaryTeamsPlugin.SlotSaveState();
        SubsidiaryTeamsPlugin.Log?.LogInfo("mainScript Start: Reset slot save state to fresh game settings.");
    }
}

[HarmonyPatch(typeof(publisherScript), "RemoveTochterfirma")]
public static class PublisherScript_RemoveTochterfirma_Patch
{
    public static void Prefix(publisherScript __instance)
    {
        if (__instance == null || !__instance) return;
        try
        {
            var data = SubsidiaryTeamsPlugin.GetStudioSlotData(__instance.myID);
            if (data != null && data.slots != null)
            {
                games gamesScript = __instance.games_ ?? SubsidiaryTeamsPlugin.GetGamesScript();
                for (int i = 0; i < data.slots.Length; i++)
                {
                    var slot = data.slots[i];
                    if (slot != null)
                    {
                        if (slot.gameID != -1)
                        {
                            gameScript game = SubsidiaryTeamsPlugin.FindGameByID(gamesScript, slot.gameID);
                            if (game != null)
                            {
                                game.inDevelopment = false;
                                game.gameObject.tag = "GameRemoved";
                                Object.Destroy(game.gameObject);
                            }
                        }
                        slot.gameID = -1;
                        slot.remainingWeeks = 0f;
                        slot.totalWeeks = 0f;
                        slot.isPlayerAssigned = false;
                        slot.isUnlocked = (i == 0);
                        slot.stars = 1;
                        slot.speed = 0;
                        slot.helpingSlotIndex = -1;
                    }
                }
                data.unlockedSlots = 1;
                data.closedYear = -1;
                data.closedMonth = -1;
                SubsidiaryTeamsPlugin.SaveState(SubsidiaryTeamsPlugin.currentSaveSlot);
            }
        }
        catch (System.Exception ex)
        {
            SubsidiaryTeamsPlugin.Log?.LogWarning("Error cleaning up sold subsidiary slots: " + ex);
        }
    }
}

[HarmonyPatch(typeof(publisherScript), "CreateNewGame2")]
public static class PublisherScript_CreateNewGame2_Patch
{
    public static bool Prefix(publisherScript __instance)
    {
        if (SubsidiaryTeamsPlugin.isBypassingPrefix) return true;
        if (__instance == null || !__instance) return true;

        try
        {
            if (__instance.IsTochterfirma() && __instance.IsMyTochterfirma())
            {
                SubsidiaryTeamsPlugin.TickActiveProjects(__instance);
                return false;
            }
        }
        catch { }
        return true;
    }
}

[HarmonyPatch(typeof(gameScript), "SetAsGameInDevelopmentNPC")]
public static class GameScript_SetAsGameInDevelopmentNPC_Patch
{
    private static float originalStars;
    private static int originalSpeed;
    private static int activeSlotIndex = -1;

    [HarmonyPrefix]
    public static void Prefix(gameScript __instance)
    {
        if (__instance == null || !__instance) return;

        int slotIdx = -1;
        if (SubsidiaryTeamsPlugin.isCreatingAutonomousSlot)
            slotIdx = SubsidiaryTeamsPlugin.targetSlotIndex;
        else if (SubsidiaryTeamsPlugin.isCreatingPlayerSlotProject)
            slotIdx = SubsidiaryTeamsPlugin.selectedSlotIndex;

        if (slotIdx >= 0 && slotIdx < 3)
        {
            publisherScript studio = SubsidiaryTeamsPlugin.FindStudioByID(__instance.developerID);
            if (studio != null)
            {
                var data = SubsidiaryTeamsPlugin.GetStudioSlotData(studio.myID);
                var slot = data.slots[slotIdx];

                originalStars = studio.stars;
                originalSpeed = studio.developmentSpeed;
                activeSlotIndex = slotIdx;

                if (slotIdx > 0)
                {
                    studio.stars = slot.stars * 20f;
                    studio.developmentSpeed = slot.speed;
                }

                SubsidiaryTeamsPlugin.currentInitializingGame = __instance;
            }
        }
    }

    [HarmonyPostfix]
    public static void Postfix(gameScript __instance)
    {
        try
        {
            if (activeSlotIndex >= 0)
            {
                publisherScript studio = SubsidiaryTeamsPlugin.FindStudioByID(__instance.developerID);
                if (studio != null)
                {
                    studio.stars = originalStars;
                    studio.developmentSpeed = originalSpeed;
                }
            }

            if (SubsidiaryTeamsPlugin.isCreatingAutonomousSlot)
            {
                int slotIdx = SubsidiaryTeamsPlugin.targetSlotIndex;
                if (slotIdx < 0 || slotIdx > 2) return;

                publisherScript studio = SubsidiaryTeamsPlugin.FindStudioByID(__instance.developerID);
                if (studio != null)
                {
                    var data = SubsidiaryTeamsPlugin.GetStudioSlotData(studio.myID);
                    for (int i = 0; i < 3; i++)
                    {
                        if (i != slotIdx && data.slots[i].gameID == __instance.myID) return;
                    }

                    float baseDuration = studio.newGameInWeeks;
                    if (baseDuration <= 2f) baseDuration = (studio.newGameInWeeksORG > 2f) ? studio.newGameInWeeksORG : 20f;
                    int finalWeeks = Mathf.Max(4, Mathf.RoundToInt(baseDuration));

                    data.slots[slotIdx].gameID = __instance.myID;
                    data.slots[slotIdx].isPlayerAssigned = false;
                    data.slots[slotIdx].remainingWeeks = finalWeeks;
                    data.slots[slotIdx].totalWeeks = finalWeeks;
                    data.slots[slotIdx].penaltyMultiplier = SubsidiaryTeamsPlugin.CalculateInitialPenaltyMultiplier(studio, slotIdx);
                }
            }
            else if (SubsidiaryTeamsPlugin.isCreatingPlayerSlotProject)
            {
                int slotIdx = SubsidiaryTeamsPlugin.selectedSlotIndex;
                if (slotIdx < 0 || slotIdx > 2) return;

                publisherScript studio = SubsidiaryTeamsPlugin.FindStudioByID(__instance.developerID);
                if (studio != null)
                {
                    var data = SubsidiaryTeamsPlugin.GetStudioSlotData(studio.myID);
                    if (data.slots[slotIdx].gameID == __instance.myID) return;

                    float duration = SubsidiaryTeamsPlugin.pendingPlayerSlotDuration;
                    if (duration <= 0f) duration = studio.newGameInWeeks;
                    if (duration <= 2f) duration = 20f;
                    int finalWeeks = Mathf.Max(4, Mathf.RoundToInt(duration));

                    data.slots[slotIdx].gameID = __instance.myID;
                    data.slots[slotIdx].isPlayerAssigned = true;
                    data.slots[slotIdx].remainingWeeks = finalWeeks;
                    data.slots[slotIdx].totalWeeks = finalWeeks;
                    data.slots[slotIdx].penaltyMultiplier = SubsidiaryTeamsPlugin.CalculateInitialPenaltyMultiplier(studio, slotIdx);

                    studio.newGameInWeeks = 9999;
                    studio.newGameInWeeksORG = 9999;
                }
            }
        }
        catch (System.Exception ex)
        {
            SubsidiaryTeamsPlugin.Log?.LogError($"Error in SetAsGameInDevelopmentNPC Postfix: {ex}");
        }
        finally
        {
            activeSlotIndex = -1;
            SubsidiaryTeamsPlugin.currentInitializingGame = null;
            SubsidiaryTeamsPlugin.pendingPlayerSlotDuration = -1f;
        }
    }
}

[HarmonyPatch(typeof(gameScript), "GetNameWithTag")]
public static class GameScript_GetNameWithTag_Patch
{
    [HarmonyPrefix]
    public static bool Prefix(gameScript __instance, ref string __result)
    {
        try
        {
            if (__instance == null) { __result = ""; return false; }
            if (__instance.mS_ == null || !__instance.mS_)
            {
                mainScript mS = Object.FindObjectOfType<mainScript>();
                if (mS != null) __instance.mS_ = mS;
            }
            if (__instance.tS_ == null || !__instance.tS_)
            {
                textScript tS = Object.FindObjectOfType<textScript>();
                if (tS != null) __instance.tS_ = tS;
            }
            if (__instance.mS_ == null || !__instance.mS_ || __instance.mS_.settings_ == null || !__instance.mS_.settings_)
            {
                __result = __instance.myName ?? "";
                return false;
            }
            return true;
        }
        catch
        {
            __result = __instance?.myName ?? "";
            return false;
        }
    }
}

[HarmonyPatch(typeof(publisherScript), "FindScripts")]
public static class PublisherScript_FindScripts_Fallback_Patch
{
    public static void Postfix(publisherScript __instance, ref GameObject ___main_, ref mainScript ___mS_, ref textScript ___tS_, ref settingsScript ___settings_, ref genres ___genres_, ref games ___games_, ref gameplayFeatures ___gF_, ref engineFeatures ___eF_, ref gamepassScript ___gpS_)
    {
        if (___mS_ == null)
        {
            ___main_ = GameObject.Find("Main");
            if (___main_ != null)
            {
                ___mS_ = ___main_.GetComponent<mainScript>();
                ___tS_ = ___main_.GetComponent<textScript>();
                ___settings_ = ___main_.GetComponent<settingsScript>();
                ___genres_ = ___main_.GetComponent<genres>();
                ___games_ = ___main_.GetComponent<games>();
                ___gF_ = ___main_.GetComponent<gameplayFeatures>();
                ___eF_ = ___main_.GetComponent<engineFeatures>();
                ___gpS_ = ___main_.GetComponent<gamepassScript>();
            }
        }
    }
}

