---
name: performance-guardian
description: Ensures BepInEx/Unity mods never tank FPS, leak memory, or cause GC spikes. Use whenever writing or reviewing mod code that touches Update loops, Harmony patches, component lookups, coroutines, collections, UI refresh, or event subscriptions. Enforces zero-allocation hot paths, strict cache discipline, and guaranteed memory cleanup. If frame rate or memory could be affected, load this skill.
---

# Performance Guardian

The mod must be invisible to the frame budget. Players should never know it is running.

## The Three Rules Before Writing Any Code

1. **Is this in a hot path?** (Called every frame, every Update, or on every game tick?) If yes, the rules below are non-negotiable.
2. **Does this allocate?** (new, LINQ, string interpolation, boxing?) If yes, move it out of the hot path.
3. **Does this hold a reference to a Unity object?** If yes, it must be clearable and you must clear it on destroy/unload.

---

## Caching

### GetComponent — cache it, always

`GetComponent<T>()` does a type search through the component list. Every call costs CPU. Cache the result on first access, never call it in Update or in any method called frequently.

```csharp
// WRONG — called every frame
void Update() {
    GetComponent<MyManager>().DoThing();
}

// CORRECT — cached once
private MyManager _manager;
void Awake() {
    _manager = GetComponent<MyManager>();
}
void Update() {
    _manager?.DoThing();
}
```

### GameObject.Find / FindObjectOfType — ban from hot paths

These scan every active GameObject in the scene. Even one call per second is too many on a large save file.

```csharp
// BANNED in Update, coroutines with short delays, or Harmony patches on frequent methods
var obj = GameObject.Find("GameManager"); // never

// CORRECT — lazy cache with null check
private GameManager _gameManagerCache;
private GameManager GameMgr => _gameManagerCache ??= FindObjectOfType<GameManager>();
```

Use the `??=` lazy init pattern. Once cached, it never searches again. Null the cache in `OnDestroy`.

### Transform — cache if accessed repeatedly

```csharp
// Slightly cheaper but still — cache if called hundreds of times per frame
private Transform _t;
void Awake() { _t = transform; }
```

### Dictionary over List for ID lookups

Never call `list.Contains()` or `list.Find()` in a hot path. `O(n)` on a list of 200 publishers runs 200 comparisons per call.

```csharp
// WRONG
bool isProcessed = processedGames.Any(g => g.iD == id); // LINQ + O(n)

// CORRECT
bool isProcessed = processedGameIds.Contains(id); // HashSet, O(1), zero alloc
```

---

## Harmony Patch Overhead

Every Harmony patch adds a function call overhead to the method it patches. The more frequently the patched method runs, the more expensive your patch is.

### Never patch high-frequency methods

Methods called every frame or every Update tick are the worst patch targets. If you must patch them, your patch body must be nearly free.

```csharp
// DANGER — Update() is called 60+ times per second
[HarmonyPatch(typeof(SomeManager), "Update")]
static void Postfix(SomeManager __instance) {
    // Even an empty postfix adds overhead 60x/sec
    // A postfix with a dictionary lookup = real cost
}

// PREFER — patch the event/callback that fires once, not the loop that checks for it
[HarmonyPatch(typeof(SomeManager), "OnGameReleased")]
static void Postfix(GameData game) { ... }
```

### Early exit is free CPU

The fastest code is code that doesn't run. Always add guard clauses at the top of every patch:

```csharp
static void Postfix(SomeManager __instance, GameData game) {
    if (__instance == null) return;
    if (game == null) return;
    if (processedIds.Contains(game.iD)) return; // already handled, skip everything
    // ... actual work
}
```

### Transpilers are the most expensive patch type

They inject IL instructions into the method's body, increasing its size and blocking inlining by the JIT. Only use when Prefix/Postfix literally cannot achieve the result.

---

## Allocation and GC Pressure

The Unity garbage collector runs on the main thread. A GC spike causes a visible frame hitch. The goal is zero allocations in any hot path.

### LINQ — banned in hot paths

