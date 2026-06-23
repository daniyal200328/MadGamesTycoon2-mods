using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

[BepInPlugin("org.bepinex.plugins.subsidiary2", "Subsidiary 2.0", "2.0.0")]
public class Subsidiary2Plugin : BaseUnityPlugin
{
    private static Subsidiary2Plugin instance;
    public static ManualLogSource log;
    private static Harmony harmonyInstance;
    private static bool isPatched = false;

    private bool showCreationMenu;
    private Rect windowRect;
    private string studioName = "My New Studio";
    private int selectedMarket = 20;
    private int selectedSpeed;
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

    public static mainScript GetMainScript()
    {
        if (cachedMainScript == null)
        {
            cachedMainScript = UnityEngine.Object.FindObjectOfType<mainScript>();
        }
        return cachedMainScript;
    }

    public static GUI_Main GetGuiMain()
    {
        if (cachedGuiMain == null)
        {
            cachedGuiMain = UnityEngine.Object.FindObjectOfType<GUI_Main>();
        }
        return cachedGuiMain;
    }

    public static textScript GetTextScript()
    {
        if (cachedTextScript == null)
        {
            cachedTextScript = UnityEngine.Object.FindObjectOfType<textScript>();
        }
        return cachedTextScript;
    }

    public static games GetGamesScript()
    {
        if (cachedGames == null)
        {
            cachedGames = UnityEngine.Object.FindObjectOfType<games>();
        }
        return cachedGames;
    }

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

    private static string SavePath => Path.Combine(Path.GetDirectoryName(typeof(Subsidiary2Plugin).Assembly.Location), "SubsidiaryData.json");

    private static readonly long[] speedCosts =
    {
        0L,
        3000000L,
        7000000L,
        12000000L,
        20000000L,
        32000000L,
        48000000L,
        70000000L,
        100000000L,
        140000000L,
        190000000L
    };

