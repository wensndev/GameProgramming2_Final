# Why Am I Alive

A 2D platformer built with MonoGame (.NET 9.0). You are infected. Reach the exit door before the infection consumes you — but how you deal with the enemies along the way determines your ending.

## How to Run

```
cd Game
dotnet run
```

Requires .NET 9.0 SDK and MonoGame 3.8.x (restored automatically via NuGet).

## Objective

Reach the locked **EXIT door** on the right side of the map. The door only unlocks once every enemy has been either **killed** or **cured**. Talk to the NPC first — they will give you information and unlock your cure attachment.

Your ending depends on your choices:
- **Good ending** — you cured more enemies than you killed
- **Bad ending** — you killed more enemies than you cured

## Controls

| Key | Action |
|-----|--------|
| Arrow Keys / WASD | Move left / right |
| Space / W / Up | Jump |
| Z / Left Ctrl | Shoot |
| Q | Toggle Kill / Cure mode (unlocked after NPC dialogue) |
| E / F | Talk to NPC (when nearby) |
| Tab | View collected notes |
| Escape / P | Pause |
| Enter / Space | Advance dialogue |

## Enemies

| Colour | Type | Behaviour |
|--------|------|-----------|
| Red | Normal enemy | Patrols back and forth |
| Dark red | Mutant enemy | Patrols and fires projectiles at you |
| Light green | Cured enemy | No longer hostile, stays in place |

## Notes

- There are paper notes scattered around the level (gold squares). Walk over them to collect. Press **Tab** to read them.
- Your **infection level** rises when you kill an enemy (+5%) and when you speak to the NPC (+15%). It drops when you cure an enemy (-10%). Watch the bar at the bottom of the screen.
- All visuals are drawn with coloured rectangles — no image assets are required to run the game.
