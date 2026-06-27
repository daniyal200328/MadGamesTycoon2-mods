using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

[BepInPlugin("org.bepinex.plugins.subsidiaryteams", "Subsidiary Teams", "2.0.0")]
public partial class SubsidiaryTeamsPlugin : BaseUnityPlugin
{
    internal static SubsidiaryTeamsPlugin Instance;
    internal static ManualLogSource Log;

    internal static ConfigEntry<long> Slot2UnlockCost;
    internal static ConfigEntry<long> Slot3UnlockCost;

    private static games cachedGames;
    private static mainScript cachedMain;
    private static GUI_Main cachedGui;
    private static sfxScript cachedSfx;
    private static textScript cachedText;

    private static Type cachedDSTType;
    private static bool searchedDST;

    internal static games GetGamesScript()
    {
        if (cachedGames == null) cachedGames = UnityEngine.Object.FindObjectOfType<games>();
        return cachedGames;
    }

    internal static mainScript GetMainScript()
    {
        if (cachedMain == null) cachedMain = UnityEngine.Object.FindObjectOfType<mainScript>();
        return cachedMain;
    }

    internal static GUI_Main GetGuiMain()
    {
        if (cachedGui == null) cachedGui = UnityEngine.Object.FindObjectOfType<GUI_Main>();
        return cachedGui;
    }

    internal static sfxScript GetSfxScript()
    {
        if (cachedSfx == null) cachedSfx = UnityEngine.Object.FindObjectOfType<sfxScript>();
        return cachedSfx;
    }

    internal static textScript GetTextScript()
    {
        if (cachedText == null)
        {
            mainScript main = GetMainScript();
            if (main != null) cachedText = main.tS_;
            if (cachedText == null) cachedText = UnityEngine.Object.FindObjectOfType<textScript>();
        }
        return cachedText;
    }