    private void Awake()
    {
        instance = this;
        log = Logger;
        windowRect = new Rect(Screen.width / 2f - 260f, Screen.height / 2f - 240f, 520f, 480f);
        harmonyInstance = new Harmony("org.bepinex.plugins.subsidiary2");
        LoadState();
        log.LogInfo("Subsidiary 2.0 loaded. Waiting for gameplay load to apply patches...");
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
                    isPatched = true; // prevent infinite retries on fatal patch failure
                }
            }
        }

        if (isPatched && Input.GetKeyDown(KeyCode.S) && IsShortcutHeld())
        {
            ToggleCreationWindow();
        }
    }

    private void OnGUI()
    {
        if (!showCreationMenu)
        {
            return;
        }

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
            if (gui != null)
            {
                gui.CloseMenu();
            }
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
        if (windowStyle != null)
        {
            return;
        }

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
        {
            return;
        }
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

        GUILayout.Label("Market / Experience (0-100): " + selectedMarket + "  (" + GetStarTier(selectedMarket) + " star tier)");
        int newMarket = Mathf.Clamp(Mathf.RoundToInt(GUILayout.HorizontalSlider(selectedMarket, 0, 100)), 0, 100);
        if (newMarket != selectedMarket)
        {
            selectedMarket = newMarket;
            lastCostMarket = -1;
        }

        GUILayout.Space(8);

        GUILayout.Label("Development Speed (0-10): " + selectedSpeed + "  (+" + FormatMoney(GetSpeedCost(selectedSpeed)) + ")");
        int newSpeed = Mathf.Clamp(Mathf.RoundToInt(GUILayout.HorizontalSlider(selectedSpeed, 0, 10)), 0, 10);
        if (newSpeed != selectedSpeed)
        {
            selectedSpeed = newSpeed;
            lastCostSpeed = -1;
        }

        GUILayout.Space(8);

        int countryMax = GetCountryMax();
        GUILayout.Label("Country ID: " + selectedCountry + "  " + GetCountryName(selectedCountry));
        int newCountry = Mathf.Clamp(Mathf.RoundToInt(GUILayout.HorizontalSlider(selectedCountry, 0, countryMax)), 0, countryMax);
        if (newCountry != selectedCountry)
        {
            selectedCountry = newCountry;
            lastCostCountry = -1;
        }

        GUILayout.Space(8);

        int logoMax = GetLogoMax();
        GUILayout.Label("Logo ID: " + selectedLogoID + " / " + logoMax + "  (PNG import optional)");
        int newLogo = Mathf.Clamp(Mathf.RoundToInt(GUILayout.HorizontalSlider(selectedLogoID, 0, logoMax)), 0, logoMax);
        if (newLogo != selectedLogoID)
        {
            selectedLogoID = newLogo;
            lastCostLogo = -1;
        }

        GUILayout.BeginHorizontal();
        GUILayout.Label("PNG Path:", GUILayout.Width(100f));
        logoImportPath = GUILayout.TextField(logoImportPath ?? "", 210);
        if (GUILayout.Button("Browse", GUILayout.Width(72f)))
        {
            string picked = BrowseForPng();
            if (!string.IsNullOrEmpty(picked))
            {
                logoImportPath = picked;
            }
        }
        if (GUILayout.Button("Import", GUILayout.Width(72f)))
        {
            int importedLogo = ImportLogoFromPath(logoImportPath);
            if (importedLogo >= 0)
            {
                selectedLogoID = importedLogo;
            }
        }
        GUILayout.EndHorizontal();

        if (!string.IsNullOrEmpty(logoStatus))
        {
            GUILayout.Label(logoStatus);
        }

        GUILayout.Space(8);

        long totalCost = CalculateCost(selectedMarket, selectedSpeed, selectedCountry, selectedLogoID);
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
                {
                    showCreationMenu = false;
                }
            }
            else
            {
                GUI_Main gui = FindObjectOfType<GUI_Main>();
                if (gui != null)
                {
                    gui.ShowNoMoney();
                }
            }
        }

        if (GUILayout.Button("Cancel", GUILayout.Height(30f)))
        {
            showCreationMenu = false;
        }

        GUI.DragWindow();
    }

    private int GetStarTier(int market)
    {
        if (market <= 0) return 1;
        return Mathf.Clamp(Mathf.CeilToInt(market / 20f), 1, 5);
    }

    private long CalculateCost(int market, int speed, int country, int logoID)
    {
        if (market == lastCostMarket && speed == lastCostSpeed && country == lastCostCountry && logoID == lastCostLogo)
        {
            return cachedCost;
        }

        long total = 2000000L; // Base startup cost
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
        if (market <= 20)
        {
            return LerpLong(1000000L, 3000000L, market / 20f);
        }
        if (market <= 40)
        {
            return LerpLong(4000000L, 10000000L, (market - 21) / 19f);
        }
        if (market <= 60)
        {
            return LerpLong(12000000L, 30000000L, (market - 41) / 19f);
        }
        if (market <= 80)
        {
            return LerpLong(40000000L, 90000000L, (market - 61) / 19f);
        }
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
        {
            return text.country_EN.Length - 1;
        }
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
        {
            return guiMain.logoSprites.Length - 1;
        }
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
            .Select(name => {
                int value;
                return int.TryParse(name, out value) ? value : -1;
            })
            .DefaultIfEmpty(-1)
            .Max() + 1;

        string destination = Path.Combine(logoDir, newLogoID + ".png");
        File.Copy(path, destination, overwrite: true);

        Sprite importedSprite = main.LoadPNG(destination);
        List<Sprite> sprites = guiMain.logoSprites != null ? guiMain.logoSprites.ToList() : new List<Sprite>();
        while (sprites.Count <= newLogoID)
        {
            sprites.Add(null);
        }
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

        if (GetOpenFileName(ofn))
        {
            return ofn.file;
        }
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
        newStudio.stars = Mathf.Clamp(market, 0, 100);
        newStudio.developmentSpeed = Mathf.Clamp(speed, 0, 10);
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

        // Safe decoupled project instantiation - triggers CreateNewGame2 in next weekly update tick after Start() executes
        newStudio.amountTrys = 0;
        newStudio.newGameInWeeks = 1;
        newStudio.newGameInWeeksORG = 1;

        Menu_Statistics_Tochterfirmen subsidiariesMenu = FindObjectOfType<Menu_Statistics_Tochterfirmen>();
        if (subsidiariesMenu != null && subsidiariesMenu.gameObject.activeInHierarchy)
        {
            subsidiariesMenu.Init();
        }

        sfxScript sfx = GameObject.Find("SFX") != null ? GameObject.Find("SFX").GetComponent<sfxScript>() : null;
        if (sfx != null)
        {
            sfx.PlaySound(22, true);
        }

        log.LogInfo("Created organic subsidiary '" + newStudio.GetName() + "' with ID=" + newStudio.myID + " cost=" + cost);
        return true;
    }

    public static bool IsOrganicStudio(publisherScript studio)
    {
        if (studio == null || !studio) return false;
        try
        {
            return (studio.myID >= 9000 && studio.myID < 10000) || (studio.myID >= 90000 && studio.myID < 100000);
        }
        catch
        {
            return false;
        }
    }

    private int GetFreePublisherID()
    {
        HashSet<int> usedIDs = new HashSet<int>(FindObjectsOfType<publisherScript>().Select(p => p.myID));
        for (int id = 9000; id < 10000; id++)
        {
            if (!usedIDs.Contains(id)) return id;
        }
        return Random.Range(9000, 9999);
    }

    private void InitSubsidiaryDefaults(publisherScript studio)
    {
        studio.tf_geschlossen = false;
        studio.tf_autoRelease = false;
        studio.tf_onlyPlayerConsole = false;
        studio.tf_allowMMO = true;
        studio.tf_allowF2P = true;
        studio.tf_allowAddon = true;
        studio.tf_noArcade = false;
        studio.tf_noHandy = false;
        studio.tf_noRetro = false;
        studio.tf_noPorts = false;
        studio.tf_noBudget = false;
        studio.tf_noGOTY = false;
        studio.tf_noBundles = false;
        studio.tf_noAddonBundles = false;
        studio.tf_noRemaster = false;
        studio.tf_noSpinoffs = false;
        studio.tf_autoGamePass = false;
        studio.tf_gameGenre = 0;
        studio.tf_gameSize = 0;
        studio.tf_entwicklungsdauer = 1;
        studio.tf_publisher = studio.publisher;
        studio.tf_developer = studio.developer;
        studio.tf_ownPublisher = true;
        studio.tf_gameTopic = -1;
        studio.tf_autoReleaseVal = 0;
        studio.tf_ownPublisherPriority = -1;
        studio.tf_umsatz_allTime = 0L;
        studio.tf_engine = -1;

        studio.awards = new int[30];
        studio.tf_umsatz = new long[24];
        studio.tf_ipFocus = new int[6];
        for (int i = 0; i < studio.tf_ipFocus.Length; i++) studio.tf_ipFocus[i] = -1;
        studio.tf_platformFocus = new int[4];
        for (int i = 0; i < studio.tf_platformFocus.Length; i++) studio.tf_platformFocus[i] = -1;
    }

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

    private static void SaveState()
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



    public static bool BlockLockedOrganicSale(Menu_W_FirmaVerkaufen menu, publisherScript studio)
    {
        try {
            if (studio == null || !IsOrganicStudio(studio))
            {
                return false;
            }

            mainScript mainScript = studio.mS_ != null ? studio.mS_ : GetMainScript();
            if (mainScript == null)
            {
                return false;
            }

            int creationYear = studio.date_year;
            int creationMonth = studio.date_month;
            try {
                StudioData data = GetStudioData(studio.myID);
                if (data != null) {
                    creationYear = data.creationYear;
                    creationMonth = data.creationMonth;
                }
            } catch {}

            int ageInMonths = (mainScript.year - creationYear) * 12 + (mainScript.month - creationMonth);
            if (ageInMonths < 12)
            {
                int monthsLeft = 12 - ageInMonths;
                GUI_Main gui = GetGuiMain();
                if (gui != null)
                {
                    gui.MessageBox("Created subsidiaries cannot be sold for the first 12 months.\n\nMonths remaining: " + monthsLeft, closeMenu: false);
                }

                if (menu != null)
                {
                    menu.gameObject.SetActive(false);
                }

                if (log != null) log.LogInfo("Blocked organic subsidiary sale during startup lock. Studio=" + studio.GetName() + " monthsLeft=" + monthsLeft);
                return true;
            }
        } catch (System.Exception ex) {
            if (log != null) log.LogError("Error in BlockLockedOrganicSale: " + ex);
        }
        return false;
    }
 
    private static System.Reflection.MethodInfo dynamicCostMethod = null;
    private static bool searchedDynamicCost = false;
 
    public static long GetDynamicUpkeepReflected(publisherScript studio)
    {
        if (studio == null) return 0L;
        if (!searchedDynamicCost)
        {
            searchedDynamicCost = true;
            try
            {
                foreach (var asm in System.AppDomain.CurrentDomain.GetAssemblies())
                {
                    var t = asm.GetType("DynamicSubsidiaryTimelinePlugin");
                    if (t != null)
                    {
                        dynamicCostMethod = t.GetMethod("CalculateDynamicVerwaltungskosten", 
                            System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                        break;
                    }
                }
            }
            catch {}
        }
 
        if (dynamicCostMethod != null)
        {
            try
            {
                return (long)dynamicCostMethod.Invoke(null, new object[] { studio });
            }
            catch {}
        }
        return studio.GetVerwaltungskosten();
    }
 
    public static string GetSafeOrganicTooltip(publisherScript studio, mainScript mS, textScript tS)
    {
        if (studio == null) return "";
        string name = studio.name_EN;
        if (string.IsNullOrEmpty(name)) name = "Organic Studio";
 
        string typeStr = (studio.developer && !studio.publisher) 
            ? (tS != null ? tS.GetText(274) : "Developer") 
            : (tS != null ? tS.GetText(432) : "Publisher");
 
        string countryName = (tS != null) ? tS.GetCountry(studio.country) : "Unknown Country";
        
        string text = $"<b><size=18>{name}</size></b>";
        if (tS != null)
        {
            text += "\n<b><size=15><color=green>" + tS.GetText(1924) + "</color></size></b>"; // Subsidiary
        }
        text += "\n<b>" + typeStr + "</b>";
        text += "\n<b>" + countryName + "</b>";
        
        // Date
        if (tS != null)
        {
            int monthIndex = Mathf.Clamp(studio.date_month - 1 + 221, 221, 232);
            text += "\n<b>" + tS.GetText(monthIndex) + " " + studio.date_year + "</b>";
        }
        
        // Stars
        string starLabel = (tS != null) ? tS.GetText(434) : "Market";
        text += "\n\n" + starLabel + ": <color=blue><size=20>";
        int starsCount = Mathf.Clamp(Mathf.RoundToInt(studio.stars / 20f), 0, 5);
        for (int j = 0; j < starsCount; j++)
        {
            text += "★";
        }
        text += "</size></color>";
        
        // Games count
        long gamesCount = 0;
        games gamesScript = studio.games_ != null ? studio.games_ : UnityEngine.Object.FindObjectOfType<games>();
        if (gamesScript != null && gamesScript.arrayGamesScripts != null)
        {
            for (int i = 0; i < gamesScript.arrayGamesScripts.Length; i++)
            {
                gameScript game = gamesScript.arrayGamesScripts[i];
                if (game != null && game.developerID == studio.myID && game.ownerID == studio.myID && !game.inDevelopment && !game.schublade && !game.auftragsspiel && !game.pubAngebot && game.sellsTotal > 0)
                {
                    gamesCount++;
                }
            }
        }
        if (tS != null)
        {
            text += "\n" + tS.GetText(271) + ": <color=blue>" + (mS != null ? mS.GetMoney(gamesCount, showDollar: false) : gamesCount.ToString()) + "</color>";
        }
 
        // Financials
        if (tS != null && mS != null)
        {
            text += "\n\n" + tS.GetText(1930) + ": <color=green><b>" + mS.GetMoney(studio.tf_umsatz_allTime, showDollar: true) + "</b></color>";
            
            long rev24 = 0;
            if (studio.tf_umsatz != null)
            {
                for (int i = 0; i < studio.tf_umsatz.Length; i++)
                {
                    if (studio.tf_umsatz[i] > 0L)
                    {
                        rev24 += studio.tf_umsatz[i];
                    }
                }
            }
            text += "\n" + tS.GetText(698) + ": <color=green><b>" + mS.GetMoney(rev24, showDollar: true) + "</b></color>";
            
            long adminCost = GetDynamicUpkeepReflected(studio);
            text += "\n" + tS.GetText(1934) + ": <color=red><b>" + mS.GetMoney(adminCost, showDollar: true) + "</b></color>";
        }
        
        return text;
    }

    public static void SafeCheckPublisherBuyedSize(platformScript platform, int publisherID)
    {
        if (platform == null || publisherID < 0) return;

        if (platform.publisherBuyed == null || platform.publisherBuyed.Length <= publisherID)
        {
            int newSize = Mathf.Max(publisherID + 1, platform.publisherBuyed != null ? platform.publisherBuyed.Length * 2 : 128);
            bool[] newArray = new bool[newSize];
            if (platform.publisherBuyed != null && platform.publisherBuyed.Length > 0)
            {
                System.Array.Copy(platform.publisherBuyed, newArray, platform.publisherBuyed.Length);
            }
            platform.publisherBuyed = newArray;
            if (log != null) log.LogInfo($"Resized platform publisherBuyed array to {newSize} to support publisherID={publisherID}");
        }
    }

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

    public static void AddOrganicUpgradeInvestment(publisherScript studio, long upgradeCost)
    {
        if (studio == null || !IsOrganicStudio(studio))
        {
            return;
        }

        if (state == null) state = new ModState();
        if (state.studios == null) state.studios = new List<StudioData>();
        StudioData data = GetStudioData(studio.myID);
        if (data != null)
        {
            data.upgradeInvestments += upgradeCost;
            SaveState();
            if (log != null) log.LogInfo($"Added upgrade investment of {upgradeCost} to organic studio '{studio.GetName()}' ID={studio.myID}. Total upgrade investment is now {data.upgradeInvestments}.");
        }
    }

    public static string GetGoodwillTrendLabel(int studioID)
    {
        try
        {
            System.Type pluginType = System.Type.GetType("DynamicStudioGoodwillPlugin, DynamicStudioGoodwill");
            if (pluginType == null)
            {
                foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
                {
                    pluginType = assembly.GetType("DynamicStudioGoodwillPlugin");
                    if (pluginType != null)
                    {
                        break;
                    }
                }
            }

            if (pluginType != null)
            {
                System.Reflection.FieldInfo statesField = pluginType.GetField("states", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                if (statesField != null)
                {
                    object statesObj = statesField.GetValue(null);
                    if (statesObj != null)
                    {
                        System.Reflection.MethodInfo tryGetValueMethod = statesObj.GetType().GetMethod("TryGetValue");
                        if (tryGetValueMethod != null)
                        {
                            object[] args = new object[] { studioID, null };
                            bool found = (bool)tryGetValueMethod.Invoke(statesObj, args);
                            if (found && args[1] != null)
                            {
                                System.Reflection.FieldInfo labelField = args[1].GetType().GetField("Label");
                                if (labelField != null)
                                {
                                    return labelField.GetValue(args[1]) as string;
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            if (log != null) log.LogWarning("Failed to query goodwill trend via reflection: " + ex.Message);
        }
        return null;
    }

    public static long GetOrganicSaleValue(publisherScript studio)
    {
        try
        {
            if (studio == null)
            {
                return 0L;
            }

            if (state == null) state = new ModState();
            if (state.studios == null) state.studios = new List<StudioData>();

            StudioData data = GetStudioData(studio.myID);
            long creationCost = data != null ? data.creationCost : studio.firmenwert;
            if (creationCost <= 0)
            {
                creationCost = studio.firmenwert > 0 ? studio.firmenwert : 2000000L;
            }
            long upgradeInvestments = data != null ? data.upgradeInvestments : 0L;

            long baseValue = (long)(creationCost * 0.60f) + (long)(upgradeInvestments * 0.70f);
            long ipValue = GetOrganicIpValue(studio);
            long recentProfitMultiplier = GetRecentPositiveRevenue(studio) / 2L;

            long totalValue = baseValue + ipValue + recentProfitMultiplier;

            if (GetReleasedOrganicGames(studio) == 0)
            {
                long cap = (long)(creationCost * 0.60f); // capped at 60% of creation cost as requested (50-60%)
                if (totalValue > cap)
                {
                    totalValue = cap;
                }
            }

            // Apply Goodwill Trend multiplier adjustments if Goodwill mod is present
            string trend = GetGoodwillTrendLabel(studio.myID);
            if (!string.IsNullOrEmpty(trend))
            {
                if (trend == "Rising")
                {
                    totalValue = (long)(totalValue * 1.15f);
                }
                else if (trend == "Commercial Powerhouse")
                {
                    totalValue = (long)(totalValue * 1.30f);
                }
                else if (trend == "Declining" || trend == "In Crisis")
                {
                    totalValue = (long)(totalValue * 0.80f);
                }
            }

            return System.Math.Max(0L, totalValue);
        }
        catch (System.Exception ex)
        {
            if (log != null) log.LogError($"GetOrganicSaleValue failed: {ex}");
            return studio != null ? studio.firmenwert : 2000000L;
        }
    }

    private static long GetOrganicIpValue(publisherScript studio)
    {
        try {
            games gamesScript = studio.games_ != null ? studio.games_ : GetGamesScript();
            if (gamesScript == null || gamesScript.arrayGamesScripts == null)
            {
                return 0L;
            }

            long value = 0L;
            for (int i = 0; i < gamesScript.arrayGamesScripts.Length; i++)
            {
                gameScript game = gamesScript.arrayGamesScripts[i];
                if (game != null && game.sellsTotal > 0 && !game.inDevelopment && !game.schublade && !game.pubAngebot && !game.auftragsspiel && game.IsMyIP(studio) && game.mainIP == game.myID)
                {
                    value += game.GetIpWert();
                }
            }
            return value;
        } catch {
            return 0L;
        }
    }

    private static long GetRecentPositiveRevenue(publisherScript studio)
    {
        try {
            if (studio.tf_umsatz == null)
            {
                return 0L;
            }

            long total = 0L;
            for (int i = 0; i < studio.tf_umsatz.Length; i++)
            {
                if (studio.tf_umsatz[i] > 0L)
                {
                    total += studio.tf_umsatz[i];
                }
            }
            return total;
        } catch {
            return 0L;
        }
    }

    private static int GetReleasedOrganicGames(publisherScript studio)
    {
        try {
            games gamesScript = studio.games_ != null ? studio.games_ : GetGamesScript();
            if (gamesScript == null || gamesScript.arrayGamesScripts == null)
            {
                return 0;
            }

            int count = 0;
            for (int i = 0; i < gamesScript.arrayGamesScripts.Length; i++)
            {
                gameScript game = gamesScript.arrayGamesScripts[i];
                if (game != null && game.developerID == studio.myID && game.ownerID == studio.myID && !game.inDevelopment && !game.schublade && !game.auftragsspiel && !game.pubAngebot && game.sellsTotal > 0)
                {
                    count++;
                }
            }
            return count;
        } catch {
            return 0;
        }
    }
}

[HarmonyPatch(typeof(publisherScript), "IsMyTochterfirma")]
public static class OrganicSubsidiaryIsMyTochterfirmaPatch
{
    public static bool Prefix(publisherScript __instance, ref bool __result)
    {
        if (__instance == null || !__instance)
        {
            __result = false;
            return false;
        }

        try {
            mainScript mainScript = __instance.mS_ != null ? __instance.mS_ : Subsidiary2Plugin.GetMainScript();
            if (mainScript == null)
            {
                __result = false;
                return false;
            }

            if (__instance.isPlayer)
            {
                __result = false;
                return false;
            }

            if (__instance.myID >= 100000)
            {
                __result = false;
                return false;
            }

            __result = (__instance.ownerID == mainScript.myID);
        } catch {
            __result = false;
        }
        return false; // block vanilla unsafe execution
    }
}

[HarmonyPatch(typeof(publisherScript), "GetName")]
public static class OrganicSubsidiaryGetNamePatch
{
    public static bool Prefix(publisherScript __instance, ref string __result)
    {
        try {
            if (__instance != null && Subsidiary2Plugin.IsOrganicStudio(__instance))
            {
                __result = !string.IsNullOrEmpty(__instance.name_EN) ? __instance.name_EN : "Organic Studio";
                return false; // skip vanilla GetName
            }
        } catch {
            if (__instance != null && Subsidiary2Plugin.IsOrganicStudio(__instance))
            {
                __result = "Organic Studio";
                return false; // skip vanilla
            }
        }
        return true;
    }
}

[HarmonyPatch(typeof(publisherScript), "GetLogo")]
public static class OrganicSubsidiaryGetLogoPatch
{
    public static bool Prefix(publisherScript __instance, ref Sprite __result)
    {
        try {
            if (__instance != null && Subsidiary2Plugin.IsOrganicStudio(__instance))
            {
                GUI_Main guiMain = __instance.guiMain_ != null ? __instance.guiMain_ : Subsidiary2Plugin.GetGuiMain();
                if (guiMain != null && guiMain.logoSprites != null && __instance.logoID >= 0 && __instance.logoID < guiMain.logoSprites.Length)
                {
                    __result = guiMain.logoSprites[__instance.logoID];
                    return false; // skip vanilla
                }
            }
        } catch {
            if (__instance != null && Subsidiary2Plugin.IsOrganicStudio(__instance))
            {
                __result = null;
                return false; // skip vanilla
            }
        }
        return true;
    }
}

[HarmonyPatch(typeof(publisherScript), "GetTooltip")]
public static class OrganicSubsidiaryGetTooltipPatch
{
    public static bool Prefix(publisherScript __instance, ref string __result)
    {
        try {
            if (__instance != null && Subsidiary2Plugin.IsOrganicStudio(__instance))
            {
                mainScript mS = __instance.mS_ != null ? __instance.mS_ : Subsidiary2Plugin.GetMainScript();
                textScript tS = __instance.tS_ != null ? __instance.tS_ : (mS != null ? mS.tS_ : Subsidiary2Plugin.GetTextScript());
                __result = Subsidiary2Plugin.GetSafeOrganicTooltip(__instance, mS, tS);
                return false; // skip vanilla GetTooltip
            }
        } catch {
            if (__instance != null && Subsidiary2Plugin.IsOrganicStudio(__instance))
            {
                __result = "";
                return false; // skip vanilla
            }
        }
        return true;
    }
}



[HarmonyPatch(typeof(publisherScript), "GetDeveloperPublisherString")]
public static class OrganicSubsidiaryDevPubStringPatch
{
    public static bool Prefix(publisherScript __instance, ref string __result)
    {
        try {
            if (__instance != null && Subsidiary2Plugin.IsOrganicStudio(__instance))
            {
                textScript tS = __instance.tS_ != null ? __instance.tS_ : Subsidiary2Plugin.GetTextScript();
                if (__instance.developer && !__instance.publisher)
                {
                    __result = tS != null ? tS.GetText(274) : "Developer";
                }
                else if (!__instance.developer && __instance.publisher)
                {
                    __result = tS != null ? tS.GetText(432) : "Publisher";
                }
                else
                {
                    __result = (tS != null ? tS.GetText(432) : "Publisher") + " & " + (tS != null ? tS.GetText(274) : "Developer");
                }
                return false;
            }
        } catch {
            if (__instance != null && Subsidiary2Plugin.IsOrganicStudio(__instance))
            {
                __result = "Developer";
                return false; // skip vanilla
            }
        }
        return true;
    }
}

[HarmonyPatch(typeof(Menu_W_FirmaVerkaufen), "Init")]
public static class OrganicSubsidiarySellWindowPatch
{
    public static bool Prefix(Menu_W_FirmaVerkaufen __instance, publisherScript script_)
    {
        try {
            if (Subsidiary2Plugin.BlockLockedOrganicSale(__instance, script_))
            {
                return false; // Skip vanilla execution
            }
        } catch {}
        return true;
    }
}




[HarmonyPatch(typeof(tooltip), "Start")]
public static class SafeTooltipStartPatch
{
    public static bool Prefix(tooltip __instance)
    {
        try
        {
            GameObject main = AccessTools.Field(typeof(tooltip), "main_").GetValue(__instance) as GameObject;
            if (main == null)
            {
                main = GameObject.FindWithTag("Main");
                AccessTools.Field(typeof(tooltip), "main_").SetValue(__instance, main);
            }
            if (main == null) return false;

            mainScript mS = AccessTools.Field(typeof(tooltip), "mS_").GetValue(__instance) as mainScript;
            if (mS == null)
            {
                mS = main.GetComponent<mainScript>();
                AccessTools.Field(typeof(tooltip), "mS_").SetValue(__instance, mS);
            }
            if (mS == null) return false;

            if (mS != null && !mS.guiMain_)
            {
                mS.FindScripts();
            }

            textScript tS = AccessTools.Field(typeof(tooltip), "tS_").GetValue(__instance) as textScript;
            if (tS == null && mS != null)
            {
                tS = mS.tS_;
                AccessTools.Field(typeof(tooltip), "tS_").SetValue(__instance, tS);
            }

            settingsScript settings = AccessTools.Field(typeof(tooltip), "settings_").GetValue(__instance) as settingsScript;
            if (settings == null && mS != null)
            {
                settings = mS.settings_;
                AccessTools.Field(typeof(tooltip), "settings_").SetValue(__instance, settings);
            }

            GUI_Main guiMain = AccessTools.Field(typeof(tooltip), "guiMain_").GetValue(__instance) as GUI_Main;
            if (guiMain == null && mS != null)
            {
                guiMain = mS.guiMain_;
                AccessTools.Field(typeof(tooltip), "guiMain_").SetValue(__instance, guiMain);
            }
        }
        catch {}
        return true; // Let vanilla run!
    }
}

[HarmonyPatch(typeof(setFont), "OnEnable")]
public static class SafeSetFontOnEnablePatch
{
    public static bool Prefix(setFont __instance)
    {
        try
        {
            GameObject main = AccessTools.Field(typeof(setFont), "main_").GetValue(__instance) as GameObject;
            if (main == null)
            {
                main = GameObject.FindGameObjectWithTag("Main");
                AccessTools.Field(typeof(setFont), "main_").SetValue(__instance, main);
            }
            if (main == null) return false;

            mainScript mS = AccessTools.Field(typeof(setFont), "mS_").GetValue(__instance) as mainScript;
            if (mS == null)
            {
                mS = main.GetComponent<mainScript>();
                AccessTools.Field(typeof(setFont), "mS_").SetValue(__instance, mS);
            }
            if (mS == null) return false;

            settingsScript settings = AccessTools.Field(typeof(setFont), "settings_").GetValue(__instance) as settingsScript;
            if (settings == null)
            {
                settings = main.GetComponent<settingsScript>();
                AccessTools.Field(typeof(setFont), "settings_").SetValue(__instance, settings);
            }
            if (settings == null) return false;
        }
        catch {}
        return true; // Let vanilla run!
    }
}

[HarmonyPatch(typeof(setText), "OnEnable")]
public static class SafeSetTextOnEnablePatch
{
    public static bool Prefix(setText __instance)
    {
        try
        {
            GameObject main = AccessTools.Field(typeof(setText), "main_").GetValue(__instance) as GameObject;
            if (main == null)
            {
                main = GameObject.Find("Main");
                if (main == null)
                {
                    main = GameObject.FindGameObjectWithTag("Main");
                }
                AccessTools.Field(typeof(setText), "main_").SetValue(__instance, main);
            }
            if (main == null) return false;

            textScript tS = AccessTools.Field(typeof(setText), "tS_").GetValue(__instance) as textScript;
            if (tS == null)
            {
                tS = main.GetComponent<textScript>();
                AccessTools.Field(typeof(setText), "tS_").SetValue(__instance, tS);
            }
            if (tS == null) return false;

            settingsScript settings = AccessTools.Field(typeof(setText), "settings_").GetValue(__instance) as settingsScript;
            if (settings == null)
            {
                settings = main.GetComponent<settingsScript>();
                AccessTools.Field(typeof(setText), "settings_").SetValue(__instance, settings);
            }
        }
        catch {}
        return true; // Let vanilla run!
    }
}

[HarmonyPatch(typeof(platformScript), "SellPlayerKonsoleToNPC")]
public static class SafeSellPlayerKonsoleToNPCPatch
{
    public static void Prefix(platformScript __instance, publisherScript pS_)
    {
        try {
            if (__instance != null && pS_ != null)
            {
                Subsidiary2Plugin.SafeCheckPublisherBuyedSize(__instance, pS_.myID);
            }
        } catch {}
    }
}

[HarmonyPatch(typeof(publisherScript), "SelectPlayerPlatform")]
public static class SafeSelectPlayerPlatformPatch
{
    public static void Prefix(publisherScript __instance, platformScript script_)
    {
        try {
            if (__instance != null && script_ != null)
            {
                Subsidiary2Plugin.SafeCheckPublisherBuyedSize(script_, __instance.myID);
            }
        } catch {}
    }
}

[HarmonyPatch(typeof(Menu_W_FirmaVerkaufen), "Init")]
public static class Menu_W_FirmaVerkaufen_Init_Patch
{
    public static void Postfix(Menu_W_FirmaVerkaufen __instance, publisherScript script_)
    {
        try {
            if (script_ != null && Subsidiary2Plugin.IsOrganicStudio(script_))
            {
                long customValue = Subsidiary2Plugin.GetOrganicSaleValue(script_);
                textScript tS = AccessTools.Field(typeof(Menu_W_FirmaVerkaufen), "tS_").GetValue(__instance) as textScript;
                mainScript mS = AccessTools.Field(typeof(Menu_W_FirmaVerkaufen), "mS_").GetValue(__instance) as mainScript;
                
                if (tS != null && mS != null)
                {
                    string text = tS.GetText(1974);
                    text = text.Replace("<NAME>", "<color=blue>" + script_.GetName() + "</color>");
                    text = text.Replace("<NUM>", "<color=blue>" + mS.GetMoney(customValue, showDollar: true) + "</color>");
                    
                    string trend = Subsidiary2Plugin.GetGoodwillTrendLabel(script_.myID);
                    if (!string.IsNullOrEmpty(trend))
                    {
                        if (trend == "Rising")
                        {
                            text += "\n\n<color=green>Negotiation: buyers are eager due to their <b>" + trend + "</b> status (+15% value)!</color>";
                        }
                        else if (trend == "Commercial Powerhouse")
                        {
                            text += "\n\n<color=green>Negotiation: buyers are extremely eager due to their <b>" + trend + "</b> status (+30% value)!</color>";
                        }
                        else if (trend == "Declining" || trend == "In Crisis")
                        {
                            text += "\n\n<color=red>Negotiation: buyers are lowballing due to their <b>" + trend + "</b> status (-20% value)!</color>";
                        }
                    }
                    
                    __instance.uiObjects[0].GetComponent<Text>().text = text;
                }
            }
        } catch (System.Exception ex) {
            if (Subsidiary2Plugin.log != null) Subsidiary2Plugin.log.LogError("Error in Menu_W_FirmaVerkaufen_Init_Patch: " + ex);
        }
    }
}

[HarmonyPatch(typeof(Menu_W_FirmaVerkaufen), "BUTTON_Yes")]
public static class Menu_W_FirmaVerkaufen_BUTTON_Yes_Patch
{
    public static bool Prefix(Menu_W_FirmaVerkaufen __instance)
    {
        try {
            publisherScript pS = AccessTools.Field(typeof(Menu_W_FirmaVerkaufen), "pS_").GetValue(__instance) as publisherScript;
            if (Subsidiary2Plugin.BlockLockedOrganicSale(__instance, pS))
            {
                return false; // Skip sell
            }
            if (pS != null && Subsidiary2Plugin.IsOrganicStudio(pS))
            {
                mainScript mS = AccessTools.Field(typeof(Menu_W_FirmaVerkaufen), "mS_").GetValue(__instance) as mainScript;
                textScript tS = AccessTools.Field(typeof(Menu_W_FirmaVerkaufen), "tS_").GetValue(__instance) as textScript;
                games gamesScript = AccessTools.Field(typeof(Menu_W_FirmaVerkaufen), "games_").GetValue(__instance) as games;
                gamepassScript gpS = AccessTools.Field(typeof(Menu_W_FirmaVerkaufen), "gpS_").GetValue(__instance) as gamepassScript;
                GUI_Main guiMain = AccessTools.Field(typeof(Menu_W_FirmaVerkaufen), "guiMain_").GetValue(__instance) as GUI_Main;

                if (mS != null && tS != null && gamesScript != null && gpS != null && guiMain != null)
                {
                    long customValue = Subsidiary2Plugin.GetOrganicSaleValue(pS);
                    mS.Earn(customValue, 12);
                    pS.RemoveTochterfirma();
                    pS.ResetTochterfirmaSettings();

                    if (mS.multiplayer)
                    {
                        if (mS.mpCalls_.isServer)
                        {
                            mS.mpCalls_.SERVER_Send_PublisherTochterfirmaSettings(pS);
                            mS.mpCalls_.SERVER_Send_Publisher(pS);
                        }
                        else
                        {
                            mS.mpCalls_.CLIENT_Send_PublisherTochterfirmaSettings(pS);
                            mS.mpCalls_.CLIENT_Send_Publisher(pS);
                        }
                    }

                    int num = 0;
                    for (int i = 0; i < gamesScript.arrayGamesScripts.Length; i++)
                    {
                        if ((bool)gamesScript.arrayGamesScripts[i] && gamesScript.arrayGamesScripts[i].IsMyIP(pS) && gamesScript.arrayGamesScripts[i].inGamePass)
                        {
                            gpS.GAMEPASS_RemoveGame(gamesScript.arrayGamesScripts[i], updateGamesAmount: false);
                            num++;
                        }
                    }
                    gpS.GetAmountGamePassGames();

                    if (num > 0)
                    {
                        string text = tS.GetText(2120);
                        text = text.Replace("<NUM>", num.ToString());
                        guiMain.MessageBox(text, closeMenu: false);
                    }

                    if (guiMain.uiObjects[387].activeSelf)
                    {
                        guiMain.uiObjects[387].SetActive(false);
                    }

                    if (guiMain.uiObjects[385].activeSelf)
                    {
                        guiMain.uiObjects[385].GetComponent<Menu_Statistics_Tochterfirmen>().BUTTON_Search();
                    }

                    __instance.BUTTON_Abbrechen();
                    return false; // skip vanilla
                }
            }
        } catch (System.Exception ex) {
            if (Subsidiary2Plugin.log != null) Subsidiary2Plugin.log.LogError("Error in Menu_W_FirmaVerkaufen_BUTTON_Yes_Patch: " + ex);
        }
        return true;
    }
}

[HarmonyPatch(typeof(Item_Stats_Tochterfirma), "SetData")]
public static class Item_Stats_Tochterfirma_SetData_Patch
{
    public static void Postfix(Item_Stats_Tochterfirma __instance)
    {
        try {
            if (__instance.pS_ != null && Subsidiary2Plugin.IsOrganicStudio(__instance.pS_))
            {
                long customValue = Subsidiary2Plugin.GetOrganicSaleValue(__instance.pS_);
                __instance.uiObjects[4].GetComponent<Text>().text = __instance.mS_.GetMoney(customValue, showDollar: true);
            }
        } catch (System.Exception ex) {
            if (Subsidiary2Plugin.log != null) Subsidiary2Plugin.log.LogError("Error in Item_Stats_Tochterfirma_SetData_Patch: " + ex);
        }
    }
}

[HarmonyPatch(typeof(Menu_Stats_Tochterfirma_Main), "Update")]
public static class Menu_Stats_Tochterfirma_Main_Update_Patch
{
    private static readonly System.Reflection.FieldInfo pSField = AccessTools.Field(typeof(Menu_Stats_Tochterfirma_Main), "pS_");
    private static readonly System.Reflection.FieldInfo mSField = AccessTools.Field(typeof(Menu_Stats_Tochterfirma_Main), "mS_");
    private static readonly System.Reflection.FieldInfo tSField = AccessTools.Field(typeof(Menu_Stats_Tochterfirma_Main), "tS_");

    public static void Postfix(Menu_Stats_Tochterfirma_Main __instance)
    {
        try {
            publisherScript pS = pSField != null ? pSField.GetValue(__instance) as publisherScript : null;
            if (pS != null && Subsidiary2Plugin.IsOrganicStudio(pS))
            {
                mainScript mS = mSField != null ? mSField.GetValue(__instance) as mainScript : null;
                textScript tS = tSField != null ? tSField.GetValue(__instance) as textScript : null;
                if (mS != null && tS != null)
                {
                    long customValue = Subsidiary2Plugin.GetOrganicSaleValue(pS);
                    __instance.uiObjects[7].GetComponent<Text>().text = tS.GetText(685) + ": <b>" + mS.GetMoney(customValue, showDollar: true) + "</b>";
                }
            }
        } catch (System.Exception ex) {
            if (Subsidiary2Plugin.log != null) Subsidiary2Plugin.log.LogError("Error in Menu_Stats_Tochterfirma_Main_Update_Patch: " + ex);
        }
    }
}

[HarmonyPatch(typeof(Menu_Stats_Tochterfirma_Main), "UpdateData")]
public static class Menu_Stats_Tochterfirma_Main_UpdateData_Patch
{
    private static readonly System.Reflection.FieldInfo pSField = AccessTools.Field(typeof(Menu_Stats_Tochterfirma_Main), "pS_");
    private static readonly System.Reflection.FieldInfo mSField = AccessTools.Field(typeof(Menu_Stats_Tochterfirma_Main), "mS_");
    private static readonly System.Reflection.FieldInfo tSField = AccessTools.Field(typeof(Menu_Stats_Tochterfirma_Main), "tS_");

    public static void Postfix(Menu_Stats_Tochterfirma_Main __instance)
    {
        try {
            publisherScript pS = pSField != null ? pSField.GetValue(__instance) as publisherScript : null;
            if (pS != null && Subsidiary2Plugin.IsOrganicStudio(pS))
            {
                mainScript mS = mSField != null ? mSField.GetValue(__instance) as mainScript : null;
                textScript tS = tSField != null ? tSField.GetValue(__instance) as textScript : null;
                if (mS != null && tS != null)
                {
                    long customValue = Subsidiary2Plugin.GetOrganicSaleValue(pS);
                    __instance.uiObjects[7].GetComponent<Text>().text = tS.GetText(685) + ": <b>" + mS.GetMoney(customValue, showDollar: true) + "</b>";
                }
            }
        } catch (System.Exception ex) {
            if (Subsidiary2Plugin.log != null) Subsidiary2Plugin.log.LogError("Error in Menu_Stats_Tochterfirma_Main_UpdateData_Patch: " + ex);
        }
    }
}

[HarmonyPatch(typeof(Menu_W_FirmaAufwerten), "BUTTON_Yes")]
public static class OrganicSubsidiaryUpgradePatch
{
    public static void Prefix(Menu_W_FirmaAufwerten __instance)
    {
        try {
            System.Reflection.FieldInfo publisherField = AccessTools.Field(typeof(Menu_W_FirmaAufwerten), "pS_");
            System.Reflection.FieldInfo mainField = AccessTools.Field(typeof(Menu_W_FirmaAufwerten), "mS_");
            publisherScript studio = publisherField != null ? publisherField.GetValue(__instance) as publisherScript : null;
            mainScript mainScript = mainField != null ? mainField.GetValue(__instance) as mainScript : null;

            if (studio == null || !Subsidiary2Plugin.IsOrganicStudio(studio) || mainScript == null || __instance.costs == null)
            {
                return;
            }

            int starsAmount = studio.GetStarsAmount();
            if (starsAmount < 0 || starsAmount >= __instance.costs.Length)
            {
                return;
            }

            long upgradeCost = __instance.costs[starsAmount];
            if (mainScript.money >= upgradeCost)
            {
                Subsidiary2Plugin.AddOrganicUpgradeInvestment(studio, upgradeCost);
            }
        } catch (System.Exception ex) {
            if (Subsidiary2Plugin.log != null) Subsidiary2Plugin.log.LogError("Error in OrganicSubsidiaryUpgradePatch: " + ex);
        }
    }
}
