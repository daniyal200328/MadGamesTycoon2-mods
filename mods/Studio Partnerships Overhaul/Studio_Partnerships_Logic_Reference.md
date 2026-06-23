# Studio Partnerships Overhaul — Complete Logic Reference

> **How to use this document:** Every NPC studio in the game is classified into one of six "subdivisions." Your relationship with each studio is tracked from 0–100 points (shown as 1–5 stars, where each star = 20 points). As your relationship grows, new perks and deals unlock. This document explains the exact classification logic, every action that raises or lowers relationship, and every perk unlocked at each star tier — with the precise numbers, cooldowns, and internal mechanics as implemented in the mod code.

---

## Table of Contents

1. [Studio Classification System](#1-studio-classification-system)
2. [Relationship System Fundamentals](#2-relationship-system-fundamentals)
3. [Subdivision 1: Platform Holders](#3-subdivision-1-platform-holders)
4. [Subdivision 2: Big Publishers](#4-subdivision-2-big-publishers)
5. [Subdivision 3: Small Publishers](#5-subdivision-3-small-publishers)
6. [Subdivision 4: Big Developers](#6-subdivision-4-big-developers)
7. [Subdivision 5: Mid-Sized Developers](#7-subdivision-5-mid-sized-developers)
8. [Subdivision 6: Indie Developers](#8-subdivision-6-indie-developers)
9. [Cross-Cutting Mechanics](#9-cross-cutting-mechanics)
10. [Partnerships Overview Panel (Ctrl+Shift+P)](#10-partnerships-overview-panel-ctrlshiftp)

---

## 1. Studio Classification System

Every NPC studio (`publisherScript`) is classified at game start based on its properties. The classification is **static** — it does not change during the playthrough (dynamic NPC star growth is on hold).

### Classification Decision Tree

```
Is the studio a developer? (developer == true)
├── YES → Is the studio also a publisher? (publisher == true)
│   ├── YES → Does the studio own a platform/console? (ownPlatform == true)
│   │   ├── YES → Subdivision 1: Platform Holder
│   │   └── NO  → What are the studio's stars?
│   │       ├── stars >= 70 → Subdivision 2: Big Publisher
│   │       └── stars < 70  → Subdivision 3: Small Publisher
│   └── NO  → (Not a developer — error/inactive)
└── NO  → Is the studio a publisher but NOT a developer? (publisher == true, developer == false)
    └── YES → Does the studio own a platform? (ownPlatform == true)
        ├── YES → Subdivision 1: Platform Holder
        └── NO  → What are the studio's stars?
            ├── stars >= 70 → Subdivision 2: Big Publisher
            └── stars < 70  → Subdivision 3: Small Publisher

If developer == true && publisher == false (pure developer):
    ├── stars >= 75 → Subdivision 4: Big Developer
    ├── stars 40–74 → Subdivision 5: Mid-Sized Developer
    └── stars < 40  → Subdivision 6: Indie Developer
```

### Classification Summary Table

| Subdivision | Name | `publisher` | `developer` | `ownPlatform` | Stars Threshold | Examples |
|:---:|:---|:---:|:---:|:---:|:---:|:---|
| 1 | Platform Holder | true | any | true | — | Sony, Nintendo, Microsoft, Sega |
| 2 | Big Publisher | true | any | false | >= 70 | EA, Activision, Ubisoft |
| 3 | Small Publisher | true | any | false | < 70 | Devolver, Annapurna |
| 4 | Big Developer | false | true | false | >= 75 | FromSoftware, Capcom, CDPR |
| 5 | Mid-Sized Developer | false | true | false | 40–74 | Remedy, IO Interactive, Techland |
| 6 | Indie Developer | false | true | false | < 40 | Small startups |

> **Note:** The `isPlayer` check excludes the player's own studio from classification (returns 0).

---

## 2. Relationship System Fundamentals

### Point System

- Relationship is stored as a `float` from **0 to 100**.
- Displayed as **1–5 stars**, where each star = 20 points.
  - 1 Star = 20 points
  - 2 Stars = 40 points
  - 3 Stars = 60 points
  - 4 Stars = 80 points
  - 5 Stars = 100 points
- Relationship is clamped: `Mathf.Clamp(value, 0f, 100f)` — it can never go below 0 or above 100.

### How Relationship Changes Are Applied

The mod uses an `AdjustRelation(publisherScript, float)` method that:
1. Reads the studio's current `relation` value.
2. Adds the delta (positive or negative).
3. Clamps the result to 0–100.
4. Saves state to disk.

Some perks apply the change directly via `pS.relation = Mathf.Clamp(pS.relation + X, 0f, 100f)` instead.

### Duplicate Prevention

The mod uses `HashSet<int>` trackers to ensure relationship changes fire **exactly once** per game/event combination. These survive save/load. The trackers use composite hashes (e.g., `gameID * 1000 + studioID`) to avoid collisions.

### Cooldown System

Perks use a "last used week" system. The game tracks time in **global weeks** calculated as:
```
globalWeek = year * 48 + month * 4 + week
```
(48 weeks per year, 4 weeks per month, 12 months per year)

Cooldowns are checked as: `currentWeek - state.LastXxxWeek < cooldownWeeks`

---

## 3. Subdivision 1: Platform Holders

**Examples:** Sony, Nintendo, Microsoft, Sega
**Classification:** `publisher == true && ownPlatform == true`

### Relationship Increases

| Action | Points | Trigger | Duplicate Protection |
|:---|:---:|:---|:---|
| Release a game on their console | +3 | When your game releases (`SetOnMarket`) and is on their platform | `processedReleasedGames` (hash: `gameID * 1000 + studioID`) |
| Release a good game (80+ review) on their console | +5 | Same trigger, if `reviewTotal >= 80` | `processedHitGames` (same hash) |
| Release a masterpiece (90+ review) on their console | +10 | Same trigger, if `reviewTotal >= 90` | `processedMasterpieces` (same hash) |
| High sales / major hit on their console | +10 | Same trigger, if `reviewTotal >= 80` (combined with hit check) | `processedHitGames` (same hash) |
| Make a console-exclusive game for them | +30 | When your game releases and `exklusiv == true` and is on their platform | `processedExclusiveGames` (hash: `gameID * 1000 + studioID`) |
| Accept their development commission | +20 | When you complete a commissioned game on their platform with review >= target | `processedCommissions` |

### Relationship Decreases

| Action | Points | Trigger | Duplicate Protection |
|:---|:---:|:---|:---|
| Release a game exclusive to a rival console | -30 | When your exclusive game releases on a different platform holder's console | `processedExclusiveGames` (hash: `gameID * 10000 + 111`) |
| Cancel a console-exclusive contract midway | -50 | When you cancel/discard a game (`Menu_W_GameVerwerfen.BUTTON_Yes`) that had an exclusivity contract | `processedCancelledContracts` (hash: `game.myID`) |
| Release very low-rated games on their console | -5 to -15 | When your game releases on their platform with low reviews | `processedReleasedGames` (same hash) |

**Low review penalty breakdown:**
- Review < 30 → **-15** points
- Review 30–44 → **-10** points
- Review 45–59 → **-5** points

### CRITICAL RIVALRY RULE

When the player releases their own console hardware to the market (`PlayerHasActiveConsole` returns true):
- **All platform holder perks become permanently locked.**
- Relationship with ALL platform holders is instantly dropped to **20 points (1 Star)** and locked there.
- The side panel shows "Locked (Rival Console Owner)" in red.
- This check runs weekly via `UpdateRivalryCheck()`.

**Console detection logic:** Any platform where `ownerID == playerID && isUnlocked && !vomMarktGenommen && (typ == 0 || typ == 1 || typ == 2)` counts as an active player console.

### Perk Tiers

#### ★ 1 Star (20 points) — Gameplay Instant-Unlock
- **What it does:** Instantly unlocks a random locked gameplay feature for free — no research room, time, or money needed.
- **Cooldown:** 8 weeks (2 months)
- **Relationship gain on use:** +2 points
- **Internal logic:** Finds all gameplay features where `!IsErforscht(i)`, picks one at random, sets `gameplayFeatures_UNLOCK[pick] = true` and `gameplayFeatures_RES_POINTS_LEFT[pick] = 0f`.
- **Cooldown field:** `state.LastGameplayUnlockWeek`

#### ★ 2 Stars (40 points) — Engine Instant-Unlock
- **What it does:** Instantly unlocks a random locked engine feature for free.
- **Cooldown:** 12 weeks (3 months)
- **Relationship gain on use:** +3 points
- **Internal logic:** Same pattern as gameplay unlock but for `engineFeatures`.
- **Cooldown field:** `state.LastEngineUnlockWeek`

#### ★ 3 Stars (60 points) — Tech Sharing / Publishing Deal
- **What it does:** The platform holder acts as publisher for your game at a **premium royalty rate of 10%** (better than any other NPC publisher). Works for in-development projects or new projects.
- **Cooldown:** 144 weeks (3 years)
- **Relationship gain on use:** +10 points
- **Internal logic:** Sets `game.publisherID = pS.myID`, `game.pubAngebot_Gewinnbeteiligung = 10f`.
- **Cooldown field:** `state.LastCoFinanceWeek`

#### ★ 4 Stars (80 points) — Exclusivity / Full Funding
- **What it does:** Make 1 game exclusive to their platform. They cover **100% of the funding** — you get refunded all development costs spent so far. They publish the game at 5% royalty. All other platforms are cancelled.
- **Cooldown:** 240 weeks (5 years)
- **Relationship gain on use:** +15 points
- **Internal logic:** Finds the platform holder's active console, sets `game.exklusiv = true`, `game.gamePlatform[0] = targetPlatId`, clears platforms 1–3, sets royalty to 5%, refunds `costs_entwicklung + costs_mitarbeiter`.
- **Cooldown field:** `state.LastExclusiveWeek`

#### ★ 5 Stars (100 points) — First-Party Partner / Debt Bailout
- **What it does:** Instantly clears ALL player debt, gives $500,000 cash (plus enough to reach $1M if below). In exchange, **all your game IPs are transferred to the platform holder**.
- **Limit:** Once per playthrough (tracked by `state.FundedByPlayer` flag)
- **Relationship gain on use:** +30 points
- **Internal logic:** Sets `mS.kredit = 0`, gives `max(0, 1M - currentMoney) + 500K`, transfers all games where `ownerID == playerID` to `ownerID = pS.myID`.

---

## 4. Subdivision 2: Big Publishers

**Examples:** EA, Activision, Ubisoft
**Classification:** `publisher == true && ownPlatform == false && stars >= 70`

### Relationship Increases

| Action | Points | Trigger | Duplicate Protection |
|:---|:---:|:---|:---|
| Use them normally for publishing a game | +5 | When your game releases and `publisherID` is this studio | `processedReleasedGames` (hash: `gameID * 1000 + studioID`) |
| Sign publishing deal and secure a hit (80+ review) | +20 | Same trigger, if `reviewTotal >= 80` and no AAA co-publish contract | `processedHitGames` |
| AAA co-publishing success (80+ review) | +25 | Same trigger, if `reviewTotal >= 80` and game has `AAACoPublish` contract | `processedHitGames` |
| Complete contract games with high reviews | +15 | When your game releases and `typ_contractGame == true` | `processedContractGames` |

### Relationship Decreases

| Action | Points | Trigger | Duplicate Protection |
|:---|:---:|:---|:---|
| Cancel a signed publishing contract or contract game | -30 | When you discard a game (`Menu_W_GameVerwerfen.BUTTON_Yes`) | `processedCancelledContracts` (hash: `game.myID`) |
| Decline their contract game offers | -5 | When you remove a contract offer (`Item_ContractAuftragsspiel.BUTTON_Remove`) | None (fires each time) |
| Release games competing with their key franchises | -10 | When your game releases and they have a competing game (same genre, on market <= 4 weeks) | `processedFailures` (hash: `gameID * 1000 + studioID`) |

**Competing release detection:** Checks if the Big Publisher has any game on market where `developerID == pub.myID && isOnMarket && weeksOnMarket <= 4 && maingenre == yourGame.maingenre`.

### Perk Tiers

#### ★ 1 Star (20 points) — Topic Instant-Unlock
- **What it does:** Instantly unlocks a random locked topic/theme for free.
- **Cooldown:** 4 weeks (1 month)
- **Relationship gain on use:** +1 point
- **Cooldown field:** `state.LastTopicUnlockWeek`

#### ★ 2 Stars (40 points) — Trusted Partner / Signing Bonus
- **What it does:** Better publishing royalty rate (10%) + upfront signing bonus based on game size.
- **Cooldown:** 96 weeks (2 years)
- **Relationship gain on use:** +8 points
- **Signing bonus by game size:**
  - B (size 0): $100,000
  - B+ (size 1): $200,000
  - A (size 2): $400,000
  - AA (size 3): $800,000
  - AAA (size 4): $1,500,000
  - AAAA (size 5): $3,000,000
- **Cooldown field:** `state.LastCoFinanceWeek`

#### ★ 3 Stars (60 points) — IP Cooperation / Partial Funding
- **What it does:** Use their IPs to make sequels/spin-offs. They own the IP. You choose funding % — higher funding = worse royalties.
- **Cooldown:** 144 weeks (3 years)
- **Relationship gain on use:** +12 points
- **Funding options:**
  - IP Cooperation High (80% funding, 25% royalty)
  - IP Cooperation Low (20% funding, 10% royalty)
- **Cooldown field:** `state.LastCoFinanceWeek`

#### ★ 4 Stars (80 points) — AAA Co-Publishing Pitch
- **What it does:** Pitch a game using their IP, your IP, or a new IP. They fund 50–100% of costs.
- **Cooldown:** 144 weeks (3 years)
- **Relationship gain on use:** +15 points (+20 more if you gift them a new IP)
- **Funding/royalty options:**
  - New IP — Gift IP: 100% funding, good royalties (`AAA_GIFT_IP`)
  - New IP — Keep IP: 50% funding, slightly worse royalties (`AAA_KEEP_IP`)
  - Player IP: 50% funding, fair royalty deal
  - Their IP: 75% funding, good royalty deal (`AAA_THEIR_IP`)
- **Cooldown field:** `state.LastAAAWeek`

#### ★ 5 Stars (100 points) — Acquisition & Consolidation
- **What it does:** Two options:
  1. **Subsidiary Buyout:** Fully acquire the studio as a subsidiary with a **30% discount** on firm value (pay 70%).
  2. **IP/Console Exclusives:** If you have a console, force their in-development game to be exclusive to your console or put it on your subscription service.
- **Buyout relationship gain:** +20 points
- **Exclusivity cooldown:** 144 weeks (3 years)
- **Exclusivity relationship gain:** +20 points
- **Cooldown field:** `state.LastExclusiveWeek`

---

## 5. Subdivision 3: Small Publishers

**Examples:** Devolver, Annapurna
**Classification:** `publisher == true && ownPlatform == false && stars < 70`

### Relationship Increases

| Action | Points | Trigger | Duplicate Protection |
|:---|:---:|:---|:---|
| Use them normally for publishing a game | +5 | When your game releases and `publisherID` is this studio | `processedReleasedGames` |
| Publish their developed games | +20 | When their game releases and `publisherID == playerID` | `processedReleasedGames` |
| Buy/Sell indie IPs to them | +10 | When you sell an IP (`Menu_Result_IpVerkauf.BUTTON_Yes`) | `processedGiftedIPs` (hash: `game.mainIP`) |
| Buy their IP | +15 | When you buy an IP (`Menu_W_IpHandel_Buy.BUTTON_Yes`) | `processedBoughtIPs` (hash: `game.mainIP`) |
| Send free engine licenses | +15 | When you gift an engine license (perk button) | `processedEngineLicenses` (hash: `studio.myID`) |

### Relationship Decreases

| Action | Points | Trigger | Duplicate Protection |
|:---|:---:|:---|:---|
| Self-publish or use another publisher when they offered a contract | -15 | When your game releases and `publisherID == playerID` and their relation >= 20 | `processedCancelledContracts` (hash: `gameID * 1000 + studioID`) |
| Cancel active publishing agreement midway | -25 | When you discard a game (`Menu_W_GameVerwerfen.BUTTON_Yes`) | `processedCancelledContracts` (hash: `game.myID`) |

### Perk Tiers

#### ★ 1 Star (20 points) — Basic Contact
- **What it does:** Passive — publishing games through them increases relationship. No button.
- **Relationship gain:** +3 points per published game (automatic)

#### ★ 2 Stars (40 points) — Indie Sponsor + Gift Engine License
- **Indie Sponsor:** Better publishing rate (20% royalty) + upfront signing bonus of `$50,000 * (gameSize + 1)`.
  - Cooldown: 96 weeks (2 years)
  - Relationship gain: +5 points
  - Cooldown field: `state.LastCoFinanceWeek`
- **Gift Engine License:** Gift a free engine license.
  - One-time per studio
  - Relationship gain: +15 points

#### ★ 3 Stars (60 points) — IP Cooperation + Sell IP
- **IP Cooperation:** Use their IPs to make sequels/spin-offs. Max 40% funding.
  - Cooldown: 144 weeks (3 years)
  - Relationship gain: +10 points
- **Sell IP to Publisher:** Sell one of your IPs to them at a high value.
  - Relationship gain: +15 points (applied via `HandleSellIPToNPC`)

#### ★ 4 Stars (80 points) — Console Exclusivity / Subscription
- **What it does:** If you have a console, force their in-development game to be exclusive or put it on your subscription service.
- **Cooldown:** 96 weeks (2 years)
- **Relationship gain:** +15 points
- **Cooldown field:** `state.LastExclusiveWeek`

#### ★ 5 Stars (100 points) — Friendly Absorption
- **What it does:** Fully buy out and acquire the entire studio with a **50% discount** on firm value (pay 50%).
- **Relationship gain:** +25 points (applied via `HandleFriendlyBuyout` → `Menu_W_FirmaKaufen.BUTTON_Yes` Postfix)

---

## 6. Subdivision 4: Big Developers

**Examples:** FromSoftware, Capcom, CDPR
**Classification:** `developer == true && publisher == false && stars >= 75`

### Relationship Increases

| Action | Points | Trigger | Duplicate Protection |
|:---|:---:|:---|:---|
| Publish their games and secure high sales (80+ review) | +20 | When their game releases and `publisherID == playerID` and `reviewTotal >= 80` | `processedReleasedGames` |
| License your engine to them with 0% royalty | +15 | When you gift an engine license (perk button) | `processedEngineLicenses` |
| Direct co-development / outsource project completion | +15 | When an outsourced game releases (`SetOnMarket`) | `state.OutsourcedGameIds` removal + `processedReleasedGames` |

### Relationship Decreases

| Action | Points | Trigger | Duplicate Protection |
|:---|:---:|:---|:---|
| Publishing their games and getting poor reviews (< 60) | -15 | When their game releases and `publisherID == playerID` and `reviewTotal < 60` | `processedReleasedGames` |
| Cancel a signed publishing contract for their game | -30 | When you discard a game (`Menu_W_GameVerwerfen.BUTTON_Yes`) | `processedCancelledContracts` |

### Perk Tiers

#### ★ 1 Star (20 points) — Basic Dev (Publish In-Dev Game)
- **What it does:** Act as publisher for their in-development game.
- **Relationship gain:** +5 points
- **Internal logic:** Sets `inDev.publisherID = playerID`, `inDev.pS_ = null`.

#### ★ 2 Stars (40 points) — IP Licensing Partner + Gift Engine License
- **IP Licensing:** Make games in their IP. They provide 15–35% funding.
  - Relationship gain: +10 points
- **Gift Engine License:** Free engine license.
  - Relationship gain: +15 points

#### ★ 3 Stars (60 points) — Outsourcing & IP Purchase
- **Outsource Project:** Assign them a project based on your IP or a new IP. You fully fund it.
  - **In-Dev Queue System:**
    - Cancel Current: Pay extra to force immediate start (**-10** relationship penalty)
    - Queue: They start once done (**+15** relationship on completion)
    - IP Gift: Gift them a new IP for **+35** relationship points
- **Buy Game IP:** Buy one of their mature IPs outright.
  - Cooldown: 480 weeks (10 years)
  - Relationship gain: +15 points
  - Cooldown field: `state.LastIPBuyWeek`

#### ★ 4 Stars (80 points) — Automatic Publication Contract
- **What it does:** All their developed games are automatically published by you. You pay a yearly fee.
- **Contract duration:** 96 weeks (2 years)
- **Initial relationship gain:** +10 points
- **Passive yearly gain:** +5 points per year
- **Yearly fee calculation:** `$50,000 * (1 + stars/50) * (1 + goodwillTrend/20)`, minimum $10,000
- **Internal logic:** Sets `state.ActiveContract = "AutoPublish"`, `state.ContractWeeksLeft = 96`. Each week, assigns player as publisher for their in-development games at 15% royalty.

#### ★ 5 Stars (100 points) — Friendly Acquisition / IP Buyout
- **What it does:** Acquire the studio as a subsidiary with a **30% discount** (pay 70%), or buy one of their IPs.
- **Acquisition relationship gain:** +20 points
- **IP purchase cooldown:** 480 weeks (10 years)

---

## 7. Subdivision 5: Mid-Sized Developers

**Examples:** Remedy, IO Interactive, Techland
**Classification:** `developer == true && publisher == false && stars >= 40 && stars < 75`

### Relationship Increases

| Action | Points | Trigger | Duplicate Protection |
|:---|:---:|:---|:---|
| Co-finance/Fund their games | +20 | When their game releases and `publisherID == playerID` | `processedReleasedGames` |
| License your engine | +15 | When you gift an engine license (perk button) | `processedEngineLicenses` |
| Publish their games | +15 | When their game releases and `publisherID == playerID` | `processedReleasedGames` |

### Relationship Decreases

| Action | Points | Trigger | Duplicate Protection |
|:---|:---:|:---|:---|
| Fund a game and then cancel it or reject the final build | -25 | When you discard a game (`Menu_W_GameVerwerfen.BUTTON_Yes`) | `processedCancelledContracts` |
| License engine but charge high royalties (> 10%) | -15 | Weekly check via `UpdateContractsWeekly()` | `processedFailures` (hash: `studioID * 10000 + 999`) |

**Engine abuse detection:** Every week, checks if any player-owned engine has `sellEngine == true && gewinnbeteiligung > 10`. If so, applies -15 penalty once.

### Perk Tiers

#### ★ 1 Star (20 points) — Standard Dev (Publish In-Dev Game)
- **What it does:** Act as publisher for their in-development game.
- **Relationship gain:** +4 points

#### ★ 2 Stars (40 points) — Full Project Financing (Outsourcing) + Gift Engine License
- **Outsource Project:** Assign them a project. You fully fund it.
  - **In-Dev Queue System:**
    - Cancel Current: **-10** relationship penalty
    - Queue: **+15** relationship on completion
    - IP Gift: **+40** relationship points (higher than Big Dev's +35)
- **Gift Engine License:** Free engine license.
  - Relationship gain: +15 points

#### ★ 3 Stars (60 points) — Console Exclusivity & Subscription
- **What it does:** If you have a console, make their in-development game exclusive or put it on your subscription service.
- **Cooldown:** 144 weeks (3 years)
- **Relationship gain:** +12 points
- **Cooldown field:** `state.LastExclusiveWeek`

#### ★ 4 Stars (80 points) — Automatic Publication Contract
- **What it does:** All their developed games are automatically published by you. You pay a yearly fee.
- **Contract duration:** 96 weeks (2 years)
- **Initial relationship gain:** +10 points
- **Passive yearly gain:** +5 points per year
- **Yearly fee:** Same formula as Big Dev (see above)

#### ★ 5 Stars (100 points) — Acquisition Option
- **What it does:** Acquire the studio as a subsidiary with a **40% discount** (pay 60%), or buy one of their IPs.
- **Acquisition relationship gain:** +20 points
- **IP purchase cooldown:** 480 weeks (10 years)

---

## 8. Subdivision 6: Indie Developers

**Examples:** Small startups
**Classification:** `developer == true && publisher == false && stars < 40`

### Relationship Increases

| Action | Points | Trigger | Duplicate Protection |
|:---|:---:|:---|:---|
| Fully fund their games | +30 | When their game releases and `publisherID == playerID` | `processedReleasedGames` |
| Offer free engine licenses | +20 | When you gift an engine license (perk button) | `processedEngineLicenses` |
| Publish their first game | +20 | When their game releases and `publisherID == playerID` and `!state.PublishedFirstGame` | `state.PublishedFirstGame` flag (one-time) |

### Relationship Decreases

| Action | Points | Trigger | Duplicate Protection |
|:---|:---:|:---|:---|
| Cancel their funded game midway | -30 | When you discard a game (`Menu_W_GameVerwerfen.BUTTON_Yes`) | `processedCancelledContracts` |
| Buy their IP for cheap and bury it (no sequels for 96 weeks) | -15 | Weekly check: 96 weeks after buying their IP | `state.LastIPBuyWeek` (one-time trigger) |

**IP burying detection:** When you buy an IP from an Indie Dev, `state.LastIPBuyWeek` is set. After 96 weeks, if no sequel has been released, -15 relationship is applied once.

### Perk Tiers

#### ★ 1 Star (20 points) — In-Development Publisher
- **What it does:** Act as publisher for their in-development game.
- **Relationship gain:** +5 points

#### ★ 2 Stars (40 points) — Full Project Financing (Outsourcing) + Gift Engine License
- **Outsource Project:** Assign them a project. You fully fund it.
  - **In-Dev Queue System:**
    - Cancel Current: **-10** relationship penalty
    - Queue: **+15** relationship on completion
    - IP Gift: **+40** relationship points
- **Gift Engine License:** Free engine license.
  - Relationship gain: **+20** points (higher than other subdivisions' +15)

#### ★ 3 Stars (60 points) — Automatic Publication Contract
- **What it does:** All their developed games are automatically published by you. You pay a yearly fee.
- **Contract duration:** 96 weeks (2 years)
- **Initial relationship gain:** +10 points
- **Passive yearly gain:** +5 points per year
- **Yearly fee:** Same formula as above

#### ★ 4 Stars (80 points) — Console Exclusivity & Subscription
- **What it does:** All their games become exclusive to your console + day one on your subscription service. Requires active console or subscription.
- **Contract type:** `ConsoleAutoPublish`
- **Contract duration:** 96 weeks (2 years)
- **Initial relationship gain:** +15 points
- **Passive yearly gain:** +5 points per year
- **Internal logic:** Each week, sets their in-development games as `exklusiv = true`, `gamePlatform[0] = playerPlatformID`, adds to Game Pass.

#### ★ 5 Stars (100 points) — Acquire Studio
- **What it does:** Acquire the entire studio as a subsidiary with a **50% discount** (pay 50%).
- **Relationship gain:** +20 points

---

## 9. Cross-Cutting Mechanics

### Development Commissions (Platform Holders Only)

Platform holders with **40+ relationship** (2+ stars) and when the player does NOT have their own console can randomly offer development commissions.

- **Trigger:** Weekly check with 2% chance per week per eligible platform holder.
- **Commission parameters:**
  - Genre: Random (0–9)
  - Size: Random B+ to AA (1–3)
  - Target review: Random 70–85%
  - Reward: `$250,000 * (size + 1)` → $500K to $1M
  - Time limit: 48 weeks
- **Success:** If you release a game on their platform matching the genre with review >= target → +20 relationship + reward money.
- **Failure:** If 48 weeks pass without completion → -15 relationship.

### Co-Funding Reimbursement System

For games with active contracts (AAA Co-Publish, IP Cooperation, Exclusivity Funding), the mod reimburses a percentage of weekly development costs:

| Contract Type | Funding % | Royalty Rate |
|:---|:---:|:---:|
| EXCL_PH_FUND (Platform Holder Exclusivity) | 100% | 5% |
| AAA_GIFT_IP (AAA Co-Publish, Gift IP) | 100% | 20% |
| AAA_THEIR_IP (AAA Co-Publish, Their IP) | 75% | 12% |
| AAA_KEEP_IP (AAA Co-Publish, Keep IP) | 50% | 10% |
| IP_COOP_HIGH (Big Publisher) | 80% | 25% |
| IP_COOP_HIGH (Small Publisher) | 40% | 25% |
| IP_COOP_LOW | 20% | 10% |

**How it works:** Each week, the mod calculates `currentCosts = costs_entwicklung + costs_mitarbeiter`, compares to last week's costs, and refunds `diff * fundingPercent` to the player.

### Auto-Publish Contract System

When active (`AutoPublish` or `ConsoleAutoPublish`):
- Each week, the mod finds the developer's in-development game and sets `publisherID = playerID` at 15% royalty.
- For `ConsoleAutoPublish`, also sets the game as exclusive to the player's console and adds it to Game Pass.
- Yearly fee is paid every 48 weeks.
- Contract lasts 96 weeks (2 years), then expires automatically.

### Outsourcing System

When you outsource a project to a developer:
1. A temporary design room is created.
2. The game creation menu opens for you to design the game.
3. When you start the game, it's assigned to the NPC developer with `developerID = npc.myID` and `publisherID = playerID`.
4. The game ID is tracked in `state.OutsourcedGameIds`.
5. When the game releases (`SetOnMarket`), +15 relationship is applied and the ID is removed from the tracking list.
6. If a queued project exists, it's triggered automatically.

### Buyout Discount System

When acquiring a studio via a perk:
- `activeBuyoutDiscountFactor` is set (e.g., 0.70 for 30% off, 0.50 for 50% off).
- The `GetFirmenwert` postfix applies the discount when the buyout menu is open.
- On successful purchase (`BUTTON_Yes`), +20 relationship is applied and the discount is reset.

### Save/Load System

All state is saved to `BepInEx/config/StudioPartnerships_State_[slot].tsv` in TSV format:
- Per-studio state (31 fields per studio)
- Global event trackers (13 HashSets)
- Active game contracts (game ID → contract type mapping)

State is loaded on game load and saved on game save.

---

## 10. Partnerships Overview Panel (Ctrl+Shift+P)

Press **Ctrl+Shift+P** in-game to open the Partnerships Overview panel. It shows all active deals:

### What's Displayed

| Section | What It Shows | Removed When |
|:---|:---|:---|
| **Game Contracts** | AAA Co-Publish, IP Cooperation, Exclusivity Funding — with game name, funding %, progress %, weeks to release | Game is released |
| **Permanent Deals** | Auto-Publish and Console Auto-Publish contracts — with current game in development, progress %, weeks to release. Auto-replaces game when one releases and next starts. | Contract expires |
| **Outsourced Projects** | Games you outsourced to developers — with game name, progress %, weeks to release | Game is released |
| **Queued Projects** | Outsource projects waiting for the developer to finish their current project | Project starts |
| **Development Commissions** | Active commissions from Platform Holders — with time left, target review, reward | Commission completed or expired |
| **Publishing Deals** | Games you're publishing through NPC publishers — with game name, progress %, weeks to release, EXCLUSIVE/Game Pass tags | Game is released |
| **Publishing For** | NPC developer games you're publishing — with game name, progress %, weeks to release, EXCLUSIVE/Game Pass tags | Game is released |

### Progress Calculation

For games in development, progress is calculated as:
```
progress% = (1 - (weeksLeft / totalWeeks)) * 100
```
Where `weeksLeft` and `totalWeeks` come from the developer studio's `newGameInWeeks` and `newGameInWeeksORG` fields.

### Color Coding

| Color | Meaning |
|:---|:---|
| Blue (#4FC3F7) | In Development |
| Green (#81C784) | Released / Active |
| Orange (#FFB74D) | Pending / Waiting |
| Yellow (#FFD54F) | EXCLUSIVE tag |
| Light Green (#A5D6A7) | Game Pass tag |

---

*This document reflects the exact implementation as of the current mod version. All numbers, cooldowns, and mechanics are sourced directly from the mod's C# code.*
