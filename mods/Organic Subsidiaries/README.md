# Organic Subsidiaries (formerly Subsidiary 2.0)

A BepInEx mod for Mad Games Tycoon 2 that creates and manages **organic subsidiary studios** — procedurally-generated NPC developers/publishers that act as your subsidiaries without manual creation.

---

## Features

### Organic Studio Lifecycle
- Studios appear as NPC publishers with IDs in the `9000-9999` or `90000-99999` range
- Automatically detected as subsidiaries via their ID range
- Custom tooltips showing name, type, country, date, stars, games count, and financials
- Custom logos and naming
- 12-month sale lock after creation

### Creation Window
- Press **Ctrl+Shift+S** in-game to open the creation window
- Configure: name, market experience (1-5 stars), development speed (1-10), country, logo
- Optional PNG logo import via Windows file dialog
- Dynamic cost calculation based on market tier and speed

### Sale Valuation Engine (`Economics.cs`)
```
Sale Value = 60% creation cost + 70% upgrade investments + IP portfolio value + 50% recent revenue
```
- **IP Valuation**: Sums market value of all released IPs owned by the studio
- **No-Game Cap**: If the studio released 0 games, value is capped at 60% of creation cost
- **Goodwill Integration**: If DynamicStudioGoodwill is present, value is adjusted by trend:
  - Rising: +15%
  - Commercial Powerhouse: +30%
  - Declining/In Crisis: -20%
- Custom sale value displayed in the sell window, subsidiary list, and detail panel

### Upgrade Tracking
- Tracks all upgrade investments (money spent on upgrading stars/speed)
- 70% of upgrade costs are recouped in sale value
- Integrates with DynamicSubsidiaryTimeline to recalculate project timelines after upgrades

### Marketing & Trade Show Integration
- Subsidiary games appear in trade show (Messe) game selection
- Subsidiary games appear in marketing campaign game selection
- Subsidiary games appear in special marketing game selection
- Auto-selects a subsidiary game if no other game is found for marketing campaigns

### Compatibility & Safety
- Prefix patches on `tooltip.Start`, `setFont.OnEnable`, `setText.OnEnable` to prevent null-reference crashes on save load
- Array bounds safety for `platformScript.publisherBuyed` when NPCs buy platforms
- Frame-based caching for publisher and game object lookups

---

## File Structure

| File | Purpose |
|------|---------|
| `Plugin_Core.cs` | BepInEx plugin lifecycle, creation window GUI, cost calculation, logo import, subsidiary creation |
| `ModState.cs` | `StudioData` and `ModState` serializable classes, JSON save/load to disk (`SubsidiaryData.json`) |
| `OrganicStudio.cs` | Organic studio detection (`IsOrganicStudio`), ID allocation, default settings, sale lock, upgrade tracking |
| `Economics.cs` | Sale valuation engine, IP valuation, revenue tracking, reflection bridges to DynamicSubsidiaryTimeline and DynamicStudioGoodwill |
| `Patches_Core.cs` | Identity patches (`IsMyTochterfirma`, `GetName`, `GetLogo`, `GetTooltip`, `GetDeveloperPublisherString`) + null-safety patches (`tooltip`, `setFont`, `setText`) + array safety patches |
| `Patches_UI.cs` | Sell window, stats menu, upgrade button, marketing/trade show game selection patches + frame-based caching utilities |

---

## Compatibility

- Requires **BepInEx 5.x**
- Compatible with **Studio Director**, **Dynamic Studio Goodwill**, **Dynamic Subsidiary Timeline**, **Custom Genres**, **Custom Themes**
- Saves data to `SubsidiaryData.json` next to the plugin DLL — safe across game updates
- Uses Harmony `PatchAll` — applied on first gameplay load (when `officeLoaded` is true)

---

## Known Limitations

- Logo import uses Windows-only P/Invoke (`comdlg32.dll`) — will be non-functional on Linux/Steam Deck
- Sale value uses reflection to bridge to DynamicSubsidiaryTimeline and DynamicStudioGoodwill — no hard dependency
- Static caches (`_cachedPublishers`, `_cachedGameObjects`) refresh per frame — does not leak across scene loads
