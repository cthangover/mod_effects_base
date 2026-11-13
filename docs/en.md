# Effects Engine — animated visual effects for scenes

Effects Engine lets a scenario play a short visual effect over a scene: a flash, smoke, sparks, a magic glow. An effect is a small Godot scene that a mod ships in its `effects/` folder; a scenario starts it by name, and it stays on screen until it finishes by itself or the dialog ends.

The engine also hosts continuous atmosphere profiles: leaves, fireflies, snow, dust and rain. Village Pack supplies the profiles used by RPG Plus; other mods can add profiles and emitter types. The same host works outside tile maps when given another coordinate projection.

## How an effect appears

1. A scenario line `effect <name>` runs.
2. Effects Engine finds the scene `effects/<name>.tscn` in the loaded mods.
3. The effect is placed over the scene's picture — above the background and its objects, below the dialog box — and plays.
4. When the dialog that started it ends, the effect is removed, unless it has already removed itself.

The scenario does not wait for the effect: the next line is shown at once.

## For authors: add an effect

Put the effect's scene in your mod:

```filestree
my_mod/
└── effects/
    └── sparks.tscn
```

Start it from a scenario:

```scenario
text "The lamp flickers."
effect sparks
text "Something moved in the dark."
end
```

**Mods must be folders.** Godot scenes can only be loaded from a mod installed as a folder. A mod distributed as a `.zip` archive has its effects skipped, with a warning in the log.

### Building blocks for the effect scene

Effects Engine provides three ways to animate frames inside your effect scene:

- **Frame set with cross-fade** — a node with the `EffectDataSet` script, a list of frame pictures and an `AnimationController` that fades from one frame to the next. Speed and the fade time are set in the inspector; the frames play once and the effect removes itself.
- **Sprite sheet in a shader** — `EffectAnimator` steps a `TextureRect` with a frame-sheet material through a set number of frames over a set duration.
- **Sprite sheet without a shader** — `EffectAnimNode` scrolls through a horizontal sprite sheet (frame width = sheet width / frame count) and removes itself at the end. It can also be created from code with `EffectAnimNode.SpawnOn(target, sheet, frameCount, seconds)`.

The frame set and the code-only sprite sheet stretch to fill the scene, so frames drawn for the scene's size line up with the background.

### A first effect without code

The simplest effect is a scene whose root is a `TextureRect` with a picture, stretched to the full screen — for example a semi-transparent glow. It appears with `effect <name>` and disappears when the dialog ends.

## If something goes wrong

1. **Nothing appears and the log says the effect was not found.** The file must be `effects/<name>.tscn` in a mod that is part of the game, and the name in the scenario must match.
2. **The effect works in development but not for players.** The mod was published as a `.zip`; publish it as a folder.
3. **The effect never goes away.** It stays until its dialog ends; end the dialog, or build the effect from the frame-set or sprite-sheet blocks, which remove themselves when they finish.

## Continuous atmosphere profiles

The engine also hosts continuous atmosphere profiles: leaves, fireflies, snow, dust and rain. Village Pack supplies the profiles used by RPG Plus; other mods can add profiles and emitter types. The same host works outside tile maps when given another coordinate projection.

```json
{
  "Kind": "atmosphere",
  "Items": [{
    "Id": "my_mod/ash",
    "Name": "my_mod/ash/name",
    "Emitters": [{
      "Type": "particles", "Shape": "flake", "Count": 80,
      "Color": "#a0a0a070", "Velocity": [0.1, 0.02, -0.4],
      "Height": [0.1, 5], "Size": [0.5, 1.2], "Sway": 0.2, "Seed": 17
    }]
  }]
}
```

Set a map's `Atmosphere` to the entity Id; the map editor lists all loaded profiles. `particles` supports
`flake`, `leaf`, `glow`, `dust` and `streak`; `ripples` draws projected rings in the host's named `Region`.
The tile host supplies `water`. Missing regions draw nothing. The Data Editor exposes the emitter JSON.

Velocity uses XYZ world units per second, with positive Z up. Height is a world-unit range; Size is a pixel-radius
range. `Pulse` controls periodic fading, `Trail` the streak duration. Counts are limited to 512 per emitter and
2048 per profile. Cosmetic phases are independent of gameplay random numbers.

For a new emitter type, depend on `effects_base>=0.1.0` and implement `IAtmosphereEmitterFactory` from
`Cthangover.Mods.Effects.Atmosphere`. `Id` becomes JSON `Type`; `Create` receives `AtmosphereEmitterInfo` plus
custom JSON in `ExtraFields` and returns `IAtmosphereEmitter`:

- `Advance(delta, context)` updates state once per host frame.
- `Draw(canvas, context)` draws without advancing state. Use `context.Project`, `Bounds` and optional `Regions`.
- `Dispose()` releases owned assets; resolver textures remain borrowed.

The mod pipeline discovers factories and removes them on unload. Duplicate type names, unknown types and invalid
ranges produce diagnostics. Hosts own emitter instances; scene exit, renderer replacement and profile changes
release them. Unchanged geometry edits retain animation state. The winter suite verifies a factory from another
mod assembly, custom fields, update/draw calls and exactly-once disposal.

For a non-tile scene, add an `AtmosphereView`, call `Configure(profileId, context)` with bounds and a
`Func<Vector3, Vector2>` projection, and call `Advance(delta)` every frame. `Release()` clears delegates and
owned emitter state before manual freeing; exiting the tree releases it too. There is no RPG dependency.
