---
name: qa-bug-hunter
description: Finds crashes, exploits, duplicate rewards, and state desyncs by thinking like a player trying to break the game. Use before shipping any feature to audit for NullReferenceExceptions, destroyed Unity objects, memory leaks, cooldown bypasses, infinite money loops, and save corruption.
---

# QA and Bug Hunter

Think like a player actively trying to break the game. Every system has an abuse path — find it before the player does.

## What to Search For

### Crashes
- `NullReferenceException` — null Unity objects, uninitialized fields, missing references after load
- Destroyed Unity objects — accessing `.gameObject`, `.transform`, or any component after the object was destroyed
- `IndexOutOfRangeException` — lists that shrink while being iterated

### Exploits
- **Cooldown bypass** — does closing and reopening a menu reset a timer?
- **Infinite money** — can the same deal be accepted more than once?
- **Infinite relationship** — can the same game trigger a reward every time it's viewed?
- **Double rewards** — do rewards fire on load AND on trigger?

### State Issues
- **Memory leaks** — are event listeners unsubscribed on close/destroy?
- **State desync** — does the UI show stale data after a game action?
- **Ownership bugs** — can a contract belong to a publisher that no longer exists?
- **Orphan contracts** — are there active contracts with no valid game or publisher reference?

### Save/Load Issues
- Does reloading re-trigger one-shot events?
- Do HashSets reset to empty after load?
- Does the UI show correct data immediately after load?
- Do destroyed objects get re-referenced from save data?

### Specific Systems
- Subscription bugs — subscribe/unsubscribe/re-subscribe loops for windfall profit
- Console exclusive bugs — exclusive flag persisting after contract ends
- Publisher bugs — actions available after publisher relationship is severed
- Broken UI — blank panels, misaligned progress bars, buttons with no listeners

## The Five Questions

For every feature ask:

1. **Can this crash?** — find every null dereference path
2. **Can this be exploited?** — find every way to repeat a one-time action
3. **Can this duplicate?** — find every reward that fires more than once
4. **Can this desync?** — find every case where UI shows wrong state
5. **Can saves break?** — load an old save and verify nothing corrupts or re-triggers

## Test Matrix

| Scenario | What to Check |
|---|---|
| New game | All collections initialized, no stale state |
| Mid-game save → load | All HashSets restored, no duplicate rewards on load |
| Old save → load | Missing fields get safe defaults, no crash |
| Rapid UI interaction | No duplicate event listeners, no index errors |
| Publisher removed | No orphan contracts, no null publisher references |
| Max values | Relationship/money capped, no overflow |
