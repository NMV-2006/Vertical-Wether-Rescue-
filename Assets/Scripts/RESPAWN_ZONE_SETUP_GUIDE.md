# Respawn Zone Setup Guide

This guide will show you how to create zones that teleport the player back to a start point - perfect for death zones, fall areas, lava, water, or out-of-bounds areas!

## What is a Respawn Zone?

A **Respawn Zone** is an invisible trigger area that teleports the player back to a spawn point when they touch it. Think of it like falling off the map or touching lava - you get sent back to a checkpoint!

**Use cases:**
- 🕳️ Fall zones (below the playable area)
- 🔥 Lava or fire areas
- 💧 Deep water
- ⚠️ Out-of-bounds areas
- 🚫 Death zones

---

## Quick Setup (4 Steps)

### Step 1: Create a Spawn Point

First, you need to mark where the player should respawn.

1. **In Unity Hierarchy**, right-click and select:
   - `Create Empty`

2. **Rename** it to `SpawnPoint` or `Checkpoint`

3. **Position** it where you want the player to respawn
   - This is usually at the start of the level or at a checkpoint

4. **Tag it** (Important!):
   - Select the SpawnPoint
   - In the Inspector, click the **Tag** dropdown (top)
   - Select `Respawn` (or create a new tag called "Respawn")

---

### Step 2: Create the Respawn Zone

Now create the zone that will trigger the respawn.

1. **In Unity Hierarchy**, right-click and select:
   - `Create Empty` (or `3D Object → Cube` to visualize it)

2. **Rename** it to `RespawnZone` or `DeathZone`

3. **Position** it where players should NOT go:
   - Below the playable area (for fall zones)
   - On lava/water surfaces
   - At the edges of the map

---

### Step 3: Add a Trigger Collider

**CRITICAL:** The zone needs a trigger collider to detect the player!

1. **Select** the RespawnZone GameObject

2. **Add a collider**:
   - Click `Add Component`
   - Search for `Box Collider` (or Sphere/Capsule)
   - Click to add it

3. **Make it a trigger**:
   - In the collider component, **check "Is Trigger"**

4. **Adjust the size**:
   - Set the `Size` to cover the area you want (e.g., `100, 1, 100` for a large floor)
   - You can click `Edit Collider` to adjust it visually in the scene

---

### Step 4: Add the RespawnZone Script

1. **Select** the RespawnZone GameObject

2. **Click** `Add Component`

3. **Search** for `RespawnZone` and select it

4. **Configure** (optional):
   - The script will automatically find the spawn point by tag
   - Or drag your SpawnPoint into the `Respawn Point` slot

---

## Testing It!

1. **Press Play**

2. **Walk into the respawn zone**

3. **You should be teleported** back to the spawn point!

4. **Check the Console** for debug messages like:
   ```
   [RespawnZone] Player touched respawn zone! Teleporting to spawn point...
   ```

---

## Advanced Settings

### Respawn Settings

| Setting | Description | Default |
|---------|-------------|---------|
| **Respawn Point** | Where to teleport the player | Auto-find by tag |
| **Spawn Point Tag** | Tag to search for if no point set | `"Respawn"` |
| **Reset Velocity** | Stop player movement on respawn | `✓ Checked` |
| **Reset Rotation** | Reset player facing direction | `☐ Unchecked` |
| **Respawn Rotation** | Direction to face when respawning | `(0, 0, 0)` |

### Effects

| Setting | Description | Default |
|---------|-------------|---------|
| **Respawn Effect** | Particle effect at spawn location | None |
| **Respawn Sound** | Sound to play when respawning | None |
| **Respawn Delay** | Delay before teleporting (seconds) | `0` |
| **Fade Screen** | Fade to black during respawn | `☐` |
| **Fade Duration** | How long the fade lasts | `0.5s` |

---

## Common Setups

### Fall Zone (Below Map)

Perfect for catching players who fall off the edge.

**Setup:**
1. Create a large flat plane below your entire level
2. Add Box Collider with size `(1000, 1, 1000)`
3. Check "Is Trigger"
4. Add RespawnZone script

**Settings:**
```
Reset Velocity: ✓ Checked
Reset Rotation: ☐ Unchecked
Respawn Delay: 0 seconds
```

---

### Lava/Water Zone

For dangerous areas that kill the player.

**Setup:**
1. Create a zone on top of lava/water surface
2. Add Box Collider matching the surface area
3. Check "Is Trigger"
4. Add RespawnZone script

**Settings:**
```
Reset Velocity: ✓ Checked
Respawn Delay: 0.5 seconds (brief delay)
Respawn Effect: Fire/splash particles
Respawn Sound: Death/splash sound
```

---

### Checkpoint System

Multiple spawn points that update as you progress.

**Setup:**
1. Create multiple spawn points throughout the level
2. Tag them all as "Respawn" or "Checkpoint"
3. Create a respawn zone below the entire level
4. Use a script to change which spawn point is active

