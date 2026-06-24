# Dynamic Studio Goodwill — Comprehensive Enhancement Plan

> 20 features to deepen, expand, and enrich the goodwill system.

---

## Feature 1: Continuous Evaluation Schedule

### Concept
Replace the binary week‑4 / week‑24 evaluation with a rolling schedule that keeps every studio "accountable" throughout the year.

### Why
The mod is currently silent for 20 weeks between launch and long‑tail. A studio that releases a game every 8‑10 months can fly under the radar. A continuous schedule creates ongoing pressure and makes the world feel reactive — every release month matters.

### Implementation
- Add evaluation points at **weeks 4, 13, 26, 39, 52** (first month, then quarterly).
- Each evaluation has its own `processedGames` HashSet to prevent double‑firing:
  ```csharp
  private static readonly HashSet<int> processedLaunchGames = new();
  private static readonly HashSet<int> processedQ1Games = new();    // week 13
  private static readonly HashSet<int> processedQ2Games = new();    // week 26
  private static readonly HashSet<int> processedQ3Games = new();    // week 39
  private static readonly HashSet<int> processedAnnualGames = new(); // week 52
  ```
- Evaluation weights cascade (diminishing returns as the game ages):
  - Week 4  (Launch)       → 1.00×
  - Week 13 (Q1 Review)    → 0.60×
  - Week 26 (Half‑Year)    → 0.50×
  - Week 39 (Q3 Review)    → 0.40×
  - Week 52 (Annual)       → 0.35×
- Annual summary at week 52: aggregate the year's trend and apply a final adjustment.
- **No‑release decay**: a studio that released nothing in the last 52 weeks gets a small natural decay (`-0.5%` to `-2%` depending on their market presence). "Out of sight, out of mind."

### Data structures
```csharp
private const int AnnualJudgmentWeek = 52;
private const float NaturalDecayRate = 1.0f; // configurable %
```

### Config
```ini
EvaluationStyle = Rolling        # Simple | Rolling | Continuous
NaturalDecayRate = 1.0           # % per year with no releases (0 = disabled)
```

### Edge cases
- A game released after week 48 should still be eligible for the annual summary.
- The old `LongTailJudgmentWeek` / `processedLongTailGames` can either coexist with the quarterly system or be deprecated — make it configurable (e.g. `UseLegacyLongTail = true`).

---

## Feature 2: Review Quality Separation

### Concept
Use both **critic reviews** (`reviewTotal`) and **user reviews** (the existing `userReviewTotal` field on `gameScript`) to differentiate critical prestige from audience love.

### Why
A game can be a critic darling but a commercial flop (art‑house hit), or trashed by critics but loved by players (guilty pleasure). These should affect goodwill differently.

### Implementation
```csharp
// Expected review for critics vs. users
float expectedCriticReview = 42f + stars * 0.43f;
float expectedUserReview   = 35f + stars * 0.35f;   // users are easier to please

float criticDelta = game.reviewTotal     - expectedCriticReview;
float userDelta   = game.userReviewTotal - expectedUserReview;

// Critics drive prestige/long‑term value, users drive commercial momentum
float criticImpact = Mathf.Clamp(criticDelta * 0.10f, -4.0f, 4.0f);
float userImpact   = Mathf.Clamp(userDelta   * 0.08f, -3.0f, 3.0f);

// Weight shifts based on studio maturity
float criticWeight, userWeight;
if (stars >= 70f) {
    criticWeight = 0.7f;  userWeight = 0.3f;   // Established: critics matter more
} else {
    criticWeight = 0.4f;  userWeight = 0.6f;   // Upstart: audience matters more
}

float combinedImpact = criticImpact * criticWeight + userImpact * userWeight;
```

### Data structures
No new save‑data needed — reads existing `gameScript` fields (`reviewTotal`, `userReviewTotal`).

### Config
```ini
CriticWeightBase = 0.5          # How much critics matter for a 50‑star studio
UserReviewWeightBase = 0.5      # How much users matter for a 50‑star studio
```

### Integration
The `LogImpact` method should log both critic and user deltas for debugging.

---

## Feature 3: Magnitude‑Scaled Events

### Concept
Instead of flat ±2–3 % for events (hit / flop / cult / overhype / comeback), **scale the magnitude by how extreme the outcome is**.

### Why
A game selling 10 M units shouldn't get the same boost as one barely exceeding expectations. Scale creates drama, tells a story, and makes the numbers feel earned.

### Implementation

#### Commercial Hit (scales upward)
```csharp
if (commercialHit) {
    float hitMagnitude = Mathf.Clamp01((float)(salesRatio - 2.25f) / 5.0f); // 2.25× → 7.25×
    eventImpact += 2.5f + hitMagnitude * 5.0f;    // +2.5 % → +7.5 %
}
```

#### Commercial Flop (scales downward)
```csharp
if (commercialFlop) {
    float flopMagnitude = Mathf.Clamp01((0.35f - (float)salesRatio) / 0.34f); // 0.35× → 0.01×
    eventImpact -= 3.0f + flopMagnitude * 4.0f;    // −3 % → −7 %
}
```

