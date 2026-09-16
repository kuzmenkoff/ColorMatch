# Color Match <!-- omit in toc -->

A small casual mobile game (Android, Unity). Colored figures fall from the top; the player drags a basket that periodically changes color and must catch figures whose color matches the basket while avoiding the rest, against the clock.

## Table of contents

- [Table of contents](#table-of-contents)
- [Gameplay](#gameplay)
- [Tech stack](#tech-stack)
- [Architecture](#architecture)
- [Project structure](#project-structure)
- [Key technical decisions](#key-technical-decisions)

## Gameplay

- Figures of different colors and shapes fall from the top.
- The player **drags the basket** horizontally; every few seconds it switches to a random color.
- Catching a figure whose color **matches** the basket **adds** points; a **mismatch subtracts** points.
- The level is **timed**; difficulty **ramps up automatically** over time (faster falling, more frequent spawns).
- The **best score** is shown in the main menu and persists between runs.

## Tech stack

- **Unity 6.3 LTS** (6000.3.24f1)
- **VContainer** — dependency injection
- **LitMotion** — tweening (catch feedback)
- **New Input System** — pointer drag (mouse and touch from one code path)
- **TextMeshPro** — UI text
- **Unity Test Framework** (NUnit) + **GitHub Actions / GameCI** — CI running the unit tests

## Architecture

The game separates **logic** from **presentation** following the **Humble Object** pattern: Unity handles physics, rendering and input, while all game rules live in plain C# services with no engine dependency. The split is enforced by assembly definitions — the Core assembly does not reference `UnityEngine` at all.

| Layer | Responsibility |
|-------|----------------|
| **Core** (`ColorMatch.Core`) | Rules, scoring, difficulty curve, timer, color cycling, spawn timing, session orchestration, state machine. |
| **Presentation** (`ColorMatch.Presentation`) | MonoBehaviours: input, basket, falling figures, pooling, UI, audio, feedback. Implements the Core's abstractions and wires everything in a VContainer composition root. |
| **Tests** (`ColorMatch.Tests`) | EditMode unit tests over the Core — run without opening a scene or the engine. |

Data flows one way: **commands go up** into the session (`ReportCatch`, `StartGame`), **events come down** to the views (`ScoreChanged`, `TimeChanged`, `FigureRequested`, `FigureCaught`, `Ended`). The Core never calls the view directly.

## Project structure

```
Assets/_Project/
├── Art/                  # Sprites (basket, figures)
├── Audio/                # Catch SFX
├── Fonts/                # TMP font asset
├── Prefabs/              # Figure prefab
├── ScriptableObjects/    # ColorPalette, GameConfig
├── Scenes/               # Game.unity
├── Settings/             # URP / Renderer2D assets
└── Scripts/
    ├── Core/             # ColorMatch.Core.asmdef
    │   ├── Config/       # ScoreConfig, DifficultyConfig
    │   ├── Enums/        # FigureColor, FigureShape, GameState
    │   ├── Model/        # FigureSpawnRequest
    │   ├── Services/     # rules, scoring, difficulty curve, timer,
    │   │                 #   basket color cycler, spawn director, session
    │   ├── StateMachine/ # GameStateMachine
    │   └── Storage/      # IHighScoreStorage
    ├── Presentation/     # ColorMatch.Presentation.asmdef
    │   ├── Config/       # ColorPalette, GameConfig (ScriptableObjects)
    │   ├── DI/           # GameLifetimeScope (VContainer composition root)
    │   ├── Gameplay/     # input, basket, figure, pool, spawner, flow, feedback
    │   ├── Services/     # UnityRandom, PlayerPrefsHighScoreStorage
    │   └── UI/           # UIController
    └── Tests/EditMode/   # ColorMatch.Tests.asmdef
```

## Key technical decisions

- **Engine-free Core.** The logic assembly sets `noEngineReferences`, so it cannot touch rendering or physics. This makes the rules unit-testable without the engine and keeps them running fast in CI.
- **Humble Object views.** Collision detection is idiomatic Unity (trigger colliders); the `BasketView` only reports a catch to the session, which applies the color-match rule and scoring. MonoBehaviours stay thin.
- **Auto-ramping difficulty.** Instead of a difficulty menu, a `DifficultyCurve` interpolates fall speed and spawn interval from start to end values over time.
- **Dependency injection.** A single `GameLifetimeScope` builds the whole Core graph from a `GameConfig` asset. Abstractions (`IRandom`, `IHighScoreStorage`) are swapped for fakes in tests.
- **Data-driven tuning.** Colors, scoring and difficulty live in ScriptableObjects, tunable in the Inspector without touching code.
- **Object pooling** for falling figures — no per-spawn allocation, no GC hitches on mobile.


