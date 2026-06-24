---
name: save-system-expert
description: Ensures mod data persists correctly across save/load cycles in BepInEx Unity mods. Use when adding new serializable fields, migrating save data from older versions, handling null-on-first-load, or debugging data loss and state desync after reloading a save file.
---

# Save System Expert

Every piece of mod state that matters must survive save/load without corruption, data loss, or re-triggering of one-shot events.

## Core Rules

- **Never store Unity object references** in save data — they become null after reload.
- **Serialize by ID**, not by reference. Store `publisher.iD`, not `publisher`.
- **Always provide safe defaults** on first load — a missing field must never crash.
- **Migrate gracefully** — old saves must load without errors even if fields were added or renamed.
- **Test on both** a fresh save and an existing save before shipping.

## The Fundamental Pattern

```csharp
// WRONG — reference becomes null after reload
[Serializable]
public class MySaveData {
    public PublisherData publisher; // null after load
}

// CORRECT — resolve by ID after load
[Serializable]
public class MySaveData {
    public int publisherId;
}

// After load, re-resolve:
var publisher = GameManager.GetPublisherById(saveData.publisherId);
```

## Serializing HashSets

`HashSet<int>` is not directly serializable. Convert to `List<int>` for save/load:

```csharp
// Saving
saveData.processedReleases = new List<int>(processedReleaseGames);

// Loading — always guard against null (field missing in old save)
processedReleaseGames = new HashSet<int>(saveData.processedReleases ?? new List<int>());
```

## Null-Safe Field Loading

Every field loaded from disk must handle being missing or null:

```csharp
// Scalar
myValue = saveData.myValue != 0 ? saveData.myValue : DEFAULT_VALUE;

// String
myString = saveData.myString ?? string.Empty;

// Collection
mySet = new HashSet<int>(saveData.myList ?? new List<int>());
```

## Save Version Field

Always include a version number for future migrations:

```csharp
[Serializable]
public class MySaveData {
    public int saveVersion = 1;
    // ... other fields
}

void OnLoad(MySaveData data) {
    if (data.saveVersion < 2) MigrateV1ToV2(data);
    if (data.saveVersion < 3) MigrateV2ToV3(data);
}
```

## Re-Resolving Unity References After Load

Never cache Unity component references across sessions. Re-resolve them in a post-load hook:

```csharp
void OnSaveLoaded() {
    // Re-resolve all cached references by ID
    cachedPublisher = GameManager.GetPublisherById(saveData.publisherId);
    cachedGame = GameManager.GetGameById(saveData.gameId);

    // Validate — remove orphan data
    if (cachedPublisher == null) ClearContractData();
}
```

## Load Safety Checklist

- [ ] Every loaded field has a safe default if null or missing
- [ ] All HashSets re-initialized before any event logic runs
- [ ] All Unity object references re-resolved by ID after load
- [ ] Save version field present and migration logic implemented
- [ ] Tested on: new game, mid-game reload, old save from previous version
- [ ] One-shot events do NOT re-fire after load