#### Overhype (scales with how big the gap was)
```csharp
if (overhyped) {
    float hypeMagnitude = Mathf.Clamp01(Mathf.Abs(reviewDelta - 12f) / 30f);
    eventImpact -= 2.5f + hypeMagnitude * 4.0f;    // −2.5 % → −6.5 %
}
```

#### Cult Breakout (grows over time)
- At detection (week 4): initial +2.0 %.
- Then track `CultGrowthRemaining` in `StudioState`: each month for 6 months add another +0.5 % (total +5.0 % if sustained).
- Simulates word‑of‑mouth snowballing.

```csharp
// In StudioState
public float CultGrowthRemaining;   // months of cult growth left
public int   CultGrowthApplied;     // months already applied

// Monthly update (OnMonthChanged)
if (state.CultGrowthRemaining > 0) {
    ApplyCultGrowth(state, studio);
    state.CultGrowthRemaining--;
}
```

### Edge cases
- A game can't be both a "Hit" and "Cult" simultaneously — Hit is commercial dominance, Cult is prestige‑over‑commerce.
- If a Cult game later becomes a commercial hit (sales ratio crosses 2.25), upgrade it to Hit status and refund the remaining cult growth.

---

## Feature 4: Track Record Moving Window

### Concept
Replace the static formula `expectedReview = 42 + stars × 0.43` with expectations based on the studio's **actual recent performance** — a rolling window of their last N games.

### Why
A studio that released three consecutive 90+ games should be expected to maintain that level. A studio with a 60‑average has room to surprise. This creates realistic pressure at the top: once you're there, it's hard to stay.

### Implementation
```csharp
private const int TrackRecordWindow = 5;   // Last 5 games

// In StudioState
public List<int> RecentReviews;
public int       TotalGamesEvaluated;

// Helper: sliding‑window expected review
private float GetExpectedReview(publisherScript studio, StudioState state) {
    if (state.RecentReviews.Count < 2) {
        // Not enough data → fall back to star‑based formula
        return 42f + studio.stars * 0.43f;
    }

    float average   = (float)state.RecentReviews.Average();
    float starBias  = studio.stars * 0.15f;
    float trendBias = state.TrendScore * 1.5f;

    // Weight: 60 % recent average, 25 % stars, 15 % trend momentum
    return average * 0.60f + starBias + trendBias;
}
```

#### Volatility multiplier
Studios with wildly inconsistent reviews have wider expectation bands (higher risk/reward).
```csharp
float stdDev = CalcStdDev(state.RecentReviews);
float volatilityMultiplier = 1.0f + Mathf.Clamp(stdDev / 30f, 0f, 0.5f);
// ±0 % → ±50 % wider impact bands
```

### Data structures (Save / Load)
```csharp
// In StudioState – serialised as comma‑separated in TSV
public List<int> RecentReviews;
```

TSV column: `recentReviews: 85,92,78,81,95`

### Config
```ini
TrackRecordWindow = 5            # Number of games to track (3–10)
ExpectationWeight = 60           # % weight of recent average
StarsWeight = 25                 # % weight of stars
TrendWeight = 15                 # % weight of trend score
```

---

## Feature 5: Momentum Chains

### Concept
Consecutive successes build a **Hot Streak** with compounding returns; consecutive failures build a **Cold Streak** with compounding penalties. Breaking a streak is a major narrative event.

### Why
Streaks are how the real world thinks — a "threepeat" is exciting; a "fifth flop in a row" is existential. This adds narrative flavour and makes every evaluation feel connected to the last one.

### Implementation
```csharp
// In StudioState
public int HotStreak;    // consecutive positive evaluations
public int ColdStreak;   // consecutive negative evaluations

// After each totalPercent calculation:
if (totalPercent > 0f) {
    state.HotStreak++;
    state.ColdStreak = 0;

    float streakBonus = Mathf.Min(state.HotStreak * 0.5f, 5.0f);
    totalPercent += streakBonus;

    if (state.HotStreak == 3)
        ShowNews($"{studio.GetName()} is on fire! Third straight hit!");
} else if (totalPercent < 0f) {
    state.ColdStreak++;
    state.HotStreak = 0;

    float streakPenalty = Mathf.Min(state.ColdStreak * 0.7f, 7.0f);
    totalPercent -= streakPenalty;

    if (state.ColdStreak == 3)
        ShowNews($"{studio.GetName()} in crisis mode after third consecutive disappointment.");
}

// Comeback bonus amplified by cold‑streak length
if (comeback) {
    float comebackMagnitude = 2.5f + state.ColdStreak * 1.5f;
    eventImpact += comebackMagnitude;   // +2.5 % → +XX %
}
```

### Config
```ini
HotStreakBonusPerGame = 0.5     # % per consecutive success
HotStreakMaxBonus = 5.0         # cap
ColdStreakPenaltyPerGame = 0.7  # % per consecutive failure
ColdStreakMaxPenalty = 7.0      # cap
ColdStreakComebackMultiplier = 1.5  # extra comeback % per cold‑streak game
```

---

## Feature 6: Studio Identity & Genre Specialization

### Concept
NPC studios develop **genre identities** based on their history. A studio that makes five RPGs becomes an "RPG Specialist" — better at RPGs, worse at anything else, and the audience expects certain things from them.

### Why
This makes NPCs feel like real studios with brand identities. It also creates strategic gameplay: you can specialise your subsidiaries or seek out partners with complementary strengths.

