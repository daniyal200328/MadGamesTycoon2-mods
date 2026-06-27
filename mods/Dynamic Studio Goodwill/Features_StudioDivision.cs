using UnityEngine;

partial class DynamicStudioGoodwillPlugin
{
    private static int GetSubdivision(publisherScript studio)
    {
        if (studio == null || studio.isPlayer) return 0;

        float stars = Mathf.Clamp(studio.stars, 0f, 100f);

        if (studio.ownPlatform)
            return 1; // Platform Holder

        if (studio.publisher)
        {
            if (stars >= 70f) return 2;  // Mega Blockbuster Publisher
            if (stars >= 40f) return 3;  // Premiere Publisher
            return 4;                     // Boutique Label
        }

        // Developer only
        if (stars >= 75f) return 5;  // Major Studio
        if (stars >= 50f) return 6;  // Premiere Studio
        if (stars >= 36f) return 7;  // Triple-I Studio
        return 8;                     // Indie Studio
    }

    private static string GetSubdivisionName(int subdiv)
    {
        switch (subdiv)
        {
            case 1: return "Platform Holder";
            case 2: return "Mega Blockbuster Publisher";
            case 3: return "Premiere Publisher";
            case 4: return "Boutique Label";
            case 5: return "Major Studio";
            case 6: return "Premiere Studio";
            case 7: return "Triple-I Studio";
            case 8: return "Indie Studio";
            default: return "";
        }
    }

    private static string GetSubdivisionColor(int subdiv)
    {
        switch (subdiv)
        {
            case 1: return "#E53935";
            case 2: return "#FDD835";
            case 3: return "#1E88E5";
            case 4: return "#43A047";
            case 5: return "#FB8C00";
            case 6: return "#8E24AA";
            case 7: return "#00ACC1";
            case 8: return "#757575";
            default: return "#808080";
        }
    }
}
