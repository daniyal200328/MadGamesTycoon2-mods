# Debug Session: subsidiary-details-crash

## 📋 Session Metadata
- **Date**: 2026-06-24
- **Status**: [OPEN]
- **Bug Report**: Game crashes when clicking subsidiary details in subsidiary menu
- **Mods Involved**: Subsidiary 2.0, Subsidiary Team Slots, Subsidiary Project Director, Dynamic Subsidiary Timeline
- **Last Action**: Initialized debug session

## 🔬 Hypotheses
1. Subsidiary Team Slots' `EnsureSidePanel` method is accessing a null/non-existent UI element
2. Subsidiary Project Director's `InjectButton` method is failing
3. Subsidiary 2.0's reflection fields are null or not found
4. `uiObjects` array access in any mod is going out of bounds despite checks
5. Missing null checks on `publisherScript` instance in menu patches

## 📊 Log Summary

### Pre-Fix Logs
- Waiting for reproduction...

### Post-Fix Logs
- Not yet available

## 🔧 Fix History
- None yet