    internal static Type GetDSTPluginType()
    {
        if (!searchedDST)
        {
            searchedDST = true;
            var t = Type.GetType("DynamicSubsidiaryTimelinePlugin, DynamicSubsidiaryTimeline");
            if (t == null)
            {
                foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    t = asm.GetType("DynamicSubsidiaryTimelinePlugin");
                    if (t != null) break;
                }
            }
            cachedDSTType = t;
        }
        return cachedDSTType;
    }

    internal static bool IsOrganicStudio(publisherScript studio)
    {
        if (studio == null) return false;
        return (studio.myID >= 9000 && studio.myID < 10000)
            || (studio.myID >= 90000 && studio.myID < 100000);
    }

    internal static string GetSlotTeamName(publisherScript studio, int slotIdx, StudioSlotData data)
    {
        if (data == null || slotIdx < 0 || slotIdx >= data.slots.Length) return $"Team {slotIdx + 1}";
        if (slotIdx == 0) return !string.IsNullOrEmpty(data.slots[0].teamName) ? data.slots[0].teamName : (studio != null ? studio.GetName() : "Main Team");
        return !string.IsNullOrEmpty(data.slots[slotIdx].teamName) ? data.slots[slotIdx].teamName : $"Team {slotIdx + 1}";
    }

    internal static float GetParallelDurationMultiplier(int activeCount)
    {
        if (activeCount == 2) return 1.74f;
        if (activeCount == 3) return 2.22f;
        return 1.00f;
    }

    internal static gameScript FindGameByID(games gamesScript, int gameID)
    {
        if (gamesScript == null || gamesScript.arrayGamesScripts == null) return null;
        for (int i = 0; i < gamesScript.arrayGamesScripts.Length; i++)
        {
            if (gamesScript.arrayGamesScripts[i] != null && gamesScript.arrayGamesScripts[i].myID == gameID)
                return gamesScript.arrayGamesScripts[i];
        }
        return null;
    }

    internal static publisherScript FindStudioByID(int studioID)
    {
        publisherScript[] allStudios = UnityEngine.Object.FindObjectsOfType<publisherScript>();
        if (allStudios != null)
        {
            foreach (publisherScript p in allStudios)
            {
                if (p != null && p.myID == studioID) return p;
            }
        }
        return null;
    }

    internal static bool IsGameTrackedInOtherSlot(int studioID, int gameID, int slotToIgnore = -1)
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

    public static void InvalidateStudioDevUI()
    {
        lastBuiltForStudio = -1;
    }

    internal static bool showSlotPicker = false;
    internal static Rect pickerRect;
    internal static publisherScript pickerTarget;

    internal static bool showRenameWindow = false;
    internal static Rect renameRect = new Rect(Screen.width / 2f - 150f, Screen.height / 2f - 75f, 300f, 150f);
    internal static int renameSlotIndex = -1;
    internal static int renameStudioID = -1;
    internal static string renameInputString = "";

    internal static bool isBypassingPrefix = false;
    public static bool isCreatingAutonomousSlot = false;
    public static int targetSlotIndex = -1;
    internal static bool isDesigningForSlot = false;
    public static int selectedSlotIndex = -1;
    public static bool isCreatingPlayerSlotProject = false;
    internal static int slotToDiscard = -1;
    internal static float pendingPlayerSlotDuration = -1f;
    internal static gameScript currentInitializingGame = null;

    private void Awake()
    {
        Instance = this;
        Log = Logger;

        Slot2UnlockCost = Config.Bind("Costs", "Slot2UnlockCost", 15000000L, "Cost to unlock team slot 2");
        Slot3UnlockCost = Config.Bind("Costs", "Slot3UnlockCost", 60000000L, "Cost to unlock team slot 3");

        pickerRect = new Rect(Screen.width / 2f - 200f, Screen.height / 2f - 150f, 400f, 300f);
        new Harmony("org.bepinex.plugins.subsidiaryteams").PatchAll();
        Log.LogInfo("Subsidiary Teams loaded successfully.");
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
        if (showRoleWindow)
        {
            GUI.depth = -1007;
            roleRect = GUI.Window(9997, roleRect, DrawRoleWindow, "Select Specialty Role", GUI.skin.window);
        }
        if (showTagWindow)
        {
            GUI.depth = -1008;
            tagWindowRect = GUI.Window(9996, tagWindowRect, DrawTagWindow, "Select Team Tag", GUI.skin.window);
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
                lastBuiltForStudio = -1;
                Menu_Stats_Tochterfirma_Main menu = UnityEngine.Object.FindObjectOfType<Menu_Stats_Tochterfirma_Main>();
                if (menu != null && menu.gameObject.activeInHierarchy) menu.UpdateData();
            }
        }
        if (GUILayout.Button("Cancel")) showRenameWindow = false;
        GUILayout.EndHorizontal();
        GUI.DragWindow();
    }

    private void DrawSlotPicker(int windowID)
    {
        if (pickerTarget == null) { showSlotPicker = false; return; }

        GUILayout.Space(10);
        GUILayout.Label("Select a team slot to assign the project:", GUI.skin.label);
        GUILayout.Space(10);

        StudioSlotData data = GetStudioSlotData(pickerTarget.myID);
        games gamesScript = pickerTarget.games_ ?? GetGamesScript();

        for (int i = 0; i < 3; i++)
        {
            SlotData slot = data.slots[i];
            string tName = string.IsNullOrEmpty(slot.teamName) ? $"Team {i + 1}" : slot.teamName;

            if (!slot.isUnlocked)
            {
                long cost = (i == 1) ? Slot2UnlockCost.Value : Slot3UnlockCost.Value;
                int reqStars = (i == 1) ? 3 : 5;
                GUILayout.BeginHorizontal();
                GUILayout.Label($"{tName}: Locked (Requires {reqStars} Stars)", GUILayout.Width(220f));
                if (GUILayout.Button($"Unlock (${cost:N0})")) TryPurchaseSlot(pickerTarget, i);
                GUILayout.EndHorizontal();
            }
            else
            {
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
                        StudioDirectorPlugin.BeginDesignForSubsidiary(pickerTarget);
                    }
                }
            }
            GUILayout.Space(5);
        }

        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Cancel")) { showSlotPicker = false; pickerTarget = null; }
        GUI.DragWindow();
    }
}

public class STSWindowLifecycleHook : MonoBehaviour
{
    public bool shiftedButtons = false;
    private void OnDisable()
    {
        SubsidiaryTeamsPlugin.ClearClonedUI();
    }
}
