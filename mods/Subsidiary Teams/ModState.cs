using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public partial class SubsidiaryTeamsPlugin
{
    [System.Serializable]
    public class SlotData
    {
        public int gameID = -1;
        public float remainingWeeks = 0f;
        public float totalWeeks = 0f;
        public bool isPlayerAssigned = false;
        public bool isUnlocked = false;
        public int unlockedYear = -1;
        public int unlockedMonth = -1;
        public string teamName = "";
        public int stars = 1;
        public int speed = 0;
        public int helpingSlotIndex = -1;
        public int role = 0;
        public float penaltyMultiplier = 1f;
        public int supportTarget = -1;
        public int teamXP = 0;
        public int traitID = -1;
        public int traitLevel = 0;
        public bool traitAwarded = false;
    }

    [System.Serializable]
    public class TrackedGameSales
    {
        public int gameID;
        public int studioID;
        public int slotIndex;
        public int milestonesMask; // Bitmask of reached milestones: 1=1M, 2=5M, 4=10M, 8=25M, 16=50M, 32=100M
    }

    [System.Serializable]
    public class StudioSlotData
    {
        public int studioID;
        public int unlockedSlots = 1;
        public SlotData[] slots = new SlotData[3] { new SlotData(), new SlotData(), new SlotData() };
        public int closedYear = -1;
        public int closedMonth = -1;
        public int studioXP = 0;
    }

    [System.Serializable]
    public class SlotSaveState
    {
        public List<StudioSlotData> studios = new List<StudioSlotData>();
        public List<TrackedGameSales> trackedGames = new List<TrackedGameSales>();
    }

    public static SlotSaveState State = new SlotSaveState();
    public static int currentSaveSlot = -1;

    private static string SaveDir => Application.persistentDataPath;

    private static string GetSavePath(int slot)
    {
        return Path.Combine(SaveDir, $"SaveGame_{slot}_slots.json");
    }

    internal static void LoadState(int slot)
    {
        currentSaveSlot = slot;
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
                if (State.trackedGames == null) State.trackedGames = new List<TrackedGameSales>();
                Log?.LogInfo($"Loaded slots state for save slot {slot}. Total studios: {State.studios.Count}");
            }
            else
            {
                State = new SlotSaveState();
                State.studios = new List<StudioSlotData>();
                State.trackedGames = new List<TrackedGameSales>();
                Log?.LogInfo($"No slots save file found for slot {slot}. Initializing empty state.");
            }
        }
        catch (System.Exception ex)
        {
            Log?.LogError($"Failed to load slots state: {ex}");
            State = new SlotSaveState();
            State.studios = new List<StudioSlotData>();
            State.trackedGames = new List<TrackedGameSales>();
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
            data.slots[0].stars = 1;
            data.slots[0].speed = 0;
            data.slots[0].helpingSlotIndex = -1;
            data.slots[1].teamName = "Team 2";
            data.slots[1].helpingSlotIndex = -1;
            data.slots[2].teamName = "Team 3";
            data.slots[2].helpingSlotIndex = -1;
            State.studios.Add(data);
        }
        else
        {
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
                if (data.slots[i] == null) data.slots[i] = new SlotData();
            }

            if (string.IsNullOrEmpty(data.slots[0].teamName)) data.slots[0].teamName = "Team 1";
            if (string.IsNullOrEmpty(data.slots[1].teamName)) data.slots[1].teamName = "Team 2";
            if (string.IsNullOrEmpty(data.slots[2].teamName)) data.slots[2].teamName = "Team 3";

            data.slots[0].isUnlocked = true;
            for (int i = 0; i < 3; i++)
            {
                if (data.slots[i].helpingSlotIndex < -1 || data.slots[i].helpingSlotIndex >= 3)
                    data.slots[i].helpingSlotIndex = -1;
            }
            for (int i = 1; i < 3; i++)
            {
                if (i < data.unlockedSlots) data.slots[i].isUnlocked = true;
            }
        }
        return data;
    }
}
