using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

[BepInPlugin("org.bepinex.plugins.subsidiary2", "Organic Subsidiaries", "2.0.0")]
public partial class Subsidiary2Plugin : BaseUnityPlugin
{
    internal static Subsidiary2Plugin instance;
    internal static ManualLogSource log;
    private static Harmony harmonyInstance;
    private static bool isPatched;

    private bool showCreationMenu;
    private Rect windowRect;
    private string studioName = "My New Studio";
    private int selectedMarket = 1;
    private int selectedSpeed = 1;
    private int selectedCountry;
    private int selectedLogoID;
    private string logoImportPath = "";
    private string logoStatus = "";
    private int lastCostMarket = -1;
    private int lastCostSpeed = -1;
    private int lastCostCountry = -1;
    private int lastCostLogo = -1;
    private long cachedCost;

    private mainScript main;
    private publisher publisherSpawner;
    private textScript text;
    private GUI_Main guiMain;
    private Texture2D windowBackground;
    private GUIStyle windowStyle;
    private float lastCacheTime;

    private static mainScript cachedMainScript;
    private static GUI_Main cachedGuiMain;
    private static textScript cachedTextScript;
    private static games cachedGames;

    private static readonly long[] speedCosts =
    {
        0L, 3000000L, 7000000L, 12000000L, 20000000L, 32000000L,
        48000000L, 70000000L, 100000000L, 140000000L, 190000000L
    };

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

    public static mainScript GetMainScript()
    {
        if (cachedMainScript == null)
            cachedMainScript = Object.FindObjectOfType<mainScript>();
        return cachedMainScript;
    }

    public static GUI_Main GetGuiMain()
    {
        if (cachedGuiMain == null)
            cachedGuiMain = Object.FindObjectOfType<GUI_Main>();
        return cachedGuiMain;
    }

    public static textScript GetTextScript()
    {
        if (cachedTextScript == null)
            cachedTextScript = Object.FindObjectOfType<textScript>();
        return cachedTextScript;
    }

    public static games GetGamesScript()
    {
        if (cachedGames == null)
            cachedGames = Object.FindObjectOfType<games>();
        return cachedGames;
    }

    private void Awake()
    {
        instance = this;
        log = Logger;
        windowRect = new Rect(Screen.width / 2f - 260f, Screen.height / 2f - 240f, 520f, 480f);
        harmonyInstance = new Harmony("org.bepinex.plugins.subsidiary2");
        LoadState();
        log.LogInfo("Organic Subsidiaries loaded. Waiting for gameplay load to apply patches...");
    }

    private void Update()
    {
        if (!isPatched)
        {
            mainScript mS = FindObjectOfType<mainScript>();
            if (mS != null && mS.officeLoaded)
            {
                try
                {
                    harmonyInstance.PatchAll();
                    isPatched = true;
                    log.LogInfo("Gameplay loaded. Harmony patches applied successfully!");
                }
                catch (System.Exception ex)
                {
                    log.LogError("Failed to apply patches on gameplay load: " + ex);
                    isPatched = true;
                }
            }
        }

        if (isPatched && Input.GetKeyDown(KeyCode.S) && IsShortcutHeld())
            ToggleCreationWindow();
    }

    private void OnGUI()
    {
        if (!showCreationMenu) return;
        GUI.depth = -1000;
        EnsureGuiStyles();
        windowRect = GUI.Window(999, windowRect, CreationWindow, "Create New Subsidiary", windowStyle);
    }

