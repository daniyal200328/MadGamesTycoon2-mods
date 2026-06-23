using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[BepInPlugin("org.bepinex.plugins.customthemes", "Custom Themes", "0.1.0")]
public class CustomThemesPlugin : BaseUnityPlugin
{
    private static CustomThemesPlugin instance;
    private static ManualLogSource log;
    private static readonly HashSet<string> appliedThemes = new HashSet<string>(System.StringComparer.OrdinalIgnoreCase);
    private List<string> cachedThemeNames;

    private bool showWindow;
    private Rect windowRect;
    private Vector2 scroll;
    private string newThemeName = "";
    private string status = "";
    private mainScript main;
    private textScript text;
    private themes themesScript;
    private genres genresScript;

    private static string ConfigPath
    {
        get { return Path.Combine(Paths.ConfigPath, "CustomThemes.txt"); }
    }

    private void Awake()
    {
        instance = this;
        log = Logger;
        windowRect = new Rect(Screen.width / 2f - 260f, Screen.height / 2f - 180f, 520f, 360f);
        Directory.CreateDirectory(Paths.ConfigPath);
        if (!File.Exists(ConfigPath))
        {
            File.WriteAllText(ConfigPath, "");
        }

        new Harmony("org.bepinex.plugins.customthemes").PatchAll();
        log.LogInfo("Custom Themes loaded. Press Ctrl+Shift+T to add themes.");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && ShortcutHeld())
        {
            showWindow = !showWindow;
            if (showWindow)
            {
                cachedThemeNames = null; // force reload from disk on open
                RefreshScripts();
                ApplyCustomThemes();
                windowRect = new Rect(Screen.width / 2f - 260f, Screen.height / 2f - 180f, 520f, 360f);
            }
        }
    }

    private void OnGUI()
    {
        if (!showWindow)
        {
            return;
        }

        GUI.depth = -1000;
        windowRect = GUI.Window(1201, windowRect, DrawWindow, "Custom Themes");
    }

    private bool ShortcutHeld()
    {
        return (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) &&
               (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
    }

    private void DrawWindow(int id)
    {
        GUILayout.Space(8);
        GUILayout.Label("New Theme Name:");
        newThemeName = GUILayout.TextField(newThemeName ?? "", 40);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Theme", GUILayout.Height(30f)))
        {
            AddThemeFromInput();
        }
        if (GUILayout.Button("Reload", GUILayout.Width(100f), GUILayout.Height(30f)))
        {
            ApplyCustomThemes();
            status = "Reloaded custom themes.";
        }
        if (GUILayout.Button("Close", GUILayout.Width(100f), GUILayout.Height(30f)))
        {
            showWindow = false;
        }
        GUILayout.EndHorizontal();

        if (!string.IsNullOrEmpty(status))
        {
            GUILayout.Label(status);
        }

        GUILayout.Space(8);
        GUILayout.Label("Saved Themes:");
        scroll = GUILayout.BeginScrollView(scroll, GUILayout.Height(190f));
        if (cachedThemeNames == null)
        {
            cachedThemeNames = LoadThemeNames();
        }
        List<string> themes = cachedThemeNames;
        for (int i = 0; i < themes.Count; i++)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("- " + themes[i]);
            if (GUILayout.Button("X", GUILayout.Width(25f)))
            {
                RemoveTheme(themes[i]);
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();

        GUI.DragWindow();
    }

    private void AddThemeFromInput()
    {
        string cleanName = CleanThemeName(newThemeName);
        if (string.IsNullOrEmpty(cleanName))
        {
            status = "Enter a theme name first.";
            return;
        }

        RefreshScripts();
        if (ThemeExists(cleanName))
        {
            status = "Theme already exists: " + cleanName;
            return;
        }

        List<string> names = LoadThemeNames();
        names.Add(cleanName);
        SaveThemeNames(names);
        cachedThemeNames = names; // update cache
        ApplyCustomThemes();
        newThemeName = "";
        status = "Added theme: " + cleanName;
        log.LogInfo("Added custom theme: " + cleanName);
    }

    private void RemoveTheme(string name)
    {
        List<string> themes = LoadThemeNames();
        if (themes.Remove(name))
        {
            SaveThemeNames(themes);
            cachedThemeNames = themes; // update cache
            status = "Removed theme '" + name + "'. Restart required to apply changes completely (warning: savegame index shifts may occur).";
            log.LogInfo("Removed custom theme: " + name);
        }
        else
        {
            status = "Theme not found in custom list.";
        }
    }

    private static string CleanThemeName(string raw)
    {
        if (string.IsNullOrEmpty(raw))
        {
            return "";
        }

        string value = raw.Trim();
        value = value.Replace("\r", " ").Replace("\n", " ");
        while (value.Contains("  "))
        {
            value = value.Replace("  ", " ");
        }
        return value;
    }

    private void RefreshScripts()
    {
        if (main == null)
        {
            main = FindObjectOfType<mainScript>();
        }
        if (main != null)
        {
            if (text == null)
            {
                text = main.GetComponent<textScript>();
            }
            if (themesScript == null)
            {
                themesScript = main.GetComponent<themes>();
            }
            if (genresScript == null)
            {
                genresScript = main.GetComponent<genres>();
            }
        }
    }

    private bool ThemeExists(string name)
    {
        if (text == null || text.themes_EN == null)
        {
            return false;
        }

        return text.themes_EN.Any(existing => string.Equals(existing, name, System.StringComparison.OrdinalIgnoreCase));
    }

    public static void ApplyCustomThemes()
    {
        if (instance == null)
        {
            return;
        }

        instance.ApplyCustomThemesInstance();
    }

    private void ApplyCustomThemesInstance()
    {
        RefreshScripts();
        if (text == null || themesScript == null || genresScript == null || text.themes_EN == null || genresScript.genres_LEVEL == null)
        {
            return;
        }

        List<string> customThemes = LoadThemeNames();
        for (int i = 0; i < customThemes.Count; i++)
        {
            string name = customThemes[i];
            if (string.IsNullOrEmpty(name) || ThemeExists(name))
            {
                continue;
            }

            AppendThemeName(name);
            appliedThemes.Add(name);
            log.LogInfo("Applied custom theme: " + name);
        }

        EnsureThemeDataArrays();
    }

    private void AppendThemeName(string name)
    {
        text.themes_EN = Append(text.themes_EN, name);
        text.themes_GE = Append(text.themes_GE, name);
        text.themes_TU = Append(text.themes_TU, name);
        text.themes_CH = Append(text.themes_CH, name);
        text.themes_FR = Append(text.themes_FR, name);
        text.themes_ES = Append(text.themes_ES, name);
        text.themes_KO = Append(text.themes_KO, name);
        text.themes_PB = Append(text.themes_PB, name);
        text.themes_HU = Append(text.themes_HU, name);
        text.themes_RU = Append(text.themes_RU, name);
        text.themes_CT = Append(text.themes_CT, name);
        text.themes_PL = Append(text.themes_PL, name);
        text.themes_CZ = Append(text.themes_CZ, name);
        text.themes_AR = Append(text.themes_AR, name);
        text.themes_IT = Append(text.themes_IT, name);
        text.themes_RO = Append(text.themes_RO, name);
        text.themes_JA = Append(text.themes_JA, name);
        text.themes_UA = Append(text.themes_UA, name);
        text.themes_LA = Append(text.themes_LA, name);
        text.themes_TH = Append(text.themes_TH, name);
    }

    private void EnsureThemeDataArrays()
    {
        int themeCount = text.themes_EN.Length;
        int genreCount = genresScript.genres_LEVEL.Length;

        themesScript.themes_RES_POINTS_LEFT = Resize(themesScript.themes_RES_POINTS_LEFT, themeCount, 0f);
        themesScript.themes_LEVEL = Resize(themesScript.themes_LEVEL, themeCount, 0);
        themesScript.themes_MARKT = Resize(themesScript.themes_MARKT, themeCount, 0);
        themesScript.themes_USES = Resize(themesScript.themes_USES, themeCount, 0);
        themesScript.themes_MGSR = Resize(themesScript.themes_MGSR, themeCount, 0);

        bool[,] oldFit = themesScript.themes_FITGENRE;
        bool[,] fit = new bool[themeCount, genreCount];
        if (oldFit != null)
        {
            int oldThemes = System.Math.Min(oldFit.GetLength(0), themeCount);
            int oldGenres = System.Math.Min(oldFit.GetLength(1), genreCount);
            for (int t = 0; t < oldThemes; t++)
            {
                for (int g = 0; g < oldGenres; g++)
                {
                    fit[t, g] = oldFit[t, g];
                }
            }
        }

        HashSet<string> custom = new HashSet<string>(LoadThemeNames(), System.StringComparer.OrdinalIgnoreCase);
        for (int t = 0; t < themeCount; t++)
        {
            if (!custom.Contains(text.themes_EN[t]))
            {
                continue;
            }

            themesScript.themes_RES_POINTS_LEFT[t] = 0f;
            themesScript.themes_MGSR[t] = 0;
            for (int g = 0; g < genreCount; g++)
            {
                fit[t, g] = true;
            }
        }

        themesScript.themes_FITGENRE = fit;
    }

    private static string[] Append(string[] source, string value)
    {
        if (source == null)
        {
            return new[] { value };
        }

        string[] result = new string[source.Length + 1];
        System.Array.Copy(source, result, source.Length);
        result[result.Length - 1] = value;
        return result;
    }

    private static int[] Resize(int[] source, int length, int fill)
    {
        int[] result = new int[length];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = fill;
        }
        if (source != null)
        {
            System.Array.Copy(source, result, System.Math.Min(source.Length, result.Length));
        }
        return result;
    }

    private static float[] Resize(float[] source, int length, float fill)
    {
        float[] result = new float[length];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = fill;
        }
        if (source != null)
        {
            System.Array.Copy(source, result, System.Math.Min(source.Length, result.Length));
        }
        return result;
    }

    private static List<string> LoadThemeNames()
    {
        if (!File.Exists(ConfigPath))
        {
            return new List<string>();
        }

        return File.ReadAllLines(ConfigPath)
            .Select(CleanThemeName)
            .Where(line => !string.IsNullOrEmpty(line))
            .Distinct(System.StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    private static void SaveThemeNames(List<string> names)
    {
        List<string> clean = names
            .Select(CleanThemeName)
            .Where(line => !string.IsNullOrEmpty(line))
            .Distinct(System.StringComparer.OrdinalIgnoreCase)
            .OrderBy(line => line)
            .ToList();
        File.WriteAllLines(ConfigPath, clean.ToArray());
    }
}

[HarmonyPatch(typeof(textScript), "LoadContent_Themes")]
public static class CustomThemesLoadContentPatch
{
    public static void Postfix()
    {
        CustomThemesPlugin.ApplyCustomThemes();
    }
}

[HarmonyPatch(typeof(savegameScript), "LoadThemes")]
public static class CustomThemesSaveLoadPatch
{
    public static void Postfix()
    {
        CustomThemesPlugin.ApplyCustomThemes();
    }
}
