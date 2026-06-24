---
name: relationship-system-designer
description: Designs publisher and partner relationship systems with proper triggers, rewards, cooldowns, and duplicate protection. Use when implementing reputation mechanics, partnership perks, or any system where player actions change relationship scores and those changes must persist and not be re-triggered on reload.
---

# Relationship System Designer

Every relationship action must be structured, capped, and protected from duplication. Unguarded relationship gains are economy exploits waiting to happen.

## Every Action Must Define

| Field | Purpose |
|---|---|
| **Trigger** | What causes this relationship change |
| **Reward** | The exact delta applied |
| **Cooldown** | Minimum time or guard before it can fire again |
| **State persistence** | How this is saved across sessions |

## Every Penalty Must Define

| Field | Purpose |
|---|---|
| **Trigger** | What causes the penalty |
| **Magnitude** | Exact negative delta |
| **Duplicate protection** | HashSet or cooldown ensuring it fires once |

## Required HashSets

Declare one HashSet per reward-bearing event type. Never use a single catch-all set:

```csharp
private HashSet<int> processedReleaseGames    = new HashSet<int>();
private HashSet<int> processedHitGames        = new HashSet<int>();
private HashSet<int> processedMasterpieces    = new HashSet<int>();
private HashSet<int> processedHighSales       = new HashSet<int>();
private HashSet<int> processedContractGames   = new HashSet<int>();
private HashSet<int> processedFailures        = new HashSet<int>();
private HashSet<int> processedCancelledContracts = new HashSet<int>();
private HashSet<int> processedBoughtIPs       = new HashSet<int>();
private HashSet<int> processedGiftedIPs       = new HashSet<int>();
private HashSet<int> processedEngineLicenses  = new HashSet<int>();
private HashSet<int> processedCoDevelopments  = new HashSet<int>();
```

## Applying a Relationship Change

```csharp
void ApplyRelationshipReward(PublisherData publisher, GameData game, float delta) {
    if (publisher == null || game == null) return;
    if (processedReleaseGames.Contains(game.iD)) return;

    processedReleaseGames.Add(game.iD);
    publisher.relationship = Mathf.Min(publisher.relationship + delta, MAX_RELATIONSHIP);
}
```

Always: null-check → duplicate-check → apply → cap.

## Preventing Exploits

- **Cap relationship** at a defined maximum (`MAX_RELATIONSHIP`). Never allow `relationship += delta` without a ceiling.
- **No re-trigger on load** — serialize all processed HashSets and restore them before any events fire.
- **Cooldowns must be persisted** — a cooldown stored only in memory resets on every reload.
- **Penalties must also be guarded** — a reputation penalty that fires every time a view opens is just as broken as a duplicate reward.

## What to Prevent

- Duplicate rewards from the same game being processed twice
- Infinite relationship via repeated valid-looking triggers
- Missing penalties that let players cancel contracts consequence-free
- Relationship values going negative beyond a sensible floor
