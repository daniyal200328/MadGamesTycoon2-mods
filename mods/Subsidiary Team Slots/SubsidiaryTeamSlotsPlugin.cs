using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

[BepInPlugin("org.bepinex.plugins.subsidiaryteamslots", "Subsidiary Team Slots", "1.0.0")]
public class SubsidiaryTeamSlotsPlugin : BaseUnityPlugin
{
    public static SubsidiaryTeamSlotsPlugin Instance;
    public static ManualLogSource Log;

    // Slots Save Directory (Eggcode Games AppData folder)
    private static string SaveDir => Application.persistentDataPath;

    // Database & Serialization Classes
    [System.Serializable]
    public class SlotData
    {
        public int gameID = -1;
        public float remainingWeeks = 0f;
        public float totalWeeks = 0f;
        public bool isPlayerAssigned = false;

        // Extended properties
        public bool isUnlocked = false;
        public int unlockedYear = -1;
        public int unlockedMonth = -1;
        public string teamName = "";
        public int stars = 1; // 1 to 5 stars
        public int speed = 0; // 0 to 10 speed (or 0 to 4 for AI)
        public int helpingSlotIndex = -1; // -1 if not helping, else 0, 1, or 2
    }

    [System.Serializable]
    public class StudioSlotData
    {
        public int studioID;
        public int unlockedSlots = 1; // for backwards compatibility
        public SlotData[] slots = new SlotData[3] { new SlotData(), new SlotData(), new SlotData() };

        // Global cooldown for closing/reopening
        public int closedYear = -1;
        public int closedMonth = -1;
    }

    [System.Serializable]
    public class SlotSaveState
    {
        public List<StudioSlotData> studios = new List<StudioSlotData>();
    }

    public static SlotSaveState State = new SlotSaveState();
    public static int currentSaveSlot = -1;

