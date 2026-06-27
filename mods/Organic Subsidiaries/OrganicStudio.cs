using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class Subsidiary2Plugin
{
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

    internal static int GetFreePublisherID()
    {
        HashSet<int> usedIDs = new HashSet<int>(Object.FindObjectsOfType<publisherScript>().Select(p => p.myID));
        for (int id = 9000; id < 10000; id++)
        {
            if (!usedIDs.Contains(id)) return id;
        }
        for (int id = 90000; id < 100000; id++)
        {
            if (!usedIDs.Contains(id)) return id;
        }
        log?.LogError("All organic studio ID ranges exhausted! Picking random ID from 90000-99999 — collision possible!");
        return Random.Range(90000, 99999);
    }

    internal static void InitSubsidiaryDefaults(publisherScript studio)
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

    public static bool BlockLockedOrganicSale(Menu_W_FirmaVerkaufen menu, publisherScript studio)
    {
        try
        {
            if (studio == null || !IsOrganicStudio(studio)) return false;

            mainScript mainScript = studio.mS_ ?? GetMainScript();
            if (mainScript == null) return false;

            int creationYear = studio.date_year;
            int creationMonth = studio.date_month;
            try
            {
                StudioData data = GetStudioData(studio.myID);
                if (data != null)
                {
                    creationYear = data.creationYear;
                    creationMonth = data.creationMonth;
                }
            }
            catch { }

            int ageInMonths = (mainScript.year - creationYear) * 12 + (mainScript.month - creationMonth);
            if (ageInMonths < 12)
            {
                int monthsLeft = 12 - ageInMonths;
                GUI_Main gui = GetGuiMain();
                if (gui != null)
                    gui.MessageBox("Created subsidiaries cannot be sold for the first 12 months.\n\nMonths remaining: " + monthsLeft, closeMenu: false);

                if (menu != null) menu.gameObject.SetActive(false);
                if (log != null) log.LogInfo("Blocked organic subsidiary sale during startup lock. Studio=" + studio.GetName() + " monthsLeft=" + monthsLeft);
                return true;
            }
        }
        catch (System.Exception ex)
        {
            if (log != null) log.LogError("Error in BlockLockedOrganicSale: " + ex);
        }
        return false;
    }

    public static void SafeCheckPublisherBuyedSize(platformScript platform, int publisherID)
    {
        if (platform == null || publisherID < 0) return;

        if (platform.publisherBuyed == null || platform.publisherBuyed.Length <= publisherID)
        {
            int newSize = Mathf.Max(publisherID + 1, platform.publisherBuyed != null ? platform.publisherBuyed.Length * 2 : 128);
            bool[] newArray = new bool[newSize];
            if (platform.publisherBuyed != null && platform.publisherBuyed.Length > 0)
                System.Array.Copy(platform.publisherBuyed, newArray, platform.publisherBuyed.Length);
            platform.publisherBuyed = newArray;
            if (log != null) log.LogInfo($"Resized platform publisherBuyed array to {newSize} to support publisherID={publisherID}");
        }
    }

    public static void AddOrganicUpgradeInvestment(publisherScript studio, long upgradeCost)
    {
        if (studio == null || !IsOrganicStudio(studio)) return;

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
}
