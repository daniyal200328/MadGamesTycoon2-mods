# Debug Session: subsidiary-details-crash

## 📋 Session Metadata
- **Date**: 2026-06-26
- **Status**: [CLOSED]
- **Bug Report**: Game crashes when clicking subsidiary details in subsidiary menu
- **Mods Involved**: Subsidiary Teams, Dynamic Subsidiary Timeline
- **Last Action**: Resolved NullReferenceException in SafeFindScripts and SafeFindScriptsSettings

## 🔬 Hypotheses & Findings
1. **Hypothesis**: Subsidiary Team Slots' `EnsureSidePanel` method is accessing a null/non-existent UI element.
   * **Finding**: The side panel setup code was correct, but it depended on the `Menu_Stats_Tochterfirma_Main` initializing successfully.
2. **Hypothesis**: Dynamic Subsidiary Timeline's prefix patches caused a crash.
   * **Finding**: **TRUE**. In `Dynamic Subsidiary Timeline`'s `SafeFindScripts` and `SafeFindScriptsSettings` (within `Patches_MainUI.cs` and `Patches_SettingsAndUpgrade.cs`), the reflection lookup code was looking up `GUI_Main` and `sfxScript` on the main `"Main"` GameObject instead of their respective `"CanvasInGameMenu"` and `"SFX"` GameObjects.
   * This returned `null`, which caused `NullReferenceException` when `UpdateData()` ran (e.g. `guiMain_.DrawStarsColor(...)` failed).
   * Furthermore, `SafeFindScripts` was erroneously resetting `menuPSField` (the `publisherScript` instance `pS_`) to `null` every time it was called.

## 🔧 Fix History
- Corrected the target GameObjects for `GUI_Main` (now `"CanvasInGameMenu"`) and `sfxScript` (now `"SFX"`) in `SafeFindScripts` and `SafeFindScriptsSettings`.
- Removed the line setting `menuPSField` to `null` inside `SafeFindScripts` to prevent clearing the publisher script instance.
- Re-compiled all plugins to ensure full compatibility.
