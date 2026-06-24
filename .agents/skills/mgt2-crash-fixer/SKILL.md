---
name: mgt2-crash-fixer
description: Fix Mad Games Tycoon 2 BepInEx/Harmony mod crashes, especially UI menu/list/detail crashes, publisher/developer/subsidiary stats crashes, organic subsidiary crashes, array index issues, patch conflicts, and FPS drops caused by unsafe Update hooks or logging.
---

# MGT2 Crash Fixer

Use this skill when a Mad Games Tycoon 2 mod crashes, freezes, loses FPS, or breaks a UI menu.

## Crash Workflow

1. Read `BepInEx/LogOutput.log` first. Tail enough lines to include the last menu click and any Harmony or Unity exception.
2. If the BepInEx log ends without an exception, assume the crash may happen before flushing. Add temporary breadcrumbs only around the suspected click/init path, then remove them before shipping.
3. Read the matching vanilla source in `GameSourceCode/` before patching. For UI crashes, inspect the exact menu, row item, button method, `Init`, `Update`, `SetData`, `DROPDOWN_Sort`, and close methods.
4. Search every mod for the same target method before adding a Harmony patch. Multiple prefixes that skip vanilla on the same UI lifecycle method can crash or leave fields half-initialized.
5. Prefer removing an overlapping patch over adding another shield when another dedicated mod already owns that menu.

## Known MGT2 UI Crash Patterns

- Vanilla stats UI often dereferences `uiObjects[index]`, `GetComponent<T>()`, `pS_`, `mS_`, `tS_`, `guiMain_`, `genres_`, and `nextGame_` without checks.
- Modded publishers or organic subsidiaries can have IDs larger than vanilla arrays expect. Avoid direct array access by publisher ID unless the array is resized and bounds-checked.
- `publisherScript.GetFirmenwertString()`, `GetFirmenwert()`, `GetLogo()`, `GetName()`, `GetTooltip()`, `GetDeveloperPublisherString()`, `FindGameInDevelopment()`, and `FindAngekuendigtesGame()` can crash when modded studio data is incomplete.
- Detail menus are more fragile than list menus because `Init()` often calls multiple getters immediately after `ActivateMenu`.
- Subsidiary detail screens are commonly patched by subsidiary mods. Do not patch `Menu_Stats_Tochterfirma_Main` or `Item_Stats_Tochterfirma` from an unrelated publisher/developer mod unless the crash is proven to originate there.

## Safe UI Patch Rules

- Null-check the menu, row item, backing studio, `uiObjects`, index bounds, and component before every UI write.
- Use safe fallbacks for display values: raw `name_EN`, estimated firm value, blank sprites, empty strings, and disabled buttons.
- Rebuild dynamic lists on open. Clear old children before adding new rows.
- Do not force a scrollbar value inside a menu `Update`; it locks scrolling.
- For list rows, disable the row button if the backing studio is missing.
- For click handlers, verify `GUI_Main.uiObjects[index]` exists before `ActivateMenu`.
- Do not cache UI objects across open/close cycles. Cache managers only, and clear static Unity object caches on destroy.

## Harmony Patch Rules

- Use the smallest patch that solves the crash. A button prefix or row `SetData` prefix is usually safer than a broad publisher getter finalizer.
- Avoid broad finalizers on core getters. They can mask real errors and add overhead everywhere.
- Avoid hot-path patches. If patching `Update`, the prefix must be near-zero cost and must not allocate.
- Never use `GameObject.Find`, `FindObjectOfType`, LINQ, string interpolation, or logging in a hot path.
- If a dedicated mod already patches a lifecycle method with a safe prefix, do not add another skip-prefix in a different mod.

## Logging Rules

- Temporary `LogInfo` breadcrumbs are allowed while isolating a crash.
- Remove breadcrumbs before compiling the user build.
- Keep only actionable `LogWarning` or `LogError` entries for genuine failed operations.
- Do not log fallback behavior inside row rendering or `Update`; a bad studio can spam the log every frame or every row.

## Regression Matrix

After fixing, compile and test:

- Open Publisher list from `Publishers & Developers`.
- Open Developer list from `Publishers & Developers`.
- Scroll both lists.
- Click a vanilla publisher.
- Click a vanilla developer.
- Click a publisher/developer that is also a subsidiary.
- Open the Subsidiaries page.
- Open an existing subsidiary detail screen.
- Create an organic/new subsidiary and open its detail screen.
- Verify FPS remains in the normal range after leaving the menu open for at least 30 seconds.

## Shipping Checklist

- No unintended patches remain on unrelated menus.
- No temporary `LogInfo` breadcrumbs remain.
- No new per-frame manager lookups remain.
- No list sort calls fragile studio getters.
- The mod compiles and the produced DLL is copied into `BepInEx/plugins/`.