### Implementation
```csharp
// In StudioState
public Dictionary<int, int> GenreReleaseCount;   // genreID → count
public int?                  PrimaryGenre;        // genre they're most associated with
public float                 SpecializationRatio; // 0–1, how locked‑in they are

private float GetGenreMultiplier(publisherScript studio, StudioState state, gameScript game) {
    if (state.GenreReleaseCount == null || state.GenreReleaseCount.Count == 0)
        return 1.0f;

    int totalGames = state.GenreReleaseCount.Values.Sum();
    if (totalGames < 3) return 1.0f;    // not enough data

    int thisGenreCount = state.GenreReleaseCount.GetValueOrDefault(game.maingenre, 0);
    float ratio = (float)thisGenreCount / totalGames;

    if (ratio >= 0.6f) {
        // Specialist: +15 % in core genre, −10 % elsewhere
        return game.maingenre == state.PrimaryGenre ? 1.15f : 0.90f;
    } else if (ratio >= 0.35f) {
        // Enthusiast: +8 % in preferred genres
        return game.maingenre == state.PrimaryGenre ? 1.08f : 1.0f;
    }

    // Generalist / Identity Crisis
    if (totalGames >= 5) {
        float diversity = (float)state.GenreReleaseCount.Keys.Count / totalGames;
        if (diversity > 0.6f) {                     // bouncing between too many genres
            state.Label = "Identity Crisis";
            return 0.93f;                           // −7 %
        }
    }

    return 1.0f;
}
```

### Data structures (TSV)
```
genreReleases: 8:3,12:5,4:1     # genreID:count pairs
primaryGenre: 12
specializationRatio: 0.42
```

### Integration with trend labels
If a studio is a "Shooter Specialist" with `primaryGenre=12`, they can't be "Declining" in that genre while still carrying the Specialist label. The label system should consider both trend trajectory **and** genre identity.

### Config
```ini
EnableGenreSpecialization = true
MinGamesForIdentity = 3
SpecialistThreshold = 0.6        # 60 %+ of games in one genre → specialist
SpecialistBonus = 15             # +15 %
DiversificationPenalty = 7       # −7 % for too many genres
```

---

## Feature 7: Franchise Stewardship

### Concept
Track how each studio treats IPs over time. Sequels / remasters that maintain quality earn **Franchise Guardian** status. Declining quality earns **Franchise Killer** — affecting both goodwill and IP asset value.

### Why
In the real industry, how a studio handles an established franchise is a major reputational factor. This makes sequel / remaster / spinoff decisions meaningful.

### Implementation
```csharp
// In StudioState
public Dictionary<int, FranchiseRecord> FranchiseHistory;

public class FranchiseRecord {
    public int    IPId;
    public int    GameCount;
    public float  AverageReview;
    public float  ReviewTrend;       // + or −
    public string Stewardship;       // "Guardian" | "Stable" | "Killer"
}

// Called during EvaluateGame for sequels / remasters / spinoffs
private void EvaluateFranchiseHandling(gameScript game, publisherScript studio, StudioState state) {
    if (!game.typ_nachfolger && !game.typ_remaster && !game.typ_spinoff)
        return;

    int ipId = game.ip_id;
    if (ipId < 0) return;

    if (!state.FranchiseHistory.TryGetValue(ipId, out var record)) {
        // First game in this IP — initialise
        state.FranchiseHistory[ipId] = new FranchiseRecord {
            IPId = ipId,
            GameCount = 1,
            AverageReview = game.reviewTotal,
            ReviewTrend = 0f,
            Stewardship = "Stable"
        };
        return;
    }

    // Update rolling average
    float prevAvg = record.AverageReview;
    record.GameCount++;
    record.AverageReview = (prevAvg * (record.GameCount - 1) + game.reviewTotal)
                           / record.GameCount;
    record.ReviewTrend = game.reviewTotal - prevAvg;

    // Assign stewardship
    if (record.GameCount >= 2 && record.ReviewTrend >= 5f)
        record.Stewardship = "Guardian";
    else if (record.GameCount >= 2 && record.ReviewTrend <= -10f)
        record.Stewardship = "Killer";
    else
        record.Stewardship = "Stable";

    // Apply goodwill impact
    if (record.Stewardship == "Guardian")
        totalPercent += 2.5f;
    else if (record.Stewardship == "Killer")
        totalPercent -= 4.0f;
}
```

### Integration
- This data can be consumed by **Subsidiary 2.0** to value IP assets more accurately.
- In the **Partnerships Overhaul** (when fixed), a "Franchise Guardian" studio is a better partner for sequel/spinoff contracts.

### Config
```ini
EnableFranchiseTracking = true
GuardianThreshold = 5.0          # review improvement needed
KillerThreshold  = -10.0         # review decline threshold
GuardianBonus   = 2.5            # % goodwill
KillerPenalty   = 4.0            # % goodwill
```

---

## Feature 8: Employee Talent Pipeline

### Concept
Goodwill affects the **quality of employees** at NPC studios. High‑goodwill studios attract and retain better talent; low‑goodwill studios lose their best people and can't replace them. This creates a realistic feedback loop.