Every LINQ method (`Where`, `Select`, `ToList`, `Any`, `First`, `OrderBy`) allocates at least one enumerator object. In a method called every frame, this is a GC spike factory.

```csharp
// WRONG — allocates an enumerator + possibly a list every call
var hits = games.Where(g => g.iD > 100).ToList();

// CORRECT — pre-filter into a cached structure, or loop manually
foreach (var g in games) {
    if (g.iD > 100) ProcessGame(g);
}
```

**The only safe place for LINQ is:**
- One-time initialization code (Awake, Start, first load)
- Code explicitly triggered by player action (not called every frame)

### String operations — allocate, always

`string.Format()`, `$"interpolation"`, and `+` concatenation all produce new string objects.

```csharp
// WRONG in hot path
Debug.Log($"Processing game {game.iD} for publisher {publisher.name}"); // allocates

// CORRECT — gate log calls
if (DebugMode) Debug.Log($"...");

// CORRECT for repeated assembly — use StringBuilder, reuse it
private static readonly StringBuilder _sb = new StringBuilder();
_sb.Clear();
_sb.Append("Processing: ").Append(game.iD);
var result = _sb.ToString(); // one alloc, not three
```

### Boxing — invisible allocations

Value types (`int`, `float`, `bool`, `enum`) boxed into `object` allocate on the heap.

```csharp
// WRONG — enum comparison via object causes boxing in some Unity versions
object state = ContractType.Premium;
if (state.Equals(ContractType.Premium)) ... // boxes twice

// CORRECT
if (contract.Type == ContractType.Premium) ... // no alloc
```

Also watch for: `Dictionary<int, object>`, calling interface methods on structs, and passing structs to methods that take `object`.

### Collections — pre-allocate, don't grow

Every time a `List<T>` or `Dictionary<K,V>` exceeds its capacity it allocates a new backing array and copies. Initialize with a known capacity:

```csharp
// WRONG — starts at 4, grows and copies repeatedly
var list = new List<GameData>();

// CORRECT — pre-allocate to expected size
var list = new List<GameData>(64);
var dict = new Dictionary<int, PublisherData>(32);
```

---

## Update() and Polling

### Never poll in Update when an event exists

Every `if` check inside `Update()` runs 60+ times per second forever, even when nothing is happening.

```csharp
// WRONG — burning CPU checking a condition that changes rarely
void Update() {
    if (_someCondition != _lastCondition) {
        HandleChange();
        _lastCondition = _someCondition;
    }
}

// CORRECT — subscribe to the event that fires when the condition actually changes
void OnEnable()  { GameEvents.OnConditionChanged += HandleChange; }
void OnDisable() { GameEvents.OnConditionChanged -= HandleChange; }
```

### If you must poll, use a coroutine with a delay

```csharp
// WRONG — checks every frame
void Update() { if (ShouldDoThing()) DoThing(); }

// CORRECT — checks 4 times per second maximum
private IEnumerator PollCoroutine() {
    while (true) {
        if (ShouldDoThing()) DoThing();
        yield return new WaitForSeconds(0.25f);
    }
}
```

### Cache WaitForSeconds — coroutine allocations

`new WaitForSeconds(x)` allocates every time it is created inside the coroutine body. Cache it.

```csharp
// WRONG — allocates a new WaitForSeconds object on every iteration
while (true) {
    yield return new WaitForSeconds(0.25f);
}

// CORRECT — one allocation total
private static readonly WaitForSeconds _quarterSecond = new WaitForSeconds(0.25f);

while (true) {
    yield return _quarterSecond;
}
```

---

## Memory Leaks

### The Four Leak Sources

1. **Event listeners not unsubscribed** — the most common mod leak.
2. **Coroutines not stopped on destroy** — coroutine holds a reference to its owner forever.
3. **Static fields holding Unity objects** — static references never go out of scope; the Unity object they point to gets destroyed, leaving a phantom reference.
4. **Collections that grow and never shrink** — a `List` or `Dictionary` that entries are added to but never pruned over a long game session.

### Event listeners — always paired

