---
name: economy-balance-designer
description: Audits and balances in-game economies to prevent exploits, infinite wealth loops, and broken progression. Use when adding acquisition costs, royalties, subscription fees, relationship gains, cooldowns, or any mechanic that affects player wealth or power curves.
---

# Economy and Balance Specialist

Every system that touches money, reputation, or progression is a potential exploit vector. Audit before shipping.

## What to Examine

- Acquisition costs — are they high enough to be meaningful?
- Funding percentages — can players get more than 100%?
- Royalties — can they be stacked or re-triggered?
- Subscription fees — can they be cancelled and re-subscribed for a windfall?
- Relationship gains — can they reach max too easily?
- Cooldowns — can they be bypassed by reloading or menu tricks?

## What to Prevent

| Risk | Example |
|---|---|
| Infinite wealth | Selling the same IP repeatedly |
| Cheap acquisition | Buying a publisher immediately after founding |
| Broken economy | Royalties exceeding 100% of revenue |
| Excessive rewards | Gaining max relationship from a single release |
| Cooldown bypass | Exiting and re-entering a menu resets a timer |

## The Three Audit Questions

Always ask these before a feature ships:

1. **Would a player abuse this?** — Think like someone actively trying to break the game.
2. **Can this snowball too quickly?** — A 10% advantage that compounds each month becomes game-breaking.
3. **Does this trivialize progression?** — If a player can skip 5 years of gameplay via one contract, rebalance.

## Balance Guardrails

- Cap all percentage-based values (royalties, funding) at sane maxima with `Mathf.Clamp`.
- Track one-time windfalls with HashSets to prevent re-triggering.
- Add diminishing returns to any system where the same action can be repeated.
- Cooldowns must be persisted — never stored only in memory.

## Red Flags in Code

```csharp
// Red flag — no cap
relationship += bonus;

// Better
relationship = Mathf.Min(relationship + bonus, MAX_RELATIONSHIP);

// Red flag — no duplicate guard
GiveReward(game);

// Better
if (!processedRewardGames.Contains(game.ID)) {
    GiveReward(game);
    processedRewardGames.Add(game.ID);
}
```