### Why
Currently only `stars` (0–100) represents studio quality, and only the crisis mechanic reduces it. A richer talent system makes goodwill consequential beyond just firm value.

### Implementation

**Monthly check (`OnMonthChanged`):**
```csharp
private void ApplyTalentEffects(publisherScript studio, StudioState state) {
    if (studio.isPlayer) return;

    characterScript[] employees = FindEmployeesForStudio(studio);
    if (employees == null || employees.Length == 0) return;

    if (state.Label == "In Crisis") {
        // Brain drain: 1–3 skill loss on 1–2 random employees each month
        for (int i = 0; i < Mathf.Min(2, employees.Length); i++) {
            var emp = employees[UnityEngine.Random.Range(0, employees.Length)];
            ReduceEmployeeSkill(emp, UnityEngine.Random.Range(1, 4));
        }

        // Chance of key employee leaving to labour market
        if (UnityEngine.Random.Range(0, 100) < 10 && state.MonthsInCrisis >= 3) {
            RemoveBestEmployee(employees, studio);
            var gui = FindObjectOfType<GUI_Main>();
            if (gui != null)
                gui.CreateTopNewsInfo($"{studio.GetName()} loses key talent as crisis deepens.");
        }
    }

    if (state.Label == "Rising" || state.Label == "Commercial Powerhouse") {
        // Attract better talent: small skill gains
        if (UnityEngine.Random.Range(0, 100) < 15) {
            var emp = employees[UnityEngine.Random.Range(0, employees.Length)];
            BoostEmployeeSkill(emp, UnityEngine.Random.Range(1, 3));
        }
    }
}

private characterScript[] FindEmployeesForStudio(publisherScript studio) {
    // Iterate mS_.arrayCharactersScripts, match by studio.myID
}
```

### Labour market integration
- Employees fired from failing studios appear in the **Arbeitsmarkt** (labour market), hireable by the player.
- High‑goodwill studios increase the average quality of new NPC‑generated employees ("graduates want to work there").

### Config
```ini
EnableTalentPipeline = true
CrisisSkillDrainPerMonth = 2       # avg skill points lost per month
RisingSkillGainPerMonth = 1        # avg skill points gained per month
KeyEmployeeDepartureChance = 10    # % per month in crisis
```

---

## Feature 9: Financial Distress & Strategic Responses

### Concept
Studios at very low goodwill / firmenwert should behave differently — fire‑sell IPs, seek desperate deals, downsize, or eventually go bankrupt. High‑goodwill studios should expand, acquire, and invest.

### Why
This makes the game economy dynamic. Failing studios exit gracefully rather than persisting as zombies. Successful ones grow into threats or acquisition targets.

### Implementation

**Monthly financial‑distress check:**
```csharp
private void CheckFinancialHealth(publisherScript studio, StudioState state) {
    long value = studio.firmenwert;
    if (state.BaseValue <= 0) return;
    float ratio = (float)value / state.BaseValue;

    if (ratio <= 0.3f && state.MonthsInCrisis >= 3) {
        // DISTRESSED
        // 1. Put IPs on the market at fire‑sale prices
        MakeIPsAvailableForPurchase(studio, fireSale: true);

        // 2. Increase frequency of pubAngebot offers (desperate for deals)
        IncreasePubAngebotFrequency(studio);

        // 3. Force game‑size reduction (smaller budgets)
        ForceGameSizeReduction(studio);

        // 4. Bankruptcy risk after 12 months of distress
        if (state.MonthsInCrisis >= 12)
            TriggerBankruptcy(studio);
    }

    if (ratio >= 3.0f && state.Label == "Commercial Powerhouse") {
        // THRIVING
        // 1. Acquire weaker studios
        TryAcquireWeakerStudio(studio);

        // 2. Invest in larger, riskier games
        EncourageAmbition(studio);

        // 3. Open second team / subsidiary
        TryExpandStudio(studio);
    }
}
```

**Bankruptcy procedure:**
```csharp
private void TriggerBankruptcy(publisherScript studio) {
    state.IsDefunct = true;
    studio.firmenwert = 0;

    var gui = FindObjectOfType<GUI_Main>();
    if (gui != null)
        gui.CreateTopNewsInfo($"{studio.GetName()} has gone bankrupt! IPs available for acquisition.");

    ReleaseIPsToMarket(studio);     // all IPs become available
    ReleaseEmployees(studio);       // all employees → labour market
    // Studio stops developing new games
}
```

### Config
```ini
EnableFinancialDistress = true
DistressThreshold = 0.3            # ratio of current / base value
BankruptcyDelay = 12               # months of distress before bankruptcy
ThrivingThreshold = 3.0            # ratio for expansion behaviour
EnableBankruptcy = true            # master toggle
```

### Edge cases
- Player‑owned subsidiaries should **not** go bankrupt (different handling path).
- Multiplayer: bankruptcy events must sync across clients.
- A defunct studio should not keep generating games or appearing in menus.

---

## Feature 10: Industry‑Wide Reputation Network

### Concept
Studios develop **B2B reputations** based on who they partner with. A publisher known for fair deals attracts better developers. A developer known for delivering on time attracts better publishers.

### Why
This adds a network‑effect layer where partnerships have lasting consequences beyond the immediate deal. Reputation spreads through the industry graph.

