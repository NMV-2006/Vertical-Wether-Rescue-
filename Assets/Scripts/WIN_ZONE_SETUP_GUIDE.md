# Win Zone & Level Transition Setup Guide

This guide explains how to create a "Win Zone" that shows a victory screen and lets the player proceed to the next level.

## 1. Create the Win Zone Object
This is the physical area the player must walk into to win.

1.  **Hierarchy** -> Right-click -> **3D Object** -> **Cube**.
2.  **Rename** it to `WinZone`.
3.  **Position** it at the end of your level.
4.  **Remove Mesh Renderer** (Optional): If you want it invisible, uncheck `MeshRenderer`. If you want it visible (e.g., a flag or portal), keep it or replace the mesh.
5.  **Box Collider**: Check the **Is Trigger** box! This is critical.
6.  **Add Script**: Drag the `WinZone.cs` script onto this object.

## 2. Create the Win Screen UI
This is the menu that pops up when you win.

1.  **Hierarchy** -> Right-click **Canvas** -> **UI** -> **Panel**.
2.  **Rename** it to `WinPanel`.
3.  **Style It**:
    *   Change color (e.g., Gold/Yellow with slight transparency).
    *   Add a **Text** (TextMeshPro) child: "LEVEL COMPLETE!".
4.  **Add Buttons**:
    *   **Next Level Button**: To go to the next stage.
    *   **Menu Button**: To return to main menu.
5.  **Hide It**: Uncheck `WinPanel` so it is invisible while playing.

## 3. Link Everything
1.  Select the **WinZone** object (the 3D cube).
2.  **Win Screen Panel**: Drag your `WinPanel` into this slot.
3.  **Next Level Scene Name**: Type the EXACT name of your next scene (e.g., `Level2`).
4.  **Is Final Level**: Check this ONLY if this is the last level (it will send you back to Main Menu).

## 4. Link Buttons
1.  Select **Next Level Button**.
    *   On Click () -> Drag `WinZone` object to slot.
    *   Function: `WinZone` -> `LoadNextLevel()`.
2.  Select **Menu Button**.
    *   On Click () -> Drag `WinZone` object to slot.
    *   Function: `WinZone` -> `LoadMainMenu()`.

## 5. Build Settings (Crucial!)
Unity cannot load levels it doesn't know about.
1.  **File** -> **Build Settings**.
2.  Drag **Level1** (current) and **Level2** (next) into the "Scenes In Build" list.
3.  Ensure "MainMenu" is also in the list if you use it.

## Testing
1.  Play game.
2.  Walk into the Win Zone.
3.  Game triggers "Level Completed!", pauses, and shows cursor.
4.  Click Next Level -> Loads next scene.
