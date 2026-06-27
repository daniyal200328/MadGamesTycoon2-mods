using UnityEngine;

public partial class SubsidiaryTeamsPlugin
{
    internal static bool showRoleWindow = false;
    internal static Rect roleRect = new Rect(Screen.width / 2f - 150f, Screen.height / 2f - 150f, 300f, 320f);
    internal static int roleSlotIndex = -1;
    internal static int roleStudioID = -1;

    private void DrawRoleWindow(int windowID)
    {
        GUILayout.Space(10);
        GUILayout.Label("Select Specialty Role:", GUI.skin.label);
        GUILayout.Space(10);

        StudioSlotData data = GetStudioSlotData(roleStudioID);

        for (int r = 0; r < RoleDisplayNames.Length; r++)
        {
            bool isCurrent = false;
            if (data != null && roleSlotIndex >= 0 && roleSlotIndex < 3)
                isCurrent = (data.slots[roleSlotIndex].role == r);

            string label = GetRoleDisplayName(r);
            if (isCurrent) label += " (Active)";

            if (GUILayout.Button(label, GUILayout.Height(28f)))
            {
                StudioSlotData d = GetStudioSlotData(roleStudioID);
                if (d != null && roleSlotIndex >= 0 && roleSlotIndex < 3)
                {
                    d.slots[roleSlotIndex].role = r;
                    d.slots[roleSlotIndex].helpingSlotIndex = -1;
                    showRoleWindow = false;
                    SaveState(currentSaveSlot);
                    lastBuiltForStudio = -1;
                    Menu_Stats_Tochterfirma_Main menu = Object.FindObjectOfType<Menu_Stats_Tochterfirma_Main>();
                    if (menu != null && menu.gameObject.activeInHierarchy) menu.UpdateData();
                }
            }
        }

        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Cancel")) showRoleWindow = false;
        GUI.DragWindow();
    }
}