### Implementation
```csharp
// In StudioState
public float ReputationScore;           // −100 to +100
public int   SuccessfulPartnerships;
public int   FailedPartnerships;
public List<int> PartnerHistory;        // studio IDs they've worked with

private void UpdateReputation(publisherScript studio, StudioState state,
                              gameScript game, publisherScript partner, string role) {
    if (partner == null) return;

    float delta = 0f;
    if (game.reviewTotal >= 75) {
        delta = 2f;
        state.SuccessfulPartnerships++;
    } else if (game.reviewTotal < 40) {
        delta = -3f;
        state.FailedPartnerships++;
    }

    state.ReputationScore = Mathf.Clamp(state.ReputationScore + delta, -100f, 100f);

    // Reputation diffusion: 30 % of the effect rubs off on the partner
    var partnerState = GetOrCreateState(partner.myID);
    if (partnerState != null) {
        partnerState.ReputationScore =
            Mathf.Clamp(partnerState.ReputationScore + delta * 0.3f, -100f, 100f);
    }

    if (!state.PartnerHistory.Contains(partner.myID))
        state.PartnerHistory.Add(partner.myID);
}

// Reputation improves the quality of future partners you attract
private float GetPartnershipQuality(StudioState state) {
    return 1.0f + (state.ReputationScore / 100f) * 0.15f;   // 0.85× → 1.15×
}
```

### Config
```ini
EnableReputationNetwork = true
ReputationDiffusionRate = 0.3    # how much partner reputation rubs off
ReputationGoodThreshold = 75     # review score for "good" partnership
ReputationBadThreshold = 40      # review score for "bad" partnership
```

---

## Feature 11: Platform Ecosystem Effects

### Concept
Studios that primarily develop for a specific console / platform build audience loyalty there. If the platform does well, so do they. Exclusivity compounds the effect.

### Why
This gives players a reason to care about the console ecosystem beyond their own hardware sales. Platform‑exclusive studios become distinct strategic assets.

### Implementation
```csharp
// In StudioState
public Dictionary<int, int> PlatformReleases;    // platformID → count
public int?                 PrimaryPlatform;

private float GetPlatformEffect(publisherScript studio, StudioState state, gameScript game) {
    if (game.gamePlatform == null || game.gamePlatform.Length == 0)
        return 1.0f;

    // Track platform usage
    foreach (int pId in game.gamePlatform) {
        if (pId < 0) continue;
        if (!state.PlatformReleases.ContainsKey(pId))
            state.PlatformReleases[pId] = 0;
        state.PlatformReleases[pId]++;
    }

    // Determine primary platform
    state.PrimaryPlatform = state.PlatformReleases
        .OrderByDescending(kvp => kvp.Value)
        .First().Key;

    // If they have a primary platform, check its health
    var platform = FindPlatformByID(state.PrimaryPlatform.Value);
    if (platform != null) {
        float health = platform.marktanteil / 100f;   // 0–1
        return 0.95f + health * 0.10f;                 // 0.95× → 1.05×
    }

    return 1.0f;
}

// Helper
private platformScript FindPlatformByID(int id) {
    // search mS_.arrayPlatformScripts
}
```

### Integration
Uses the existing `platformScript.marktanteil` (market share) to measure platform health.

### Config
```ini
EnablePlatformEcosystem = true
PlatformLoyaltyMin = 0.95        # floor multiplier
PlatformLoyaltyMax = 1.05        # ceiling multiplier
```

---

## Feature 12: Market Trends & Tech Cycles

### Concept
The overall market context affects studio goodwill. Studios in hot genres get tailwinds; those in declining genres face headwinds. Technology shifts (new hardware, new engine generations) favour early adopters.

### Why
The game already tracks genre popularity (`genres.genres_MARKT`). Tapping this data makes goodwill sensitive to the broader industry, which is more realistic.

### Implementation
```csharp
private float GetMarketTrendEffect(gameScript game, publisherScript studio) {
    var mS = game.mS_ ?? FindObjectOfType<mainScript>();
    if (mS == null) return 1.0f;

    // 1. Genre popularity
    var genresScript = FindObjectOfType<genres>();
    if (genresScript?.genres_MARKT == null) return 1.0f;

    float genrePop = genresScript.genres_MARKT[game.maingenre] / 100f;
    float genreEffect = 0.95f + genrePop * 0.10f;      // 0.95× → 1.05×

    // 2. New platform generation = innovation opportunity
    bool isNewGen = IsNextGenPlatform(game, mS.year);
    float platEffect = isNewGen ? 1.03f : 1.0f;

    // 3. Tech relevancy (game uses current‑gen engine features)
    bool usesModern = GameUsesCurrentGenTech(game, mS.year);
    float techEffect = usesModern ? 1.02f : 0.97f;

    return genreEffect * platEffect * techEffect;
}
```

### Config
```ini
EnableMarketTrends = true
GenreTrendStrength = 0.10        # max multiplier from genre popularity
TechCycleStrength = 0.03         # max effect from modern tech
```

---

## Feature 13: Goodwill‑Driven NPC Strategy Shifts

### Concept
NPC studios change their development strategy based on goodwill state. High‑goodwill studios take risks; low‑goodwill studios play it safe; crisis studios get desperate.

