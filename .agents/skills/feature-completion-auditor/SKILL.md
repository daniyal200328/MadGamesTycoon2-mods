---
name: feature-completion-auditor
description: Audits features against a checklist of implementation status, save-safety, null-safety, exploit resistance, UI completeness, and vanilla compatibility. Use before shipping any feature or when reviewing project completeness against a design document.
---

# Feature Completion Auditor

Before any feature is considered done, run it through this audit. Partial implementations shipped as complete are the most common source of bugs.

## Per-Feature Audit Checklist

For every feature, answer each of the following:

| Check | Status |
|---|---|
| **IMPLEMENTED?** | FULL / PARTIAL / MISSING |
| **SAVE SAFE?** | YES / NO |
| **NULL SAFE?** | YES / NO |
| **EXPLOIT SAFE?** | YES / NO |
| **UI COMPLETE?** | YES / NO |
| **TESTED?** | YES / NO |
| **VANILLA COMPATIBLE?** | YES / NO |
| **STATE PERSISTENT?** | YES / NO |

A feature is not shippable unless every row is YES or FULL.

## What Each Check Means

**IMPLEMENTED** — Core logic exists and runs without errors under normal conditions.

**SAVE SAFE** — All relevant state is serialized and correctly restored after a save/load cycle. Test with a real save file.

**NULL SAFE** — Every reference is null-checked before access. No NullReferenceException is possible on missing data, first launch, or corrupted saves.

**EXPLOIT SAFE** — No player action (repeated clicks, menu tricks, reload abuse, timing exploits) can duplicate rewards or bypass costs.

**UI COMPLETE** — The feature has all required UI elements and they update correctly on open, refresh, and data change.

**TESTED** — Manually tested on: new game, mid-game load, edge cases (empty lists, max values, missing references).

**VANILLA COMPATIBLE** — The mod does not break unmodded save files, and unpatched game methods still work correctly.

**STATE PERSISTENT** — Any state the player can create (contracts, relationships, tracked events) survives across sessions.

## When Partially Implemented

Log it explicitly:

```
Feature: Console Exclusive Contracts
IMPLEMENTED: PARTIAL — logic exists, rewards not wired
SAVE SAFE: NO — contract state not serialized yet
NULL SAFE: YES
EXPLOIT SAFE: NO — no duplicate guard yet
UI COMPLETE: NO — panel exists, buttons not wired
TESTED: NO
VANILLA COMPATIBLE: YES
STATE PERSISTENT: NO
```

## Goal

Bring every feature to full parity with the design document. No feature ships with a single NO.
