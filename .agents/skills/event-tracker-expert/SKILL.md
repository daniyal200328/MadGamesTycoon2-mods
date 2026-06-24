---
name: event-tracker-expert
description: Prevents duplicate rewards and one-shot event bugs using HashSet tracking. Use whenever implementing game events — releases, milestones, contract completions, IP transactions — that must fire exactly once per object and survive save/load without re-triggering.
---

# Event Tracking Specialist

Any reward or penalty that can fire more than once is a bug waiting to be exploited. Use `HashSet<int>` to guarantee one-shot behavior for every significant game event.

## Core Pattern

```csharp
private HashSet<int> processedReleaseGames = new HashSet<int>();

void OnGameReleased(GameData game) {
    if (processedReleaseGames.Contains(game.iD)) return;
    processedReleaseGames.Add(game.iD);
    // Apply reward exactly once
}
```

## Events to Track

Every one of these can duplicate if unguarded:

- **Releases** — game launch rewards
- **Hits** — bonus on reaching review threshold
- **Masterpieces** — top-score rewards
- **High sales milestones** — sales-count bonuses
- **Exclusive games** — console exclusivity bonuses
- **Contract games** — delivery rewards
- **Commissions** — payment on completion
- **Failures** — reputation penalties
- **Cancelled contracts** — penalty on publisher cancellation
- **IP purchases** — one-time acquisition bonuses
- **IP gifts** — received IP bonuses
- **Engine licenses** — licensing fee events
- **Co-development projects** — collaboration rewards

## Save/Load Safety

HashSets must be serialized and restored, or they lose all tracking after a load:

```csharp
// Saving — convert to List for JSON
saveData.processedReleases = new List<int>(processedReleaseGames);

// Loading — restore from List, guard against null
processedReleaseGames = new HashSet<int>(saveData.processedReleases ?? new List<int>());
```

## Rules

- **Events fire once.** Never remove an ID from a processed set after adding it.
- **No double rewards.** Check the set before any reward or penalty call.
- **Persist when necessary.** If the event should survive save/load, serialize the HashSet.
- **Clear stale entries only intentionally.** Clearing a set mid-game re-arms all those events — only do this on new game start.

## Naming Convention

Name your HashSets after what they protect:

```
processedReleaseGames
processedHitGames
processedMasterpieces
processedHighSales
processedContractGames
processedCommissions
processedFailures
processedCancelledContracts
processedBoughtIPs
processedGiftedIPs
processedEngineLicenses
processedCoDevelopments
```