### Why
Currently all NPCs behave identically. This makes the world feel alive — you can infer a studio's situation from their behaviour.

### Implementation
This requires patching the game's NPC decision‑making logic. The exact hook depends on how NPCs choose their next game.

```csharp
[HarmonyPatch(typeof(publisherScript), nameof(publisherScript.DecideNextGame))]  // hypothetical
public static class NPCDecisionPatch {
    public static void Prefix(publisherScript __instance) {
        var state = DynamicStudioGoodwillPlugin.GetState(__instance.myID);
        if (state == null) return;

        switch (state.Label) {
            case "Commercial Powerhouse":
            case "Rising":
                // Aggressive: new IPs, bigger budgets, riskier genres
                __instance.aiRiskTolerance = 0.8f;
                __instance.aiInnovationBias = 0.7f;
                break;
            case "Declining":
            case "In Crisis":
                // Conservative: sequels, licensed IPs, smaller budgets
                __instance.aiRiskTolerance = 0.2f;
                __instance.aiInnovationBias = 0.2f;
                break;
            case "Overhyped":
                // Reckless: over‑invest, chase trends
                __instance.aiRiskTolerance = 0.9f;
                __instance.aiInnovationBias = 0.3f;
                break;
            default: // Stable, Cult, Comeback
                __instance.aiRiskTolerance = 0.5f;
                __instance.aiInnovationBias = 0.5f;
                break;
        }
    }
}
```

### Behaviour table

| Label         | Budget    | Genre Choice    | IP Strategy      | Risk  |
|---------------|-----------|-----------------|------------------|-------|
| Powerhouse    | Increase  | Core + expand   | New IPs          | High  |
| Rising        | Increase  | Attempt expand  | Mix              | Med   |
| Cult Favorite | Stable    | Niche / same    | Niche IPs        | Low–high |
| Stable        | Stable    | Current mix     | Sequels          | Med   |
| Declining     | Decrease  | Safe genres     | Licensed IPs     | Low   |
| In Crisis     | Decrease  | Contract work   | Fire‑sale IPs    | Min   |
| Overhyped     | Over‑invest| Any trend      | Any              | VHigh |
| Comeback      | Increase  | Proven formula  | Established IPs  | Calc  |

### Work required
- **Very high.** You need to reverse‑engineer how NPCs decide their next game (likely in `publisherScript` or a dedicated AI class).
- Start by searching for methods related to `NextGame`, `DecideGame`, `ChooseGenre`, `SetBudget`.

### Config
```ini
EnableNPCStrategyShifts = true
BaseRiskTolerance = 0.5          # default
HighRiskTolerance = 0.8
LowRiskTolerance = 0.2
RecklessRiskTolerance = 0.9
```

---

## Feature 14: Player Tools to Influence NPC Goodwill

### Concept
Let players actively manipulate NPC goodwill through cost‑ed actions.

### Why
Adds strategic depth and player agency over the industry landscape.

### Implementation actions

**1. Marketing Campaign** — spend money to boost an NPC's goodwill.
```csharp
void LaunchMarketingCampaign(publisherScript target, long budget) {
    float boost = Mathf.Log10(budget / 100000f) * 1.5f;
    boost = Mathf.Clamp(boost, 0f, 5f);
    target.firmenwert = (long)(target.firmenwert * (1f + boost / 100f));
    // News item
}
```

**2. Poach Talent** — degrade an NPC by hiring away key employees.
```csharp
void PoachTalent(publisherScript target, characterScript employee) {
    // Transfer employee to player studio
    // target loses skill points and −1 to −3 stars temporarily
}
```

**3. Smear Campaign** — negative PR against a competitor.
```csharp
void LaunchSmearCampaign(publisherScript target) {
    // Next 1–2 games from target get hidden review penalty
    // Target loses 2–5 % goodwill
}
```

**4. Bailout** — rescue a struggling studio.
```csharp
void BailoutStudio(publisherScript target, long amount) {
    target.firmenwert += amount;
    var state = GetOrCreateState(target.myID);
    state.TrendScore = Mathf.Max(state.TrendScore, 0f);
    state.Label = "Stable";
    state.MonthsInCrisis = 0;
}
```

**5. Acquisition Bid** — goodwill affects price and willingness.
```csharp
long GetAcquisitionPrice(publisherScript target) {
    var state = GetOrCreateState(target.myID);
    float multiplier = 1.0f + (state.TrendScore / 20f);   // 0.5× → 1.5×
    return (long)(target.firmenwert * multiplier);
}
```

### UI
These actions need menu entries — either a new sub‑menu or integration into existing studio interaction screens (e.g. in `Menu_MP_Tochterfirmen`).

### Config
```ini
EnablePlayerGoodwillActions = true
MarketingCampaignEfficiency = 1.5    # % boost per 100 k spent
BailoutMultiplier = 1.0              # goodwill per dollar bailed
SmearCampaignCost = 500000           # base cost
SmearCampaignEffect = 4.0            # % goodwill loss
```

---

## Feature 15: Prestige & Awards Feedback

### Concept
GOTY wins, award nominations, and other prestige events should significantly impact goodwill.

### Why
Awards are a major reputational driver in the real game industry. The game already has an awards system — tapping it adds prestige as a distinct reputational vector.

