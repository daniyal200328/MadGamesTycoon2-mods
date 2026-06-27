using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public partial class SubsidiaryTeamsPlugin
{
    private class SlotStateCache
    {
        public int gameID = -2;
        public float remainingWeeks = -2f;
        public int stars = -2;
        public int speed = -2;
        public bool isUnlocked = false;
        public string teamName = null;
        public int studioStars = -2;
        public int studioSpeed = -2;
        public int activeCount = -2;
        public int helpingSlotIndex = -2;
        public float acceleration = -2f;
        public int teamXP = -1;
        public int studioXP = -1;
        public int traitID = -2;
    }

    private static SlotStateCache[] cachedSlotStates = new SlotStateCache[3]
        { new SlotStateCache(), new SlotStateCache(), new SlotStateCache() };

    private static GameObject sidePanel;
    internal static Menu_Stats_Tochterfirma_Main activeMenuInstance;
    private static SlotUIRow slotRow1;
    private static SlotUIRow slotRow2;
    private static SlotUIRow slotRow3;

    internal static readonly FieldInfo f_pS = AccessTools.Field(typeof(Menu_Stats_Tochterfirma_Main), "pS_");
    internal static readonly FieldInfo f_guiMain = AccessTools.Field(typeof(Menu_Stats_Tochterfirma_Main), "guiMain_");
    internal static readonly FieldInfo f_tS = AccessTools.Field(typeof(Menu_Stats_Tochterfirma_Main), "tS_");
    internal static readonly FieldInfo f_nextGame = AccessTools.Field(typeof(Menu_Stats_Tochterfirma_Main), "nextGame_");

    private static int lastBuiltForStudio = -1;
    private static System.Collections.Generic.HashSet<int> tagPromptedStudios = new System.Collections.Generic.HashSet<int>();

    private class SlotUIRow
    {
        public GameObject container;
        public GameObject projectBox;
        public Image progressFill;
        public Text progressText;
        public Text gameNameText;
        public Text weeksText;
        public Button discardButton;
        public Button infoButton;
        public Image gameTypeIcon;
        public Image gameSizeIcon;
        public Text ipPopularityText;
        public Text genreText;
        public GameObject clockIcon;
        public Text headerText;
        public Button renameButton;
        // Status text shown when idle/helping (replaces idlePanel)
        public Text statusText;
        public Button upgradeStarsButton;
        public Text upgradeStarsText;
        public Button closeTeamButton;
        public Image xpBarFill;
        public Text xpBarText;
        public GameObject lockOverlay;
        public Button unlockButton;
        public Text unlockButtonText;
        public Button roleButton;
        public Text roleText;
        public Button supportTargetButton;
        public Text supportTargetText;
        public Button chooseTagButton;
        public Text chooseTagButtonText;
    }

    internal static void ClearClonedUI()
    {
        if (sidePanel != null) UnityEngine.Object.Destroy(sidePanel);
        sidePanel = null;
        slotRow1 = null;
        slotRow2 = null;
        slotRow3 = null;
        lastBuiltForStudio = -1;

        for (int i = 0; i < 3; i++)
        {
            cachedSlotStates[i].gameID = -2;
            cachedSlotStates[i].remainingWeeks = -2f;
            cachedSlotStates[i].stars = -2;
            cachedSlotStates[i].speed = -2;
            cachedSlotStates[i].isUnlocked = false;
            cachedSlotStates[i].teamName = null;
            cachedSlotStates[i].studioStars = -2;
            cachedSlotStates[i].studioSpeed = -2;
            cachedSlotStates[i].activeCount = -2;
            cachedSlotStates[i].teamXP = -1;
            cachedSlotStates[i].studioXP = -1;
            cachedSlotStates[i].traitID = -2;
        }

        if (activeMenuInstance != null)
        {
            Transform menueTransform = activeMenuInstance.transform.Find("Menue");
            if (menueTransform != null)
            {
                RectTransform rt = menueTransform.GetComponent<RectTransform>();
                if (rt != null) rt.anchoredPosition = Vector2.zero;
            }
            activeMenuInstance = null;
        }
        tagPromptedStudios.Clear();
    }

    internal static void RefreshSlotUI(Menu_Stats_Tochterfirma_Main menu, publisherScript studio)
    {
        if (menu == null || studio == null) return;
        activeMenuInstance = menu;
        if (!studio.developer) { ClearClonedUI(); return; }

        if (lastBuiltForStudio != studio.myID)
        {
            ClearClonedUI();
            lastBuiltForStudio = studio.myID;
        }

        GUI_Main guiMain = f_guiMain.GetValue(menu) as GUI_Main;
        textScript tS = f_tS.GetValue(menu) as textScript;
        StudioSlotData data = GetStudioSlotData(studio.myID);
        games gamesScript = studio.games_ ?? GetGamesScript();

        EnsureSidePanel(menu, data, tS, guiMain, studio);

        if (slotRow1 != null) UpdateSlotRow(slotRow1, data, 0, gamesScript, studio, guiMain, tS, menu);
        if (slotRow2 != null) UpdateSlotRow(slotRow2, data, 1, gamesScript, studio, guiMain, tS, menu);
        if (slotRow3 != null) UpdateSlotRow(slotRow3, data, 2, gamesScript, studio, guiMain, tS, menu);

        // Auto-prompt tag selection: once per studio open, if any slot is ★3+ with no tag
        if (!tagPromptedStudios.Contains(studio.myID) && data != null && data.slots != null)
        {
            tagPromptedStudios.Add(studio.myID);
            for (int i = 0; i < 3 && i < data.slots.Length; i++)
            {
                SlotData s = data.slots[i];
                if (s != null && s.isUnlocked && s.stars >= 3 && s.traitID == -1)
                {
                    System.Random rng = new System.Random();
                    tagWindowOptions = GetRandomTagIDs(3, rng);
                    tagWindowStudioID = studio.myID;
                    tagWindowSlotIdx = i;
                    showTagWindow = true;
                    break;
                }
            }
        }
    }

    private static void NormalizeRectTransform(Transform t, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, Vector2 sizeDelta, Vector2 anchoredPosition)
    {
        if (t == null) return;
        RectTransform rt = t.GetComponent<RectTransform>();
        if (rt == null) return;
        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        rt.pivot = pivot;
        rt.sizeDelta = sizeDelta;
        rt.anchoredPosition = anchoredPosition;
        rt.localScale = Vector3.one;
    }

    private static void EnsureSidePanel(Menu_Stats_Tochterfirma_Main menu, StudioSlotData data, textScript tS, GUI_Main guiMain, publisherScript studio)
    {
        if (menu.uiObjects == null || menu.uiObjects.Length <= 31) return;

        var hook = menu.gameObject.GetComponent<STSWindowLifecycleHook>();
        if (hook == null)
            hook = menu.gameObject.AddComponent<STSWindowLifecycleHook>();

        if (sidePanel != null) return;

        // Shift the left vanilla panel further left to make room for wider side panel
        Transform menueTransform = menu.transform.Find("Menue");
        if (menueTransform != null)
        {
            RectTransform menueRt = menueTransform.GetComponent<RectTransform>();
            if (menueRt != null) menueRt.anchoredPosition = new Vector2(-215f, 0f);
        }

        Transform targetParent = menu.transform;

        sidePanel = new GameObject("STS_SidePanel", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        sidePanel.transform.SetParent(targetParent, false);

        Image mainBg = menu.GetComponent<Image>() ?? menueTransform?.GetComponent<Image>();
        Image sideBg = sidePanel.GetComponent<Image>();
        if (mainBg != null && sideBg != null)
        {
            sideBg.sprite = mainBg.sprite;
            sideBg.color = mainBg.color;
            sideBg.type = mainBg.type;
            sideBg.material = mainBg.material;
        }

        // 460px wide x 660px tall — enough room for 3 cards of 190px + gaps
        RectTransform sideRect = sidePanel.GetComponent<RectTransform>();
        sideRect.anchorMin = new Vector2(0.5f, 0.5f);
        sideRect.anchorMax = new Vector2(0.5f, 0.5f);
        sideRect.pivot = new Vector2(0f, 0.5f);
        sideRect.sizeDelta = new Vector2(460f, 660f);
        sideRect.anchoredPosition = new Vector2(240f, 0f);
        sideRect.localScale = Vector3.one;

        GameObject titleObj = new GameObject("Title", typeof(RectTransform), typeof(Text));
        titleObj.transform.SetParent(sidePanel.transform, false);
        NormalizeRectTransform(titleObj.transform, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, 40f), new Vector2(0f, -15f));

        Text titleText = titleObj.GetComponent<Text>();
        titleText.alignment = TextAnchor.MiddleCenter;
        titleText.fontSize = 18;
        titleText.fontStyle = FontStyle.Bold;
        titleText.color = Color.white;
        titleText.text = "Development Teams";
        if (menu.uiObjects[23] != null)
        {
            Text refT = menu.uiObjects[23].GetComponent<Text>();
            if (refT != null) titleText.font = refT.font;
        }

        // Cards: 190px tall each, 15px gaps, stacked from top
        slotRow1 = BuildSlotRowFromTemplate(menu, "STS_Slot1_Box", sidePanel.transform, -50f,  0, studio, guiMain);
        slotRow2 = BuildSlotRowFromTemplate(menu, "STS_Slot2_Box", sidePanel.transform, -255f, 1, studio, guiMain);
        slotRow3 = BuildSlotRowFromTemplate(menu, "STS_Slot3_Box", sidePanel.transform, -460f, 2, studio, guiMain);
    }

    private static string GetRelativePath(Transform root, Transform child)
    {
        if (root == child) return "";
        List<string> pathParts = new List<string>();
        Transform curr = child;
        while (curr != null && curr != root)
        {
            pathParts.Add(curr.name);
            curr = curr.parent;
        }
        pathParts.Reverse();
        return string.Join("/", pathParts);
    }

    private static SlotUIRow BuildSlotRowFromTemplate(
        Menu_Stats_Tochterfirma_Main menu,
        string name, Transform parent, float yPosition, int slotIdx,
        publisherScript studio, GUI_Main guiMain)
    {
        GameObject refNameText = menu.uiObjects[23];
        if (refNameText == null) return null;
        Transform origBox = refNameText.transform.parent;
        if (origBox == null) return null;

        GameObject slotBox = UnityEngine.Object.Instantiate(origBox.gameObject, parent);
        slotBox.name = name;

        RectTransform boxRect = slotBox.GetComponent<RectTransform>();
        boxRect.anchorMin = new Vector2(0.03f, 1f);
        boxRect.anchorMax = new Vector2(0.97f, 1f);
        boxRect.pivot = new Vector2(0.5f, 1f);
        boxRect.sizeDelta = new Vector2(0f, 190f);
        boxRect.anchoredPosition = new Vector2(0f, yPosition);
        boxRect.localScale = Vector3.one;

        int capturedSlot = slotIdx;
        SlotUIRow row = new SlotUIRow();
        row.container = slotBox;
        row.projectBox = slotBox;

        string namePath = GetRelativePath(origBox, refNameText.transform);
        string barPath = menu.uiObjects[19] != null ? GetRelativePath(origBox, menu.uiObjects[19].transform) : "";
        string txtPath = menu.uiObjects[20] != null ? GetRelativePath(origBox, menu.uiObjects[20].transform) : "";
        string weeksPath = menu.uiObjects[29] != null ? GetRelativePath(origBox, menu.uiObjects[29].transform) : "";
        string infoPath = menu.uiObjects[30] != null ? GetRelativePath(origBox, menu.uiObjects[30].transform) : "";
        string discardPath = menu.uiObjects[31] != null ? GetRelativePath(origBox, menu.uiObjects[31].transform) : "";
        string typePath = menu.uiObjects[24] != null ? GetRelativePath(origBox, menu.uiObjects[24].transform) : "";
        string sizePath = menu.uiObjects[25] != null ? GetRelativePath(origBox, menu.uiObjects[25].transform) : "";
        string ipPath = menu.uiObjects[26] != null ? GetRelativePath(origBox, menu.uiObjects[26].transform) : "";
        string genrePath = menu.uiObjects[27] != null ? GetRelativePath(origBox, menu.uiObjects[27].transform) : "";
        string clockPath = menu.uiObjects[28] != null ? GetRelativePath(origBox, menu.uiObjects[28].transform) : "";

        row.gameNameText = slotBox.transform.Find(namePath)?.GetComponent<Text>();
        row.progressFill = !string.IsNullOrEmpty(barPath) ? slotBox.transform.Find(barPath)?.GetComponent<Image>() : null;
        row.progressText = !string.IsNullOrEmpty(txtPath) ? slotBox.transform.Find(txtPath)?.GetComponent<Text>() : null;
        row.weeksText = !string.IsNullOrEmpty(weeksPath) ? slotBox.transform.Find(weeksPath)?.GetComponent<Text>() : null;
        row.infoButton = !string.IsNullOrEmpty(infoPath) ? slotBox.transform.Find(infoPath)?.GetComponent<Button>() : null;
        row.discardButton = !string.IsNullOrEmpty(discardPath) ? slotBox.transform.Find(discardPath)?.GetComponent<Button>() : null;
        if (!string.IsNullOrEmpty(typePath)) row.gameTypeIcon = slotBox.transform.Find(typePath)?.GetComponent<Image>();
        if (!string.IsNullOrEmpty(sizePath)) row.gameSizeIcon = slotBox.transform.Find(sizePath)?.GetComponent<Image>();
        if (!string.IsNullOrEmpty(ipPath)) row.ipPopularityText = slotBox.transform.Find(ipPath)?.GetComponent<Text>();
        if (!string.IsNullOrEmpty(genrePath)) row.genreText = slotBox.transform.Find(genrePath)?.GetComponent<Text>();
        if (!string.IsNullOrEmpty(clockPath)) row.clockIcon = slotBox.transform.Find(clockPath)?.gameObject;

        foreach (Transform child in slotBox.transform.GetComponentsInChildren<Transform>(true))
        {
            if (child.gameObject.name == "Button_SubsidiaryProjectDirector" || child.gameObject.name == "Button_StudioDirector")
            {
                child.gameObject.SetActive(false);
            }
        }

        Transform blueBar = slotBox.transform.Find("TxtTop (3)");
        if (blueBar != null)
        {
            NormalizeRectTransform(blueBar, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, 20f), new Vector2(0f, 0f));
            Transform blueBarText = blueBar.Find("Text");
            if (blueBarText != null) blueBarText.gameObject.SetActive(false);
        }

        if (row.gameNameText != null)
        {
            NormalizeRectTransform(row.gameNameText.transform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 0.5f), new Vector2(340f, 20f), new Vector2(0f, -35f));
            row.gameNameText.alignment = TextAnchor.MiddleCenter;
            row.gameNameText.fontSize = 11;
        }

        if (row.genreText != null)
        {
            NormalizeRectTransform(row.genreText.transform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 0.5f), new Vector2(340f, 16f), new Vector2(0f, -55f));
            row.genreText.alignment = TextAnchor.MiddleCenter;
            row.genreText.fontSize = 9;
        }

        if (row.gameTypeIcon != null)
            NormalizeRectTransform(row.gameTypeIcon.transform, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0.5f, 0.5f), new Vector2(24f, 24f), new Vector2(20f, -35f));
        if (row.gameSizeIcon != null)
            NormalizeRectTransform(row.gameSizeIcon.transform, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0.5f, 0.5f), new Vector2(24f, 24f), new Vector2(48f, -35f));

        if (row.progressFill != null)
        {
            Transform progressBG = row.progressFill.transform.parent;
            if (progressBG != null)
            {
                NormalizeRectTransform(progressBG, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 0.5f), new Vector2(250f, 18f), new Vector2(0f, -82f));
                NormalizeRectTransform(row.progressFill.transform, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f), new Vector2(-4f, -4f), Vector2.zero);
            }
        }

        if (row.progressText != null)
        {
            NormalizeRectTransform(row.progressText.transform, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
            row.progressText.alignment = TextAnchor.MiddleCenter;
            row.progressText.fontSize = 9;
        }

        if (row.clockIcon != null)
        {
            NormalizeRectTransform(row.clockIcon.transform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 0.5f), new Vector2(22f, 22f), new Vector2(-150f, -82f));
            Transform weeksNum = row.clockIcon.transform.Find("TextWeeksNum");
            if (weeksNum != null)
            {
                NormalizeRectTransform(weeksNum, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 1f), new Vector2(40f, 14f), new Vector2(0f, -2f));
                Text weeksNumText = weeksNum.GetComponent<Text>();
                if (weeksNumText != null) { weeksNumText.alignment = TextAnchor.MiddleCenter; weeksNumText.fontSize = 9; }
            }
        }

        if (row.ipPopularityText != null)
        {
            Transform iconIP = row.ipPopularityText.transform.parent;
            if (iconIP != null)
            {
                NormalizeRectTransform(iconIP, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 0.5f), new Vector2(22f, 22f), new Vector2(150f, -82f));
                NormalizeRectTransform(row.ipPopularityText.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 1f), new Vector2(40f, 14f), new Vector2(0f, -2f));
                row.ipPopularityText.alignment = TextAnchor.MiddleCenter;
                row.ipPopularityText.fontSize = 9;
            }
        }

        if (row.weeksText != null)
        {
            row.weeksText.transform.SetParent(slotBox.transform, false);
            NormalizeRectTransform(row.weeksText.transform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 0.5f), new Vector2(60f, 16f), new Vector2(-150f, -108f));
            row.weeksText.alignment = TextAnchor.MiddleCenter;
            row.weeksText.fontSize = 10;
            row.weeksText.fontStyle = FontStyle.Bold;
            row.weeksText.color = Color.black;
        }

        if (row.infoButton != null)
        {
            NormalizeRectTransform(row.infoButton.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(160f, 22f), new Vector2(-15f, 8f));
            Text infoBtnText = row.infoButton.transform.Find("Text")?.GetComponent<Text>();
            if (infoBtnText != null) { infoBtnText.alignment = TextAnchor.MiddleCenter; infoBtnText.fontSize = 9; }

            row.infoButton.onClick.RemoveAllListeners();
            row.infoButton.onClick.AddListener(() =>
            {
                publisherScript targetStudio = activeMenuInstance != null ? f_pS.GetValue(activeMenuInstance) as publisherScript : studio;
                if (targetStudio == null) return;

                StudioSlotData slotData = GetStudioSlotData(targetStudio.myID);
                if (slotData == null) return;
                int gID = slotData.slots[capturedSlot].gameID;
                if (gID != -1)
                {
                    games gScriptObj = targetStudio.games_ ?? GetGamesScript();
                    gameScript gameToReport = FindGameByID(gScriptObj, gID);
                    if (gameToReport != null)
                    {
                        GUI_Main gui = GetGuiMain();
                        if (gui != null)
                        {
                            gui.ActivateMenu(gui.uiObjects[406]);
                            gui.uiObjects[406].GetComponent<Menu_Stats_Tochterfirma_GameEntwicklungsbericht>().Init(gameToReport);
                        }
                    }
                }
                else
                {
                    TryOpenStudioDirector(targetStudio);
                }
            });
        }

        if (row.discardButton != null)
        {
            NormalizeRectTransform(row.discardButton.transform, new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(24f, 24f), new Vector2(-10f, 8f));
            Transform discardIcon = row.discardButton.transform.Find("Icon");
            if (discardIcon != null) NormalizeRectTransform(discardIcon, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);

            row.discardButton.onClick.RemoveAllListeners();
            row.discardButton.onClick.AddListener(() =>
            {
                publisherScript targetStudio = activeMenuInstance != null ? f_pS.GetValue(activeMenuInstance) as publisherScript : studio;
                if (targetStudio != null)
                {
                    StudioSlotData slotData = GetStudioSlotData(targetStudio.myID);
                    int gID = slotData.slots[capturedSlot].gameID;
                    if (gID != -1)
                    {
                        games gScriptObj = targetStudio.games_ ?? GetGamesScript();
                        gameScript gameToDiscard = FindGameByID(gScriptObj, gID);
                        if (gameToDiscard != null)
                        {
                            slotToDiscard = capturedSlot;
                            GUI_Main gui = GetGuiMain();
                            if (gui != null)
                            {
                                gui.ActivateMenu(gui.uiObjects[93]);
                                gui.uiObjects[93].GetComponent<Menu_W_GameVerwerfen>().Init(gameToDiscard, null);
                            }
                        }
                    }
                }
            });
        }

        GameObject headerObj = new GameObject("SlotHeader", typeof(RectTransform), typeof(Text));
        headerObj.transform.SetParent(slotBox.transform, false);
        NormalizeRectTransform(headerObj.transform, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f), new Vector2(-20f, 20f), new Vector2(10f, 0f));

        row.headerText = headerObj.GetComponent<Text>();
        row.headerText.alignment = TextAnchor.MiddleLeft;
        row.headerText.fontSize = 11;
        row.headerText.fontStyle = FontStyle.Bold;
        row.headerText.color = new Color(0.9f, 0.9f, 0.9f, 1f);
        if (row.gameNameText != null) row.headerText.font = row.gameNameText.font;

        GameObject renameBtnObj = new GameObject("RenameBtn", typeof(RectTransform), typeof(Image), typeof(Button));
        renameBtnObj.transform.SetParent(slotBox.transform, false);
        NormalizeRectTransform(renameBtnObj.transform, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(50f, 16f), new Vector2(-10f, -28f));

        Image renameImg = renameBtnObj.GetComponent<Image>();
        renameImg.color = new Color(0.2f, 0.2f, 0.2f, 0.5f);
        row.renameButton = renameBtnObj.GetComponent<Button>();
        row.renameButton.targetGraphic = renameImg;

        GameObject renameTxtObj = new GameObject("Text", typeof(RectTransform), typeof(Text));
        renameTxtObj.transform.SetParent(renameBtnObj.transform, false);
        NormalizeRectTransform(renameTxtObj.transform, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
        Text renameTxt = renameTxtObj.GetComponent<Text>();
        renameTxt.alignment = TextAnchor.MiddleCenter;
        renameTxt.fontSize = 10;
        renameTxt.color = Color.white;
        renameTxt.text = "Rename";
        if (row.gameNameText != null) renameTxt.font = row.gameNameText.font;

        row.renameButton.onClick.AddListener(() =>
        {
            showRenameWindow = true;
            renameSlotIndex = capturedSlot;
            renameStudioID = studio.myID;
            renameInputString = GetStudioSlotData(studio.myID).slots[capturedSlot].teamName;
        });

        // Status text — shown when idle or supporting (replaces idlePanel)
        GameObject statusObj = new GameObject("StatusText", typeof(RectTransform), typeof(Text));
        statusObj.transform.SetParent(slotBox.transform, false);
        NormalizeRectTransform(statusObj.transform, new Vector2(0f, 0.5f), new Vector2(1f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(-20f, 30f), new Vector2(0f, 20f));
        row.statusText = statusObj.GetComponent<Text>();
        row.statusText.alignment = TextAnchor.MiddleCenter;
        row.statusText.fontSize = 13;
        row.statusText.color = new Color(0.7f, 0.7f, 0.7f, 1f);
        row.statusText.text = "Idle";
        if (row.gameNameText != null) row.statusText.font = row.gameNameText.font;
        SetActive(row.statusText, false);

        // ── XP bar ──────────────────────────────────────────────────────────
        // Fixed at y=64 from bottom — always visible, above the button rows.
        // Layout (bottom-up): y=10 action row | y=36 role row | y=62 XP bar
        // ────────────────────────────────────────────────────────────────────
        GameObject xpBarBg = new GameObject("XPBarBg", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        xpBarBg.transform.SetParent(slotBox.transform, false);
        NormalizeRectTransform(xpBarBg.transform,
            new Vector2(0.05f, 0f), new Vector2(0.95f, 0f),
            new Vector2(0.5f, 0f), new Vector2(0f, 14f), new Vector2(0f, 62f));
        Image xpBarBgImg = xpBarBg.GetComponent<Image>();
        xpBarBgImg.color = new Color(0.1f, 0.1f, 0.1f, 0.95f);

        GameObject xpBarFill = new GameObject("XPBarFill", typeof(RectTransform), typeof(Image));
        xpBarFill.transform.SetParent(xpBarBg.transform, false);
        row.xpBarFill = xpBarFill.GetComponent<Image>();
        row.xpBarFill.type = Image.Type.Filled;
        row.xpBarFill.fillMethod = Image.FillMethod.Horizontal;
        row.xpBarFill.fillAmount = 0f;
        row.xpBarFill.color = new Color(0.18f, 0.55f, 1f, 1f);
        NormalizeRectTransform(xpBarFill.transform, Vector2.zero, Vector2.one,
            new Vector2(0.5f, 0.5f), new Vector2(-2f, -2f), Vector2.zero);

        GameObject xpBarTextObj = new GameObject("XPBarText", typeof(RectTransform), typeof(Text));
        xpBarTextObj.transform.SetParent(xpBarBg.transform, false);
        row.xpBarText = xpBarTextObj.GetComponent<Text>();
        row.xpBarText.alignment = TextAnchor.MiddleCenter;
        row.xpBarText.fontSize = 9;
        row.xpBarText.color = Color.white;
        row.xpBarText.text = "XP: 0/0";
        if (row.gameNameText != null) row.xpBarText.font = row.gameNameText.font;
        NormalizeRectTransform(xpBarTextObj.transform, Vector2.zero, Vector2.one,
            new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);

        // ── Role button ─────────────────────────────────────────────────────
        // Always at y=36 (middle row). Clickable when team is idle or supporting.
        // ────────────────────────────────────────────────────────────────────
        row.roleButton = CreateActionButton(slotBox.transform, "RoleBtn",
            new Vector2(0f, 36f), new Vector2(240f, 22f), row.gameNameText?.font, out row.roleText);
        row.roleButton.onClick.AddListener(() =>
        {
            showRoleWindow = true;
            roleSlotIndex = capturedSlot;
            roleStudioID = studio.myID;
        });
        SetActive(row.roleButton, false);

        // Support target button — shown below role button when role=SUPPORT.
        // Clicking cycles through available target teams.
        row.supportTargetButton = CreateActionButton(slotBox.transform, "SupportTargetBtn",
            new Vector2(0f, 36f), new Vector2(240f, 22f), row.gameNameText?.font, out row.supportTargetText);
        row.supportTargetButton.onClick.AddListener(() =>
        {
            publisherScript targetStudio = activeMenuInstance != null
                ? f_pS.GetValue(activeMenuInstance) as publisherScript : studio;
            if (targetStudio == null) return;
            StudioSlotData slotData = GetStudioSlotData(targetStudio.myID);
            SlotData s = slotData.slots[capturedSlot];
            int nextTarget = s.supportTarget;
            while (true)
            {
                nextTarget++;
                if (nextTarget > 2) nextTarget = -1;
                if (nextTarget == -1 || nextTarget != capturedSlot) break;
            }
            s.supportTarget = nextTarget;
            SaveState(currentSaveSlot);
            RefreshSupportAssignments(targetStudio);
            lastBuiltForStudio = -1;
            if (activeMenuInstance != null) activeMenuInstance.UpdateData();
        });
        SetActive(row.supportTargetButton, false);

        // ── Action buttons (bottom row, y=10) ───────────────────────────────
        // Slot 0: single wide "Upgrade Studio ★" (no Close — main team is permanent)
        // Slots 1-2: "Upgrade ★" (left) + "Close Team" (right)
        // ────────────────────────────────────────────────────────────────────
        if (slotIdx == 0)
        {
            row.upgradeStarsButton = CreateActionButton(slotBox.transform, "UpgradeStudio",
                new Vector2(0f, 10f), new Vector2(260f, 24f), row.gameNameText?.font, out row.upgradeStarsText);
            row.upgradeStarsButton.onClick.AddListener(() => TryUpgradeTeam(studio, capturedSlot));
        }
        else
        {
            row.upgradeStarsButton = CreateActionButton(slotBox.transform, "UpgradeTeam",
                new Vector2(-68f, 10f), new Vector2(170f, 24f), row.gameNameText?.font, out row.upgradeStarsText);
            row.upgradeStarsButton.onClick.AddListener(() => TryUpgradeTeam(studio, capturedSlot));
        }
        SetActive(row.upgradeStarsButton, false);

        row.closeTeamButton = CreateActionButton(slotBox.transform, "CloseTeam",
            new Vector2(92f, 10f), new Vector2(120f, 24f), row.gameNameText?.font, out Text closeText);
        closeText.text = "Close Team";
        row.closeTeamButton.onClick.AddListener(() => TryCloseSlot(studio, capturedSlot));
        SetActive(row.closeTeamButton, false);

        row.chooseTagButton = CreateActionButton(slotBox.transform, "ChooseTag",
            new Vector2(0f, 112f), new Vector2(160f, 22f), row.gameNameText?.font, out row.chooseTagButtonText);
        row.chooseTagButton.onClick.AddListener(() =>
        {
            StudioSlotData sd = GetStudioSlotData(studio.myID);
            if (sd == null || capturedSlot < 0 || capturedSlot >= sd.slots.Length) return;
            if (sd.slots[capturedSlot].traitID != -1) return;
            System.Random rng = new System.Random();
            tagWindowOptions = GetRandomTagIDs(3, rng);
            tagWindowStudioID = studio.myID;
            tagWindowSlotIdx = capturedSlot;
            showTagWindow = true;
        });
        SetActive(row.chooseTagButton, false);

        if (slotIdx > 0)
        {
            row.lockOverlay = new GameObject("LockOverlay", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            row.lockOverlay.transform.SetParent(slotBox.transform, false);
            NormalizeRectTransform(row.lockOverlay.transform, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
            Image lockImg = row.lockOverlay.GetComponent<Image>();
            lockImg.color = new Color(0.12f, 0.12f, 0.12f, 0.9f);

            row.unlockButton = CreateActionButton(row.lockOverlay.transform, "UnlockBtn", new Vector2(0f, 0f), new Vector2(240f, 32f), row.gameNameText?.font, out row.unlockButtonText);
            row.unlockButton.GetComponent<Image>().color = new Color(0.28f, 0.56f, 0.92f, 1f);
            row.unlockButton.onClick.AddListener(() => TryPurchaseSlot(studio, capturedSlot));
            NormalizeRectTransform(row.unlockButton.transform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(240f, 32f), new Vector2(0f, 0f));
        }

        return row;
    }

    private static Button CreateActionButton(Transform parent, string name, Vector2 anchoredPos, Vector2 size, Font font, out Text buttonText)
    {
        GameObject btnObj = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
        btnObj.transform.SetParent(parent, false);
        RectTransform rect = btnObj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0f);
        rect.anchorMax = new Vector2(0.5f, 0f);
        rect.pivot = new Vector2(0.5f, 0f);
        rect.sizeDelta = size;
        rect.anchoredPosition = anchoredPos;
        rect.localScale = Vector3.one;

        Image img = btnObj.GetComponent<Image>();
        img.color = new Color(0.3f, 0.3f, 0.3f, 0.9f);
        Button btn = btnObj.GetComponent<Button>();
        btn.targetGraphic = img;

        GameObject txtObj = new GameObject("Text", typeof(RectTransform), typeof(Text));
        txtObj.transform.SetParent(btnObj.transform, false);
        RectTransform txtRect = txtObj.GetComponent<RectTransform>();
        txtRect.anchorMin = Vector2.zero;
        txtRect.anchorMax = Vector2.one;
        txtRect.offsetMin = Vector2.zero;
        txtRect.offsetMax = Vector2.zero;
        txtRect.localScale = Vector3.one;

        buttonText = txtObj.GetComponent<Text>();
        buttonText.alignment = TextAnchor.MiddleCenter;
        buttonText.fontSize = 11;
        buttonText.fontStyle = FontStyle.Bold;
        buttonText.color = Color.white;
        if (font != null) buttonText.font = font;
        return btn;
    }

    private static void UpdateSlotRow(SlotUIRow row, StudioSlotData data, int slotIdx, games gamesScript, publisherScript studio, GUI_Main guiMain, textScript tS, Menu_Stats_Tochterfirma_Main menu)
    {
        if (row == null || row.container == null) return;

        SanitizeSlotSpeeds(studio, data);

        SlotData slot = data.slots[slotIdx];
        int maxSpeed = studio.developmentSpeed;
        int tStars = slot.stars;
        int tSpeed = (slotIdx == 0) ? maxSpeed : slot.speed;
        float acceleration = GetAccelerationFactor(studio, slotIdx);

        SlotStateCache cache = cachedSlotStates[slotIdx];
        bool dirty = false;
        if (cache.gameID != slot.gameID || cache.stars != tStars || cache.speed != tSpeed ||
            cache.isUnlocked != slot.isUnlocked || cache.teamName != slot.teamName ||
            cache.studioStars != slot.stars || cache.studioSpeed != maxSpeed ||
            cache.helpingSlotIndex != slot.helpingSlotIndex ||
            cache.teamXP != slot.teamXP || cache.studioXP != data.studioXP ||
            cache.traitID != slot.traitID ||
            Mathf.Abs(cache.acceleration - acceleration) > 0.01f)
        {
            dirty = true;
        }

        if (slot.gameID != -1 && Mathf.Abs(cache.remainingWeeks - slot.remainingWeeks) > 0.05f)
            dirty = true;

        if (!dirty) return;

        cache.gameID = slot.gameID;
        cache.remainingWeeks = slot.remainingWeeks;
        cache.stars = tStars;
        cache.speed = tSpeed;
        cache.isUnlocked = slot.isUnlocked;
        cache.teamName = slot.teamName;
        cache.studioStars = slot.stars;
        cache.studioSpeed = maxSpeed;
        cache.helpingSlotIndex = slot.helpingSlotIndex;
        cache.acceleration = acceleration;
        cache.teamXP = slot.teamXP;
        cache.studioXP = data.studioXP;
        cache.traitID = slot.traitID;

        int weeks = 0;
        int progressPercent = 0;
        if (slot.gameID != -1)
        {
            gameScript game = FindGameByID(gamesScript, slot.gameID);
            int minFloor = (game != null) ? GetMinimumFloorWeeks(game.gameSize) : 1;
            int calculatedWeeks = Mathf.CeilToInt(slot.remainingWeeks / acceleration);
            weeks = Mathf.Max(1, calculatedWeeks);
            float progress = (slot.totalWeeks > 0f) ? (1f - (slot.remainingWeeks / slot.totalWeeks)) : 0f;
            progress = Mathf.Clamp01(progress);
            progressPercent = Mathf.RoundToInt(progress * 100f);
        }

        RefreshXPBar(row, slot, data, slotIdx);

        if (slotIdx > 0 && !slot.isUnlocked)
        {
            if (row.lockOverlay != null) row.lockOverlay.SetActive(true);
            // Show what studio star level is required using the MOD's progression
            int modStars = data.slots[0].stars;
            int reqStars = (slotIdx == 1) ? 3 : 5;
            if (row.headerText != null)
                row.headerText.text = $"Team {slotIdx + 1} — Locked (Studio ★{reqStars} required • current: ★{modStars})";
            if (row.unlockButtonText != null)
            {
                long cost = (slotIdx == 1) ? Slot2UnlockCost.Value : Slot3UnlockCost.Value;
                if (modStars >= reqStars)
                    row.unlockButtonText.text = $"Unlock Team {slotIdx + 1}  (${cost:N0})";
                else
                    row.unlockButtonText.text = $"Requires Studio ★{reqStars} (at ★{modStars})";
                if (row.unlockButton != null)
                    row.unlockButton.interactable = modStars >= reqStars;
            }
            return;
        }

        if (row.lockOverlay != null) row.lockOverlay.SetActive(false);
        SetActive(row.renameButton, true);

        bool isOrganic = IsOrganicStudio(studio);
        long upkeep = GetTeamUpkeep(studio, slotIdx);
        if (row.headerText != null)
        {
            mainScript main = studio.mS_ ?? GetMainScript();
            string upkeepText = main != null ? main.GetMoney(upkeep, showDollar: true) : upkeep.ToString();
            string headerBase = $"{slot.teamName} (★ {slot.stars}/5 | ⚡ {tSpeed}/{(isOrganic ? 10 : 4)}) | Upkeep: {upkeepText}";
            if (slot.traitID >= 0)
            {
                string tName = GetTagName(slot.traitID);
                row.headerText.text = $"{headerBase} | Tag: {tName}";
                row.headerText.color = new Color(1f, 0.84f, 0f, 1f);
                tooltip tt = row.headerText.GetComponent<tooltip>();
                if (tt == null) tt = row.headerText.gameObject.AddComponent<tooltip>();
                tt.c = GetTagTooltipText(slot.traitID);
            }
            else if (slot.stars >= 3)
            {
                row.headerText.text = $"{headerBase} | <color=#00ff00>[Choose Tag]</color>";
                row.headerText.color = new Color(0.7f, 1f, 0.7f, 1f);
                tooltip tt = row.headerText.GetComponent<tooltip>();
                if (tt != null) tt.c = "Click 'Choose Tag' button to select a team specialization tag.";
            }
            else
            {
                row.headerText.text = headerBase;
                row.headerText.color = new Color(0.9f, 0.9f, 0.9f, 1f);
                tooltip tt = row.headerText.GetComponent<tooltip>();
                if (tt != null) Object.Destroy(tt);
            }
        }

        if (slot.gameID != -1)
        {
            SetActive(row.statusText, false);
            SetActive(row.supportTargetButton, false);

            gameScript game = FindGameByID(gamesScript, slot.gameID);
            if (game != null)
            {
                SetActive(row.gameNameText, true);
                if (row.gameNameText != null) row.gameNameText.text = game.GetNameWithTag();
                SetProgressActive(row, true);

                float progress = (slot.totalWeeks > 0f) ? (1f - (slot.remainingWeeks / slot.totalWeeks)) : 0f;
                progress = Mathf.Clamp01(progress);

                if (row.progressFill != null)
                {
                    row.progressFill.fillAmount = progress;
                }

                if (row.progressText != null) { row.progressText.gameObject.SetActive(true); row.progressText.text = $"Progress: {Mathf.RoundToInt(progress * 100f)}%"; }
                if (row.weeksText != null) { row.weeksText.gameObject.SetActive(true); row.weeksText.text = weeks.ToString() + "w"; }
                if (row.discardButton != null) { row.discardButton.gameObject.SetActive(true); row.discardButton.interactable = true; }

                    // ── In-development layout ──────────────────────────────
                    // XP bar    (y=62) always visible
                    // Role      (y=36) grayed out — shows current role
                    // Actions   (y=10) Dev Report | Upgrade ★ | Close
                    // ──────────────────────────────────────────────────────
                    SetActive(row.statusText, false);
                    if (row.xpBarFill != null) row.xpBarFill.transform.parent.gameObject.SetActive(true);

                    // Role row — grayed, not clickable while developing
                    if (row.roleButton != null)
                    {
                        row.roleButton.gameObject.SetActive(true);
                        row.roleButton.interactable = false;
                        NormalizeRectTransform(row.roleButton.transform,
                            new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                            new Vector2(240f, 22f), new Vector2(0f, 36f));
                        if (row.roleText != null) row.roleText.text = "Role: " + GetRoleDisplayName(slot.role);
                    }
                    SetActive(row.supportTargetButton, false);

                    // Action row
                    if (slotIdx > 0)
                    {
                        // Dev Report (left) | Upgrade ★ (center) | Close (right)
                        if (row.infoButton != null)
                        {
                            row.infoButton.gameObject.SetActive(true);
                            row.infoButton.interactable = true;
                            NormalizeRectTransform(row.infoButton.transform,
                                new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                                new Vector2(110f, 24f), new Vector2(-135f, 10f));
                            Text infoBtnTxt = row.infoButton.transform.Find("Text")?.GetComponent<Text>();
                            if (infoBtnTxt != null) { infoBtnTxt.text = "Dev Report"; infoBtnTxt.fontSize = 10; }
                        }
                        if (row.upgradeStarsButton != null)
                        {
                            row.upgradeStarsButton.gameObject.SetActive(true);
                            NormalizeRectTransform(row.upgradeStarsButton.transform,
                                new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                                new Vector2(125f, 24f), new Vector2(5f, 10f));
                            UpdateUpgradeButtonState(row, data, slot, slotIdx, studio);
                        }
                        if (row.closeTeamButton != null)
                        {
                            row.closeTeamButton.gameObject.SetActive(true);
                            row.closeTeamButton.interactable = true;
                            NormalizeRectTransform(row.closeTeamButton.transform,
                                new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                                new Vector2(100f, 24f), new Vector2(127f, 10f));
                        }
                    }
                    else
                    {
                        // Slot 0: Dev Report centered, no upgrade button on card
                        SetActive(row.closeTeamButton, false);
                        SetActive(row.upgradeStarsButton, false);
                        if (row.infoButton != null)
                        {
                            row.infoButton.gameObject.SetActive(true);
                            row.infoButton.interactable = true;
                            NormalizeRectTransform(row.infoButton.transform,
                                new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                                new Vector2(240f, 24f), new Vector2(0f, 10f));
                            Text infoBtnTxt = row.infoButton.transform.Find("Text")?.GetComponent<Text>();
                            if (infoBtnTxt != null) { infoBtnTxt.text = "Dev Report"; infoBtnTxt.fontSize = 10; }
                        }
                    }

                if (row.gameTypeIcon != null) { row.gameTypeIcon.gameObject.SetActive(true); row.gameTypeIcon.enabled = true; row.gameTypeIcon.sprite = game.GetTypSprite(); }
                if (row.gameSizeIcon != null) { row.gameSizeIcon.gameObject.SetActive(true); row.gameSizeIcon.enabled = true; row.gameSizeIcon.sprite = game.GetSizeSprite(); }
                SetIPActive(row, true);
                if (row.ipPopularityText != null)
                {
                    mainScript main = studio.mS_ ?? GetMainScript();
                    if (main != null) row.ipPopularityText.text = main.Round(game.GetIpBekanntheit(), 1).ToString();
                    else row.ipPopularityText.text = game.GetIpBekanntheit().ToString("F1");
                }
                if (row.genreText != null)
                {
                    row.genreText.gameObject.SetActive(true);
                    string gText = game.GetGenreString();
                    if (game.subgenre != -1) gText += " / " + game.GetSubGenreString();
                    row.genreText.text = gText;
                }
                if (row.clockIcon != null)
                {
                    row.clockIcon.SetActive(true);
                    tooltip clockTooltip = row.clockIcon.GetComponent<tooltip>();
                    if (clockTooltip != null)
                    {
                        string text = tS.GetText(1948);
                        text = text.Replace("<NUM>", "<color=blue><b>" + weeks + "</b></color>");
                        clockTooltip.c = text;
                    }
                }
            }
            else
            {
                slot.gameID = -1;
            }
            // During development, hide the Choose Tag button
            SetActive(row.chooseTagButton, false);
        }

        if (slot.gameID == -1)
        {
            SetActive(row.gameNameText, false);
            SetProgressActive(row, false);
            SetActive(row.progressText, false);
            SetActive(row.weeksText, false);
            SetActive(row.discardButton, false);
            SetActive(row.infoButton, false);
            SetActive(row.gameTypeIcon, false);
            SetActive(row.gameSizeIcon, false);
            SetIPActive(row, false);
            SetActive(row.genreText, false);
            SetActive(row.clockIcon, false);

            // ── IDLE / SUPPORTING state ────────────────────────────────────
            // XP bar    (y=62) always visible
            // Row 1     (y=36) Role button (clickable)
            //   + if role=SUPPORT: Support target button stacked below at y=36
            //     (role button moves up to y=62 to avoid clash with XP bar)
            // Row 0     (y=10) Upgrade ★ [+ Close for slots 1-2]
            //   Hidden entirely if team is currently supporting another team
            // ─────────────────────────────────────────────────────────────────
            if (row.xpBarFill != null) row.xpBarFill.transform.parent.gameObject.SetActive(true);

            bool isSupporting = slot.helpingSlotIndex >= 0;

            // Status text
            if (row.statusText != null)
            {
                row.statusText.gameObject.SetActive(true);
                if (isSupporting)
                {
                    string targetName = GetSlotTeamName(studio, slot.helpingSlotIndex, data);
                    row.statusText.text = $"Supporting {targetName}";
                    row.statusText.color = new Color(0.5f, 0.8f, 1f, 1f);
                }
                else if (slotIdx == 0)
                {
                    row.statusText.text = "Main Team — Idle";
                    row.statusText.color = new Color(0.7f, 0.7f, 0.7f, 1f);
                }
                else
                {
                    row.statusText.text = "Idle";
                    row.statusText.color = new Color(0.7f, 0.7f, 0.7f, 1f);
                }
            }

            // Role button — always row 1 (y=36), clickable in idle/supporting states
            if (row.roleButton != null)
            {
                row.roleButton.gameObject.SetActive(true);
                row.roleButton.interactable = true;
                NormalizeRectTransform(row.roleButton.transform,
                    new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                    new Vector2(240f, 22f), new Vector2(0f, 36f));
                if (row.roleText != null) row.roleText.text = "Role: " + GetRoleDisplayName(slot.role);
            }

            // Support target cycling button (only when role=SUPPORT)
            if (row.supportTargetButton != null)
            {
                if (slot.role == ROLE_SUPPORT)
                {
                    // Stack above role button; role button shifts up to y=62
                    NormalizeRectTransform(row.roleButton.transform,
                        new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                        new Vector2(240f, 22f), new Vector2(0f, 60f));
                    row.supportTargetButton.gameObject.SetActive(true);
                    NormalizeRectTransform(row.supportTargetButton.transform,
                        new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                        new Vector2(240f, 22f), new Vector2(0f, 36f));
                    if (slot.supportTarget == -1)
                        row.supportTargetText.text = "Target: Auto (Closest)";
                    else
                    {
                        string tName = GetSlotTeamName(studio, slot.supportTarget, data);
                        row.supportTargetText.text = "Target: " + tName;
                    }
                    // XP bar moves up to y=84 to avoid clash
                    if (row.xpBarFill != null)
                        NormalizeRectTransform(row.xpBarFill.transform.parent,
                            new Vector2(0.05f, 0f), new Vector2(0.95f, 0f),
                            new Vector2(0.5f, 0f), new Vector2(0f, 14f), new Vector2(0f, 84f));
                }
                else
                {
                    row.supportTargetButton.gameObject.SetActive(false);
                    // Reset XP bar to default y=62
                    if (row.xpBarFill != null)
                        NormalizeRectTransform(row.xpBarFill.transform.parent,
                            new Vector2(0.05f, 0f), new Vector2(0.95f, 0f),
                            new Vector2(0.5f, 0f), new Vector2(0f, 14f), new Vector2(0f, 62f));
                }
            }

            // Action buttons (bottom row) — hidden while supporting
            if (isSupporting)
            {
                SetActive(row.upgradeStarsButton, false);
                SetActive(row.closeTeamButton, false);
            }
            else if (slotIdx == 0)
            {
                SetActive(row.upgradeStarsButton, false);
                SetActive(row.closeTeamButton, false);
            }
            else
            {
                if (row.upgradeStarsButton != null)
                {
                    row.upgradeStarsButton.gameObject.SetActive(true);
                    NormalizeRectTransform(row.upgradeStarsButton.transform,
                        new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                        new Vector2(170f, 24f), new Vector2(-68f, 10f));
                    UpdateUpgradeButtonState(row, data, slot, slotIdx, studio);
                }
                if (row.closeTeamButton != null)
                {
                    row.closeTeamButton.gameObject.SetActive(true);
                    NormalizeRectTransform(row.closeTeamButton.transform,
                        new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                        new Vector2(120f, 24f), new Vector2(92f, 10f));
                    row.closeTeamButton.interactable = true;
                }
            }

            // Choose Tag button — visible when unlocked, idle, ★3+, and no tag yet
            if (row.chooseTagButton != null)
            {
                if (!isSupporting && slot.traitID == -1 && slot.stars >= 3)
                {
                    row.chooseTagButton.gameObject.SetActive(true);
                    row.chooseTagButton.interactable = true;
                    NormalizeRectTransform(row.chooseTagButton.transform,
                        new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                        new Vector2(160f, 22f), new Vector2(0f, 112f));
                    if (row.chooseTagButtonText != null)
                        row.chooseTagButtonText.text = "Choose Tag";
                }
                else
                {
                    row.chooseTagButton.gameObject.SetActive(false);
                }
            }
        }
    }

    private static void RefreshXPBar(SlotUIRow row, SlotData slot, StudioSlotData data, int slotIdx)
    {
        if (row.xpBarFill == null || row.xpBarText == null) return;

        int currentXP;
        int requiredXP;
        string label;

        if (slotIdx == 0)
        {
            currentXP = data.studioXP;
            requiredXP = GetStudioXPRequired(slot.stars);
            label = "Studio";
        }
        else
        {
            currentXP = slot.teamXP;
            requiredXP = GetTeamXPRequired(slot.stars);
            label = "Team";
        }

        if (slot.stars >= 5)
        {
            row.xpBarFill.fillAmount = 1f;
            row.xpBarText.text = $"{label} ★ Maxed";
            return;
        }

        float fill = (requiredXP > 0) ? Mathf.Clamp01((float)currentXP / requiredXP) : 0f;
        row.xpBarFill.fillAmount = fill;
        row.xpBarText.text = $"{label} XP: {currentXP}/{requiredXP}";
    }

    private static void UpdateUpgradeButtonState(SlotUIRow row, StudioSlotData data, SlotData slot, int slotIdx, publisherScript studio)
    {
        if (row.upgradeStarsButton == null || row.upgradeStarsText == null) return;

        mainScript main = studio.mS_ ?? GetMainScript();
        if (main == null) return;

        Image btnImg = row.upgradeStarsButton.GetComponent<Image>();

        if (slotIdx == 0)
        {
            if (slot.stars >= 5)
            {
                row.upgradeStarsButton.interactable = false;
                row.upgradeStarsText.text = "Studio ★ Maxed";
                row.upgradeStarsText.fontSize = 11;
                if (btnImg != null) btnImg.color = new Color(0.25f, 0.25f, 0.25f, 0.8f);
                return;
            }
            int requiredXP = GetStudioXPRequired(slot.stars);
            long fee = GetStudioUpgradeFee(slot.stars);
            bool hasXP = data.studioXP >= requiredXP;
            bool hasMoney = main.money >= fee;

            if (!hasXP)
            {
                row.upgradeStarsButton.interactable = false;
                row.upgradeStarsText.text = $"{requiredXP - data.studioXP} XP to unlock upgrade";
                row.upgradeStarsText.fontSize = 10;
                if (btnImg != null) btnImg.color = new Color(0.22f, 0.22f, 0.22f, 0.8f);
            }
            else if (!hasMoney)
            {
                row.upgradeStarsButton.interactable = false;
                row.upgradeStarsText.text = $"Need ${(double)fee / 1000000.0:0.#}M to upgrade";
                row.upgradeStarsText.fontSize = 10;
                if (btnImg != null) btnImg.color = new Color(0.35f, 0.2f, 0.1f, 0.9f);
            }
            else
            {
                row.upgradeStarsButton.interactable = true;
                row.upgradeStarsText.text = $"Upgrade Studio ★{slot.stars + 1}  (${(double)fee / 1000000.0:0.#}M)";
                row.upgradeStarsText.fontSize = 11;
                if (btnImg != null) btnImg.color = new Color(0.1f, 0.45f, 0.15f, 1f);
            }
        }
        else
        {
            int studioStars = data.slots[0].stars;
            if (slot.stars >= 5)
            {
                row.upgradeStarsButton.interactable = false;
                row.upgradeStarsText.text = "★ Maxed";
                row.upgradeStarsText.fontSize = 11;
                if (btnImg != null) btnImg.color = new Color(0.25f, 0.25f, 0.25f, 0.8f);
                return;
            }
            if (slot.stars >= studioStars)
            {
                row.upgradeStarsButton.interactable = false;
                row.upgradeStarsText.text = $"Upgrade Studio to ★{slot.stars + 1} first";
                row.upgradeStarsText.fontSize = 10;
                if (btnImg != null) btnImg.color = new Color(0.22f, 0.22f, 0.22f, 0.8f);
                return;
            }
            int requiredXP = GetTeamXPRequired(slot.stars);
            long fee = GetTeamUpgradeFee(slot.stars);
            bool hasXP = slot.teamXP >= requiredXP;
            bool hasMoney = main.money >= fee;

            if (!hasXP)
            {
                row.upgradeStarsButton.interactable = false;
                row.upgradeStarsText.text = $"{requiredXP - slot.teamXP} XP to unlock upgrade";
                row.upgradeStarsText.fontSize = 10;
                if (btnImg != null) btnImg.color = new Color(0.22f, 0.22f, 0.22f, 0.8f);
            }
            else if (!hasMoney)
            {
                row.upgradeStarsButton.interactable = false;
                row.upgradeStarsText.text = $"Need ${(double)fee / 1000000.0:0.#}M to upgrade";
                row.upgradeStarsText.fontSize = 10;
                if (btnImg != null) btnImg.color = new Color(0.35f, 0.2f, 0.1f, 0.9f);
            }
            else
            {
                row.upgradeStarsButton.interactable = true;
                row.upgradeStarsText.text = $"Upgrade Team ★{slot.stars + 1}  (${(double)fee / 1000000.0:0.#}M)";
                row.upgradeStarsText.fontSize = 11;
                if (btnImg != null) btnImg.color = new Color(0.1f, 0.45f, 0.15f, 1f);
            }
        }
    }

    private static void TryOpenStudioDirector(publisherScript subsidiary)
    {
        GUI_Main gui = GetGuiMain();
        if (gui == null) return;

        System.Type sdType = null;
        foreach (var asm in System.AppDomain.CurrentDomain.GetAssemblies())
        {
            sdType = asm.GetType("StudioDirectorPlugin");
            if (sdType != null) break;
        }

        if (sdType != null)
        {
            var method = sdType.GetMethod("BeginDesignForSubsidiary",
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            if (method != null)
            {
                try
                {
                    method.Invoke(null, new object[] { subsidiary });
                    return;
                }
                catch (System.Exception ex)
                {
                    SubsidiaryTeamsPlugin.Log?.LogWarning($"StudioDirector.BeginDesignForSubsidiary failed: {ex.Message}");
                }
            }
        }

        gui.MessageBox("No game assigned to this team.\nUse the main development menu to direct a game to this studio.", closeMenu: false);
    }

    private static void SetActive(Component c, bool active)
    {
        if (c != null) c.gameObject.SetActive(active);
    }

    private static void SetActive(GameObject go, bool active)
    {
        if (go != null) go.SetActive(active);
    }


    private static void SetProgressActive(SlotUIRow row, bool active)
    {
        if (row.progressFill != null)
        {
            row.progressFill.gameObject.SetActive(active);
            if (row.progressFill.transform.parent != null && row.progressFill.transform.parent.gameObject != row.container)
            {
                row.progressFill.transform.parent.gameObject.SetActive(active);
            }
        }
    }

    private static void SetIPActive(SlotUIRow row, bool active)
    {
        if (row.ipPopularityText != null)
        {
            row.ipPopularityText.gameObject.SetActive(active);
            if (row.ipPopularityText.transform.parent != null && row.ipPopularityText.transform.parent.gameObject != row.container)
            {
                row.ipPopularityText.transform.parent.gameObject.SetActive(active);
            }
        }
    }
}