```csharp
void OnEnable() {
    GameEvents.OnGameReleased    += HandleRelease;
    GameEvents.OnPublisherUpdate += HandlePublisher;
}

void OnDisable() {
    GameEvents.OnGameReleased    -= HandleRelease;
    GameEvents.OnPublisherUpdate -= HandlePublisher;
}
```

Write `OnDisable`/`OnDestroy` the moment you write `OnEnable`. Never let them drift apart.

### UI button listeners — always RemoveAllListeners first

```csharp
button.onClick.RemoveAllListeners(); // clear previous subscriptions
button.onClick.AddListener(OnClick);
```

Failing to clear UI listeners is the most common source of duplicate callbacks and cumulative memory growth.

### Coroutines — stop them on destroy

```csharp
private Coroutine _pollRoutine;

void OnEnable()  { _pollRoutine = StartCoroutine(PollLoop()); }
void OnDisable() {
    if (_pollRoutine != null) {
        StopCoroutine(_pollRoutine);
        _pollRoutine = null;
    }
}
```

### Static references — null them on destroy

```csharp
public static MyManager Instance;

void OnDestroy() {
    Instance = null; // release the static reference
    // null all other caches
    _gameManagerCache = null;
    _publisherCache = null;
}
```

### Collections — prune stale entries

HashSets and dictionaries tracking game/publisher IDs will grow over a very long session. On each new game start, clear sets that are no longer valid:

```csharp
void OnNewGameStarted() {
    processedReleaseGames.Clear();
    processedHitGames.Clear();
    // etc. — re-arm for new session
}
```

---

## Harmony Patch Cache Pattern

If your patch needs data that requires a lookup (finding a manager, resolving a publisher by ID), cache it:

```csharp
[HarmonyPatch(typeof(GameManager), "ReleaseGame")]
static class ReleaseGamePatch {
    private static MyRelationshipManager _relManager;

    static void Postfix(GameData game) {
        // lazy cache — Find runs once, never again
        _relManager ??= UnityEngine.Object.FindObjectOfType<MyRelationshipManager>();
        if (_relManager == null || game == null) return;
        _relManager.OnGameReleased(game);
    }
}
```

---

## Performance Review Checklist

Run through this before shipping any feature:

### Hot path audit
- [ ] No `GetComponent` inside `Update`, coroutines with < 1s delay, or frequently-patched methods
- [ ] No `GameObject.Find` / `FindObjectOfType` outside initialization
- [ ] No LINQ (`Where`, `Select`, `ToList`, `Any`, `First`) in any method called > once per second
- [ ] No `new WaitForSeconds` inside a coroutine loop — cached as `static readonly`
- [ ] No string interpolation or `Debug.Log` outside a `if (DebugMode)` gate

### Harmony audit
- [ ] No patches on `Update`, `LateUpdate`, or `FixedUpdate` without near-zero patch bodies
- [ ] Every patch starts with null guard + early exit
- [ ] No Transpiler patches where a Postfix could work

### Memory audit
- [ ] Every `OnEnable` subscription has a matching `OnDisable` unsubscription
- [ ] Every `StartCoroutine` has a matching `StopCoroutine` in `OnDisable` or `OnDestroy`
- [ ] Every `button.onClick.AddListener` is preceded by `RemoveAllListeners`
- [ ] All static fields holding Unity objects are nulled in `OnDestroy`
- [ ] All collections have a clear path to being pruned (new game, scene unload, etc.)

### Allocation audit
- [ ] All `List<T>` / `Dictionary<K,V>` initialized with a capacity estimate
- [ ] No per-frame allocations (verified by reading Unity Profiler's GC Alloc column)
- [ ] No enum boxing in hot paths

---

## Profiling: How to Confirm Performance

Use Unity Profiler (Window > Analysis > Profiler) attached to the game process:

1. Look at the **GC Alloc** column — any value > 0 in a method called every frame is a problem.
2. Look at **CPU ms** — your entire mod should not appear in the top 10 hottest methods.
3. Look at **frame time spikes** — a spike every N seconds is a coroutine or timer firing with an allocation.

The target: your mod adds < 0.1ms per frame to the CPU budget and zero GC alloc in the hot path.