### Implementation
```csharp
[HarmonyPatch]  // Hook into Menu_Awards when winners are decided
public static void AwardGoodwillEffect() {
    // Find GOTY winner and category winners
    foreach (var winner in awardWinners) {
        var studio = FindStudioForGame(winner.game);
        if (studio == null || !CanAdjustStudio(studio)) continue;

        float bonus = winner.category switch {
            "GOTY"       => 15f,
            "BestGenre"  => 8f,
            "Nominee"    => 3f,
            _            => 0f
        };

        if (bonus > 0f) {
            studio.firmenwert = ApplyPercentChange(studio, state, studio.firmenwert, bonus);
            if (winner.category == "GOTY")
                SetTemporaryLabel(studio, "Game of the Year Winner", 52);  // 1‑year label
        }
    }
}
```

### StudioState additions
```csharp
public int AwardCount;   // lifetime awards
private bool IsPrestigeStudio => AwardCount >= 3;
```

### Config
```ini
EnableAwardEffects = true
GOTYBonus = 15.0               # % goodwill boost
AwardNominationBonus = 3.0     # % for nomination
PrestigeThreshold = 3          # awards needed for "Prestige" status
```

---

## Feature 16: Regional & Localized Reputation

### Concept
Studio goodwill can vary by region / market. A studio strong in North America might be unknown in Japan.

### Why
Adds world texture and gives players opportunities to acquire studios strong in regions where they're weak.

### Implementation
```csharp
// In StudioState
public Dictionary<int, long>   RegionalFirmenwert;    // regionID → value
public Dictionary<int, float>  RegionalTrendScore;    // regionID → trend

private void ApplyRegionalEval(gameScript game, publisherScript studio, StudioState state) {
    int region = DetermineGameTargetRegion(game);   // read game's target market

    if (!state.RegionalFirmenwert.ContainsKey(region)) {
        state.RegionalFirmenwert[region] = studio.firmenwert;
        state.RegionalTrendScore[region] = state.TrendScore;
    }

    // Use regional value for expected‑sales calculation
    long regionalValue = state.RegionalFirmenwert[region];
    // ... similar to GetExpectedSales but using regionalValue
}

// Displayed firmenwert: weighted average of regions
private long GetDisplayedFirmenwert(StudioState state) {
    if (state.RegionalFirmenwert == null || state.RegionalFirmenwert.Count == 0)
        return 0L;
    return (long)state.RegionalFirmenwert.Values.Average();
}
```

### Caveat
The game's regional tracking is limited. This feature is best implemented as a **simplification**: a studio is "strong in X region", which affects their attractiveness as a publishing partner for that region.

### Config
```ini
EnableRegionalReputation = true
```

---

## Feature 17: Technology & Innovation Leadership

### Concept
Studios that adopt new technologies early (engine features, hardware, gameplay systems) get an innovation bonus. Studios stuck on old tech get a penalty.

### Why
Tech leadership is a real differentiator. This gives an advantage to studios that invest in R&D and makes the engine / hardware research system more impactful.

### Implementation
```csharp
// In StudioState
public float TechInnovationScore;   // 0–100
public int   InnovationsAdopted;

private float GetTechEffect(gameScript game, publisherScript studio, StudioState state) {
    float effect = 1.0f;

    // Check engine‑feature usage
    if (game.engineScript_ != null) {
        int featureCount = CountEngineFeatures(game.engineScript_);
        float avgLevel = featureCount / 50f;   // normalise
        if (avgLevel >= 0.7f && state.TechInnovationScore < 100f) {
            state.TechInnovationScore = Mathf.Min(100f, state.TechInnovationScore + 5f);
        }
    }

    // Check if targeting new hardware
    if (IsNewHardware(game, FindObjectOfType<mainScript>().year)) {
        state.InnovationsAdopted++;
        effect += 0.03f;
    }

    // Innovation score adds a small expected‑review bump
    float innovationBonus = state.TechInnovationScore / 100f * 10f;   // 0–10 review points
    expectedReview += innovationBonus;

    return effect;
}
```

### Config
```ini
EnableTechInnovation = true
InnovationScorePerFeature = 5    # score gained per advanced feature
InnovationScoreMax = 100
InnovationReviewBonus = 10       # max review boost from innovation (points)
```

---

## Feature 18: Community Loyalty Buffer

### Concept
A studio's existing goodwill acts as a shock absorber. High‑goodwill studios get the benefit of the doubt from fans — one bad game hurts less. Low‑goodwill studios face confirmation bias — one bad game is catastrophic.

### Why
Asymmetric risk/reward. Building goodwill provides insurance against future mistakes, which is both realistic and strategic.

### Implementation
```csharp
private float ApplyCommunityBuffer(StudioState state, float totalPercent) {
    if (totalPercent >= 0f) return totalPercent;   // no buffer for positive impacts

    if (state.TrendScore > 2f) {
        // High goodwill reduces the sting
        float buffering = state.TrendScore / 20f;              // 0 → 0.5
        return totalPercent * (1f - buffering);
    }

    if (state.TrendScore < -3f) {
        // Low goodwill amplifies the damage
        float amplification = Mathf.Abs(state.TrendScore) / 25f;  // 0 → 0.4
        return totalPercent * (1f + amplification);
    }

    return totalPercent;
}
```

