# Studio Director

Design games for your owned developer subsidiaries from the subsidiary management screen.

## How to Use

1. Open a subsidiary's detail page (`Menu_Stats_Tochterfirma_Main`)
2. Click the **circular button** (appears above the existing action buttons) or press **Ctrl+Shift+M**
3. The game design menu opens — set genre, topic, platforms, features
4. Click **Start** — the game is assigned to the subsidiary as its current project

If the subsidiary already has a project, discard it first with the trashcan button on the subsidiary page.

## Features

### Button Injection
- Adds a circular button to the subsidiary stats page
- Automatically refreshes when switching subsidiaries
- Disabled for invalid subsidiaries (closed, locked, non-developer)
- Tooltip: "Design the next game for this subsidiary."

### Hotkey
- **Ctrl+Shift+M** opens the design menu from any open subsidiary page
- Only works when the subsidiary details screen is visible

### Game Design
- Full game design: genre, sub-genre, topics, features, platforms, game size
- Sequel, remaster, spin-off, and port menus work within the subsidiary context
- IP eligibility checks use the subsidiary's owned IPs (not the player's)

### Project Conversion
When you click Start:
- The designed game is assigned to the subsidiary (`ownerID`, `developerID`)
- The player is refunded the development cost (subsidiary pays instead)
- Subsidiary quality stats (`SetPoints`, `SetStudioAufwertungen`) are applied
- Fallback quality scaling based on subsidiary star rating if reflection fails
- Bonus detection for new genre/topic combinations
- Parallel development cleanup: removes other in-dev games for the same subsidiary

### Validation
- Sequels: only subsidiary's own IPs, not already has a sequel, not archived
- Remasters: only subsidiary's own IPs, standard/sequel/spin-off types, not already on market
- Spin-offs: only subsidiary's own IPs, main IP is self, not a sequel
- Ports: only subsidiary's own IPs, limited to 2 existing ports, standard game types

### Hotkey Shortcut
The hotkey only activates when the subsidiary details menu (`uiObjects[387]`) is actually visible, so it won't fire accidentally.

## Compatible Mods

- **Subsidiary Team Slots**: parallel development cleanup skips slot-tracked games
- **Subsidiary 2.0**: compatible (optional dependency)
- **Dynamic Subsidiary Timeline**: compatible (optional dependency)

## Technical Notes

- Uses a temporary `roomScript` to open the design menu — the room is destroyed on cancel
- Refunds player development cost to avoid double-charging (subsidiary pays during development)
- Multiplayer is disabled
