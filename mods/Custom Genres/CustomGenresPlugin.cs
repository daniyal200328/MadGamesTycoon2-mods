using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

[BepInPlugin("org.bepinex.plugins.customgenres", "Custom Genres", "0.1.0")]
public class CustomGenresPlugin : BaseUnityPlugin
{
    private static CustomGenresPlugin instance;
    private static ManualLogSource log;
    private static readonly HashSet<string> appliedGenres = new HashSet<string>(System.StringComparer.OrdinalIgnoreCase);

    private bool showWindow;
    private Rect windowRect;
    private Vector2 scroll;
    private string status = "";
    
    // UI Form States
    private string nameEN = "";
    private string descEN = "";
    private string priceStr = "30000";
    private string devCostsStr = "3000";
    private string resPointsStr = "50";
    private int dateYear = 1976;
    private int dateMonthIdx = 0; // 0 = JAN, 1 = FEB, etc.
    private string iconFile = "iconSkill.png";
    private bool sucYear = false;
    private bool unlockImmediately = true;
    
    private bool tGroupKid = true;
    private bool tGroupTeen = true;
    private bool tGroupAdult = true;
    private bool tGroupOld = true;
    private bool tGroupAll = true;

    private int focusGameplay = 25;
    private int focusGraphic = 25;
    private int focusSound = 25;
    private int focusControl = 25;

    private int[] designFocus = new int[8] { 5, 5, 5, 5, 5, 5, 5, 5 };
    private int[] designAlign = new int[3] { 5, 5, 5 };

    private int sellPC = 100;
    private int sellConsole = 100;
    private int sellHandheld = 100;
    private int sellPhone = 100;
    private int sellArcade = 100;

    private List<bool> selectedCombinations = new List<bool>();

    private mainScript main;
    private textScript text;
    private themes themesScript;
    private genres genresScript;

    private static readonly string[] MonthNames = new string[] {
        "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC"
    };

    // Actual In-Game Design Focus and Direction Names (English)
    private static readonly string[] FocusNames = new string[] {
        "Story / Quest",
        "Character Design",
        "Level Design",
        "Mission Design",
        "Game World",
        "Cutscenes",
        "Gameplay Depth",
        "Beginner Friendliness"
    };

    private static readonly string[] AlignNames = new string[] {
        "Core vs. Casual Gamers",
        "Gameplay vs. Graphics",
        "Innovation vs. Familiar"
    };

    private static string ConfigPath
    {
        get { return Path.Combine(Paths.ConfigPath, "CustomGenres.txt"); }
    }

    private void Awake()
    {
        instance = this;
        log = Logger;
        windowRect = new Rect(Screen.width / 2f - 400f, Screen.height / 2f - 325f, 800f, 650f);
        Directory.CreateDirectory(Paths.ConfigPath);

        if (!File.Exists(ConfigPath))
        {
            WriteDefaultTemplate();
        }

        new Harmony("org.bepinex.plugins.customgenres").PatchAll();
        log.LogInfo("Custom Genres loaded. Press Ctrl+Shift+G to create/reload genres.");
    }