**Code Example:**
```csharp
// In another script:
RespawnZone zone = FindObjectOfType<RespawnZone>();
zone.SetRespawnPoint(newCheckpoint.transform);
```

---

## Adding Visual/Audio Effects

### Add Particle Effects (Respawn Visual)

1. **Create a Particle System**:
   - Right-click in Hierarchy → `Effects → Particle System`
   - Position it at your spawn point

2. **Configure the particles**:
   - Set `Looping` to `OFF`
   - Set `Play On Awake` to `OFF`
   - Adjust colors/size to match your theme

3. **Assign to RespawnZone**:
   - Select the RespawnZone GameObject
   - Drag the Particle System into the `Respawn Effect` slot

---

### Add Sound Effects (Respawn Audio)

1. **Add an Audio Source**:
   - Select RespawnZone (or SpawnPoint)
   - `Add Component → Audio Source`

2. **Configure it**:
   - Assign a respawn sound clip
   - Set `Play On Awake` to `OFF`
   - Set `Loop` to `OFF`

3. **Assign to RespawnZone**:
   - In the RespawnZone script, drag the Audio Source into the `Respawn Sound` slot

---

## Troubleshooting

### Problem: Nothing happens when I touch the zone

**Solutions:**

1. **Check the collider**:
   - Make sure the zone has a collider
   - Make sure "Is Trigger" is checked
   - Make sure the collider is big enough

2. **Check the spawn point**:
   - Make sure you have a spawn point in the scene
   - Make sure it's tagged as "Respawn"
   - Or drag it into the `Respawn Point` slot

3. **Check the Console**:
   - You should see `[RespawnZone] Player touched respawn zone!`
   - If you don't see this, the player isn't being detected

4. **Check the player**:
   - Player must have `ThirdPersonController` script
   - Player must have a collider (not trigger)

---

### Problem: Player gets stuck or falls through floor after respawn

**Solution:**
1. **Position spawn point higher**:
   - Move the spawn point slightly above the ground (0.5-1 unit)
   
2. **Check ground colliders**:
   - Make sure the ground has a collider where you're spawning

---

### Problem: Player respawns but keeps falling

**Solution:**
- Make sure `Reset Velocity` is checked
- This stops the player's downward momentum

---

### Problem: Can't find spawn point by tag

**Solution:**
1. **Check the tag**:
   - Select your spawn point
   - Make sure it's tagged as "Respawn"

2. **Or assign manually**:
   - Drag the spawn point into the `Respawn Point` slot

---

## Creative Ideas

### 1. **Multiple Checkpoints**
Create spawn points throughout your level that activate as the player reaches them.

### 2. **Timed Respawn**
Set `Respawn Delay` to 2-3 seconds to create a "death" feeling.

### 3. **Respawn with Effects**
Add particle effects and sounds to make respawning feel impactful.

### 4. **Damage Zones**
Modify the script to deal damage instead of instant respawn (for advanced users).

### 5. **Moving Spawn Points**
Attach spawn points to moving platforms for dynamic checkpoints.

---

## Tips & Best Practices

1. **Large Fall Zone**: Create one huge respawn zone below your entire level to catch all falls

2. **Visual Feedback**: Add a brief screen fade or particle effect so respawning doesn't feel jarring

3. **Sound Design**: Use a "whoosh" or "teleport" sound to make respawning feel intentional

4. **Spawn Point Height**: Position spawn points slightly above the ground (0.5 units) to prevent falling through

5. **Multiple Zones**: You can have multiple respawn zones that all point to the same spawn point

6. **Debug Logs**: Enable `Show Debug Logs` during development to see when respawns trigger

---

## Gizmo Visualization

When you select the RespawnZone in the editor:
- **Red transparent box** = The respawn zone area
- **Green line** = Points to the spawn point
- **Cyan sphere** = The spawn point location

This helps you visualize where players will be teleported!

---

## Checklist Before Testing

- [ ] RespawnZone has a **Collider** component
- [ ] Collider has **"Is Trigger"** checked
- [ ] **Spawn Point** exists in the scene
- [ ] Spawn Point is **tagged as "Respawn"** (or assigned manually)
- [ ] **Reset Velocity** is checked
- [ ] Player has **ThirdPersonController** script
- [ ] **Show Debug Logs** is enabled (for troubleshooting)

---

## Quick Reference: Common Configurations

| Zone Type | Size | Position | Delay | Reset Velocity |
|-----------|------|----------|-------|----------------|
| **Fall Zone** | `(1000, 1, 1000)` | Below map | `0s` | `✓` |
| **Lava** | Match surface | On lava | `0.5s` | `✓` |
| **Water** | Match surface | On water | `0.3s` | `✓` |
| **Out of Bounds** | Surround map | Edges | `0s` | `✓` |

---

Good luck creating your respawn system! 🎮✨