    public class SlotStateCache
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
    }

    private static SlotStateCache[] cachedSlotStates = new SlotStateCache[3] { new SlotStateCache(), new SlotStateCache(), new SlotStateCache() };

    // Per-slot UI row containers (slots 1, 2, and 3 inside the side panel)
    private static GameObject sidePanel;
    public static Menu_Stats_Tochterfirma_Main activeMenuInstance;
    private static SlotUIRow slotRow1;
    private static SlotUIRow slotRow2;
    private static SlotUIRow slotRow3;

    // Reflection helpers for private Menu_Stats_Tochterfirma_Main fields
    public static readonly FieldInfo f_pS = AccessTools.Field(typeof(Menu_Stats_Tochterfirma_Main), "pS_");
    public static readonly FieldInfo f_guiMain = AccessTools.Field(typeof(Menu_Stats_Tochterfirma_Main), "guiMain_");
    public static readonly FieldInfo f_tS = AccessTools.Field(typeof(Menu_Stats_Tochterfirma_Main), "tS_");
    public static readonly FieldInfo f_nextGame = AccessTools.Field(typeof(Menu_Stats_Tochterfirma_Main), "nextGame_");

    // Mod Integration State Flags
    public static bool isBypassingPrefix = false;
    public static bool isCreatingAutonomousSlot = false;
    public static int targetSlotIndex = -1;
    public static bool isDesigningForSlot = false;
    public static int selectedSlotIndex = -1;
    public static bool isCreatingPlayerSlotProject = false;
    public static int slotToDiscard = -1;

    // Duration capture for player-assigned projects
    // Set by DynamicSubsidiaryTimeline's SetAsGameInDevelopmentNPC postfix (via our hook),
    // then consumed by our GameScript_SetAsGameInDevelopmentNPC postfix.
    public static float pendingPlayerSlotDuration = -1f;
    public static gameScript currentInitializingGame = null;

    // GUI Slot Picker State
    public static bool showSlotPicker = false;
    public static Rect pickerRect;
    public static publisherScript pickerTarget;

    // GUI Rename Window State
    public static bool showRenameWindow = false;
    public static Rect renameRect = new Rect(Screen.width / 2f - 150f, Screen.height / 2f - 75f, 300f, 150f);
    public static int renameSlotIndex = -1;
    public static int renameStudioID = -1;
    public static string renameInputString = "";

    // UI Stretched State tracker
    public static HashSet<int> shiftedMenuInstances = new HashSet<int>();

    // Track which slot UI was last built for which studio, to avoid stale references
    private static int lastBuiltForStudio = -1;

    /// <summary>
    /// Clears cached subsidiary slot UI so the next UpdateData rebuilds week counts.
    /// </summary>
    public static void InvalidateStudioDevUI()
    {
        lastBuiltForStudio = -1;
        shiftedMenuInstances.Clear();
    }

    private static games cachedGames;
    private static mainScript cachedMain;
    private static GUI_Main cachedGui;
    private static sfxScript cachedSfx;
    private static textScript cachedText;

    public static games GetGamesScript()
    {
        if (cachedGames == null) cachedGames = UnityEngine.Object.FindObjectOfType<games>();
        return cachedGames;
    }

    public static mainScript GetMainScript()
    {
        if (cachedMain == null) cachedMain = UnityEngine.Object.FindObjectOfType<mainScript>();
        return cachedMain;
    }

    public static GUI_Main GetGuiMain()
    {
        if (cachedGui == null) cachedGui = UnityEngine.Object.FindObjectOfType<GUI_Main>();
        return cachedGui;
    }

    public static sfxScript GetSfxScript()
    {
        if (cachedSfx == null) cachedSfx = UnityEngine.Object.FindObjectOfType<sfxScript>();
        return cachedSfx;
    }

    public static textScript GetTextScript()
    {
        if (cachedText == null)
        {
            mainScript main = GetMainScript();
            if (main != null) cachedText = main.tS_;
            if (cachedText == null) cachedText = UnityEngine.Object.FindObjectOfType<textScript>();
        }
        return cachedText;
    }

    private void Awake()
    {
        Instance = this;
        Log = Logger;
        pickerRect = new Rect(Screen.width / 2f - 200f, Screen.height / 2f - 150f, 400f, 300f);
        new Harmony("org.bepinex.plugins.subsidiaryteamslots").PatchAll();
        Log.LogInfo("Subsidiary Team Slots mod loaded successfully.");
    }



    private void OnGUI()
    {
        if (showSlotPicker && pickerTarget != null)
        {
            GUI.depth = -1005;
            pickerRect = GUI.Window(9998, pickerRect, DrawSlotPicker, "Select Development Slot", GUI.skin.window);
        }
        if (showRenameWindow)
        {
            GUI.depth = -1006;
            renameRect = GUI.Window(9999, renameRect, DrawRenameWindow, "Rename Team", GUI.skin.window);
        }
    }

    private void DrawRenameWindow(int windowID)
    {
        GUILayout.Space(10);
        GUILayout.Label("Enter new team name:", GUI.skin.label);
        renameInputString = GUILayout.TextField(renameInputString, 30);
        GUILayout.Space(15);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            if (!string.IsNullOrEmpty(renameInputString))
            {
                StudioSlotData data = GetStudioSlotData(renameStudioID);
                data.slots[renameSlotIndex].teamName = renameInputString;
                SaveState(currentSaveSlot);
                showRenameWindow = false;
                lastBuiltForStudio = -1; // Force UI rebuild

                Menu_Stats_Tochterfirma_Main menu = UnityEngine.Object.FindObjectOfType<Menu_Stats_Tochterfirma_Main>();
                if (menu != null && menu.gameObject.activeInHierarchy)
                {
                    menu.UpdateData();
                }
            }
        }
        if (GUILayout.Button("Cancel"))
        {
            showRenameWindow = false;
        }
        GUILayout.EndHorizontal();
        GUI.DragWindow();
    }

    private void DrawSlotPicker(int windowID)
    {
        if (pickerTarget == null)
        {
            showSlotPicker = false;
            return;
        }

        GUILayout.Space(10);
        GUILayout.Label("Select a team slot to assign the project:", GUI.skin.label);
        GUILayout.Space(10);

        StudioSlotData data = GetStudioSlotData(pickerTarget.myID);
        games gamesScript = pickerTarget.games_ != null ? pickerTarget.games_ : UnityEngine.Object.FindObjectOfType<games>();

        for (int i = 0; i < 3; i++)
        {
            SlotData slot = data.slots[i];
            string tName = string.IsNullOrEmpty(slot.teamName) ? $"Team {i + 1}" : slot.teamName;

            if (!slot.isUnlocked)
            {
                // Slot is locked
                long cost = (i == 1) ? 15000000L : 60000000L;
                int reqStars = (i == 1) ? 3 : 5;
                string costText = "$" + cost.ToString("N0");
                GUILayout.BeginHorizontal();
                GUILayout.Label($"{tName}: Locked (Requires {reqStars} Stars)", GUILayout.Width(220f));
                if (GUILayout.Button($"Unlock ({costText})"))
                {
                    TryPurchaseSlot(pickerTarget, i);
                }
                GUILayout.EndHorizontal();
            }
            else
            {
                // Slot is unlocked
                if (slot.gameID != -1)
                {
                    gameScript game = FindGameByID(gamesScript, slot.gameID);
                    string gameName = game != null ? game.myName : "Busy";
                    GUI.enabled = false;
                    GUILayout.Button($"{tName}: Busy - {gameName} ({Mathf.CeilToInt(slot.remainingWeeks)} weeks remaining)");
                    GUI.enabled = true;
                }
                else
                {
                    if (GUILayout.Button($"{tName}: Idle - Click to Assign"))
                    {
                        showSlotPicker = false;
                        selectedSlotIndex = i;
                        isDesigningForSlot = true;

                        // Re-trigger project design menu
                        SubsidiaryProjectDirectorPlugin.BeginDesignForSubsidiary(pickerTarget);
                    }
                }
            }
            GUILayout.Space(5);
        }

        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Cancel"))
        {
            showSlotPicker = false;
            pickerTarget = null;
        }
        GUI.DragWindow();
    }

    public static StudioSlotData GetStudioSlotData(int studioID)
    {
        if (State == null) State = new SlotSaveState();
        if (State.studios == null) State.studios = new List<StudioSlotData>();

        StudioSlotData data = State.studios.FirstOrDefault(s => s.studioID == studioID);
        if (data == null)
        {
            data = new StudioSlotData { studioID = studioID, unlockedSlots = 1 };
            data.slots = new SlotData[3] { new SlotData(), new SlotData(), new SlotData() };
            data.slots[0].isUnlocked = true;
            data.slots[0].teamName = "Team 1";
            data.slots[0].stars = 5;
            data.slots[0].speed = 10;
            data.slots[0].helpingSlotIndex = -1;
            data.slots[1].teamName = "Team 2";
            data.slots[1].helpingSlotIndex = -1;
            data.slots[2].teamName = "Team 3";
            data.slots[2].helpingSlotIndex = -1;
            State.studios.Add(data);
        }
        else
        {
            // Backwards compatibility migration & robust validation
            if (data.slots == null || data.slots.Length < 3)
            {
                SlotData[] oldSlots = data.slots;
                data.slots = new SlotData[3] { new SlotData(), new SlotData(), new SlotData() };
                if (oldSlots != null)
                {
                    for (int i = 0; i < oldSlots.Length && i < 3; i++)
                    {
                        if (oldSlots[i] != null) data.slots[i] = oldSlots[i];
                    }
                }
            }

            for (int i = 0; i < 3; i++)
            {
                if (data.slots[i] == null)
                {
                    data.slots[i] = new SlotData();
                }
            }

            if (string.IsNullOrEmpty(data.slots[0].teamName)) data.slots[0].teamName = "Team 1";
            if (string.IsNullOrEmpty(data.slots[1].teamName)) data.slots[1].teamName = "Team 2";
            if (string.IsNullOrEmpty(data.slots[2].teamName)) data.slots[2].teamName = "Team 3";

            data.slots[0].isUnlocked = true;
            for (int i = 0; i < 3; i++)
            {
                // If migration missed, initialize helpingSlotIndex
                if (data.slots[i].helpingSlotIndex < -1 || data.slots[i].helpingSlotIndex >= 3)
                {
                    data.slots[i].helpingSlotIndex = -1;
                }
            }
            for (int i = 1; i < 3; i++)
            {
                if (i < data.unlockedSlots)
                {
                    data.slots[i].isUnlocked = true;
                }
            }
        }
        return data;
    }

    public static long GetTeamUpkeep(publisherScript studio, int slotIdx)
    {
        if (studio == null) return 0L;
        StudioSlotData data = GetStudioSlotData(studio.myID);
        if (slotIdx >= data.unlockedSlots) return 0L;
        SlotData slot = data.slots[slotIdx];
        if (!slot.isUnlocked) return 0L;

        try
        {
            System.Type timelineClass = System.Type.GetType("DynamicSubsidiaryTimelinePlugin, DynamicSubsidiaryTimeline");
            if (timelineClass == null)
            {
                foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
                {
                    timelineClass = assembly.GetType("DynamicSubsidiaryTimelinePlugin");
                    if (timelineClass != null) break;
                }
            }
            if (timelineClass != null)
            {
                var method = AccessTools.Method(timelineClass, "CalculateSingleSlotUpkeep");
                if (method != null)
                {
                    return (long)method.Invoke(null, new object[] { studio, slotIdx });
                }
            }
        }
        catch (System.Exception ex)
        {
            Log?.LogWarning("Failed to call CalculateSingleSlotUpkeep: " + ex);
        }

        // Fallback to vanilla/slots division
        return studio.GetVerwaltungskosten() / data.unlockedSlots;
    }

    private static string GetSavePath(int slot)
    {
        return Path.Combine(SaveDir, $"SaveGame_{slot}_slots.json");
    }

    public static void LoadState(int slot)
    {
        currentSaveSlot = slot;
        shiftedMenuInstances.Clear();
        lastBuiltForStudio = -1;
        string path = GetSavePath(slot);
        try
        {
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                State = JsonUtility.FromJson<SlotSaveState>(json);
                if (State == null) State = new SlotSaveState();
                if (State.studios == null) State.studios = new List<StudioSlotData>();
                Log?.LogInfo($"Loaded slots state for save slot {slot}. Total studios: {State.studios.Count}");
            }
            else
            {
                State = new SlotSaveState();
                Log?.LogInfo($"No slots save file found for slot {slot}. Initializing empty state.");
            }
        }
        catch (System.Exception ex)
        {
            Log?.LogError($"Failed to load slots state: {ex}");
            State = new SlotSaveState();
        }
    }

    public static void SaveState(int slot)
    {
        if (slot < 0) return;
        string path = GetSavePath(slot);
        try
        {
            if (State == null) State = new SlotSaveState();
            string json = JsonUtility.ToJson(State, true);
            File.WriteAllText(path, json);
            Log?.LogInfo($"Saved slots state for save slot {slot}.");
        }
        catch (System.Exception ex)
        {
            Log?.LogError($"Failed to save slots state: {ex}");
        }
    }

    public static gameScript FindGameByID(games gamesScript, int gameID)
    {
        if (gamesScript == null || gamesScript.arrayGamesScripts == null) return null;
        for (int i = 0; i < gamesScript.arrayGamesScripts.Length; i++)
        {
            if (gamesScript.arrayGamesScripts[i] != null && gamesScript.arrayGamesScripts[i].myID == gameID)
            {
                return gamesScript.arrayGamesScripts[i];
            }
        }
        return null;
    }

    /// <summary>Returns true if the given gameID is tracked in ANY slot for ANY studio (other than slotToIgnore).</summary>
    public static bool IsGameTrackedInOtherSlot(int studioID, int gameID, int slotToIgnore = -1)
    {
        if (State == null) return false;
        StudioSlotData data = State.studios?.FirstOrDefault(s => s.studioID == studioID);
        if (data == null) return false;
        for (int i = 0; i < 3; i++)
        {
            if (i == slotToIgnore) continue;
            if (data.slots[i].gameID == gameID) return true;
        }
        return false;
    }

    public static publisherScript FindStudioByID(int studioID)
    {
        publisherScript[] allStudios = UnityEngine.Object.FindObjectsOfType<publisherScript>();
        if (allStudios != null)
        {
            foreach (publisherScript p in allStudios)
            {
                if (p != null && p.myID == studioID)
                    return p;
            }
        }
        return null;
    }

    public static float GetParallelDurationMultiplier(int activeCount)
    {
        if (activeCount == 2) return 1.74f;
        if (activeCount == 3) return 2.22f;
        return 1.00f;
    }

    // Weekly Ticking logic
    public static void TickActiveProjects(publisherScript studio)
    {
        if (studio == null || studio.Geschlossen() || studio.TochterfirmaGeschlossen()) return;
        if (!studio.developer) return;

        StudioSlotData data = GetStudioSlotData(studio.myID);
        games gamesScript = studio.games_ != null ? studio.games_ : UnityEngine.Object.FindObjectOfType<games>();
        if (gamesScript == null) return;

        // Clean stale projects (game was destroyed externally)
        for (int i = 0; i < 3; i++)
        {
            if (i >= data.unlockedSlots)
            {
                data.slots[i].gameID = -1;
                data.slots[i].helpingSlotIndex = -1;
                continue;
            }
            if (data.slots[i].gameID != -1 && FindGameByID(gamesScript, data.slots[i].gameID) == null)
            {
                Log?.LogInfo($"Slot {i}: game {data.slots[i].gameID} no longer exists, clearing.");
                data.slots[i].gameID = -1;
                data.slots[i].remainingWeeks = 0f;
                data.slots[i].totalWeeks = 0f;
            }
        }

        // Start autonomous projects for idle unlocked slots (not helping and no game)
        for (int i = 0; i < data.unlockedSlots; i++)
        {
            if (data.slots[i].gameID == -1 && data.slots[i].helpingSlotIndex == -1 && !data.slots[i].isPlayerAssigned)
            {
                StartAutonomousProject(studio, i);
            }
        }

        // Tick remaining raw weeks down by acceleration factor
        for (int i = 0; i < data.unlockedSlots; i++)
        {
            SlotData slot = data.slots[i];
            if (slot.gameID == -1) continue;

            gameScript game = FindGameByID(gamesScript, slot.gameID);
            if (game == null)
            {
                slot.gameID = -1;
                continue;
            }

            // Get Acceleration Factor from DST
            float acceleration = GetAccelerationFactor(studio, i);

            slot.remainingWeeks -= acceleration;

            if (slot.remainingWeeks <= 0f)
            {
                ReleaseSlotProject(studio, game, slot);
            }
        }
    }

    private static void StartAutonomousProject(publisherScript studio, int slotIndex)
    {
        try
        {
            isBypassingPrefix = true;
            isCreatingAutonomousSlot = true;
            targetSlotIndex = slotIndex;

            // Force vanilla to think we have no game and should start one
            studio.newGameInWeeks = 0;
            studio.newGameInWeeksORG = 0;

            studio.CreateNewGame2(false, true);
        }
        catch (System.Exception ex)
        {
            Log?.LogError($"Failed to start autonomous project for slot {slotIndex}: {ex}");
        }
        finally
        {
            isBypassingPrefix = false;
            isCreatingAutonomousSlot = false;
            targetSlotIndex = -1;

            // Lock weeks so vanilla doesn't tick down autonomously in UpdateWeek
            studio.newGameInWeeks = 9999;
            studio.newGameInWeeksORG = 9999;
        }
    }

    private static void ReleaseSlotProject(publisherScript studio, gameScript game, SlotData slot)
    {
        // When slot project is released, we must clear this slot's gameID, remainingWeeks, etc.
        // We also need to clear helpingSlotIndex for any other slot that was helping this slot!
        int releasedSlotIdx = -1;
        StudioSlotData data = GetStudioSlotData(studio.myID);
        for (int i = 0; i < 3; i++)
        {
            if (data.slots[i] == slot)
            {
                releasedSlotIdx = i;
                break;
            }
        }

        slot.gameID = -1;
        slot.remainingWeeks = 0f;
        slot.totalWeeks = 0f;
        slot.isPlayerAssigned = false;

        if (releasedSlotIdx != -1)
        {
            for (int i = 0; i < 3; i++)
            {
                if (data.slots[i].helpingSlotIndex == releasedSlotIdx)
                {
                    data.slots[i].helpingSlotIndex = -1; // stop helping
                }
            }
        }

        game.inDevelopment = false;

        if (studio.IsTochterfirma() && studio.IsMyTochterfirma())
        {
            int reviewTotal = game.reviewTotal;
            if (game.reviewTotal <= 0)
            {
                game.CalcReview(entwicklungsbericht: true);
                reviewTotal = game.reviewTotal;
                game.ClearReview();
            }

            if (!studio.tf_autoRelease || (studio.tf_autoRelease && studio.tf_autoReleaseVal != 0 && reviewTotal >= studio.tf_autoReleaseVal * 10))
            {
                // Invoke private coroutine via reflection
                MethodInfo waitCorMethod = AccessTools.Method(typeof(publisherScript), "iWaitTochterfirmaReleaseGame");
                if (waitCorMethod != null)
                {
                    System.Collections.IEnumerator cor = waitCorMethod.Invoke(studio, new object[] { game, studio }) as System.Collections.IEnumerator;
                    studio.StartCoroutine(cor);
                }
            }
            else
            {
                if (game.reviewTotal <= 0)
                {
                    game.CalcReview(entwicklungsbericht: false);
                    studio.reviewText_.GetReviewText(game);
                }
                else
                {
                    game.date_year = studio.mS_.year;
                    game.date_month = studio.mS_.month;
                    studio.reviewText_.GetReviewText(game);
                }

                studio.ReleaseTheGame(game, forceContractGame: false, ignoreTochterfirma: true);
                studio.SetGameOnMarket(game);

                if (studio.tf_autoGamePass && studio.gpS_.gamePass_aktiv && game.CanBeInGamePass())
                {
                    studio.gpS_.GAMEPASS_AddGame(game, updateGamesAmount: true);
                }
            }
        }
    }

    public static void TryPurchaseSlot(publisherScript studio, int slotIdx)
    {
        long cost = (slotIdx == 1) ? 15000000L : 60000000L;
        int reqStars = (slotIdx == 1) ? 3 : 5;

        mainScript main = studio.mS_ != null ? studio.mS_ : UnityEngine.Object.FindObjectOfType<mainScript>();
        GUI_Main gui = UnityEngine.Object.FindObjectOfType<GUI_Main>();

        if (main == null || gui == null) return;

        int starsCount = studio.GetStarsAmount();
        if (starsCount < reqStars)
        {
            gui.MessageBox($"Requires a {reqStars}-Star Studio to unlock this development team!", closeMenu: false);
            return;
        }

        // Check global closed cooldown (lockout for 12 months)
        StudioSlotData data = GetStudioSlotData(studio.myID);
        if (data.closedYear != -1 && data.closedMonth != -1)
        {
            int monthsSinceClose = (main.year - data.closedYear) * 12 + (main.month - data.closedMonth);
            if (monthsSinceClose < 12)
            {
                gui.MessageBox($"Cannot open any team! Global cooldown active: a team was closed {monthsSinceClose} months ago (requires 12 months).", closeMenu: false);
                return;
            }
        }

        if (main.money < cost)
        {
            gui.ShowNoMoney();
            return;
        }

        main.Pay(cost, 29);
        data.slots[slotIdx].isUnlocked = true;
        data.slots[slotIdx].unlockedYear = main.year;
        data.slots[slotIdx].unlockedMonth = main.month;
        data.slots[slotIdx].stars = 1;
        data.slots[slotIdx].speed = 0;
        data.unlockedSlots = Mathf.Max(data.unlockedSlots, slotIdx + 1); // backward compatibility
        SaveState(currentSaveSlot);

        sfxScript sfx = UnityEngine.Object.FindObjectOfType<sfxScript>();
        if (sfx != null)
        {
            sfx.PlaySound(22, true); // Upgrade sound
        }

        gui.MessageBox($"Successfully unlocked Team Slot {slotIdx + 1}!", closeMenu: false);

        // Force UI rebuild
        lastBuiltForStudio = -1;
        shiftedMenuInstances.Clear();
        Menu_Stats_Tochterfirma_Main menu = UnityEngine.Object.FindObjectOfType<Menu_Stats_Tochterfirma_Main>();
        if (menu != null && menu.gameObject.activeInHierarchy)
        {
            menu.UpdateData();
        }
    }

    public static void TryCloseSlot(publisherScript studio, int slotIdx)
    {
        if (slotIdx < 1 || slotIdx > 2) return;

        mainScript main = studio.mS_ != null ? studio.mS_ : UnityEngine.Object.FindObjectOfType<mainScript>();
        GUI_Main gui = UnityEngine.Object.FindObjectOfType<GUI_Main>();

        if (main == null || gui == null) return;

        StudioSlotData data = GetStudioSlotData(studio.myID);
        SlotData slot = data.slots[slotIdx];

        int monthsSinceOpen = (main.year - slot.unlockedYear) * 12 + (main.month - slot.unlockedMonth);
        if (monthsSinceOpen < 12 && slot.unlockedYear != -1)
        {
            gui.MessageBox($"Cannot close this team yet! It must remain open for at least 12 months (currently open for {monthsSinceOpen} months).", closeMenu: false);
            return;
        }

        if (slot.gameID != -1)
        {
            games gamesScript = studio.games_ != null ? studio.games_ : UnityEngine.Object.FindObjectOfType<games>();
            gameScript game = FindGameByID(gamesScript, slot.gameID);
            if (game != null)
            {
                game.gameObject.tag = "GameRemoved";
                UnityEngine.Object.Destroy(game.gameObject);
                Log?.LogInfo($"Programmatically destroyed game '{game.myName}' (ID={game.myID}) to close slot {slotIdx}");
            }
        }

        // Close it
        slot.isUnlocked = false;
        slot.stars = 1;
        slot.speed = 0;
        slot.gameID = -1;
        slot.remainingWeeks = 0f;
        slot.totalWeeks = 0f;
        slot.isPlayerAssigned = false;

        data.closedYear = main.year;
        data.closedMonth = main.month;

        SaveState(currentSaveSlot);

        sfxScript sfx = UnityEngine.Object.FindObjectOfType<sfxScript>();
        if (sfx != null)
        {
            sfx.PlaySound(22, true); // sound
        }

        gui.MessageBox($"Successfully closed {slot.teamName}! Global 12-month cooldown for reopening has started.", closeMenu: false);

        // Force UI rebuild
        lastBuiltForStudio = -1;
        shiftedMenuInstances.Clear();
        Menu_Stats_Tochterfirma_Main menu = UnityEngine.Object.FindObjectOfType<Menu_Stats_Tochterfirma_Main>();
        if (menu != null && menu.gameObject.activeInHierarchy)
        {
            menu.UpdateData();
        }
    }

    public static void TryUpgradeTeamStars(publisherScript studio, int slotIdx)
    {
        if (slotIdx < 1 || slotIdx > 2) return;

        mainScript main = studio.mS_ != null ? studio.mS_ : UnityEngine.Object.FindObjectOfType<mainScript>();
        GUI_Main gui = UnityEngine.Object.FindObjectOfType<GUI_Main>();

        if (main == null || gui == null) return;

        StudioSlotData data = GetStudioSlotData(studio.myID);
        SlotData slot = data.slots[slotIdx];

        int maxStars = studio.GetStarsAmount();
        if (slot.stars >= maxStars)
        {
            gui.MessageBox($"Cannot upgrade team stars beyond the studio's star level ({maxStars} Stars)!", closeMenu: false);
            return;
        }

        long[] starCosts = new long[] { 0L, 2000000L, 5000000L, 12000000L, 25000000L };
        long cost = starCosts[slot.stars];

        if (main.money < cost)
        {
            gui.ShowNoMoney();
            return;
        }

        // Capture pre-upgrade slot-specific values BEFORE modifying slot.stars.
        // DST's RecalculateActiveProjectTimeline reads its static preUpgradeStars field
        // (set -1 by default) to determine the old star level. Without setting this first,
        // DST falls back to studio.GetStarsAmount() which is already the post-upgrade
        // value, making pre==post and producing zero timeline reduction.
        int preStars = slot.stars;
        int preSpeed = slot.speed;

        main.Pay(cost, 29);
        slot.stars++;
        SaveState(currentSaveSlot);

        // Recalculate duration instantly
        try
        {
            System.Type timelineClass = System.Type.GetType("DynamicSubsidiaryTimelinePlugin, DynamicSubsidiaryTimeline");
            if (timelineClass == null)
            {
                foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
                {
                    timelineClass = assembly.GetType("DynamicSubsidiaryTimelinePlugin");
                    if (timelineClass != null) break;
                }
            }
            if (timelineClass != null)
            {
                // Inject pre-upgrade slot-specific stars into DST's tracking fields
                // so DST knows what the old star level was for this slot.
                var preStarsField = AccessTools.Field(timelineClass, "preUpgradeStars");
                var preSpeedField = AccessTools.Field(timelineClass, "preUpgradeSpeed");
                var preStudioField = AccessTools.Field(timelineClass, "preUpgradeIsStudioLevel");
                var preSlotField = AccessTools.Field(timelineClass, "preUpgradeSlotIndex");
                if (preStarsField != null) preStarsField.SetValue(null, preStars);
                if (preSpeedField != null) preSpeedField.SetValue(null, preSpeed);
                if (preStudioField != null) preStudioField.SetValue(null, false);
                if (preSlotField != null) preSlotField.SetValue(null, slotIdx);

                var method = AccessTools.Method(timelineClass, "RecalculateActiveProjectTimeline");
                if (method != null) method.Invoke(null, new object[] { studio });
            }
        }
        catch (System.Exception ex)
        {
            Log?.LogWarning("Failed to trigger RecalculateActiveProjectTimeline: " + ex);
        }

        sfxScript sfx = UnityEngine.Object.FindObjectOfType<sfxScript>();
        if (sfx != null) sfx.PlaySound(22, true);

        gui.MessageBox($"Upgraded {slot.teamName} to {slot.stars} Stars!", closeMenu: false);

        lastBuiltForStudio = -1;
        shiftedMenuInstances.Clear();
        Menu_Stats_Tochterfirma_Main menu = UnityEngine.Object.FindObjectOfType<Menu_Stats_Tochterfirma_Main>();
        if (menu != null && menu.gameObject.activeInHierarchy)
        {
            menu.UpdateData();
        }
    }

    public static void TryUpgradeTeamSpeed(publisherScript studio, int slotIdx)
    {
        if (slotIdx < 0 || slotIdx > 2) return;

        mainScript main = studio.mS_ != null ? studio.mS_ : UnityEngine.Object.FindObjectOfType<mainScript>();
        GUI_Main gui = UnityEngine.Object.FindObjectOfType<GUI_Main>();

        if (main == null || gui == null) return;

        bool isOrganic = IsOrganicStudio(studio);

        if (slotIdx == 0)
        {
            int maxSpeed = isOrganic ? 10 : 4;
            if (studio.developmentSpeed >= maxSpeed)
            {
                gui.MessageBox($"Studio speed is already at max level ({maxSpeed})!", closeMenu: false);
                return;
            }

            long cost = isOrganic ? (500000L * (studio.developmentSpeed + 1)) : (1500000L * (studio.developmentSpeed + 1));

            if (main.money < cost)
            {
                gui.ShowNoMoney();
                return;
            }

            int preStars = studio.GetStarsAmount();
            int preSpeed = studio.developmentSpeed;

            main.Pay(cost, 29);
            studio.developmentSpeed++;
            SaveState(currentSaveSlot);

            try
            {
                System.Type timelineClass = System.Type.GetType("DynamicSubsidiaryTimelinePlugin, DynamicSubsidiaryTimeline");
                if (timelineClass == null)
                {
                    foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
                    {
                        timelineClass = assembly.GetType("DynamicSubsidiaryTimelinePlugin");
                        if (timelineClass != null) break;
                    }
                }
                if (timelineClass != null)
                {
                    var preStarsField = AccessTools.Field(timelineClass, "preUpgradeStars");
                    var preSpeedField = AccessTools.Field(timelineClass, "preUpgradeSpeed");
                    var preStudioField = AccessTools.Field(timelineClass, "preUpgradeIsStudioLevel");
                    var preSlotField = AccessTools.Field(timelineClass, "preUpgradeSlotIndex");
                    if (preStarsField != null) preStarsField.SetValue(null, preStars);
                    if (preSpeedField != null) preSpeedField.SetValue(null, preSpeed);
                    if (preStudioField != null) preStudioField.SetValue(null, false);
                    if (preSlotField != null) preSlotField.SetValue(null, slotIdx);

                    var method = AccessTools.Method(timelineClass, "RecalculateActiveProjectTimeline");
                    if (method != null) method.Invoke(null, new object[] { studio });
                }
            }
            catch (System.Exception ex)
            {
                Log?.LogWarning("Failed to trigger RecalculateActiveProjectTimeline: " + ex);
            }

            sfxScript sfx = UnityEngine.Object.FindObjectOfType<sfxScript>();
            if (sfx != null) sfx.PlaySound(22, true);

            gui.MessageBox($"Upgraded main team speed to level {studio.developmentSpeed}!", closeMenu: false);
        }
        else
        {
            StudioSlotData data = GetStudioSlotData(studio.myID);
            SlotData slot = data.slots[slotIdx];

            int maxSpeed = studio.developmentSpeed;
            if (slot.speed >= maxSpeed)
            {
                gui.MessageBox($"Cannot upgrade team speed beyond the studio's speed level (Level {maxSpeed})!", closeMenu: false);
                return;
            }

            long cost = isOrganic ? (500000L * (slot.speed + 1)) : (1500000L * (slot.speed + 1));

            if (main.money < cost)
            {
                gui.ShowNoMoney();
                return;
            }

            int preStars = slot.stars;
            int preSpeed = slot.speed;

            main.Pay(cost, 29);
            slot.speed++;
            SaveState(currentSaveSlot);

            try
            {
                System.Type timelineClass = System.Type.GetType("DynamicSubsidiaryTimelinePlugin, DynamicSubsidiaryTimeline");
                if (timelineClass == null)
                {
                    foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
                    {
                        timelineClass = assembly.GetType("DynamicSubsidiaryTimelinePlugin");
                        if (timelineClass != null) break;
                    }
                }
                if (timelineClass != null)
                {
                    var preStarsField = AccessTools.Field(timelineClass, "preUpgradeStars");
                    var preSpeedField = AccessTools.Field(timelineClass, "preUpgradeSpeed");
                    var preStudioField = AccessTools.Field(timelineClass, "preUpgradeIsStudioLevel");
                    var preSlotField = AccessTools.Field(timelineClass, "preUpgradeSlotIndex");
                    if (preStarsField != null) preStarsField.SetValue(null, preStars);
                    if (preSpeedField != null) preSpeedField.SetValue(null, preSpeed);
                    if (preStudioField != null) preStudioField.SetValue(null, false);
                    if (preSlotField != null) preSlotField.SetValue(null, slotIdx);

                    var method = AccessTools.Method(timelineClass, "RecalculateActiveProjectTimeline");
                    if (method != null) method.Invoke(null, new object[] { studio });
                }
            }
            catch (System.Exception ex)
            {
                Log?.LogWarning("Failed to trigger RecalculateActiveProjectTimeline: " + ex);
            }

            sfxScript sfx = UnityEngine.Object.FindObjectOfType<sfxScript>();
            if (sfx != null) sfx.PlaySound(22, true);

            gui.MessageBox($"Upgraded {slot.teamName} speed to level {slot.speed}!", closeMenu: false);
        }

        lastBuiltForStudio = -1;
        shiftedMenuInstances.Clear();
        Menu_Stats_Tochterfirma_Main menu = UnityEngine.Object.FindObjectOfType<Menu_Stats_Tochterfirma_Main>();
        if (menu != null && menu.gameObject.activeInHierarchy)
        {
            menu.UpdateData();
        }
    }

    public static float GetAccelerationFactor(publisherScript studio, int slotIdx)
    {
        if (studio == null) return 1f;
        try
        {
            System.Type timelineClass = System.Type.GetType("DynamicSubsidiaryTimelinePlugin, DynamicSubsidiaryTimeline");
            if (timelineClass == null)
            {
                foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
                {
                    timelineClass = assembly.GetType("DynamicSubsidiaryTimelinePlugin");
                    if (timelineClass != null) break;
                }
            }
            if (timelineClass != null)
            {
                var method = AccessTools.Method(timelineClass, "GetAccelerationFactor");
                if (method != null)
                {
                    return (float)method.Invoke(null, new object[] { studio, slotIdx });
                }
            }
        }
        catch (System.Exception ex)
        {
            Log?.LogWarning("Failed to call GetAccelerationFactor: " + ex);
        }
        return 1f;
    }

    /// <summary>
    /// Get the minimum duration floor for a given game size from Dynamic Subsidiary Timeline config.
    /// Returns 1 if DST is not available or if game size is invalid.
    /// </summary>
    public static int GetMinimumFloorWeeks(int gameSize)
    {
        if (gameSize < 0 || gameSize > 5) return 1;

        try
        {
            System.Type timelineClass = System.Type.GetType("DynamicSubsidiaryTimelinePlugin, DynamicSubsidiaryTimeline");
            if (timelineClass == null)
            {
                foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
                {
                    timelineClass = assembly.GetType("DynamicSubsidiaryTimelinePlugin");
                    if (timelineClass != null) break;
                }
            }
            if (timelineClass != null)
            {
                // Access cfgHardFloorWeeks field
                var cfgHardFloorWeeksField = AccessTools.Field(timelineClass, "cfgHardFloorWeeks");
                if (cfgHardFloorWeeksField != null)
                {
                    var cfgArray = cfgHardFloorWeeksField.GetValue(null) as System.Array;
                    if (cfgArray != null && gameSize < cfgArray.Length)
                    {
                        var configEntry = cfgArray.GetValue(gameSize);
                        if (configEntry != null)
                        {
                            var valueProperty = configEntry.GetType().GetProperty("Value");
                            if (valueProperty != null)
                            {
                                object value = valueProperty.GetValue(configEntry);
                                if (value is int floorValue)
                                {
                                    return floorValue;
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            Log?.LogWarning($"Failed to get minimum floor weeks for game size {gameSize}: {ex}");
        }

        // Fallback defaults if DST not available
        int[] defaultFloors = { 6, 12, 26, 78, 146, 188 };
        return (gameSize >= 0 && gameSize < defaultFloors.Length) ? defaultFloors[gameSize] : 1;
    }

    public static void TryStartHelping(publisherScript studio, int helperSlotIdx, int targetSlotIdx)
    {
        if (studio == null) return;
        if (helperSlotIdx < 0 || helperSlotIdx > 2 || targetSlotIdx < 0 || targetSlotIdx > 2) return;
        if (helperSlotIdx == targetSlotIdx) return;

        StudioSlotData data = GetStudioSlotData(studio.myID);
        if (data == null || data.slots == null) return;

        SlotData helperSlot = data.slots[helperSlotIdx];
        SlotData targetSlot = data.slots[targetSlotIdx];

        if (helperSlot == null || !helperSlot.isUnlocked || helperSlot.gameID != -1) return;
        if (targetSlot == null || !targetSlot.isUnlocked || targetSlot.gameID == -1) return;

        // Check if there are already 2 helper teams working on targetSlot
        int existingHelpers = 0;
        for (int i = 0; i < data.slots.Length; i++)
        {
            if (i != targetSlotIdx && data.slots[i].isUnlocked && data.slots[i].helpingSlotIndex == targetSlotIdx)
            {
                existingHelpers++;
            }
        }
        if (existingHelpers >= 2)
        {
            GUI_Main gui = UnityEngine.Object.FindObjectOfType<GUI_Main>();
            if (gui != null) gui.MessageBox("Cannot assign more than 2 helper teams to a single project!", closeMenu: false);
            return;
        }

        // Set cooperation
        helperSlot.helpingSlotIndex = targetSlotIdx;
        SaveState(currentSaveSlot);

        // Recalculate duration instantly
        try
        {
            System.Type timelineClass = System.Type.GetType("DynamicSubsidiaryTimelinePlugin, DynamicSubsidiaryTimeline");
            if (timelineClass == null)
            {
                foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
                {
                    timelineClass = assembly.GetType("DynamicSubsidiaryTimelinePlugin");
                    if (timelineClass != null) break;
                }
            }
            if (timelineClass != null)
            {
                var method = AccessTools.Method(timelineClass, "RecalculateActiveProjectTimeline");
                if (method != null) method.Invoke(null, new object[] { studio });
            }
        }
        catch (System.Exception ex)
        {
            Log?.LogWarning("Failed to trigger RecalculateActiveProjectTimeline: " + ex);
        }

        sfxScript sfx = UnityEngine.Object.FindObjectOfType<sfxScript>();
        if (sfx != null) sfx.PlaySound(22, true);

        lastBuiltForStudio = -1;
        shiftedMenuInstances.Clear();
        Menu_Stats_Tochterfirma_Main menu = UnityEngine.Object.FindObjectOfType<Menu_Stats_Tochterfirma_Main>();
        if (menu != null && menu.gameObject.activeInHierarchy)
        {
            menu.UpdateData();
        }
    }

    public static void TryStopHelping(publisherScript studio, int helperSlotIdx)
    {
        if (studio == null) return;
        if (helperSlotIdx < 0 || helperSlotIdx > 2) return;

        StudioSlotData data = GetStudioSlotData(studio.myID);
        if (data == null || data.slots == null) return;

        SlotData helperSlot = data.slots[helperSlotIdx];
        if (helperSlot == null || helperSlot.helpingSlotIndex < 0) return;

        // Clear cooperation
        helperSlot.helpingSlotIndex = -1;
        SaveState(currentSaveSlot);

        // Recalculate duration instantly
        try
        {
            System.Type timelineClass = System.Type.GetType("DynamicSubsidiaryTimelinePlugin, DynamicSubsidiaryTimeline");
            if (timelineClass == null)
            {
                foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
                {
                    timelineClass = assembly.GetType("DynamicSubsidiaryTimelinePlugin");
                    if (timelineClass != null) break;
                }
            }
            if (timelineClass != null)
            {
                var method = AccessTools.Method(timelineClass, "RecalculateActiveProjectTimeline");
                if (method != null) method.Invoke(null, new object[] { studio });
            }
        }
        catch (System.Exception ex)
        {
            Log?.LogWarning("Failed to trigger RecalculateActiveProjectTimeline: " + ex);
        }

        sfxScript sfx = UnityEngine.Object.FindObjectOfType<sfxScript>();
        if (sfx != null) sfx.PlaySound(22, true);

        lastBuiltForStudio = -1;
        shiftedMenuInstances.Clear();
        Menu_Stats_Tochterfirma_Main menu = UnityEngine.Object.FindObjectOfType<Menu_Stats_Tochterfirma_Main>();
        if (menu != null && menu.gameObject.activeInHierarchy)
        {
            menu.UpdateData();
        }
    }

    public static bool IsOrganicStudio(publisherScript studio)
    {
        if (studio == null) return false;
        return (studio.myID >= 9000 && studio.myID < 10000)
            || (studio.myID >= 90000 && studio.myID < 100000);
    }

    public static string GetSlotTeamName(publisherScript studio, int slotIdx, StudioSlotData data)
    {
        if (data == null || slotIdx < 0 || slotIdx >= data.slots.Length) return $"Team {slotIdx + 1}";
        if (slotIdx == 0) return !string.IsNullOrEmpty(data.slots[0].teamName) ? data.slots[0].teamName : (studio != null ? studio.GetName() : "Main Team");
        return !string.IsNullOrEmpty(data.slots[slotIdx].teamName) ? data.slots[slotIdx].teamName : $"Team {slotIdx + 1}";
    }

    // =========================================================
    //  UI LAYOUT
    // =========================================================



    // Container for one slot row's UI references
    private class SlotUIRow
    {
        public GameObject container;
        public GameObject projectBox;     // The bordered panel
        public Image progressFill;
        public Text progressText;
        public Text gameNameText;
        public Text weeksText;
        public Button discardButton;
        public Button infoButton;

        // Cloned vanilla widgets
        public Image gameTypeIcon;
        public Image gameSizeIcon;
        public Text ipPopularityText;
        public Text genreText;
        public GameObject clockIcon;

        // Extended widgets
        public Text headerText;
        public Button renameButton;
        public GameObject idlePanel;
        public Text idleStatusText;
        public Button upgradeStarsButton;
        public Text upgradeStarsText;
        public Button upgradeSpeedButton;
        public Text upgradeSpeedText;
        public Button closeTeamButton;
        public GameObject lockOverlay;
        public Button unlockButton;
        public Text unlockButtonText;

        // Cooperative help buttons
        public Button helpTeam1Button;
        public Text helpTeam1Text;
        public Button helpTeam2Button;
        public Text helpTeam2Text;
        public Button helpTeam3Button;
        public Text helpTeam3Text;
        public Button stopHelpingButton;
        public Text stopHelpingText;
    }

    public static void ClearClonedUI()
    {
        if (sidePanel != null)
            UnityEngine.Object.Destroy(sidePanel);

        sidePanel = null;
        slotRow1 = null;
        slotRow2 = null;
        slotRow3 = null;
        lastBuiltForStudio = -1;

        // Clear cache
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
    }

    public static void RefreshSlotUI(Menu_Stats_Tochterfirma_Main menu, publisherScript studio)
    {
        #region debug-point A:refresh-slot-ui
        SubsidiaryTeamSlotsPlugin.Log?.LogInfo("[DEBUG] RefreshSlotUI - Entering");
        #endregion
        if (menu == null || studio == null) return;
        activeMenuInstance = menu;
        if (!studio.developer)
        {
            #region debug-point A:studio-not-developer
            SubsidiaryTeamSlotsPlugin.Log?.LogInfo("[DEBUG] RefreshSlotUI - Studio not developer");
            #endregion
            ClearClonedUI();
            return;
        }

        // If we switched to a different studio, clear and rebuild
        if (lastBuiltForStudio != studio.myID)
        {
            #region debug-point A:switching-studio
            SubsidiaryTeamSlotsPlugin.Log?.LogInfo("[DEBUG] RefreshSlotUI - Switching to new studio: " + studio.myID);
            #endregion
            ClearClonedUI();
            lastBuiltForStudio = studio.myID;
        }

        GUI_Main guiMain = f_guiMain.GetValue(menu) as GUI_Main;
        textScript tS = f_tS.GetValue(menu) as textScript;
        StudioSlotData data = GetStudioSlotData(studio.myID);
        games gamesScript = studio.games_ != null ? studio.games_ : GetGamesScript();
        
        #region debug-point A:before-ensure-side-panel
        SubsidiaryTeamSlotsPlugin.Log?.LogInfo($"[DEBUG] RefreshSlotUI - About to call EnsureSidePanel. guiMain: {guiMain != null}, tS: {tS != null}, data: {data != null}");
        #endregion

        // Ensure Side Panel & UI rows exist (lazy build)
        EnsureSidePanel(menu, data, tS, guiMain, studio);

        // Update Slot 1
        if (slotRow1 != null)
            UpdateSlotRow(slotRow1, data, 0, gamesScript, studio, guiMain, tS, menu);

        // Update Slot 2
        if (slotRow2 != null)
            UpdateSlotRow(slotRow2, data, 1, gamesScript, studio, guiMain, tS, menu);

        // Update Slot 3
        if (slotRow3 != null)
            UpdateSlotRow(slotRow3, data, 2, gamesScript, studio, guiMain, tS, menu);
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
        #region debug-point A:ensure-side-panel-start
        SubsidiaryTeamSlotsPlugin.Log?.LogInfo($"[DEBUG] EnsureSidePanel - Entering. uiObjects null: {menu.uiObjects == null}, uiObjects length: {menu.uiObjects?.Length ?? -1}");
        #endregion
        if (menu.uiObjects == null || menu.uiObjects.Length <= 31) return;

        // If sidePanel exists, return
        if (sidePanel != null) {
            #region debug-point A:side-panel-exists
            SubsidiaryTeamSlotsPlugin.Log?.LogInfo("[DEBUG] EnsureSidePanel - Side panel already exists");
            #endregion
            return;
        }

        // Add lifecycle hook component to clean up side panel on close
        if (menu.gameObject.GetComponent<STSWindowLifecycleHook>() == null)
        {
            menu.gameObject.AddComponent<STSWindowLifecycleHook>();
        }

        Transform menueTransform = menu.transform.Find("Menue");
        if (menueTransform != null)
        {
            RectTransform menueRt = menueTransform.GetComponent<RectTransform>();
            if (menueRt != null)
            {
                // Shift the main window left to center the overall screen combination
                menueRt.anchoredPosition = new Vector2(-185f, 0f);
            }
        }

        // Parent to Menue instead of menu.transform to stay with the main window
        Transform targetParent = menueTransform != null ? menueTransform : menu.transform;

        // Create Side Panel container
        sidePanel = new GameObject("STS_SidePanel", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        sidePanel.transform.SetParent(targetParent, false);

        Image mainBg = menu.GetComponent<Image>();
        if (mainBg == null && menueTransform != null)
        {
            mainBg = menueTransform.GetComponent<Image>();
        }
        Image sideBg = sidePanel.GetComponent<Image>();
        if (mainBg != null && sideBg != null)
        {
            sideBg.sprite = mainBg.sprite;
            sideBg.color = mainBg.color;
            sideBg.type = mainBg.type;
            sideBg.material = mainBg.material;
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
        titleRect.sizeDelta = new Vector2(0f, 40f);
        titleRect.anchoredPosition = new Vector2(0f, -15f);
        titleRect.localScale = Vector3.one;

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

        // Build the three slot rows vertically inside the side panel
        slotRow1 = BuildSlotRowFromTemplate(menu, "STS_Slot1_Box", sidePanel.transform, -60f, 0, studio, guiMain);
        slotRow2 = BuildSlotRowFromTemplate(menu, "STS_Slot2_Box", sidePanel.transform, -210f, 1, studio, guiMain);
        slotRow3 = BuildSlotRowFromTemplate(menu, "STS_Slot3_Box", sidePanel.transform, -360f, 2, studio, guiMain);
    }

    private static string GetRelativePath(Transform root, Transform child)
    {
        if (root == child) return "";
        System.Collections.Generic.List<string> pathParts = new System.Collections.Generic.List<string>();
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
        string name,
        Transform parent,
        float yPosition,
        int slotIdx,
        publisherScript studio,
        GUI_Main guiMain)
    {
        GameObject refNameText = menu.uiObjects[23];
        if (refNameText == null) return null;
        Transform origBox = refNameText.transform.parent;
        if (origBox == null) return null;

        // Clone the whole project box container!
        GameObject slotBox = UnityEngine.Object.Instantiate(origBox.gameObject, parent);
        slotBox.name = name;

        // Adjust RectTransform of slotBox (width 324, height 140)
        RectTransform boxRect = slotBox.GetComponent<RectTransform>();
        boxRect.anchorMin = new Vector2(0.05f, 1f);
        boxRect.anchorMax = new Vector2(0.95f, 1f);
        boxRect.pivot = new Vector2(0.5f, 1f);
        boxRect.sizeDelta = new Vector2(0f, 140f);
        boxRect.anchoredPosition = new Vector2(0f, yPosition);
        boxRect.localScale = Vector3.one;

        int capturedSlot = slotIdx;

        SlotUIRow row = new SlotUIRow();
        row.container = slotBox;
        row.projectBox = slotBox;

        // Resolve paths from original box
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

        // Bind cloned widgets
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

        // Deactivate Subsidiary Project Director button inside slot box if cloned
        Transform spdBtn = slotBox.transform.Find("Button_SubsidiaryProjectDirector");
        if (spdBtn != null) spdBtn.gameObject.SetActive(false);

        // Normalize blue bar background (TxtTop (3))
        Transform blueBar = slotBox.transform.Find("TxtTop (3)");
        if (blueBar != null)
        {
            NormalizeRectTransform(blueBar, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, 20f), new Vector2(0f, 0f));
            Transform blueBarText = blueBar.Find("Text");
            if (blueBarText != null)
            {
                blueBarText.gameObject.SetActive(false); // Hide the "Current project" vanilla text
            }
        }

        // Normalize game name text component
        if (row.gameNameText != null)
        {
            NormalizeRectTransform(row.gameNameText.transform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 0.5f), new Vector2(300f, 20f), new Vector2(0f, -35f));
            row.gameNameText.alignment = TextAnchor.MiddleCenter;
            row.gameNameText.fontSize = 11;
        }

        // Normalize genre text component
        if (row.genreText != null)
        {
            NormalizeRectTransform(row.genreText.transform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 0.5f), new Vector2(300f, 16f), new Vector2(0f, -53f));
            row.genreText.alignment = TextAnchor.MiddleCenter;
            row.genreText.fontSize = 9;
        }

        // Normalize game type and size icons
        if (row.gameTypeIcon != null)
        {
            NormalizeRectTransform(row.gameTypeIcon.transform, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0.5f, 0.5f), new Vector2(24f, 24f), new Vector2(20f, -35f));
        }
        if (row.gameSizeIcon != null)
        {
            NormalizeRectTransform(row.gameSizeIcon.transform, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0.5f, 0.5f), new Vector2(24f, 24f), new Vector2(48f, -35f));
        }

        // Normalize progress bar background and fill
        if (row.progressFill != null)
        {
            Transform progressBG = row.progressFill.transform.parent;
            if (progressBG != null)
            {
                NormalizeRectTransform(progressBG, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 0.5f), new Vector2(220f, 18f), new Vector2(0f, -76f));
                NormalizeRectTransform(row.progressFill.transform, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f), new Vector2(-4f, -4f), Vector2.zero);
            }
        }

        // Normalize progress percent text
        if (row.progressText != null)
        {
            NormalizeRectTransform(row.progressText.transform, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
            row.progressText.alignment = TextAnchor.MiddleCenter;
            row.progressText.fontSize = 9;
        }

        // Normalize clock icon and remaining weeks text below it
        if (row.clockIcon != null)
        {
            NormalizeRectTransform(row.clockIcon.transform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 0.5f), new Vector2(22f, 22f), new Vector2(-140f, -76f));
            Transform weeksNum = row.clockIcon.transform.Find("TextWeeksNum");
            if (weeksNum != null)
            {
                NormalizeRectTransform(weeksNum, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 1f), new Vector2(40f, 14f), new Vector2(0f, -2f));
                Text weeksNumText = weeksNum.GetComponent<Text>();
                if (weeksNumText != null)
                {
                    weeksNumText.alignment = TextAnchor.MiddleCenter;
                    weeksNumText.fontSize = 9;
                }
            }
        }

        // Normalize IP icon and its popularity text
        if (row.ipPopularityText != null)
        {
            Transform iconIP = row.ipPopularityText.transform.parent;
            if (iconIP != null)
            {
                NormalizeRectTransform(iconIP, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 0.5f), new Vector2(22f, 22f), new Vector2(140f, -76f));
                NormalizeRectTransform(row.ipPopularityText.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 1f), new Vector2(40f, 14f), new Vector2(0f, -2f));
                row.ipPopularityText.alignment = TextAnchor.MiddleCenter;
                row.ipPopularityText.fontSize = 9;
            }
        }

        // Normalize weeks remaining label
        if (row.weeksText != null)
        {
            row.weeksText.transform.SetParent(slotBox.transform, false);
            NormalizeRectTransform(row.weeksText.transform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 0.5f), new Vector2(60f, 16f), new Vector2(-140f, -98f));
            row.weeksText.alignment = TextAnchor.MiddleCenter;
            row.weeksText.fontSize = 10;
            row.weeksText.fontStyle = FontStyle.Bold;
            row.weeksText.color = Color.black;
        }

        // Normalize development report button
        if (row.infoButton != null)
        {
            NormalizeRectTransform(row.infoButton.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(160f, 22f), new Vector2(-15f, 8f));
            Text infoBtnText = row.infoButton.transform.Find("Text")?.GetComponent<Text>();
            if (infoBtnText != null)
            {
                infoBtnText.alignment = TextAnchor.MiddleCenter;
                infoBtnText.fontSize = 9;
            }

            row.infoButton.onClick.RemoveAllListeners();
            row.infoButton.onClick.AddListener(() =>
            {
                publisherScript targetStudio = activeMenuInstance != null ? f_pS.GetValue(activeMenuInstance) as publisherScript : studio;
                if (targetStudio != null)
                {
                    StudioSlotData slotData = GetStudioSlotData(targetStudio.myID);
                    int gID = slotData.slots[capturedSlot].gameID;
                    if (gID != -1)
                    {
                        games gScriptObj = targetStudio.games_ != null ? targetStudio.games_ : UnityEngine.Object.FindObjectOfType<games>();
                        gameScript gameToReport = FindGameByID(gScriptObj, gID);
                        if (gameToReport != null)
                        {
                            GUI_Main gui = UnityEngine.Object.FindObjectOfType<GUI_Main>();
                            if (gui != null)
                            {
                                gui.ActivateMenu(gui.uiObjects[406]);
                                gui.uiObjects[406].GetComponent<Menu_Stats_Tochterfirma_GameEntwicklungsbericht>().Init(gameToReport);
                            }
                        }
                    }
                }
            });
        }

        // Normalize discard / trashcan button
        if (row.discardButton != null)
        {
            NormalizeRectTransform(row.discardButton.transform, new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(24f, 24f), new Vector2(-10f, 8f));
            Transform discardIcon = row.discardButton.transform.Find("Icon");
            if (discardIcon != null)
            {
                NormalizeRectTransform(discardIcon, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
            }

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
                        games gScriptObj = targetStudio.games_ != null ? targetStudio.games_ : UnityEngine.Object.FindObjectOfType<games>();
                        gameScript gameToDiscard = FindGameByID(gScriptObj, gID);
                        if (gameToDiscard != null)
                        {
                            slotToDiscard = capturedSlot; // Set global context so ResetGame patch clears this slot
                            
                            GUI_Main gui = UnityEngine.Object.FindObjectOfType<GUI_Main>();
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

        // Add SlotHeader text inside the blue bar header
        GameObject headerObj = new GameObject("SlotHeader", typeof(RectTransform), typeof(Text));
        headerObj.transform.SetParent(slotBox.transform, false);
        NormalizeRectTransform(headerObj.transform, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f), new Vector2(-20f, 20f), new Vector2(10f, 0f));

        row.headerText = headerObj.GetComponent<Text>();
        row.headerText.alignment = TextAnchor.MiddleLeft;
        row.headerText.fontSize = 11;
        row.headerText.fontStyle = FontStyle.Bold;
        row.headerText.color = new Color(0.9f, 0.9f, 0.9f, 1f);
        if (row.gameNameText != null) row.headerText.font = row.gameNameText.font;

        // Add Rename Button inside the blue bar header
        GameObject renameBtnObj = new GameObject("RenameBtn", typeof(RectTransform), typeof(Image), typeof(Button));
        renameBtnObj.transform.SetParent(slotBox.transform, false);
        NormalizeRectTransform(renameBtnObj.transform, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(50f, 16f), new Vector2(-10f, -2f));

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

        // capturedSlot is already declared at the top of this method
        row.renameButton.onClick.AddListener(() =>
        {
            showRenameWindow = true;
            renameSlotIndex = capturedSlot;
            renameStudioID = studio.myID;
            renameInputString = GetStudioSlotData(studio.myID).slots[capturedSlot].teamName;
        });

        // Add Idle Panel
        row.idlePanel = new GameObject("IdlePanel", typeof(RectTransform));
        row.idlePanel.transform.SetParent(slotBox.transform, false);
        RectTransform idleRect = row.idlePanel.GetComponent<RectTransform>();
        idleRect.anchorMin = Vector2.zero;
        idleRect.anchorMax = Vector2.one;
        idleRect.offsetMin = new Vector2(0f, 0f);
        idleRect.offsetMax = new Vector2(0f, -25f);
        idleRect.localScale = Vector3.one;

        // Idle Status Text
        GameObject idleStatusObj = new GameObject("IdleStatusText", typeof(RectTransform), typeof(Text));
        idleStatusObj.transform.SetParent(row.idlePanel.transform, false);
        NormalizeRectTransform(idleStatusObj.transform, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f), new Vector2(-20f, 25f), new Vector2(0f, -15f));

        row.idleStatusText = idleStatusObj.GetComponent<Text>();
        row.idleStatusText.alignment = TextAnchor.MiddleCenter;
        row.idleStatusText.fontSize = 13;
        row.idleStatusText.fontStyle = FontStyle.Normal;
        row.idleStatusText.color = new Color(0.7f, 0.7f, 0.7f, 1f);
        row.idleStatusText.text = "Idle";
        if (row.gameNameText != null) row.idleStatusText.font = row.gameNameText.font;

        // Helper to build action buttons
        row.upgradeStarsButton = CreateActionButton(slotBox.transform, "UpgradeStars", new Vector2(-105f, 8f), new Vector2(95f, 22f), row.gameNameText?.font, out row.upgradeStarsText);
        row.upgradeStarsButton.onClick.AddListener(() => TryUpgradeTeamStars(studio, capturedSlot));

        row.upgradeSpeedButton = CreateActionButton(slotBox.transform, "UpgradeSpeed", new Vector2(0f, 8f), new Vector2(95f, 22f), row.gameNameText?.font, out row.upgradeSpeedText);
        row.upgradeSpeedButton.onClick.AddListener(() => TryUpgradeTeamSpeed(studio, capturedSlot));

        row.closeTeamButton = CreateActionButton(slotBox.transform, "CloseTeam", new Vector2(105f, 8f), new Vector2(95f, 22f), row.gameNameText?.font, out Text closeText);
        closeText.text = "Close Team";
        row.closeTeamButton.onClick.AddListener(() => TryCloseSlot(studio, capturedSlot));

        // Cooperation buttons (Help Team 1, 2, 3) inside idle panel
        row.helpTeam1Button = CreateActionButton(row.idlePanel.transform, "HelpTeam1Btn", new Vector2(-110f, 35f), new Vector2(100f, 22f), row.gameNameText?.font, out row.helpTeam1Text);
        row.helpTeam1Text.text = "Help Team 1";
        row.helpTeam1Button.onClick.AddListener(() => TryStartHelping(studio, capturedSlot, 0));

        row.helpTeam2Button = CreateActionButton(row.idlePanel.transform, "HelpTeam2Btn", new Vector2(0f, 35f), new Vector2(100f, 22f), row.gameNameText?.font, out row.helpTeam2Text);
        row.helpTeam2Text.text = "Help Team 2";
        row.helpTeam2Button.onClick.AddListener(() => TryStartHelping(studio, capturedSlot, 1));

        row.helpTeam3Button = CreateActionButton(row.idlePanel.transform, "HelpTeam3Btn", new Vector2(110f, 35f), new Vector2(100f, 22f), row.gameNameText?.font, out row.helpTeam3Text);
        row.helpTeam3Text.text = "Help Team 3";
        row.helpTeam3Button.onClick.AddListener(() => TryStartHelping(studio, capturedSlot, 2));

        // Stop Helping button inside slotBox
        row.stopHelpingButton = CreateActionButton(slotBox.transform, "StopHelpingBtn", new Vector2(0f, 35f), new Vector2(160f, 22f), row.gameNameText?.font, out row.stopHelpingText);
        row.stopHelpingText.text = "Stop Helping";
        row.stopHelpingButton.onClick.AddListener(() => TryStopHelping(studio, capturedSlot));

        // Add Lock Overlay (Only for Slot 2 & Slot 3)
        if (slotIdx > 0)
        {
            row.lockOverlay = new GameObject("LockOverlay", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            row.lockOverlay.transform.SetParent(slotBox.transform, false);
            NormalizeRectTransform(row.lockOverlay.transform, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);

            Image lockImg = row.lockOverlay.GetComponent<Image>();
            lockImg.color = new Color(0.12f, 0.12f, 0.12f, 0.9f);

            // Unlock Button
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

    private static void UpdateSlotRow(
        SlotUIRow row,
        StudioSlotData data, int slotIdx,
        games gamesScript, publisherScript studio, GUI_Main guiMain, textScript tS,
        Menu_Stats_Tochterfirma_Main menu)
    {
        if (row == null || row.container == null) return;

        SlotData slot = data.slots[slotIdx];

        int maxStars = studio.GetStarsAmount();
        int maxSpeed = studio.developmentSpeed;
        int tStars = (slotIdx == 0) ? maxStars : slot.stars;
        int tSpeed = (slotIdx == 0) ? maxSpeed : slot.speed;

        float acceleration = GetAccelerationFactor(studio, slotIdx);

        // Cache check
        SlotStateCache cache = cachedSlotStates[slotIdx];
        bool dirty = false;
        if (cache.gameID != slot.gameID ||
            cache.stars != tStars ||
            cache.speed != tSpeed ||
            cache.isUnlocked != slot.isUnlocked ||
            cache.teamName != slot.teamName ||
            cache.studioStars != maxStars ||
            cache.studioSpeed != maxSpeed ||
            cache.helpingSlotIndex != slot.helpingSlotIndex ||
            Mathf.Abs(cache.acceleration - acceleration) > 0.01f)
        {
            dirty = true;
        }

        int weeks = 0;
        int progressPercent = 0;
        if (slot.gameID != -1)
        {
            // Get the game to check its size for minimum floor
            gameScript game = FindGameByID(gamesScript, slot.gameID);
            int minFloor = (game != null) ? GetMinimumFloorWeeks(game.gameSize) : 1;
            
            // Calculate weeks with acceleration, but respect the minimum floor
            int calculatedWeeks = Mathf.CeilToInt(slot.remainingWeeks / acceleration);
            weeks = Mathf.Max(minFloor, calculatedWeeks);
            
            float progress = (slot.totalWeeks > 0f) ? (1f - (slot.remainingWeeks / slot.totalWeeks)) : 0f;
            progress = Mathf.Clamp01(progress);
            progressPercent = Mathf.RoundToInt(progress * 100f);
        }

        if (slot.gameID != -1 && Mathf.Abs(cache.remainingWeeks - slot.remainingWeeks) > 0.05f)
        {
            // Get the game to check its size for minimum floor (reuse from above if available)
            gameScript game = FindGameByID(gamesScript, slot.gameID);
            int minFloor = (game != null) ? GetMinimumFloorWeeks(game.gameSize) : 1;
            
            int oldCalculatedWeeks = Mathf.CeilToInt(cache.remainingWeeks / acceleration);
            int oldWeeks = (cache.gameID != -1 && cache.remainingWeeks > 0f) ? Mathf.Max(minFloor, oldCalculatedWeeks) : -1;
            
            float oldProgress = (cache.gameID != -1 && cache.remainingWeeks > 0f && slot.totalWeeks > 0f) ? (1f - (cache.remainingWeeks / slot.totalWeeks)) : 0f;
            int oldProgressPercent = Mathf.RoundToInt(Mathf.Clamp01(oldProgress) * 100f);

            if (weeks != oldWeeks || progressPercent != oldProgressPercent)
            {
                dirty = true;
            }
        }

        if (!dirty)
        {
            return;
        }

        // Update cache
        cache.gameID = slot.gameID;
        cache.remainingWeeks = slot.remainingWeeks;
        cache.stars = tStars;
        cache.speed = tSpeed;
        cache.isUnlocked = slot.isUnlocked;
        cache.teamName = slot.teamName;
        cache.studioStars = maxStars;
        cache.studioSpeed = maxSpeed;
        cache.helpingSlotIndex = slot.helpingSlotIndex;
        cache.acceleration = acceleration;

        if (slotIdx > 0 && !slot.isUnlocked)
        {
            // Slot is locked
            if (row.lockOverlay != null)
            {
                row.lockOverlay.SetActive(true);
                if (row.unlockButtonText != null)
                {
                    long cost = (slotIdx == 1) ? 15000000L : 60000000L;
                    row.unlockButtonText.text = $"Unlock Team {slotIdx + 1} (${cost:N0})";
                }
            }

            // Hide everything else
            if (row.gameNameText != null) row.gameNameText.gameObject.SetActive(false);
            if (row.progressFill != null)
            {
                if (row.progressFill.transform.parent != null && row.progressFill.transform.parent.gameObject != row.container)
                    row.progressFill.transform.parent.gameObject.SetActive(false);
                else
                    row.progressFill.gameObject.SetActive(false);
            }
            if (row.progressText != null) row.progressText.gameObject.SetActive(false);
            if (row.weeksText != null) row.weeksText.gameObject.SetActive(false);
            if (row.discardButton != null) row.discardButton.gameObject.SetActive(false);
            if (row.infoButton != null) row.infoButton.gameObject.SetActive(false);
            if (row.gameTypeIcon != null) row.gameTypeIcon.gameObject.SetActive(false);
            if (row.gameSizeIcon != null) row.gameSizeIcon.gameObject.SetActive(false);
            if (row.ipPopularityText != null) row.ipPopularityText.gameObject.SetActive(false);
            if (row.genreText != null) row.genreText.gameObject.SetActive(false);
            if (row.clockIcon != null) row.clockIcon.SetActive(false);
            if (row.idlePanel != null) row.idlePanel.SetActive(false);
            if (row.renameButton != null) row.renameButton.gameObject.SetActive(false);
            if (row.upgradeStarsButton != null) row.upgradeStarsButton.gameObject.SetActive(false);
            if (row.upgradeSpeedButton != null) row.upgradeSpeedButton.gameObject.SetActive(false);
            if (row.closeTeamButton != null) row.closeTeamButton.gameObject.SetActive(false);
            if (row.headerText != null) row.headerText.text = $"Locked (Requires {((slotIdx == 1) ? 3 : 5)} Stars)";
            return;
        }

        // Slot is unlocked
        if (row.lockOverlay != null) row.lockOverlay.SetActive(false);
        if (row.renameButton != null) row.renameButton.gameObject.SetActive(true);

        bool isOrganic = IsOrganicStudio(studio);

        long upkeep = GetTeamUpkeep(studio, slotIdx);
        if (row.headerText != null)
        {
            mainScript main = studio.mS_ != null ? studio.mS_ : GetMainScript();
            string upkeepText = main != null ? main.GetMoney(upkeep, showDollar: true) : upkeep.ToString();
            row.headerText.text = $"{slot.teamName} (★ {tStars}/5 | ⚡ {tSpeed}/{(isOrganic ? 10 : 4)}) | Upkeep: {upkeepText}";
        }

        if (slot.gameID != -1)
        {
            // Active project
            if (row.idlePanel != null) row.idlePanel.SetActive(false);
            if (row.stopHelpingButton != null) row.stopHelpingButton.gameObject.SetActive(false);

            gameScript game = FindGameByID(gamesScript, slot.gameID);
            if (game != null)
            {
                if (row.gameNameText != null)
                {
                    row.gameNameText.gameObject.SetActive(true);
                    row.gameNameText.text = game.GetNameWithTag();
                }

                float progress = (slot.totalWeeks > 0f) ? (1f - (slot.remainingWeeks / slot.totalWeeks)) : 0f;
                progress = Mathf.Clamp01(progress);

                if (row.progressFill != null)
                {
                    if (row.progressFill.transform.parent != null && row.progressFill.transform.parent.gameObject != row.container)
                        row.progressFill.transform.parent.gameObject.SetActive(true);
                    else
                        row.progressFill.gameObject.SetActive(true);

                    row.progressFill.fillAmount = progress;
                }

                if (row.progressText != null)
                {
                    row.progressText.gameObject.SetActive(true);
                    row.progressText.text = $"Progress: {Mathf.RoundToInt(progress * 100f)}%";
                }

                if (row.weeksText != null)
                {
                    row.weeksText.gameObject.SetActive(true);
                    row.weeksText.text = weeks.ToString() + "w";
                }

                if (row.discardButton != null)
                {
                    row.discardButton.gameObject.SetActive(true);
                    row.discardButton.interactable = true;
                }

                if (slotIdx > 0)
                {
                    if (row.infoButton != null)
                    {
                        row.infoButton.gameObject.SetActive(true);
                        row.infoButton.interactable = true;
                        NormalizeRectTransform(row.infoButton.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(110f, 22f), new Vector2(-110f, 8f));
                    }

                    if (row.upgradeStarsButton != null)
                    {
                        row.upgradeStarsButton.gameObject.SetActive(true);
                        NormalizeRectTransform(row.upgradeStarsButton.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(70f, 22f), new Vector2(-10f, 8f));
                        if (slot.stars >= maxStars)
                        {
                            row.upgradeStarsButton.interactable = false;
                            if (row.upgradeStarsText != null) row.upgradeStarsText.text = "Max ★";
                        }
                        else
                        {
                            long[] starCosts = new long[] { 0L, 2000000L, 5000000L, 12000000L, 25000000L };
                            long cost = starCosts[slot.stars];
                            row.upgradeStarsButton.interactable = true;
                            if (row.upgradeStarsText != null) row.upgradeStarsText.text = $"★Up({cost / 1000000L}M)";
                        }
                    }

                    if (row.upgradeSpeedButton != null)
                    {
                        // Hide speed upgrade button when active to prevent UI crowding and overlap with close/discard buttons
                        row.upgradeSpeedButton.gameObject.SetActive(false);
                    }

                    if (row.closeTeamButton != null)
                    {
                        row.closeTeamButton.gameObject.SetActive(true);
                        row.closeTeamButton.interactable = true;
                        NormalizeRectTransform(row.closeTeamButton.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(80f, 22f), new Vector2(75f, 8f));
                    }
                }
                else
                {
                    if (row.upgradeStarsButton != null) row.upgradeStarsButton.gameObject.SetActive(false);
                    if (row.upgradeSpeedButton != null) row.upgradeSpeedButton.gameObject.SetActive(false);
                    if (row.closeTeamButton != null) row.closeTeamButton.gameObject.SetActive(false);

                    if (row.infoButton != null)
                    {
                        row.infoButton.gameObject.SetActive(true);
                        row.infoButton.interactable = true;
                        NormalizeRectTransform(row.infoButton.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(160f, 22f), new Vector2(-15f, 8f));
                    }
                }

                if (row.gameTypeIcon != null)
                {
                    row.gameTypeIcon.gameObject.SetActive(true);
                    row.gameTypeIcon.enabled = true;
                    row.gameTypeIcon.sprite = game.GetTypSprite();
                }

                if (row.gameSizeIcon != null)
                {
                    row.gameSizeIcon.gameObject.SetActive(true);
                    row.gameSizeIcon.enabled = true;
                    row.gameSizeIcon.sprite = game.GetSizeSprite();
                }

                if (row.ipPopularityText != null)
                {
                    row.ipPopularityText.gameObject.SetActive(true);
                    mainScript main = studio.mS_ != null ? studio.mS_ : GetMainScript();
                    if (main != null)
                        row.ipPopularityText.text = main.Round(game.GetIpBekanntheit(), 1).ToString();
                    else
                        row.ipPopularityText.text = game.GetIpBekanntheit().ToString("F1");
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
        }

        if (slot.gameID == -1)
        {
            // Idle state
            if (row.gameNameText != null) row.gameNameText.gameObject.SetActive(false);
            if (row.progressFill != null)
            {
                if (row.progressFill.transform.parent != null && row.progressFill.transform.parent.gameObject != row.container)
                    row.progressFill.transform.parent.gameObject.SetActive(false);
                else
                    row.progressFill.gameObject.SetActive(false);
            }
            if (row.progressText != null) row.progressText.gameObject.SetActive(false);
            if (row.weeksText != null) row.weeksText.gameObject.SetActive(false);
            if (row.discardButton != null) row.discardButton.gameObject.SetActive(false);
            if (row.infoButton != null) row.infoButton.gameObject.SetActive(false);
            if (row.gameTypeIcon != null) row.gameTypeIcon.gameObject.SetActive(false);
            if (row.gameSizeIcon != null) row.gameSizeIcon.gameObject.SetActive(false);
            if (row.ipPopularityText != null) row.ipPopularityText.gameObject.SetActive(false);
            if (row.genreText != null) row.genreText.gameObject.SetActive(false);
            if (row.clockIcon != null) row.clockIcon.SetActive(false);

            if (row.idlePanel != null)
            {
                row.idlePanel.SetActive(true);

                if (slot.helpingSlotIndex >= 0)
                {
                    // Cooperating State
                    string targetName = GetSlotTeamName(studio, slot.helpingSlotIndex, data);
                    if (row.idleStatusText != null) row.idleStatusText.text = $"Helping {targetName}";

                    // Show stop helping button
                    if (row.stopHelpingButton != null)
                    {
                        row.stopHelpingButton.gameObject.SetActive(true);
                        row.stopHelpingButton.interactable = true;
                        NormalizeRectTransform(row.stopHelpingButton.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(160f, 22f), new Vector2(0f, 35f));
                    }

                    // Hide upgrade, close and help buttons
                    if (row.upgradeStarsButton != null) row.upgradeStarsButton.gameObject.SetActive(false);
                    if (row.upgradeSpeedButton != null) row.upgradeSpeedButton.gameObject.SetActive(false);
                    if (row.closeTeamButton != null) row.closeTeamButton.gameObject.SetActive(false);

                    if (row.helpTeam1Button != null) row.helpTeam1Button.gameObject.SetActive(false);
                    if (row.helpTeam2Button != null) row.helpTeam2Button.gameObject.SetActive(false);
                    if (row.helpTeam3Button != null) row.helpTeam3Button.gameObject.SetActive(false);
                }
                else
                {
                    // Idle state (not helping)
                    if (row.stopHelpingButton != null) row.stopHelpingButton.gameObject.SetActive(false);

                    if (slotIdx == 0)
                    {
                        if (row.idleStatusText != null) row.idleStatusText.text = "Main Team";
                        if (row.upgradeStarsButton != null) row.upgradeStarsButton.gameObject.SetActive(false);
                        if (row.closeTeamButton != null) row.closeTeamButton.gameObject.SetActive(false);

                        if (row.upgradeSpeedButton != null)
                        {
                            row.upgradeSpeedButton.gameObject.SetActive(true);
                            NormalizeRectTransform(row.upgradeSpeedButton.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(95f, 22f), new Vector2(0f, 8f));
                            int speedCap = isOrganic ? 10 : 4;
                            if (studio.developmentSpeed >= speedCap)
                            {
                                row.upgradeSpeedButton.interactable = false;
                                if (row.upgradeSpeedText != null) row.upgradeSpeedText.text = "Max Speed";
                            }
                            else
                            {
                                long cost = isOrganic ? (500000L * (studio.developmentSpeed + 1)) : (1500000L * (studio.developmentSpeed + 1));
                                row.upgradeSpeedButton.interactable = true;
                                if (row.upgradeSpeedText != null) row.upgradeSpeedText.text = $"⚡ Up (${cost / 1000f:N0}k)";
                            }
                        }
                    }
                    else
                    {
                        if (row.idleStatusText != null) row.idleStatusText.text = "Idle";

                        // Stars button
                        if (row.upgradeStarsButton != null)
                        {
                            row.upgradeStarsButton.gameObject.SetActive(true);
                            NormalizeRectTransform(row.upgradeStarsButton.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(95f, 22f), new Vector2(-105f, 8f));
                            if (slot.stars >= maxStars)
                            {
                                row.upgradeStarsButton.interactable = false;
                                if (row.upgradeStarsText != null) row.upgradeStarsText.text = "Max Stars";
                            }
                            else
                            {
                                long[] starCosts = new long[] { 0L, 2000000L, 5000000L, 12000000L, 25000000L };
                                long cost = starCosts[slot.stars];
                                row.upgradeStarsButton.interactable = true;
                                if (row.upgradeStarsText != null) row.upgradeStarsText.text = $"★ Up (${cost / 1000000L}M)";
                            }
                        }

                        // Speed button
                        if (row.upgradeSpeedButton != null)
                        {
                            row.upgradeSpeedButton.gameObject.SetActive(true);
                            NormalizeRectTransform(row.upgradeSpeedButton.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(95f, 22f), new Vector2(0f, 8f));
                            if (slot.speed >= maxSpeed)
                            {
                                row.upgradeSpeedButton.interactable = false;
                                if (row.upgradeSpeedText != null) row.upgradeSpeedText.text = "Max Speed";
                            }
                            else
                            {
                                long cost = isOrganic ? (500000L * (slot.speed + 1)) : (1500000L * (slot.speed + 1));
                                row.upgradeSpeedButton.interactable = true;
                                if (row.upgradeSpeedText != null) row.upgradeSpeedText.text = $"⚡ Up (${cost / 1000f:N0}k)";
                            }
                        }

                        // Close button
                        if (row.closeTeamButton != null)
                        {
                            row.closeTeamButton.gameObject.SetActive(true);
                            NormalizeRectTransform(row.closeTeamButton.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(95f, 22f), new Vector2(105f, 8f));
                            row.closeTeamButton.interactable = true;
                        }
                    }

                    // Dynamically position help buttons based on active projects
                    List<int> validTargets = new List<int>();
                    for (int t = 0; t < 3; t++)
                    {
                        if (t == slotIdx) continue;
                        var targetSlot = data.slots[t];
                        if (targetSlot != null && targetSlot.isUnlocked && targetSlot.gameID != -1)
                        {
                            validTargets.Add(t);
                        }
                    }

                    // Hide all by default
                    if (row.helpTeam1Button != null) row.helpTeam1Button.gameObject.SetActive(false);
                    if (row.helpTeam2Button != null) row.helpTeam2Button.gameObject.SetActive(false);
                    if (row.helpTeam3Button != null) row.helpTeam3Button.gameObject.SetActive(false);

                    if (validTargets.Count == 1)
                    {
                        int target = validTargets[0];
                        Button btn = null; Text txt = null;
                        if (target == 0) { btn = row.helpTeam1Button; txt = row.helpTeam1Text; }
                        else if (target == 1) { btn = row.helpTeam2Button; txt = row.helpTeam2Text; }
                        else if (target == 2) { btn = row.helpTeam3Button; txt = row.helpTeam3Text; }

                        if (btn != null)
                        {
                            btn.gameObject.SetActive(true);
                            if (txt != null) txt.text = "Help " + GetSlotTeamName(studio, target, data);
                            NormalizeRectTransform(btn.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(160f, 22f), new Vector2(0f, 35f));
                        }
                    }
                    else if (validTargets.Count == 2)
                    {
                        int targetA = validTargets[0];
                        int targetB = validTargets[1];

                        Button btnA = null; Text txtA = null;
                        if (targetA == 0) { btnA = row.helpTeam1Button; txtA = row.helpTeam1Text; }
                        else if (targetA == 1) { btnA = row.helpTeam2Button; txtA = row.helpTeam2Text; }
                        else if (targetA == 2) { btnA = row.helpTeam3Button; txtA = row.helpTeam3Text; }

                        Button btnB = null; Text txtB = null;
                        if (targetB == 0) { btnB = row.helpTeam1Button; txtB = row.helpTeam1Text; }
                        else if (targetB == 1) { btnB = row.helpTeam2Button; txtB = row.helpTeam2Text; }
                        else if (targetB == 2) { btnB = row.helpTeam3Button; txtB = row.helpTeam3Text; }

                        if (btnA != null)
                        {
                            btnA.gameObject.SetActive(true);
                            if (txtA != null) txtA.text = "Help " + GetSlotTeamName(studio, targetA, data);
                            NormalizeRectTransform(btnA.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(110f, 22f), new Vector2(-60f, 35f));
                        }
                        if (btnB != null)
                        {
                            btnB.gameObject.SetActive(true);
                            if (txtB != null) txtB.text = "Help " + GetSlotTeamName(studio, targetB, data);
                            NormalizeRectTransform(btnB.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(110f, 22f), new Vector2(60f, 35f));
                        }
                    }
                }
            }
        }
    }
}

// =========================================================
//  HARMONY PATCHES
// =========================================================

[HarmonyPatch(typeof(savegameScript), "Save")]
public static class SavegameScript_Save_Patch
{
    public static void Postfix(int i)
    {
        SubsidiaryTeamSlotsPlugin.SaveState(i);
    }
}

[HarmonyPatch(typeof(savegameScript), "Load")]
public static class SavegameScript_Load_Patch
{
    public static void Postfix(int i)
    {
        SubsidiaryTeamSlotsPlugin.LoadState(i);
    }
}

[HarmonyPatch(typeof(mainScript), "Start")]
public static class MainScript_Awake_Patch
{
    public static void Postfix()
    {
        SubsidiaryTeamSlotsPlugin.currentSaveSlot = -1;
        SubsidiaryTeamSlotsPlugin.State = new SubsidiaryTeamSlotsPlugin.SlotSaveState();
        SubsidiaryTeamSlotsPlugin.Log?.LogInfo("mainScript Start: Reset slot save slot and state to fresh game settings.");
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
            // Clear and reset the slots state when a subsidiary is sold
            SubsidiaryTeamSlotsPlugin.StudioSlotData data = SubsidiaryTeamSlotsPlugin.GetStudioSlotData(__instance.myID);
            if (data != null && data.slots != null)
            {
                games gamesScript = __instance.games_ != null ? __instance.games_ : SubsidiaryTeamSlotsPlugin.GetGamesScript();
                for (int i = 0; i < data.slots.Length; i++)
                {
                    SubsidiaryTeamSlotsPlugin.SlotData slot = data.slots[i];
                    if (slot != null)
                    {
                        if (slot.gameID != -1)
                        {
                            gameScript game = SubsidiaryTeamSlotsPlugin.FindGameByID(gamesScript, slot.gameID);
                            if (game != null)
                            {
                                game.inDevelopment = false;
                                game.gameObject.tag = "GameRemoved";
                                UnityEngine.Object.Destroy(game.gameObject);
                                SubsidiaryTeamSlotsPlugin.Log?.LogInfo($"[Sell Cleanup] Destroyed active project game ID {slot.gameID} in slot {i} of sold subsidiary '{__instance.GetName()}'");
                            }
                        }
                        slot.gameID = -1;
                        slot.remainingWeeks = 0f;
                        slot.totalWeeks = 0f;
                        slot.isPlayerAssigned = false;
                        slot.isUnlocked = (i == 0); // only first slot remains unlocked
                        slot.stars = 1;
                        slot.speed = 0;
                        slot.helpingSlotIndex = -1;
                    }
                }
                data.unlockedSlots = 1;
                data.closedYear = -1;
                data.closedMonth = -1;
                SubsidiaryTeamSlotsPlugin.SaveState(SubsidiaryTeamSlotsPlugin.currentSaveSlot);
                SubsidiaryTeamSlotsPlugin.Log?.LogInfo($"[Sell Cleanup] Reset team slots data for sold subsidiary '{__instance.GetName()}' (ID={__instance.myID})");
            }
        }
        catch (System.Exception ex)
        {
            SubsidiaryTeamSlotsPlugin.Log?.LogWarning("Error cleaning up sold subsidiary slots: " + ex);
        }
    }
}

// Intercept weekly tick — replace vanilla CreateNewGame2 with our parallel tick
[HarmonyPatch(typeof(publisherScript), "CreateNewGame2")]
public static class PublisherScript_CreateNewGame2_Patch
{
    public static bool Prefix(publisherScript __instance)
    {
        // If we called this ourselves for an autonomous slot, let it through
        if (SubsidiaryTeamSlotsPlugin.isBypassingPrefix) return true;
        if (__instance == null || !__instance) return true;

        try
        {
            if (__instance.IsTochterfirma() && __instance.IsMyTochterfirma())
            {
                SubsidiaryTeamSlotsPlugin.TickActiveProjects(__instance);
                return false; // Bypass vanilla CreateNewGame2 loop
            }
        }
        catch (System.Exception)
        {
        }
        return true;
    }
}

// Capture project assignment from autonomous creation
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

        // Determine which slot is initializing
        int slotIdx = -1;
        if (SubsidiaryTeamSlotsPlugin.isCreatingAutonomousSlot)
        {
            slotIdx = SubsidiaryTeamSlotsPlugin.targetSlotIndex;
        }
        else if (SubsidiaryTeamSlotsPlugin.isCreatingPlayerSlotProject)
        {
            slotIdx = SubsidiaryTeamSlotsPlugin.selectedSlotIndex;
        }

        if (slotIdx >= 0 && slotIdx < 3)
        {
            publisherScript studio = SubsidiaryTeamSlotsPlugin.FindStudioByID(__instance.developerID);
            if (studio != null)
            {
                SubsidiaryTeamSlotsPlugin.StudioSlotData data = SubsidiaryTeamSlotsPlugin.GetStudioSlotData(studio.myID);
                SubsidiaryTeamSlotsPlugin.SlotData slot = data.slots[slotIdx];

                // Backup original values
                originalStars = studio.stars;
                originalSpeed = studio.developmentSpeed;
                activeSlotIndex = slotIdx;

                if (slotIdx > 0)
                {
                    // Swap values temporarily so the timeline mod calculates duration based on this team's level
                    studio.stars = slot.stars * 20f;
                    studio.developmentSpeed = slot.speed;
                }

                // Set context variable
                SubsidiaryTeamSlotsPlugin.currentInitializingGame = __instance;
            }
        }
    }

    [HarmonyPostfix]
    public static void Postfix(gameScript __instance)
    {
        // Restore original values
        if (activeSlotIndex >= 0)
        {
            publisherScript studio = SubsidiaryTeamSlotsPlugin.FindStudioByID(__instance.developerID);
            if (studio != null)
            {
                studio.stars = originalStars;
                studio.developmentSpeed = originalSpeed;
            }
            activeSlotIndex = -1;
            SubsidiaryTeamSlotsPlugin.currentInitializingGame = null;
        }

        if (SubsidiaryTeamSlotsPlugin.isCreatingAutonomousSlot)
        {
            int slotIdx = SubsidiaryTeamSlotsPlugin.targetSlotIndex;
            if (slotIdx < 0 || slotIdx > 2) return;

            publisherScript studio = SubsidiaryTeamSlotsPlugin.FindStudioByID(__instance.developerID);
            if (studio != null)
            {
                SubsidiaryTeamSlotsPlugin.StudioSlotData data = SubsidiaryTeamSlotsPlugin.GetStudioSlotData(studio.myID);

                // Safety: ensure we don't overwrite another slot that already has this game
                for (int i = 0; i < 3; i++)
                {
                    if (i != slotIdx && data.slots[i].gameID == __instance.myID)
                    {
                        SubsidiaryTeamSlotsPlugin.Log?.LogWarning($"Autonomous: game {__instance.myID} already in slot {i}, skipping slot {slotIdx}");
                        return;
                    }
                }

                // Use DynamicSubsidiaryTimeline's calculated weeks if already set, otherwise use studio value
                float baseDuration = studio.newGameInWeeks;
                if (baseDuration <= 2f) baseDuration = (studio.newGameInWeeksORG > 2f) ? studio.newGameInWeeksORG : 20f;

                int finalWeeks = Mathf.Max(4, Mathf.RoundToInt(baseDuration));

                data.slots[slotIdx].gameID = __instance.myID;
                data.slots[slotIdx].isPlayerAssigned = false;
                data.slots[slotIdx].remainingWeeks = finalWeeks;
                data.slots[slotIdx].totalWeeks = finalWeeks;

                SubsidiaryTeamSlotsPlugin.Log?.LogInfo($"Slot {slotIdx} autonomously started: '{__instance.myName}' ({finalWeeks}w, base={baseDuration}w)");
            }
        }
        else if (SubsidiaryTeamSlotsPlugin.isCreatingPlayerSlotProject)
        {
            int slotIdx = SubsidiaryTeamSlotsPlugin.selectedSlotIndex;
            if (slotIdx < 0 || slotIdx > 2) return;

            publisherScript studio = SubsidiaryTeamSlotsPlugin.FindStudioByID(__instance.developerID);
            if (studio != null)
            {
                SubsidiaryTeamSlotsPlugin.StudioSlotData data = SubsidiaryTeamSlotsPlugin.GetStudioSlotData(studio.myID);

                // Safety: don't double-assign if this game is already in this slot
                if (data.slots[slotIdx].gameID == __instance.myID)
                {
                    SubsidiaryTeamSlotsPlugin.Log?.LogWarning($"Player: game {__instance.myID} already in slot {slotIdx}, skipping duplicate assignment.");
                    return;
                }

                // Use the duration captured by DynamicSubsidiaryTimeline
                float duration = SubsidiaryTeamSlotsPlugin.pendingPlayerSlotDuration;
                if (duration <= 0f) duration = studio.newGameInWeeks;
                if (duration <= 2f) duration = 20f; // Fallback

                int finalWeeks = Mathf.Max(4, Mathf.RoundToInt(duration));

                data.slots[slotIdx].gameID = __instance.myID;
                data.slots[slotIdx].isPlayerAssigned = true;
                data.slots[slotIdx].remainingWeeks = finalWeeks;
                data.slots[slotIdx].totalWeeks = finalWeeks;

                // Lock vanilla weeks counter so vanilla doesn't tick down
                studio.newGameInWeeks = 9999;
                studio.newGameInWeeksORG = 9999;

                SubsidiaryTeamSlotsPlugin.pendingPlayerSlotDuration = -1f;
                SubsidiaryTeamSlotsPlugin.Log?.LogInfo($"Slot {slotIdx} player-designed: '{__instance.myName}' ({finalWeeks}w)");
            }
        }
    }
}

// Integration with Dynamic Subsidiary Timeline game finder
public static class DynamicSubsidiaryTimeline_SafeFindGame_Patch
{
    public static bool Prefix(ref gameScript __result)
    {
        if (SubsidiaryTeamSlotsPlugin.currentInitializingGame != null)
        {
            __result = SubsidiaryTeamSlotsPlugin.currentInitializingGame;
            return false; // Return initializing game
        }
        return true;
    }
}


// Patch card on subsidiary page to display active slot projects


// GUI Hooks - Subsidiary Details Panel
[HarmonyPatch(typeof(Menu_Stats_Tochterfirma_Main), "UpdateData")]
public static class Menu_Stats_Tochterfirma_Main_UpdateData_Patch
{
    public static void Prefix(Menu_Stats_Tochterfirma_Main __instance)
    {
        if (__instance == null || !__instance) return;
        try
        {
            if (__instance.uiObjects == null || __instance.uiObjects.Length <= 31) return;

            publisherScript studio = SubsidiaryTeamSlotsPlugin.f_pS.GetValue(__instance) as publisherScript;
            if (studio != null && studio.developer)
            {
                // Sync Slot 1 data to vanilla widgets so slot 1 renders naturally in the main panel
                SubsidiaryTeamSlotsPlugin.StudioSlotData data = SubsidiaryTeamSlotsPlugin.GetStudioSlotData(studio.myID);
                if (data == null || data.slots == null || data.slots.Length == 0) return;
                
                games gamesScript = studio.games_ != null ? studio.games_ : SubsidiaryTeamSlotsPlugin.GetGamesScript();

                gameScript slot1Game = SubsidiaryTeamSlotsPlugin.FindGameByID(gamesScript, data.slots[0].gameID);
                if (SubsidiaryTeamSlotsPlugin.f_nextGame != null)
                {
                    SubsidiaryTeamSlotsPlugin.f_nextGame.SetValue(__instance, slot1Game);
                }

                if (slot1Game != null)
                {
                    float acceleration = SubsidiaryTeamSlotsPlugin.GetAccelerationFactor(studio, 0);
                    int minFloor = SubsidiaryTeamSlotsPlugin.GetMinimumFloorWeeks(slot1Game.gameSize);
                    
                    int calculatedWeeks = Mathf.RoundToInt(data.slots[0].remainingWeeks / acceleration);
                    studio.newGameInWeeks = Mathf.Max(minFloor, Mathf.Max(2, calculatedWeeks));
                    
                    int calculatedTotalWeeks = Mathf.RoundToInt(data.slots[0].totalWeeks / acceleration);
                    studio.newGameInWeeksORG = Mathf.Max(minFloor, Mathf.Max(2, calculatedTotalWeeks));
                }
                else
                {
                    studio.newGameInWeeks = 0;
                    studio.newGameInWeeksORG = 0;
                }
            }
        }
        catch (System.Exception ex)
        {
            UnityEngine.Debug.LogError("Error in SubsidiaryTeamSlots UpdateData Prefix patch: " + ex);
        }
    }

    public static void Postfix(Menu_Stats_Tochterfirma_Main __instance)
    {
        if (__instance == null || !__instance) return;
        try
        {
            publisherScript studio = SubsidiaryTeamSlotsPlugin.f_pS.GetValue(__instance) as publisherScript;
            SubsidiaryTeamSlotsPlugin.RefreshSlotUI(__instance, studio);
        }
        catch (System.Exception ex)
        {
            UnityEngine.Debug.LogError("Error in SubsidiaryTeamSlots UpdateData Postfix patch: " + ex);
        }
    }
}

[HarmonyPatch(typeof(Menu_Stats_Tochterfirma_Main), "Update")]
public static class Menu_Stats_Tochterfirma_Main_Update_Patch
{
    public static void Postfix(Menu_Stats_Tochterfirma_Main __instance)
    {
        if (__instance == null || !__instance) return;
        try
        {
            publisherScript studio = SubsidiaryTeamSlotsPlugin.f_pS.GetValue(__instance) as publisherScript;
            if (studio != null && studio.developer && __instance.uiObjects != null && __instance.uiObjects.Length > 29)
            {
                // Update the side panel slots in real-time every frame
                SubsidiaryTeamSlotsPlugin.RefreshSlotUI(__instance, studio);

                // Sync Slot 1 state to the main window in real-time
                SubsidiaryTeamSlotsPlugin.StudioSlotData data = SubsidiaryTeamSlotsPlugin.GetStudioSlotData(studio.myID);
                if (data == null || data.slots == null || data.slots.Length == 0) return;
                
                SubsidiaryTeamSlotsPlugin.SlotData slot1 = data.slots[0];
                if (slot1 == null) return;
                
                if (slot1.gameID == -1)
                {
                    // Slot 1 is idle
                    var txt = __instance.uiObjects[29]?.GetComponent<Text>();
                    if (txt != null) txt.text = "";
                    var fill = __instance.uiObjects[19]?.GetComponent<Image>();
                    if (fill != null) fill.fillAmount = 0f;
                    var prgTxt = __instance.uiObjects[20]?.GetComponent<Text>();
                    if (prgTxt != null) prgTxt.text = "";
                }
                else
                {
                    // Slot 1 is active
                    float acceleration = SubsidiaryTeamSlotsPlugin.GetAccelerationFactor(studio, 0);

                    games gamesScript = studio.games_ != null ? studio.games_ : SubsidiaryTeamSlotsPlugin.GetGamesScript();
                    gameScript slot1Game = SubsidiaryTeamSlotsPlugin.FindGameByID(gamesScript, slot1.gameID);
                    int minFloor = (slot1Game != null) ? SubsidiaryTeamSlotsPlugin.GetMinimumFloorWeeks(slot1Game.gameSize) : 1;

                    float progress = (slot1.totalWeeks > 0f) ? (1f - (slot1.remainingWeeks / slot1.totalWeeks)) : 0f;
                    progress = Mathf.Clamp01(progress);

                    // Update progress fill
                    var fill = __instance.uiObjects[19]?.GetComponent<Image>();
                    if (fill != null) fill.fillAmount = progress;

                    // Update progress text
                    var prgTxt = __instance.uiObjects[20]?.GetComponent<Text>();
                    if (prgTxt != null) prgTxt.text = $"Progress: {Mathf.RoundToInt(progress * 100f)}%";

                    // Update weeks remaining (with minimum floor)
                    int calculatedWeeks = Mathf.CeilToInt(slot1.remainingWeeks / acceleration);
                    int weeks = Mathf.Max(minFloor, calculatedWeeks);
                    var txt = __instance.uiObjects[29]?.GetComponent<Text>();
                    if (txt != null) txt.text = weeks.ToString();

                    // Update clock tooltip
                    var clockIcon = __instance.uiObjects[28];
                    if (clockIcon != null)
                    {
                        tooltip clockTooltip = clockIcon.GetComponent<tooltip>();
                        textScript tS = SubsidiaryTeamSlotsPlugin.f_tS.GetValue(__instance) as textScript;
                        if (clockTooltip != null && tS != null)
                        {
                            string text = tS.GetText(1948);
                            text = text.Replace("<NUM>", "<color=blue><b>" + weeks + "</b></color>");
                            clockTooltip.c = text;
                        }
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            UnityEngine.Debug.LogError("Error in SubsidiaryTeamSlots Update Postfix patch: " + ex);
        }
    }
}

[HarmonyPatch(typeof(Menu_Stats_Tochterfirma_Main), "ResetGame")]
public static class Menu_Stats_Tochterfirma_Main_ResetGame_Patch
{
    public static bool Prefix(Menu_Stats_Tochterfirma_Main __instance)
    {
        publisherScript studio = SubsidiaryTeamSlotsPlugin.f_pS.GetValue(__instance) as publisherScript;
        if (studio != null)
        {
            SubsidiaryTeamSlotsPlugin.StudioSlotData data = SubsidiaryTeamSlotsPlugin.GetStudioSlotData(studio.myID);
            int slotIdx = SubsidiaryTeamSlotsPlugin.slotToDiscard >= 0 ? SubsidiaryTeamSlotsPlugin.slotToDiscard : 0;

            if (slotIdx < 3)
            {
                data.slots[slotIdx].gameID = -1;
                data.slots[slotIdx].remainingWeeks = 0f;
                data.slots[slotIdx].totalWeeks = 0f;
                data.slots[slotIdx].isPlayerAssigned = false;
            }

            SubsidiaryTeamSlotsPlugin.slotToDiscard = -1;
            SubsidiaryTeamSlotsPlugin.SaveState(SubsidiaryTeamSlotsPlugin.currentSaveSlot);

            __instance.UpdateData();
            return false; // Skip vanilla
        }
        return true;
    }
}

// =========================================================
//  SUBSIDIARY PROJECT DIRECTOR INTEGRATION
// =========================================================

[HarmonyPatch(typeof(SubsidiaryProjectDirectorPlugin), "BeginDesignForSubsidiary")]
public static class SubsidiaryProjectDirector_BeginDesignForSubsidiary_Patch
{
    public static bool Prefix(publisherScript subsidiary)
    {
        if (subsidiary == null) return true;

        SubsidiaryTeamSlotsPlugin.StudioSlotData data = SubsidiaryTeamSlotsPlugin.GetStudioSlotData(subsidiary.myID);

        // Check if there are idle slots available
        bool hasIdle = false;
        for (int i = 0; i < data.unlockedSlots; i++)
        {
            if (data.slots[i].gameID == -1)
            {
                hasIdle = true;
                break;
            }
        }

        if (!hasIdle)
        {
            GUI_Main gui = UnityEngine.Object.FindObjectOfType<GUI_Main>();
            if (gui != null)
            {
                gui.MessageBox("All development teams are currently busy! Discard or wait for a project to finish first.", closeMenu: false);
            }
            return false;
        }

        if (data.unlockedSlots <= 1)
        {
            // Only 1 slot unlocked: auto-select slot 0
            SubsidiaryTeamSlotsPlugin.selectedSlotIndex = 0;
            SubsidiaryTeamSlotsPlugin.isDesigningForSlot = true;
            return true;
        }

        if (SubsidiaryTeamSlotsPlugin.isDesigningForSlot)
        {
            // Already went through picker, let it run
            return true;
        }

        // Open slot picker
        SubsidiaryTeamSlotsPlugin.showSlotPicker = true;
        SubsidiaryTeamSlotsPlugin.pickerTarget = subsidiary;
        return false; // Block until user picks
    }
}

[HarmonyPatch(typeof(SubsidiaryProjectDirectorPlugin), "FindActiveSubsidiaryProject")]
public static class SubsidiaryProjectDirector_FindActiveSubsidiaryProject_Patch
{
    public static bool Prefix(ref gameScript __result)
    {
        // When we're designing for a specific slot, bypass the "already has a project" check
        if (SubsidiaryTeamSlotsPlugin.isDesigningForSlot)
        {
            __result = null;
            return false;
        }
        return true;
    }
}

/// <summary>
/// Intercept the "clean up other active games" logic inside ConvertCreatedGameToSubsidiaryProject
/// to ensure it does NOT destroy games that belong to other active slots.
/// We do this by temporarily wrapping the destroy logic via the Prefix to set up context.
/// </summary>
[HarmonyPatch(typeof(SubsidiaryProjectDirectorPlugin), "ConvertCreatedGameToSubsidiaryProject")]
public static class SubsidiaryProjectDirector_ConvertCreatedGame_Patch
{
    public static void Prefix()
    {
        SubsidiaryTeamSlotsPlugin.isCreatingPlayerSlotProject = true;
        SubsidiaryTeamSlotsPlugin.pendingPlayerSlotDuration = -1f;
    }

    public static void Postfix()
    {
        SubsidiaryTeamSlotsPlugin.isCreatingPlayerSlotProject = false;
        SubsidiaryTeamSlotsPlugin.isDesigningForSlot = false;
        SubsidiaryTeamSlotsPlugin.SaveState(SubsidiaryTeamSlotsPlugin.currentSaveSlot);
    }
}

/// <summary>
/// Intercept the "destroy other in-development games for this subsidiary" cleanup loop
/// inside ConvertCreatedGameToSubsidiaryProject. We prevent it from destroying games
/// that are tracked in OTHER slots.
/// </summary>
[HarmonyPatch(typeof(gameScript), "get_inDevelopment")]
public static class GameScript_InDevelopmentGetter_SafetyPatch
{
    // Not patching getter — instead we patch Object.Destroy indirectly via a tag filter.
    // The real fix is patching the loop in ConvertCreatedGameToSubsidiaryProject which checks
    // oldGame.inDevelopment && oldGame.developerID == subsidiary.myID.
    // We handle this by NOT allowing that code path to destroy our slot-tracked games.
    // This is done via the gameObject tag "GameRemoved" in the original code.
}

// The actual fix: patch gameObject.tag assignment to "GameRemoved" when
// isCreatingPlayerSlotProject is true, to prevent destroying our other-slot games.
// We achieve this by patching at the method level via a Transpiler on ConvertCreatedGameToSubsidiaryProject
// — but that requires IL editing. Instead, we patch the simpler entry point:
// Since the original code calls `oldGame.gameObject.tag = "GameRemoved"` followed by Destroy,
// we patch the Destroy call by patching Object.Destroy for GameObjects with our tracked game IDs.

[HarmonyPatch(typeof(UnityEngine.Object), "Destroy", new System.Type[] { typeof(UnityEngine.Object) })]
public static class UnityObjectDestroy_Patch
{
    public static bool Prefix(UnityEngine.Object obj)
    {
        // Only intercept during ConvertCreatedGame cleanup to protect our slot-tracked games
        if (!SubsidiaryTeamSlotsPlugin.isCreatingPlayerSlotProject) return true;

        GameObject go = obj as GameObject;
        if (go == null) return true;

        gameScript game = go.GetComponent<gameScript>();
        if (game == null) return true;

        // Check if this game is tracked in any slot other than the one being assigned
        if (SubsidiaryTeamSlotsPlugin.State?.studios != null)
        {
            foreach (var studio in SubsidiaryTeamSlotsPlugin.State.studios)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (studio.slots[i].gameID == game.myID)
                    {
                        SubsidiaryTeamSlotsPlugin.Log?.LogInfo($"[SafeDestroy] Blocked destruction of slot-tracked game '{game.myName}' (ID={game.myID}) in slot {i}");
                        // Undo the "GameRemoved" tag so the game stays alive
                        go.tag = "Game";
                        return false; // Cancel destroy
                    }
                }
            }
        }

        return true; // Allow destroy for non-tracked games
    }
}

[HarmonyPatch(typeof(publisherScript), "ReleaseGameInDevelopment")]
public static class PublisherScript_ReleaseGameInDevelopment_Patch
{
    public static bool Prefix(publisherScript __instance)
    {
        if (__instance == null || !__instance) return true;
        try
        {
            if (__instance.IsTochterfirma() && __instance.IsMyTochterfirma())
            {
                SubsidiaryTeamSlotsPlugin.Log?.LogInfo($"Bypassing vanilla ReleaseGameInDevelopment for subsidiary '{__instance.GetName()}'");
                return false; // Skip vanilla ReleaseGameInDevelopment entirely
            }
        }
        catch (System.Exception)
        {
        }
        return true;
    }
}

public class STSWindowLifecycleHook : MonoBehaviour
{
    private void OnDisable()
    {
        SubsidiaryTeamSlotsPlugin.ClearClonedUI();
    }
}

[HarmonyPatch(typeof(Menu_Stats_Tochterfirma_GameEntwicklungsbericht), "Init")]
public static class Menu_Stats_Tochterfirma_GameEntwicklungsbericht_Init_Patch
{
    public static void Postfix(Menu_Stats_Tochterfirma_GameEntwicklungsbericht __instance, gameScript game_)
    {
        if (game_ == null) return;
        publisherScript studio = SubsidiaryTeamSlotsPlugin.FindStudioByID(game_.developerID);
        if (studio == null) return;

        // Check if this game is managed in any slot
        SubsidiaryTeamSlotsPlugin.StudioSlotData data = SubsidiaryTeamSlotsPlugin.GetStudioSlotData(studio.myID);
        int slotIdx = -1;
        for (int i = 0; i < 3; i++)
        {
            if (data.slots[i].gameID == game_.myID)
            {
                slotIdx = i;
                break;
            }
        }

        if (slotIdx >= 0)
        {
            SubsidiaryTeamSlotsPlugin.SlotData slot = data.slots[slotIdx];
            
            float acceleration = SubsidiaryTeamSlotsPlugin.GetAccelerationFactor(studio, slotIdx);
            int minFloor = SubsidiaryTeamSlotsPlugin.GetMinimumFloorWeeks(game_.gameSize);

            float progress = (slot.totalWeeks > 0f) ? (1f - (slot.remainingWeeks / slot.totalWeeks)) : 0f;
            progress = Mathf.Clamp01(progress);

            // Calculate weeks with minimum floor enforced
            int calculatedWeeks = Mathf.CeilToInt(slot.remainingWeeks / acceleration);
            int weeks = Mathf.Max(minFloor, calculatedWeeks);

            if (__instance.uiObjects != null)
            {
                if (__instance.uiObjects.Length > 2 && __instance.uiObjects[2] != null)
                {
                    Image img = __instance.uiObjects[2].GetComponent<Image>();
                    if (img != null) img.fillAmount = progress;
                }

                textScript tS = null;
                mainScript mS = UnityEngine.Object.FindObjectOfType<mainScript>();
                if (mS != null) tS = mS.tS_;

                if (tS != null)
                {
                    if (__instance.uiObjects.Length > 3 && __instance.uiObjects[3] != null)
                    {
                        Text txt = __instance.uiObjects[3].GetComponent<Text>();
                        if (txt != null)
                        {
                            if (slot.remainingWeeks > 2f)
                                txt.text = tS.GetText(1944) + ": " + Mathf.RoundToInt(progress * 100f) + "%";
                            else
                                txt.text = tS.GetText(1947);
                        }
                    }

                    if (__instance.uiObjects.Length > 31 && __instance.uiObjects[31] != null)
                    {
                        Text txt = __instance.uiObjects[31].GetComponent<Text>();
                        if (txt != null)
                        {
                            string text = tS.GetText(1948);
                            text = text.Replace("<NUM>", "<color=blue><b>" + weeks + "</b></color>");
                            txt.text = text;
                        }
                    }

                    if (__instance.uiObjects.Length > 5 && __instance.uiObjects[5] != null)
                    {
                        Text txt = __instance.uiObjects[5].GetComponent<Text>();
                        if (txt != null)
                        {
                            game_.CalcReview(entwicklungsbericht: true);
                            float f = (float)game_.reviewTotal * progress;
                            int num2 = Mathf.RoundToInt(f) - 10;
                            int num3 = Mathf.RoundToInt(f) + 10;
                            num2 = num2 / 10 * 10;
                            num3 = num3 / 10 * 10;
                            if (num2 < 1) num2 = 1;
                            if (num3 > 100) num3 = 100;
                            string textRange = " " + num2 + "% - " + num3 + "%";
                            txt.text = tS.GetText(452) + "<color=blue>" + textRange + "</color>";
                            game_.ClearReview();
                        }
                    }
                }

                if (__instance.uiObjects.Length > 6 && __instance.uiObjects[6] != null)
                {
                    genres genComp = UnityEngine.Object.FindObjectOfType<genres>();
                    Image img = __instance.uiObjects[6].GetComponent<Image>();
                    if (genComp != null && img != null)
                    {
                        img.sprite = genComp.GetScreenshot(game_.maingenre, Mathf.RoundToInt(game_.points_grafik * progress));
                    }
                }

                if (__instance.uiObjects.Length > 7 && __instance.uiObjects[7] != null)
                {
                    Text txt = __instance.uiObjects[7].GetComponent<Text>();
                    if (txt != null) txt.text = Mathf.RoundToInt(game_.points_gameplay * progress).ToString();
                }
                if (__instance.uiObjects.Length > 8 && __instance.uiObjects[8] != null)
                {
                    Text txt = __instance.uiObjects[8].GetComponent<Text>();
                    if (txt != null) txt.text = Mathf.RoundToInt(game_.points_grafik * progress).ToString();
                }
                if (__instance.uiObjects.Length > 9 && __instance.uiObjects[9] != null)
                {
                    Text txt = __instance.uiObjects[9].GetComponent<Text>();
                    if (txt != null) txt.text = Mathf.RoundToInt(game_.points_sound * progress).ToString();
                }
                if (__instance.uiObjects.Length > 10 && __instance.uiObjects[10] != null)
                {
                    Text txt = __instance.uiObjects[10].GetComponent<Text>();
                    if (txt != null) txt.text = Mathf.RoundToInt(game_.points_technik * progress).ToString();
                }

                if (__instance.uiObjects.Length > 17 && __instance.uiObjects[17] != null)
                {
                    Component_Aufwertungen comp = __instance.uiObjects[17].GetComponent<Component_Aufwertungen>();
                    if (comp != null)
                    {
                        comp.InitNpcGame(game_, progress);
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(gameScript), "GetNameWithTag")]
    public static class GameScriptGetNameWithTagPatch
    {
        [HarmonyPrefix]
        public static bool Prefix(gameScript __instance, ref string __result)
        {
            try
            {
                if (__instance == null)
                {
                    __result = "";
                    return false;
                }

                if (__instance.mS_ == null || !__instance.mS_)
                {
                    mainScript mS = UnityEngine.Object.FindObjectOfType<mainScript>();
                    if (mS != null)
                    {
                        __instance.mS_ = mS;
                    }
                }

                if (__instance.tS_ == null || !__instance.tS_)
                {
                    textScript tS = UnityEngine.Object.FindObjectOfType<textScript>();
                    if (tS != null)
                    {
                        __instance.tS_ = tS;
                    }
                }

                if (__instance.mS_ == null || !__instance.mS_ || __instance.mS_.settings_ == null || !__instance.mS_.settings_)
                {
                    __result = __instance.myName ?? "";
                    return false;
                }

                return true;
            }
            catch (System.Exception)
            {
                __result = __instance?.myName ?? "";
                return false;
            }
        }
    }
}

