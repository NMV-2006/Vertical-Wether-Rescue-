# Options Menu Setup Guide

This guide explains how to integrate the `OptionsMenu.cs` script into your Main Menu to control Volume and Sensitivity.

## 1. Prepare existing Options Panel
Assuming you have an **OptionsPanel** (as described in the Main Menu Guide):
1. Select the `OptionsPanel` (or the `MainMenuManager` if you prefer centralized scripts, but attaching to `OptionsPanel` is cleaner for this).
2. Add Component -> `OptionsMenu`.

## 2. Create UI Sliders
Inside the `OptionsPanel`, create two sliders.

### A. Volume Slider
1. Right Click `OptionsPanel` -> **UI** -> **Slider**.
2. Rename to `VolumeSlider`.
3. Select `VolumeSlider` in Inspector:
   - **Min Value**: `0`
   - **Max Value**: `1`
   - **Whole Numbers**: Unchecked
   - **Value**: `1` (Default)
4. Add a Text label nearby "Master Volume" so the user knows what it is.

### B. Sensitivity Slider
1. Right Click `OptionsPanel` -> **UI** -> **Slider**.
2. Rename to `SensitivitySlider`.
3. Select `SensitivitySlider` in Inspector:
   - **Min Value**: `0.5`
   - **Max Value**: `10` (Adjust based on preference)
   - **Whole Numbers**: Unchecked
   - **Value**: `2` (Default from Controller)
4. Add a Text label nearby "Camera Sensitivity".

## 3. Link Script Variables
1. Select the object with the `OptionsMenu` script (e.g., `OptionsPanel`).
2. Drag `VolumeSlider` from Hierarchy to the **Volume Slider** slot in the script.
3. Drag `SensitivitySlider` from Hierarchy to the **Sensitivity Slider** slot in the script.

## 4. Test It
1. Play the Scene.
2. Open Options.
3. Move Sliders.
   - **Volume**: Should affect game sound immediately (AudioListener).
   - **Sensitivity**: Will be saved. When you start the game (load Level1), the `ThirdPersonController` will read this value.

## Troubleshooting
- **Sensitivity not changing in-game?** 
  - Ensure `ThirdPersonController` is loading the value in its `Start()` method (I've already updated the script to do this!).
  - If you want *real-time* adjustment while playing (Pause Menu), ensure the `OptionsMenu` is in the same scene as the player and active.