    private void WriteDefaultTemplate()
    {
        string content = @"////                                            CUSTOM GENRES CODE DEFINITION                                     ////
// You can define your custom genres here using the exact same format as the game's original Genres.txt.
// Make sure to start each genre block with [ID] (the ID does not matter, they will be loaded in order).
// Keep the file encoding as Unicode.
//////////////////////////////////////////////////////////////////////////////////////////

[ID]0
[NAME EN]Survival
[DESC EN]The player must gather resources, build shelter, and survive against hazards.
[DATE]JAN 1976
[RES POINTS]50
[PRICE]30000
[DEV COSTS]3000
[SUC YEAR]false
[UNLOCK]true
[PIC]iconSkill.png
[TGROUP]<KID><TEEN><ADULT><OLD><ALL>
[GAMEPLAY]40
[GRAPHIC]20
[SOUND]20
[CONTROL]20
[FOCUS0]5
[FOCUS1]5
[FOCUS2]5
[FOCUS3]5
[FOCUS4]5
[FOCUS5]5
[FOCUS6]5
[FOCUS7]5
[ALIGN0]5
[ALIGN1]5
[ALIGN2]5
[P_PC]100
[P_CONSOLE]100
[P_HANDHELD]100
[P_PHONE]100
[P_ARCADE]100
[GENRE COMB]<0><1><2>
[EOF]
";
        File.WriteAllText(ConfigPath, content, System.Text.Encoding.Unicode);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && ShortcutHeld())
        {
            showWindow = !showWindow;
            if (showWindow)
            {
                RefreshScripts();
                ApplyCustomGenres();
                windowRect = new Rect(Screen.width / 2f - 400f, Screen.height / 2f - 325f, 800f, 650f);
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
        windowRect = GUI.Window(1202, windowRect, DrawWindow, "Custom Genres Creator");
    }

    private bool ShortcutHeld()
    {
        return (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) &&
               (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
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

    private void DrawWindow(int id)
    {
        GUILayout.Space(8);
        scroll = GUILayout.BeginScrollView(scroll, false, false);

        // Creator Form
        GUILayout.Label("<b>CREATE NEW GENRE</b>", GUILayout.ExpandWidth(true));
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Name (EN):", GUILayout.Width(120f));
        nameEN = GUILayout.TextField(nameEN ?? "", 40);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Description (EN):", GUILayout.Width(120f));
        descEN = GUILayout.TextField(descEN ?? "", 200);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Research Points:", GUILayout.Width(120f));
        resPointsStr = GUILayout.TextField(resPointsStr ?? "50", 10);
        GUILayout.Label("Price:", GUILayout.Width(60f));
        priceStr = GUILayout.TextField(priceStr ?? "30000", 10);
        GUILayout.Label("Dev Costs:", GUILayout.Width(70f));
        devCostsStr = GUILayout.TextField(devCostsStr ?? "3000", 10);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Avail. Year:", GUILayout.Width(80f));
        string yearStr = GUILayout.TextField(dateYear.ToString(), 5, GUILayout.Width(50f));
        if (int.TryParse(yearStr, out int yr)) dateYear = yr;

        GUILayout.Label("Month:", GUILayout.Width(60f));
        for (int m = 0; m < MonthNames.Length; m++)
        {
            if (GUILayout.Toggle(dateMonthIdx == m, MonthNames[m], GUILayout.Width(40f)))
            {
                dateMonthIdx = m;
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        unlockImmediately = GUILayout.Toggle(unlockImmediately, "Unlock & Research Immediately (Available now in current game)");
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        sucYear = GUILayout.Toggle(sucYear, "Use Year as Sequel Number (e.g. 'Football Manager 98' in 1998)");
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Icon Filename:", GUILayout.Width(120f));
        iconFile = GUILayout.TextField(iconFile ?? "iconSkill.png", 50);
        if (GUILayout.Button("Browse PNG", GUILayout.Width(90f)))
        {
            string picked = BrowseForPng();
            if (!string.IsNullOrEmpty(picked) && File.Exists(picked))
            {
                string fileName = Path.GetFileName(picked);
                string destDir = Path.Combine(Application.dataPath, "Extern/Icons_Genres");
                Directory.CreateDirectory(destDir);
                string destPath = Path.Combine(destDir, fileName);
                try
                {
                    File.Copy(picked, destPath, true);
                    iconFile = fileName;
                    status = "Imported icon logo: " + fileName;
                }
                catch (System.Exception ex)
                {
                    status = "Failed to copy icon: " + ex.Message;
                }
            }
        }
        GUILayout.EndHorizontal();

        // Target Groups
        GUILayout.Space(4);
        GUILayout.Label("<b>Target Groups:</b>");
        GUILayout.BeginHorizontal();
        tGroupKid = GUILayout.Toggle(tGroupKid, "Kids");
        tGroupTeen = GUILayout.Toggle(tGroupTeen, "Teens");
        tGroupAdult = GUILayout.Toggle(tGroupAdult, "Adults");
        tGroupOld = GUILayout.Toggle(tGroupOld, "Seniors");
        tGroupAll = GUILayout.Toggle(tGroupAll, "All");
        GUILayout.EndHorizontal();

        // Main Focus Priorities (Gameplay, Graphic, Sound, Control)
        GUILayout.Space(4);
        GUILayout.Label($"<b>Gameplay Priorities (Total: {focusGameplay + focusGraphic + focusSound + focusControl}%):</b>");
        
        GUILayout.BeginHorizontal();
        GUILayout.Label($"Gameplay ({focusGameplay}%):", GUILayout.Width(120f));
        focusGameplay = Mathf.RoundToInt(GUILayout.HorizontalSlider(focusGameplay, 0, 100));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label($"Graphic ({focusGraphic}%):", GUILayout.Width(120f));
        focusGraphic = Mathf.RoundToInt(GUILayout.HorizontalSlider(focusGraphic, 0, 100));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label($"Sound ({focusSound}%):", GUILayout.Width(120f));
        focusSound = Mathf.RoundToInt(GUILayout.HorizontalSlider(focusSound, 0, 100));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label($"Control ({focusControl}%):", GUILayout.Width(120f));
        focusControl = Mathf.RoundToInt(GUILayout.HorizontalSlider(focusControl, 0, 100));
        GUILayout.EndHorizontal();

        // Design Focus (Focus0 to Focus7)
        GUILayout.Space(4);
        GUILayout.Label("<b>Design Focus Settings (0 - 10):</b>");
        for (int i = 0; i < 8; i++)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(FocusNames[i] + ":", GUILayout.Width(180f));
            designFocus[i] = Mathf.RoundToInt(GUILayout.HorizontalSlider(designFocus[i], 0, 10));
            GUILayout.Label(designFocus[i].ToString(), GUILayout.Width(30f));
            GUILayout.EndHorizontal();
        }

        // Design Direction (Align0 to Align2)
        GUILayout.Space(4);
        GUILayout.Label("<b>Design Direction Settings (0 - 10):</b>");
        for (int i = 0; i < 3; i++)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(AlignNames[i] + ":", GUILayout.Width(180f));
            designAlign[i] = Mathf.RoundToInt(GUILayout.HorizontalSlider(designAlign[i], 0, 10));
            GUILayout.Label(designAlign[i].ToString(), GUILayout.Width(30f));
            GUILayout.EndHorizontal();
        }

        // Platform Sells Suitabilities
        GUILayout.Space(4);
        GUILayout.Label("<b>Platform Sells Suitabilities (0 - 100%):</b>");
        GUILayout.BeginHorizontal();
        GUILayout.Label($"PC: {sellPC}%", GUILayout.Width(70f));
        sellPC = Mathf.RoundToInt(GUILayout.HorizontalSlider(sellPC, 0, 100, GUILayout.Width(90f)));
        GUILayout.Label($"Console: {sellConsole}%", GUILayout.Width(90f));
        sellConsole = Mathf.RoundToInt(GUILayout.HorizontalSlider(sellConsole, 0, 100, GUILayout.Width(90f)));
        GUILayout.Label($"Handheld: {sellHandheld}%", GUILayout.Width(100f));
        sellHandheld = Mathf.RoundToInt(GUILayout.HorizontalSlider(sellHandheld, 0, 100, GUILayout.Width(90f)));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label($"Phone: {sellPhone}%", GUILayout.Width(80f));
        sellPhone = Mathf.RoundToInt(GUILayout.HorizontalSlider(sellPhone, 0, 100, GUILayout.Width(90f)));
        GUILayout.Label($"Arcade: {sellArcade}%", GUILayout.Width(90f));
        sellArcade = Mathf.RoundToInt(GUILayout.HorizontalSlider(sellArcade, 0, 100, GUILayout.Width(90f)));
        GUILayout.EndHorizontal();

        // Suitable Genre Combinations
        GUILayout.Space(4);
        GUILayout.Label("<b>Suitable Genre Combinations:</b>");
        if (genresScript != null && genresScript.genres_LEVEL != null)
        {
            int totalGenres = genresScript.genres_LEVEL.Length;
            if (selectedCombinations.Count != totalGenres)
            {
                selectedCombinations.Clear();
                for (int i = 0; i < totalGenres; i++)
                {
                    selectedCombinations.Add(false);
                }
            }

            int cols = 3;
            int rows = (int)System.Math.Ceiling((double)totalGenres / cols);
            for (int r = 0; r < rows; r++)
            {
                GUILayout.BeginHorizontal();
                for (int c = 0; c < cols; c++)
                {
                    int index = r * cols + c;
                    if (index < totalGenres)
                    {
                        selectedCombinations[index] = GUILayout.Toggle(selectedCombinations[index], genresScript.GetName(index), GUILayout.Width(180f));
                    }
                }
                GUILayout.EndHorizontal();
            }
        }

        GUILayout.Space(12);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Create & Apply Genre", GUILayout.Height(30f)))
        {
            CreateGenreFromForm();
        }
        if (GUILayout.Button("Reload From File", GUILayout.Width(130f), GUILayout.Height(30f)))
        {
            ApplyCustomGenres();
            status = "Reloaded custom genres from config.";
        }
        if (GUILayout.Button("Close", GUILayout.Width(100f), GUILayout.Height(30f)))
        {
            showWindow = false;
        }
        GUILayout.EndHorizontal();

        if (!string.IsNullOrEmpty(status))
        {
            GUILayout.Space(4);
            GUILayout.Label(status);
        }

        GUILayout.Space(12);
        GUILayout.Label("<b>Loaded Custom Genres:</b>");
        List<CustomGenre> custom = LoadCustomGenres();
        if (custom.Count == 0)
        {
            GUILayout.Label("- None");
        }
        else
        {
            foreach (var g in custom)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label($"- {g.nameEN} (Year: {g.dateYear}, Price: ${g.price})");
                if (GUILayout.Button("X", GUILayout.Width(25f)))
                {
                    RemoveGenre(g.nameEN);
                }
                GUILayout.EndHorizontal();
            }
        }

        GUILayout.EndScrollView();
        GUI.DragWindow();
    }

    private void CreateGenreFromForm()
    {
        string name = (nameEN ?? "").Trim();
        if (string.IsNullOrEmpty(name))
        {
            status = "Enter a genre name first.";
            return;
        }

        RefreshScripts();
        if (GenreExists(name))
        {
            status = "Genre already exists: " + name;
            return;
        }

        // Build config format string
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendLine();
        sb.AppendLine("[ID]" + (100 + Random.Range(0, 10000)));
        sb.AppendLine("[NAME EN]" + name);
        sb.AppendLine("[DESC EN]" + (descEN ?? ""));
        sb.AppendLine("[DATE]" + MonthNames[dateMonthIdx] + " " + dateYear);
        sb.AppendLine("[RES POINTS]" + (int.TryParse(resPointsStr, out int rp) ? rp : 50));
        sb.AppendLine("[PRICE]" + (int.TryParse(priceStr, out int pr) ? pr : 30000));
        sb.AppendLine("[DEV COSTS]" + (int.TryParse(devCostsStr, out int dc) ? dc : 3000));
        sb.AppendLine("[SUC YEAR]" + (sucYear ? "true" : "false"));
        sb.AppendLine("[UNLOCK]" + (unlockImmediately ? "true" : "false"));
        sb.AppendLine("[PIC]" + (iconFile ?? "iconSkill.png"));

        // Build target group string
        sb.Append("[TGROUP]");
        if (tGroupKid) sb.Append("<KID>");
        if (tGroupTeen) sb.Append("<TEEN>");
        if (tGroupAdult) sb.Append("<ADULT>");
        if (tGroupOld) sb.Append("<OLD>");
        if (tGroupAll) sb.Append("<ALL>");
        sb.AppendLine();

        sb.AppendLine("[GAMEPLAY]" + focusGameplay);
        sb.AppendLine("[GRAPHIC]" + focusGraphic);
        sb.AppendLine("[SOUND]" + focusSound);
        sb.AppendLine("[CONTROL]" + focusControl);

        for (int i = 0; i < 8; i++)
        {
            sb.AppendLine("[FOCUS" + i + "]" + designFocus[i]);
        }
        for (int i = 0; i < 3; i++)
        {
            sb.AppendLine("[ALIGN" + i + "]" + designAlign[i]);
        }

        sb.AppendLine("[P_PC]" + sellPC);
        sb.AppendLine("[P_CONSOLE]" + sellConsole);
        sb.AppendLine("[P_HANDHELD]" + sellHandheld);
        sb.AppendLine("[P_PHONE]" + sellPhone);
        sb.AppendLine("[P_ARCADE]" + sellArcade);

        // Build combinations list
        sb.Append("[GENRE COMB]");
        for (int i = 0; i < selectedCombinations.Count; i++)
        {
            if (selectedCombinations[i])
            {
                sb.Append("<" + i + ">");
            }
        }
        sb.AppendLine();
        sb.AppendLine("[EOF]");

        // Append to file
        File.AppendAllText(ConfigPath, sb.ToString(), System.Text.Encoding.Unicode);
        log.LogInfo("Appended custom genre to file: " + name);

        // Apply
        ApplyCustomGenres();
        
        nameEN = "";
        descEN = "";
        status = "Created & Applied custom genre: " + name;
    }

    public static void ApplyCustomGenres()
    {
        if (instance == null)
        {
            return;
        }
        instance.ApplyCustomGenresInstance();
    }

    private void ApplyCustomGenresInstance()
    {
        RefreshScripts();
        if (genresScript == null || text == null || genresScript.genres_LEVEL == null)
        {
            return;
        }

        List<CustomGenre> custom = LoadCustomGenres();
        for (int i = 0; i < custom.Count; i++)
        {
            CustomGenre g = custom[i];
            string name = g.nameEN;
            if (string.IsNullOrEmpty(name) || GenreExists(name))
            {
                continue;
            }

            AppendGenre(g);
            appliedGenres.Add(name);
            log.LogInfo("Applied custom genre to memory: " + name);
        }

        EnsureGenreDataArrays(custom);
    }

    private bool GenreExists(string name)
    {
        if (genresScript == null || genresScript.genres_NAME_EN == null)
        {
            return false;
        }
        return genresScript.genres_NAME_EN.Any(existing => string.Equals(existing, name, System.StringComparison.OrdinalIgnoreCase));
    }

    private void AppendGenre(CustomGenre g)
    {
        genresScript.genres_NAME_EN = Append(genresScript.genres_NAME_EN, g.nameEN);
        genresScript.genres_NAME_GE = Append(genresScript.genres_NAME_GE, g.nameGE);
        genresScript.genres_NAME_TU = Append(genresScript.genres_NAME_TU, g.nameTU);
        genresScript.genres_NAME_CH = Append(genresScript.genres_NAME_CH, g.nameCH);
        genresScript.genres_NAME_FR = Append(genresScript.genres_NAME_FR, g.nameFR);
        genresScript.genres_NAME_PB = Append(genresScript.genres_NAME_PB, g.namePB);
        genresScript.genres_NAME_HU = Append(genresScript.genres_NAME_HU, g.nameHU);
        genresScript.genres_NAME_CT = Append(genresScript.genres_NAME_CT, g.nameCT);
        genresScript.genres_NAME_ES = Append(genresScript.genres_NAME_ES, g.nameES);
        genresScript.genres_NAME_PL = Append(genresScript.genres_NAME_PL, g.namePL);
        genresScript.genres_NAME_CZ = Append(genresScript.genres_NAME_CZ, g.nameCZ);
        genresScript.genres_NAME_KO = Append(genresScript.genres_NAME_KO, g.nameKO);
        genresScript.genres_NAME_IT = Append(genresScript.genres_NAME_IT, g.nameIT);
        genresScript.genres_NAME_AR = Append(genresScript.genres_NAME_AR, g.nameAR);
        genresScript.genres_NAME_JA = Append(genresScript.genres_NAME_JA, g.nameJA);
        genresScript.genres_NAME_UA = Append(genresScript.genres_NAME_UA, g.nameUA);
        genresScript.genres_NAME_TH = Append(genresScript.genres_NAME_TH, g.nameTH);
        genresScript.genres_NAME_RU = Append(genresScript.genres_NAME_RU, g.nameRU);

        genresScript.genres_DESC_EN = Append(genresScript.genres_DESC_EN, g.descEN);
        genresScript.genres_DESC_GE = Append(genresScript.genres_DESC_GE, g.descGE);
        genresScript.genres_DESC_TU = Append(genresScript.genres_DESC_TU, g.descTU);
        genresScript.genres_DESC_CH = Append(genresScript.genres_DESC_CH, g.descCH);
        genresScript.genres_DESC_FR = Append(genresScript.genres_DESC_FR, g.descFR);
        genresScript.genres_DESC_PB = Append(genresScript.genres_DESC_PB, g.descPB);
        genresScript.genres_DESC_HU = Append(genresScript.genres_DESC_HU, g.descHU);
        genresScript.genres_DESC_CT = Append(genresScript.genres_DESC_CT, g.descCT);
        genresScript.genres_DESC_ES = Append(genresScript.genres_DESC_ES, g.descES);
        genresScript.genres_DESC_PL = Append(genresScript.genres_DESC_PL, g.descPL);
        genresScript.genres_DESC_CZ = Append(genresScript.genres_DESC_CZ, g.descCZ);
        genresScript.genres_DESC_KO = Append(genresScript.genres_DESC_KO, g.descKO);
        genresScript.genres_DESC_IT = Append(genresScript.genres_DESC_IT, g.descIT);
        genresScript.genres_DESC_AR = Append(genresScript.genres_DESC_AR, g.descAR);
        genresScript.genres_DESC_JA = Append(genresScript.genres_DESC_JA, g.descJA);
        genresScript.genres_DESC_UA = Append(genresScript.genres_DESC_UA, g.descUA);
        genresScript.genres_DESC_TH = Append(genresScript.genres_DESC_TH, g.descTH);
        genresScript.genres_DESC_RU = Append(genresScript.genres_DESC_RU, g.descRU);
    }

    private void EnsureGenreDataArrays(List<CustomGenre> customGenres)
    {
        int genreCount = genresScript.genres_NAME_EN.Length;
        int oldLength = (genresScript.genres_UNLOCK != null) ? genresScript.genres_UNLOCK.Length : 0;

        // 1D Arrays Resizing
        genresScript.genres_BELIEBTHEIT = Resize(genresScript.genres_BELIEBTHEIT, genreCount, 50f);
        genresScript.genres_BELIEBTHEIT_SOLL = Resize(genresScript.genres_BELIEBTHEIT_SOLL, genreCount, false);
        genresScript.genres_RES_POINTS = Resize(genresScript.genres_RES_POINTS, genreCount, 50);
        genresScript.genres_RES_POINTS_LEFT = Resize(genresScript.genres_RES_POINTS_LEFT, genreCount, 50f);
        genresScript.genres_PRICE = Resize(genresScript.genres_PRICE, genreCount, 30000);
        genresScript.genres_DEV_COSTS = Resize(genresScript.genres_DEV_COSTS, genreCount, 3000);
        genresScript.genres_DATE_YEAR = Resize(genresScript.genres_DATE_YEAR, genreCount, 1976);
        genresScript.genres_DATE_MONTH = Resize(genresScript.genres_DATE_MONTH, genreCount, 1);
        genresScript.genres_LEVEL = Resize(genresScript.genres_LEVEL, genreCount, 0);
        genresScript.genres_UNLOCK = Resize(genresScript.genres_UNLOCK, genreCount, false);
        genresScript.genres_SUC_YEAR = Resize(genresScript.genres_SUC_YEAR, genreCount, false);
        genresScript.genres_GAMEPLAY = Resize(genresScript.genres_GAMEPLAY, genreCount, 25f);
        genresScript.genres_GRAPHIC = Resize(genresScript.genres_GRAPHIC, genreCount, 25f);
        genresScript.genres_SOUND = Resize(genresScript.genres_SOUND, genreCount, 25f);
        genresScript.genres_CONTROL = Resize(genresScript.genres_CONTROL, genreCount, 25f);
        genresScript.genres_ICONFILE = Resize(genresScript.genres_ICONFILE, genreCount, "");
        genresScript.genres_FANS = Resize(genresScript.genres_FANS, genreCount, 0);
        genresScript.genres_MARKT = Resize(genresScript.genres_MARKT, genreCount, 0);

        // 2D Arrays Resizing
        genresScript.genres_TARGETGROUP = Resize2D(genresScript.genres_TARGETGROUP, genreCount, 5, true);
        genresScript.genres_PLATFORM_SELLS = Resize2D(genresScript.genres_PLATFORM_SELLS, genreCount, 5, 100);
        genresScript.genres_FOCUS = Resize2D(genresScript.genres_FOCUS, genreCount, 8, 5);
        genresScript.genres_ALIGN = Resize2D(genresScript.genres_ALIGN, genreCount, 3, 5);

        // Square Matrix Resizing
        genresScript.genres_COMBINATION = ResizeSquare(genresScript.genres_COMBINATION, genreCount, false);

        // Populate Custom Genres Attributes
        int originalCount = genreCount - customGenres.Count;
        for (int i = 0; i < customGenres.Count; i++)
        {
            int idx = originalCount + i;
            CustomGenre g = customGenres[i];

            genresScript.genres_RES_POINTS[idx] = g.resPoints;
            genresScript.genres_PRICE[idx] = g.price;
            genresScript.genres_DEV_COSTS[idx] = g.devCosts;
            genresScript.genres_DATE_YEAR[idx] = g.dateYear;
            genresScript.genres_DATE_MONTH[idx] = g.dateMonth;
            genresScript.genres_SUC_YEAR[idx] = g.sucYear;
            genresScript.genres_GAMEPLAY[idx] = g.gameplay;
            genresScript.genres_GRAPHIC[idx] = g.graphic;
            genresScript.genres_SOUND[idx] = g.sound;
            genresScript.genres_CONTROL[idx] = g.control;
            genresScript.genres_ICONFILE[idx] = g.iconFile;

            bool isLoading = false;
            if (main != null)
            {
                var savegame = main.GetComponent<savegameScript>();
                if (savegame != null && savegame.loadingSavegame)
                {
                    isLoading = true;
                }
            }

            if (idx >= oldLength || !isLoading)
            {
                bool pastDate = false;
                if (main != null)
                {
                    if (main.year > g.dateYear || (main.year == g.dateYear && main.month >= g.dateMonth))
                    {
                        pastDate = true;
                    }
                }

                bool wantUnlock = g.unlockImmediately || pastDate || (g.dateYear == 1976 && g.dateMonth == 1);

                if (wantUnlock)
                {
                    genresScript.genres_UNLOCK[idx] = true;
                    if (g.unlockImmediately || (g.dateYear == 1976 && g.dateMonth == 1))
                    {
                        genresScript.genres_RES_POINTS_LEFT[idx] = 0f;
                    }
                    else
                    {
                        if (idx >= oldLength || genresScript.genres_RES_POINTS_LEFT[idx] >= g.resPoints)
                        {
                            genresScript.genres_RES_POINTS_LEFT[idx] = g.resPoints;
                        }
                    }
                }
                else
                {
                    if (idx >= oldLength || !genresScript.genres_UNLOCK[idx])
                    {
                        genresScript.genres_UNLOCK[idx] = false;
                        genresScript.genres_RES_POINTS_LEFT[idx] = g.resPoints;
                    }
                }
            }

            for (int tg = 0; tg < 5; tg++)
            {
                genresScript.genres_TARGETGROUP[idx, tg] = g.targetGroup[tg];
            }

            for (int p = 0; p < 5; p++)
            {
                genresScript.genres_PLATFORM_SELLS[idx, p] = g.platformSells[p];
            }

            for (int f = 0; f < 8; f++)
            {
                genresScript.genres_FOCUS[idx, f] = g.focus[f];
            }

            for (int al = 0; al < 3; al++)
            {
                genresScript.genres_ALIGN[idx, al] = g.align[al];
            }

            foreach (int cmbId in g.combinations)
            {
                if (cmbId < genreCount)
                {
                    genresScript.genres_COMBINATION[idx, cmbId] = true;
                    genresScript.genres_COMBINATION[cmbId, idx] = true;
                }
            }
        }

        // Resize themesScript.themes_FITGENRE
        if (themesScript != null && themesScript.themes_FITGENRE != null)
        {
            int themeCount = themesScript.themes_LEVEL.Length;
            bool[,] oldFit = themesScript.themes_FITGENRE;
            bool[,] fit = new bool[themeCount, genreCount];
            int oldThemes = System.Math.Min(oldFit.GetLength(0), themeCount);
            int oldGenres = System.Math.Min(oldFit.GetLength(1), genreCount);
            for (int t = 0; t < oldThemes; t++)
            {
                for (int g = 0; g < oldGenres; g++)
                {
                    fit[t, g] = oldFit[t, g];
                }
            }

            for (int t = 0; t < themeCount; t++)
            {
                for (int g = oldGenres; g < genreCount; g++)
                {
                    fit[t, g] = true; // default fit for all themes
                }
            }
            themesScript.themes_FITGENRE = fit;
        }

        // Reflected private and screenshots fields
        int[] screenshotsAmount = GetReflectedField<int[]>(genresScript, "genres_SCREENSHOTS_AMOUNT");
        screenshotsAmount = Resize(screenshotsAmount, genreCount, 0);
        SetReflectedField(genresScript, "genres_SCREENSHOTS_AMOUNT", screenshotsAmount);

        Sprite[,] screenshots = GetReflectedField<Sprite[,]>(genresScript, "genres_SCREENSHOTS");
        screenshots = Resize2D(screenshots, genreCount, 99, (Sprite)null);
        SetReflectedField(genresScript, "genres_SCREENSHOTS", screenshots);

        Texture2D[,] screenshotsTexture = GetReflectedField<Texture2D[,]>(genresScript, "genres_SCREENSHOTS_TEXTURE");
        screenshotsTexture = Resize2D(screenshotsTexture, genreCount, 99, (Texture2D)null);
        SetReflectedField(genresScript, "genres_SCREENSHOTS_TEXTURE", screenshotsTexture);

        Sprite[] pics = GetReflectedField<Sprite[]>(genresScript, "genres_PIC");
        pics = Resize(pics, genreCount, (Sprite)null);
        SetReflectedField(genresScript, "genres_PIC", pics);
    }

    private static T[] Resize<T>(T[] source, int length, T fill)
    {
        T[] result = new T[length];
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

    private static T[,] Resize2D<T>(T[,] source, int dim1, int dim2, T fill)
    {
        T[,] result = new T[dim1, dim2];
        for (int i = 0; i < dim1; i++)
        {
            for (int j = 0; j < dim2; j++)
            {
                result[i, j] = fill;
            }
        }
        if (source != null)
        {
            int oldDim1 = source.GetLength(0);
            int oldDim2 = source.GetLength(1);
            int copyDim1 = System.Math.Min(oldDim1, dim1);
            int copyDim2 = System.Math.Min(oldDim2, dim2);
            for (int i = 0; i < copyDim1; i++)
            {
                for (int j = 0; j < copyDim2; j++)
                {
                    result[i, j] = source[i, j];
                }
            }
        }
        return result;
    }

    private static T[,] ResizeSquare<T>(T[,] source, int size, T fill)
    {
        T[,] result = new T[size, size];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                result[i, j] = fill;
            }
        }
        if (source != null)
        {
            int oldSize = source.GetLength(0);
            int copySize = System.Math.Min(oldSize, size);
            for (int i = 0; i < copySize; i++)
            {
                for (int j = 0; j < copySize; j++)
                {
                    result[i, j] = source[i, j];
                }
            }
        }
        return result;
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

    private static void SetReflectedField<T>(object target, string fieldName, T value)
    {
        var field = target.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (field != null)
        {
            field.SetValue(target, value);
        }
    }

    private static T GetReflectedField<T>(object target, string fieldName)
    {
        var field = target.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (field != null)
        {
            return (T)field.GetValue(target);
        }
        return default(T);
    }

    private List<CustomGenre> LoadCustomGenres()
    {
        List<CustomGenre> list = new List<CustomGenre>();
        if (!File.Exists(ConfigPath))
        {
            return list;
        }

        string[] lines = File.ReadAllLines(ConfigPath, System.Text.Encoding.Unicode);
        List<string> cleanLines = new List<string>();
        foreach (string line in lines)
        {
            if (!line.Trim().StartsWith("//"))
            {
                cleanLines.Add(line);
            }
        }

        CustomGenre current = null;
        for (int i = 0; i < cleanLines.Count; i++)
        {
            string line = cleanLines[i];
            if (line.Contains("[ID]"))
            {
                if (current != null)
                {
                    list.Add(current);
                }
                current = new CustomGenre();
                continue;
            }
            if (current == null) continue;

            if (line.Contains("[NAME EN]")) { current.nameEN = GetVal(line, "[NAME EN]"); current.FillLanguages(); }
            else if (line.Contains("[DESC EN]")) { current.descEN = GetVal(line, "[DESC EN]"); current.FillDescLanguages(); }
            else if (line.Contains("[RES POINTS]")) current.resPoints = int.Parse(GetVal(line, "[RES POINTS]"));
            else if (line.Contains("[PRICE]")) current.price = int.Parse(GetVal(line, "[PRICE]"));
            else if (line.Contains("[DEV COSTS]")) current.devCosts = int.Parse(GetVal(line, "[DEV COSTS]"));
            else if (line.Contains("[SUC YEAR]")) current.sucYear = GetVal(line, "[SUC YEAR]").Contains("true");
            else if (line.Contains("[UNLOCK]")) current.unlockImmediately = GetVal(line, "[UNLOCK]").Contains("true");
            else if (line.Contains("[PIC]")) current.iconFile = GetVal(line, "[PIC]");

            else if (line.Contains("[GAMEPLAY]")) current.gameplay = float.Parse(GetVal(line, "[GAMEPLAY]"));
            else if (line.Contains("[GRAPHIC]")) current.graphic = float.Parse(GetVal(line, "[GRAPHIC]"));
            else if (line.Contains("[SOUND]")) current.sound = float.Parse(GetVal(line, "[SOUND]"));
            else if (line.Contains("[CONTROL]")) current.control = float.Parse(GetVal(line, "[CONTROL]"));

            else if (line.Contains("[TGROUP]"))
            {
                current.targetGroup[0] = line.Contains("<KID>");
                current.targetGroup[1] = line.Contains("<TEEN>");
                current.targetGroup[2] = line.Contains("<ADULT>");
                current.targetGroup[3] = line.Contains("<OLD>");
                current.targetGroup[4] = line.Contains("<ALL>");
            }
            else if (line.Contains("[DATE]"))
            {
                string val = GetVal(line, "[DATE]");
                current.dateMonth = 1;
                for (int m = 0; m < MonthNames.Length; m++)
                {
                    if (val.Contains(MonthNames[m]))
                    {
                        current.dateMonth = m + 1;
                        break;
                    }
                }
                string[] parts = val.Split(' ');
                foreach (string p in parts)
                {
                    if (int.TryParse(p.Trim(), out int y))
                    {
                        current.dateYear = y;
                        break;
                    }
                }
            }
            else if (line.Contains("[P_PC]")) current.platformSells[0] = int.Parse(GetVal(line, "[P_PC]"));
            else if (line.Contains("[P_CONSOLE]")) current.platformSells[1] = int.Parse(GetVal(line, "[P_CONSOLE]"));
            else if (line.Contains("[P_HANDHELD]")) current.platformSells[2] = int.Parse(GetVal(line, "[P_HANDHELD]"));
            else if (line.Contains("[P_PHONE]")) current.platformSells[3] = int.Parse(GetVal(line, "[P_PHONE]"));
            else if (line.Contains("[P_ARCADE]")) current.platformSells[4] = int.Parse(GetVal(line, "[P_ARCADE]"));
            else if (line.Contains("[GENRE COMB]"))
            {
                string val = GetVal(line, "[GENRE COMB]");
                int start = 0;
                while ((start = val.IndexOf('<', start)) != -1)
                {
                    int end = val.IndexOf('>', start);
                    if (end == -1) break;
                    string idStr = val.Substring(start + 1, end - start - 1);
                    if (int.TryParse(idStr, out int cmbId))
                    {
                        current.combinations.Add(cmbId);
                    }
                    start = end + 1;
                }
            }
            else
            {
                for (int k = 0; k < 8; k++)
                {
                    if (line.Contains("[FOCUS" + k + "]"))
                    {
                        current.focus[k] = int.Parse(GetVal(line, "[FOCUS" + k + "]"));
                        break;
                    }
                }
                for (int l = 0; l < 3; l++)
                {
                    if (line.Contains("[ALIGN" + l + "]"))
                    {
                        current.align[l] = int.Parse(GetVal(line, "[ALIGN" + l + "]"));
                        break;
                    }
                }
            }
        }

        if (current != null)
        {
            list.Add(current);
        }

        return list;
    }

    private string GetVal(string line, string token)
    {
        string val = line.Replace(token, "").Trim();
        val = val.Replace("\r", "").Replace("\n", "");
        return val;
    }

    private void RemoveGenre(string name)
    {
        List<CustomGenre> genres = LoadCustomGenres();
        CustomGenre toRemove = genres.FirstOrDefault(g => string.Equals(g.nameEN, name, System.StringComparison.OrdinalIgnoreCase));
        if (toRemove != null)
        {
            genres.Remove(toRemove);
            SaveCustomGenres(genres);
            status = "Removed genre '" + name + "'. Restart required to apply changes completely (warning: savegame index shifts may occur).";
            log.LogInfo("Removed custom genre: " + name);
        }
        else
        {
            status = "Genre not found in custom list.";
        }
    }

    private void SaveCustomGenres(List<CustomGenre> list)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendLine("////                                            CUSTOM GENRES CODE DEFINITION                                     ////");
        sb.AppendLine("// You can define your custom genres here using the exact same format as the game's original Genres.txt.");
        sb.AppendLine("// Make sure to start each genre block with [ID] (the ID does not matter, they will be loaded in order).");
        sb.AppendLine("// Keep the file encoding as Unicode.");
        sb.AppendLine("//////////////////////////////////////////////////////////////////////////////////////////");
        sb.AppendLine();

        foreach (var g in list)
        {
            sb.AppendLine("[ID]" + (100 + UnityEngine.Random.Range(0, 10000)));
            sb.AppendLine("[NAME EN]" + g.nameEN);
            sb.AppendLine("[DESC EN]" + g.descEN);
            sb.AppendLine("[DATE]" + MonthNames[g.dateMonth - 1] + " " + g.dateYear);
            sb.AppendLine("[RES POINTS]" + g.resPoints);
            sb.AppendLine("[PRICE]" + g.price);
            sb.AppendLine("[DEV COSTS]" + g.devCosts);
            sb.AppendLine("[SUC YEAR]" + (g.sucYear ? "true" : "false"));
            sb.AppendLine("[UNLOCK]" + (g.unlockImmediately ? "true" : "false"));
            sb.AppendLine("[PIC]" + g.iconFile);

            sb.Append("[TGROUP]");
            if (g.targetGroup[0]) sb.Append("<KID>");
            if (g.targetGroup[1]) sb.Append("<TEEN>");
            if (g.targetGroup[2]) sb.Append("<ADULT>");
            if (g.targetGroup[3]) sb.Append("<OLD>");
            if (g.targetGroup[4]) sb.Append("<ALL>");
            sb.AppendLine();

            sb.AppendLine("[GAMEPLAY]" + g.gameplay);
            sb.AppendLine("[GRAPHIC]" + g.graphic);
            sb.AppendLine("[SOUND]" + g.sound);
            sb.AppendLine("[CONTROL]" + g.control);

            for (int i = 0; i < 8; i++)
            {
                sb.AppendLine("[FOCUS" + i + "]" + g.focus[i]);
            }
            for (int i = 0; i < 3; i++)
            {
                sb.AppendLine("[ALIGN" + i + "]" + g.align[i]);
            }

            sb.AppendLine("[P_PC]" + g.platformSells[0]);
            sb.AppendLine("[P_CONSOLE]" + g.platformSells[1]);
            sb.AppendLine("[P_HANDHELD]" + g.platformSells[2]);
            sb.AppendLine("[P_PHONE]" + g.platformSells[3]);
            sb.AppendLine("[P_ARCADE]" + g.platformSells[4]);

            sb.Append("[GENRE COMB]");
            foreach (int cmbId in g.combinations)
            {
                sb.Append("<" + cmbId + ">");
            }
            sb.AppendLine();
            sb.AppendLine("[EOF]");
            sb.AppendLine();
        }

        File.WriteAllText(ConfigPath, sb.ToString(), System.Text.Encoding.Unicode);
    }

    // Windows Native File Browser P/Invoke
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private class OpenFileName
    {
        public int structSize;
        public System.IntPtr dlgOwner = System.IntPtr.Zero;
        public System.IntPtr instance = System.IntPtr.Zero;
        public string filter;
        public string customFilter = null;
        public int maxCustFilter = 0;
        public int filterIndex = 1;
        public string file;
        public int maxFile;
        public string fileTitle = new string(new char[260]);
        public int maxFileTitle = 260;
        public string initialDir = null;
        public string title;
        public int flags;
        public short fileOffset = 0;
        public short fileExtension = 0;
        public string defExt;
        public System.IntPtr custData = System.IntPtr.Zero;
        public System.IntPtr hook = System.IntPtr.Zero;
        public string templateName = null;
        public System.IntPtr reservedPtr = System.IntPtr.Zero;
        public int reservedInt = 0;
        public int flagsEx = 0;
    }

    [DllImport("comdlg32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern bool GetOpenFileName([In, Out] OpenFileName ofn);

    private string BrowseForPng()
    {
        OpenFileName ofn = new OpenFileName();
        ofn.structSize = Marshal.SizeOf(typeof(OpenFileName));
        ofn.filter = "PNG files\0*.png\0All files\0*.*\0";
        ofn.file = new string(new char[1024]);
        ofn.maxFile = ofn.file.Length;
        ofn.title = "Select Genre Icon PNG";
        ofn.defExt = "png";
        
        if (GetOpenFileName(ofn))
        {
            return ofn.file;
        }
        return "";
    }
}

public class CustomGenre
{
    public string nameEN = "";
    public string nameGE = "";
    public string nameTU = "";
    public string nameCH = "";
    public string nameFR = "";
    public string namePB = "";
    public string nameHU = "";
    public string nameCT = "";
    public string nameES = "";
    public string namePL = "";
    public string nameCZ = "";
    public string nameKO = "";
    public string nameIT = "";
    public string nameAR = "";
    public string nameJA = "";
    public string nameUA = "";
    public string nameTH = "";
    public string nameRU = "";

    public string descEN = "";
    public string descGE = "";
    public string descTU = "";
    public string descCH = "";
    public string descFR = "";
    public string descPB = "";
    public string descHU = "";
    public string descCT = "";
    public string descES = "";
    public string descPL = "";
    public string descCZ = "";
    public string descKO = "";
    public string descIT = "";
    public string descAR = "";
    public string descJA = "";
    public string descUA = "";
    public string descTH = "";
    public string descRU = "";

    public int dateMonth = 1;
    public int dateYear = 1976;
    public int resPoints = 50;
    public int price = 30000;
    public int devCosts = 3000;
    public string iconFile = "iconSkill.png";
    public bool[] targetGroup = new bool[5] { true, true, true, true, true };
    public float gameplay = 25f;
    public float graphic = 25f;
    public float sound = 25f;
    public float control = 25f;
    public int[] focus = new int[8] { 5, 5, 5, 5, 5, 5, 5, 5 };
    public int[] align = new int[3] { 5, 5, 5 };
    public List<int> combinations = new List<int>();
    public int[] platformSells = new int[5] { 100, 100, 100, 100, 100 };
    public bool sucYear = false;
    public bool unlockImmediately = false;

    public void FillLanguages()
    {
        nameGE = nameEN; nameTU = nameEN; nameCH = nameEN; nameFR = nameEN;
        namePB = nameEN; nameHU = nameEN; nameCT = nameEN; nameES = nameEN;
        namePL = nameEN; nameCZ = nameEN; nameKO = nameEN; nameIT = nameEN;
        nameAR = nameEN; nameJA = nameEN; nameUA = nameEN; nameTH = nameEN;
        nameRU = nameEN;
    }

    public void FillDescLanguages()
    {
        descGE = descEN; descTU = descEN; descCH = descEN; descFR = descEN;
        descPB = descEN; descHU = descEN; descCT = descEN; descES = descEN;
        descPL = descEN; descCZ = descEN; descKO = descEN; descIT = descEN;
        descAR = descEN; descJA = descEN; descUA = descEN; descTH = descEN;
        descRU = descEN;
    }
}

[HarmonyPatch(typeof(genres), "LoadGenres")]
public static class CustomGenresLoadContentPatch
{
    public static void Postfix()
    {
        CustomGenresPlugin.ApplyCustomGenres();
    }
}

[HarmonyPatch(typeof(savegameScript), "LoadGenres")]
public static class CustomGenresSaveLoadPatch
{
    public static void Postfix()
    {
        CustomGenresPlugin.ApplyCustomGenres();
    }
}
