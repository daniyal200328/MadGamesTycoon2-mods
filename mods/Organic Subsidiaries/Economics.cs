using System.Reflection;
using UnityEngine;

public partial class Subsidiary2Plugin
{
    private static MethodInfo dynamicCostMethod;
    private static bool searchedDynamicCost;

    private static System.Type goodwillPluginType;
    private static FieldInfo goodwillStatesField;
    private static bool searchedGoodwill;

    internal static long GetDynamicUpkeepReflected(publisherScript studio)
    {
        if (studio == null) return 0L;
        if (!searchedDynamicCost)
        {
            searchedDynamicCost = true;
            try
            {
                foreach (var asm in System.AppDomain.CurrentDomain.GetAssemblies())
                {
                    var t = asm.GetType("DynamicSubsidiaryTimelinePlugin");
                    if (t != null)
                    {
                        dynamicCostMethod = t.GetMethod("CalculateDynamicVerwaltungskosten",
                            BindingFlags.Public | BindingFlags.Static);
                        break;
                    }
                }
            }
            catch { }
        }

        if (dynamicCostMethod != null)
        {
            try { return (long)dynamicCostMethod.Invoke(null, new object[] { studio }); }
            catch { }
        }
        return studio.GetVerwaltungskosten();
    }

    internal static string GetGoodwillTrendLabel(int studioID)
    {
        try
        {
            if (!searchedGoodwill)
            {
                searchedGoodwill = true;
                var t = System.Type.GetType("DynamicStudioGoodwillPlugin, DynamicStudioGoodwill");
                if (t == null)
                {
                    foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
                    {
                        t = assembly.GetType("DynamicStudioGoodwillPlugin");
                        if (t != null) break;
                    }
                }
                goodwillPluginType = t;
                if (goodwillPluginType != null)
                    goodwillStatesField = goodwillPluginType.GetField("states", BindingFlags.NonPublic | BindingFlags.Static);
            }

            if (goodwillStatesField != null)
            {
                object statesObj = goodwillStatesField.GetValue(null);
                if (statesObj != null)
                {
                    var tryGetValue = statesObj.GetType().GetMethod("TryGetValue");
                    if (tryGetValue != null)
                    {
                        object[] args = new object[] { studioID, null };
                        bool found = (bool)tryGetValue.Invoke(statesObj, args);
                        if (found && args[1] != null)
                        {
                            var labelField = args[1].GetType().GetField("Label");
                            if (labelField != null)
                                return labelField.GetValue(args[1]) as string;
                        }
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            if (log != null) log.LogWarning("Failed to query goodwill trend via reflection: " + ex.Message);
        }
        return null;
    }

    public static long GetOrganicSaleValue(publisherScript studio)
    {
        try
        {
            if (studio == null) return 0L;

            if (state == null) state = new ModState();
            if (state.studios == null) state.studios = new System.Collections.Generic.List<StudioData>();

            StudioData data = GetStudioData(studio.myID);
            long creationCost = data != null ? data.creationCost : studio.firmenwert;
            if (creationCost <= 0)
                creationCost = studio.firmenwert > 0 ? studio.firmenwert : 2000000L;
            long upgradeInvestments = data != null ? data.upgradeInvestments : 0L;

            long baseValue = (long)(creationCost * 0.60f) + (long)(upgradeInvestments * 0.70f);
            long ipValue = GetOrganicIpValue(studio);
            long recentProfitMultiplier = GetRecentPositiveRevenue(studio) / 2L;
            long totalValue = baseValue + ipValue + recentProfitMultiplier;

            if (GetReleasedOrganicGames(studio) == 0)
            {
                long cap = (long)(creationCost * 0.60f);
                if (totalValue > cap) totalValue = cap;
            }

            string trend = GetGoodwillTrendLabel(studio.myID);
            if (!string.IsNullOrEmpty(trend))
            {
                if (trend == "Rising") totalValue = (long)(totalValue * 1.15f);
                else if (trend == "Commercial Powerhouse") totalValue = (long)(totalValue * 1.30f);
                else if (trend == "Declining" || trend == "In Crisis") totalValue = (long)(totalValue * 0.80f);
            }

            return System.Math.Max(0L, totalValue);
        }
        catch (System.Exception ex)
        {
            if (log != null) log.LogError($"GetOrganicSaleValue failed: {ex}");
            return studio != null ? studio.firmenwert : 2000000L;
        }
    }

    private static long GetOrganicIpValue(publisherScript studio)
    {
        try
        {
            games gamesScript = studio.games_ ?? GetGamesScript();
            if (gamesScript == null || gamesScript.arrayGamesScripts == null) return 0L;

            long value = 0L;
            for (int i = 0; i < gamesScript.arrayGamesScripts.Length; i++)
            {
                gameScript game = gamesScript.arrayGamesScripts[i];
                if (game != null && game.sellsTotal > 0 && !game.inDevelopment && !game.schublade && !game.pubAngebot && !game.auftragsspiel && game.IsMyIP(studio) && game.mainIP == game.myID)
                    value += game.GetIpWert();
            }
            return value;
        }
        catch { return 0L; }
    }

    private static long GetRecentPositiveRevenue(publisherScript studio)
    {
        try
        {
            if (studio.tf_umsatz == null) return 0L;
            long total = 0L;
            for (int i = 0; i < studio.tf_umsatz.Length; i++)
            {
                if (studio.tf_umsatz[i] > 0L) total += studio.tf_umsatz[i];
            }
            return total;
        }
        catch { return 0L; }
    }

    private static int GetReleasedOrganicGames(publisherScript studio)
    {
        try
        {
            games gamesScript = studio.games_ ?? GetGamesScript();
            if (gamesScript == null || gamesScript.arrayGamesScripts == null) return 0;

            int count = 0;
            for (int i = 0; i < gamesScript.arrayGamesScripts.Length; i++)
            {
                gameScript game = gamesScript.arrayGamesScripts[i];
                if (game != null && game.developerID == studio.myID && game.ownerID == studio.myID && !game.inDevelopment && !game.schublade && !game.auftragsspiel && !game.pubAngebot && game.sellsTotal > 0)
                    count++;
            }
            return count;
        }
        catch { return 0; }
    }

    public static string GetSafeOrganicTooltip(publisherScript studio, mainScript mS, textScript tS)
    {
        if (studio == null) return "";
        string name = studio.name_EN;
        if (string.IsNullOrEmpty(name)) name = "Organic Studio";

        string typeStr = (studio.developer && !studio.publisher)
            ? (tS != null ? tS.GetText(274) : "Developer")
            : (tS != null ? tS.GetText(432) : "Publisher");

        string countryName = (tS != null) ? tS.GetCountry(studio.country) : "Unknown Country";

        string text = $"<b><size=18>{name}</size></b>";
        if (tS != null)
            text += "\n<b><size=15><color=green>" + tS.GetText(1924) + "</color></size></b>";

        text += "\n<b>" + typeStr + "</b>";
        text += "\n<b>" + countryName + "</b>";

        if (tS != null)
        {
            int monthIndex = Mathf.Clamp(studio.date_month - 1 + 221, 221, 232);
            text += "\n<b>" + tS.GetText(monthIndex) + " " + studio.date_year + "</b>";
        }

        string starLabel = (tS != null) ? tS.GetText(434) : "Market";
        text += "\n\n" + starLabel + ": <color=blue><size=20>";
        int starsCount = Mathf.Clamp(Mathf.RoundToInt(studio.stars / 20f), 0, 5);
        for (int j = 0; j < starsCount; j++) text += "★";
        text += "</size></color>";

        long gamesCount = 0;
        games gamesScript = studio.games_ ?? Object.FindObjectOfType<games>();
        if (gamesScript != null && gamesScript.arrayGamesScripts != null)
        {
            for (int i = 0; i < gamesScript.arrayGamesScripts.Length; i++)
            {
                gameScript game = gamesScript.arrayGamesScripts[i];
                if (game != null && game.developerID == studio.myID && game.ownerID == studio.myID && !game.inDevelopment && !game.schublade && !game.auftragsspiel && !game.pubAngebot && game.sellsTotal > 0)
                    gamesCount++;
            }
        }
        if (tS != null)
            text += "\n" + tS.GetText(271) + ": <color=blue>" + (mS != null ? mS.GetMoney(gamesCount, showDollar: false) : gamesCount.ToString()) + "</color>";

        if (tS != null && mS != null)
        {
            text += "\n\n" + tS.GetText(1930) + ": <color=green><b>" + mS.GetMoney(studio.tf_umsatz_allTime, showDollar: true) + "</b></color>";

            long rev24 = 0;
            if (studio.tf_umsatz != null)
            {
                for (int i = 0; i < studio.tf_umsatz.Length; i++)
                {
                    if (studio.tf_umsatz[i] > 0L) rev24 += studio.tf_umsatz[i];
                }
            }
            text += "\n" + tS.GetText(698) + ": <color=green><b>" + mS.GetMoney(rev24, showDollar: true) + "</b></color>";

            long adminCost = GetDynamicUpkeepReflected(studio);
            text += "\n" + tS.GetText(1934) + ": <color=red><b>" + mS.GetMoney(adminCost, showDollar: true) + "</b></color>";
        }

        return text;
    }
}
