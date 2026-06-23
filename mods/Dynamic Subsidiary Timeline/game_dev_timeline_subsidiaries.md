# Subsidiary Game Development Timeline Formulas

This document details the mathematical models and formulas used by the **Dynamic Subsidiary Timeline** mod in *Mad Games Tycoon 2* to compute game development durations.

---

## The Core Equation

The final development duration (in weeks) is computed by multiplying a baseline duration by several factors:

$$FinalWeeks = BaseWeeks \times M_{stars} \times M_{speed} \times M_{platform} \times M_{type} \times M_{trend} \times M_{random}$$

The result is clamped between a size-specific floor and ceiling to preserve game balance.

---

## 1. Base Weeks ($BaseWeeks$)

The baseline weeks are determined by the game's size category (`game.gameSize`):

| Index | Size | Base Duration | Floors (Min) | Ceilings (Max) |
|---|---|---|---|---|
| **0** | B | $12\text{ weeks}$ | $4\text{ weeks}$ | $52\text{ weeks}$ |
| **1** | B+ | $19\text{ weeks}$ | $6\text{ weeks}$ | $78\text{ weeks}$ |
| **2** | A | $32\text{ weeks}$ | $10\text{ weeks}$ | $130\text{ weeks}$ |
| **3** | AA | $60\text{ weeks}$ | $20\text{ weeks}$ | $260\text{ weeks}$ |
| **4** | AAA | $113\text{ weeks}$ | $40\text{ weeks}$ | $520\text{ weeks}$ |
| **5** | AAAA | $185\text{ weeks}$ | $80\text{ weeks}$ | $1040\text{ weeks}$ |

---

## 2. Studio Stars Multiplier ($M_{stars}$)

The star rating reflects the capability and department maturity of the subsidiary:
* Derived using `studio.GetStarsAmount()` (range: $0\text{--}5$).
* A quadratic curve penalizes lower-tier studios attempting large projects:

$$M_{stars} = 1.0 + (5 - StarsAmount)^2 \times 0.15$$

* **5 Stars**: $1.00\times$ (Peak capability, zero penalty)
* **4 Stars**: $1.15\times$
* **3 Stars**: $1.60\times$
* **2 Stars**: $2.35\times$
* **1 Star**: $3.40\times$ (Very high friction for small studios)
* **0 Stars**: $4.75\times$ (Initial startup / garage setup)

---

## 3. Normalized Development Speed Multiplier ($M_{speed}$)

Dev speed scales differ between player-created organic studios and AI-acquired studios:
* **Organic Subsidiary**: $S_{norm} = speed$ (range: $0\text{--}10$)
* **AI / Acquired Subsidiary**: $S_{norm} = speed \times 2.5$ (range: $0\text{--}4$ in vanilla)
* The multiplier applies a linear scaling effect:

$$M_{speed} = 1.30 - 0.05 \times S_{norm}$$

* **Speed 10**: $0.80\times$ (Fastest)
* **Speed 5**: $1.05\times$ (Average)
* **Speed 0**: $1.30\times$ (Slowest)

---

## 4. Platform Count Multiplier ($M_{platform}$)

Developing for multiple platforms introduces overhead due to compilation targets, SDK management, cross-platform inputs, and QA testing:

$$M_{platform} = 1.0 + 0.10 \times (PlatformCount - 1)$$

* **1 Platform**: $1.00\times$
* **2 Platforms**: $1.10\times$
* **3 Platforms**: $1.20\times$
* **4 Platforms**: $1.30\times$

---

## 5. Project Type Complexity Multiplier ($M_{type}$)

The type of game project modifies its duration. Reusing existing structures and lore significantly accelerates development:

* **New IP (Standard Game)**: $1.15\times$ (Requires new concept, lore, artwork, engine adjustments, and assets)
* **Spin-off**: $0.90\times$ (Reuses core engine and graphics assets, but requires new design focus)
* **Sequel (Nachfolger)**: $0.85\times$ (Benefits from asset pipelines, lore, and engine templates from the predecessor)
* **Remaster**: $0.45\times$ (Graphics overhaul, core code remains intact)
* **Port**: $0.25\times$ (Re-targeting compiled builds to secondary platforms)
* **MMO Add-on**: $0.35\times$ (Expansion content)
* **Standard Add-on**: $0.30\times$ (Minor assets and scenarios)
* **Standalone Add-on**: $0.40\times$
* **Contract Game**: $0.70\times$ (Fixed guidelines, zero iteration delays)
* **Budget / GOTY / Bundle**: $0.10\times$ (Instant repackaging compilations)

---

## 6. Studio Goodwill Trend Multiplier ($M_{trend}$)

Dynamic integration with the *Dynamic Studio Goodwill* mod represents the motivation, morale, and financial stability of the subsidiary:

* **Commercial Powerhouse**: $0.85\times$ (Streamlined workflow, high motivation)
* **Rising**: $0.92\times$
* **Stable**: $1.00\times$
* **Declining**: $1.15\times$
* **In Crisis**: $1.35\times$ (Key developer departures, workflow friction)
* *Fallback (Mod not loaded)*: $1.00\times$

---

## 7. Random Variance ($M_{random}$)

Simulates minor, unpredictable delays or early completions in real-world game development:
* Range: $[0.92\text{--}1.08]$ (+/- 8% variance)

---

## Verification Example

For a fully upgraded subsidiary (**5 Stars**, **10 Speed**) developing a brand-new **AAAA IP** on **1 Platform**:
1. Base = $185\text{ weeks}$
2. Multipliers:
   * $M_{stars} = 1.0$ (5 Stars)
   * $M_{speed} = 0.8$ (10 Speed)
   * $M_{platform} = 1.0$ (1 Platform)
   * $M_{type} = 1.15$ (New IP)
   * $M_{trend} = 1.0$ (Stable)
3. Calculation:
   $$Final = 185 \times 1.0 \times 0.8 \times 1.0 \times 1.15 \times 1.0 = \mathbf{170.2\text{ weeks}}$$
4. A sequel developed by the same studio:
   $$Final = 185 \times 1.0 \times 0.8 \times 1.0 \times 0.85 \times 1.0 = \mathbf{125.8\text{ weeks}}$$
5. A 1-Star, 0-Speed studio attempting the same new AAAA IP:
   * $M_{stars} = 1.0 + (5-1)^2 \times 0.15 = 3.40$
   * $M_{speed} = 1.30$
   * $M_{type} = 1.15$
   $$Final = 185 \times 3.40 \times 1.30 \times 1.15 = \mathbf{940.7\text{ weeks}}$$
