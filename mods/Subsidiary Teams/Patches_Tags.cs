using HarmonyLib;
using UnityEngine;

// ──────────────────────────────────────────────────────────
// Postfix on CalcReview — apply tag review score modifiers
// ──────────────────────────────────────────────────────────
[HarmonyPatch(typeof(gameScript), "CalcReview")]
public static class CalcReview_TagPatch
{
    public static void Postfix(gameScript __instance)
    {
        if (__instance == null) return;

        SubsidiaryTeamsPlugin.StudioSlotData data;
        int slotIdx;
        if (!SubsidiaryTeamsPlugin.TryGetSlotForGame(__instance.myID, out data, out slotIdx)) return;
        if (data == null || slotIdx < 0 || slotIdx >= data.slots.Length) return;

        SubsidiaryTeamsPlugin.SlotData slot = data.slots[slotIdx];
        if (slot == null || slot.traitID < 0) return;

        int traitID = slot.traitID;
        float review = (float)__instance.reviewTotal;

        if (review <= 0f) return;

        // Over-Engineered (13): remove engine age penalty
        if (traitID == 13)
        {
            var eF = __instance.eF_;
            var mS = __instance.mS_;
            if (eF != null && mS != null)
            {
                var genres = Object.FindObjectOfType<genres>();
                if (genres != null && __instance.gameEngineFeature != null && __instance.gameEngineFeature.Length >= 4)
                {
                    int oAmt0 = eF.GetOutdatetAmount(__instance.gameEngineFeature[0]);
                    int oAmt1 = eF.GetOutdatetAmount(__instance.gameEngineFeature[1]);
                    int oAmt2 = eF.GetOutdatetAmount(__instance.gameEngineFeature[2]);
                    int oAmt3 = eF.GetOutdatetAmount(__instance.gameEngineFeature[3]);

                    float penaltyFromGameplay = (float)(oAmt2 + oAmt3) * 0.01f * (float)genres.genres_GAMEPLAY[__instance.maingenre];
                    float penaltyFromGrafik = (float)(oAmt0 * 2) * 0.01f * (float)genres.genres_GRAPHIC[__instance.maingenre];
                    float penaltyFromSound = (float)(oAmt1 * 2) * 0.01f * (float)genres.genres_SOUND[__instance.maingenre];

                    review += (penaltyFromGameplay + penaltyFromGrafik + penaltyFromSound);
                }
            }
        }

        // Apply tag multipliers
        switch (traitID)
        {
            case 0: review *= 0.92f; break;
            case 2: review *= 0.92f; break;
            case 9: review *= 1.12f; break;
            case 15: review *= 0.90f; break;
            case 19: review *= 0.80f; break;
        }

        // Genre-specific tags
        switch (traitID)
        {
            case 1: // Pixel Purists
                if (__instance.maingenre == 3 || __instance.maingenre == 2)
                    review *= 1.15f;
                else if (__instance.maingenre == 8 || __instance.maingenre == 0 || __instance.maingenre == 1)
                    review *= 0.85f;
                break;
            case 4: // Underdog Champions
                if (__instance.gameSize <= 1)
                    review *= 1.15f;
                else if (__instance.gameSize >= 4)
                    review *= 0.85f;
                break;
            case 5: // Trend Riders
            {
                bool trending = false;
                if (__instance.mS_ != null)
                {
                    if (__instance.maingenre == __instance.mS_.trendGenre ||
                        __instance.gameMainTheme == __instance.mS_.trendTheme)
                        trending = true;
                }
                review *= trending ? 1.20f : 0.85f;
                break;
            }
            case 7: // Sequel Machine
                if (__instance.typ_nachfolger || __instance.typ_remaster || __instance.typ_spinoff)
                    review *= 1.15f;
                else if (__instance.typ_standard)
                    review *= 0.85f;
                break;
            case 16: // AAA Veterans
                if (__instance.gameSize >= 4)
                    review *= 1.15f;
                break;
            case 17: // Mobile Specialists
                if (__instance.handy)
                    review *= 1.20f;
                else
                    review *= 0.80f;
                break;
            case 18: // Storytelling Auteurs
                if (__instance.maingenre == 3 || __instance.maingenre == 2)
                    review *= 1.20f;
                else if (__instance.maingenre == 14 || __instance.maingenre == 16 || __instance.maingenre == 8)
                    review *= 0.80f;
                break;
        }

        // Clamp
        int finalReview = Mathf.RoundToInt(review);
        if (finalReview < 1) finalReview = 1;
        if (finalReview > 100) finalReview = 100;
        __instance.reviewTotal = finalReview;
    }
}

