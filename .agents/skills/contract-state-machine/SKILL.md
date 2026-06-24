---
name: contract-state-machine
description: Designs and enforces state-driven contract systems for game mods. Use when implementing publisher deals, IP licensing, console exclusives, or any contract that must persist across saves, transition cleanly between states, and never produce orphan or duplicate entries.
---

# Contract State Machine Architect

Every contract must be fully state-driven. Never rely on magic values or raw integers to represent contract types — they rot over time and become impossible to debug.

## Bad vs Good

Bad:
```csharp
if (royalty == 25)
```

Good:
```csharp
if (contract.Type == ContractType.IPCooperationHigh)
```

## Contract Type Enum

Always represent contract types as an enum:

```csharp
public enum ContractType
{
    None,
    MarketingDeal,
    PremiumDeal,
    IPCooperationLow,
    IPCooperationHigh,
    AAAPlayerIP,
    AAATheirIP,
    AAANewIP,
    ConsoleExclusive,
    AutoPublish,
    SubscriptionDeal,
    Outsource
}
```

## State Transition Rules

- All transitions must be **explicit** — never infer state from surrounding conditions.
- Every state must define: entry action, exit action, and valid next states.
- Expired or cancelled contracts must clean themselves up immediately — never leave stale entries.
- Orphan contracts (no valid owner or game reference) must be detected and purged on load.

## Save Safety

- Serialize contract state as enum names or int IDs, never as Unity object references.
- On load, re-validate all active contracts — remove any whose referenced objects no longer exist.
- Add a `contractVersion` field to support future migrations.

## Checklist Before Adding a New Contract Type

- [ ] Added to the `ContractType` enum
- [ ] Has defined entry and exit behavior
- [ ] Cleaned up on expiry or cancellation
- [ ] Survives save/load without data loss
- [ ] Cannot be duplicated by re-triggering
- [ ] UI correctly reflects its state
