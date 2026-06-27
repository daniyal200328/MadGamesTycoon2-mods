using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public partial class Subsidiary2Plugin
{
    [System.Serializable]
    public class StudioData
    {
        public int studioID;
        public long creationCost;
        public long upgradeInvestments;
        public int creationYear;
        public int creationMonth;
    }

    [System.Serializable]
    public class ModState
    {
        public List<StudioData> studios = new List<StudioData>();
    }

    private static ModState state = new ModState();

    private static string SavePath =>
        Path.Combine(Path.GetDirectoryName(typeof(Subsidiary2Plugin).Assembly.Location), "SubsidiaryData.json");

    private static void LoadState()
    {
        try
        {
            string path = SavePath;
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                state = JsonUtility.FromJson<ModState>(json);
                if (state == null) state = new ModState();
                if (state.studios == null) state.studios = new List<StudioData>();
                if (log != null) log.LogInfo($"Loaded {state.studios.Count} organic studios from persistent data.");
            }
            else
            {
                state = new ModState();
                state.studios = new List<StudioData>();
            }
        }
        catch (System.Exception ex)
        {
            if (log != null) log.LogError($"LoadState failed: {ex}");
            state = new ModState();
        }
    }

    internal static void SaveState()
    {
        try
        {
            string path = SavePath;
            string json = JsonUtility.ToJson(state, true);
            File.WriteAllText(path, json);
        }
        catch (System.Exception ex)
        {
            if (log != null) log.LogError($"SaveState failed: {ex}");
        }
    }

    public static StudioData GetStudioData(int id)
    {
        if (state == null) state = new ModState();
        if (state.studios == null) state.studios = new List<StudioData>();
        return state.studios.FirstOrDefault(s => s.studioID == id);
    }

    public static void AddStudioData(int id, long cost, int year, int month)
    {
        if (state == null) state = new ModState();
        if (state.studios == null) state.studios = new List<StudioData>();
        StudioData data = GetStudioData(id);
        if (data == null)
        {
            data = new StudioData { studioID = id };
            state.studios.Add(data);
        }
        data.creationCost = cost;
        data.creationYear = year;
        data.creationMonth = month;
        data.upgradeInvestments = 0L;
        SaveState();
    }
}
