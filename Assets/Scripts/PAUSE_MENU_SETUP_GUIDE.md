# Pause Menu Setup Guide

This guide explains how to add a functional Pause Menu to your game level.

## 1. Create the UI Canvas (If not already present)
1. If you don't have a UI Canvas, right-click in Hierarchy -> **UI** -> **Canvas**.
2. Set **UI Scale Mode** to **Scale With Screen Size** (Resolution: 1920 x 1080).

## 2. Create the Pause Panels
We need a structure that has a "Pause Menu" and (optionally) an "Options Menu".

### A. Create the Main Container
1. Right-click Canvas -> **Create Empty**. Rename it `PauseSystem`.
2. Add the `PauseMenu` script to this object.

### B. Create the Pause Menu UI
1. Right-click `PauseSystem` -> **UI** -> **Panel**.
2. Rename it `PausePanel`.
3. Set the layout/color (e.g., black background with 0.8 alpha).
4. **Add Buttons**:
   - **Resume Button**: Unpauses game.
   - **Options Button**: Opens settings.
   - **Menu Button**: Goes to Main Menu.
   - **Quit Button**: Closes game.

### C. Create the Options Menu UI
1. You can reuse the prefabs/design from your Main Menu!
2. Right-click `PauseSystem` -> **UI** -> **Panel**.
3. Rename it `OptionsPanel`.
4. Add your sliders (Volume, Sensitivity) and the `OptionsMenu` script setup (if you want separate logic, but usually you just need the sliders here).
5. Add a **Back Button** inside `OptionsPanel`.
6. **Important**: Uncheck the checkbox for `OptionsPanel` so it starts **hidden**.
7. **Important**: Also uncheck `PausePanel` so it starts **hidden** (game starts unpaused).

## 3. Link Script References
1. Select `PauseSystem`.
2. **Pause Menu UI**: Drag the `PausePanel` object here.
3. **Options Menu UI**: Drag the `OptionsPanel` object here.
4. **Main Menu Scene Name**: Ensure this matches your menu scene name (e.g., "MainMenu").

## 4. Hook up Buttons
Select each button and add an **On Click()** event, dragging the `PauseSystem` object into the slot.

- **Resume Button**: Function -> `PauseMenu` -> `Resume()`
- **Options Button**: Function -> `PauseMenu` -> `OpenOptions()`
- **Menu Button**: Function -> `PauseMenu` -> `LoadMenu()`
- **Quit Button**: Function -> `PauseMenu` -> `QuitGame()`

### Inside Options Panel:
- **Back Button**: Function -> `PauseMenu` -> `CloseOptions()`
- **Sliders**: Set them up exactly like the Main Menu (using `OptionsMenu` script if you want to save/load settings there too).

## 5. Important Step
**I have automatically removed the Escape key logic from `ThirdPersonController.cs`** to prevent conflicts. 
Now, only the `PauseMenu` script listens for the **Escape** key.

## Testing
1. Play the game.
2. Press **Escape**. 
   - Game should freeze.
   - Cursor should appear.
   - Menu should show.
3. Click **Resume**.
   - Game continues.
   - Cursor disappears.
