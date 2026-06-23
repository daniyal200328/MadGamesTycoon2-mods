using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[BepInPlugin("org.bepinex.plugins.studiopartnershipsoverhaul", "Studio Partnerships and Relationships Overhaul", "1.0.0")]
public class StudioPartnershipsOverhaulPlugin : BaseUnityPlugin
{
    private static StudioPartnershipsOverhaulPlugin Instance;
    private static ManualLogSource log;
    private static string statePath;

    private static int activeOutsourceNpcId = -1;
    private static int activeOutsourceIsNewIP = -1; // 0 = Player IP, 1 = New IP, 2 = NPC IP
    private static StartSnapshot pendingStartSnapshot;

    // Cache of original game design room references for interception
    private static roomScript borrowedRoom;
    private static int borrowedRoomOriginalTaskId = -1;
    private static GameObject borrowedRoomOriginalTaskObject;
    private static GameObject temporaryDesignRoomObject;

    private static float activeBuyoutDiscountFactor = 1.0f;
    private static GameObject choiceDialog;
    private static int framesSinceNoDesignMenu = 0;
    private static mainScript cachedMainScript;
    private static GUI_Main cachedGuiMain;
    private static games cachedGamesScript;
    private static GameObject partnershipsPanel;
    private static GameObject partnershipsScrollContent;
    
    // Global Event Trackers for Relationships (survive save/load)
    private static HashSet<int> processedReleasedGames = new HashSet<int>();
    private static HashSet<int> processedHitGames = new HashSet<int>();
    private static HashSet<int> processedMasterpieces = new HashSet<int>();
    private static HashSet<int> processedHighSales = new HashSet<int>();
    private static HashSet<int> processedExclusiveGames = new HashSet<int>();
    private static HashSet<int> processedContractGames = new HashSet<int>();
    private static HashSet<int> processedCommissions = new HashSet<int>();
    private static HashSet<int> processedFailures = new HashSet<int>();
    private static HashSet<int> processedCancelledContracts = new HashSet<int>();
    private static HashSet<int> processedBoughtIPs = new HashSet<int>();
    private static HashSet<int> processedGiftedIPs = new HashSet<int>();
    private static HashSet<int> processedEngineLicenses = new HashSet<int>();
    private static HashSet<int> processedCoDevelopments = new HashSet<int>();

    // Game contract mappings (survive save/load)
    public static Dictionary<int, string> gameActiveContracts = new Dictionary<int, string>();

    // Database of Mod States
    public class StudioState
    {
        public int StudioId;
        public float CustomRelation; // Not used as primary, we use pS.relation, but kept for sync
        public string ActiveContract = ""; // "None", "Exclusivity", "AutoPublish", "CoPublish"
        public int ContractWeeksLeft = 0;
        public int LastGameplayUnlockWeek = -999;
        public int LastEngineUnlockWeek = -999;
        public int LastTopicUnlockWeek = -999;
        public int LastCoFinanceWeek = -999;
        public int LastAAAWeek = -999;
        public int LastExclusiveWeek = -999;
        public int LastOutsourceWeek = -999;
        public bool FundedByPlayer = false;
        
        // Queued Outsource Project Data
        public bool HasQueuedProject = false;
        public string QueuedName = "";
        public int QueuedGenre = -1;
        public int QueuedSubGenre = -1;
        public int QueuedTheme = -1;
        public int QueuedSubTheme = -1;
        public int QueuedSize = -1;
        public int QueuedEngineId = -1;
        public int[] QueuedPlatforms = new int[4] { -1, -1, -1, -1 };
        public bool[] QueuedFeatures = new bool[50];
        public bool QueuedLetKeepIP = false;
        public List<int> OutsourcedGameIds = new List<int>();

        // New system variables
        public bool PublishedFirstGame = false;
        public int CommissionWeeksLeft = 0;
        public int CommissionGenre = -1;
        public int CommissionTargetReview = 0;
        public long CommissionReward = 0L;
        public int ContractYearlyTimer = 0;
        public int LastIPBuyWeek = -999;
    }

    public static Dictionary<int, StudioState> states = new Dictionary<int, StudioState>();

    private void Awake()
    {
        Instance = this;
        log = Logger;
        statePath = Path.Combine(Paths.ConfigPath, "StudioPartnerships_State.tsv");
        LoadState();
        new Harmony("org.bepinex.plugins.studiopartnershipsoverhaul").PatchAll();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }

        cachedMainScript = null;
        cachedGuiMain = null;
        cachedGamesScript = null;
        choiceDialog = null;
        CleanupTemporaryDesignRoom();
    }

    // ==================== PARTNERSHIPS OVERVIEW PANEL (Ctrl+Shift+P) ====================

    private static void ShowPartnershipsPanel()
    {
        mainScript mS = GetMainScriptCached();
        GUI_Main gui = GetGuiMainCached();
        if (mS == null || !mS || gui == null || !gui) return;

        ClosePartnershipsPanel();

        Font font = ResolveFont(gui, gui.uiObjects);
        Canvas canvas = gui.GetComponentInParent<Canvas>();
        if (canvas == null) canvas = UnityEngine.Object.FindObjectOfType<Canvas>();
        if (canvas == null) return;

        // Main panel background
        partnershipsPanel = new GameObject("SPO_PartnershipsPanel", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        partnershipsPanel.transform.SetParent(canvas.transform, false);

        Image bg = partnershipsPanel.GetComponent<Image>();
        bg.color = new Color(0.08f, 0.08f, 0.10f, 0.98f);
        bg.material = null;

        RectTransform panelRt = partnershipsPanel.GetComponent<RectTransform>();
        panelRt.anchorMin = new Vector2(0.5f, 0.5f);
        panelRt.anchorMax = new Vector2(0.5f, 0.5f);
        panelRt.pivot = new Vector2(0.5f, 0.5f);
        panelRt.sizeDelta = new Vector2(900f, 600f);
        panelRt.anchoredPosition = Vector2.zero;
        panelRt.localScale = Vector3.one;

        // Title bar
        GameObject titleBar = new GameObject("TitleBar", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        titleBar.transform.SetParent(partnershipsPanel.transform, false);
        RectTransform titleRt = titleBar.GetComponent<RectTransform>();
        titleRt.anchorMin = new Vector2(0f, 1f);
        titleRt.anchorMax = new Vector2(1f, 1f);
        titleRt.pivot = new Vector2(0.5f, 1f);
        titleRt.sizeDelta = new Vector2(0f, 40f);
        titleRt.anchoredPosition = new Vector2(0f, 0f);
        titleBar.GetComponent<Image>().color = new Color(0.15f, 0.25f, 0.45f, 1f);

        Text titleText = new GameObject("TitleText", typeof(RectTransform), typeof(Text)).GetComponent<Text>();
        titleText.transform.SetParent(titleBar.transform, false);
        RectTransform ttRt = titleText.GetComponent<RectTransform>();
        ttRt.anchorMin = new Vector2(0.05f, 0f);
        ttRt.anchorMax = new Vector2(0.85f, 1f);
        ttRt.offsetMin = Vector2.zero;
        ttRt.offsetMax = Vector2.zero;
        titleText.font = font;
        titleText.fontSize = 18;
        titleText.fontStyle = FontStyle.Bold;
        titleText.color = Color.white;
        titleText.alignment = TextAnchor.MiddleLeft;
        titleText.text = "Active Partnerships & Deals";

        // Close button
        GameObject closeBtn = new GameObject("CloseBtn", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
        closeBtn.transform.SetParent(titleBar.transform, false);
        RectTransform cbRt = closeBtn.GetComponent<RectTransform>();
        cbRt.anchorMin = new Vector2(0.88f, 0.15f);
        cbRt.anchorMax = new Vector2(0.98f, 0.85f);
        cbRt.offsetMin = Vector2.zero;
        cbRt.offsetMax = Vector2.zero;
        closeBtn.GetComponent<Image>().color = new Color(0.7f, 0.15f, 0.15f, 1f);
        Button cb = closeBtn.GetComponent<Button>();
        cb.targetGraphic = closeBtn.GetComponent<Image>();
        Text cbText = new GameObject("Text", typeof(RectTransform), typeof(Text)).GetComponent<Text>();
        cbText.transform.SetParent(closeBtn.transform, false);
        RectTransform cbtRt = cbText.GetComponent<RectTransform>();
        cbtRt.anchorMin = Vector2.zero;
        cbtRt.anchorMax = Vector2.one;
        cbtRt.offsetMin = Vector2.zero;
        cbtRt.offsetMax = Vector2.zero;
        cbText.font = font;
        cbText.fontSize = 14;
        cbText.fontStyle = FontStyle.Bold;
        cbText.color = Color.white;
        cbText.alignment = TextAnchor.MiddleCenter;
        cbText.text = "X";
        cb.onClick.AddListener(() => ClosePartnershipsPanel());

        // Scroll view container
        GameObject scrollObj = new GameObject("ScrollView", typeof(RectTransform), typeof(Image), typeof(ScrollRect));
        scrollObj.transform.SetParent(partnershipsPanel.transform, false);
        RectTransform scrollRt = scrollObj.GetComponent<RectTransform>();
        scrollRt.anchorMin = new Vector2(0f, 0f);
        scrollRt.anchorMax = new Vector2(1f, 1f);
        scrollRt.pivot = new Vector2(0.5f, 0.5f);
        scrollRt.offsetMin = new Vector2(10f, 10f);
        scrollRt.offsetMax = new Vector2(-10f, -50f);
        scrollObj.GetComponent<Image>().color = new Color(0.05f, 0.05f, 0.07f, 0.5f);

        // Content container (holds all partnership entries)
        partnershipsScrollContent = new GameObject("Content", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        partnershipsScrollContent.transform.SetParent(scrollObj.transform, false);
        RectTransform contentRt = partnershipsScrollContent.GetComponent<RectTransform>();
        contentRt.anchorMin = new Vector2(0f, 1f);
        contentRt.anchorMax = new Vector2(1f, 1f);
        contentRt.pivot = new Vector2(0.5f, 1f);
        contentRt.sizeDelta = new Vector2(0f, 0f);
        contentRt.anchoredPosition = Vector2.zero;
        partnershipsScrollContent.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);

        // Add VerticalLayoutGroup + ContentSizeFitter for auto-layout
        VerticalLayoutGroup vlg = partnershipsScrollContent.AddComponent<VerticalLayoutGroup>();
        vlg.spacing = 6f;
        vlg.padding = new RectOffset(8, 8, 8, 8);
        vlg.childAlignment = TextAnchor.UpperCenter;
        vlg.childControlWidth = true;
        vlg.childControlHeight = false;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = false;
        ContentSizeFitter csf = partnershipsScrollContent.AddComponent<ContentSizeFitter>();
        csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        ScrollRect scrollRect = scrollObj.GetComponent<ScrollRect>();
        scrollRect.content = contentRt;
        scrollRect.horizontal = false;
        scrollRect.vertical = true;
        scrollRect.scrollSensitivity = 20f;

        // Populate entries
        PopulatePartnershipsEntries(mS, gui, font);

        // Set content size after population
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRt);
    }

      private static void PopulatePartnershipsEntries(mainScript mS, GUI_Main gui, Font font)
    {
        if (partnershipsScrollContent == null) return;
        bool foundAny = false;

        // 1. Active Game Contracts (Co-Publish, AAA Co-Publish, IP Cooperation, Exclusivity Funding)
        games gamesScript = GetGamesCached();
        if (gamesScript != null && gamesScript.arrayGamesScripts != null)
        {
            foreach (var kvp in gameActiveContracts)
            {
                gameScript game = null;
                for (int i = 0; i < gamesScript.arrayGamesScripts.Length; i++)
                {
                    if (gamesScript.arrayGamesScripts[i] != null && gamesScript.arrayGamesScripts[i].myID == kvp.Key)
                    {
                        game = gamesScript.arrayGamesScripts[i];
                        break;
                    }
                }
                if (game == null) continue;

                publisherScript partner = FindStudioByID(mS, game.publisherID);
                if (partner == null || partner.isPlayer)
                {
                    partner = FindStudioByID(mS, game.developerID);
                }
                if (partner == null) continue;

                string contractLabel = GetContractLabel(kvp.Value);
                string partnerName = SafeStudioName(partner);
                int subdiv = GetSubdivision(partner);
                string subdivName = GetSubdivisionName(subdiv);

                string gameName = game.myName ?? "Unknown";
                string gameSize = SafeGetGameSizeString(game);
                string genreStr = SafeGetGenreString(game);

                if (game.isOnMarket) continue; // Remove released games from list
                if (!game.inDevelopment) continue; // Only show in-development games

                string progressStr;
                string progressColor;
                {
                    float fundingPct = GetCoFundingPercentage(game) * 100f;
                    publisherScript devStudio = FindStudioByID(mS, game.developerID);
                    int weeksLeft = devStudio != null ? devStudio.newGameInWeeks : 0;
                    int totalWeeks = devStudio != null ? devStudio.newGameInWeeksORG : 0;
                    float pct = 0f;
                    if (totalWeeks > 0) pct = Mathf.Clamp01(1f - ((float)weeksLeft / totalWeeks));
                    progressStr = $"In Dev: {pct * 100f:F0}% | ~{weeksLeft}w to release | Funding: {fundingPct:F0}%";
                    if (game.angekuendigt) progressStr += " | Announced";
                    progressColor = "#4FC3F7";
                }

                string detail = $"<b>{contractLabel}</b> with <b>{partnerName}</b> ({subdivName})\n" +
                                $"Game: {gameName} [{gameSize}] {genreStr}\n" +
                                $"<color={progressColor}>{progressStr}</color>";

                if (game.exklusiv) detail += " | <color=#FFD54F>EXCLUSIVE</color>";
                if (game.inGamePass) detail += " | <color=#A5D6A7>Game Pass</color>";

                AddPartnershipEntry(detail, font, GetContractColor(kvp.Value));
                foundAny = true;
            }
        }

        // 2. Active Studio Contracts (AutoPublish, ConsoleAutoPublish) - with current game
        foreach (var kvp in states)
        {
            StudioState state = kvp.Value;
            if (state.ContractWeeksLeft <= 0) continue;
            if (state.ActiveContract != "AutoPublish" && state.ActiveContract != "ConsoleAutoPublish") continue;

            publisherScript partner = FindStudioByID(mS, kvp.Key);
            if (partner == null) continue;

            string partnerName = SafeStudioName(partner);
            int subdiv = GetSubdivision(partner);
            string subdivName = GetSubdivisionName(subdiv);
            int weeksLeft = state.ContractWeeksLeft;
            int yearsLeft = weeksLeft / 48;
            int remWeeks = weeksLeft % 48;

            string contractLabel = state.ActiveContract == "ConsoleAutoPublish" ? "Permanent: Console Auto-Publish + Exclusivity" : "Permanent: Auto-Publish Contract";
            string detail = $"<b>{contractLabel}</b> with <b>{partnerName}</b> ({subdivName})\n" +
                            $"<color=#81C784>Active</color> | Time left: {yearsLeft}y {remWeeks}w | Yearly fee: {mS.GetMoney(CalculateAutoPublishYearlyFee(partner), true)}";

            if (state.ActiveContract == "ConsoleAutoPublish")
            {
                detail += " | <color=#FFD54F>Console Exclusive + Game Pass</color>";
            }

            // Show current game in development under this permanent deal
            gameScript currentGame = partner.FindGameInDevelopment();
            if (currentGame != null && currentGame.inDevelopment && !currentGame.isOnMarket)
            {
                string gameName = currentGame.myName ?? "Unknown";
                string gameSize = SafeGetGameSizeString(currentGame);
                float pct = 0f;
                int gameWeeksLeft = partner.newGameInWeeks;
                int totalWeeks = partner.newGameInWeeksORG;
                if (totalWeeks > 0)
                {
                    pct = Mathf.Clamp01(1f - ((float)gameWeeksLeft / totalWeeks));
                }
                detail += $"\nCurrent Game: {gameName} [{gameSize}] | <color=#4FC3F7>In Dev: {pct * 100f:F0}% | ~{gameWeeksLeft}w to release</color>";
            }
            else
            {
                detail += "\n<color=#FFB74D>No game currently in development (next game will appear here automatically)</color>";
            }

            AddPartnershipEntry(detail, font, new Color(0.15f, 0.35f, 0.15f, 0.9f));
            foundAny = true;
        }

        // 3. Outsource Projects (in-progress or queued)
        foreach (var kvp in states)
        {
            StudioState state = kvp.Value;

            // Active outsourced games
            if (state.OutsourcedGameIds != null && state.OutsourcedGameIds.Count > 0)
            {
                publisherScript partner = FindStudioByID(mS, kvp.Key);
                if (partner == null) continue;

                string partnerName = SafeStudioName(partner);
                int subdiv = GetSubdivision(partner);
                string subdivName = GetSubdivisionName(subdiv);

                foreach (int gameId in state.OutsourcedGameIds)
                {
                    gameScript game = null;
                    if (gamesScript != null && gamesScript.arrayGamesScripts != null)
                    {
                        for (int i = 0; i < gamesScript.arrayGamesScripts.Length; i++)
                        {
                            if (gamesScript.arrayGamesScripts[i] != null && gamesScript.arrayGamesScripts[i].myID == gameId)
                            {
                                game = gamesScript.arrayGamesScripts[i];
                                break;
                            }
                        }
                    }
                    if (game == null) continue;

                    string gameName = game.myName ?? "Unknown";
                    string gameSize = SafeGetGameSizeString(game);
                    string progressStr;
                    string progressColor;

                    if (game.isOnMarket) continue; // Remove released games
                    if (!game.inDevelopment) continue; // Only show in-dev
                    {
                        float pct = 0f;
                        int weeksLeft = partner.newGameInWeeks;
                        int totalWeeks = partner.newGameInWeeksORG;
                        if (totalWeeks > 0)
                        {
                            pct = Mathf.Clamp01(1f - ((float)weeksLeft / totalWeeks));
                        }
                        progressStr = $"In Dev: {pct * 100f:F0}% | ~{weeksLeft}w to release";
                        progressColor = "#4FC3F7";
                    }

                    string detail = $"<b>Outsourced Project</b> to <b>{partnerName}</b> ({subdivName})\n" +
                                    $"Game: {gameName} [{gameSize}]\n" +
                                    $"<color={progressColor}>{progressStr}</color>";

                    AddPartnershipEntry(detail, font, new Color(0.2f, 0.2f, 0.4f, 0.9f));
                    foundAny = true;
                }
            }

            // Queued outsource project
            if (state.HasQueuedProject)
            {
                publisherScript partner = FindStudioByID(mS, kvp.Key);
                if (partner == null) continue;

                string partnerName = SafeStudioName(partner);
                int subdiv = GetSubdivision(partner);
                string subdivName = GetSubdivisionName(subdiv);

                string detail = $"<b>Queued Outsource Project</b> with <b>{partnerName}</b> ({subdivName})\n" +
                                $"Game: {state.QueuedName} | <color=#FFB74D>Waiting for studio to finish current project</color>";

                AddPartnershipEntry(detail, font, new Color(0.3f, 0.25f, 0.1f, 0.9f));
                foundAny = true;
            }
        }

        // 4. Active Commissions from Platform Holders
        foreach (var kvp in states)
        {
            StudioState state = kvp.Value;
            if (state.CommissionWeeksLeft <= 0) continue;

            publisherScript partner = FindStudioByID(mS, kvp.Key);
            if (partner == null) continue;

            string partnerName = SafeStudioName(partner);
            int subdiv = GetSubdivision(partner);
            string subdivName = GetSubdivisionName(subdiv);

            string detail = $"<b>Development Commission</b> from <b>{partnerName}</b> ({subdivName})\n" +
                            $"<color=#FFB74D>Time left: {state.CommissionWeeksLeft}w</color> | Target review: {state.CommissionTargetReview}+ | Reward: {mS.GetMoney(state.CommissionReward, true)}";

            AddPartnershipEntry(detail, font, new Color(0.35f, 0.2f, 0.35f, 0.9f));
            foundAny = true;
        }

        // 5. Games we are publishing through NPC publishers (in-dev only, removed on release)
        if (gamesScript != null && gamesScript.arrayGamesScripts != null)
        {
            for (int i = 0; i < gamesScript.arrayGamesScripts.Length; i++)
            {
                gameScript game = gamesScript.arrayGamesScripts[i];
                if (game == null || game.developerID != mS.myID) continue;
                if (game.publisherID == mS.myID || game.publisherID < 0) continue;
                if (gameActiveContracts.ContainsKey(game.myID)) continue;
                if (!game.inDevelopment || game.isOnMarket) continue; // Only show in-dev, remove on release

                publisherScript pub = FindStudioByID(mS, game.publisherID);
                if (pub == null) continue;

                string pubName = SafeStudioName(pub);
                int subdiv = GetSubdivision(pub);
                string subdivName = GetSubdivisionName(subdiv);
                string gameName = game.myName ?? "Unknown";
                string gameSize = SafeGetGameSizeString(game);
                string genreStr = SafeGetGenreString(game);

                publisherScript playerStudio = FindStudioByID(mS, mS.myID);
                float pct = 0f;
                int weeksLeft = playerStudio != null ? playerStudio.newGameInWeeks : 0;
                int totalWeeks = playerStudio != null ? playerStudio.newGameInWeeksORG : 0;
                if (totalWeeks > 0) pct = Mathf.Clamp01(1f - ((float)weeksLeft / totalWeeks));
                string progressStr = $"In Dev: {pct * 100f:F0}% | ~{weeksLeft}w to release";

                string dealType = "Publishing Deal";
                if (game.exklusiv) dealType = "Exclusive Publishing Deal";
                if (game.inGamePass) dealType += " + Game Pass";

                string detail = $"<b>{dealType}</b> with <b>{pubName}</b> ({subdivName})\n" +
                                $"Game: {gameName} [{gameSize}] {genreStr}\n" +
                                $"<color=#4FC3F7>{progressStr}</color>";

                if (game.exklusiv) detail += " | <color=#FFD54F>EXCLUSIVE</color>";
                if (game.inGamePass) detail += " | <color=#A5D6A7>Game Pass</color>";

                AddPartnershipEntry(detail, font, new Color(0.15f, 0.2f, 0.3f, 0.9f));
                foundAny = true;
            }
        }

        // 6. Games we are publishing for NPC developers (in-dev only, removed on release)
        if (gamesScript != null && gamesScript.arrayGamesScripts != null)
        {
            for (int i = 0; i < gamesScript.arrayGamesScripts.Length; i++)
            {
                gameScript game = gamesScript.arrayGamesScripts[i];
                if (game == null || game.publisherID != mS.myID) continue;
                if (game.developerID == mS.myID || game.developerID < 0) continue;
                if (!game.inDevelopment || game.isOnMarket) continue; // Only show in-dev, remove on release

                publisherScript dev = FindStudioByID(mS, game.developerID);
                if (dev == null) continue;

                string devName = SafeStudioName(dev);
                int subdiv = GetSubdivision(dev);
                string subdivName = GetSubdivisionName(subdiv);
                string gameName = game.myName ?? "Unknown";
                string gameSize = SafeGetGameSizeString(game);
                string genreStr = SafeGetGenreString(game);

                float pct = 0f;
                int weeksLeft = dev.newGameInWeeks;
                int totalWeeks = dev.newGameInWeeksORG;
                if (totalWeeks > 0) pct = Mathf.Clamp01(1f - ((float)weeksLeft / totalWeeks));
                string progressStr = $"In Dev: {pct * 100f:F0}% | ~{weeksLeft}w to release";

                string dealType = "Publishing For";
                if (game.exklusiv) dealType = "Exclusive Publishing For";
                if (game.inGamePass) dealType += " | Game Pass";

                string detail = $"<b>{dealType}</b> <b>{devName}</b> ({subdivName})\n" +
                                $"Game: {gameName} [{gameSize}] {genreStr}\n" +
                                $"<color=#4FC3F7>{progressStr}</color>";

                if (game.exklusiv) detail += " | <color=#FFD54F>EXCLUSIVE</color>";
                if (game.inGamePass) detail += " | <color=#A5D6A7>Game Pass</color>";

                AddPartnershipEntry(detail, font, new Color(0.2f, 0.3f, 0.2f, 0.9f));
                foundAny = true;
            }
        }

        if (!foundAny)
        {
            AddPartnershipEntry("<i>No active partnerships or deals found.</i>\nUse the Partnerships & Perks panel on studio detail screens to start deals.", font, new Color(0.15f, 0.15f, 0.15f, 0.8f));
        }
    }


    private static void AddPartnershipEntry(string text, Font font, Color bgColor)
    {
        if (partnershipsScrollContent == null) return;

        GameObject entry = new GameObject("Entry", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        entry.transform.SetParent(partnershipsScrollContent.transform, false);

        Image entryImg = entry.GetComponent<Image>();
        entryImg.color = bgColor;
        entryImg.material = null;

        LayoutElement le = entry.AddComponent<LayoutElement>();
        le.preferredHeight = 70f;
        le.flexibleWidth = 1f;

        Text entryText = new GameObject("Text", typeof(RectTransform), typeof(Text)).GetComponent<Text>();
        entryText.transform.SetParent(entry.transform, false);
        RectTransform rt = entryText.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = new Vector2(10f, 5f);
        rt.offsetMax = new Vector2(-10f, -5f);
        entryText.font = font;
        entryText.fontSize = 12;
        entryText.color = Color.white;
        entryText.alignment = TextAnchor.MiddleLeft;
        entryText.supportRichText = true;
        entryText.horizontalOverflow = HorizontalWrapMode.Wrap;
        entryText.verticalOverflow = VerticalWrapMode.Overflow;
        entryText.text = text;
    }

    private static void ClosePartnershipsPanel()
    {
        if (partnershipsPanel != null)
        {
            UnityEngine.Object.Destroy(partnershipsPanel);
            partnershipsPanel = null;
        }
        partnershipsScrollContent = null;
    }

    private static string GetContractLabel(string contractType)
    {
        switch (contractType)
        {
            case "EXCL_PH_FUND": return "Platform Holder Exclusivity + Full Funding";
            case "AAA_GIFT_IP": return "AAA Co-Publish (Gifted IP)";
            case "AAA_THEIR_IP": return "AAA Co-Publish (Their IP)";
            case "AAA_KEEP_IP": return "AAA Co-Publish (Keep IP)";
            case "IP_COOP_LOW": return "IP Cooperation (Low Funding)";
            case "IP_COOP_HIGH": return "IP Cooperation (High Funding)";
            case "AAACoPublish": return "AAA Co-Publishing Deal";
            case "CoPublish": return "Co-Publishing Deal";
            default: return contractType;
        }
    }

    private static Color GetContractColor(string contractType)
    {
        switch (contractType)
        {
            case "EXCL_PH_FUND": return new Color(0.4f, 0.2f, 0.15f, 0.9f);
            case "AAA_GIFT_IP":
            case "AAA_THEIR_IP":
            case "AAA_KEEP_IP":
            case "AAACoPublish": return new Color(0.2f, 0.25f, 0.4f, 0.9f);
            case "IP_COOP_LOW":
            case "IP_COOP_HIGH": return new Color(0.2f, 0.35f, 0.25f, 0.9f);
            default: return new Color(0.2f, 0.2f, 0.25f, 0.9f);
        }
    }

    private static string SafeGetGameSizeString(gameScript game)
    {
        try
        {
            if (game == null) return "?";
            return game.GetGamesizeString();
        }
        catch { return "?"; }
    }

    private static string SafeGetGenreString(gameScript game)
    {
        try
        {
            if (game == null) return "";
            string s = game.GetGenreString();
            if (game.subgenre != -1) s += " / " + game.GetSubGenreString();
            return s;
        }
        catch { return ""; }
    }

    private void Update()

    {
        CleanupStaleDesignContext();
        UpdateContractsWeekly();

        if (Input.GetKeyDown(KeyCode.P) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            if (partnershipsPanel != null)
            {
                ClosePartnershipsPanel();
            }
            else
            {
                ShowPartnershipsPanel();
            }
        }
    }

    private static mainScript GetMainScriptCached()
    {
        if (cachedMainScript == null || !cachedMainScript)
        {
            cachedMainScript = UnityEngine.Object.FindObjectOfType<mainScript>();
        }
        return cachedMainScript;
    }

    private static GUI_Main GetGuiMainCached()
    {
        if (cachedGuiMain == null || !cachedGuiMain)
        {
            cachedGuiMain = UnityEngine.Object.FindObjectOfType<GUI_Main>();
        }
        return cachedGuiMain;
    }

    private static games GetGamesCached()
    {
        if (cachedGamesScript == null || !cachedGamesScript)
        {
            cachedGamesScript = UnityEngine.Object.FindObjectOfType<games>();
        }
        return cachedGamesScript;
    }

    private static void UpdateRivalryCheck(mainScript mS)
    {
        if (mS == null || !mS || !mS.officeLoaded) return;
        if (PlayerHasActiveConsole(mS))
        {
            bool modified = false;
            foreach (publisherScript pS in mS.arrayPublisherScripts)
            {
                if (pS != null && !pS.isPlayer && GetSubdivision(pS) == 1)
                {
                    if (pS.relation > 20f)
                    {
                        pS.relation = 20f;
                        modified = true;
                    }
                }
            }
            if (modified)
            {
                GUI_Main gui = GetGuiMainCached();
                if (gui != null)
                {
                    gui.CreateTopNewsInfo("Platform holders have locked relationships to 1 Star due to your console release!");
                }
                SaveState();
            }
        }
    }

    // Weekly countdown for active agreements
    private int lastCheckedWeek = -1;
    private static Dictionary<int, long> lastRefundedGameCosts = new Dictionary<int, long>();

    private static float GetCoFundingPercentage(gameScript game)
    {
        if (game == null || !game.inDevelopment) return 0f;
        mainScript mS = GetMainScriptCached();
        if (mS == null) return 0f;

        publisherScript pub = FindStudioByID(mS, game.publisherID);
        if (pub == null || pub.isPlayer) return 0f;

        string contractType;
        if (gameActiveContracts.TryGetValue(game.myID, out contractType))
        {
            if (contractType == "EXCL_PH_FUND") return 1.0f; // 100% funding
            if (contractType == "AAA_GIFT_IP") return 1.0f; // 100% funding
            if (contractType == "AAA_THEIR_IP") return 0.75f; // 75% funding
            if (contractType == "AAA_KEEP_IP") return 0.5f; // 50% funding
            if (contractType == "IP_COOP_LOW") return 0.2f; // 20% funding
            if (contractType == "IP_COOP_HIGH")
            {
                int subdiv = GetSubdivision(pub);
                return subdiv == 3 ? 0.4f : 0.8f; // 40% (Small Pub) vs 80% (Big Pub)
            }
        }

        // Fallback to royalty-based calculation
        int subdivPH = GetSubdivision(pub);
        float royalty = game.pubAngebot_Gewinnbeteiligung;

        if (subdivPH == 1) // Platform Holder
        {
            if (game.exklusiv && Mathf.Approximately(royalty, 5f))
            {
                return 1.0f; // 100% funding
            }
        }
        else if (subdivPH == 2) // Big Publisher
        {
            if (Mathf.Approximately(royalty, 25f)) return 0.8f; // 80% funding (IP Cooperation High)
            if (Mathf.Approximately(royalty, 20f)) return 1.0f; // 100% funding (AAA Co-Publish Gift IP)
            if (Mathf.Approximately(royalty, 12f)) return 0.75f; // 75% funding (AAA Co-Publish Their IP)
            if (Mathf.Approximately(royalty, 10f)) return 0.5f; // 50% funding (AAA Co-Publish Keep IP or IP Cooperation Low)
        }
        else if (subdivPH == 3) // Small Publisher
        {
            if (Mathf.Approximately(royalty, 25f)) return 0.4f; // 40% funding (IP Cooperation High)
            if (Mathf.Approximately(royalty, 10f)) return 0.2f; // 20% funding (IP Cooperation Low)
        }
        return 0f;
    }

    private void UpdateContractsWeekly()
    {
        mainScript mS = GetMainScriptCached();
        if (mS != null && mS.officeLoaded && GetGlobalWeek(mS) != lastCheckedWeek)
        {
            int currentWeek = GetGlobalWeek(mS);
            lastCheckedWeek = currentWeek;
            UpdateRivalryCheck(mS);

            bool stateChanged = false;
            foreach (var kvp in states.Values.ToList())
            {
                // Tick yearly contract timer if active
                if (kvp.ContractWeeksLeft > 0)
                {
                    kvp.ContractWeeksLeft--;
                    stateChanged = true;

                    // Tick yearly timer
                    if (kvp.ContractYearlyTimer > 0)
                    {
                        kvp.ContractYearlyTimer--;
                    }

                    // AutoPublish weekly check: assign player as publisher for their in-development games
                    if (kvp.ActiveContract == "AutoPublish" || kvp.ActiveContract == "ConsoleAutoPublish")
                    {
                        publisherScript dev = FindStudioByID(mS, kvp.StudioId);
                        if (dev != null)
                        {
                            gameScript inDev = dev.FindGameInDevelopment();
                            if (inDev != null)
                            {
                                if (inDev.publisherID != mS.myID)
                                {
                                    inDev.publisherID = mS.myID;
                                    inDev.pS_ = null;
                                    inDev.pubAngebot_Gewinnbeteiligung = 15f; // Good publishing deal (15%)
                                }

                                if (kvp.ActiveContract == "ConsoleAutoPublish")
                                {
                                    inDev.exklusiv = true;
                                    inDev.gamePlatform[0] = GetPlayerPlatformID(mS);
                                    for (int i = 1; i < 4; i++) inDev.gamePlatform[i] = -1;

                                    if (mS.gpS_ != null && mS.gpS_.gamePass_aktiv && !inDev.inGamePass)
                                    {
                                        mS.gpS_.GAMEPASS_AddGame(inDev, true);
                                    }
                                }
                            }
                        }

                        // Yearly timer payment triggering
                        if (kvp.ContractYearlyTimer <= 0)
                        {
                            kvp.ContractYearlyTimer = 48; // reset to 48 weeks

                            publisherScript devStudio = FindStudioByID(mS, kvp.StudioId);
                            if (devStudio != null)
                            {
                                long fee = CalculateAutoPublishYearlyFee(devStudio);
                                mS.Pay(fee, 10);
                                AdjustRelation(devStudio, 5f); // +5 relationship points passively every year
                                
                                GUI_Main gui = GetGuiMainCached();
                                if (gui != null)
                                {
                                    gui.CreateTopNewsInfo($"Paid yearly contract fee of {mS.GetMoney(fee, true)} to {devStudio.GetName()}. Relationship increased passively (+5).");
                                }
                            }
                        }
                    }

                    if (kvp.ContractWeeksLeft <= 0)
                    {
                        kvp.ActiveContract = "";
                        kvp.ContractYearlyTimer = 0;
                        publisherScript pS = FindStudioByID(mS, kvp.StudioId);
                        if (pS != null)
                        {
                            GUI_Main gui = GetGuiMainCached();
                            if (gui != null)
                            {
                                gui.CreateTopNewsInfo("Contract expired with " + pS.GetName() + ".");
                            }
                        }
                    }
                }

                // Tick Active Commissions
                if (kvp.CommissionWeeksLeft > 0)
                {
                    kvp.CommissionWeeksLeft--;
                    stateChanged = true;

                    if (kvp.CommissionWeeksLeft <= 0)
                    {
                        publisherScript PH = FindStudioByID(mS, kvp.StudioId);
                        if (PH != null)
                        {
                            AdjustRelation(PH, -15f); // Failure penalty
                            GUI_Main gui = GetGuiMainCached();
                            if (gui != null)
                            {
                                gui.MessageBox($"Commission expired with {PH.GetName()}! Failed to complete project in time (-15 Relationship).", false);
                            }
                        }
                    }
                }

                // Random Commission Offers from Platform Holders
                if (kvp.CommissionWeeksLeft <= 0)
                {
                    publisherScript PH = FindStudioByID(mS, kvp.StudioId);
                    if (PH != null && GetSubdivision(PH) == 1 && PH.GetRelation() >= 40f && !PlayerHasActiveConsole(mS))
                    {
                        if (UnityEngine.Random.Range(0, 100) < 2) // 2% weekly chance
                        {
                            int genre = UnityEngine.Random.Range(0, 10); // pick a random genre
                            int size = UnityEngine.Random.Range(1, 4); // B+ to AA
                            int targetReview = UnityEngine.Random.Range(70, 86); // 70% to 85%
                            long reward = 250000L * (size + 1);

                            ShowCommissionOfferDialog(PH, genre, size, targetReview, reward);
                        }
                    }
                }

                // IP Burying check for Indie Developers
                if (kvp.LastIPBuyWeek > 0 && currentWeek - kvp.LastIPBuyWeek >= 96)
                {
                    kvp.LastIPBuyWeek = -999; // reset so it only triggers once
                    stateChanged = true;

                    publisherScript dev = FindStudioByID(mS, kvp.StudioId);
                    if (dev != null)
                    {
                        AdjustRelation(dev, -15f); // Buy IP and bury it penalty
                        GUI_Main gui = GetGuiMainCached();
                        if (gui != null)
                        {
                            gui.CreateTopNewsInfo($"{dev.GetName()} relationship decreased by -15 because you bought their IP and buried it.");
                        }
                    }
                }
            }

            // Engine license abuse weekly check for Mid Developers
            foreach (publisherScript pub in mS.arrayPublisherScripts)
            {
                if (pub != null && !pub.isPlayer && GetSubdivision(pub) == 5 && processedEngineLicenses.Contains(pub.myID))
                {
                    bool hasAbused = false;
                    for (int e = 0; e < mS.arrayEnginesScripts.Length; e++)
                    {
                        engineScript eng = mS.arrayEnginesScripts[e];
                        if (eng != null && eng.myID == mS.myID && eng.sellEngine && eng.gewinnbeteiligung > 10)
                        {
                            hasAbused = true;
                            break;
                        }
                    }
                    if (hasAbused)
                    {
                        int abuseHash = pub.myID * 10000 + 999;
                        if (!processedFailures.Contains(abuseHash))
                        {
                            processedFailures.Add(abuseHash);
                            AdjustRelation(pub, -15f); // Engine abuse penalty
                            stateChanged = true;

                            GUI_Main gui = GetGuiMainCached();
                            if (gui != null)
                            {
                                gui.CreateTopNewsInfo($"{pub.GetName()} reduced relationship by -15 due to high engine royalties (>10%).");
                            }
                        }
                    }
                }
            }

            // Co-funding reimbursement weekly update
            games g = GetGamesCached();
            if (g != null && g.arrayGamesScripts != null)
            {
                List<int> activeInDevIds = new List<int>();
                for (int i = 0; i < g.arrayGamesScripts.Length; i++)
                {
                    gameScript game = g.arrayGamesScripts[i];
                    if (game != null && game.inDevelopment && game.developerID == mS.myID)
                    {
                        activeInDevIds.Add(game.myID);
                        long currentCosts = game.costs_entwicklung + game.costs_mitarbeiter;
                        float refundPercent = GetCoFundingPercentage(game);

                        if (refundPercent > 0f)
                        {
                            long lastCosts = 0L;
                            if (lastRefundedGameCosts.TryGetValue(game.myID, out lastCosts))
                            {
                                if (currentCosts > lastCosts)
                                {
                                    long diff = currentCosts - lastCosts;
                                    long refund = (long)(diff * refundPercent);
                                    if (refund > 0)
                                    {
                                        mS.Earn(refund, 10);
                                    }
                                }
                            }
                            else
                            {
                                // Initial reimbursement
                                long refund = (long)(currentCosts * refundPercent);
                                if (refund > 0)
                                {
                                    mS.Earn(refund, 10);
                                }
                            }
                            lastRefundedGameCosts[game.myID] = currentCosts;
                        }
                    }
                }

                // Cleanup released/deleted games from cache
                List<int> cachedIds = lastRefundedGameCosts.Keys.ToList();
                foreach (int cid in cachedIds)
                {
                    if (!activeInDevIds.Contains(cid))
                    {
                        lastRefundedGameCosts.Remove(cid);
                    }
                }
            }

            if (stateChanged)
            {
                SaveState();
            }
        }
    }

    public static int GetGlobalWeek(mainScript mS)
    {
        if (mS == null) return 0;
        return mS.year * 48 + mS.month * 4 + mS.week;
    }

    public static int GetSubdivision(publisherScript pS)
    {
        try
        {
            if (pS == null || !pS || pS.isPlayer) return 0;

            if (pS.publisher && pS.ownPlatform)
            {
                return 1; // Platform Holder
            }
            if (pS.publisher && !pS.ownPlatform)
            {
                return pS.stars >= 70f ? 2 : 3; // Big Publisher vs Small Publisher
            }
            if (!pS.publisher && pS.developer)
            {
                if (pS.stars >= 75f) return 4; // Big Developer
                if (pS.stars >= 40f) return 5; // Mid Developer
                return 6; // Indie Developer
            }
        }
        catch (Exception)
        {
            // Safeguard against destroyed/uninitialized Unity objects
        }
        return 0;
    }

    public static string GetSubdivisionName(int subdiv)
    {
        switch (subdiv)
        {
            case 1: return "Platform Holder";
            case 2: return "Big Publisher";
            case 3: return "Small/Indie Publisher";
            case 4: return "Big Developer";
            case 5: return "Mid-Sized Developer";
            case 6: return "Indie Developer";
            default: return "Unknown Studio Type";
        }
    }

    private static string GetPerkTooltipText(int starRating, int subdiv)
    {
        if (subdiv == 1) // Platform Holders
        {
            switch (starRating)
            {
                case 1: return "<b>Gameplay Instant-Unlock</b>\n\nInstantly unlocks a free gameplay feature element without needing any time, a research room, or money.\n\n<b>Cooldown</b>: Once every 2 months (8 weeks).\n<b>Gain</b>: +2 Relationship points.";
                case 2: return "<b>Engine Instant-Unlock</b>\n\nInstantly unlocks a free engine research element without needing any time, a research room, or money.\n\n<b>Cooldown</b>: Once every 3 months (12 weeks).\n<b>Gain</b>: +3 Relationship points.";
                case 3: return "<b>Tech Sharing / Publishing Deal</b>\n\nAct as publisher for your game at a premium royalty rate better than any other NPC publisher, even if it's on other platforms (marketing rights deal). Can be a brand new project (goes to game creation menu) or an in-development project.\n\n<b>Cooldown</b>: Once every 3 years (144 weeks).\n<b>Gain</b>: +10 Relationship points.";
                case 4: return "<b>Exclusivity / Full Funding</b>\n\nAbility to make 1 game exclusive to their platform. They cover the entire funding/budget of the game (reimburse 100% of money spent on the project each month). They publish the game, and it must remain exclusive. Can be a brand new project or in-development project.\n\n<b>Cancellation Penalty</b>: If the game is in development and multiplatform, all other platforms are cancelled.\n\n<b>Cooldown</b>: Once every 5 years (240 weeks).\n<b>Gain</b>: +15 Relationship points.";
                case 5: return "<b>First-Party Partner / Debt Bailout</b>\n\nInstantly bail out all player debt and receive cash to fund your next game. In exchange, they take all of your game IPs.\n\n<b>Limit</b>: Only once per playthrough.\n<b>Gain</b>: +30 Relationship points.";
            }
        }
        else if (subdiv == 2) // Big Publishers
        {
            switch (starRating)
            {
                case 1: return "<b>Topic Instant-Unlock</b>\n\nInstantly unlocks a free topic element without needing any time, a research room, or money.\n\n<b>Cooldown</b>: Once every month (4 weeks).\n<b>Gain</b>: +1 Relationship point.";
                case 2: return "<b>Trusted Partner / Signing Bonus</b>\n\nBetter publishing royalty rate than standard + a signing bonus depending on game size (B, B+, A, AA, AAA, or AAAA). Can be a brand new project or an in-development project.\n\n<b>Cooldown</b>: Once every 2 years (96 weeks).\n<b>Gain</b>: +8 Relationship points.";
                case 3: return "<b>IP Cooperation / Partial Funding</b>\n\nUse their IPs to make sequels/spin-offs. They own the IP. Player chooses the funding % they want to receive (publisher returns that % of monthly costs). The higher the funding %, the worse the publishing royalties the player receives (and vice versa).\n\n<b>Cooldown</b>: Once every 3 years (144 weeks).\n<b>Gain</b>: +12 Relationship points.";
                case 4: return "<b>AAA Co-Publishing Pitch</b>\n\nPitch a game using their IP (sequel/spin-off), our IP (sequel/spin-off), or a new IP. They fund 50-100% of the game costs (returned each month).\n\n1. <i>New IP</i>: Option to give them the IP rights for 100% funding and good royalties, or keep the IP for 50% funding and slightly worse royalties.\n2. <i>Player IP</i>: Keep IP, get 50% funding and a fair royalty deal.\n3. <i>Their IP</i>: Get 75% funding and a good royalty deal.\n\n<b>Note</b>: Brand new projects only.\n<b>Cooldown</b>: Once every 3 years (144 weeks).\n<b>Gain</b>: +15 Relationship points (plus another +20 points if you gift them a new IP).";
                case 5: return "<b>Acquisition & Consolidation</b>\n\n1. Ability to fully acquire the studio as a subsidiary.\n2. Full right to make games in any of their IPs with good royalties and 50-100% funding (depending on IP importance) (cooldown: Once every 5 years / 240 weeks).\n3. <i>If player has console</i>: Force their in-development game to be exclusive to your console by paying an upfront fee, or put their games on your subscription service for a lower but high fee (cooldown: Once every 3 years / 144 weeks).\n\n<b>Gain</b>: +20 Relationship points for exclusivity/subscription deals.";
            }
        }
        else if (subdiv == 3) // Small Publishers
        {
            switch (starRating)
            {
                case 1: return "<b>Basic Contact</b>\n\nGive you a standard publishing offer, and publishing with them increases relationship.\n\n<b>Gain</b>: +3 Relationship points per published game.";
                case 2: return "<b>Indie Sponsor</b>\n\nBetter publishing rate + upfront signing bonus depending on IP and game size (B to AAAA).\n\n<b>Gain</b>: +5 Relationship points.";
                case 3: return "<b>First-Look / IP Trades & Buyouts</b>\n\nThey always offer games to you to publish first. They are willing to buy player's IPs at a good rate. Player can use their IPs to make sequels/spin-offs (max 40% funding).\n\n<b>Cooldown</b>: Once every 3 years (144 weeks).\n<b>Gain</b>: +10 Relationship points (or +15 points if you buy their IP).";
                case 4: return "<b>Exclusivity & Subscription Service</b>\n\n<i>If player has console</i>: Force their in-development game to be exclusive to your console by paying an upfront fee, or put their games on your subscription service for a lower fee.\n\n<b>Cooldown</b>: Once every 2 years (96 weeks).\n<b>Gain</b>: +15 Relationship points.";
                case 5: return "<b>Friendly Absorption</b>\n\nFully buy out and acquire the entire studio.\n\n<b>Gain</b>: +25 Relationship points.";
            }
        }
        else if (subdiv == 4) // Big Developers
        {
            switch (starRating)
            {
                case 1: return "<b>Basic Dev</b>\n\nAct as publisher for their in-development game. If it does well, gain extra relationship points. Must offer a good upfront amount or a good royalty deal (or balanced).\n\n<b>Gain</b>: +5 Relationship points.";
                case 2: return "<b>Preferred Publisher / IP Licensing</b>\n\nThey allow us to make games in their IP. We publish it if we have a publishing room; if not, they decide the publisher and we get a good publishing deal. They also provide 15% - 35% funding depending on game size and IP value.\n\n<b>Gain</b>: +10 Relationship points.";
                case 3: return "<b>Outsourcing & IP Purchase</b>\n\n1. <i>Outsourcing</i>: Assign them a project based on our IP or a new IP we will own. Player fully funds the project with a lump sum payment.\n2. <i>IP Purchase</i>: Option to buy any one of their game IPs outright at a good rate (limit: Once every 10 years / 480 weeks).\n3. <i>In-Dev Queue System</i>: Cancel current project (slight relation penalty -10 points) or Queue project (relation boost +15 points upon completion). Gift new IP: +35 relationship points.";
                case 4: return "<b>Automatic Publication Contract</b>\n\nAll of their developed games are automatically published by us. We pay them a yearly amount (based on their Goodwill, IP value, and trend from Goodwill mod) and offer them a good publishing deal.\n\n<b>Gain</b>: +10 Relationship points initially, and +5 points passively every year.";
                case 5: return "<b>Friendly Acquisition / IP Buyout</b>\n\nOption to acquire the entire studio as a subsidiary with a 30% discount on firm value, or buy any one of their IPs (IP purchase cooldown: Once every 10 years / 480 weeks).\n\n<b>Gain on Acquisition</b>: +20 Relationship points.";
            }
        }
        else if (subdiv == 5) // Mid Developers
        {
            switch (starRating)
            {
                case 1: return "<b>Standard Dev</b>\n\nAct as publisher for their in-development game. If it does well, gain extra relationship points. Must offer a good upfront amount or a good royalty deal (or balanced).\n\n<b>Gain</b>: +4 Relationship points.";
                case 2: return "<b>Full Project Financing (Outsourcing)</b>\n\nAssign them a project based on our IP or a new IP we will own. Player fully funds the project with a lump sum payment (based on studio stars, Goodwill reputation, size of game, IP value, and surplus).\n\n<i>In-Dev Queue System</i>:\n1. <i>Cancel Current</i>: Pay extra to force them to cancel their current project to work on ours (slight relationship penalty -10 points).\n2. <i>Queue System</i>: Put project in a queue; they start once done (relationship gain boost +15 points when completed).\n3. <i>IP Gift</i>: If it is a new IP, you can let them keep the IP for a massive relationship boost (+40 points).";
                case 3: return "<b>Console Exclusivity & Subscription Service</b>\n\n<i>If player has console</i>: Make their in-development game exclusive to your console by paying an upfront fee, or put their games on your subscription service for a lower fee.\n\n<b>Cooldown</b>: Once every 3 years (144 weeks).\n<b>Gain</b>: +12 Relationship points.";
                case 4: return "<b>Automatic Publication Contract</b>\n\nAll of their developed games are automatically published by us. We pay them a yearly amount (based on their Goodwill, IP value, and trend from Goodwill mod) and offer them a good publishing deal.\n\n<b>Gain</b>: +10 Relationship points initially, and +5 points passively every year.";
                case 5: return "<b>Acquisition Option</b>\n\nAcquire the entire studio as a subsidiary with a 40% discount, or buy any one of their IPs (IP purchase cooldown: Once every 10 years / 480 weeks).\n\n<b>Gain on Acquisition</b>: +20 Relationship points.";
            }
        }
        else if (subdiv == 6) // Indie Developers
        {
            switch (starRating)
            {
                case 1: return "<b>In-Development Publisher</b>\n\nAct as publisher for their in-development game. If it does well, gain extra relationship points. Must offer a good upfront amount or a good royalty deal (or balanced).\n\n<b>Gain</b>: +5 Relationship points.";
                case 2: return "<b>Full Project Financing (Outsourcing)</b>\n\nAssign them a project based on our IP or a new IP we will own. Player fully funds the project with a lump sum payment.\n\n<i>In-Dev Queue System</i>:\n1. <i>Cancel Current</i>: Pay extra to force them to cancel their current project to work on ours (slight relationship penalty -10 points).\n2. <i>Queue System</i>: Put project in a queue; they start once done (relationship gain boost +15 points when completed).\n3. <i>IP Gift</i>: If it is a new IP, you can let them keep the IP for a massive relationship boost (+40 points).";
                case 3: return "<b>Automatic Publication Contract</b>\n\nAll of their developed games are automatically published by us. We pay them a yearly amount (based on their Goodwill, IP value, and trend from Goodwill mod) and offer them a good publishing deal.\n\n<b>Gain</b>: +10 Relationship points initially, and +5 points passively every year.";
                case 4: return "<b>Console Exclusivity & Subscription Service</b>\n\nAll of their games exclusive to our console plus day one on our subscription service (only if we have a console in market or have an active subscription service). We pay them a yearly amount (based on their Goodwill, IP value, and trend from Goodwill mod) and offer them a good publishing deal.\n\n<b>Gain</b>: +15 Relationship points initially, and +5 points passively every year.";
                case 5: return "<b>Acquire Studio</b>\n\nAbility to acquire the entire studio as a subsidiary.\n\n<b>Gain on Acquisition</b>: +20 Relationship points.";
            }
        }
        return "";
    }

    public static bool PlayerHasActiveConsole(mainScript mS)
    {
        if (mS == null || mS.arrayPlatformsScripts == null) return false;
        for (int i = 0; i < mS.arrayPlatformsScripts.Length; i++)
        {
            platformScript plat = mS.arrayPlatformsScripts[i];
            if (plat != null && plat.ownerID == mS.myID && plat.isUnlocked && !plat.vomMarktGenommen && (plat.typ == 0 || plat.typ == 1 || plat.typ == 2))
            {
                return true;
            }
        }
        return false;
    }

    public static bool PlayerHasActiveSubscription(mainScript mS)
    {
        return mS != null && mS.gpS_ != null && mS.gpS_.gamePass_aktiv;
    }

    public static StudioState EnsureState(int studioId)
    {
        StudioState state;
        if (!states.TryGetValue(studioId, out state))
        {
            state = new StudioState { StudioId = studioId };
            states[studioId] = state;
        }
        return state;
    }

    private static publisherScript FindStudioByID(mainScript mainScript, int id)
    {
        if (id < 0) return null;
        if (mainScript != null && mainScript.arrayPublisherScripts != null)
        {
            for (int i = 0; i < mainScript.arrayPublisherScripts.Length; i++)
            {
                publisherScript studio = mainScript.arrayPublisherScripts[i];
                if (studio != null && studio.myID == id)
                {
                    return studio;
                }
            }
        }
        return UnityEngine.Object.FindObjectsOfType<publisherScript>().FirstOrDefault(studio => studio != null && studio.myID == id);
    }

    private static long SafeGetStudioFirmValueForList(publisherScript studio)
    {
        return EstimateStudioFirmValue(studio);
    }

    private static long EstimateStudioFirmValue(publisherScript studio)
    {
        if (studio == null || !studio) return 0L;

        long baseValue = studio.firmenwert;
        if (baseValue <= 0L)
        {
            baseValue = Math.Max(1L, (long)(studio.stars * 100000L));
        }

        if (baseValue > 30000000L)
        {
            baseValue = 30000000L;
        }

        mainScript main = GetMainScriptCached();
        long yearFactor = main != null ? Math.Max(0L, Math.Min(50L, (long)main.year - 1975L)) : 1L;
        long estimate = baseValue * Math.Max(1L, yearFactor);
        if (studio.ownPlatform)
        {
            estimate *= 3L;
        }

        if (main != null)
        {
            switch (main.difficulty)
            {
                case 0: estimate /= 5L; break;
                case 1: estimate /= 4L; break;
                case 2: estimate /= 3L; break;
                case 3: estimate /= 2L; break;
                case 5: estimate *= 2L; break;
            }
        }

        return Math.Max(0L, estimate);
    }

    // TSV state parser (No Json assembly reference dependencies)
    private static void LoadState()
    {
        states.Clear();
        processedReleasedGames.Clear();
        processedHitGames.Clear();
        processedMasterpieces.Clear();
        processedHighSales.Clear();
        processedExclusiveGames.Clear();
        processedContractGames.Clear();
        processedCommissions.Clear();
        processedFailures.Clear();
        processedCancelledContracts.Clear();
        processedBoughtIPs.Clear();
        processedGiftedIPs.Clear();
        processedEngineLicenses.Clear();
        processedCoDevelopments.Clear();
        gameActiveContracts.Clear();

        if (!File.Exists(statePath))
        {
            return;
        }

        try
        {
            string[] lines = File.ReadAllLines(statePath);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                {
                    continue;
                }

                if (line.StartsWith("GLOBAL_TRACKER"))
                {
                    string[] trackerParts = line.Split('\t');
                    if (trackerParts.Length >= 3)
                    {
                        string name = trackerParts[1];
                        string val = trackerParts[2];
                        HashSet<int> targetSet = null;
                        if (name == "processedReleasedGames") targetSet = processedReleasedGames;
                        else if (name == "processedHitGames") targetSet = processedHitGames;
                        else if (name == "processedMasterpieces") targetSet = processedMasterpieces;
                        else if (name == "processedHighSales") targetSet = processedHighSales;
                        else if (name == "processedExclusiveGames") targetSet = processedExclusiveGames;
                        else if (name == "processedContractGames") targetSet = processedContractGames;
                        else if (name == "processedCommissions") targetSet = processedCommissions;
                        else if (name == "processedFailures") targetSet = processedFailures;
                        else if (name == "processedCancelledContracts") targetSet = processedCancelledContracts;
                        else if (name == "processedBoughtIPs") targetSet = processedBoughtIPs;
                        else if (name == "processedGiftedIPs") targetSet = processedGiftedIPs;
                        else if (name == "processedEngineLicenses") targetSet = processedEngineLicenses;
                        else if (name == "processedCoDevelopments") targetSet = processedCoDevelopments;

                        if (targetSet != null && !string.IsNullOrEmpty(val))
                        {
                            string[] ids = val.Split(',');
                            foreach (string idStr in ids)
                            {
                                int idVal;
                                if (int.TryParse(idStr, out idVal))
                                {
                                    targetSet.Add(idVal);
                                }
                            }
                        }
                    }
                    continue;
                }

                if (line.StartsWith("GLOBAL_CONTRACT"))
                {
                    string[] contractParts = line.Split('\t');
                    if (contractParts.Length >= 3)
                    {
                        int gId;
                        if (int.TryParse(contractParts[1], out gId))
                        {
                            gameActiveContracts[gId] = contractParts[2];
                        }
                    }
                    continue;
                }

                string[] parts = line.Split('\t');
                if (parts.Length < 12)
                {
                    continue;
                }

                int id;
                if (!int.TryParse(parts[0], out id)) continue;

                StudioState state = EnsureState(id);
                float.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out state.CustomRelation);
                state.ActiveContract = parts[2];
                int.TryParse(parts[3], out state.ContractWeeksLeft);
                int.TryParse(parts[4], out state.LastGameplayUnlockWeek);
                int.TryParse(parts[5], out state.LastEngineUnlockWeek);
                int.TryParse(parts[6], out state.LastTopicUnlockWeek);
                int.TryParse(parts[7], out state.LastCoFinanceWeek);
                int.TryParse(parts[8], out state.LastAAAWeek);
                int.TryParse(parts[9], out state.LastExclusiveWeek);
                int.TryParse(parts[10], out state.LastOutsourceWeek);
                bool.TryParse(parts[11], out state.FundedByPlayer);

                if (parts.Length >= 21)
                {
                    bool.TryParse(parts[12], out state.HasQueuedProject);
                    state.QueuedName = parts[13];
                    int.TryParse(parts[14], out state.QueuedGenre);
                    int.TryParse(parts[15], out state.QueuedSubGenre);
                    int.TryParse(parts[16], out state.QueuedTheme);
                    int.TryParse(parts[17], out state.QueuedSubTheme);
                    int.TryParse(parts[18], out state.QueuedSize);
                    int.TryParse(parts[19], out state.QueuedEngineId);
                    bool.TryParse(parts[20], out state.QueuedLetKeepIP);

                    if (parts.Length >= 22)
                    {
                        string[] platParts = parts[21].Split(',');
                        for (int p = 0; p < Math.Min(4, platParts.Length); p++)
                        {
                            int.TryParse(platParts[p], out state.QueuedPlatforms[p]);
                        }
                    }

                    if (parts.Length >= 23)
                    {
                        string[] featParts = parts[22].Split(',');
                        for (int f = 0; f < Math.Min(50, featParts.Length); f++)
                        {
                            bool.TryParse(featParts[f], out state.QueuedFeatures[f]);
                        }
                    }

                    if (parts.Length >= 24)
                    {
                        string[] osParts = parts[23].Split(',');
                        state.OutsourcedGameIds.Clear();
                        for (int o = 0; o < osParts.Length; o++)
                        {
                            int osId;
                            if (int.TryParse(osParts[o], out osId))
                            {
                                state.OutsourcedGameIds.Add(osId);
                            }
                        }
                    }
                }

                if (parts.Length >= 31)
                {
                    bool.TryParse(parts[24], out state.PublishedFirstGame);
                    int.TryParse(parts[25], out state.CommissionWeeksLeft);
                    int.TryParse(parts[26], out state.CommissionGenre);
                    int.TryParse(parts[27], out state.CommissionTargetReview);
                    long.TryParse(parts[28], out state.CommissionReward);
                    int.TryParse(parts[29], out state.ContractYearlyTimer);
                    int.TryParse(parts[30], out state.LastIPBuyWeek);
                }
            }
        }
        catch (Exception ex)
        {
            log.LogWarning("Error loading Studio Partnerships state: " + ex);
        }
    }

    private static void SaveState()
    {
        try
        {
            string dir = Path.GetDirectoryName(statePath);
            if (!string.IsNullOrEmpty(dir))
            {
                Directory.CreateDirectory(dir);
            }

            List<string> lines = new List<string>();
            lines.Add("# studioId\trelation\tactiveContract\tcontractWeeksLeft\tlastGameplay\tlastEngine\tlastTopic\tlastCoFinance\tlastAAA\tlastExclusive\tlastOutsource\tfundedByPlayer\thasQueued\tqName\tqGenre\tqSubGenre\tqTheme\tqSubTheme\tqSize\tqEngineId\tqKeepIP\tqPlatforms\tqFeatures\tqOutsourcedGameIds\tpubFirstGame\tcommWeeks\tcommGenre\tcommTarget\tcommReward\tcontractYearlyTimer\tlastIpBuyWeek");
            foreach (StudioState state in states.Values.OrderBy(s => s.StudioId))
            {
                string platStr = string.Join(",", state.QueuedPlatforms);
                string featStr = string.Join(",", state.QueuedFeatures);
                string osStr = string.Join(",", state.OutsourcedGameIds);

                lines.Add(
                    state.StudioId.ToString(CultureInfo.InvariantCulture) + "\t" +
                    state.CustomRelation.ToString("0.###", CultureInfo.InvariantCulture) + "\t" +
                    (state.ActiveContract ?? "") + "\t" +
                    state.ContractWeeksLeft.ToString(CultureInfo.InvariantCulture) + "\t" +
                    state.LastGameplayUnlockWeek.ToString(CultureInfo.InvariantCulture) + "\t" +
                    state.LastEngineUnlockWeek.ToString(CultureInfo.InvariantCulture) + "\t" +
                    state.LastTopicUnlockWeek.ToString(CultureInfo.InvariantCulture) + "\t" +
                    state.LastCoFinanceWeek.ToString(CultureInfo.InvariantCulture) + "\t" +
                    state.LastAAAWeek.ToString(CultureInfo.InvariantCulture) + "\t" +
                    state.LastExclusiveWeek.ToString(CultureInfo.InvariantCulture) + "\t" +
                    state.LastOutsourceWeek.ToString(CultureInfo.InvariantCulture) + "\t" +
                    state.FundedByPlayer.ToString(CultureInfo.InvariantCulture) + "\t" +
                    state.HasQueuedProject.ToString(CultureInfo.InvariantCulture) + "\t" +
                    (state.QueuedName ?? "").Replace("\t", " ") + "\t" +
                    state.QueuedGenre.ToString(CultureInfo.InvariantCulture) + "\t" +
                    state.QueuedSubGenre.ToString(CultureInfo.InvariantCulture) + "\t" +
                    state.QueuedTheme.ToString(CultureInfo.InvariantCulture) + "\t" +
                    state.QueuedSubTheme.ToString(CultureInfo.InvariantCulture) + "\t" +
                    state.QueuedSize.ToString(CultureInfo.InvariantCulture) + "\t" +
                    state.QueuedEngineId.ToString(CultureInfo.InvariantCulture) + "\t" +
                    state.QueuedLetKeepIP.ToString(CultureInfo.InvariantCulture) + "\t" +
                    platStr + "\t" +
                    featStr + "\t" +
                    osStr + "\t" +
                    state.PublishedFirstGame.ToString(CultureInfo.InvariantCulture) + "\t" +
                    state.CommissionWeeksLeft.ToString(CultureInfo.InvariantCulture) + "\t" +
                    state.CommissionGenre.ToString(CultureInfo.InvariantCulture) + "\t" +
                    state.CommissionTargetReview.ToString(CultureInfo.InvariantCulture) + "\t" +
                    state.CommissionReward.ToString(CultureInfo.InvariantCulture) + "\t" +
                    state.ContractYearlyTimer.ToString(CultureInfo.InvariantCulture) + "\t" +
                    state.LastIPBuyWeek.ToString(CultureInfo.InvariantCulture)
                );
            }

            // Save global trackers
            lines.Add("GLOBAL_TRACKER\tprocessedReleasedGames\t" + string.Join(",", processedReleasedGames));
            lines.Add("GLOBAL_TRACKER\tprocessedHitGames\t" + string.Join(",", processedHitGames));
            lines.Add("GLOBAL_TRACKER\tprocessedMasterpieces\t" + string.Join(",", processedMasterpieces));
            lines.Add("GLOBAL_TRACKER\tprocessedHighSales\t" + string.Join(",", processedHighSales));
            lines.Add("GLOBAL_TRACKER\tprocessedExclusiveGames\t" + string.Join(",", processedExclusiveGames));
            lines.Add("GLOBAL_TRACKER\tprocessedContractGames\t" + string.Join(",", processedContractGames));
            lines.Add("GLOBAL_TRACKER\tprocessedCommissions\t" + string.Join(",", processedCommissions));
            lines.Add("GLOBAL_TRACKER\tprocessedFailures\t" + string.Join(",", processedFailures));
            lines.Add("GLOBAL_TRACKER\tprocessedCancelledContracts\t" + string.Join(",", processedCancelledContracts));
            lines.Add("GLOBAL_TRACKER\tprocessedBoughtIPs\t" + string.Join(",", processedBoughtIPs));
            lines.Add("GLOBAL_TRACKER\tprocessedGiftedIPs\t" + string.Join(",", processedGiftedIPs));
            lines.Add("GLOBAL_TRACKER\tprocessedEngineLicenses\t" + string.Join(",", processedEngineLicenses));
            lines.Add("GLOBAL_TRACKER\tprocessedCoDevelopments\t" + string.Join(",", processedCoDevelopments));

            // Save active game contracts
            foreach (var kvp in gameActiveContracts)
            {
                lines.Add("GLOBAL_CONTRACT\t" + kvp.Key.ToString(CultureInfo.InvariantCulture) + "\t" + kvp.Value);
            }

            File.WriteAllLines(statePath, lines.ToArray());
        }
        catch (Exception ex)
        {
            log.LogWarning("Could not save Studio Partnerships state: " + ex);
        }
    }

    // UI Injections for Stats Menus
    private static GameObject sidePanel;
    private static List<GameObject> cardsList = new List<GameObject>();
    private static RectTransform statsWindowRect;
    private static Vector2 statsWindowOriginalPos;

    private static Font ResolveFont(MonoBehaviour menu, GameObject[] uiObjects)
    {
        if (uiObjects != null)
        {
            foreach (GameObject obj in uiObjects)
            {
                if (obj != null)
                {
                    Text t = obj.GetComponent<Text>();
                    if (t != null && t.font != null) return t.font;
                    Text tChild = obj.GetComponentInChildren<Text>();
                    if (tChild != null && tChild.font != null) return tChild.font;
                }
            }
        }
        if (menu != null)
        {
            Text[] texts = menu.GetComponentsInChildren<Text>(true);
            foreach (Text t in texts)
            {
                if (t != null && t.font != null) return t.font;
            }
        }
        Font[] fonts = Resources.FindObjectsOfTypeAll<Font>();
        foreach (Font f in fonts)
        {
            if (f != null && f.name != "Arial" && !string.IsNullOrEmpty(f.name)) return f;
        }
        foreach (Font f in fonts)
        {
            if (f != null) return f;
        }
        return Resources.GetBuiltinResource<Font>("Arial.ttf");
    }

    public static void InjectSidePanel(MonoBehaviour menu, publisherScript pS, GameObject[] uiObjects)
    {
        if (menu == null || pS == null || !pS) return;

        // Clean previous UI elements to avoid duplicates
        ClearSidePanel();

        Transform menueTransform = menu.transform.Find("Menue");
        if (menueTransform == null)
        {
            menueTransform = menu.transform;
        }
        else
        {
            RectTransform menueRt = menueTransform.GetComponent<RectTransform>();
            if (menueRt != null)
            {
                // Store original position if not already stored
                if (statsWindowRect == null)
                {
                    statsWindowRect = menueRt;
                    statsWindowOriginalPos = menueRt.anchoredPosition;
                }
                // Shift stats window left to make room for side panel
                menueRt.anchoredPosition = statsWindowOriginalPos + new Vector2(-225f, 0f);
            }
        }

        // Create Panel Background Container
        sidePanel = new GameObject("SPO_SidePanel", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        sidePanel.transform.SetParent(menueTransform, false);

        Image sideBg = sidePanel.GetComponent<Image>();
        if (sideBg != null)
        {
            Color bgColor;
            Image.Type bgType;
            Sprite bgSprite = FindPanelSprite(menu, out bgColor, out bgType);
            if (bgSprite != null)
            {
                sideBg.sprite = bgSprite;
                sideBg.type = bgType;
                sideBg.color = new Color(bgColor.r, bgColor.g, bgColor.b, 0.95f);
            }
            else
            {
                sideBg.color = new Color(0.12f, 0.12f, 0.15f, 0.95f);
            }
            sideBg.material = null; // Prevent diagonal slashes/materials
        }

        RectTransform sideRect = sidePanel.GetComponent<RectTransform>();
        sideRect.anchorMin = new Vector2(1f, 0.5f);
        sideRect.anchorMax = new Vector2(1f, 0.5f);
        sideRect.pivot = new Vector2(0f, 0.5f);
        sideRect.sizeDelta = new Vector2(450f, 540f);
        sideRect.anchoredPosition = new Vector2(10f, 0f);
        sideRect.localScale = Vector3.one;

        // Title Text
        GameObject titleObj = new GameObject("Title", typeof(RectTransform), typeof(Text));
        titleObj.transform.SetParent(sidePanel.transform, false);
        RectTransform titleRect = titleObj.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0f, 1f);
        titleRect.anchorMax = new Vector2(1f, 1f);
        titleRect.pivot = new Vector2(0.5f, 1f);
        titleRect.sizeDelta = new Vector2(0f, 30f);
        titleRect.anchoredPosition = new Vector2(0f, -10f);
        titleRect.localScale = Vector3.one;

        Text titleText = titleObj.GetComponent<Text>();
        titleText.alignment = TextAnchor.MiddleCenter;
        titleText.fontSize = 18;
        titleText.fontStyle = FontStyle.Bold;
        titleText.color = Color.white;
        titleText.text = "Partnerships & Perks";

        titleText.font = ResolveFont(menu, uiObjects);

        int subdiv = GetSubdivision(pS);

        // Subdivision classification label
        GameObject subdivObj = new GameObject("SubdivisionLabel", typeof(RectTransform), typeof(Text));
        subdivObj.transform.SetParent(sidePanel.transform, false);
        RectTransform subdivRect = subdivObj.GetComponent<RectTransform>();
        subdivRect.anchorMin = new Vector2(0f, 1f);
        subdivRect.anchorMax = new Vector2(1f, 1f);
        subdivRect.pivot = new Vector2(0.5f, 1f);
        subdivRect.sizeDelta = new Vector2(0f, 20f);
        subdivRect.anchoredPosition = new Vector2(0f, -42f);
        subdivRect.localScale = Vector3.one;

        Text subdivText = subdivObj.GetComponent<Text>();
        subdivText.alignment = TextAnchor.MiddleCenter;
        subdivText.fontSize = 13;
        subdivText.fontStyle = FontStyle.Italic;
        subdivText.color = new Color(0.7f, 0.9f, 1f);
        subdivText.font = titleText.font;
        subdivText.text = $"Classification: <b>{GetSubdivisionName(subdiv)}</b>";

        // Draw 5 rectangle cards shifted down to accommodate subdivision label
        float startY = -75f;
        float spacing = 90f;
        for (int i = 0; i < 5; i++)
        {
            GameObject card = CreateCard(sidePanel.transform, startY - (i * spacing), i + 1, pS, titleText.font, menu);
            cardsList.Add(card);
        }
    }

    private static void ClearSidePanel()
    {
        if (sidePanel != null)
        {
            UnityEngine.Object.Destroy(sidePanel);
            sidePanel = null;
        }
        cardsList.Clear();
        
        // Reset stats window position
        if (statsWindowRect != null && statsWindowRect)
        {
            statsWindowRect.anchoredPosition = statsWindowOriginalPos;
        }
        statsWindowRect = null;
    }

    private static Sprite FindPanelSprite(MonoBehaviour menu, out Color color, out Image.Type type)
    {
        color = new Color(0.12f, 0.12f, 0.15f, 0.85f);
        type = Image.Type.Simple;
        if (menu == null) return null;

        Image[] images = menu.GetComponentsInChildren<Image>(true);
        foreach (Image img in images)
        {
            if (img != null && img.sprite != null && img.gameObject != menu.gameObject)
            {
                string n = img.name.ToLower();
                if (n.Contains("panel") || n.Contains("box") || n.Contains("bg") || n.Contains("hintergrund") || n.Contains("list") || n.Contains("item"))
                {
                    color = img.color;
                    type = img.type;
                    return img.sprite;
                }
            }
        }

        foreach (Image img in images)
        {
            if (img != null && img.sprite != null && img.gameObject != menu.gameObject && img.type == Image.Type.Sliced)
            {
                color = img.color;
                type = img.type;
                return img.sprite;
            }
        }

        return null;
    }

    private static GameObject CreateCard(Transform parent, float yPos, int starRating, publisherScript pS, Font font, MonoBehaviour menu)
    {
        GameObject card = new GameObject($"Card_{starRating}", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        card.transform.SetParent(parent, false);

        RectTransform cardRect = card.GetComponent<RectTransform>();
        cardRect.anchorMin = new Vector2(0.05f, 1f);
        cardRect.anchorMax = new Vector2(0.95f, 1f);
        cardRect.pivot = new Vector2(0.5f, 1f);
        cardRect.sizeDelta = new Vector2(0f, 80f);
        cardRect.anchoredPosition = new Vector2(0f, yPos);
        cardRect.localScale = Vector3.one;

        Color cardColor;
        Image.Type cardType;
        Sprite panelSprite = FindPanelSprite(menu, out cardColor, out cardType);

        Image cardImg = card.GetComponent<Image>();
        cardImg.material = null; // Prevent diagonal slashes/materials
        if (panelSprite != null)
        {
            cardImg.sprite = panelSprite;
            cardImg.type = cardType;
            cardImg.color = new Color(cardColor.r, cardColor.g, cardColor.b, 0.95f);
        }
        else
        {
            cardImg.color = new Color(0.12f, 0.12f, 0.15f, 0.85f);
        }

        // Check locks
        int subdiv = GetSubdivision(pS);
        mainScript mS = FindObjectOfType<mainScript>();
        
        bool isRivalConsoleLocked = (subdiv == 1 && PlayerHasActiveConsole(mS));
        bool isUnlocked = pS.GetRelation() >= (starRating * 20f) && !isRivalConsoleLocked;

        // Lock Display
        if (!isUnlocked)
        {
            if (panelSprite != null)
            {
                cardImg.color = new Color(cardColor.r * 0.5f, cardColor.g * 0.5f, cardColor.b * 0.5f, 0.70f);
            }
            else
            {
                cardImg.color = new Color(0.08f, 0.08f, 0.10f, 0.60f);
            }

            GameObject lockObj = new GameObject("LockText", typeof(RectTransform), typeof(Text));
            lockObj.transform.SetParent(card.transform, false);
            RectTransform lockRt = lockObj.GetComponent<RectTransform>();
            lockRt.anchorMin = Vector2.zero;
            lockRt.anchorMax = Vector2.one;
            lockRt.offsetMin = Vector2.zero;
            lockRt.offsetMax = Vector2.zero;

            Text lockTxt = lockObj.GetComponent<Text>();
            lockTxt.font = font;
            lockTxt.fontSize = 14;
            lockTxt.alignment = TextAnchor.MiddleCenter;
            
            if (isRivalConsoleLocked)
            {
                lockTxt.color = Color.red;
                lockTxt.text = $"★ {starRating} Stars - Locked (Rival Console Owner)";
            }
            else
            {
                lockTxt.color = Color.grey;
                lockTxt.text = $"★ {starRating} Stars - Locked (Requires {starRating * 20} Relationship)";
            }
            return card;
        }

        // Draw Card Title label
        GameObject labelObj = new GameObject("Label", typeof(RectTransform), typeof(Text));
        labelObj.transform.SetParent(card.transform, false);
        RectTransform labelRt = labelObj.GetComponent<RectTransform>();
        labelRt.anchorMin = new Vector2(0.05f, 0.6f);
        labelRt.anchorMax = new Vector2(0.95f, 0.95f);
        labelRt.offsetMin = Vector2.zero;
        labelRt.offsetMax = Vector2.zero;

        Text labelTxt = labelObj.GetComponent<Text>();
        labelTxt.font = font;
        labelTxt.fontSize = 12;
        labelTxt.fontStyle = FontStyle.Bold;
        labelTxt.color = new Color(0.9f, 0.7f, 0.2f);
        labelTxt.text = $"★ Level {starRating} Perk Unlocked";

        // Add tooltip summary to card background
        string cardTooltip = GetPerkTooltipText(starRating, subdiv);
        if (!string.IsNullOrEmpty(cardTooltip))
        {
            tooltip tt = card.AddComponent<tooltip>();
            tt.c = cardTooltip;
        }

        // Setup Perk buttons inside card
        PopulatePerkButtons(card, starRating, subdiv, pS, font, menu);

        return card;
    }

    private static void PopulatePerkButtons(GameObject card, int tier, int subdiv, publisherScript pS, Font font, MonoBehaviour menu)
    {
        // Platform Holder
        if (subdiv == 1)
        {
            if (tier == 1) AddPerkButton(card, "Gameplay Instant-Unlock", GetPerkTooltipText(1, 1), () => HandleInstantUnlock(3, pS), font, menu);
            if (tier == 2) AddPerkButton(card, "Engine Instant-Unlock", GetPerkTooltipText(2, 1), () => HandleInstantUnlock(2, pS), font, menu);
            if (tier == 3) AddPerkButton(card, "Secured Marketing Deal", GetPerkTooltipText(3, 1), () => HandleSecuredMarketing(pS), font, menu);
            if (tier == 4) AddPerkButton(card, "Exclusivity Full Funding", GetPerkTooltipText(4, 1), () => HandleExclusiveFunding(pS), font, menu);
            if (tier == 5) AddPerkButton(card, "First-Party Partner Bailout", GetPerkTooltipText(5, 1), () => HandleFirstPartyBailout(pS), font, menu);
        }
        // Big Publisher
        else if (subdiv == 2)
        {
            if (tier == 1) AddPerkButton(card, "Topic Instant-Unlock", GetPerkTooltipText(1, 2), () => HandleInstantUnlock(1, pS), font, menu);
            if (tier == 2) AddPerkButton(card, "Premium Contract Deal", GetPerkTooltipText(2, 2), () => HandlePremiumContract(pS), font, menu);
            if (tier == 3) AddPerkButton(card, "IP Cooperation Pitch", GetPerkTooltipText(3, 2), () => HandleIPCooperation(pS), font, menu);
            if (tier == 4) AddPerkButton(card, "AAA Co-Publish Pitch", GetPerkTooltipText(4, 2), () => HandleAAACoPublish(pS), font, menu);
            if (tier == 5)
            {
                AddPerkButton(card, "Subsidiary Buyout", "<b>Subsidiary Buyout</b>\n\nFully acquire the publisher as a subsidiary with a friendly 30% discount on their company value.\n\n<b>Gain</b>: +20 Relationship points.", () => HandleFriendlyBuyout(pS, 0.70f, menu), font, menu, new Vector2(0.05f, 0.05f), new Vector2(0.48f, 0.55f));
                AddPerkButton(card, "IP/Console Exclusives", "<b>IP Licensing & Exclusivity deals</b>\n\nEnables two perks:\n1. Full right to develop games using their IPs with good royalties and 50-100% funding (depending on IP importance) (cooldown: 5 years).\n2. If you own a console: Force their in-development game to be exclusive to your console, or put their games on your subscription service.\n\n<b>Gain</b>: +20 Relationship points for exclusivity/subscription deals.", () => HandleBigPublisherExclusives(pS), font, menu, new Vector2(0.52f, 0.05f), new Vector2(0.95f, 0.55f));
            }
        }
        // Small Publisher
        else if (subdiv == 3)
        {
            if (tier == 1) AddPerkLabel(card, "Publish Standard Games (+Publishing Rel Boost)", "<b>Basic Publishing Contact</b>\n\nAllows you to publish standard games through them. Normal publishing with this studio awards relationship points.\n\n<b>Gain</b>: +3 Relationship points per published game.", font);
            if (tier == 2)
            {
                AddPerkButton(card, "Indie Sponsor Contract", GetPerkTooltipText(2, 3), () => HandleIndieSponsor(pS), font, menu, new Vector2(0.05f, 0.05f), new Vector2(0.48f, 0.55f));
                AddPerkButton(card, "Gift Engine License", "Gift a free engine license for +15 relationship points.", () => HandleGiftEngineLicense(pS), font, menu, new Vector2(0.52f, 0.05f), new Vector2(0.95f, 0.55f));
            }
            if (tier == 3)
            {
                AddPerkButton(card, "IP Cooperation", GetPerkTooltipText(3, 3), () => HandleIPCooperationSmall(pS), font, menu, new Vector2(0.05f, 0.05f), new Vector2(0.48f, 0.55f));
                AddPerkButton(card, "Sell IP to Publisher", "<b>Sell IP to Publisher</b>\n\nSell one of your IPs to them at a high value.\n\n<b>Gain</b>: +15 Relationship points.", () => HandleSellIPToNPC(pS), font, menu, new Vector2(0.52f, 0.05f), new Vector2(0.95f, 0.55f));
            }
            if (tier == 4) AddPerkButton(card, "Console Exclusivity / Sub", GetPerkTooltipText(4, 3), () => HandleConsoleExclusivityNPC(pS, 3), font, menu);
            if (tier == 5) AddPerkButton(card, "Friendly Absorption Buyout", GetPerkTooltipText(5, 3), () => HandleFriendlyBuyout(pS, 0.50f, menu), font, menu);
        }
        // Big Developer
        else if (subdiv == 4)
        {
            if (tier == 1) AddPerkButton(card, "Publish In-Dev Game", GetPerkTooltipText(1, 4), () => HandlePublishInDevGame(pS), font, menu);
            if (tier == 2)
            {
                AddPerkButton(card, "IP Licensing Partner", GetPerkTooltipText(2, 4), () => HandleIPLicensingPartner(pS), font, menu, new Vector2(0.05f, 0.05f), new Vector2(0.48f, 0.55f));
                AddPerkButton(card, "Gift Engine License", "Gift a free engine license for +15 relationship points.", () => HandleGiftEngineLicense(pS), font, menu, new Vector2(0.52f, 0.05f), new Vector2(0.95f, 0.55f));
            }
            if (tier == 3)
            {
                AddPerkButton(card, "Buy Game IP (10yr CD)", "<b>IP Purchase</b>\n\nBuy one of their mature IPs outright at a good rate.\n\n<b>Cooldown</b>: Once every 10 years (480 weeks).\n<b>Gain</b>: +15 Relationship points.", () => HandleBuyIPFromNPC(pS, 10), font, menu, new Vector2(0.05f, 0.05f), new Vector2(0.48f, 0.55f));
                AddPerkButton(card, "Outsource Project", "<b>Full Project Financing (Outsourcing)</b>\n\nAssign them a project based on your IP or a new IP. You fully fund the project with a lump sum payment.\n\n<b>In-Dev Queue System</b>:\n1. <i>Cancel Current</i>: Pay extra to force immediate start (slight relation penalty -10 points).\n2. <i>Queue</i>: They start once done (+15 points on completion).\n3. <i>IP Gift</i>: Gift them a new IP for +35 relationship points.", () => HandleOutsourceProject(pS), font, menu, new Vector2(0.52f, 0.05f), new Vector2(0.95f, 0.55f));
            }
            if (tier == 4) AddPerkButton(card, "Auto Publication Agreement", GetPerkTooltipText(4, 4), () => HandleAutoPublishContract(pS), font, menu);
            if (tier == 5) AddPerkButton(card, "Friendly Acquisition buyout", GetPerkTooltipText(5, 4), () => HandleFriendlyBuyout(pS, 0.70f, menu), font, menu);
        }
        // Mid Developer
        else if (subdiv == 5)
        {
            if (tier == 1) AddPerkButton(card, "Publish In-Dev Game", GetPerkTooltipText(1, 5), () => HandlePublishInDevGame(pS), font, menu);
            if (tier == 2)
            {
                AddPerkButton(card, "Outsource Project", GetPerkTooltipText(2, 5), () => HandleOutsourceProject(pS), font, menu, new Vector2(0.05f, 0.05f), new Vector2(0.48f, 0.55f));
                AddPerkButton(card, "Gift Engine License", "Gift a free engine license for +15 relationship points.", () => HandleGiftEngineLicense(pS), font, menu, new Vector2(0.52f, 0.05f), new Vector2(0.95f, 0.55f));
            }
            if (tier == 3) AddPerkButton(card, "Console Exclusivity / Sub", GetPerkTooltipText(3, 5), () => HandleConsoleExclusivityNPC(pS, 5), font, menu);
            if (tier == 4) AddPerkButton(card, "Auto Publication Agreement", GetPerkTooltipText(4, 5), () => HandleAutoPublishContract(pS), font, menu);
            if (tier == 5) AddPerkButton(card, "Friendly Buyout", GetPerkTooltipText(5, 5), () => HandleFriendlyBuyout(pS, 0.60f, menu), font, menu);
        }
        // Indie Developer
        else if (subdiv == 6)
        {
            if (tier == 1) AddPerkButton(card, "Publish In-Dev Game", GetPerkTooltipText(1, 6), () => HandlePublishInDevGame(pS), font, menu);
            if (tier == 2)
            {
                AddPerkButton(card, "Outsource Project", GetPerkTooltipText(2, 6), () => HandleOutsourceProject(pS), font, menu, new Vector2(0.05f, 0.05f), new Vector2(0.48f, 0.55f));
                AddPerkButton(card, "Gift Engine License", "Gift a free engine license for +20 relationship points.", () => HandleGiftEngineLicense(pS), font, menu, new Vector2(0.52f, 0.05f), new Vector2(0.95f, 0.55f));
            }
            if (tier == 3) AddPerkButton(card, "Auto Publication Agreement", GetPerkTooltipText(3, 6), () => HandleAutoPublishContract(pS), font, menu);
            if (tier == 4) AddPerkButton(card, "Console Exclusivity / Sub", GetPerkTooltipText(4, 6), () => HandleConsoleExclusivityNPC(pS, 6), font, menu);
            if (tier == 5) AddPerkButton(card, "Acquire Studio", GetPerkTooltipText(5, 6), () => HandleFriendlyBuyout(pS, 0.50f, menu), font, menu);
        }
    }

    private static void AddPerkLabel(GameObject card, string text, string tooltipText, Font font)
    {
        GameObject textObj = new GameObject("LabelText", typeof(RectTransform), typeof(Text));
        textObj.transform.SetParent(card.transform, false);
        RectTransform rt = textObj.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.05f, 0.05f);
        rt.anchorMax = new Vector2(0.95f, 0.55f);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        Text txt = textObj.GetComponent<Text>();
        txt.font = font ?? ResolveFont(null, null);
        txt.fontSize = 11;
        txt.color = Color.white;
        txt.alignment = TextAnchor.MiddleLeft;
        txt.text = text;

        if (!string.IsNullOrEmpty(tooltipText))
        {
            tooltip tt = textObj.AddComponent<tooltip>();
            tt.c = tooltipText;
        }
    }

    private static void CopyButtonStyling(Button src, Button dest)
    {
        if (src == null || dest == null) return;
        Image srcImg = src.GetComponent<Image>();
        Image destImg = dest.GetComponent<Image>();
        if (srcImg != null && destImg != null)
        {
            destImg.sprite = srcImg.sprite;
            destImg.type = srcImg.type;
            destImg.color = srcImg.color;
            destImg.material = null; // Prevent custom materials/patterns on buttons
        }
        dest.transition = src.transition;
        dest.colors = src.colors;
        dest.spriteState = src.spriteState;
    }

    private static void AddPerkButton(GameObject card, string btnText, string tooltipText, Action onClick, Font font, MonoBehaviour menu, Vector2? customMin = null, Vector2? customMax = null)
    {
        GameObject btnObj = new GameObject("Button", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
        btnObj.transform.SetParent(card.transform, false);

        RectTransform rt = btnObj.GetComponent<RectTransform>();
        rt.anchorMin = customMin ?? new Vector2(0.05f, 0.08f);
        rt.anchorMax = customMax ?? new Vector2(0.95f, 0.53f);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        Image img = btnObj.GetComponent<Image>();
        img.color = new Color(0.2f, 0.35f, 0.2f, 1f);
        img.material = null; // Prevent diagonal slashes/materials

        Button btn = btnObj.GetComponent<Button>();
        Button templateBtn = null;
        if (menu != null)
        {
            Button[] btns = menu.GetComponentsInChildren<Button>(true);
            // 1. Try to find a standard text-based button (English or German names)
            foreach (Button b in btns)
            {
                if (b != null)
                {
                    string name = b.name.ToLower();
                    if (name.Contains("games") || name.Contains("spiele") || 
                        name.Contains("ips") || name.Contains("awards") || 
                        name.Contains("firmakaufen") || name.Contains("vertrieben"))
                    {
                        Text t = b.GetComponentInChildren<Text>();
                        if (t != null)
                        {
                            templateBtn = b;
                            break;
                        }
                    }
                }
            }

            // 2. Fallback to any button that has text and isn't a close/cancel button
            if (templateBtn == null)
            {
                foreach (Button b in btns)
                {
                    if (b != null)
                    {
                        string name = b.name.ToLower();
                        if (!name.Contains("close") && !name.Contains("abbrechen") && 
                            !name.Contains("cancel") && !name.Contains("exit") && 
                            !name.Contains("x") && !name.Contains("back") && 
                            b.name.Length > 2)
                        {
                            Text t = b.GetComponentInChildren<Text>();
                            if (t != null && !string.IsNullOrEmpty(t.text))
                            {
                                templateBtn = b;
                                break;
                            }
                        }
                    }
                }
            }
        }

        if (templateBtn != null)
        {
            CopyButtonStyling(templateBtn, btn);
        }
        btn.targetGraphic = img;

        GameObject txtObj = new GameObject("Text", typeof(RectTransform), typeof(Text));
        txtObj.transform.SetParent(btnObj.transform, false);
        RectTransform txtRt = txtObj.GetComponent<RectTransform>();
        txtRt.anchorMin = Vector2.zero;
        txtRt.anchorMax = Vector2.one;
        txtRt.offsetMin = Vector2.zero;
        txtRt.offsetMax = Vector2.zero;

        Text txt = txtObj.GetComponent<Text>();
        Text templateText = templateBtn != null ? templateBtn.GetComponentInChildren<Text>() : null;
        if (templateText != null && templateText.font != null)
        {
            txt.color = Color.black; // Force black text for high readability and match vanilla UI
            txt.font = templateText.font;
            txt.fontSize = templateText.fontSize;
            txt.fontStyle = templateText.fontStyle;
        }
        else
        {
            txt.color = Color.black;
            txt.font = font ?? ResolveFont(menu, null);
            txt.fontSize = 11;
            txt.fontStyle = FontStyle.Bold;
        }
        txt.alignment = TextAnchor.MiddleCenter;
        txt.text = btnText;

        if (!string.IsNullOrEmpty(tooltipText))
        {
            tooltip tt = btnObj.AddComponent<tooltip>();
            tt.c = tooltipText;
        }

        btn.onClick.AddListener(() =>
        {
            try
            {
                // Force close the tooltip to prevent it from getting stuck on screen
                GUI_Tooltip guiTooltip = UnityEngine.Object.FindObjectOfType<GUI_Tooltip>();
                if (guiTooltip != null)
                {
                    guiTooltip.SetInactive();
                }

                onClick();
            }
            catch (Exception ex)
            {
                log?.LogWarning($"Error in button click for '{btnText}': {ex}");
            }
        });
    }

    // Action implementation handlers

    private static void HandleInstantUnlock(int type, publisherScript pS)
    {
        mainScript mS = FindObjectOfType<mainScript>();
        GUI_Main gui = GetGuiMainCached();
        if (mS == null || gui == null) return;

        StudioState state = EnsureState(pS.myID);
        int currentWeek = GetGlobalWeek(mS);

        // Cooldown checks
        if (type == 3) // Gameplay
        {
            if (currentWeek - state.LastGameplayUnlockWeek < 8)
            {
                gui.MessageBox("This action is on cooldown. Available in " + (8 - (currentWeek - state.LastGameplayUnlockWeek)) + " weeks.", false);
                return;
            }
            gameplayFeatures gF = mS.GetComponent<gameplayFeatures>();
            List<int> lockedList = new List<int>();
            for (int i = 0; i < gF.gameplayFeatures_UNLOCK.Length; i++)
            {
                if (!gF.IsErforscht(i)) lockedList.Add(i);
            }
            if (lockedList.Count == 0)
            {
                gui.MessageBox("All gameplay features have already been researched!", false);
                return;
            }
            int pick = lockedList[UnityEngine.Random.Range(0, lockedList.Count)];
            gF.gameplayFeatures_UNLOCK[pick] = true;
            gF.gameplayFeatures_RES_POINTS_LEFT[pick] = 0f;
            state.LastGameplayUnlockWeek = currentWeek;
            pS.relation = Mathf.Clamp(pS.relation + 2f, 0f, 100f);
            SaveState();
            gui.MessageBox($"Instantly unlocked research for: <color=blue><b>{gF.GetName(pick)}</b></color>!", false);
        }
        else if (type == 2) // Engine
        {
            if (currentWeek - state.LastEngineUnlockWeek < 12)
            {
                gui.MessageBox("This action is on cooldown. Available in " + (12 - (currentWeek - state.LastEngineUnlockWeek)) + " weeks.", false);
                return;
            }
            engineFeatures eF = mS.GetComponent<engineFeatures>();
            List<int> lockedList = new List<int>();
            for (int i = 0; i < eF.engineFeatures_UNLOCK.Length; i++)
            {
                if (!eF.IsErforscht(i)) lockedList.Add(i);
            }
            if (lockedList.Count == 0)
            {
                gui.MessageBox("All engine features have already been researched!", false);
                return;
            }
            int pick = lockedList[UnityEngine.Random.Range(0, lockedList.Count)];
            eF.engineFeatures_UNLOCK[pick] = true;
            eF.engineFeatures_RES_POINTS_LEFT[pick] = 0f;
            state.LastEngineUnlockWeek = currentWeek;
            pS.relation = Mathf.Clamp(pS.relation + 3f, 0f, 100f);
            SaveState();
            gui.MessageBox($"Instantly unlocked research for: <color=blue><b>{eF.GetName(pick)}</b></color>!", false);
        }
        else if (type == 1) // Topic / Theme
        {
            if (currentWeek - state.LastTopicUnlockWeek < 4)
            {
                gui.MessageBox("This action is on cooldown. Available in " + (4 - (currentWeek - state.LastTopicUnlockWeek)) + " weeks.", false);
                return;
            }
            themes th = mS.GetComponent<themes>();
            List<int> lockedList = new List<int>();
            for (int i = 0; i < th.themes_RES_POINTS_LEFT.Length; i++)
            {
                if (!th.IsErforscht(i)) lockedList.Add(i);
            }
            if (lockedList.Count == 0)
            {
                gui.MessageBox("All topics/themes have already been researched!", false);
                return;
            }
            int pick = lockedList[UnityEngine.Random.Range(0, lockedList.Count)];
            th.themes_RES_POINTS_LEFT[pick] = 0f;
            state.LastTopicUnlockWeek = currentWeek;
            pS.relation = Mathf.Clamp(pS.relation + 1f, 0f, 100f);
            SaveState();
            gui.MessageBox($"Instantly unlocked topic research: <color=blue><b>{mS.tS_.GetThemes(pick)}</b></color>!", false);
        }
    }

    private static void HandleIndieSponsor(publisherScript pS)
    {
        mainScript mS = FindObjectOfType<mainScript>();
        GUI_Main gui = FindObjectOfType<GUI_Main>();
        if (mS == null || gui == null) return;

        StudioState state = EnsureState(pS.myID);
        int currentWeek = GetGlobalWeek(mS);
        if (currentWeek - state.LastCoFinanceWeek < 96)
        {
            gui.MessageBox("This action is on cooldown. Available in " + (96 - (currentWeek - state.LastCoFinanceWeek)) + " weeks.", false);
            return;
        }

        ShowProjectSelectorDialog(pS, "Select in-development project for Indie Sponsorship:", 
            (game) => {
                long bonus = 50000L * (game.gameSize + 1);
                mS.Earn(bonus, 10);
                game.publisherID = pS.myID;
                game.pS_ = pS;
                game.pubAngebot_Gewinnbeteiligung = 20f; // Premium royalty rate (20%)

                state.LastCoFinanceWeek = currentWeek;
                AdjustRelation(pS, 5f); // +5 relationship points
                SaveState();
                gui.MessageBox($"Game '{game.myName}' sponsored by {pS.GetName()}! Received upfront bonus of {mS.GetMoney(bonus, true)}.", false);
            },
            () => {
                activeOutsourceNpcId = pS.myID;
                activeOutsourceIsNewIP = 1; // Indie Sponsor
                BeginOutsourceDesignInterception(pS);
            }
        );
    }

    private static void HandleSecuredMarketing(publisherScript pS)
    {
        mainScript mS = FindObjectOfType<mainScript>();
        GUI_Main gui = FindObjectOfType<GUI_Main>();
        if (mS == null || gui == null) return;

        StudioState state = EnsureState(pS.myID);
        int currentWeek = GetGlobalWeek(mS);
        if (currentWeek - state.LastCoFinanceWeek < 144)
        {
            gui.MessageBox("This action is on cooldown. Available in " + (144 - (currentWeek - state.LastCoFinanceWeek)) + " weeks.", false);
            return;
        }

        ShowProjectSelectorDialog(pS, "Select in-development project for Tech Sharing / Publishing Deal:", 
            (game) => {
                game.publisherID = pS.myID;
                game.pS_ = pS;
                game.pubAngebot_Gewinnbeteiligung = 10f; // Premium royalty rate (10%)

                state.LastCoFinanceWeek = currentWeek;
                AdjustRelation(pS, 10f); // +10 relationship points
                SaveState();
                gui.MessageBox($"Game '{game.myName}' is now published by {pS.GetName()} at 10% royalty rate!", false);
            },
            () => {
                activeOutsourceNpcId = pS.myID;
                activeOutsourceIsNewIP = 2; // NPC IP
                BeginOutsourceDesignInterception(pS);
            }
        );
    }

    private static void HandleExclusiveFunding(publisherScript pS)
    {
        mainScript mS = FindObjectOfType<mainScript>();
        GUI_Main gui = FindObjectOfType<GUI_Main>();
        if (mS == null || gui == null) return;

        StudioState state = EnsureState(pS.myID);
        int currentWeek = GetGlobalWeek(mS);
        if (currentWeek - state.LastExclusiveWeek < 240)
        {
            gui.MessageBox("This action is on cooldown. Available in " + (240 - (currentWeek - state.LastExclusiveWeek)) + " weeks.", false);
            return;
        }

        ShowProjectSelectorDialog(pS, "Select in-development project for Console Exclusivity:", 
            (game) => {
                int targetPlatId = -1;
                for (int i = 0; i < mS.arrayPlatformsScripts.Length; i++)
                {
                    platformScript plat = mS.arrayPlatformsScripts[i];
                    if (plat != null && plat.ownerID == pS.myID && plat.isUnlocked && !plat.vomMarktGenommen)
                    {
                        targetPlatId = plat.myID;
                        break;
                    }
                }
                if (targetPlatId == -1)
                {
                    gui.MessageBox($"{pS.GetName()} does not have an active console on the market!", false);
                    return;
                }
                
                game.exklusiv = true;
                game.publisherID = pS.myID;
                game.pS_ = pS;
                game.pubAngebot_Gewinnbeteiligung = 5f; // 5% royalty for full funding
                game.gamePlatform[0] = targetPlatId;
                for (int i = 1; i < 4; i++) game.gamePlatform[i] = -1;

                // Refund 100% of costs spent so far
                long refund = game.costs_entwicklung + game.costs_mitarbeiter;
                mS.Earn(refund, 10);

                state.LastExclusiveWeek = currentWeek;
                AdjustRelation(pS, 15f); // +15 relationship points
                SaveState();
                gui.MessageBox($"Game '{game.myName}' is now exclusive to {pS.GetName()}'s console. Refunded 100% of costs spent so far ({mS.GetMoney(refund, true)}).", false);
            },
            () => {
                activeOutsourceNpcId = pS.myID;
                activeOutsourceIsNewIP = 0; // Exclusivity / Full Funding
                BeginOutsourceDesignInterception(pS);
            }
        );
    }

    private static void HandleFirstPartyBailout(publisherScript pS)
    {
        mainScript mS = FindObjectOfType<mainScript>();
        GUI_Main gui = FindObjectOfType<GUI_Main>();
        if (mS == null || gui == null) return;

        StudioState state = EnsureState(pS.myID);
        if (state.FundedByPlayer) // Used as "bailout used" flag for this playthrough
        {
            gui.MessageBox("You can only activate First-Party Partner Bailout once per playthrough.", false);
            return;
        }

        // Clear loans and increase cash
        mS.kredit = 0;
        mS.Earn(Math.Max(0, 1000000L - mS.money) + 500000L, 1);

        // Take all player game IPs
        games g = FindObjectOfType<games>();
        if (g != null && g.arrayGamesScripts != null)
        {
            for (int i = 0; i < g.arrayGamesScripts.Length; i++)
            {
                gameScript game = g.arrayGamesScripts[i];
                if (game != null && game.ownerID == mS.myID)
                {
                    game.ownerID = pS.myID;
                    game.ownerS_ = pS;
                }
            }
        }

        state.FundedByPlayer = true;
        pS.relation = Mathf.Clamp(pS.relation + 30f, 0f, 100f);
        SaveState();
        gui.MessageBox("First-Party Partner Bailout executed successfully! All debt cleared, received $500,000. All game IPs transferred to " + pS.GetName() + ".", false);
    }

    private static void HandlePremiumContract(publisherScript pS)
    {
        mainScript mS = FindObjectOfType<mainScript>();
        GUI_Main gui = FindObjectOfType<GUI_Main>();
        if (mS == null || gui == null) return;

        StudioState state = EnsureState(pS.myID);
        int currentWeek = GetGlobalWeek(mS);
        if (currentWeek - state.LastCoFinanceWeek < 96)
        {
            gui.MessageBox("This action is on cooldown. Available in " + (96 - (currentWeek - state.LastCoFinanceWeek)) + " weeks.", false);
            return;
        }

        ShowProjectSelectorDialog(pS, "Select in-development project for Signing Bonus:", 
            (game) => {
                long bonus = 100000L;
                if (game.gameSize == 1) bonus = 200000L;
                else if (game.gameSize == 2) bonus = 400000L;
                else if (game.gameSize == 3) bonus = 800000L;
                else if (game.gameSize == 4) bonus = 1500000L;
                else if (game.gameSize == 5) bonus = 3000000L;

                mS.Earn(bonus, 10);
                game.publisherID = pS.myID;
                game.pS_ = pS;
                game.pubAngebot_Gewinnbeteiligung = 10f; // 10% royalty rate

                state.LastCoFinanceWeek = currentWeek;
                AdjustRelation(pS, 8f); // +8 relationship points
                SaveState();
                gui.MessageBox($"Game '{game.myName}' signed with {pS.GetName()}! Received signing bonus of {mS.GetMoney(bonus, true)}.", false);
            },
            () => {
                activeOutsourceNpcId = pS.myID;
                activeOutsourceIsNewIP = 1; // Trusted Partner / Signing Bonus
                BeginOutsourceDesignInterception(pS);
            }
        );
    }

    private static void HandleIPCooperation(publisherScript pS)
    {
        mainScript mS = FindObjectOfType<mainScript>();
        GUI_Main gui = FindObjectOfType<GUI_Main>();
        if (mS == null || gui == null) return;

        StudioState state = EnsureState(pS.myID);
        int currentWeek = GetGlobalWeek(mS);
        if (currentWeek - state.LastCoFinanceWeek < 144)
        {
            gui.MessageBox("This action is on cooldown. Available in " + (144 - (currentWeek - state.LastCoFinanceWeek)) + " weeks.", false);
            return;
        }

        // Cooperate using their IP
        activeOutsourceNpcId = pS.myID;
        activeOutsourceIsNewIP = 2; // NPC IP Cooperation
        BeginOutsourceDesignInterception(pS);
    }

    private static void HandleIPCooperationSmall(publisherScript pS)
    {
        HandleIPCooperation(pS);
    }

    private static void HandleAAACoPublish(publisherScript pS)
    {
        mainScript mS = FindObjectOfType<mainScript>();
        GUI_Main gui = FindObjectOfType<GUI_Main>();
        if (mS == null || gui == null) return;

        StudioState state = EnsureState(pS.myID);
        int currentWeek = GetGlobalWeek(mS);
        if (currentWeek - state.LastAAAWeek < 144)
        {
            gui.MessageBox("This action is on cooldown. Available in " + (144 - (currentWeek - state.LastAAAWeek)) + " weeks.", false);
            return;
        }

        activeOutsourceNpcId = pS.myID;
        activeOutsourceIsNewIP = 4; // AAA Co-Publishing Pitch (not 1)
        BeginOutsourceDesignInterception(pS);
    }

    private static void HandleBigPublisherExclusives(publisherScript pS)
    {
        mainScript mS = FindObjectOfType<mainScript>();
        GUI_Main gui = FindObjectOfType<GUI_Main>();
        if (mS == null || gui == null) return;

        if (!PlayerHasActiveConsole(mS) && !PlayerHasActiveSubscription(mS))
        {
            gui.MessageBox("Requires an active player console on the market or an active subscription service.", false);
            return;
        }

        StudioState state = EnsureState(pS.myID);
        int currentWeek = GetGlobalWeek(mS);
        if (currentWeek - state.LastExclusiveWeek < 144)
        {
            gui.MessageBox("This action is on cooldown. Available in " + (144 - (currentWeek - state.LastExclusiveWeek)) + " weeks.", false);
            return;
        }

        gameScript inDev = pS.FindGameInDevelopment();
        if (inDev != null)
        {
            ShowExclusivityOrSubChoiceDialog(pS, inDev, 3, 20f);
        }
        else
        {
            gui.MessageBox($"{pS.GetName()} does not currently have any active games in development.", false);
        }
    }

    private static void HandleFriendlyBuyout(publisherScript pS, float discountFactor, MonoBehaviour menu)
    {
        GUI_Main gui = FindObjectOfType<GUI_Main>();
        if (gui == null) return;

        activeBuyoutDiscountFactor = discountFactor; // Set before initialization!

        gui.ActivateMenu(gui.uiObjects[386]);
        Menu_W_FirmaKaufen fK = gui.uiObjects[386].GetComponent<Menu_W_FirmaKaufen>();
        if (fK != null)
        {
            fK.Init(pS);
            
            long cost = pS.GetFirmenwert();
            long discountedCost = (long)(cost * discountFactor);
            
            Text costText = fK.uiObjects[1].GetComponent<Text>();
            if (costText != null)
            {
                mainScript mS = FindObjectOfType<mainScript>();
                costText.text = mS.GetMoney(discountedCost, true);
            }
        }
    }

    private static void HandleSellIPToNPC(publisherScript pS)
    {
        GUI_Main gui = FindObjectOfType<GUI_Main>();
        if (gui != null)
        {
            gui.ActivateMenu(gui.uiObjects[361]); // Open player IPs list to sell
            pS.relation = Mathf.Clamp(pS.relation + 10f, 0f, 100f);
            SaveState();
        }
    }

    private static void HandleConsoleExclusivityNPC(publisherScript pS, int subdiv)
    {
        mainScript mS = FindObjectOfType<mainScript>();
        GUI_Main gui = FindObjectOfType<GUI_Main>();
        if (mS == null || gui == null) return;

        if (subdiv == 6) // Indie Dev Tier 4 Console Auto-Publish Contract
        {
            if (!PlayerHasActiveConsole(mS) && !PlayerHasActiveSubscription(mS))
            {
                gui.MessageBox("Requires an active player console on the market or an active subscription service.", false);
                return;
            }

            StudioState state = EnsureState(pS.myID);
            long fee = CalculateAutoPublishYearlyFee(pS);
            if (mS.money < fee)
            {
                gui.ShowNoMoney();
                return;
            }
            mS.Pay(fee, 10);
            
            state.ActiveContract = "ConsoleAutoPublish";
            state.ContractWeeksLeft = 48 * 2;
            AdjustRelation(pS, 15f); // +15 relationship points initially
            SaveState();
            gui.MessageBox($"Signed Console Exclusivity & Subscription Contract with {pS.GetName()} for 2 years! All their games will be exclusive to your console and on your subscription service.", false);
            return;
        }

        // For Mid-Sized Dev (subdiv == 5) and Small Pub (subdiv == 3): choice dialog
        gameScript inDev = pS.FindGameInDevelopment();
        if (inDev != null)
        {
            float relGain = (subdiv == 3) ? 15f : 12f;
            int cdYears = (subdiv == 3) ? 2 : 3;
            ShowExclusivityOrSubChoiceDialog(pS, inDev, cdYears, relGain);
        }
        else
        {
            gui.MessageBox($"{pS.GetName()} does not currently have any active games in development.", false);
        }
    }

    private static void HandlePublishInDevGame(publisherScript pS)
    {
        mainScript mS = FindObjectOfType<mainScript>();
        GUI_Main gui = FindObjectOfType<GUI_Main>();
        if (mS == null || gui == null) return;

        gameScript inDev = pS.FindGameInDevelopment();
        if (inDev != null)
        {
            inDev.publisherID = mS.myID;
            inDev.pS_ = null;
            pS.relation = Mathf.Clamp(pS.relation + 5f, 0f, 100f);
            SaveState();
            gui.MessageBox($"You signed as publisher for {pS.GetName()}'s project: <color=blue><b>{inDev.GetNameWithTag()}</b></color>!", false);
        }
        else
        {
            gui.MessageBox($"{pS.GetName()} does not currently have any active games in development to publish.", false);
        }
    }

    private static void HandleIPLicensingPartner(publisherScript pS)
    {
        mainScript mS = FindObjectOfType<mainScript>();
        GUI_Main gui = FindObjectOfType<GUI_Main>();
        if (mS == null || gui == null) return;

        activeOutsourceNpcId = pS.myID;
        activeOutsourceIsNewIP = 2; // Let us make games in their IP
        BeginOutsourceDesignInterception(pS);
    }

    private static void HandleBuyIPFromNPC(publisherScript pS, int cdYears)
    {
        mainScript mS = FindObjectOfType<mainScript>();
        GUI_Main gui = FindObjectOfType<GUI_Main>();
        if (mS == null || gui == null) return;

        StudioState state = EnsureState(pS.myID);
        int currentWeek = GetGlobalWeek(mS);
        int cdWeeks = cdYears * 48;
        if (currentWeek - state.LastIPBuyWeek < cdWeeks)
        {
            gui.MessageBox("IP purchase is on cooldown. Available in " + (cdWeeks - (currentWeek - state.LastIPBuyWeek)) + " weeks.", false);
            return;
        }

        games g = FindObjectOfType<games>();
        List<gameScript> ips = new List<gameScript>();
        if (g != null && g.arrayGamesScripts != null)
        {
            for (int i = 0; i < g.arrayGamesScripts.Length; i++)
            {
                gameScript game = g.arrayGamesScripts[i];
                if (game != null && game.developerID == pS.myID && game.ownerID == pS.myID && !game.inDevelopment)
                {
                    if (!ips.Any(x => x.myName == game.myName))
                    {
                        ips.Add(game);
                    }
                }
            }
        }

        if (ips.Count == 0)
        {
            gui.MessageBox($"{pS.GetName()} does not have any mature IPs to buy.", false);
            return;
        }

        ShowIPSelectorDialog(pS, ips, (boughtGame) =>
        {
            boughtGame.ownerID = mS.myID;
            boughtGame.ownerS_ = null;

            if (g != null && g.arrayGamesScripts != null)
            {
                for (int i = 0; i < g.arrayGamesScripts.Length; i++)
                {
                    gameScript game = g.arrayGamesScripts[i];
                    if (game != null && game.myName == boughtGame.myName)
                    {
                        game.ownerID = mS.myID;
                        game.ownerS_ = null;
                    }
                }
            }

            state.LastIPBuyWeek = currentWeek; 
            pS.relation = Mathf.Clamp(pS.relation + 15f, 0f, 100f);
            SaveState();
            gui.MessageBox($"Successfully bought all IP rights for: <color=blue><b>{boughtGame.myName}</b></color> from {pS.GetName()}!", false);
        });
    }

    private static void HandleGiftEngineLicense(publisherScript pS)
    {
        mainScript mS = FindObjectOfType<mainScript>();
        GUI_Main gui = FindObjectOfType<GUI_Main>();
        if (mS == null || gui == null) return;

        bool hasPlayerEngine = false;
        if (mS.arrayEnginesScripts != null)
        {
            for (int i = 0; i < mS.arrayEnginesScripts.Length; i++)
            {
                engineScript engine = mS.arrayEnginesScripts[i];
                if (engine != null && engine.myID == mS.myID && engine.sellEngine)
                {
                    hasPlayerEngine = true;
                    break;
                }
            }
        }

        if (!hasPlayerEngine)
        {
            gui.MessageBox("You must create and sell at least one engine to gift a license!", false);
            return;
        }

        if (processedEngineLicenses.Contains(pS.myID))
        {
            gui.MessageBox($"You have already gifted a free engine license to {pS.GetName()}.", false);
            return;
        }

        processedEngineLicenses.Add(pS.myID);
        
        int subdiv = GetSubdivision(pS);
        float gain = 15f;
        if (subdiv == 6) gain = 20f; // Indie gets +20

        AdjustRelation(pS, gain);
        SaveState();

        gui.MessageBox($"You gifted a free engine license to {pS.GetName()}! Relationship increased (+{gain}).", false);
    }

    private static void ShowCommissionOfferDialog(publisherScript PH, int genre, int size, int targetReview, long reward)
    {
        mainScript mS = FindObjectOfType<mainScript>();
        GUI_Main gui = FindObjectOfType<GUI_Main>();
        if (gui == null || mS == null) return;

        if (choiceDialog != null) UnityEngine.Object.Destroy(choiceDialog);

        Font font = ResolveFont(gui, gui.uiObjects);

        choiceDialog = new GameObject("SPO_CommissionChoiceDialog", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        StyleDialogBackground(choiceDialog, gui);

        RectTransform rt = choiceDialog.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = new Vector2(420f, 240f);
        rt.anchoredPosition = Vector2.zero;
        rt.localScale = Vector3.one;

        Text txt = new GameObject("Text", typeof(RectTransform), typeof(Text)).GetComponent<Text>();
        txt.transform.SetParent(choiceDialog.transform, false);
        RectTransform txtRt = txt.GetComponent<RectTransform>();
        txtRt.anchorMin = new Vector2(0.05f, 0.35f);
        txtRt.anchorMax = new Vector2(0.95f, 0.95f);
        txtRt.offsetMin = Vector2.zero;
        txtRt.offsetMax = Vector2.zero;
        txt.font = font;
        txt.fontSize = 13;
        txt.color = Color.white;
        txt.alignment = TextAnchor.MiddleCenter;

        genres genres = mS.GetComponent<genres>();
        string genreName = genres != null ? genres.GetName(genre) : "Unknown";

        txt.text = $"<b>Development Commission from {PH.GetName()}</b>\n\n" +
                   $"They want you to develop and release a new <b>{genreName}</b> game of size <b>{GetGameSizeString(size)}</b> exclusive to their platform.\n" +
                   $"<b>Target Review</b>: >= {targetReview}%\n" +
                   $"<b>Time Limit</b>: 48 weeks\n" +
                   $"<b>Reward</b>: {mS.GetMoney(reward, true)}\n\n" +
                   $"Accept this commission?";

        GameObject btn1Obj = new GameObject("BtnAccept", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
        btn1Obj.transform.SetParent(choiceDialog.transform, false);
        RectTransform btn1Rt = btn1Obj.GetComponent<RectTransform>();
        btn1Rt.anchorMin = new Vector2(0.05f, 0.1f);
        btn1Rt.anchorMax = new Vector2(0.48f, 0.3f);
        btn1Rt.offsetMin = Vector2.zero;
        btn1Rt.offsetMax = Vector2.zero;
        Image btn1Img = btn1Obj.GetComponent<Image>();
        btn1Img.color = new Color(0.2f, 0.4f, 0.2f);
        Button btn1 = btn1Obj.GetComponent<Button>();
        btn1.targetGraphic = btn1Img;

        Text btn1Txt = new GameObject("Text", typeof(RectTransform), typeof(Text)).GetComponent<Text>();
        btn1Txt.transform.SetParent(btn1Obj.transform, false);
        RectTransform btn1TxtRt = btn1Txt.GetComponent<RectTransform>();
        btn1TxtRt.anchorMin = Vector2.zero;
        btn1TxtRt.anchorMax = Vector2.one;
        btn1TxtRt.offsetMin = Vector2.zero;
        btn1TxtRt.offsetMax = Vector2.zero;
        btn1Txt.font = font;
        btn1Txt.fontSize = 12;
        btn1Txt.fontStyle = FontStyle.Bold;
        btn1Txt.color = Color.white;
        btn1Txt.alignment = TextAnchor.MiddleCenter;
        btn1Txt.text = "Accept";

        btn1.onClick.AddListener(() =>
        {
            StudioState state = EnsureState(PH.myID);
            state.CommissionWeeksLeft = 48;
            state.CommissionGenre = genre;
            state.CommissionTargetReview = targetReview;
            state.CommissionReward = reward;
            SaveState();

            gui.MessageBox($"Accepted commission from {PH.GetName()}! You have 48 weeks to release a {genreName} game.", false);

            UnityEngine.Object.Destroy(choiceDialog);
            choiceDialog = null;
        });

        GameObject btn2Obj = new GameObject("BtnDecline", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
        btn2Obj.transform.SetParent(choiceDialog.transform, false);
        RectTransform btn2Rt = btn2Obj.GetComponent<RectTransform>();
        btn2Rt.anchorMin = new Vector2(0.52f, 0.1f);
        btn2Rt.anchorMax = new Vector2(0.95f, 0.3f);
        btn2Rt.offsetMin = Vector2.zero;
        btn2Rt.offsetMax = Vector2.zero;
        Image btn2Img = btn2Obj.GetComponent<Image>();
        btn2Img.color = new Color(0.5f, 0.2f, 0.2f);
        Button btn2 = btn2Obj.GetComponent<Button>();
        btn2.targetGraphic = btn2Img;

        Text btn2Txt = new GameObject("Text", typeof(RectTransform), typeof(Text)).GetComponent<Text>();
        btn2Txt.transform.SetParent(btn2Obj.transform, false);
        RectTransform btn2TxtRt = btn2Txt.GetComponent<RectTransform>();
        btn2TxtRt.anchorMin = Vector2.zero;
        btn2TxtRt.anchorMax = Vector2.one;
        btn2TxtRt.offsetMin = Vector2.zero;
        btn2TxtRt.offsetMax = Vector2.zero;
        btn2Txt.font = font;
        btn2Txt.fontSize = 12;
        btn2Txt.fontStyle = FontStyle.Bold;
        btn2Txt.color = Color.white;
        btn2Txt.alignment = TextAnchor.MiddleCenter;
        btn2Txt.text = "Decline";

        btn2.onClick.AddListener(() =>
        {
            UnityEngine.Object.Destroy(choiceDialog);
            choiceDialog = null;
        });
    }

    private static void HandleOutsourceProject(publisherScript pS)
    {
        activeOutsourceNpcId = pS.myID;
        activeOutsourceIsNewIP = 0; // Outsource player IP
        BeginOutsourceDesignInterception(pS);
    }

    private static void HandleAutoPublishContract(publisherScript pS)
    {
        mainScript mS = FindObjectOfType<mainScript>();
        GUI_Main gui = FindObjectOfType<GUI_Main>();
        if (mS == null || gui == null) return;

        StudioState state = EnsureState(pS.myID);
        long fee = CalculateAutoPublishYearlyFee(pS);
        if (mS.money < fee)
        {
            gui.ShowNoMoney();
            return;
        }
        mS.Pay(fee, 10);
        
        state.ActiveContract = "AutoPublish";
        state.ContractWeeksLeft = 48 * 2; // 2 years contract
        AdjustRelation(pS, 10f); // +10 relationship points initially
        SaveState();
        gui.MessageBox($"Signed Automatic Publication Contract with {pS.GetName()} for 2 years! All their games will be published by you.", false);
    }

    // Interception of Game Design Screens for NPC Outsourcing

    private static System.Collections.IEnumerator OpenDevGameMainDelayed(GUI_Main gui, roomScript room)
    {
        yield return null; // Wait 1 frame for menus to close and canvas sorting to update
        if (gui != null && gui.uiObjects != null && gui.uiObjects[57] != null)
        {
            gui.OpenMenu(false);
            gui.ActivateMenu(gui.uiObjects[57]); // Open DevGameMain menu
            gui.uiObjects[57].transform.SetAsLastSibling();
            gui.uiObjects[57].GetComponent<Menu_DevGameMain>().Init(room);
        }
    }

    private static void BeginOutsourceDesignInterception(publisherScript pS)
    {
        mainScript main = FindObjectOfType<mainScript>();
        GUI_Main gui = FindObjectOfType<GUI_Main>();
        if (main == null || gui == null) return;

        // Close details screens
        Menu_Stats_Developer_Main statsDevMain = FindObjectOfType<Menu_Stats_Developer_Main>();
        if (statsDevMain != null && statsDevMain.gameObject.activeSelf)
        {
            statsDevMain.BUTTON_Abbrechen();
        }
        Menu_Stats_Publisher_Main statsPubMain = FindObjectOfType<Menu_Stats_Publisher_Main>();
        if (statsPubMain != null && statsPubMain.gameObject.activeSelf)
        {
            statsPubMain.BUTTON_Abbrechen();
        }

        // Close statistics menus
        Menu_Statistics_Developer statsDev = FindObjectOfType<Menu_Statistics_Developer>();
        if (statsDev != null && statsDev.gameObject.activeSelf)
        {
            statsDev.BUTTON_Close();
        }
        Menu_Statistics_Publisher statsPub = FindObjectOfType<Menu_Statistics_Publisher>();
        if (statsPub != null && statsPub.gameObject.activeSelf)
        {
            statsPub.BUTTON_Close();
        }

        ClearSidePanel();

        roomScript room = CreateTemporaryDesignRoom();
        if (room == null)
        {
            gui.MessageBox("Could not create a design context for outsourcing.", false);
            return;
        }

        borrowedRoom = room;
        borrowedRoomOriginalTaskId = room.taskID;
        borrowedRoomOriginalTaskObject = room.taskGameObject;
        pendingStartSnapshot = null;

        // Start delayed menu activation on the next frame to avoid sorting issues
        if (Instance != null)
        {
            Instance.StartCoroutine(OpenDevGameMainDelayed(gui, room));
        }
        else
        {
            // Fallback if Instance is not ready
            gui.OpenMenu(false);
            gui.ActivateMenu(gui.uiObjects[57]);
            gui.uiObjects[57].transform.SetAsLastSibling();
            gui.uiObjects[57].GetComponent<Menu_DevGameMain>().Init(room);
        }
    }

    private static roomScript CreateTemporaryDesignRoom()
    {
        CleanupTemporaryDesignRoom();
        temporaryDesignRoomObject = new GameObject("SPO_TemporaryDesignRoom");
        temporaryDesignRoomObject.hideFlags = HideFlags.HideAndDontSave;
        roomScript room = temporaryDesignRoomObject.AddComponent<roomScript>();
        room.myID = -99988877;
        room.typ = 1;
        room.taskID = -1;
        room.taskGameObject = null;
        room.myName = "NPC Outsource Context";
        room.uiPos = Vector3.zero;
        temporaryDesignRoomObject.SetActive(false);
        return room;
    }

    private static void CleanupTemporaryDesignRoom()
    {
        if (temporaryDesignRoomObject != null)
        {
            UnityEngine.Object.Destroy(temporaryDesignRoomObject);
            temporaryDesignRoomObject = null;
        }
    }

    private static void CleanupStaleDesignContext()
    {
        if (activeOutsourceNpcId == -1)
        {
            if (temporaryDesignRoomObject != null)
            {
                CleanupTemporaryDesignRoom();
            }
            framesSinceNoDesignMenu = 0;
            return;
        }

        if (pendingStartSnapshot != null)
        {
            framesSinceNoDesignMenu = 0;
            return;
        }

        GUI_Main gui = GetGuiMainCached();
        if (gui == null || gui.uiObjects == null)
        {
            return;
        }

        if (!IsAnyDirectorDesignMenuActive(gui))
        {
            framesSinceNoDesignMenu++;
            if (framesSinceNoDesignMenu >= 10) // Grace period of 10 frames
            {
                CancelDesignContext();
                framesSinceNoDesignMenu = 0;
            }
        }
        else
        {
            framesSinceNoDesignMenu = 0;
        }
    }

    private static bool IsAnyDirectorDesignMenuActive(GUI_Main gui)
    {
        return IsUiActive(gui, 56) ||
               IsUiActive(gui, 57) ||
               IsUiActive(gui, 97) ||
               IsUiActive(gui, 98) ||
               IsUiActive(gui, 310) ||
               IsUiActive(gui, 312) ||
               IsUiActive(gui, 313) ||
               IsUiActive(gui, 369) ||
               IsUiActive(gui, 370) ||
               IsUiActive(gui, 441);
    }

    private static bool IsUiActive(GUI_Main gui, int index)
    {
        return gui != null &&
               gui.uiObjects != null &&
               gui.uiObjects.Length > index &&
               gui.uiObjects[index] != null &&
               gui.uiObjects[index].activeSelf;
    }

    private static void CancelDesignContext()
    {
        activeOutsourceNpcId = -1;
        activeOutsourceIsNewIP = -1;
        CleanupTemporaryDesignRoom();
        borrowedRoom = null;
        borrowedRoomOriginalTaskId = -1;
        borrowedRoomOriginalTaskObject = null;
        pendingStartSnapshot = null;
    }

    public static void CaptureStartSnapshot()
    {
        if (activeOutsourceNpcId == -1)
        {
            pendingStartSnapshot = null;
            return;
        }

        games games = FindObjectOfType<games>();
        HashSet<int> knownGameIds = new HashSet<int>();
        if (games != null && games.arrayGamesScripts != null)
        {
            for (int i = 0; i < games.arrayGamesScripts.Length; i++)
            {
                gameScript game = games.arrayGamesScripts[i];
                if (game != null)
                {
                    knownGameIds.Add(game.myID);
                }
            }
        }

        pendingStartSnapshot = new StartSnapshot
        {
            KnownGameIds = knownGameIds,
            Room = borrowedRoom,
            OriginalTaskId = borrowedRoom != null ? borrowedRoom.taskID : borrowedRoomOriginalTaskId,
            OriginalTaskObject = borrowedRoom != null ? borrowedRoom.taskGameObject : borrowedRoomOriginalTaskObject
        };
    }

    public static void AdjustRelation(publisherScript pS, float amt)
    {
        if (pS == null) return;
        mainScript mS = FindObjectOfType<mainScript>();
        
        // CRIVALRY LOCKOUT RULE: Platform Holder and player has active console -> relationship is locked to 20
        int subdiv = GetSubdivision(pS);
        if (subdiv == 1 && PlayerHasActiveConsole(mS))
        {
            pS.relation = 20f;
            SaveState();
            return;
        }

        pS.relation = Mathf.Clamp(pS.relation + amt, 0f, 100f);
        SaveState();
    }

    private static void GetGoodwillTrend(int studioId, out float trendScore, out string label)
    {
        trendScore = 0f;
        label = "Stable";
        string path = Path.Combine(Paths.ConfigPath, "DynamicStudioGoodwill_State.tsv");
        if (!File.Exists(path)) return;
        try
        {
            string[] lines = File.ReadAllLines(path);
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) continue;
                string[] parts = line.Split('\t');
                if (parts.Length >= 4)
                {
                    int id;
                    if (int.TryParse(parts[0], out id) && id == studioId)
                    {
                        float.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out trendScore);
                        label = parts[3];
                        return;
                    }
                }
            }
        }
        catch { }
    }

    private static long CalculateOutsourceCost(publisherScript npc, gameScript game)
    {
        float starMultiplier = 1.0f + (npc.stars / 50f);
        
        float trendScore;
        string label;
        GetGoodwillTrend(npc.myID, out trendScore, out label);
        float trendMultiplier = 1.0f + (trendScore / 20f);
        if (trendMultiplier < 0.5f) trendMultiplier = 0.5f;

        float sizeFactor = 1.0f;
        if (game.gameSize == 0) sizeFactor = 0.5f;
        else if (game.gameSize == 1) sizeFactor = 1.0f;
        else if (game.gameSize == 2) sizeFactor = 2.0f;
        else if (game.gameSize == 3) sizeFactor = 4.0f;
        else if (game.gameSize == 4) sizeFactor = 8.0f;

        long baseFee = (long)(game.costs_entwicklung * starMultiplier * trendMultiplier * sizeFactor);
        if (baseFee < 10000L) baseFee = 10000L;
        return baseFee;
    }

    private static long CalculateAutoPublishYearlyFee(publisherScript dev)
    {
        float starMultiplier = 1.0f + (dev.stars / 50f);
        
        float trendScore;
        string label;
        GetGoodwillTrend(dev.myID, out trendScore, out label);
        float trendMultiplier = 1.0f + (trendScore / 20f);
        if (trendMultiplier < 0.5f) trendMultiplier = 0.5f;

        long baseFee = (long)(50000L * starMultiplier * trendMultiplier);
        if (baseFee < 10000L) baseFee = 10000L;
        return baseFee;
    }

    public static string GetGameSizeString(int size)
    {
        switch (size)
        {
            case 0: return "B";
            case 1: return "B+";
            case 2: return "A";
            case 3: return "AA";
            case 4: return "AAA";
            case 5: return "AAAA";
            default: return "Unknown";
        }
    }

    public static int GetPlayerPlatformID(mainScript mS)
    {
        if (mS == null || mS.arrayPlatformsScripts == null) return -1;
        for (int i = 0; i < mS.arrayPlatformsScripts.Length; i++)
        {
            platformScript plat = mS.arrayPlatformsScripts[i];
            if (plat != null && plat.ownerID == mS.myID && plat.isUnlocked && !plat.vomMarktGenommen && (plat.typ == 0 || plat.typ == 1 || plat.typ == 2))
            {
                return plat.myID;
            }
        }
        return -1;
    }

    public static void ShowOutsourceChoiceDialog(publisherScript npc, gameScript createdGame, StartSnapshot snapshot)
    {
        mainScript mS = FindObjectOfType<mainScript>();
        GUI_Main gui = FindObjectOfType<GUI_Main>();
        if (gui == null || mS == null) return;

        if (choiceDialog != null) UnityEngine.Object.Destroy(choiceDialog);

        Font font = ResolveFont(gui, gui.uiObjects);

        choiceDialog = new GameObject("SPO_ChoiceDialog", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        choiceDialog.transform.SetParent(gui.uiObjects[57].transform.parent, false);

        Image bg = choiceDialog.GetComponent<Image>();
        bg.color = new Color(0.1f, 0.1f, 0.12f, 0.98f);

        RectTransform rt = choiceDialog.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = new Vector2(400f, 220f);
        rt.anchoredPosition = Vector2.zero;
        rt.localScale = Vector3.one;

        Text txt = new GameObject("Text", typeof(RectTransform), typeof(Text)).GetComponent<Text>();
        txt.transform.SetParent(choiceDialog.transform, false);
        RectTransform txtRt = txt.GetComponent<RectTransform>();
        txtRt.anchorMin = new Vector2(0.05f, 0.4f);
        txtRt.anchorMax = new Vector2(0.95f, 0.95f);
        txtRt.offsetMin = Vector2.zero;
        txtRt.offsetMax = Vector2.zero;
        txt.font = font;
        txt.fontSize = 14;
        txt.color = Color.white;
        txt.alignment = TextAnchor.MiddleCenter;

        long normalCost = CalculateOutsourceCost(npc, createdGame);
        long cancelCost = (long)(normalCost * 1.5);

        txt.text = npc.GetName() + " is currently busy developing another game.\n\n" +
                   "Choose action for outsourcing:\n" +
                   "1. Queue project (Cost: " + mS.GetMoney(normalCost, true) + ")\n" +
                   "2. Cancel current project (Cost: " + mS.GetMoney(cancelCost, true) + ", -10 Relationship)";

        GameObject btn1Obj = new GameObject("BtnQueue", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
        btn1Obj.transform.SetParent(choiceDialog.transform, false);
        RectTransform btn1Rt = btn1Obj.GetComponent<RectTransform>();
        btn1Rt.anchorMin = new Vector2(0.05f, 0.1f);
        btn1Rt.anchorMax = new Vector2(0.48f, 0.35f);
        btn1Rt.offsetMin = Vector2.zero;
        btn1Rt.offsetMax = Vector2.zero;
        Image btn1Img = btn1Obj.GetComponent<Image>();
        btn1Img.color = new Color(0.2f, 0.4f, 0.2f);
        Button btn1 = btn1Obj.GetComponent<Button>();
        btn1.targetGraphic = btn1Img;

        Text btn1Txt = new GameObject("Text", typeof(RectTransform), typeof(Text)).GetComponent<Text>();
        btn1Txt.transform.SetParent(btn1Obj.transform, false);
        RectTransform btn1TxtRt = btn1Txt.GetComponent<RectTransform>();
        btn1TxtRt.anchorMin = Vector2.zero;
        btn1TxtRt.anchorMax = Vector2.one;
        btn1TxtRt.offsetMin = Vector2.zero;
        btn1TxtRt.offsetMax = Vector2.zero;
        btn1Txt.font = font;
        btn1Txt.fontSize = 12;
        btn1Txt.fontStyle = FontStyle.Bold;
        btn1Txt.color = Color.white;
        btn1Txt.alignment = TextAnchor.MiddleCenter;
        btn1Txt.text = "Queue Project";

        btn1.onClick.AddListener(() =>
        {
            mS.Pay(normalCost, 10);
            StudioState state = EnsureState(npc.myID);
            QueueOutsourceProject(state, createdGame);
            gui.MessageBox("Outsource project queued for " + npc.GetName() + ".", false);
            CancelDesignContext();
            UnityEngine.Object.Destroy(choiceDialog);
            choiceDialog = null;
        });

        GameObject btn2Obj = new GameObject("BtnCancel", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
        btn2Obj.transform.SetParent(choiceDialog.transform, false);
        RectTransform btn2Rt = btn2Obj.GetComponent<RectTransform>();
        btn2Rt.anchorMin = new Vector2(0.52f, 0.1f);
        btn2Rt.anchorMax = new Vector2(0.95f, 0.35f);
        btn2Rt.offsetMin = Vector2.zero;
        btn2Rt.offsetMax = Vector2.zero;
        Image btn2Img = btn2Obj.GetComponent<Image>();
        btn2Img.color = new Color(0.5f, 0.2f, 0.2f);
        Button btn2 = btn2Obj.GetComponent<Button>();
        btn2.targetGraphic = btn2Img;

        Text btn2Txt = new GameObject("Text", typeof(RectTransform), typeof(Text)).GetComponent<Text>();
        btn2Txt.transform.SetParent(btn2Obj.transform, false);
        RectTransform btn2TxtRt = btn2Txt.GetComponent<RectTransform>();
        btn2TxtRt.anchorMin = Vector2.zero;
        btn2TxtRt.anchorMax = Vector2.one;
        btn2TxtRt.offsetMin = Vector2.zero;
        btn2TxtRt.offsetMax = Vector2.zero;
        btn2Txt.font = font;
        btn2Txt.fontSize = 12;
        btn2Txt.fontStyle = FontStyle.Bold;
        btn2Txt.color = Color.white;
        btn2Txt.alignment = TextAnchor.MiddleCenter;
        btn2Txt.text = "Force Cancel";

        btn2.onClick.AddListener(() =>
        {
            mS.Pay(cancelCost, 10);
            
            gameScript inDev = npc.FindGameInDevelopment();
            if (inDev != null)
            {
                inDev.gameObject.tag = "GameRemoved";
                UnityEngine.Object.Destroy(inDev.gameObject);
            }

            AdjustRelation(npc, -10f);
            StartOutsourceProjectImmediately(npc, createdGame);
            gui.MessageBox("Forced cancel of current project! Outsource project assigned to " + npc.GetName() + " immediately.", false);
            CancelDesignContext();
            UnityEngine.Object.Destroy(choiceDialog);
            choiceDialog = null;
        });
    }

    public static void ShowCoPublishChoiceDialog(publisherScript npc, gameScript createdGame)
    {
        mainScript mS = FindObjectOfType<mainScript>();
        GUI_Main gui = FindObjectOfType<GUI_Main>();
        if (gui == null || mS == null) return;

        if (choiceDialog != null) UnityEngine.Object.Destroy(choiceDialog);

        Font font = ResolveFont(gui, gui.uiObjects);

        choiceDialog = new GameObject("SPO_CoPublishChoiceDialog", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        choiceDialog.transform.SetParent(gui.uiObjects[57].transform.parent, false);

        Image bg = choiceDialog.GetComponent<Image>();
        bg.color = new Color(0.1f, 0.1f, 0.12f, 0.98f);

        RectTransform rt = choiceDialog.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = new Vector2(400f, 220f);
        rt.anchoredPosition = Vector2.zero;
        rt.localScale = Vector3.one;

        Text txt = new GameObject("Text", typeof(RectTransform), typeof(Text)).GetComponent<Text>();
        txt.transform.SetParent(choiceDialog.transform, false);
        txt.GetComponent<RectTransform>().anchorMin = new Vector2(0.05f, 0.4f);
        txt.GetComponent<RectTransform>().anchorMax = new Vector2(0.95f, 0.95f);
        txt.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        txt.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        txt.font = font;
        txt.fontSize = 14;
        txt.color = Color.white;
        txt.alignment = TextAnchor.MiddleCenter;
        txt.text = "AAA Co-Publishing Deal with " + npc.GetName() + "\n\n" +
                   "Select Agreement Type:\n" +
                   "1. Keep IP Rights (Get 50% funding, lower royalties)\n" +
                   "2. Gift IP Rights (Get 100% funding, better royalties, +20 Relationship)";

        GameObject btn1Obj = new GameObject("BtnKeep", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
        btn1Obj.transform.SetParent(choiceDialog.transform, false);
        RectTransform btn1Rt = btn1Obj.GetComponent<RectTransform>();
        btn1Rt.anchorMin = new Vector2(0.05f, 0.1f);
        btn1Rt.anchorMax = new Vector2(0.48f, 0.35f);
        btn1Rt.offsetMin = Vector2.zero;
        btn1Rt.offsetMax = Vector2.zero;
        Image btn1Img = btn1Obj.GetComponent<Image>();
        btn1Img.color = new Color(0.2f, 0.4f, 0.2f);
        Button btn1 = btn1Obj.GetComponent<Button>();
        btn1.targetGraphic = btn1Img;

        Text btn1Txt = new GameObject("Text", typeof(RectTransform), typeof(Text)).GetComponent<Text>();
        btn1Txt.transform.SetParent(btn1Obj.transform, false);
        RectTransform btn1TxtRt = btn1Txt.GetComponent<RectTransform>();
        btn1TxtRt.anchorMin = Vector2.zero;
        btn1TxtRt.anchorMax = Vector2.one;
        btn1TxtRt.offsetMin = Vector2.zero;
        btn1TxtRt.offsetMax = Vector2.zero;
        btn1Txt.font = font;
        btn1Txt.fontSize = 12;
        btn1Txt.fontStyle = FontStyle.Bold;
        btn1Txt.color = Color.white;
        btn1Txt.alignment = TextAnchor.MiddleCenter;
        btn1Txt.text = "Keep IP";

        btn1.onClick.AddListener(() =>
        {
            long refund = (long)(createdGame.costs_entwicklung * 0.5f);
            mS.Earn(refund, 10);
            
            createdGame.publisherID = npc.myID;
            createdGame.pS_ = npc;
            createdGame.ownerID = mS.myID;
            createdGame.ownerS_ = null;
            createdGame.pubAngebot_Gewinnbeteiligung = 10f;

            StudioState state = EnsureState(npc.myID);
            state.LastAAAWeek = GetGlobalWeek(mS);
            gameActiveContracts[createdGame.myID] = "AAACoPublish";
            AdjustRelation(npc, 15f);
            SaveState();
            gui.MessageBox("Signed Co-Publishing Deal! Received 50% funding refund of " + mS.GetMoney(refund, true) + ". You retain the IP.", false);
            
            UnityEngine.Object.Destroy(choiceDialog);
            choiceDialog = null;
        });

        GameObject btn2Obj = new GameObject("BtnGift", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
        btn2Obj.transform.SetParent(choiceDialog.transform, false);
        RectTransform btn2Rt = btn2Obj.GetComponent<RectTransform>();
        btn2Rt.anchorMin = new Vector2(0.52f, 0.1f);
        btn2Rt.anchorMax = new Vector2(0.95f, 0.35f);
        btn2Rt.offsetMin = Vector2.zero;
        btn2Rt.offsetMax = Vector2.zero;
        Image btn2Img = btn2Obj.GetComponent<Image>();
        btn2Img.color = new Color(0.2f, 0.2f, 0.5f);
        Button btn2 = btn2Obj.GetComponent<Button>();
        btn2.targetGraphic = btn2Img;

        Text btn2Txt = new GameObject("Text", typeof(RectTransform), typeof(Text)).GetComponent<Text>();
        btn2Txt.transform.SetParent(btn2Obj.transform, false);
        RectTransform btn2TxtRt = btn2Txt.GetComponent<RectTransform>();
        btn2TxtRt.anchorMin = Vector2.zero;
        btn2TxtRt.anchorMax = Vector2.one;
        btn2TxtRt.offsetMin = Vector2.zero;
        btn2TxtRt.offsetMax = Vector2.zero;
        btn2Txt.font = font;
        btn2Txt.fontSize = 12;
        btn2Txt.fontStyle = FontStyle.Bold;
        btn2Txt.color = Color.white;
        btn2Txt.alignment = TextAnchor.MiddleCenter;
        btn2Txt.text = "Gift IP";

        btn2.onClick.AddListener(() =>
        {
            long refund = createdGame.costs_entwicklung;
            mS.Earn(refund, 10);
            
            createdGame.publisherID = npc.myID;
            createdGame.pS_ = npc;
            createdGame.ownerID = npc.myID;
            createdGame.ownerS_ = npc;
            createdGame.pubAngebot_Gewinnbeteiligung = 20f;

            StudioState state = EnsureState(npc.myID);
            state.LastAAAWeek = GetGlobalWeek(mS);
            gameActiveContracts[createdGame.myID] = "AAACoPublish";
            AdjustRelation(npc, 35f);
            SaveState();
            gui.MessageBox("Signed Co-Publishing Deal! Received 100% funding refund of " + mS.GetMoney(refund, true) + ". IP transferred to " + npc.GetName() + ".", false);

            UnityEngine.Object.Destroy(choiceDialog);
            choiceDialog = null;
        });
    }

    public static void ShowIPCooperationDialog(publisherScript npc, gameScript createdGame)
    {
        mainScript mS = FindObjectOfType<mainScript>();
        GUI_Main gui = FindObjectOfType<GUI_Main>();
        if (gui == null || mS == null) return;

        if (choiceDialog != null) UnityEngine.Object.Destroy(choiceDialog);

        Font font = ResolveFont(gui, gui.uiObjects);

        choiceDialog = new GameObject("SPO_IPCoopChoiceDialog", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        choiceDialog.transform.SetParent(gui.uiObjects[57].transform.parent, false);

        Image bg = choiceDialog.GetComponent<Image>();
        bg.color = new Color(0.1f, 0.1f, 0.12f, 0.98f);

        RectTransform rt = choiceDialog.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = new Vector2(420f, 220f);
        rt.anchoredPosition = Vector2.zero;
        rt.localScale = Vector3.one;

        Text txt = new GameObject("Text", typeof(RectTransform), typeof(Text)).GetComponent<Text>();
        txt.transform.SetParent(choiceDialog.transform, false);
        txt.GetComponent<RectTransform>().anchorMin = new Vector2(0.05f, 0.4f);
        txt.GetComponent<RectTransform>().anchorMax = new Vector2(0.95f, 0.95f);
        txt.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        txt.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        txt.font = font;
        txt.fontSize = 14;
        txt.color = Color.white;
        txt.alignment = TextAnchor.MiddleCenter;
        
        int subdiv = GetSubdivision(npc);
        int maxFunding = subdiv == 3 ? 40 : 80;

        txt.text = "IP Cooperation with " + npc.GetName() + "\n\n" +
                   "Select Funding Split:\n" +
                   "1. Low Funding (Get 20% refund, 25% royalties)\n" +
                   "2. High Funding (Get " + maxFunding + "% refund, 10% royalties)";

        GameObject btn1Obj = new GameObject("BtnLow", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
        btn1Obj.transform.SetParent(choiceDialog.transform, false);
        RectTransform btn1Rt = btn1Obj.GetComponent<RectTransform>();
        btn1Rt.anchorMin = new Vector2(0.05f, 0.1f);
        btn1Rt.anchorMax = new Vector2(0.48f, 0.35f);
        btn1Rt.offsetMin = Vector2.zero;
        btn1Rt.offsetMax = Vector2.zero;
        Image btn1Img = btn1Obj.GetComponent<Image>();
        btn1Img.color = new Color(0.2f, 0.4f, 0.2f);
        Button btn1 = btn1Obj.GetComponent<Button>();
        btn1.targetGraphic = btn1Img;

        Text btn1Txt = new GameObject("Text", typeof(RectTransform), typeof(Text)).GetComponent<Text>();
        btn1Txt.transform.SetParent(btn1Obj.transform, false);
        RectTransform btn1TxtRt = btn1Txt.GetComponent<RectTransform>();
        btn1TxtRt.anchorMin = Vector2.zero;
        btn1TxtRt.anchorMax = Vector2.one;
        btn1TxtRt.offsetMin = Vector2.zero;
        btn1TxtRt.offsetMax = Vector2.zero;
        btn1Txt.font = font;
        btn1Txt.fontSize = 12;
        btn1Txt.fontStyle = FontStyle.Bold;
        btn1Txt.color = Color.white;
        btn1Txt.alignment = TextAnchor.MiddleCenter;
        btn1Txt.text = "20% Funding";

        btn1.onClick.AddListener(() =>
        {
            long refund = (long)(createdGame.costs_entwicklung * 0.2f);
            mS.Earn(refund, 10);
            
            createdGame.publisherID = npc.myID;
            createdGame.pS_ = npc;
            createdGame.ownerID = npc.myID;
            createdGame.ownerS_ = npc;
            createdGame.pubAngebot_Gewinnbeteiligung = 25f;

            StudioState state = EnsureState(npc.myID);
            state.LastCoFinanceWeek = GetGlobalWeek(mS);
            AdjustRelation(npc, subdiv == 3 ? 10f : 12f);
            SaveState();
            gui.MessageBox("IP Cooperation started! Received 20% funding refund of " + mS.GetMoney(refund, true) + ". Royalty set to 25%.", false);

            UnityEngine.Object.Destroy(choiceDialog);
            choiceDialog = null;
        });

        GameObject btn2Obj = new GameObject("BtnHigh", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
        btn2Obj.transform.SetParent(choiceDialog.transform, false);
        RectTransform btn2Rt = btn2Obj.GetComponent<RectTransform>();
        btn2Rt.anchorMin = new Vector2(0.52f, 0.1f);
        btn2Rt.anchorMax = new Vector2(0.95f, 0.35f);
        btn2Rt.offsetMin = Vector2.zero;
        btn2Rt.offsetMax = Vector2.zero;
        Image btn2Img = btn2Obj.GetComponent<Image>();
        btn2Img.color = new Color(0.2f, 0.2f, 0.5f);
        Button btn2 = btn2Obj.GetComponent<Button>();
        btn2.targetGraphic = btn2Img;

        Text btn2Txt = new GameObject("Text", typeof(RectTransform), typeof(Text)).GetComponent<Text>();
        btn2Txt.transform.SetParent(btn2Obj.transform, false);
        RectTransform btn2TxtRt = btn2Txt.GetComponent<RectTransform>();
        btn2TxtRt.anchorMin = Vector2.zero;
        btn2TxtRt.anchorMax = Vector2.one;
        btn2TxtRt.offsetMin = Vector2.zero;
        btn2TxtRt.offsetMax = Vector2.zero;
        btn2Txt.font = font;
        btn2Txt.fontSize = 12;
        btn2Txt.fontStyle = FontStyle.Bold;
        btn2Txt.color = Color.white;
        btn2Txt.alignment = TextAnchor.MiddleCenter;
        btn2Txt.text = maxFunding + "% Funding";

        btn2.onClick.AddListener(() =>
        {
            long refund = (long)(createdGame.costs_entwicklung * (maxFunding / 100f));
            mS.Earn(refund, 10);
            
            createdGame.publisherID = npc.myID;
            createdGame.pS_ = npc;
            createdGame.ownerID = npc.myID;
            createdGame.ownerS_ = npc;
            createdGame.pubAngebot_Gewinnbeteiligung = 10f;

            StudioState state = EnsureState(npc.myID);
            state.LastCoFinanceWeek = GetGlobalWeek(mS);
            AdjustRelation(npc, subdiv == 3 ? 10f : 12f);
            SaveState();
            gui.MessageBox("IP Cooperation started! Received " + maxFunding + "% funding refund of " + mS.GetMoney(refund, true) + ". Royalty set to 10%.", false);

            UnityEngine.Object.Destroy(choiceDialog);
            choiceDialog = null;
        });
    }

    private static void StyleDialogBackground(GameObject dialog, GUI_Main gui)
    {
        if (dialog == null || gui == null) return;
        dialog.transform.SetParent(gui.transform, false);
        dialog.transform.SetAsLastSibling();

        Image bg = dialog.GetComponent<Image>();
        if (bg != null)
        {
            Color pColor = new Color(0.1f, 0.1f, 0.12f, 0.98f);
            Image.Type pType = Image.Type.Simple;
            Sprite pSprite = null;
            
            // Shielded boundary array size validation to prevent layout thread crashes
            if (gui.uiObjects != null && gui.uiObjects.Length > 359 && gui.uiObjects[359] != null) 
                pSprite = FindPanelSprite(gui.uiObjects[359].GetComponent<Menu_Stats_Developer_Main>(), out pColor, out pType);
            if (pSprite == null && gui.uiObjects != null && gui.uiObjects.Length > 373 && gui.uiObjects[373] != null) 
                pSprite = FindPanelSprite(gui.uiObjects[373].GetComponent<Menu_Stats_Publisher_Main>(), out pColor, out pType);
            if (pSprite != null)
            {
                bg.sprite = pSprite;
                bg.type = pType;
                bg.color = new Color(pColor.r, pColor.g, pColor.b, 0.98f);
            }
            else
            {
                bg.color = new Color(0.1f, 0.1f, 0.12f, 0.98f);
            }
            bg.material = null;
        }
    }

    private static void ShowProjectSelectorDialog(publisherScript pS, string titleText, Action<gameScript> onSelected, Action onDesignNew)
    {
        mainScript mS = FindObjectOfType<mainScript>();
        GUI_Main gui = FindObjectOfType<GUI_Main>();
        if (gui == null || mS == null) return;

        if (choiceDialog != null) UnityEngine.Object.Destroy(choiceDialog);

        Font font = ResolveFont(gui, gui.uiObjects);

        choiceDialog = new GameObject("SPO_ProjectSelectorDialog", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        StyleDialogBackground(choiceDialog, gui);

        RectTransform rt = choiceDialog.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        
        games g = FindObjectOfType<games>();
        List<gameScript> playerInDevGames = new List<gameScript>();
        if (g != null && g.arrayGamesScripts != null)
        {
            for (int i = 0; i < g.arrayGamesScripts.Length; i++)
            {
                gameScript game = g.arrayGamesScripts[i];
                if (game != null && game.inDevelopment && game.developerID == mS.myID)
                {
                    playerInDevGames.Add(game);
                }
            }
        }

        int buttonCount = playerInDevGames.Count + 2;
        float width = 450f;
        float height = 80f + (buttonCount * 45f);
        rt.sizeDelta = new Vector2(width, height);
        rt.anchoredPosition = Vector2.zero;
        rt.localScale = Vector3.one;

        Text txt = new GameObject("TitleText", typeof(RectTransform), typeof(Text)).GetComponent<Text>();
        txt.transform.SetParent(choiceDialog.transform, false);
        RectTransform txtRt = txt.GetComponent<RectTransform>();
        txtRt.anchorMin = new Vector2(0.05f, 1f);
        txtRt.anchorMax = new Vector2(0.95f, 1f);
        txtRt.pivot = new Vector2(0.5f, 1f);
        txtRt.sizeDelta = new Vector2(0f, 50f);
        txtRt.anchoredPosition = new Vector2(0f, -10f);
        txt.font = font;
        txt.fontSize = 14;
        txt.fontStyle = FontStyle.Bold;
        txt.color = Color.white;
        txt.alignment = TextAnchor.MiddleCenter;
        txt.text = titleText;

        float currY = -65f;

        for (int i = 0; i < playerInDevGames.Count; i++)
        {
            gameScript game = playerInDevGames[i];
            GameObject btnObj = new GameObject("BtnGame_" + i, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
            btnObj.transform.SetParent(choiceDialog.transform, false);
            RectTransform btnRt = btnObj.GetComponent<RectTransform>();
            btnRt.anchorMin = new Vector2(0.05f, 1f);
            btnRt.anchorMax = new Vector2(0.95f, 1f);
            btnRt.pivot = new Vector2(0.5f, 1f);
            btnRt.sizeDelta = new Vector2(0f, 38f);
            btnRt.anchoredPosition = new Vector2(0f, currY);
            
            Image btnImg = btnObj.GetComponent<Image>();
            btnImg.color = new Color(0.2f, 0.3f, 0.4f);
            Button btn = btnObj.GetComponent<Button>();
            btn.targetGraphic = btnImg;

            Text btnTxt = new GameObject("Text", typeof(RectTransform), typeof(Text)).GetComponent<Text>();
            btnTxt.transform.SetParent(btnObj.transform, false);
            RectTransform btnTxtRt = btnTxt.GetComponent<RectTransform>();
            btnTxtRt.anchorMin = Vector2.zero;
            btnTxtRt.anchorMax = Vector2.one;
            btnTxtRt.offsetMin = Vector2.zero;
            btnTxtRt.offsetMax = Vector2.zero;
            btnTxt.font = font;
            btnTxt.fontSize = 12;
            btnTxt.color = Color.white;
            btnTxt.alignment = TextAnchor.MiddleCenter;
            btnTxt.text = game.GetNameWithTag() + " (Size: " + GetGameSizeString(game.gameSize) + ")";

            btn.onClick.AddListener(() =>
            {
                onSelected(game);
                UnityEngine.Object.Destroy(choiceDialog);
                choiceDialog = null;
            });

            currY -= 45f;
        }

        {
            GameObject btnObj = new GameObject("BtnDesignNew", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
            btnObj.transform.SetParent(choiceDialog.transform, false);
            RectTransform btnRt = btnObj.GetComponent<RectTransform>();
            btnRt.anchorMin = new Vector2(0.05f, 1f);
            btnRt.anchorMax = new Vector2(0.95f, 1f);
            btnRt.pivot = new Vector2(0.5f, 1f);
            btnRt.sizeDelta = new Vector2(0f, 38f);
            btnRt.anchoredPosition = new Vector2(0f, currY);

            Image btnImg = btnObj.GetComponent<Image>();
            btnImg.color = new Color(0.2f, 0.4f, 0.2f);
            Button btn = btnObj.GetComponent<Button>();
            btn.targetGraphic = btnImg;

            Text btnTxt = new GameObject("Text", typeof(RectTransform), typeof(Text)).GetComponent<Text>();
            btnTxt.transform.SetParent(btnObj.transform, false);
            RectTransform btnTxtRt = btnTxt.GetComponent<RectTransform>();
            btnTxtRt.anchorMin = Vector2.zero;
            btnTxtRt.anchorMax = Vector2.one;
            btnTxtRt.offsetMin = Vector2.zero;
            btnTxtRt.offsetMax = Vector2.zero;
            btnTxt.font = font;
            btnTxt.fontSize = 12;
            btnTxt.fontStyle = FontStyle.Bold;
            btnTxt.color = Color.white;
            btnTxt.alignment = TextAnchor.MiddleCenter;
            btnTxt.text = "Design a New Project";

            btn.onClick.AddListener(() =>
            {
                onDesignNew();
                UnityEngine.Object.Destroy(choiceDialog);
                choiceDialog = null;
            });

            currY -= 45f;
        }

        {
            GameObject btnObj = new GameObject("BtnCancel", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
            btnObj.transform.SetParent(choiceDialog.transform, false);
            RectTransform btnRt = btnObj.GetComponent<RectTransform>();
            btnRt.anchorMin = new Vector2(0.05f, 1f);
            btnRt.anchorMax = new Vector2(0.95f, 1f);
            btnRt.pivot = new Vector2(0.5f, 1f);
            btnRt.sizeDelta = new Vector2(0f, 38f);
            btnRt.anchoredPosition = new Vector2(0f, currY);

            Image btnImg = btnObj.GetComponent<Image>();
            btnImg.color = new Color(0.5f, 0.2f, 0.2f);
            Button btn = btnObj.GetComponent<Button>();
            btn.targetGraphic = btnImg;

            Text btnTxt = new GameObject("Text", typeof(RectTransform), typeof(Text)).GetComponent<Text>();
            btnTxt.transform.SetParent(btnObj.transform, false);
            RectTransform btnTxtRt = btnTxt.GetComponent<RectTransform>();
            btnTxtRt.anchorMin = Vector2.zero;
            btnTxtRt.anchorMax = Vector2.one;
            btnTxtRt.offsetMin = Vector2.zero;
            btnTxtRt.offsetMax = Vector2.zero;
            btnTxt.font = font;
            btnTxt.fontSize = 12;
            btnTxt.color = Color.white;
            btnTxt.alignment = TextAnchor.MiddleCenter;
            btnTxt.text = "Cancel";

            btn.onClick.AddListener(() =>
            {
                UnityEngine.Object.Destroy(choiceDialog);
                choiceDialog = null;
            });
        }
    }

    private static void ShowIPSelectorDialog(publisherScript pS, List<gameScript> ips, Action<gameScript> onSelected)
    {
        mainScript mS = FindObjectOfType<mainScript>();
        GUI_Main gui = FindObjectOfType<GUI_Main>();
        if (gui == null || mS == null) return;

        if (choiceDialog != null) UnityEngine.Object.Destroy(choiceDialog);

        Font font = ResolveFont(gui, gui.uiObjects);

        choiceDialog = new GameObject("SPO_IPSelectorDialog", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        StyleDialogBackground(choiceDialog, gui);

        RectTransform rt = choiceDialog.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);

        int itemCount = ips.Count;
        float width = 500f;
        float height = Math.Min(450f, 80f + (itemCount * 50f) + 50f);
        rt.sizeDelta = new Vector2(width, height);
        rt.anchoredPosition = Vector2.zero;
        rt.localScale = Vector3.one;

        Text txt = new GameObject("TitleText", typeof(RectTransform), typeof(Text)).GetComponent<Text>();
        txt.transform.SetParent(choiceDialog.transform, false);
        RectTransform txtRt = txt.GetComponent<RectTransform>();
        txtRt.anchorMin = new Vector2(0.05f, 1f);
        txtRt.anchorMax = new Vector2(0.95f, 1f);
        txtRt.pivot = new Vector2(0.5f, 1f);
        txtRt.sizeDelta = new Vector2(0f, 50f);
        txtRt.anchoredPosition = new Vector2(0f, -10f);
        txt.font = font;
        txt.fontSize = 14;
        txt.fontStyle = FontStyle.Bold;
        txt.color = Color.white;
        txt.alignment = TextAnchor.MiddleCenter;
        txt.text = "Select IP to Purchase from " + pS.GetName() + ":";

        GameObject container = choiceDialog;

        float currY = -60f;
        for (int i = 0; i < ips.Count; i++)
        {
            gameScript game = ips[i];
            long cost = 100000L + (long)(game.ipPunkte * 2000f) + (game.gameSize * 150000L);

            GameObject btnObj = new GameObject("BtnIP_" + i, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
            btnObj.transform.SetParent(container.transform, false);
            RectTransform btnRt = btnObj.GetComponent<RectTransform>();
            btnRt.anchorMin = new Vector2(0.05f, 1f);
            btnRt.anchorMax = new Vector2(0.95f, 1f);
            btnRt.pivot = new Vector2(0.5f, 1f);
            btnRt.sizeDelta = new Vector2(0f, 42f);
            btnRt.anchoredPosition = new Vector2(0f, currY);

            Image btnImg = btnObj.GetComponent<Image>();
            bool canAfford = (mS.money >= cost);
            btnImg.color = canAfford ? new Color(0.2f, 0.35f, 0.25f) : new Color(0.35f, 0.2f, 0.2f);
            Button btn = btnObj.GetComponent<Button>();
            btn.targetGraphic = btnImg;

            Text btnTxt = new GameObject("Text", typeof(RectTransform), typeof(Text)).GetComponent<Text>();
            btnTxt.transform.SetParent(btnObj.transform, false);
            RectTransform btnTxtRt = btnTxt.GetComponent<RectTransform>();
            btnTxtRt.anchorMin = Vector2.zero;
            btnTxtRt.anchorMax = Vector2.one;
            btnTxtRt.offsetMin = Vector2.zero;
            btnTxtRt.offsetMax = Vector2.zero;
            btnTxt.font = font;
            btnTxt.fontSize = 12;
            btnTxt.color = Color.white;
            btnTxt.alignment = TextAnchor.MiddleCenter;
            btnTxt.text = "<b>" + game.myName + "</b> (Popularity: " + game.ipPunkte.ToString("F0") + ") - Cost: " + mS.GetMoney(cost, true);

            btn.onClick.AddListener(() =>
            {
                if (mS.money < cost)
                {
                    gui.ShowNoMoney();
                    return;
                }
                mS.Pay(cost, 10);
                onSelected(game);
                UnityEngine.Object.Destroy(choiceDialog);
                choiceDialog = null;
            });

            currY -= 48f;
            if (currY < -370f && ips.Count > 7)
            {
                break;
            }
        }

        {
            GameObject btnObj = new GameObject("BtnCancel", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
            btnObj.transform.SetParent(choiceDialog.transform, false);
            RectTransform btnRt = btnObj.GetComponent<RectTransform>();
            btnRt.anchorMin = new Vector2(0.05f, 0f);
            btnRt.anchorMax = new Vector2(0.95f, 0f);
            btnRt.pivot = new Vector2(0.5f, 0f);
            btnRt.sizeDelta = new Vector2(0f, 38f);
            btnRt.anchoredPosition = new Vector2(0f, 10f);

            Image btnImg = btnObj.GetComponent<Image>();
            btnImg.color = new Color(0.5f, 0.2f, 0.2f);
            Button btn = btnObj.GetComponent<Button>();
            btn.targetGraphic = btnImg;

            Text btnTxt = new GameObject("Text", typeof(RectTransform), typeof(Text)).GetComponent<Text>();
            btnTxt.transform.SetParent(btnObj.transform, false);
            RectTransform btnTxtRt = btnTxt.GetComponent<RectTransform>();
            btnTxtRt.anchorMin = Vector2.zero;
            btnTxtRt.anchorMax = Vector2.one;
            btnTxtRt.offsetMin = Vector2.zero;
            btnTxtRt.offsetMax = Vector2.zero;
            btnTxt.font = font;
            btnTxt.fontSize = 12;
            btnTxt.color = Color.white;
            btnTxt.alignment = TextAnchor.MiddleCenter;
            btnTxt.text = "Cancel";

            btn.onClick.AddListener(() =>
            {
                UnityEngine.Object.Destroy(choiceDialog);
                choiceDialog = null;
            });
        }
    }

    private static void ShowExclusivityOrSubChoiceDialog(publisherScript pS, gameScript inDev, int cdYears, float relGain)
    {
        mainScript mS = FindObjectOfType<mainScript>();
        GUI_Main gui = FindObjectOfType<GUI_Main>();
        if (gui == null || mS == null) return;

        if (choiceDialog != null) UnityEngine.Object.Destroy(choiceDialog);

        Font font = ResolveFont(gui, gui.uiObjects);

        choiceDialog = new GameObject("SPO_ExclusivityOrSubChoiceDialog", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        StyleDialogBackground(choiceDialog, gui);

        RectTransform rt = choiceDialog.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = new Vector2(460f, 250f);
        rt.anchoredPosition = Vector2.zero;
        rt.localScale = Vector3.one;

        long exclCost = 200000L * (inDev.gameSize + 1);
        long subCost = 50000L * (inDev.gameSize + 1);

        Text txt = new GameObject("Text", typeof(RectTransform), typeof(Text)).GetComponent<Text>();
        txt.transform.SetParent(choiceDialog.transform, false);
        RectTransform txtRt = txt.GetComponent<RectTransform>();
        txtRt.anchorMin = new Vector2(0.05f, 0.35f);
        txtRt.anchorMax = new Vector2(0.95f, 0.95f);
        txtRt.offsetMin = Vector2.zero;
        txtRt.offsetMax = Vector2.zero;
        txt.font = font;
        txt.fontSize = 13;
        txt.color = Color.white;
        txt.alignment = TextAnchor.MiddleCenter;

        txt.text = "Choose contract type for " + pS.GetName() + "'s project <b>" + inDev.GetNameWithTag() + "</b>:\n\n" +
                   "1. Console Exclusivity (Cost: " + mS.GetMoney(exclCost, true) + ")\n" +
                   "   Locks the game to your console platform.\n" +
                   "2. Subscription Day-One (Cost: " + mS.GetMoney(subCost, true) + ")\n" +
                   "   Places the game on your subscription service on launch.";

        GameObject btn1Obj = new GameObject("BtnExcl", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
        btn1Obj.transform.SetParent(choiceDialog.transform, false);
        RectTransform btn1Rt = btn1Obj.GetComponent<RectTransform>();
        btn1Rt.anchorMin = new Vector2(0.05f, 0.1f);
        btn1Rt.anchorMax = new Vector2(0.48f, 0.3f);
        btn1Rt.offsetMin = Vector2.zero;
        btn1Rt.offsetMax = Vector2.zero;
        Image btn1Img = btn1Obj.GetComponent<Image>();
        
        bool hasConsole = PlayerHasActiveConsole(mS);
        bool canAffordExcl = (mS.money >= exclCost && hasConsole);
        btn1Img.color = canAffordExcl ? new Color(0.2f, 0.4f, 0.2f) : new Color(0.35f, 0.2f, 0.2f);
        
        Button btn1 = btn1Obj.GetComponent<Button>();
        btn1.targetGraphic = btn1Img;

        Text btn1Txt = new GameObject("Text", typeof(RectTransform), typeof(Text)).GetComponent<Text>();
        btn1Txt.transform.SetParent(btn1Obj.transform, false);
        RectTransform btn1TxtRt = btn1Txt.GetComponent<RectTransform>();
        btn1TxtRt.anchorMin = Vector2.zero;
        btn1TxtRt.anchorMax = Vector2.one;
        btn1TxtRt.offsetMin = Vector2.zero;
        btn1TxtRt.offsetMax = Vector2.zero;
        btn1Txt.font = font;
        btn1Txt.fontSize = 11;
        btn1Txt.fontStyle = FontStyle.Bold;
        btn1Txt.color = Color.white;
        btn1Txt.alignment = TextAnchor.MiddleCenter;
        btn1Txt.text = hasConsole ? "Console Exclusivity" : "No Console";

        btn1.onClick.AddListener(() =>
        {
            if (!hasConsole)
            {
                gui.MessageBox("You do not have an active console on the market!", false);
                return;
            }
            if (mS.money < exclCost)
            {
                gui.ShowNoMoney();
                return;
            }
            
            mS.Pay(exclCost, 10);
            
            int platId = GetPlayerPlatformID(mS);
            inDev.exklusiv = true;
            inDev.gamePlatform[0] = platId;
            for (int i = 1; i < 4; i++) inDev.gamePlatform[i] = -1;

            StudioState state = EnsureState(pS.myID);
            state.LastExclusiveWeek = GetGlobalWeek(mS);
            AdjustRelation(pS, relGain);
            SaveState();
            
            gui.MessageBox("Game '" + inDev.myName + "' is now exclusive to your console!", false);
            UnityEngine.Object.Destroy(choiceDialog);
            choiceDialog = null;
        });

        GameObject btn2Obj = new GameObject("BtnSub", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
        btn2Obj.transform.SetParent(choiceDialog.transform, false);
        RectTransform btn2Rt = btn2Obj.GetComponent<RectTransform>();
        btn2Rt.anchorMin = new Vector2(0.52f, 0.1f);
        btn2Rt.anchorMax = new Vector2(0.95f, 0.3f);
        btn2Rt.offsetMin = Vector2.zero;
        btn2Rt.offsetMax = Vector2.zero;
        Image btn2Img = btn2Obj.GetComponent<Image>();
        
        bool hasSub = PlayerHasActiveSubscription(mS);
        bool canAffordSub = (mS.money >= subCost && hasSub);
        btn2Img.color = canAffordSub ? new Color(0.2f, 0.2f, 0.5f) : new Color(0.35f, 0.2f, 0.2f);
        
        Button btn2 = btn2Obj.GetComponent<Button>();
        btn2.targetGraphic = btn2Img;

        Text btn2Txt = new GameObject("Text", typeof(RectTransform), typeof(Text)).GetComponent<Text>();
        btn2Txt.transform.SetParent(btn2Obj.transform, false);
        RectTransform btn2TxtRt = btn2Txt.GetComponent<RectTransform>();
        btn2TxtRt.anchorMin = Vector2.zero;
        btn2TxtRt.anchorMax = Vector2.one;
        btn2TxtRt.offsetMin = Vector2.zero;
        btn2TxtRt.offsetMax = Vector2.zero;
        btn2Txt.font = font;
        btn2Txt.fontSize = 11;
        btn2Txt.fontStyle = FontStyle.Bold;
        btn2Txt.color = Color.white;
        btn2Txt.alignment = TextAnchor.MiddleCenter;
        btn2Txt.text = hasSub ? "Subscription" : "No Sub Service";

        btn2.onClick.AddListener(() =>
        {
            if (!hasSub)
            {
                gui.MessageBox("You do not have an active subscription service!", false);
                return;
            }
            if (mS.money < subCost)
            {
                gui.ShowNoMoney();
                return;
            }
            
            mS.Pay(subCost, 10);
            
            mS.gpS_.GAMEPASS_AddGame(inDev, true);

            StudioState state = EnsureState(pS.myID);
            state.LastExclusiveWeek = GetGlobalWeek(mS);
            AdjustRelation(pS, relGain);
            SaveState();
            
            gui.MessageBox("Game '" + inDev.myName + "' will list on your subscription service on launch!", false);
            UnityEngine.Object.Destroy(choiceDialog);
            choiceDialog = null;
        });
    }

    private static void ShowOutsourceIPGiftDialog(publisherScript pS, gameScript createdGame, Action<bool> onChosen)
    {
        mainScript mS = FindObjectOfType<mainScript>();
        GUI_Main gui = FindObjectOfType<GUI_Main>();
        if (gui == null || mS == null) return;

        if (choiceDialog != null) UnityEngine.Object.Destroy(choiceDialog);

        Font font = ResolveFont(gui, gui.uiObjects);

        choiceDialog = new GameObject("SPO_OutsourceIPGiftDialog", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        StyleDialogBackground(choiceDialog, gui);

        RectTransform rt = choiceDialog.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = new Vector2(420f, 220f);
        rt.anchoredPosition = Vector2.zero;
        rt.localScale = Vector3.one;

        int subdiv = GetSubdivision(pS);
        float giftPoints = (subdiv == 4) ? 35f : 40f;

        Text txt = new GameObject("Text", typeof(RectTransform), typeof(Text)).GetComponent<Text>();
        txt.transform.SetParent(choiceDialog.transform, false);
        RectTransform txtRt = txt.GetComponent<RectTransform>();
        txtRt.anchorMin = new Vector2(0.05f, 0.35f);
        txtRt.anchorMax = new Vector2(0.95f, 0.95f);
        txtRt.offsetMin = Vector2.zero;
        txtRt.offsetMax = Vector2.zero;
        txt.font = font;
        txt.fontSize = 13;
        txt.color = Color.white;
        txt.alignment = TextAnchor.MiddleCenter;
        txt.text = "Outsourcing brand-new IP <b>" + createdGame.myName + "</b>:\n\n" +
                   "Do you want to keep the IP rights, or gift them to " + pS.GetName() + "?\n" +
                   "1. Keep IP (You own the IP, get +15 Relationship on completion)\n" +
                   "2. Gift IP (They own the IP, get +" + giftPoints + " Relationship instantly)";

        GameObject btn1Obj = new GameObject("BtnKeep", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
        btn1Obj.transform.SetParent(choiceDialog.transform, false);
        RectTransform btn1Rt = btn1Obj.GetComponent<RectTransform>();
        btn1Rt.anchorMin = new Vector2(0.05f, 0.1f);
        btn1Rt.anchorMax = new Vector2(0.48f, 0.3f);
        btn1Rt.offsetMin = Vector2.zero;
        btn1Rt.offsetMax = Vector2.zero;
        Image btn1Img = btn1Obj.GetComponent<Image>();
        btn1Img.color = new Color(0.2f, 0.3f, 0.4f);
        Button btn1 = btn1Obj.GetComponent<Button>();
        btn1.targetGraphic = btn1Img;

        Text btn1Txt = new GameObject("Text", typeof(RectTransform), typeof(Text)).GetComponent<Text>();
        btn1Txt.transform.SetParent(btn1Obj.transform, false);
        RectTransform btn1TxtRt = btn1Txt.GetComponent<RectTransform>();
        btn1TxtRt.anchorMin = Vector2.zero;
        btn1TxtRt.anchorMax = Vector2.one;
        btn1TxtRt.offsetMin = Vector2.zero;
        btn1TxtRt.offsetMax = Vector2.zero;
        btn1Txt.font = font;
        btn1Txt.fontSize = 12;
        btn1Txt.fontStyle = FontStyle.Bold;
        btn1Txt.color = Color.white;
        btn1Txt.alignment = TextAnchor.MiddleCenter;
        btn1Txt.text = "Keep IP";

        btn1.onClick.AddListener(() =>
        {
            onChosen(false);
            UnityEngine.Object.Destroy(choiceDialog);
            choiceDialog = null;
        });

        GameObject btn2Obj = new GameObject("BtnGift", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
        btn2Obj.transform.SetParent(choiceDialog.transform, false);
        RectTransform btn2Rt = btn2Obj.GetComponent<RectTransform>();
        btn2Rt.anchorMin = new Vector2(0.52f, 0.1f);
        btn2Rt.anchorMax = new Vector2(0.95f, 0.3f);
        btn2Rt.offsetMin = Vector2.zero;
        btn2Rt.offsetMax = Vector2.zero;
        Image btn2Img = btn2Obj.GetComponent<Image>();
        btn2Img.color = new Color(0.2f, 0.4f, 0.2f);
        Button btn2 = btn2Obj.GetComponent<Button>();
        btn2.targetGraphic = btn2Img;

        Text btn2Txt = new GameObject("Text", typeof(RectTransform), typeof(Text)).GetComponent<Text>();
        btn2Txt.transform.SetParent(btn2Obj.transform, false);
        RectTransform btn2TxtRt = btn2Txt.GetComponent<RectTransform>();
        btn2TxtRt.anchorMin = Vector2.zero;
        btn2TxtRt.anchorMax = Vector2.one;
        btn2TxtRt.offsetMin = Vector2.zero;
        btn2TxtRt.offsetMax = Vector2.zero;
        btn2Txt.font = font;
        btn2Txt.fontSize = 12;
        btn2Txt.fontStyle = FontStyle.Bold;
        btn2Txt.color = Color.white;
        btn2Txt.alignment = TextAnchor.MiddleCenter;
        btn2Txt.text = "Gift IP";

        btn2.onClick.AddListener(() =>
        {
            onChosen(true);
            UnityEngine.Object.Destroy(choiceDialog);
            choiceDialog = null;
        });
    }

    private static void PromptAndProcessOutsourceIP(publisherScript npc, gameScript createdGame, StartSnapshot snapshot, bool isBusy)
    {
        mainScript main = FindObjectOfType<mainScript>();
        GUI_Main gui = FindObjectOfType<GUI_Main>();
        
        bool isNewIP = !createdGame.typ_nachfolger && !createdGame.typ_spinoff && !createdGame.typ_remaster;
        
        if (isNewIP)
        {
            ShowOutsourceIPGiftDialog(npc, createdGame, (giftIP) =>
            {
                if (giftIP)
                {
                    createdGame.ownerID = npc.myID;
                    createdGame.ownerS_ = npc;
                    
                    int subdiv = GetSubdivision(npc);
                    float instantGain = (subdiv == 4) ? 35f : 40f;
                    AdjustRelation(npc, instantGain);
                    
                    gui.MessageBox("You gifted the IP rights for '" + createdGame.myName + "' to " + npc.GetName() + "! Relationship increased (+" + instantGain + ").", false);
                }
                else
                {
                    createdGame.ownerID = main.myID;
                    createdGame.ownerS_ = null;
                }
                
                FinalizeOutsourceRouting(npc, createdGame, snapshot, isBusy, giftIP);
            });
        }
        else
        {
            FinalizeOutsourceRouting(npc, createdGame, snapshot, isBusy, false);
        }
    }

    private static void FinalizeOutsourceRouting(publisherScript npc, gameScript createdGame, StartSnapshot snapshot, bool isBusy, bool giftIP)
    {
        mainScript main = FindObjectOfType<mainScript>();
        GUI_Main gui = FindObjectOfType<GUI_Main>();
        
        activeOutsourceIsNewIP = giftIP ? 1 : 0;

        if (isBusy)
        {
            ShowOutsourceChoiceDialog(npc, createdGame, snapshot);
        }
        else
        {
            long normalCost = CalculateOutsourceCost(npc, createdGame);
            main.Pay(normalCost, 10);
            StartOutsourceProjectImmediately(npc, createdGame);
            gui.MessageBox("Assigned outsourcing project to " + npc.GetName() + ". They are starting development now.", false);
            CancelDesignContext();
        }
    }

    public static void ConvertCreatedGameToOutsourceProject()
    {
        if (activeOutsourceNpcId == -1 || pendingStartSnapshot == null)
        {
            return;
        }

        StartSnapshot snapshot = pendingStartSnapshot;
        pendingStartSnapshot = null;

        games games = FindObjectOfType<games>();
        mainScript main = FindObjectOfType<mainScript>();
        GUI_Main gui = FindObjectOfType<GUI_Main>();
        publisherScript npc = FindStudioByID(main, activeOutsourceNpcId);

        if (games == null || main == null || npc == null)
        {
            CancelDesignContext();
            return;
        }

        games.FindGames();
        gameScript createdGame = FindNewlyCreatedPlayerGame(games, main, snapshot.KnownGameIds);
        if (createdGame == null)
        {
            CancelDesignContext();
            return;
        }

        DestroyCreatedTaskForGame(createdGame, snapshot);
        RestoreBorrowedRoom(snapshot);
        RefundPlayerDevelopmentCost(main, createdGame);

        int subdiv = GetSubdivision(npc);

        if (subdiv == 1)
        {
            if (activeOutsourceIsNewIP == 0)
            {
                createdGame.exklusiv = true;
                createdGame.publisherID = npc.myID;
                createdGame.pS_ = npc;
                createdGame.ownerID = main.myID;
                createdGame.ownerS_ = null;

                int targetPlatId = -1;
                for (int i = 0; i < main.arrayPlatformsScripts.Length; i++)
                {
                    platformScript plat = main.arrayPlatformsScripts[i];
                    if (plat != null && plat.ownerID == npc.myID && plat.isUnlocked && !plat.vomMarktGenommen)
                    {
                        targetPlatId = plat.myID;
                        break;
                    }
                }
                createdGame.gamePlatform[0] = targetPlatId;
                for (int i = 1; i < 4; i++) createdGame.gamePlatform[i] = -1;

                main.Earn(createdGame.costs_entwicklung, 10);
                StudioState state = EnsureState(npc.myID);
                state.LastExclusiveWeek = GetGlobalWeek(main);
                AdjustRelation(npc, 15f);
                SaveState();

                gui.MessageBox($"Secured Exclusive Platform Funding with {npc.GetName()}! Refunded 100% of dev cost ({main.GetMoney(createdGame.costs_entwicklung, true)}). All other platforms cancelled.", false);
                CancelDesignContext();
                return;
            }
            if (activeOutsourceIsNewIP == 2)
            {
                createdGame.publisherID = npc.myID;
                createdGame.pS_ = npc;
                createdGame.pubAngebot_Gewinnbeteiligung = 25f;

                StudioState state = EnsureState(npc.myID);
                state.LastCoFinanceWeek = GetGlobalWeek(main);
                AdjustRelation(npc, 10f);
                SaveState();
                gui.MessageBox($"Signed Secured Marketing Deal with {npc.GetName()}! They will publish the game with 25% royalties.", false);
                CancelDesignContext();
                return;
            }
        }

        if (subdiv == 2 || subdiv == 3)
        {
            if (activeOutsourceIsNewIP == 4 && subdiv == 2)
            {
                ShowCoPublishChoiceDialog(npc, createdGame);
                CancelDesignContext();
                return;
            }
            if (activeOutsourceIsNewIP == 2)
            {
                ShowIPCooperationDialog(npc, createdGame);
                CancelDesignContext();
                return;
            }
            if (activeOutsourceIsNewIP == 1)
            {
                long bonus;
                float royalty;
                float relationGain;
                string messageName;
                if (subdiv == 2)
                {
                    bonus = 100000L;
                    if (createdGame.gameSize == 1) bonus = 200000L;
                    else if (createdGame.gameSize == 2) bonus = 400000L;
                    else if (createdGame.gameSize == 3) bonus = 800000L;
                    else if (createdGame.gameSize == 4) bonus = 1500000L;
                    else if (createdGame.gameSize == 5) bonus = 3000000L;
                    royalty = 10f;
                    relationGain = 8f;
                    messageName = "Premium Contract";
                }
                else
                {
                    bonus = 50000L * (createdGame.gameSize + 1);
                    royalty = 20f;
                    relationGain = 5f;
                    messageName = "Indie Sponsorship";
                }

                main.Earn(bonus, 10);
                createdGame.publisherID = npc.myID;
                createdGame.pS_ = npc;
                createdGame.pubAngebot_Gewinnbeteiligung = royalty;

                StudioState state = EnsureState(npc.myID);
                state.LastCoFinanceWeek = GetGlobalWeek(main);
                AdjustRelation(npc, relationGain);
                SaveState();
                gui.MessageBox($"Signed {messageName} with {npc.GetName()}! Received signing bonus of {main.GetMoney(bonus, true)}.", false);
                CancelDesignContext();
                return;
            }
        }

        if (subdiv == 4 && activeOutsourceIsNewIP == 2)
        {
            long refund = (long)(createdGame.costs_entwicklung * 0.25f);
            main.Earn(refund, 10);
            createdGame.ownerID = npc.myID;
            createdGame.ownerS_ = npc;

            bool hasPubRoom = UnityEngine.Object.FindObjectsOfType<roomScript>().Any(r => r != null && r.typ == 14);
            if (hasPubRoom)
            {
                createdGame.publisherID = main.myID;
                createdGame.pS_ = null;
            }
            else
            {
                createdGame.publisherID = npc.myID;
                createdGame.pS_ = npc;
                createdGame.pubAngebot_Gewinnbeteiligung = 15f;
            }

            AdjustRelation(npc, 10f);
            gui.MessageBox($"Signed IP Licensing Partnership! Received 25% funding refund ({main.GetMoney(refund, true)}).", false);
            CancelDesignContext();
            return;
        }

        PromptAndProcessOutsourceIP(npc, createdGame, snapshot, npc.FindGameInDevelopment() != null);
    }

    private static void StartOutsourceProjectImmediately(publisherScript npc, gameScript game)
    {
        mainScript mS = FindObjectOfType<mainScript>();
        if (activeOutsourceIsNewIP == 1) // Gifted IP
        {
            game.ownerID = npc.myID;
            game.ownerS_ = npc;
        }
        else if (activeOutsourceIsNewIP == 2) // NPC IP (safeguard)
        {
            game.ownerID = npc.myID;
            game.ownerS_ = npc;
        }
        else // Kept IP / Player IP (activeOutsourceIsNewIP == 0)
        {
            game.ownerID = mS.myID;
            game.ownerS_ = null;
        }

        game.developerID = npc.myID;
        game.devS_ = npc;
        game.publisherID = mS.myID;
        game.pS_ = null;
        game.inDevelopment = true;
        game.schublade = false;

        game.SetAsGameInDevelopmentNPC();
        npc.SetNewGameInWeeks(0);

        StudioState npcState = EnsureState(npc.myID);
        npcState.OutsourcedGameIds.Add(game.myID);
        SaveState();
    }

    public static void TriggerQueuedProject(publisherScript npc, StudioState state)
    {
        if (!state.HasQueuedProject) return;
        state.HasQueuedProject = false;
        SaveState();

        games games = FindObjectOfType<games>();
        if (games == null) return;

        gameScript game = games.CreateNewGame(false, true);
        game.myName = state.QueuedName;
        game.maingenre = state.QueuedGenre;
        game.subgenre = state.QueuedSubGenre;
        game.gameMainTheme = state.QueuedTheme;
        game.gameSubTheme = state.QueuedSubTheme;
        game.gameSize = state.QueuedSize;
        game.engineID = state.QueuedEngineId;

        for (int i = 0; i < 4; i++)
        {
            game.gamePlatform[i] = state.QueuedPlatforms[i];
        }
        for (int i = 0; i < Math.Min(50, game.gameGameplayFeatures.Length); i++)
        {
            game.gameGameplayFeatures[i] = state.QueuedFeatures[i];
        }

        activeOutsourceIsNewIP = state.QueuedLetKeepIP ? 1 : 0;
        StartOutsourceProjectImmediately(npc, game);
    }

    private static void QueueOutsourceProject(StudioState state, gameScript game)
    {
        state.HasQueuedProject = true;
        state.QueuedName = game.myName;
        state.QueuedGenre = game.maingenre;
        state.QueuedSubGenre = game.subgenre;
        state.QueuedTheme = game.gameMainTheme;
        state.QueuedSubTheme = game.gameSubTheme;
        state.QueuedSize = game.gameSize;
        state.QueuedEngineId = game.engineID;

        for (int i = 0; i < 4; i++)
        {
            state.QueuedPlatforms[i] = game.gamePlatform[i];
        }
        for (int i = 0; i < Math.Min(50, game.gameGameplayFeatures.Length); i++)
        {
            if (i < game.gameGameplayFeatures.Length)
            {
                state.QueuedFeatures[i] = game.gameGameplayFeatures[i];
            }
            else
            {
                state.QueuedFeatures[i] = false;
            }
        }
        state.QueuedLetKeepIP = (activeOutsourceIsNewIP == 1);
        SaveState();
    }

    private static gameScript FindNewlyCreatedPlayerGame(games games, mainScript main, HashSet<int> knownGameIds)
    {
        if (games == null || games.arrayGamesScripts == null) return null;
        for (int i = 0; i < games.arrayGamesScripts.Length; i++)
        {
            gameScript game = games.arrayGamesScripts[i];
            if (game != null && !knownGameIds.Contains(game.myID))
            {
                return game;
            }
        }
        return null;
    }

    private static void DestroyCreatedTaskForGame(gameScript game, StartSnapshot snapshot)
    {
        taskGame[] tasks = UnityEngine.Object.FindObjectsOfType<taskGame>();
        for (int i = 0; i < tasks.Length; i++)
        {
            if (tasks[i] != null && tasks[i].gS_ == game)
            {
                tasks[i].gameObject.tag = "GameRemoved";
                UnityEngine.Object.Destroy(tasks[i].gameObject);
                break;
            }
        }
    }

    private static void RestoreBorrowedRoom(StartSnapshot snapshot)
    {
        if (snapshot.Room != null)
        {
            snapshot.Room.taskID = snapshot.OriginalTaskId;
            snapshot.Room.taskGameObject = snapshot.OriginalTaskObject;
        }
    }

    private static void RefundPlayerDevelopmentCost(mainScript main, gameScript game)
    {
        if (main == null || game == null || game.costs_entwicklung <= 0) return;
        main.Pay(-game.costs_entwicklung, 10); // Refund using the game's native method
    }

    public class StartSnapshot
    {
        public HashSet<int> KnownGameIds;
        public roomScript Room;
        public int OriginalTaskId;
        public GameObject OriginalTaskObject;
    }

    // =========================================================
    //  HARMONY PATCHES FOR STATS DETAIL SCREENS (FIXED & SHIELDED)
    // =========================================================

    [HarmonyPatch(typeof(Menu_Stats_Developer_Main), "Init")]
    public static class MenuStatsDeveloperInitPatch
    {
        public static bool Prefix(Menu_Stats_Developer_Main __instance, publisherScript script_)
        {
            try
            {
                SafeInitDeveloperDetail(__instance, script_);
            }
            catch (Exception ex)
            {
                log?.LogError("Safe Developer detail Init failed: " + ex);
            }
            return false;
        }

        public static void Postfix(Menu_Stats_Developer_Main __instance, publisherScript script_)
        {
            try
            {
                if (__instance != null && __instance.uiObjects != null)
                {
                    InjectSidePanel(__instance, script_, __instance.uiObjects);
                }
            }
            catch (Exception ex)
            {
                log.LogWarning("Side panel injection failed on Developer stats menu: " + ex);
            }
        }
    }

    [HarmonyPatch(typeof(Menu_Stats_Developer_Main), "BUTTON_Abbrechen")]
    public static class MenuStatsDeveloperClosePatch
    {
        public static void Postfix()
        {
            ClearSidePanel();
        }
    }

    [HarmonyPatch(typeof(Menu_Stats_Publisher_Main), "Init")]
    public static class MenuStatsPublisherInitPatch
    {
        public static bool Prefix(Menu_Stats_Publisher_Main __instance, publisherScript script_)
        {
            try
            {
                SafeInitPublisherDetail(__instance, script_);
            }
            catch (Exception ex)
            {
                log?.LogError("Safe Publisher detail Init failed: " + ex);
            }
            return false;
        }

        public static void Postfix(Menu_Stats_Publisher_Main __instance, publisherScript script_)
        {
            try
            {
                if (__instance != null && __instance.uiObjects != null)
                {
                    InjectSidePanel(__instance, script_, __instance.uiObjects);
                }
            }
            catch (Exception ex)
            {
                log.LogWarning("Side panel injection failed on Publisher stats menu: " + ex);
            }
        }
    }

    [HarmonyPatch(typeof(Menu_Stats_Publisher_Main), "BUTTON_Abbrechen")]
    public static class MenuStatsPublisherClosePatch
    {
        public static void Postfix()
        {
            ClearSidePanel();
        }
    }

    [HarmonyPatch(typeof(Menu_Statistics_DevPubsMain), "BUTTON_Publisher")]
    public static class MenuStatisticsDevPubsMainPublisherButtonPatch
    {
        public static bool Prefix()
        {
            return SafeOpenStatisticsList("Publisher", 119);
        }
    }

    [HarmonyPatch(typeof(Menu_Statistics_DevPubsMain), "BUTTON_Entwickler")]
    public static class MenuStatisticsDevPubsMainDeveloperButtonPatch
    {
        public static bool Prefix()
        {
            return SafeOpenStatisticsList("Developer", 120);
        }
    }

    private static bool SafeOpenStatisticsList(string label, int uiIndex)
    {
        try
        {

            GUI_Main gui = GetGuiMainCached();
            if (gui == null || gui.uiObjects == null || gui.uiObjects.Length <= uiIndex || gui.uiObjects[uiIndex] == null)
            {
                log?.LogError($"Cannot open {label} statistics list: GUI_Main/uiObjects[{uiIndex}] is missing.");
                return false;
            }

            sfxScript sfx = UnityEngine.Object.FindObjectOfType<sfxScript>();
            if (sfx != null)
            {
                sfx.PlaySound(3, force: true);
            }

            gui.ActivateMenu(gui.uiObjects[uiIndex]);
            return false;
        }
        catch (Exception ex)
        {
            log?.LogError($"Error opening {label} statistics list: {ex}");
            return false;
        }
    }

    private static GameObject GetUiObject(GameObject[] uiObjects, int index)
    {
        if (uiObjects == null || index < 0 || uiObjects.Length <= index)
        {
            return null;
        }

        GameObject obj = uiObjects[index];
        return obj != null && obj ? obj : null;
    }

    private static void SetUiText(GameObject[] uiObjects, int index, string value)
    {
        GameObject obj = GetUiObject(uiObjects, index);
        if (obj == null) return;

        Text text = obj.GetComponent<Text>();
        if (text != null)
        {
            text.text = value ?? "";
        }
    }

    private static void SetUiImage(GameObject[] uiObjects, int index, Sprite sprite)
    {
        GameObject obj = GetUiObject(uiObjects, index);
        if (obj == null) return;

        Image image = obj.GetComponent<Image>();
        if (image != null)
        {
            image.sprite = sprite;
        }
    }

    private static void SetUiActive(GameObject[] uiObjects, int index, bool active)
    {
        GameObject obj = GetUiObject(uiObjects, index);
        if (obj != null && obj.activeSelf != active)
        {
            obj.SetActive(active);
        }
    }

    private static tooltip GetRowTooltip(tooltip current, Component owner)
    {
        if (current != null && current)
        {
            return current;
        }

        return owner != null ? owner.GetComponent<tooltip>() : null;
    }

    private static string SafeStudioName(publisherScript studio)
    {
        if (studio == null || !studio) return "Unknown Studio";

        try
        {
            string name = studio.GetName();
            if (!string.IsNullOrEmpty(name))
            {
                return name;
            }
        }
        catch { }

        if (!string.IsNullOrEmpty(studio.name_EN))
        {
            return studio.name_EN;
        }

        return "Studio " + studio.myID.ToString(CultureInfo.InvariantCulture);
    }

    private static Sprite SafeStudioLogo(publisherScript studio)
    {
        if (studio == null || !studio) return null;

        try
        {
            return studio.GetLogo();
        }
        catch { return null; }
    }

    private static float SafeStudioRelation(publisherScript studio)
    {
        if (studio == null || !studio) return 0f;

        try
        {
            return studio.GetRelation();
        }
        catch { return Mathf.Clamp(studio.relation, 0f, 100f); }
    }

    private static string SafeStudioFirmValueString(publisherScript studio, mainScript main)
    {
        return SafeMoneyString(main, EstimateStudioFirmValue(studio));
    }

    private static string SafeStudioTypeString(publisherScript studio)
    {
        if (studio == null || !studio) return "";

        try
        {
            string value = studio.GetDeveloperPublisherString();
            if (value != null)
            {
                return value;
            }
        }
        catch { }

        if (studio.publisher && studio.developer) return "Publisher & Developer";
        if (studio.publisher) return "Publisher";
        if (studio.developer) return "Developer";
        return "";
    }

    private static string SafeStudioTooltip(publisherScript studio)
    {
        if (studio != null && studio)
        {
            try
            {
                string tooltipText = studio.GetTooltip();
                if (tooltipText != null)
                {
                    return tooltipText;
                }
            }
            catch { }
        }

        return "<b><size=18>" + SafeStudioName(studio) + "</size></b>";
    }

    private static bool SafeKaufangebotFree(publisherScript studio)
    {
        if (studio == null || !studio) return false;
        try { return studio.KaufangebotFree(); }
        catch { return false; }
    }

    private static bool SafeIsTochterfirma(publisherScript studio)
    {
        if (studio == null || !studio) return false;
        try { return studio.IsTochterfirma(); }
        catch { return false; }
    }

    private static bool SafeIsMyTochterfirma(publisherScript studio)
    {
        if (studio == null || !studio) return false;
        try { return studio.IsMyTochterfirma(); }
        catch { return false; }
    }

    private static bool SafeGeschlossen(publisherScript studio)
    {
        if (studio == null || !studio) return false;
        try { return studio.Geschlossen(); }
        catch { return false; }
    }

    private static int SafeStudioAmountGames(publisherScript studio)
    {
        if (studio == null || !studio) return 0;
        try { return Math.Max(0, studio.GetAmountGames()); }
        catch { return 0; }
    }

    private static int SafeStudioAmountPublishedGames(publisherScript studio)
    {
        if (studio == null || !studio) return 0;
        try { return Math.Max(0, studio.GetAmountVertriebeneSpiele()); }
        catch { return 0; }
    }

    private static string SafeMoneyString(mainScript main, long amount)
    {
        if (main != null && main)
        {
            try
            {
                return main.GetMoney(amount, showDollar: true);
            }
            catch
            {
                // Fall through to invariant fallback.
            }
        }

        return "$" + amount.ToString(CultureInfo.InvariantCulture);
    }

    private static string SafeShareString(mainScript main, float share)
    {
        if (main != null && main)
        {
            try
            {
                return "$" + main.Round(share, 1);
            }
            catch
            {
                // Fall through to invariant fallback.
            }
        }

        return "$" + share.ToString("0.#", CultureInfo.InvariantCulture);
    }

    private static string SafeText(textScript text, int id, string fallback)
    {
        if (text != null && text)
        {
            try
            {
                string value = text.GetText(id);
                if (!string.IsNullOrEmpty(value))
                {
                    return value;
                }
            }
            catch
            {
                // Fall through to fallback.
            }
        }

        return fallback;
    }

    private static textScript GetTextScriptCached(mainScript main)
    {
        if (main == null || !main) return null;

        try
        {
            return main.GetComponent<textScript>();
        }
        catch
        {
            return null;
        }
    }

    private static sfxScript GetSfxScriptCached()
    {
        try
        {
            return UnityEngine.Object.FindObjectOfType<sfxScript>();
        }
        catch
        {
            return null;
        }
    }

    private static genres GetGenresScriptCached(mainScript main)
    {
        if (main == null || !main) return null;

        try
        {
            return main.GetComponent<genres>();
        }
        catch
        {
            return null;
        }
    }

    private static bool SafeStatisticsMenuUpdate(GameObject[] uiObjects)
    {
        return false;
    }

    private static bool SafePopulateDropdown(GameObject[] uiObjects, textScript text, int[] textIds, string[] fallbacks)
    {
        GameObject dropdownObject = GetUiObject(uiObjects, 5);
        if (dropdownObject == null) return false;

        Dropdown dropdown = dropdownObject.GetComponent<Dropdown>();
        if (dropdown == null) return false;

        int value = PlayerPrefs.GetInt(dropdownObject.name);
        List<string> options = new List<string>(textIds.Length);
        for (int i = 0; i < textIds.Length; i++)
        {
            string fallback = fallbacks != null && fallbacks.Length > i ? fallbacks[i] : "";
            options.Add(SafeText(text, textIds[i], fallback));
        }

        dropdown.ClearOptions();
        dropdown.AddOptions(options);
        dropdown.value = Mathf.Clamp(value, 0, Math.Max(0, options.Count - 1));
        return false;
    }

    private static Sprite SafeGenreSprite(genres genres, int genreId)
    {
        if (genres == null || !genres) return null;

        try
        {
            return genres.GetPic(genreId);
        }
        catch
        {
            return null;
        }
    }

    private static void TryDrawStars(GUI_Main gui, GameObject target, int stars)
    {
        if (gui == null || !gui || target == null || !target) return;

        try
        {
            gui.DrawStars(target, Mathf.Clamp(stars, 0, 5));
        }
        catch { }
    }

    private static void TryDrawRelationStars(GUI_Main gui, GameObject target, int stars)
    {
        if (gui == null || !gui || target == null || !target) return;

        try
        {
            Color color = Color.green;
            if (gui.colors != null && gui.colors.Length > 5)
            {
                color = gui.colors[5];
            }
            gui.DrawStarsColor(target, Mathf.Clamp(stars, 0, 5), color);
        }
        catch { }
    }

    private static void EnsurePublisherRowDependencies(Item_Stats_Publisher item)
    {
        mainScript main = item.mS_;
        if (main == null || !main)
        {
            main = GetMainScriptCached();
            item.mS_ = main;
        }

        if (item.guiMain_ == null || !item.guiMain_)
        {
            item.guiMain_ = GetGuiMainCached();
        }

        if ((item.tS_ == null || !item.tS_) && main != null && main)
        {
            item.tS_ = main.GetComponent<textScript>();
        }

        if ((item.genres_ == null || !item.genres_) && main != null && main)
        {
            item.genres_ = main.GetComponent<genres>();
        }

        item.tooltip_ = GetRowTooltip(item.tooltip_, item);
    }

    private static void EnsureDeveloperRowDependencies(Item_Stats_Developer item)
    {
        mainScript main = item.mS_;
        if (main == null || !main)
        {
            main = GetMainScriptCached();
            item.mS_ = main;
        }

        if (item.guiMain_ == null || !item.guiMain_)
        {
            item.guiMain_ = GetGuiMainCached();
        }

        if ((item.tS_ == null || !item.tS_) && main != null && main)
        {
            item.tS_ = main.GetComponent<textScript>();
        }

        if ((item.genres_ == null || !item.genres_) && main != null && main)
        {
            item.genres_ = main.GetComponent<genres>();
        }

        item.tooltip_ = GetRowTooltip(item.tooltip_, item);
    }

    private static void DisableRowButton(Component item)
    {
        if (item == null) return;

        Button button = item.GetComponent<Button>();
        if (button != null)
        {
            button.interactable = false;
        }
    }

    private static void SafeSetPublisherStatsRow(Item_Stats_Publisher item)
    {
        if (item == null || !item) return;

        EnsurePublisherRowDependencies(item);
        GameObject[] ui = item.uiObjects;
        publisherScript studio = item.pS_;
        if (studio == null || !studio)
        {
            SetUiText(ui, 0, "Unknown Studio");
            DisableRowButton(item);
            return;
        }

        mainScript main = item.mS_;
        GUI_Main gui = item.guiMain_;
        SetUiText(ui, 0, SafeStudioName(studio));
        SetUiImage(ui, 1, SafeStudioLogo(studio));
        SetUiImage(ui, 2, SafeGenreSprite(item.genres_, studio.fanGenre));
        SetUiText(ui, 5, SafeShareString(main, studio.share));
        SetUiText(ui, 11, SafeStudioFirmValueString(studio, main));

        TryDrawStars(gui, GetUiObject(ui, 3), Mathf.RoundToInt(studio.stars / 20f));
        TryDrawRelationStars(gui, GetUiObject(ui, 4), Mathf.RoundToInt(SafeStudioRelation(studio) / 20f));

        tooltip tooltip = GetRowTooltip(item.tooltip_, item);
        if (tooltip != null)
        {
            tooltip.c = SafeStudioTooltip(studio);
            item.tooltip_ = tooltip;
        }

        bool isTochterfirma = SafeIsTochterfirma(studio);
        bool isPlayer = studio.isPlayer;
        SetUiActive(ui, 10, SafeKaufangebotFree(studio));
        SetUiActive(ui, 8, isTochterfirma);
        SetUiActive(ui, 9, isPlayer);
        SetUiActive(ui, 7, !isPlayer && !isTochterfirma);
        SetUiActive(ui, 6, SafeGeschlossen(studio));
    }

    private static void SafeSetDeveloperStatsRow(Item_Stats_Developer item)
    {
        if (item == null || !item) return;

        EnsureDeveloperRowDependencies(item);
        GameObject[] ui = item.uiObjects;
        publisherScript studio = item.pS_;
        mainScript main = item.mS_;
        GUI_Main gui = item.guiMain_;

        if (studio != null && studio)
        {
            SetUiText(ui, 0, SafeStudioName(studio));
            SetUiImage(ui, 1, SafeStudioLogo(studio));
            TryDrawStars(gui, GetUiObject(ui, 3), Mathf.RoundToInt(studio.stars / 20f));
            SetUiText(ui, 2, SafeStudioAmountGames(studio).ToString(CultureInfo.InvariantCulture));
            SetUiText(ui, 4, SafeStudioFirmValueString(studio, main));
            SetUiText(ui, 7, SafeStudioTypeString(studio));

            tooltip tooltip = GetRowTooltip(item.tooltip_, item);
            if (tooltip != null)
            {
                tooltip.c = SafeStudioTooltip(studio);
                item.tooltip_ = tooltip;
            }

            bool isTochterfirma = SafeIsTochterfirma(studio);
            bool isPlayer = studio.isPlayer;
            SetUiActive(ui, 11, SafeKaufangebotFree(studio));
            SetUiActive(ui, 6, isTochterfirma);
            SetUiActive(ui, 9, isPlayer);
            SetUiActive(ui, 10, !isPlayer && !isTochterfirma);
            SetUiActive(ui, 8, SafeGeschlossen(studio));
            return;
        }

        if (main != null && main && main.multiplayer && item.playerID != -1)
        {
            try
            {
                item.gameObject.transform.SetAsFirstSibling();
                DisableRowButton(item);
            }
            catch
            {
                // Row ordering is cosmetic; keep the menu alive.
            }

            string companyName = "Player " + item.playerID.ToString(CultureInfo.InvariantCulture);
            Sprite logo = null;
            long money = 0L;
            if (main.mpCalls_ != null)
            {
                try { companyName = main.mpCalls_.GetCompanyName(item.playerID); }
                catch { }
                try { logo = gui != null ? gui.GetCompanyLogo(main.mpCalls_.GetLogo(item.playerID)) : null; }
                catch { }
                try { money = main.mpCalls_.GetMoney(item.playerID); }
                catch { }
            }

            SetUiText(ui, 0, companyName);
            SetUiImage(ui, 1, logo);
            TryDrawStars(gui, GetUiObject(ui, 3), 5);
            SetUiText(ui, 2, "---");
            SetUiText(ui, 4, SafeMoneyString(main, money));
            SetUiText(ui, 7, SafeText(item.tS_, 432, "Publisher") + " & " + SafeText(item.tS_, 274, "Developer"));

            tooltip tooltip = GetRowTooltip(item.tooltip_, item);
            if (tooltip != null)
            {
                tooltip.c = "";
                item.tooltip_ = tooltip;
            }
            return;
        }

        SetUiText(ui, 0, "Unknown Studio");
        DisableRowButton(item);
    }

    private static bool SafePublisherStatsRowUpdate(Item_Stats_Publisher item)
    {
        if (item == null || !item) return false;
        if (item.mS_ == null || !item.mS_)
        {
            item.mS_ = GetMainScriptCached();
        }

        if (item.mS_ == null || !item.mS_ || !item.mS_.multiplayer)
        {
            return false;
        }

        return true;
    }

    private static bool SafeDeveloperStatsRowUpdate(Item_Stats_Developer item)
    {
        if (item == null || !item) return false;
        if (item.mS_ == null || !item.mS_)
        {
            item.mS_ = GetMainScriptCached();
        }

        if (item.mS_ == null || !item.mS_ || !item.mS_.multiplayer)
        {
            return false;
        }

        return true;
    }

    private static bool SafeOpenPublisherDetail(Item_Stats_Publisher item)
    {
        try
        {
            if (item == null || !item || item.pS_ == null || !item.pS_) return false;
            // FIX: If the selected studio is a player subsidiary, bypass this mod's screen and let vanilla handle it
            if (item.pS_.IsTochterfirma()) return true;

            if (item.guiMain_ == null || !item.guiMain_)
            {
                item.guiMain_ = GetGuiMainCached();
            }

            if (item.guiMain_ == null || item.guiMain_.uiObjects == null || item.guiMain_.uiObjects.Length <= 373 || item.guiMain_.uiObjects[373] == null)
            {
                log?.LogError("Cannot open Publisher detail: uiObjects[373] is missing.");
                return false;
            }

            if (item.sfx_ != null && item.sfx_)
            {
                item.sfx_.PlaySound(3, force: true);
            }

            item.guiMain_.ActivateMenu(item.guiMain_.uiObjects[373]);
            Menu_Stats_Publisher_Main detail = item.guiMain_.uiObjects[373].GetComponent<Menu_Stats_Publisher_Main>();
            if (detail != null)
            {
                detail.Init(item.pS_);
            }
        }
        catch (Exception ex)
        {
            log?.LogError("Safe Publisher row click failed: " + ex);
        }

        return false;
    }

    private static bool SafeOpenDeveloperDetail(Item_Stats_Developer item)
    {
        try
        {
            if (item == null || !item || item.pS_ == null || !item.pS_) return false;
            // FIX: If the selected studio is a player subsidiary, bypass this mod's screen and let vanilla handle it
            if (item.pS_.IsTochterfirma()) return true;

            if (item.guiMain_ == null || !item.guiMain_)
            {
                item.guiMain_ = GetGuiMainCached();
            }

            if (item.guiMain_ == null || item.guiMain_.uiObjects == null || item.guiMain_.uiObjects.Length <= 359 || item.guiMain_.uiObjects[359] == null)
            {
                log?.LogError("Cannot open Developer detail: uiObjects[359] is missing.");
                return false;
            }

            if (item.sfx_ != null && item.sfx_)
            {
                item.sfx_.PlaySound(3, force: true);
            }

            item.guiMain_.ActivateMenu(item.guiMain_.uiObjects[359]);
            Menu_Stats_Developer_Main detail = item.guiMain_.uiObjects[359].GetComponent<Menu_Stats_Developer_Main>();
            if (detail != null)
            {
                detail.Init(item.pS_);
            }
        }
        catch (Exception ex)
        {
            log?.LogError("Safe Developer row click failed: " + ex);
        }

        return false;
    }

    private static void SetButtonInteractable(GameObject[] uiObjects, int index, bool interactable)
    {
        GameObject obj = GetUiObject(uiObjects, index);
        if (obj == null) return;

        Button button = obj.GetComponent<Button>();
        if (button != null)
        {
            button.interactable = interactable;
        }
    }

    private static void SetTooltipText(GameObject[] uiObjects, int index, string value)
    {
        GameObject obj = GetUiObject(uiObjects, index);
        if (obj == null) return;

        tooltip tip = obj.GetComponent<tooltip>();
        if (tip != null)
        {
            tip.c = value ?? "";
        }
    }

    private static string SafeStudioDateString(publisherScript studio)
    {
        if (studio == null || !studio) return "";
        try { return studio.GetDateString(); }
        catch { return ""; }
    }

    private static string SafeGenreName(genres genres, int genreId)
    {
        if (genres == null || !genres) return "";
        try { return genres.GetName(genreId); }
        catch { return ""; }
    }

    private static gameScript SafeFindAnnouncedGame(publisherScript studio)
    {
        if (studio == null || !studio) return null;
        try { return studio.FindAngekuendigtesGame(); }
        catch { return null; }
    }

    private static gameScript SafeFindInDevelopmentGame(publisherScript studio)
    {
        if (studio == null || !studio) return null;
        try { return studio.FindGameInDevelopment(); }
        catch { return null; }
    }

    private static void AssignDetailCommon(object menu, GameObject[] uiObjects, publisherScript studio, bool useAnnouncedGame)
    {
        mainScript main = GetMainScriptCached();
        GUI_Main gui = GetGuiMainCached();
        textScript text = GetTextScriptCached(main);
        sfxScript sfx = GetSfxScriptCached();
        genres genreScript = GetGenresScriptCached(main);

        Traverse tr = Traverse.Create(menu);
        tr.Field("mS_").SetValue(main);
        tr.Field("tS_").SetValue(text);
        tr.Field("guiMain_").SetValue(gui);
        tr.Field("sfx_").SetValue(sfx);
        tr.Field("genres_").SetValue(genreScript);
        tr.Field("pS_").SetValue(studio);

        try
        {
            tr.Field("nextGame_").SetValue(useAnnouncedGame ? SafeFindAnnouncedGame(studio) : SafeFindInDevelopmentGame(studio));
        }
        catch
        {
            // Some detail menus do not have nextGame_.
        }
    }

    private static void SafeInitDeveloperDetail(Menu_Stats_Developer_Main menu, publisherScript studio)
    {
        if (menu == null || !menu || studio == null || !studio) return;

        AssignDetailCommon(menu, menu.uiObjects, studio, useAnnouncedGame: true);
        mainScript main = GetMainScriptCached();
        GUI_Main gui = GetGuiMainCached();
        textScript text = GetTextScriptCached(main);
        genres genreScript = GetGenresScriptCached(main);
        gameScript nextGame = SafeFindAnnouncedGame(studio);

        SetUiText(menu.uiObjects, 0, SafeStudioName(studio));
        SetUiImage(menu.uiObjects, 1, SafeStudioLogo(studio));
        TryDrawRelationStars(gui, GetUiObject(menu.uiObjects, 2), Mathf.RoundToInt(studio.stars / 20f));
        if (gui != null && gui.flagSprites != null && studio.country >= 0 && studio.country < gui.flagSprites.Length)
        {
            SetUiImage(menu.uiObjects, 9, gui.flagSprites[studio.country]);
        }
        SetUiText(menu.uiObjects, 4, SafeText(text, 685, "Goodwill") + ": <b>" + SafeStudioFirmValueString(studio, main) + "</b>");
        SetUiImage(menu.uiObjects, 7, SafeGenreSprite(genreScript, studio.fanGenre));
        SetTooltipText(menu.uiObjects, 7, SafeText(text, 437, "Genre") + ": <b>" + SafeGenreName(genreScript, studio.fanGenre) + "</b>");
        SetUiText(menu.uiObjects, 3, SafeStudioDateString(studio) + (SafeGeschlossen(studio) ? "\n<color=red><b>" + SafeText(text, 1969, "Closed") + "</b></color>" : ""));

        if (nextGame != null && !SafeGeschlossen(studio))
        {
            try { SetUiText(menu.uiObjects, 10, nextGame.GetNameWithTag()); } catch { SetUiText(menu.uiObjects, 10, ""); }
            try { SetUiText(menu.uiObjects, 11, nextGame.GetGenreString()); } catch { SetUiText(menu.uiObjects, 11, ""); }
        }
        else
        {
            SetUiText(menu.uiObjects, 10, "<color=grey>" + SafeText(text, 2057, "No announced game") + "</color>");
            SetUiText(menu.uiObjects, 11, "");
        }

        bool isTochter = SafeIsTochterfirma(studio);
        SetUiActive(menu.uiObjects, 8, isTochter);
        SetUiActive(menu.uiObjects, 6, SafeGeschlossen(studio));
        bool buyable = !isTochter && !studio.notForSale && SafeKaufangebotFree(studio);
        SetButtonInteractable(menu.uiObjects, 5, buyable);
        SetUiActive(menu.uiObjects, 12, !isTochter && !studio.notForSale && !buyable);
    }

    private static void SafeInitPublisherDetail(Menu_Stats_Publisher_Main menu, publisherScript studio)
    {
        if (menu == null || !menu || studio == null || !studio) return;

        AssignDetailCommon(menu, menu.uiObjects, studio, useAnnouncedGame: false);
        mainScript main = GetMainScriptCached();
        GUI_Main gui = GetGuiMainCached();
        textScript text = GetTextScriptCached(main);
        genres genreScript = GetGenresScriptCached(main);

        SetUiText(menu.uiObjects, 0, SafeStudioName(studio));
        SetUiImage(menu.uiObjects, 1, SafeStudioLogo(studio));
        if (gui != null && gui.flagSprites != null && studio.country >= 0 && studio.country < gui.flagSprites.Length)
        {
            SetUiImage(menu.uiObjects, 10, gui.flagSprites[studio.country]);
        }
        TryDrawRelationStars(gui, GetUiObject(menu.uiObjects, 2), Mathf.RoundToInt(studio.stars / 20f));
        TryDrawRelationStars(gui, GetUiObject(menu.uiObjects, 3), Mathf.RoundToInt(SafeStudioRelation(studio) / 20f));
        SetUiText(menu.uiObjects, 4, SafeText(text, 436, "Share") + ": <b>" + (studio.isPlayer ? "---" : SafeShareString(main, studio.share)) + "</b>");
        SetUiText(menu.uiObjects, 5, SafeStudioDateString(studio) + (SafeGeschlossen(studio) ? "\n<color=red><b>" + SafeText(text, 1969, "Closed") + "</b></color>" : ""));
        SetUiImage(menu.uiObjects, 6, SafeGenreSprite(genreScript, studio.fanGenre));
        SetTooltipText(menu.uiObjects, 6, SafeText(text, 437, "Genre") + ": <b>" + SafeGenreName(genreScript, studio.fanGenre) + "</b>");
        SetUiText(menu.uiObjects, 7, SafeText(text, 685, "Goodwill") + ": <b>" + SafeStudioFirmValueString(studio, main) + "</b>");

        bool isTochter = SafeIsTochterfirma(studio);
        SetUiActive(menu.uiObjects, 11, isTochter);
        SetUiActive(menu.uiObjects, 9, SafeGeschlossen(studio));
        bool buyable = !isTochter && !studio.notForSale && SafeKaufangebotFree(studio);
        SetButtonInteractable(menu.uiObjects, 8, buyable);
        SetUiActive(menu.uiObjects, 12, !isTochter && !studio.notForSale && !buyable);
    }

    private static bool SafeDeveloperDetailUpdate(Menu_Stats_Developer_Main menu)
    {
        if (menu == null || !menu) return false;

        try
        {
            publisherScript studio = Traverse.Create(menu).Field("pS_").GetValue<publisherScript>();
            if (studio == null || !studio) return false;

            bool disabled = SafeIsTochterfirma(studio) || studio.notForSale;
            if (disabled)
            {
                SetButtonInteractable(menu.uiObjects, 5, false);
            }
        }
        catch (Exception ex)
        {
            log?.LogWarning("Safe Developer detail Update skipped: " + ex.GetType().Name);
        }

        return false;
    }

    private static bool SafePublisherDetailUpdate(Menu_Stats_Publisher_Main menu)
    {
        if (menu == null || !menu) return false;

        try
        {
            publisherScript studio = Traverse.Create(menu).Field("pS_").GetValue<publisherScript>();
            if (studio == null || !studio) return false;

            bool disabled = SafeIsTochterfirma(studio) || studio.notForSale;
            if (disabled)
            {
                SetButtonInteractable(menu.uiObjects, 8, false);
            }
        }
        catch (Exception ex)
        {
            log?.LogWarning("Safe Publisher detail Update skipped: " + ex.GetType().Name);
        }

        return false;
    }

    [HarmonyPatch(typeof(Item_Stats_Publisher), "SetData")]
    public static class ItemStatsPublisherSetDataPatch
    {
        public static bool Prefix(Item_Stats_Publisher __instance)
        {
            try
            {
                SafeSetPublisherStatsRow(__instance);
            }
            catch (Exception ex)
            {
                log?.LogError("Safe Publisher statistics row render failed: " + ex);
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(Item_Stats_Developer), "SetData")]
    public static class ItemStatsDeveloperSetDataPatch
    {
        public static bool Prefix(Item_Stats_Developer __instance)
        {
            try
            {
                SafeSetDeveloperStatsRow(__instance);
            }
            catch (Exception ex)
            {
                log?.LogError("Safe Developer statistics row render failed: " + ex);
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(Item_Stats_Publisher), "Update")]
    public static class ItemStatsPublisherUpdatePatch
    {
        public static bool Prefix(Item_Stats_Publisher __instance)
        {
            return SafePublisherStatsRowUpdate(__instance);
        }
    }

    [HarmonyPatch(typeof(Item_Stats_Developer), "Update")]
    public static class ItemStatsDeveloperUpdatePatch
    {
        public static bool Prefix(Item_Stats_Developer __instance)
        {
            return SafeDeveloperStatsRowUpdate(__instance);
        }
    }

    [HarmonyPatch(typeof(Item_Stats_Publisher), "BUTTON_Click")]
    public static class ItemStatsPublisherButtonClickPatch
    {
        public static bool Prefix(Item_Stats_Publisher __instance)
        {
            return SafeOpenPublisherDetail(__instance);
        }
    }

    [HarmonyPatch(typeof(Item_Stats_Developer), "BUTTON_Click")]
    public static class ItemStatsDeveloperButtonClickPatch
    {
        public static bool Prefix(Item_Stats_Developer __instance)
        {
            return SafeOpenDeveloperDetail(__instance);
        }
    }

    [HarmonyPatch(typeof(Menu_Stats_Developer_Main), "Update")]
    public static class MenuStatsDeveloperMainSafeUpdatePatch
    {
        public static bool Prefix(Menu_Stats_Developer_Main __instance)
        {
            return SafeDeveloperDetailUpdate(__instance);
        }
    }

    [HarmonyPatch(typeof(Menu_Stats_Publisher_Main), "Update")]
    public static class MenuStatsPublisherMainSafeUpdatePatch
    {
        public static bool Prefix(Menu_Stats_Publisher_Main __instance)
        {
            return SafePublisherDetailUpdate(__instance);
        }
    }

    [HarmonyPatch(typeof(Menu_Stats_Developer_Main), "Init")]
    public static class MenuStatsDeveloperMainInitShieldPatch
    {
        public static Exception Finalizer(Menu_Stats_Developer_Main __instance, publisherScript script_, Exception __exception)
        {
            if (__exception == null) return null;
            log?.LogError("Shielded Developer detail Init for studio " + (script_ != null ? script_.myID.ToString(CultureInfo.InvariantCulture) : "-1") + ": " + __exception);
            return null;
        }
    }

    [HarmonyPatch(typeof(Menu_Stats_Publisher_Main), "Init")]
    public static class MenuStatsPublisherMainInitShieldPatch
    {
        public static Exception Finalizer(Menu_Stats_Publisher_Main __instance, publisherScript script_, Exception __exception)
        {
            if (__exception == null) return null;
            log?.LogError("Shielded Publisher detail Init for studio " + (script_ != null ? script_.myID.ToString(CultureInfo.InvariantCulture) : "-1") + ": " + __exception);
            return null;
        }
    }

    [HarmonyPatch(typeof(Menu_Statistics_Publisher), "Update")]
    public static class MenuStatisticsPublisherUpdatePatch
    {
        public static bool Prefix(Menu_Statistics_Publisher __instance)
        {
            return SafeStatisticsMenuUpdate(__instance != null ? __instance.uiObjects : null);
        }
    }

    [HarmonyPatch(typeof(Menu_Statistics_Developer), "Update")]
    public static class MenuStatisticsDeveloperUpdatePatch
    {
        public static bool Prefix(Menu_Statistics_Developer __instance)
        {
            return SafeStatisticsMenuUpdate(__instance != null ? __instance.uiObjects : null);
        }
    }

    [HarmonyPatch(typeof(Menu_Statistics_Publisher), "InitDropdowns")]
    public static class MenuStatisticsPublisherInitDropdownsPatch
    {
        public static bool Prefix(Menu_Statistics_Publisher __instance)
        {
            try
            {
                mainScript main = GetMainScriptCached();
                textScript text = GetTextScriptCached(main);
                int[] ids = { 355, 434, 435, 436, 437, 1745, 685, 1923, 1969 };
                string[] fallbacks = { "Name", "Stars", "Relationship", "Share", "Genre", "Released Games", "Company Value", "Subsidiary", "Closed" };
                return SafePopulateDropdown(__instance != null ? __instance.uiObjects : null, text, ids, fallbacks);
            }
            catch (Exception ex)
            {
                log?.LogError("Safe Publisher InitDropdowns failed: " + ex);
                return false;
            }
        }
    }

    [HarmonyPatch(typeof(Menu_Statistics_Developer), "InitDropdowns")]
    public static class MenuStatisticsDeveloperInitDropdownsPatch
    {
        public static bool Prefix(Menu_Statistics_Developer __instance)
        {
            try
            {
                mainScript main = GetMainScriptCached();
                textScript text = GetTextScriptCached(main);
                int[] ids = { 355, 687, 685, 271, 1923, 1969 };
                string[] fallbacks = { "Name", "Stars", "Company Value", "Games", "Subsidiary", "Closed" };
                return SafePopulateDropdown(__instance != null ? __instance.uiObjects : null, text, ids, fallbacks);
            }
            catch (Exception ex)
            {
                log?.LogError("Safe Developer InitDropdowns failed: " + ex);
                return false;
            }
        }
    }

    [HarmonyPatch(typeof(Menu_Statistics_Publisher), "Init")]
    public static class MenuStatisticsPublisherInitPatch
    {
        public static bool Prefix(Menu_Statistics_Publisher __instance)
        {
            return MenuStatisticsPublisherSetDataPatch.Prefix(__instance);
        }
    }

    [HarmonyPatch(typeof(Menu_Statistics_Developer), "Init")]
    public static class MenuStatisticsDeveloperInitPatch
    {
        public static bool Prefix(Menu_Statistics_Developer __instance)
        {
            return MenuStatisticsDeveloperSetDataPatch.Prefix(__instance);
        }
    }

    // Keep the vanilla sort behavior, but avoid one broken studio record crashing the whole list.
    [HarmonyPatch(typeof(Menu_Statistics_Publisher), "DROPDOWN_Sort")]
    public static class MenuStatisticsPublisherDropdownSortPatch
    {
        [HarmonyPrefix]
        public static bool Prefix(Menu_Statistics_Publisher __instance)
        {
            try
            {
                if (__instance == null || !__instance) return true;
                if (__instance.uiObjects == null || __instance.uiObjects.Length <= 5) return false; 

                Dropdown dropdown = __instance.uiObjects[5]?.GetComponent<Dropdown>();
                if (dropdown == null) return false;

                int value = dropdown.value;
                PlayerPrefs.SetInt(__instance.uiObjects[5].name, value);

                GameObject contentPanel = __instance.uiObjects[0];
                if (contentPanel == null) return false;

                int childCount = contentPanel.transform.childCount;
                for (int i = 0; i < childCount; i++)
                {
                    Transform child = contentPanel.transform.GetChild(i);
                    if (child == null || child.gameObject == null || !child.gameObject.activeSelf) continue;

                    Item_Stats_Publisher component = child.GetComponent<Item_Stats_Publisher>();
                    if (component == null || component.pS_ == null || !component.pS_)
                    {
                        continue;
                    }

                    publisherScript pS = component.pS_;
                    switch (value)
                    {
                        case 0:
                            child.gameObject.name = SafeStudioName(pS);
                            break;
                        case 1:
                            child.gameObject.name = pS.stars.ToString();
                            break;
                        case 2:
                            child.gameObject.name = SafeStudioRelation(pS).ToString(CultureInfo.InvariantCulture);
                            break;
                        case 3:
                            child.gameObject.name = pS.share.ToString(CultureInfo.InvariantCulture);
                            break;
                        case 4:
                            child.gameObject.name = pS.fanGenre.ToString();
                            break;
                        case 5:
                            child.gameObject.name = SafeStudioAmountPublishedGames(pS).ToString(CultureInfo.InvariantCulture);
                            break;
                        case 6:
                            child.gameObject.name = SafeGetStudioFirmValueForList(pS).ToString(CultureInfo.InvariantCulture);
                            break;
                        case 7:
                            child.gameObject.name = SafeIsMyTochterfirma(pS) ? "1" : "0";
                            break;
                        case 8:
                            child.gameObject.name = SafeGeschlossen(pS) ? "1" : "0";
                            break;
                    }
                }

                mainScript mS = Traverse.Create(__instance).Field("mS_").GetValue<mainScript>();
                if (mS == null || !mS)
                {
                    mS = GetMainScriptCached();
                }
                if (mS != null)
                {
                    if (value == 0)
                    {
                        mS.SortChildrenByName(contentPanel);
                    }
                    else
                    {
                        mS.SortChildrenByFloat(contentPanel);
                    }
                }

                return false; 
            }
            catch (Exception ex)
            {
                if (log != null) log.LogError("Error in Menu_Statistics_Publisher.DROPDOWN_Sort Prefix: " + ex);
                return false; 
            }
        }
    }

    // Keep the vanilla sort behavior, but avoid one broken studio record crashing the whole list.
    [HarmonyPatch(typeof(Menu_Statistics_Developer), "DROPDOWN_Sort")]
    public static class MenuStatisticsDeveloperDropdownSortPatch
    {
        [HarmonyPrefix]
        public static bool Prefix(Menu_Statistics_Developer __instance)
        {
            try
            {
                if (__instance == null || !__instance) return true;
                if (__instance.uiObjects == null || __instance.uiObjects.Length <= 5) return false; 

                Dropdown dropdown = __instance.uiObjects[5]?.GetComponent<Dropdown>();
                if (dropdown == null) return false;

                int value = dropdown.value;
                PlayerPrefs.SetInt(__instance.uiObjects[5].name, value);

                GameObject contentPanel = __instance.uiObjects[0];
                if (contentPanel == null) return false;

                int childCount = contentPanel.transform.childCount;
                for (int i = 0; i < childCount; i++)
                {
                    Transform child = contentPanel.transform.GetChild(i);
                    if (child == null || child.gameObject == null || !child.gameObject.activeSelf) continue;

                    Item_Stats_Developer component = child.GetComponent<Item_Stats_Developer>();
                    if (component == null || component.pS_ == null || !component.pS_)
                    {
                        continue;
                    }

                    publisherScript pS = component.pS_;
                    switch (value)
                    {
                        case 0:
                            child.gameObject.name = SafeStudioName(pS);
                            break;
                        case 1:
                            child.gameObject.name = pS.stars.ToString(CultureInfo.InvariantCulture);
                            break;
                        case 2:
                            child.gameObject.name = SafeGetStudioFirmValueForList(pS).ToString(CultureInfo.InvariantCulture);
                            break;
                        case 3:
                            child.gameObject.name = SafeStudioAmountGames(pS).ToString(CultureInfo.InvariantCulture);
                            break;
                        case 4:
                            child.gameObject.name = SafeIsMyTochterfirma(pS) ? "1" : "0";
                            break;
                        case 5:
                            child.gameObject.name = SafeGeschlossen(pS) ? "1" : "0";
                            break;
                    }
                }

                mainScript mS = Traverse.Create(__instance).Field("mS_").GetValue<mainScript>();
                if (mS == null || !mS)
                {
                    mS = GetMainScriptCached();
                }
                if (mS != null)
                {
                    if (value == 0)
                    {
                        mS.SortChildrenByName(contentPanel);
                    }
                    else
                    {
                        mS.SortChildrenByFloat(contentPanel);
                    }
                }

                return false; 
            }
            catch (Exception ex)
            {
                if (log != null) log.LogError("Error in Menu_Statistics_Developer.DROPDOWN_Sort Prefix: " + ex);
                return false; 
            }
        }
    }

    // Build the vanilla Publisher list defensively so a bad tagged object cannot abort menu opening.
    [HarmonyPatch(typeof(Menu_Statistics_Publisher), "SetData")]
    public static class MenuStatisticsPublisherSetDataPatch
    {
        [HarmonyPrefix]
        public static bool Prefix(Menu_Statistics_Publisher __instance)
        {
            try
            {
                if (__instance == null || !__instance) return true;
                if (__instance.uiObjects == null || __instance.uiObjects.Length <= 7) return false;
                if (__instance.uiPrefabs == null || __instance.uiPrefabs.Length == 0 || __instance.uiPrefabs[0] == null) return false;

                mainScript mS = GetMainScriptCached();
                textScript tS = mS != null && mS ? mS.GetComponent<textScript>() : null;
                sfxScript sfx = UnityEngine.Object.FindObjectOfType<sfxScript>();
                GUI_Main guiMain = GetGuiMainCached();
                genres genres = mS != null ? mS.GetComponent<genres>() : null;

                if (mS == null || !mS) return false;

                // Wire them to __instance so vanilla code and other patches can access them
                Traverse.Create(__instance).Field("mS_").SetValue(mS);
                Traverse.Create(__instance).Field("tS_").SetValue(tS);
                Traverse.Create(__instance).Field("sfx_").SetValue(sfx);
                Traverse.Create(__instance).Field("guiMain_").SetValue(guiMain);
                Traverse.Create(__instance).Field("genres_").SetValue(genres);

                // Deactivate existing children individually (triggers OnDisable -> Object.Destroy on each).
                // Must NOT use SetActive(false) on parent or DestroyImmediate — both cause crash via
                // OnDisable feedback loop (Item_Stats_Publisher.OnDisable calls Object.Destroy on itself).
                GameObject contentPanel = GetUiObject(__instance.uiObjects, 0);
                if (contentPanel == null) return false;

                if (contentPanel != null)
                {
                    for (int c = 0; c < contentPanel.transform.childCount; c++)
                    {
                        Transform child = contentPanel.transform.GetChild(c);
                        if (child != null && child.gameObject != null && child.gameObject.activeSelf)
                        {
                            child.gameObject.SetActive(false);
                        }
                    }
                }

                int rowsBuilt = 0;
                GameObject[] array = GameObject.FindGameObjectsWithTag("Publisher");
                for (int i = 0; i < array.Length; i++)
                {
                    if (!array[i]) continue;
                    
                    publisherScript component = array[i].GetComponent<publisherScript>();
                    if (component == null || !component) continue;
                    if (!component.isUnlocked || !component.publisher || component.onlyMobile || component.myID == mS.myID) continue;
                    
                    bool flag = false;
                    if (mS.multiplayer && component.isPlayer && mS.mpCalls_ != null)
                    {
                        player_mp player_mp2 = mS.mpCalls_.FindPlayer(component.myID);
                        if (player_mp2 != null && player_mp2.forschungSonstiges[33])
                        {
                            flag = true;
                        }
                    }

                    Toggle tglTochter = GetUiObject(__instance.uiObjects, 1)?.GetComponent<Toggle>();
                    Toggle tglNotForSale = GetUiObject(__instance.uiObjects, 7)?.GetComponent<Toggle>();
                    bool tochterIsOn = tglTochter != null ? tglTochter.isOn : false;
                    bool notForSaleIsOn = tglNotForSale != null ? tglNotForSale.isOn : false;

                    if (!flag && (!tochterIsOn || !SafeIsTochterfirma(component)) && (!notForSaleIsOn || SafeKaufangebotFree(component)))
                    {
                        string text = SafeStudioName(component);
                        string searchStringA = Traverse.Create(__instance).Field("searchStringA").GetValue<string>();
                        if (searchStringA == null) searchStringA = "";
                        searchStringA = searchStringA.ToLower();
                        if (text == null) text = "";
                        text = text.ToLower();

                        InputField inputSearch = GetUiObject(__instance.uiObjects, 6)?.GetComponent<InputField>();
                        string searchText = inputSearch != null ? inputSearch.text : "";

                        if (searchText.Length <= 0 || text.Contains(searchStringA))
                        {
                            if (contentPanel != null)
                            {
                                Item_Stats_Publisher component2 = UnityEngine.Object.Instantiate(__instance.uiPrefabs[0], Vector3.zero, Quaternion.identity, contentPanel.transform).GetComponent<Item_Stats_Publisher>();
                                if (component2 != null)
                                {
                                    component2.pS_ = component;
                                    component2.mS_ = mS;
                                    component2.tS_ = tS;
                                    component2.sfx_ = sfx;
                                    component2.guiMain_ = guiMain;
                                    component2.genres_ = genres;
                                    rowsBuilt++;
                                }
                            }
                        }
                    }
                }

                __instance.DROPDOWN_Sort();
                GameObject emptyLabel = GetUiObject(__instance.uiObjects, 4);
                if (guiMain != null && emptyLabel != null)
                {
                    guiMain.KeinEintrag(contentPanel, emptyLabel);
                }

                return false; 
            }
            catch (Exception ex)
            {
                if (log != null) log.LogError("Error in Menu_Statistics_Publisher.SetData Prefix: " + ex);
                return false; 
            }
        }
    }

    // Build the vanilla Developer list defensively so a bad tagged object cannot abort menu opening.
    [HarmonyPatch(typeof(Menu_Statistics_Developer), "SetData")]
    public static class MenuStatisticsDeveloperSetDataPatch
    {
        [HarmonyPrefix]
        public static bool Prefix(Menu_Statistics_Developer __instance)
        {
            try
            {
                if (__instance == null || !__instance) return true;
                if (__instance.uiObjects == null || __instance.uiObjects.Length <= 7) return false;
                if (__instance.uiPrefabs == null || __instance.uiPrefabs.Length == 0 || __instance.uiPrefabs[0] == null) return false;

                mainScript mS = GetMainScriptCached();
                textScript tS = mS != null && mS ? mS.GetComponent<textScript>() : null;
                sfxScript sfx = UnityEngine.Object.FindObjectOfType<sfxScript>();
                GUI_Main guiMain = GetGuiMainCached();
                genres genres = mS != null ? mS.GetComponent<genres>() : null;

                if (mS == null || !mS) return false;

                // Wire them to __instance so vanilla code and other patches can access them
                Traverse.Create(__instance).Field("mS_").SetValue(mS);
                Traverse.Create(__instance).Field("tS_").SetValue(tS);
                Traverse.Create(__instance).Field("sfx_").SetValue(sfx);
                Traverse.Create(__instance).Field("guiMain_").SetValue(guiMain);
                Traverse.Create(__instance).Field("genres_").SetValue(genres);

                // Deactivate existing children individually (triggers OnDisable -> Object.Destroy on each).
                // Must NOT use SetActive(false) on parent or DestroyImmediate — both cause crash via
                // OnDisable feedback loop (Item_Stats_Developer.OnDisable calls Object.Destroy on itself).
                GameObject contentPanel = GetUiObject(__instance.uiObjects, 0);
                if (contentPanel == null) return false;

                if (contentPanel != null)
                {
                    for (int c = 0; c < contentPanel.transform.childCount; c++)
                    {
                        Transform child = contentPanel.transform.GetChild(c);
                        if (child != null && child.gameObject != null && child.gameObject.activeSelf)
                        {
                            child.gameObject.SetActive(false);
                        }
                    }
                }

                int rowsBuilt = 0;
                GameObject[] array = GameObject.FindGameObjectsWithTag("Publisher");
                for (int i = 0; i < array.Length; i++)
                {
                    if (!array[i]) continue;
                    
                    publisherScript component = array[i].GetComponent<publisherScript>();
                    if (component == null || !component) continue;

                    Toggle tglTochter = GetUiObject(__instance.uiObjects, 1)?.GetComponent<Toggle>();
                    Toggle tglNotForSale = GetUiObject(__instance.uiObjects, 7)?.GetComponent<Toggle>();
                    bool tochterIsOn = tglTochter != null ? tglTochter.isOn : false;
                    bool notForSaleIsOn = tglNotForSale != null ? tglNotForSale.isOn : false;

                    if (component.isUnlocked && component.developer && component.myID != mS.myID && (!tochterIsOn || !SafeIsTochterfirma(component)) && (!notForSaleIsOn || SafeKaufangebotFree(component)))
                    {
                        string text = SafeStudioName(component);
                        string searchStringA = Traverse.Create(__instance).Field("searchStringA").GetValue<string>();
                        if (searchStringA == null) searchStringA = "";
                        searchStringA = searchStringA.ToLower();
                        if (text == null) text = "";
                        text = text.ToLower();

                        InputField inputSearch = GetUiObject(__instance.uiObjects, 6)?.GetComponent<InputField>();
                        string searchText = inputSearch != null ? inputSearch.text : "";

                        if (searchText.Length <= 0 || text.Contains(searchStringA))
                        {
                            if (contentPanel != null)
                            {
                                Item_Stats_Developer component2 = UnityEngine.Object.Instantiate(__instance.uiPrefabs[0], Vector3.zero, Quaternion.identity, contentPanel.transform).GetComponent<Item_Stats_Developer>();
                                if (component2 != null)
                                {
                                    component2.pS_ = component;
                                    component2.playerID = -1;
                                    component2.mS_ = mS;
                                    component2.tS_ = tS;
                                    component2.sfx_ = sfx;
                                    component2.guiMain_ = guiMain;
                                    component2.genres_ = genres;
                                    rowsBuilt++;
                                }
                            }
                        }
                    }
                }

                __instance.DROPDOWN_Sort();
                GameObject emptyLabel = GetUiObject(__instance.uiObjects, 4);
                if (guiMain != null && emptyLabel != null)
                {
                    guiMain.KeinEintrag(contentPanel, emptyLabel);
                }

                return false; 
            }
            catch (Exception ex)
            {
                if (log != null) log.LogError("Error in Menu_Statistics_Developer.SetData Prefix: " + ex);
                return false; 
            }
        }
    }

    [HarmonyPatch(typeof(savegameScript), "Save")]
    public static class SavegameScriptSavePatch
    {
        public static void Prefix(int i)
        {
            statePath = Path.Combine(Paths.ConfigPath, "StudioPartnerships_State_" + i + ".tsv");
        }

        public static void Postfix(int i)
        {
            SaveState();
        }
    }

    [HarmonyPatch(typeof(savegameScript), "Load")]
    public static class SavegameScriptLoadPatch
    {
        public static void Prefix(int i)
        {
            statePath = Path.Combine(Paths.ConfigPath, "StudioPartnerships_State_" + i + ".tsv");
            LoadState();
        }
    }

    [HarmonyPatch(typeof(publisherScript), "GetFirmenwert")]
    public static class PublisherScriptGetFirmenwertPatch
    {
        public static void Postfix(publisherScript __instance, ref long __result)
        {
            try
            {
                if (__instance == null || !__instance) return;
                if (Math.Abs(activeBuyoutDiscountFactor - 1.0f) < 0.0001f) return;
                GUI_Main gui = GetGuiMainCached();
                if (gui != null && gui.uiObjects != null && gui.uiObjects.Length > 386 && gui.uiObjects[386] != null && gui.uiObjects[386].activeSelf)
                {
                    Menu_W_FirmaKaufen fK = gui.uiObjects[386].GetComponent<Menu_W_FirmaKaufen>();
                    if (fK != null)
                    {
                        publisherScript pS = Traverse.Create(fK).Field("pS_").GetValue<publisherScript>();
                        if (pS == __instance)
                        {
                            __result = (long)(__result * activeBuyoutDiscountFactor);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogWarning("Error in GetFirmenwert Postfix: " + ex);
            }
        }
    }

    [HarmonyPatch(typeof(Menu_W_FirmaKaufen), "BUTTON_Yes")]
    public static class MenuWFirmaKaufenButtonYesPatch
    {
        public static void Postfix(Menu_W_FirmaKaufen __instance)
        {
            try
            {
                if (__instance == null || !__instance) return;
                publisherScript pS = Traverse.Create(__instance).Field("pS_").GetValue<publisherScript>();
                if (pS != null && pS.IsTochterfirma())
                {
                    AdjustRelation(pS, 20f); // +20 points for acquisition
                    activeBuyoutDiscountFactor = 1.0f;
                    SaveState();
                }
            }
            catch (Exception ex)
            {
                log.LogWarning("Error in Menu_W_FirmaKaufen.BUTTON_Yes Postfix: " + ex);
            }
        }
    }

    [HarmonyPatch(typeof(Menu_W_FirmaKaufen), "BUTTON_Abbrechen")]
    public static class MenuWFirmaKaufenButtonAbbrechenPatch
    {
        public static void Postfix()
        {
            activeBuyoutDiscountFactor = 1.0f;
        }
    }

    [HarmonyPatch(typeof(Item_ContractAuftragsspiel), "BUTTON_Remove")]
    public static class ItemContractAuftragsspielButtonRemovePatch
    {
        public static void Postfix(Item_ContractAuftragsspiel __instance)
        {
            try
            {
                if (__instance == null || !__instance) return;
                gameScript game = __instance.game_;
                mainScript mS = Traverse.Create(__instance).Field("mS_").GetValue<mainScript>();
                if (game != null && mS != null)
                {
                    publisherScript pub = FindStudioByID(mS, game.publisherID);
                    if (pub != null && GetSubdivision(pub) == 2)
                    {
                        AdjustRelation(pub, -5f); // decline contract offers = -5
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogWarning("Error in Item_ContractAuftragsspiel.BUTTON_Remove Postfix: " + ex);
            }
        }
    }

    [HarmonyPatch(typeof(Menu_W_GameVerwerfen), "BUTTON_Yes")]
    public static class MenuWGameVerwerfenButtonYesPatch
    {
        public static void Prefix(Menu_W_GameVerwerfen __instance)
        {
            try
            {
                if (__instance == null || !__instance) return;
                gameScript game = Traverse.Create(__instance).Field("gS_").GetValue<gameScript>();
                if (game != null)
                {
                    mainScript mS = FindObjectOfType<mainScript>();
                    if (mS == null) return;
                    publisherScript pub = FindStudioByID(mS, game.publisherID);
                    if (pub != null)
                    {
                        if (!processedCancelledContracts.Contains(game.myID))
                        {
                            processedCancelledContracts.Add(game.myID);
                            int subdiv = GetSubdivision(pub);

                            if (subdiv == 1 && game.exklusiv)
                            {
                                AdjustRelation(pub, -50f); // Cancel exclusivity contract = -50
                            }
                            else if (subdiv == 2)
                            {
                                AdjustRelation(pub, -30f); // Cancel publishing contract = -30
                            }
                            else if (subdiv == 3)
                            {
                                AdjustRelation(pub, -25f); // Cancel publishing agreement = -25
                            }
                            else if (subdiv == 4)
                            {
                                AdjustRelation(pub, -30f); // Cancel publishing agreement = -30
                            }
                            else if (subdiv == 5)
                            {
                                AdjustRelation(pub, -25f); // Cancel funded game = -25
                            }
                            else if (subdiv == 6)
                            {
                                AdjustRelation(pub, -30f); // Cancel funded game = -30
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogWarning("Error in Menu_W_GameVerwerfen.BUTTON_Yes Prefix: " + ex);
            }
        }
    }

    [HarmonyPatch(typeof(Menu_Result_IpVerkauf), "BUTTON_Yes")]
    public static class MenuResultIpVerkaufButtonYesPatch
    {
        public static void Postfix(Menu_Result_IpVerkauf __instance)
        {
            try
            {
                if (__instance == null || !__instance) return;
                gameScript game = Traverse.Create(__instance).Field("game_").GetValue<gameScript>();
                publisherScript pub = Traverse.Create(__instance).Field("pub_").GetValue<publisherScript>();
                if (game != null && pub != null)
                {
                    if (!processedGiftedIPs.Contains(game.mainIP))
                    {
                        processedGiftedIPs.Add(game.mainIP);
                        int subdiv = GetSubdivision(pub);
                        if (subdiv == 3)
                        {
                            AdjustRelation(pub, 10f); // Buy/Sell IPs (+10)
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogWarning("Error in Menu_Result_IpVerkauf.BUTTON_Yes Postfix: " + ex);
            }
        }
    }

    [HarmonyPatch(typeof(Menu_W_IpHandel_Buy), "BUTTON_Yes")]
    public static class MenuWIpHandelBuyButtonYesPatch
    {
        public static void Postfix(Menu_W_IpHandel_Buy __instance)
        {
            try
            {
                if (__instance == null || !__instance) return;
                gameScript game = Traverse.Create(__instance).Field("game_").GetValue<gameScript>();
                if (game != null)
                {
                    mainScript mS = FindObjectOfType<mainScript>();
                    if (mS == null) return;
                    publisherScript owner = FindStudioByID(mS, game.ownerID);
                    if (owner != null && !processedBoughtIPs.Contains(game.mainIP))
                    {
                        processedBoughtIPs.Add(game.mainIP);
                        int subdiv = GetSubdivision(owner);
                        if (subdiv == 3)
                        {
                            AdjustRelation(owner, 10f); // Buy/Sell IPs (+10)
                        }
                        else if (subdiv == 4 || subdiv == 5)
                        {
                            AdjustRelation(owner, 15f); // Buy IP (+15)
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogWarning("Error in Menu_W_IpHandel_Buy.BUTTON_Yes Postfix: " + ex);
            }
        }
    }

    [HarmonyPatch(typeof(gameScript), "SetOnMarket")]
    public static class GameScriptSetOnMarketPatch
    {
        public static void Postfix(gameScript __instance)
        {
            try
            {
                if (__instance == null || !__instance) return;
                mainScript mS = FindObjectOfType<mainScript>();
                if (mS == null) return;

                // 1. Process project completion for outsourced projects
                if (__instance.developerID != mS.myID)
                {
                    publisherScript dev = FindStudioByID(mS, __instance.developerID);
                    if (dev != null)
                    {
                        StudioState state = EnsureState(dev.myID);
                        if (state.OutsourcedGameIds.Contains(__instance.myID))
                        {
                            state.OutsourcedGameIds.Remove(__instance.myID);
                            AdjustRelation(dev, 15f); // Queue project completion (+15)

                            GUI_Main gui = FindObjectOfType<GUI_Main>();
                            if (gui != null)
                            {
                                gui.CreateTopNewsInfo($"{dev.GetName()} completed outsourced project '{__instance.myName}'! Relationship increased (+15).");
                            }

                            if (state.HasQueuedProject)
                            {
                                TriggerQueuedProject(dev, state);
                            }
                            SaveState();
                        }
                    }
                }

                // 2. Process relationship gains/penalties on game release
                if (__instance.developerID == mS.myID)
                {
                    // Platform Holders
                    if (__instance.gamePlatform != null)
                    {
                        for (int p = 0; p < 4; p++)
                        {
                            int platId = __instance.gamePlatform[p];
                            if (platId != -1)
                            {
                                platformScript platform = mS.arrayPlatformsScripts.FirstOrDefault(x => x != null && x.myID == platId);
                                if (platform != null)
                                {
                                    publisherScript platformHolder = FindStudioByID(mS, platform.ownerID);
                                    if (platformHolder != null && !platformHolder.isPlayer && GetSubdivision(platformHolder) == 1)
                                    {
                                        int releaseHash = __instance.myID * 1000 + platformHolder.myID;
                                        if (!processedReleasedGames.Contains(releaseHash))
                                        {
                                            processedReleasedGames.Add(releaseHash);
                                            AdjustRelation(platformHolder, 3f); // Release game on console (+3)

                                            if (__instance.reviewTotal >= 80)
                                            {
                                                AdjustRelation(platformHolder, 5f); // Review >= 80 (+5)
                                                int hitHash = __instance.myID * 1000 + platformHolder.myID;
                                                if (!processedHitGames.Contains(hitHash))
                                                {
                                                    processedHitGames.Add(hitHash);
                                                    AdjustRelation(platformHolder, 10f); // High sales / major hit (+10)
                                                }
                                            }

                                            if (__instance.reviewTotal >= 90)
                                            {
                                                int mpHash = __instance.myID * 1000 + platformHolder.myID;
                                                if (!processedMasterpieces.Contains(mpHash))
                                                {
                                                    processedMasterpieces.Add(mpHash);
                                                    AdjustRelation(platformHolder, 10f); // Masterpiece >= 90 (+10)
                                                }
                                            }

                                            if (__instance.exklusiv)
                                            {
                                                int exclHash = __instance.myID * 1000 + platformHolder.myID;
                                                if (!processedExclusiveGames.Contains(exclHash))
                                                {
                                                    processedExclusiveGames.Add(exclHash);
                                                    AdjustRelation(platformHolder, 30f); // Exclusivity (+30)
                                                }
                                            }

                                            // Low review penalties (-5 to -15)
                                            if (__instance.reviewTotal < 60)
                                            {
                                                float penalty = -5f;
                                                if (__instance.reviewTotal < 30) penalty = -15f;
                                                else if (__instance.reviewTotal < 45) penalty = -10f;
                                                AdjustRelation(platformHolder, penalty);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // Penalties for rival console owners on console exclusives
                    if (__instance.exklusiv && __instance.gamePlatform != null && __instance.gamePlatform[0] != -1)
                    {
                        platformScript platform = mS.arrayPlatformsScripts.FirstOrDefault(x => x != null && x.myID == __instance.gamePlatform[0]);
                        if (platform != null)
                        {
                            int activeExclHash = __instance.myID * 10000 + 111;
                            if (!processedExclusiveGames.Contains(activeExclHash))
                            {
                                processedExclusiveGames.Add(activeExclHash);
                                foreach (publisherScript ph in mS.arrayPublisherScripts)
                                {
                                    if (ph != null && !ph.isPlayer && GetSubdivision(ph) == 1 && ph.myID != platform.ownerID)
                                    {
                                        AdjustRelation(ph, -30f); // Rival console exclusive = -30
                                    }
                                }
                            }
                        }
                    }

                    // Publishers (if player uses NPC publisher)
                    if (__instance.publisherID != mS.myID)
                    {
                        publisherScript pub = FindStudioByID(mS, __instance.publisherID);
                        if (pub != null)
                        {
                            int pubSub = GetSubdivision(pub);
                            if (pubSub == 2) // Big Publisher
                            {
                                int pubHash = __instance.myID * 1000 + pub.myID;
                                if (!processedReleasedGames.Contains(pubHash))
                                {
                                    processedReleasedGames.Add(pubHash);
                                    AdjustRelation(pub, 5f); // Publish normally (+5)

                                    if (__instance.reviewTotal >= 80)
                                    {
                                        int hitHash = __instance.myID * 1000 + pub.myID;
                                        if (!processedHitGames.Contains(hitHash))
                                        {
                                            processedHitGames.Add(hitHash);
                                            StudioState state = EnsureState(pub.myID);
                                            if (state.ActiveContract == "CoPublish" || state.ActiveContract == "AAACoPublish" || gameActiveContracts.ContainsKey(__instance.myID))
                                            {
                                                AdjustRelation(pub, 25f); // AAA co-publishing success (+25)
                                            }
                                            else
                                            {
                                                AdjustRelation(pub, 20f); // Successful publishing deal (+20)
                                            }
                                        }
                                    }
                                }

                                if (__instance.typ_contractGame)
                                {
                                    int contractHash = __instance.myID * 1000 + pub.myID;
                                    if (!processedContractGames.Contains(contractHash))
                                    {
                                        processedContractGames.Add(contractHash);
                                        AdjustRelation(pub, 15f); // Contract games completed successfully (+15)
                                    }
                                }
                            }
                            else if (pubSub == 3) // Small Publisher
                            {
                                int pubHash = __instance.myID * 1000 + pub.myID;
                                if (!processedReleasedGames.Contains(pubHash))
                                {
                                    processedReleasedGames.Add(pubHash);
                                    AdjustRelation(pub, 5f); // Publish through them (+5)
                                }
                            }
                        }
                    }
                    else if (__instance.publisherID == mS.myID)
                    {
                        // Self-publish check: Ignore offered contract and self publish = -15
                        foreach (publisherScript pub in mS.arrayPublisherScripts)
                        {
                            if (pub != null && !pub.isPlayer && GetSubdivision(pub) == 3 && pub.GetRelation() >= 20f)
                            {
                                int selfPubHash = __instance.myID * 1000 + pub.myID;
                                if (!processedCancelledContracts.Contains(selfPubHash))
                                {
                                    processedCancelledContracts.Add(selfPubHash);
                                    AdjustRelation(pub, -15f); // Ignore offered contract penalty
                                }
                            }
                        }
                    }

                    // Competing releases check for Big Publishers
                    foreach (publisherScript pub in mS.arrayPublisherScripts)
                    {
                        if (pub != null && !pub.isPlayer && GetSubdivision(pub) == 2)
                        {
                            int competeHash = __instance.myID * 1000 + pub.myID;
                            if (!processedFailures.Contains(competeHash))
                            {
                                bool hasCompeting = false;
                                games g = FindObjectOfType<games>();
                                if (g != null && g.arrayGamesScripts != null)
                                {
                                    for (int j = 0; j < g.arrayGamesScripts.Length; j++)
                                    {
                                        gameScript otherGame = g.arrayGamesScripts[j];
                                        if (otherGame != null && otherGame.developerID == pub.myID && otherGame.isOnMarket && otherGame.weeksOnMarket <= 4 && otherGame.maingenre == __instance.maingenre)
                                        {
                                            hasCompeting = true;
                                            break;
                                        }
                                    }
                                }
                                if (hasCompeting)
                                {
                                    processedFailures.Add(competeHash);
                                    AdjustRelation(pub, -10f); // Competing releases = -10
                                }
                            }
                        }
                    }

                    // Development Commissions Completion Check
                    foreach (var kvp in states.Values)
                    {
                        if (kvp.CommissionWeeksLeft > 0)
                        {
                            publisherScript PH = FindStudioByID(mS, kvp.StudioId);
                            if (PH != null && GetSubdivision(PH) == 1)
                            {
                                bool onPlatform = false;
                                if (__instance.gamePlatform != null)
                                {
                                    for (int p = 0; p < 4; p++)
                                    {
                                        int platId = __instance.gamePlatform[p];
                                        if (platId != -1)
                                        {
                                            platformScript platform = mS.arrayPlatformsScripts.FirstOrDefault(x => x != null && x.myID == platId);
                                            if (platform != null && platform.ownerID == PH.myID)
                                            {
                                                onPlatform = true;
                                                break;
                                            }
                                        }
                                    }
                                }

                                if (onPlatform && __instance.maingenre == kvp.CommissionGenre && __instance.reviewTotal >= kvp.CommissionTargetReview)
                                {
                                    kvp.CommissionWeeksLeft = 0;
                                    mS.Earn(kvp.CommissionReward, 10);
                                    AdjustRelation(PH, 20f); // Accept commission / completed success (+20)
                                    SaveState();

                                    GUI_Main gui = FindObjectOfType<GUI_Main>();
                                    if (gui != null)
                                    {
                                        gui.MessageBox($"Commission completed successfully for {PH.GetName()}! Received {mS.GetMoney(kvp.CommissionReward, true)} and +20 Relationship.", false);
                                    }
                                }
                            }
                        }
                    }
                }

                // 3. Process developer releases (if player publishes their game)
                if (__instance.developerID != mS.myID && __instance.publisherID == mS.myID)
                {
                    publisherScript dev = FindStudioByID(mS, __instance.developerID);
                    if (dev != null)
                    {
                        int devSub = GetSubdivision(dev);
                        if (devSub == 3) // Small Publisher
                        {
                            int devHash = __instance.myID * 1000 + dev.myID;
                            if (!processedReleasedGames.Contains(devHash))
                            {
                                processedReleasedGames.Add(devHash);
                                AdjustRelation(dev, 20f); // Publish their games (+20)
                            }
                        }
                        else if (devSub == 4) // Big Developer
                        {
                            int devHash = __instance.myID * 1000 + dev.myID;
                            if (!processedReleasedGames.Contains(devHash))
                            {
                                processedReleasedGames.Add(devHash);
                                if (__instance.reviewTotal >= 80)
                                {
                                    AdjustRelation(dev, 20f); // Publish their games successfully (+20)
                                }
                                else if (__instance.reviewTotal < 60)
                                {
                                    AdjustRelation(dev, -15f); // Publishing failure = -15
                                }
                            }
                        }
                        else if (devSub == 5) // Mid Developer
                        {
                            int devHash = __instance.myID * 1000 + dev.myID;
                            if (!processedReleasedGames.Contains(devHash))
                            {
                                processedReleasedGames.Add(devHash);
                                AdjustRelation(dev, 15f); // Publish their games (+15)
                            }
                        }
                        else if (devSub == 6) // Indie Developer
                        {
                            int devHash = __instance.myID * 1000 + dev.myID;
                            if (!processedReleasedGames.Contains(devHash))
                            {
                                processedReleasedGames.Add(devHash);
                                StudioState devState = EnsureState(dev.myID);
                                if (!devState.PublishedFirstGame)
                                {
                                    devState.PublishedFirstGame = true;
                                    AdjustRelation(dev, 20f); // Publish first game (+20)
                                    SaveState();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogWarning("Error in gameScript.SetOnMarket Postfix: " + ex);
            }
        }
    }

    [HarmonyPatch(typeof(mainScript), "InitNewGame")]
    public static class MainScriptInitNewGamePatch
    {
        public static void Postfix()
        {
            states.Clear();
            processedReleasedGames.Clear();
            processedHitGames.Clear();
            processedMasterpieces.Clear();
            processedHighSales.Clear();
            processedExclusiveGames.Clear();
            processedContractGames.Clear();
            processedCommissions.Clear();
            processedFailures.Clear();
            processedCancelledContracts.Clear();
            processedBoughtIPs.Clear();
            processedGiftedIPs.Clear();
            processedEngineLicenses.Clear();
            processedCoDevelopments.Clear();
            gameActiveContracts.Clear();
            statePath = Path.Combine(Paths.ConfigPath, "StudioPartnerships_State.tsv");
            SaveState();
        }
    }

    [HarmonyPatch(typeof(Menu_DevGame), "BUTTON_Start")]
    public static class MenuDevGameButtonStartPatch
    {
        public static void Prefix()
        {
            CaptureStartSnapshot();
        }

        public static void Postfix()
        {
            ConvertCreatedGameToOutsourceProject();
        }
    }

    [HarmonyPatch(typeof(Menu_DevGameMain), "BUTTON_Abbrechen")]
    public static class MenuDevGameMainButtonAbbrechenPatch
    {
        public static void Postfix()
        {
            CancelDesignContext();
        }
    }

    [HarmonyPatch(typeof(Menu_DevGame), "BUTTON_Abbrechen")]
    public static class MenuDevGameButtonAbbrechenPatch
    {
        public static void Postfix()
        {
            CancelDesignContext();
        }
    }
}
