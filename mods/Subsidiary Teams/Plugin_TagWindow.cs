using UnityEngine;

public partial class SubsidiaryTeamsPlugin
{
    internal static bool showTagWindow = false;
    internal static Rect tagWindowRect = new Rect(Screen.width / 2f - 300f, Screen.height / 2f - 200f, 600f, 400f);
    internal static int tagWindowStudioID = -1;
    internal static int tagWindowSlotIdx = -1;
    internal static int[] tagWindowOptions = new int[3];

    private void DrawTagWindow(int windowID)
    {
        GUILayout.Space(12);
        GUILayout.Label("This team has reached ★3 and unlocked a Team Tag!", GUI.skin.label);
        GUILayout.Label("Choose one tag permanently. Its effects apply to all future releases.", GUI.skin.label);
        GUILayout.Space(12);

        GUILayout.BeginHorizontal();

        for (int opt = 0; opt < 3; opt++)
        {
            int tid = tagWindowOptions[opt];
            TagDefinition tag = GetTagByID(tid);

            GUILayout.BeginVertical("box", GUILayout.Width(180f), GUILayout.Height(270f));
            GUILayout.Space(4);

            GUILayout.Label($"<b>{tag.name}</b>", GUI.skin.label);
            GUILayout.Space(6);

            GUILayout.Label($"<color=#00ff00>✓ {tag.pro}</color>", GUI.skin.label);
            GUILayout.Space(4);

            GUILayout.Label($"<color=#ff4444>✗ {tag.con}</color>", GUI.skin.label);
            GUILayout.Space(4);

            GUILayout.Label($"<i><color=grey>{tag.flavor}</color></i>", GUI.skin.label);
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Select", GUILayout.Height(30f)))
            {
                StudioSlotData data = GetStudioSlotData(tagWindowStudioID);
                if (data != null && tagWindowSlotIdx >= 0 && tagWindowSlotIdx < 3)
                {
                    data.slots[tagWindowSlotIdx].traitID = tid;
                    showTagWindow = false;
                    SaveState(currentSaveSlot);
                    lastBuiltForStudio = -1;
                    Menu_Stats_Tochterfirma_Main menu = Object.FindObjectOfType<Menu_Stats_Tochterfirma_Main>();
                    if (menu != null && menu.gameObject.activeInHierarchy) menu.UpdateData();
                }
            }
            GUILayout.EndVertical();
        }

        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Roll Later")) showTagWindow = false;
        GUI.DragWindow();
    }
}
