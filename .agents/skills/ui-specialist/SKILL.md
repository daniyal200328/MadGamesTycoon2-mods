---
name: ui-specialist
description: Builds and debugs Unity UI for BepInEx mods, including popup panels, progress bars, dynamic lists, and button wiring. Use when creating mod UI, fixing blank or stale UI elements, handling panel open/close/refresh lifecycle, or debugging NullReferenceExceptions in UI component code.
---

# UI Specialist

Mod UI must be stable across open/close cycles, always show fresh data, and never hold stale references to destroyed objects.

## Core Rules

- **Never cache UI element references across open/close cycles.** Re-fetch or re-create them on each open.
- **Null-check every component** before accessing it — UI elements can be destroyed without warning.
- **Rebuild dynamic lists on every open**, not once at Awake or Start.
- **Wire button events in code**, not in the Inspector — Inspector wiring is unreliable in mods.
- **Match UI lifecycle to data lifecycle** — if the data changes, the UI must reflect it immediately.

## Panel Open/Refresh Pattern

Always re-populate on open:

```csharp
void OnPanelOpen() {
    ClearDynamicList();
    PopulateDynamicList();
    RefreshProgressBar();
    RefreshLabels();
}
```

Never rely on data populated at Awake — it will be stale by the next open.

## Progress Bar

Calculate from live data, clamp, null-check:

```csharp
void RefreshProgressBar() {
    if (progressBar == null) return;
    float progress = Mathf.Clamp01(currentValue / (float)maxValue);
    progressBar.value = progress; // Slider
    // or progressBar.fillAmount = progress; // Image fill
}
```

Never store progress as a cached float — always recalculate from source data.

## Dynamic List

Clear before rebuild, use prefabs, null-check:

```csharp
void ClearDynamicList() {
    foreach (Transform child in listContainer) {
        Destroy(child.gameObject);
    }
}

void PopulateDynamicList() {
    if (listContainer == null || rowPrefab == null) return;
    foreach (var item in dataSource) {
        var row = Instantiate(rowPrefab, listContainer);
        row.GetComponent<RowController>()?.Init(item);
    }
}
```

## Button Event Wiring

Always remove listeners before adding to prevent duplicate callbacks:

```csharp
confirmButton.onClick.RemoveAllListeners();
confirmButton.onClick.AddListener(OnConfirmClicked);
```

Do this on every open, not just once at initialization.

## Panel Lifecycle

| Event | What to do |
|---|---|
| **Open** | Fetch fresh data → clear UI → rebuild UI → wire events |
| **Refresh** | Same as open, triggered on data change |
| **Close** | Unsubscribe events, optionally destroy dynamic content |
| **Destroy** | Null all cached references, unsubscribe all events |

## Debugging Blank UI

Work through this checklist in order:

1. Is the panel `GameObject` active? (`SetActive(true)`)
2. Is data available before `OnOpen` fires? (check ordering)
3. Is the list container null? (reference lost after a scene reload)
4. Are layout groups collapsing content? (check `preferredHeight` and `minHeight`)
5. Are any parent `GameObject`s inactive?
6. Are progress bar values correctly clamped between 0 and 1?
7. Are event listeners wired after the UI is built, not before?

## Memory Safety

Unsubscribe from all events when the panel closes or is destroyed:

```csharp
void OnDestroy() {
    GameEvents.OnGameReleased -= HandleGameReleased;
    confirmButton?.onClick.RemoveAllListeners();
}
```

Failing to do this causes phantom event callbacks to fire on destroyed UI objects, producing NullReferenceExceptions and difficult-to-trace bugs.
