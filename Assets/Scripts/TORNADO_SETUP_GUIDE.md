# Tornado Particle System Setup Guide

This guide will help you create a realistic tornado effect using Unity's Particle System and the provided `TornadoEffect.cs` script.

## 1. Create the Particle System
1. In the Hierarchy, right-click -> **Effects** -> **Particle System**.
2. Rename it to `Tornado`.
3. Reset its Transform (Position: 0, 0, 0) and Rotation (Rotation: -90, 0, 0 is default, set it to **0, 0, 0** so it points upwards if using "Cone" shape directed up, or keep -90 if "Cone" is directed along Z. Usually for a tornado on the ground, we want it vertical).
   - *Tip*: Set Rotation to **(-90, 0, 0)** so the default Cone emits upwards.

## 2. Configure Particle System Modules
Adjust the following modules in the Inspector:

### **Main Module** (Top Section)
- **Duration**: `5.00`
- **Looping**: `Checked`
- **Start Lifetime**: `3` to `5` (Longer lifetime = taller tornado)
- **Start Speed**: `0` (We want rotation, not linear speed initially)
- **Start Size**: `1` to `5` (Random between two constants) - or use a Curve to make them smaller at bottom.
- **Start Rotation**: `0` to `360` (Randomize particle initial angle)
- **Start Color**: Light Grey or slightly transparent white/brown (Dust color)
- **Gravity Modifier**: `0` or slightly negative (e.g., `-0.1`) to help them float up.
- **Simulation Space**: `World` (Important so particles don't move with the tornado root too rigidly) or `Local` (if you want the whole tornado to move as one unit). `Local` is usually easier for a moving tornado.

### **Emission**
- **Rate over Time**: `50` to `200` (More particles = denser tornado).

### **Shape**
- **Shape**: `Cone`
- **Angle**: `10` to `25` (Narrower angle = tighter funnel).
- **Radius**: `1.0` (Bottom width).
- **Position**: `0, 0, 0`.
- **Rotation**: `0, 0, 0`.

### **Velocity over Lifetime** (CRITICAL for the funnel shape)
- **Linear**: `Y: 5` (Upward speed).
- **Orbital**: `Z: 3` to `5` (This creates the spin!).
- **Radial**: `-1` to `-2` (Negative value pulls particles INWARD, keeping the funnel tight).

### **Color over Lifetime**
- **Top Bar (Alpha)**: Fade in at start (0 -> 1) and fade out at end (1 -> 0).
- **Bottom Bar (Color)**: Darker at the bottom (dirt), getting lighter/whiter at the top.

### **Size over Lifetime**
- Enable this.
- Curve: Start small implies the bottom of the funnel, grow larger over time.
- *Wait*, a Tornado is usually narrow at bottom, wide at top. Since particles move UP (Lifetime 0 -> End), making the curve *grow* (Line going up) produces the correct funnel shape.

### **Rotation over Lifetime**
- **Angular Velocity**: `45` to `90` (Makes individual particles spin, adding chaos).

## 3. Adding the Physics Script
1. Select your `Tornado` GameObject.
2. Drag and drop the `TornadoEffect.cs` script onto it.
3. Configure the **Tornado Physics** in the Inspector:
   - **Pull Radius**: `15` (How wide the suction area is).
   - **Pull Force**: `20` (Suction strength).
   - **Lift Force**: `10` (Updraft strength).
   - **Spin Force**: `50` (Swirling strength).

## 4. Testing
1. Place the Tornado in the scene.
2. Place some rigidbodies (crates, barrels) or your player nearby.
3. Press **Play**.
4. The particles should look like a funnel, and objects should get sucked in and spun around!

## 5. Advanced Visuals (Optional)
- **Texture**: Use a "Smoke" or "Cloud" texture instead of the default circle.
- **Trails**: Enable the **Trails** module for a more "windy" look.
- **Collision**: Enable **Collision** module (type: World) if you want particles to bounce off the ground.
