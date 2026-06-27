# Dynamic Subsidiary Timeline

Replaces the fixed development timeline for subsidiary games with a dynamic formula based on studio stats, game type, platform count, and market trends.

## How It Works

When a subsidiary starts a new game, the development duration is calculated from:
- **Game size** (B–AAAA) → configurable midpoint/floor/ceiling per tier
- **Studio star rating** → fewer stars = longer dev time
- **Development speed stat** → higher speed = shorter dev time
- **Platform count** → +10% per extra platform
- **Project type** → ports/addons are fast, new IPs/MMOs are slow
- **Market trend** (if Dynamic Studio Goodwill is installed) → crisis/decline/stable/rising/powerhouse
- **Random variance** → ±7% (configurable)
- **Multi-team penalty** (if Subsidiary Teams is installed) → +X% per concurrent project

## Controls

- **Ctrl+Shift+T** opens the in-game config window
- Tune midpoint, floor, ceiling weeks per size tier
- Adjust inflation, upkeep cap, dev duration multipliers, multi-team penalty

## Features

### Timeline Calculation
- 6 game sizes (B, B+, A, AA, AAA, AAAA) each with configurable midpoint, floor, ceiling
- Star multiplier: gap to 5 stars × weight per tier
- Speed multiplier: linear scaling (organic vs acquired have different curves)
- Platform multiplier: +10% per platform beyond the first
- Project type table: bundles (0.10×) → standard new IP (1.15×) → MMO (1.55×)
- Trend integration: reads `studioTrends` from Dynamic Studio Goodwill mod

### Dynamic Upkeep Costs
- Monthly administration costs scale with studio value, year, difficulty, star rating, speed, and game size
- Idle studios pay less (configurable multiplier)
- Yearly inflation compounds costs over time
- Complexity factor increases with unlocked team slots (Subsidiary Teams integration)

### Upgrade Recalculation
When a subsidiary is upgraded (star/speed increase), active project timelines are recalculated preserving progress fraction — no abrupt finish or wasted overhang.

### Slot Awareness
Fully integrates with Subsidiary Teams:
- Reads slot-specific star/speed stats for each team slot
- Tracks per-slot remaining/total weeks
- Applies acceleration factor for helper slots
- Recalculates dev points proportionally on timeline changes

### Settings
- Per-subsidiary settings: dev duration preference, game size/genre filters, auto-release thresholds, IP/platform focus, engine lock, own-publisher priority
- Toggle auto-release by review threshold (10%–90%)
- Platform/console exclusivity enforcement
- MMO/F2P/addon allowance per studio

## Config Options (Ctrl+Shift+T)

| Section | Key | Default | Description |
|---------|-----|---------|-------------|
| General | EnableDynamicTimeline | true | Master toggle |
| General | ApplyToOrganicStudios | true | Apply to player-created studios |
| General | ApplyToAcquiredStudios | true | Apply to bought studios |
| General | TrendIntegrationEnabled | true | Read Dynamic Studio Goodwill trends |
| General | RandomVarianceEnabled | true | ±7% variance after floor/ceiling |
| Debug | LogCalculations | false | Log every calc to BepInEx console |
| Costs | EnableDynamicCosts | true | Dynamic monthly admin costs |
| Costs | IdleCostMultiplier | 0.5 | Cost multiplier when idle |
| Costs | AAAAMultiplier | 5.0 | Max tier cost multiplier |
| Costs | MaxUpkeepCap | 17500000 | Monthly cost cap |
| Costs | BaseInflationRate | 2.0 | Yearly inflation % |
| MultiTeamPenalty | Enabled | true | Penalty for concurrent projects |
| MultiTeamPenalty | PercentPerExtraProject | 18 | % per extra active project |
| DevDuration | ShortMultiplier | 0.65 | Duration multiplier (short setting) |
| DevDuration | BalancedMultiplier | 0.85 | Duration multiplier (balanced) |
| DevDuration | GenerousMultiplier | 1.00 | Duration multiplier (generous) |
| Timelines | B/B+/A/AA/AAA/AAAA | — | Midpoint, floor, ceiling per tier |

## Compatible Mods

- **Dynamic Studio Goodwill** — trend state integration
- **Subsidiary Teams** — multi-team slots, per-slot stats, helper acceleration
- **Organic Subsidiaries** — organic studio sale value for upkeep calculations
- **Studio Director** — compatible (separate systems)

## Technical Notes

- Replaces `SetAsGameInDevelopmentNPC`, `SetNewGameInWeeks`, and `CreateNewGame2` via Harmony prefixes
- Patches `Menu_Stats_Tochterfirma_Main` and `Item_Stats_Tochterfirma` for dynamic data display
- Patches `Menu_Stats_TochterfirmaSettings` for per-studio configuration
- Soft-dependency on Subsidiary Teams via reflection probes and direct (optional) assembly reference
- All config persisted via BepInEx `Config.Save()`