// ──────────────────────────────────────────────────────────
// Postfix on SellGame — apply tag sales modifiers
// ──────────────────────────────────────────────────────────
[HarmonyPatch(typeof(gameScript), "SellGame")]
public static class SellGame_TagPatch
{
    public static void Postfix(gameScript __instance)
    {
        if (__instance == null) return;

        SubsidiaryTeamsPlugin.StudioSlotData data;
        int slotIdx;
        if (!SubsidiaryTeamsPlugin.TryGetSlotForGame(__instance.myID, out data, out slotIdx)) return;
        if (data == null || slotIdx < 0 || slotIdx >= data.slots.Length) return;

        SubsidiaryTeamsPlugin.SlotData slot = data.slots[slotIdx];
        if (slot == null || slot.traitID < 0) return;

        int traitID = slot.traitID;

        switch (traitID)
        {
            case 2: // Cash-Cow Factory: +25% sales
                if (__instance.sellsPerWeek != null && __instance.sellsPerWeek.Length > 0)
                    __instance.sellsPerWeek[0] = Mathf.RoundToInt(__instance.sellsPerWeek[0] * 1.25f);
                break;

            case 6: // Cult Classic Creators
            {
                int weeks = __instance.weeksOnMarket;
                if (weeks == 1 && __instance.sellsPerWeek != null && __instance.sellsPerWeek.Length > 0)
                    __instance.sellsPerWeek[0] = Mathf.RoundToInt(__instance.sellsPerWeek[0] * 0.85f);
                else if (weeks > 2 && __instance.sellsPerWeek != null && __instance.sellsPerWeek.Length > 0)
                    __instance.sellsPerWeek[0] = Mathf.RoundToInt(__instance.sellsPerWeek[0] * 1.50f);
                break;
            }

            case 14: // Hype Beasts: if score < 75, sales drop twice as fast after week 2
                if (__instance.reviewTotal < 75 && __instance.weeksOnMarket > 2
                    && __instance.sellsPerWeek != null && __instance.sellsPerWeek.Length > 0)
                    __instance.sellsPerWeek[0] = Mathf.RoundToInt(__instance.sellsPerWeek[0] * 0.50f);
                break;
        }
    }
}

// ──────────────────────────────────────────────────────────
// Prefix on AddHype — apply Hype Beasts modifier (+50% Hype generation)
// ──────────────────────────────────────────────────────────
[HarmonyPatch(typeof(gameScript), "AddHype")]
public static class GameScript_AddHype_Patch
{
    public static void Prefix(gameScript __instance, ref float f)
    {
        if (__instance == null || f <= 0f) return;

        SubsidiaryTeamsPlugin.StudioSlotData data;
        int slotIdx;
        if (!SubsidiaryTeamsPlugin.TryGetSlotForGame(__instance.myID, out data, out slotIdx)) return;
        if (data == null || slotIdx < 0 || slotIdx >= data.slots.Length) return;

        SubsidiaryTeamsPlugin.SlotData slot = data.slots[slotIdx];
        if (slot == null || slot.traitID < 0) return;

        if (slot.traitID == 14) // Hype Beasts: +50% Hype generation
        {
            f *= 1.50f;
        }
    }
}