### Config
```ini
EnableCommunityBuffer = true
MaxBufferReduction = 0.5         # max 50 % reduction of negative impact at max goodwill
MaxAmplification = 0.4           # max 40 % amplification of negative impact at min goodwill
```

---

## Feature 19: Multi‑Market Exposure & Diversification

### Concept
Studios operating across many platforms and genres are more stable but less likely to have breakout hits. Specialised studios have higher volatility — bigger highs and lows.

### Why
Makes diversification a meaningful strategic choice. A broad studio is a safe investment; a niche specialist is high‑risk / high‑reward.

### Implementation
```csharp
// In StudioState
public float DiversificationScore;   // 0–1

private void RecalcDiversification(StudioState state) {
    if (state.GenreReleaseCount == null || state.GenreReleaseCount.Count < 1) {
        state.DiversificationScore = 0f;
        return;
    }

    int totalGames = state.GenreReleaseCount.Values.Sum();
    int uniqueGenres = state.GenreReleaseCount.Count;
    int uniquePlatforms = state.PlatformReleases?.Count ?? 0;

    float genreDiv = Mathf.Min(1f, uniqueGenres / 5f) * 0.5f;
    float platDiv  = Mathf.Min(1f, uniquePlatforms / 3f) * 0.5f;

    state.DiversificationScore = genreDiv + platDiv;   // 0–1
}

private float GetDiversificationEffect(StudioState state, float totalPercent) {
    float div = state.DiversificationScore;
    if (totalPercent > 0f)
        return totalPercent * (1f - div * 0.3f);   // less upside
    else
        return totalPercent * (1f + div * 0.3f);   // less downside (smoothing)
}
```

### Integration
The diversification score can feed into **Subsidiary 2.0** sale valuation — diversified studios get a more stable price; specialists have higher variance.

### Config
```ini
EnableDiversification = true
DiversificationStabilizer = 0.3  # how much diversification smooths impact (±30 % max)
```

---

## Feature 20: Console / Platform Manufacturer Goodwill (Separate Track)

### Concept
Platform holders (studios with `ownPlatform = true`) should have a **separate goodwill track** based on platform performance — hardware sales, exclusive quality, developer satisfaction.

### Why
Console manufacturers' reputation is fundamentally different from game developers'. Sony / Nintendo / Microsoft's value comes from their platform ecosystem, not individual game reviews.

### Implementation
```csharp
public class PlatformState {
    public int   StudioId;
    public long  PlatformValue;        // platform‑specific goodwill
    public long  HardwareSold;         // console sales
    public float ExclusiveQuality;     // average review of exclusives
    public float DeveloperSatisfaction; // how well they treat devs
    public string Label;
}

// Evaluated annually
private void EvaluatePlatformPerformance(platformScript platform, publisherScript studio) {
    float salesGrowth   = CalcAnnualSalesGrowth(platform);
    float exclusiveAvg  = CalcExclusiveAvg(platform);
    int   thirdPartyDevs = CountThirdPartyDevs(platform);

    float score = salesGrowth * 0.3f + exclusiveAvg * 0.4f + thirdPartyDevs * 0.3f;

    studio.firmenwert += (long)(score * 100000f);
}
```

### Separate subsystem
This is a **distinct module** from the main goodwill mod. It can use the same state file but should be structurally separate to avoid complexity.

### Config
```ini
EnablePlatformManufacturerGW = true
PlatformSalesWeight = 0.3
PlatformExclusiveWeight = 0.4
PlatformDevCountWeight = 0.3
```

---

## Summary Table

| # | Feature | Core Concept | Effort | Impact |
|---|---------|-------------|--------|--------|
| 1 | Continuous Evaluation | More evaluation points | Medium | High |
| 2 | Review Quality Separation | Critic vs user reviews | Low | Medium |
| 3 | Magnitude‑Scaled Events | Events scale with extremity | Low | High |
| 4 | Track Record Window | Expectations from history | Medium | High |
| 5 | Momentum Chains | Streak compounding | Low | Medium |
| 6 | Genre Specialization | Studio identity via genre | Medium | High |
| 7 | Franchise Stewardship | IP management reputation | Medium | High |
| 8 | Talent Pipeline | Employee quality feedback | High | Very High |
| 9 | Financial Distress | Bankruptcy & expansion | High | Very High |
| 10 | Reputation Network | B2B reputation graph | High | Medium |
| 11 | Platform Ecosystem | Platform loyalty effects | Medium | Medium |
| 12 | Market Trends | Genre/tech market effects | Low | Medium |
| 13 | NPC Strategy Shifts | Behaviour changes by goodwill | Very High | Very High |
| 14 | Player Tools | Active goodwill manipulation | High | High |
| 15 | Awards Feedback | GOTY/prestige impact | Low | Medium |
| 16 | Regional Reputation | Market‑specific goodwill | Medium | Low |
| 17 | Tech Innovation | Early adopter rewards | Medium | Medium |
| 18 | Community Buffer | Loyalty as shock absorber | Low | Medium |
| 19 | Diversification | Broad vs focused risk profile | Low | Low |
| 20 | Platform Manufacturer GW | Separate track for console makers | Very High | High |