    private bool IsShortcutHeld()
    {
        return (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) &&
               (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
    }

    private void ToggleCreationWindow()
    {
        if (!showCreationMenu)
        {
            RefreshCachedGameObjects();
            CenterCreationWindow();
            showCreationMenu = true;
            GUI_Main gui = FindObjectOfType<GUI_Main>();
            if (gui != null) gui.CloseMenu();
            log.LogInfo("Creation window opened via Ctrl+Shift+S.");
        }
        else
        {
            showCreationMenu = false;
            log.LogInfo("Creation window closed.");
        }
    }

    private void CenterCreationWindow()
    {
        windowRect = new Rect(Screen.width / 2f - 260f, Screen.height / 2f - 240f, 520f, 480f);
    }

    private void EnsureGuiStyles()
    {
        if (windowStyle != null) return;
        windowBackground = new Texture2D(1, 1);
        windowBackground.SetPixel(0, 0, new Color(0.12f, 0.12f, 0.12f, 0.98f));
        windowBackground.Apply();
        windowStyle = new GUIStyle(GUI.skin.window);
        windowStyle.normal.background = windowBackground;
        windowStyle.padding = new RectOffset(14, 14, 24, 14);
    }

    private void RefreshCachedGameObjects()
    {
        if (Time.time - lastCacheTime < 2.0f && main != null && publisherSpawner != null && text != null && guiMain != null)
            return;
        lastCacheTime = Time.time;
        if (main == null) main = FindObjectOfType<mainScript>();
        if (publisherSpawner == null) publisherSpawner = FindObjectOfType<publisher>();
        if (text == null) text = FindObjectOfType<textScript>();
        if (guiMain == null) guiMain = FindObjectOfType<GUI_Main>();
    }

    private void CreationWindow(int windowID)
    {
        GUILayout.Space(8);
        GUILayout.Label("Studio Name:");
        studioName = GUILayout.TextField(studioName, 30);

        GUILayout.Space(8);
        GUILayout.Label("Market / Experience (1-5 stars): " + selectedMarket);
        int newMarket = Mathf.Clamp(Mathf.RoundToInt(GUILayout.HorizontalSlider(selectedMarket, 1, 5)), 1, 5);
        if (newMarket != selectedMarket) { selectedMarket = newMarket; lastCostMarket = -1; }

        GUILayout.Space(8);
        GUILayout.Label("Development Speed (1-10): " + selectedSpeed + "  (+" + FormatMoney(GetSpeedCost(selectedSpeed)) + ")");
        int newSpeed = Mathf.Clamp(Mathf.RoundToInt(GUILayout.HorizontalSlider(selectedSpeed, 1, 10)), 1, 10);
        if (newSpeed != selectedSpeed) { selectedSpeed = newSpeed; lastCostSpeed = -1; }

        GUILayout.Space(8);
        int countryMax = GetCountryMax();
        GUILayout.Label("Country ID: " + selectedCountry + "  " + GetCountryName(selectedCountry));
        int newCountry = Mathf.Clamp(Mathf.RoundToInt(GUILayout.HorizontalSlider(selectedCountry, 0, countryMax)), 0, countryMax);
        if (newCountry != selectedCountry) { selectedCountry = newCountry; lastCostCountry = -1; }

        GUILayout.Space(8);
        int logoMax = GetLogoMax();
        GUILayout.Label("Logo ID: " + selectedLogoID + " / " + logoMax + "  (PNG import optional)");
        int newLogo = Mathf.Clamp(Mathf.RoundToInt(GUILayout.HorizontalSlider(selectedLogoID, 0, logoMax)), 0, logoMax);
        if (newLogo != selectedLogoID) { selectedLogoID = newLogo; lastCostLogo = -1; }

        GUILayout.BeginHorizontal();
        GUILayout.Label("PNG Path:", GUILayout.Width(100f));
        logoImportPath = GUILayout.TextField(logoImportPath ?? "", 210);
        if (GUILayout.Button("Browse", GUILayout.Width(72f)))
        {
            string picked = BrowseForPng();
            if (!string.IsNullOrEmpty(picked)) logoImportPath = picked;
        }
        if (GUILayout.Button("Import", GUILayout.Width(72f)))
        {
            int importedLogo = ImportLogoFromPath(logoImportPath);
            if (importedLogo >= 0) selectedLogoID = importedLogo;
        }
        GUILayout.EndHorizontal();

        if (!string.IsNullOrEmpty(logoStatus)) GUILayout.Label(logoStatus);

        GUILayout.Space(8);
        long totalCost = CalculateCost(selectedMarket * 20, selectedSpeed, selectedCountry, selectedLogoID);
        bool canAfford = main != null && main.money >= totalCost;
        string costText = main != null ? main.GetMoney(totalCost, true) : "$" + totalCost;
        GUI.color = canAfford ? Color.white : Color.red;
        GUILayout.Label("Total Cost: " + costText);
        GUI.color = Color.white;

        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Create Studio", GUILayout.Height(42f)))
        {
            if (canAfford)
            {
                if (CreateSubsidiary(studioName, selectedMarket, selectedSpeed, selectedCountry, selectedLogoID, totalCost))
                    showCreationMenu = false;
            }
            else
            {
                GUI_Main gui = FindObjectOfType<GUI_Main>();
                if (gui != null) gui.ShowNoMoney();
            }
        }

        if (GUILayout.Button("Cancel", GUILayout.Height(30f)))
            showCreationMenu = false;

        GUI.DragWindow();
    }

    private long CalculateCost(int market, int speed, int country, int logoID)
    {
        if (market == lastCostMarket && speed == lastCostSpeed && country == lastCostCountry && logoID == lastCostLogo)
            return cachedCost;

        long total = 2000000L;
        total += GetMarketCost(market);
        total += GetSpeedCost(speed);
        lastCostMarket = market;
        lastCostSpeed = speed;
        lastCostCountry = country;
        lastCostLogo = logoID;
        cachedCost = total;
        return total;
    }

    private static long GetMarketCost(int market)
    {
        market = Mathf.Clamp(market, 0, 100);
        if (market <= 20) return LerpLong(1000000L, 3000000L, market / 20f);
        if (market <= 40) return LerpLong(4000000L, 10000000L, (market - 21) / 19f);
        if (market <= 60) return LerpLong(12000000L, 30000000L, (market - 41) / 19f);
        if (market <= 80) return LerpLong(40000000L, 90000000L, (market - 61) / 19f);
        return LerpLong(120000000L, 250000000L, (market - 81) / 19f);
    }

    private static long GetSpeedCost(int speed)
    {
        speed = Mathf.Clamp(speed, 0, speedCosts.Length - 1);
        return speedCosts[speed];
    }

    private static long LerpLong(long min, long max, float t)
    {
        t = Mathf.Clamp01(t);
        return min + (long)((max - min) * t);
    }

    private string FormatMoney(long amount)
    {
        if (main != null) return main.GetMoney(amount, true);
        return "$" + amount.ToString("N0");
    }

    private int GetCountryMax()
    {
        if (text != null && text.country_EN != null && text.country_EN.Length > 0)
            return text.country_EN.Length - 1;
        return 63;
    }

    private string GetCountryName(int countryID)
    {
        if (text == null) return "";
        return text.GetCountry(countryID);
    }

    private int GetLogoMax()
    {
        RefreshCachedGameObjects();
        if (guiMain != null && guiMain.logoSprites != null && guiMain.logoSprites.Length > 0)
            return guiMain.logoSprites.Length - 1;
        return 0;
    }

    private int ImportLogoFromPath(string path)
    {
        RefreshCachedGameObjects();
        if (main == null || guiMain == null)
        {
            logoStatus = "Logo import failed: game scripts are not ready.";
            return -1;
        }

        if (string.IsNullOrEmpty(path) || !File.Exists(path))
        {
            logoStatus = "Logo import failed: PNG path not found.";
            return -1;
        }

        if (!path.ToLowerInvariant().EndsWith(".png"))
        {
            logoStatus = "Logo import failed: please use a .png file.";
            return -1;
        }

        string logoDir = Path.Combine(Application.dataPath, "Extern", "CompanyLogos");
        Directory.CreateDirectory(logoDir);
        int newLogoID = Directory.GetFiles(logoDir, "*.png")
            .Select(file => Path.GetFileNameWithoutExtension(file))
            .Select(name => { int v; return int.TryParse(name, out v) ? v : -1; })
            .DefaultIfEmpty(-1)
            .Max() + 1;

        string destination = Path.Combine(logoDir, newLogoID + ".png");
        File.Copy(path, destination, overwrite: true);

        Sprite importedSprite = main.LoadPNG(destination);
        List<Sprite> sprites = guiMain.logoSprites != null ? guiMain.logoSprites.ToList() : new List<Sprite>();
        while (sprites.Count <= newLogoID) sprites.Add(null);
        sprites[newLogoID] = importedSprite;
        guiMain.logoSprites = sprites.ToArray();

        logoStatus = "Imported logo as ID " + newLogoID + ".";
        log.LogInfo("Imported logo '" + path + "' as company logo ID=" + newLogoID);
        return newLogoID;
    }

    private string BrowseForPng()
    {
        OpenFileName ofn = new OpenFileName();
        ofn.structSize = Marshal.SizeOf(typeof(OpenFileName));
        ofn.filter = "PNG files\0*.png\0All files\0*.*\0";
        ofn.file = new string(new char[1024]);
        ofn.maxFile = ofn.file.Length;
        ofn.title = "Select company logo PNG";
        ofn.defExt = "png";
        ofn.flags = 0x00001000 | 0x00000800 | 0x00080000;

        if (GetOpenFileName(ofn)) return ofn.file;
        return "";
    }

    private bool CreateSubsidiary(string name, int market, int speed, int country, int logoID, long cost)
    {
        RefreshCachedGameObjects();

        if (main == null || publisherSpawner == null)
        {
            log.LogError("Cannot create studio: mainScript or publisher spawner is missing.");
            return false;
        }

        if (main.money < cost)
        {
            GUI_Main gui = FindObjectOfType<GUI_Main>();
            if (gui != null) gui.ShowNoMoney();
            return false;
        }

        publisherScript newStudio = publisherSpawner.CreatePublisher();
        if (newStudio == null)
        {
            log.LogError("CreatePublisher returned null.");
            return false;
        }

        main.Pay(cost, 29);
        newStudio.myID = GetFreePublisherID();
        newStudio.SetOwnName(string.IsNullOrEmpty(name) ? "New Studio" : name.Trim());
        newStudio.stars = Mathf.Clamp(market * 20, 20, 100);
        newStudio.developmentSpeed = Mathf.Clamp(speed, 1, 10);
        newStudio.isUnlocked = true;
        newStudio.developer = true;
        newStudio.publisher = false;
        newStudio.relation = 100f;
        newStudio.share = 0f;
        newStudio.onlyMobile = false;
        newStudio.ownPlatform = false;
        newStudio.exklusive = false;
        newStudio.notForSale = false;
        newStudio.fanGenre = Random.Range(0, 10);
        newStudio.country = Mathf.Clamp(country, 0, GetCountryMax());
        newStudio.logoID = Mathf.Clamp(logoID, 0, GetLogoMax());
        newStudio.date_year = main.year;
        newStudio.date_month = main.month;
        newStudio.date_year_end = main.year + 100;
        newStudio.date_month_end = 1;
        newStudio.firmenwert = cost;

        newStudio.Init();
        newStudio.SetAsTochterfirma(main.myID);
        InitSubsidiaryDefaults(newStudio);

        AddStudioData(newStudio.myID, cost, main.year, main.month);
        main.FindPublishers();

        newStudio.amountTrys = 0;
        newStudio.newGameInWeeks = 1;
        newStudio.newGameInWeeksORG = 1;

        Menu_Statistics_Tochterfirmen subsidiariesMenu = FindObjectOfType<Menu_Statistics_Tochterfirmen>();
        if (subsidiariesMenu != null && subsidiariesMenu.gameObject.activeInHierarchy)
            subsidiariesMenu.Init();

        sfxScript sfx = GameObject.Find("SFX")?.GetComponent<sfxScript>();
        if (sfx != null) sfx.PlaySound(22, true);

        log.LogInfo("Created organic subsidiary '" + newStudio.GetName() + "' with ID=" + newStudio.myID + " cost=" + cost);
        return true;
    }
}
