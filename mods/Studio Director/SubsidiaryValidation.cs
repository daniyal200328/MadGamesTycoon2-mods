using System;
using System.Collections;
using System.Reflection;

partial class StudioDirectorPlugin
{
    internal static bool IsValidSubsidiary(publisherScript subsidiary)
    {
        if (subsidiary == null || !subsidiary) return false;
        try
        {
            return subsidiary.isUnlocked &&
                   subsidiary.developer &&
                   subsidiary.IsMyTochterfirma() &&
                   !subsidiary.Geschlossen() &&
                   !subsidiary.TochterfirmaGeschlossen();
        }
        catch
        {
            return false;
        }
    }

    public static gameScript FindActiveSubsidiaryProject(publisherScript subsidiary)
    {
        if (subsidiary == null) return null;

        gameScript game = subsidiary.FindGameInDevelopment();
        if (game != null) return game;

        games games = FindGames();
        if (games == null || games.arrayGamesScripts == null) return null;

        for (int i = 0; i < games.arrayGamesScripts.Length; i++)
        {
            gameScript candidate = games.arrayGamesScripts[i];
            if (candidate != null && candidate.inDevelopment &&
                !candidate.isOnMarket && candidate.developerID == subsidiary.myID)
                return candidate;
        }

        return null;
    }

    internal static bool IsGameTrackedInTeamSlots(int gameId)
    {
        try
        {
            Type slotsType = Type.GetType("SubsidiaryTeamSlotsPlugin, SubsidiaryTeamSlots");
            if (slotsType == null)
            {
                foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    slotsType = asm.GetType("SubsidiaryTeamSlotsPlugin");
                    if (slotsType != null) break;
                }
            }

            if (slotsType == null) return false;

            var stateField = slotsType.GetField("State", BindingFlags.Public | BindingFlags.Static);
            if (stateField == null) return false;

            object state = stateField.GetValue(null);
            if (state == null) return false;

            var studiosField = state.GetType().GetField("studios", BindingFlags.Public | BindingFlags.Instance);
            if (studiosField == null) return false;

            IEnumerable studios = studiosField.GetValue(state) as IEnumerable;
            if (studios == null) return false;

            foreach (object studio in studios)
            {
                var slotsField = studio.GetType().GetField("slots", BindingFlags.Public | BindingFlags.Instance);
                if (slotsField == null) continue;

                Array slots = slotsField.GetValue(studio) as Array;
                if (slots == null) continue;

                for (int i = 0; i < slots.Length; i++)
                {
                    object slot = slots.GetValue(i);
                    if (slot == null) continue;

                    var gameIdField = slot.GetType().GetField("gameID", BindingFlags.Public | BindingFlags.Instance);
                    if (gameIdField == null) continue;

                    int slotGameId = (int)gameIdField.GetValue(slot);
                    if (slotGameId == gameId) return true;
                }
            }
        }
        catch (Exception ex)
        {
            Log?.LogWarning($"Error in IsGameTrackedInTeamSlots: {ex}");
        }

        return false;
    }

    internal static bool CheckNachfolgerForSubsidiary(gameScript script)
    {
        int ownerId = GetActiveSubsidiaryId();
        if (ownerId == -1 || script == null) return false;

        return script.ownerID == ownerId &&
               !script.inDevelopment &&
               !script.schublade &&
               !script.typ_budget &&
               !script.typ_goty &&
               script.portID == -1 &&
               !script.auftragsspiel &&
               !script.typ_bundle &&
               !script.typ_bundleAddon &&
               !script.f2pConverted &&
               (script.typ_standard || script.typ_nachfolger || script.typ_spinoff) &&
               (!script.pubOffer || script.ownerID == ownerId) &&
               !script.nachfolger_created &&
               !script.GetIpIsInArchiv();
    }

    internal static bool CheckRemasterForSubsidiary(gameScript script)
    {
        int ownerId = GetActiveSubsidiaryId();
        if (ownerId == -1 || script == null) return false;

        return script.ownerID == ownerId &&
               !script.inDevelopment &&
               !script.schublade &&
               script.gameTyp == 0 &&
               script.portID == -1 &&
               !script.auftragsspiel &&
               (script.typ_standard || script.typ_nachfolger || script.typ_spinoff) &&
               !script.remaster_created &&
               !script.isOnMarket &&
               !script.typ_budget &&
               !script.typ_goty &&
               !script.typ_remaster &&
               !script.typ_mmoaddon &&
               !script.typ_bundleAddon &&
               !script.typ_bundle &&
               (!script.pubOffer || script.ownerID == ownerId) &&
               !script.GetIpIsInArchiv();
    }

    internal static bool CheckSpinoffForSubsidiary(gameScript script)
    {
        int ownerId = GetActiveSubsidiaryId();
        if (ownerId == -1 || script == null) return false;

        return script.ownerID == ownerId &&
               !script.inDevelopment &&
               script.mainIP == script.myID &&
               (!script.pubOffer || script.ownerID == ownerId) &&
               !script.typ_nachfolger &&
               !script.auftragsspiel &&
               !script.GetIpIsInArchiv();
    }

    internal static bool CheckPortForSubsidiary(gameScript script)
    {
        int ownerId = GetActiveSubsidiaryId();
        if (ownerId == -1 || script == null || script.ownerID != ownerId || script.portID != -1)
            return false;

        int existingPorts = 0;
        for (int i = 0; i < script.portExist.Length; i++)
        {
            if (script.portExist[i]) existingPorts++;
        }

        mainScript main = FindMain();
        if (main != null && !main.unlock_.Get(65)) existingPorts++;
        if (script.gameTyp == 1 || script.gameTyp == 2) existingPorts++;

        return existingPorts < 2 &&
               !script.typ_contractGame &&
               !script.typ_addon &&
               !script.typ_addonStandalone &&
               !script.typ_mmoaddon &&
               !script.typ_bundle &&
               !script.typ_budget &&
               !script.typ_bundleAddon &&
               !script.typ_goty &&
               !script.auftragsspiel &&
               !script.f2pConverted &&
               (!script.pubOffer || script.ownerID == ownerId) &&
               !script.GetIpIsInArchiv();
    }
}
