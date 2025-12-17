# Main Menu Setup Guide

This guide explains how to create a professional Main Menu using the `MainMenu.cs` script.

## 1. Create the UI Canvas
1. **Hierarchy** -> Right Click -> **UI** -> **Canvas**.
2. Select the **Canvas** object.
   - In **Canvas Scaler**, change *UI Scale Mode* to **Scale With Screen Size**.
   - Set *Reference Resolution* to **1920 x 1080**.

## 2. Create the Panels
You need two panels: one for the buttons, one for the options.

### A. Main Menu Panel
1. Right Click on **Canvas** -> **UI** -> **Panel**.
2. Rename it to `MainMenuPanel`.
3. (Optional) Remove the *Image* component or change the color to make it transparent/styled.

### B. Options Panel
1. Right Click on **Canvas** -> **UI** -> **Panel**.
2. Rename it to `OptionsPanel`.
3. Set its background color to something dark (e.g., Black with high alpha) to distinct it.
4. **Disable it** by unchecking the checkbox next to its name in the Inspector (so it starts hidden).

## 3. Create the Buttons

### In `MainMenuPanel`:
1. Right Click `MainMenuPanel` -> **UI** -> **Button - TextMeshPro** (or Legacy Button).
2. Create 3 Buttons:
   - **Start Button**
   - **Options Button**
   - **Exit Button**
3. Customize their text and position them nicely in the center.

### In `OptionsPanel`:
1. Right Click `OptionsPanel` -> **UI** -> **Button**.
2. Rename it to `BackButton`.
3. Place it in the corner or bottom.
4. Add desired option sliders/toggles here later (e.g., Volume).

## 4. Connect the Script
1. In the Hierarchy, create an **Empty GameObject**.
   - Rename it to `MainMenuManager`.
2. Drag the `MainMenu.cs` script onto it.
3. **Configure the Inspector**:
   - **First Level Scene Name**: Type the exact name of your game scene (e.g., `Level1`, `Game`, `Playground`).
   - **Main Menu Panel**: Drag your `MainMenuPanel` object here.
   - **Options Panel**: Drag your `OptionsPanel` object here.

## 5. Link Buttons to Script
Now, tell the buttons what to do when clicked.

### Start Button
1. Select **Start Button**.
2. Scroll to `On Click ()`.
3. Click **+**.
4. Drag `MainMenuManager` into the *Object* slot.
5. In the function dropdown: **MainMenu** -> `StartGame()`.

### Options Button
1. Select **Options Button**.
2. `On Click ()` -> **+**.
3. Drag `MainMenuManager`.
4. Function: **MainMenu** -> `OpenOptions()`.

### Exit Button
1. Select **Exit Button**.
2. `On Click ()` -> **+**.
3. Drag `MainMenuManager`.
4. Function: **MainMenu** -> `QuitGame()`.

### Back Button (in Options)
1. Select **Back Button**.
2. `On Click ()` -> **+**.
3. Drag `MainMenuManager`.
4. Function: **MainMenu** -> `ReturnToMainMenu()`.

## 6. Critical Final Step: Build Settings
For the "Start Game" button to work, Unity needs to know about your scenes.

1. Go to **File** -> **Build Settings**.
2. Open your Main Menu scene. Click **Add Open Scenes**.
3. Open your Game Level scene. Click **Add Open Scenes**.
4. Ensure your Game Level name matches the text you typed in the script's **First Level Scene Name**.

---
**Done!** Press Play to test navigation. The Exit button only closes the app in a real build, but the console will show "Quitting Game..." in the editor.
