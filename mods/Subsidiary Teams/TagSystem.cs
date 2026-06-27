using System.Collections.Generic;
using UnityEngine;

public partial class SubsidiaryTeamsPlugin
{
    public class TagDefinition
    {
        public int id;
        public string name;
        public string pro;
        public string con;
        public string flavor;

        public TagDefinition(int id, string name, string pro, string con, string flavor)
        {
            this.id = id;
            this.name = name;
            this.pro = pro;
            this.con = con;
            this.flavor = flavor;
        }
    }

    private static TagDefinition[] _allTags;

    private static TagDefinition[] GetAllTags()
    {
        if (_allTags == null)
        {
            _allTags = new TagDefinition[20]
            {
                new TagDefinition(0, "Caffeine Addicts", "+25% Development Speed", "-8% Review Score", "Powered by coffee and energy drinks. Fast but misses polish."),
                new TagDefinition(1, "Pixel Purists", "+15% Review Score on RPG and Adventure games", "-15% Review Score on Sports and Arcade/Puzzle games", "They refuse to recognize polygons as art."),
                new TagDefinition(2, "Cash-Cow Factory", "+25% Sales volume", "-8% Review Score", "Masters of monetization, even if critics hate it."),
                new TagDefinition(3, "Crunch Monsters", "Immune to multi-team speed penalty", "+25% Upkeep cost", "Sleeping under desks is standard, but they demand overtime pay."),
                new TagDefinition(4, "Underdog Champions", "+15% Review Score on Small/Indie games (B/B+)", "-15% Review Score on AAA/AAAA games", "Indie legends who struggle with massive project scale."),
                new TagDefinition(5, "Trend Riders", "+20% Review Score on trending genre/theme", "-15% Review Score on off-trend projects", "They jump on every social media craze."),
                new TagDefinition(6, "Cult Classic Creators", "Sales decline 50% slower over time", "-15% Initial week sales", "Their games start slow but gather a dedicated long-term following."),
                new TagDefinition(7, "Sequel Machine", "+15% Review Score on Sequels, Remakes, and Spin-offs", "-15% Review Score on New IPs", "They know exactly how to iterate on a familiar formula."),
                new TagDefinition(8, "Lone Wolves", "+20% Development Speed when developing solo", "-20% Contribution Speed when supporting other teams", "Brilliant individuals who don't play well with others."),
                new TagDefinition(9, "Perfectionists", "+12% Review Score on all releases", "+20% Development Time (speed * 0.80)", "A delayed game is eventually good, but a rushed game is forever bad."),
                new TagDefinition(10, "Modding Darlings", "+30 Initial Hype upon release", "+15% Upkeep cost", "Releasing robust SDKs builds immense community hype."),
                new TagDefinition(11, "Bargain Bin Devs", "Upkeep cost reduced by -40%", "-25% XP earned from releases", "Mostly unpaid interns. Very cheap, but learn slowly."),
                new TagDefinition(12, "Spaghetti Coders", "+30% Development Speed on sequels", "-15% Development Speed on New IPs", "Copy-pasting old code speeds up sequels but drags down original concepts."),
                new TagDefinition(13, "Over-Engineered", "Old game engines do not suffer tech/graphics score penalties", "+25% Development Time (speed * 0.80)", "Writing custom physics libraries instead of engine defaults."),
                new TagDefinition(14, "Hype Beasts", "+50% Hype generation from advertising/marketing", "If score < 75, sales drop twice as fast after week 2", "Flashy trailers but the game must back up the talk."),
                new TagDefinition(15, "Early Access Addicts", "+15% Sales during development phase (TODO)", "-10% final Review Score", "Selling unfinished builds gets early cash but irritates critics."),
                new TagDefinition(16, "AAA Veterans", "+15% Review Score on AAA/AAAA projects", "+30% Upkeep cost", "Expensive industry veterans who know how to deliver blockbusters."),
                new TagDefinition(17, "Mobile Specialists", "+20% Review Score on Mobile platforms", "-20% Review Score on Arcade or Console/PC", "Masters of touch screen layouts and mobile controls."),
                new TagDefinition(18, "Storytelling Auteurs", "+20% Review Score on RPG and Adventure games", "-20% Review Score on Shooter and Sports games", "They prioritize dialogue trees and emotional narratives."),
                new TagDefinition(19, "Asset Flippers", "-40% Development Time (speed * 1.66)", "-20% Review Score", "Buying ready-made store assets. Super fast, but lazy."),
            };
        }
        return _allTags;
    }

    public static TagDefinition GetTagByID(int id)
    {
        TagDefinition[] all = GetAllTags();
        if (id >= 0 && id < all.Length) return all[id];
        return null;
    }

    public static string GetTagName(int id)
    {
        TagDefinition t = GetTagByID(id);
        return t != null ? t.name : "Unknown";
    }

    public static string GetTagTooltipText(int id)
    {
        TagDefinition t = GetTagByID(id);
        if (t == null) return "";
        return $"<color=#00ff00>Pro: {t.pro}</color>\n<color=#ff4444>Con: {t.con}</color>\n<i>{t.flavor}</i>";
    }

    public static int[] GetRandomTagIDs(int count, System.Random rng)
    {
        TagDefinition[] all = GetAllTags();
        int total = all.Length;
        List<int> ids = new List<int>();
        for (int i = 0; i < total; i++) ids.Add(i);

        for (int i = total - 1; i > 0; i--)
        {
            int j = rng.Next(0, i + 1);
            int tmp = ids[i];
            ids[i] = ids[j];
            ids[j] = tmp;
        }

        int[] result = new int[count];
        for (int i = 0; i < count && i < total; i++)
            result[i] = ids[i];
        return result;
    }

    public static int GetTagCount()
    {
        return GetAllTags().Length;
    }

    internal static bool TryGetSlotForGame(int gameID, out StudioSlotData outData, out int outSlotIdx)
    {
        outData = null;
        outSlotIdx = -1;
        if (State == null) return false;
        foreach (var sd in State.studios)
        {
            if (sd == null || sd.slots == null) continue;
            for (int i = 0; i < sd.slots.Length; i++)
            {
                if (sd.slots[i] != null && sd.slots[i].gameID == gameID)
                {
                    outData = sd;
                    outSlotIdx = i;
                    return true;
                }
            }
        }
        return false;
    }
}
