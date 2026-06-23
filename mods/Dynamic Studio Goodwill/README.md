# Dynamic Studio Goodwill

Standalone BepInEx/Harmony mod for Mad Games Tycoon 2.

## Goal

Make studio goodwill dynamic. In the game, goodwill is the studio/company worth (`publisherScript.firmenwert`), not the 0-100 market/stars value.

## Current Behavior

The mod evaluates games twice:

- Week 4 after release: launch performance.
- Week 24 after release: long-tail performance.

It adjusts the developer studio and, if different, the publisher studio.

## Features

- Dynamic goodwill gains/losses from review and sales performance.
- Expectations scale with studio size and market/stars.
- Commercial hit bonus.
- Commercial flop penalty.
- Overhype penalty for large/high-star studios releasing weak games.
- Cult studio bonus for small/mid studios releasing high-review niche games.
- Comeback bonus for declining studios that release a strong game.
- Acquisition flavor through changed company value.
- Floors to prevent permanent death spirals.
- Optional support for player-owned subsidiaries.

## Config

Generated under `BepInEx/config/org.bepinex.plugins.dynamicstudiogoodwill.cfg`.

- `GlobalStrength`: scales all goodwill changes.
- `ShowMajorNews`: shows top-news messages for major swings. Default is `false` to avoid spam.
- `AdjustPlayerOwnedSubsidiaries`: lets subsidiaries gain/lose goodwill. Default is `true`.

## State

The mod stores lightweight trend/baseline data in:

`BepInEx/config/DynamicStudioGoodwill_State.tsv`

The game still saves the actual goodwill value normally through `publisherScript.firmenwert`.
