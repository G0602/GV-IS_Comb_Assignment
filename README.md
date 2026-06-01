# GV-IS Comb Assignment

Unity project for the GV-IS combined assignment.

Last updated: 2026-06-01

## Unity Version
- 6000.4.2f1

## Quick Start
1. Open the project in Unity Hub using the version above.
2. Open a scene from Assets/Scenes (see list below).
3. If needed, set the active scene in Build Settings.
4. Press Play.

## Project Overview
This project is a Unity-based assignment with a focus on interactive gameplay systems. Core gameplay features are implemented in the Scripts folders listed below, with input configured through the Input System asset.

## Controls
Controls are defined in Assets/InputSystem_Actions.inputactions.

Suggested workflow to review bindings:
1. Open the Input Actions asset in Unity.
2. Inspect the action maps and bindings.
3. Update or rebind as needed for keyboard, mouse, or gamepad.

## Build and Run
1. Open File > Build Settings.
2. Add the intended scene(s) to the build list.
3. Select target platform and press Build.

## Troubleshooting
- If input does not respond, re-open Assets/InputSystem_Actions.inputactions and ensure the correct action map is enabled.
- If lighting appears incorrect, verify URP is assigned in ProjectSettings/Graphics.
- If a scene loads empty, confirm it is added to Build Settings and set as active.

## QA Checklist
- Scene loads without errors.
- Player input works (movement, interactions, flashlight).
- Core interactables respond (doors, keycards, pushable objects, torch).
- UI prompts display when in range.
- Performance stays stable in the main scene.

## Scenes
- MainScene.unity
- MainGasmeScene.unity

## Key Scripts
- Assets/Scripts/AI
  - AlienFreezeTest, AlienPathController, BlockedPathTrigger
  - GraphEdge, GraphManager, SimplePathFinder
- Assets/Scripts/Interactables
  - DoorInteractable, KeycardPickup, PushableObject, TorchPickup
- Assets/Scripts/Inventory
  - PlayerInventory
- Assets/Scripts/Player
  - FlashLightAlienFreeze, PlayerFlashlightController, PlayerPushInteraction
- Assets/Scripts/UI
  - InteractionPromptUI

## Assets Overview
- InputSystem_Actions.inputactions (input bindings)
- Prefabs (environment and prop prefabs)
- Materials, Textures, models (art assets)
- StarterAssets (Unity starter content)
- TextMesh Pro (TMP assets)
- Free Wood Door Pack (asset pack)
- TutorialInfo (tutorial/readme content)

## Project Layout
- Assets (game content)
- Packages (Unity packages and dependencies)
- ProjectSettings (Unity project settings)
- Library, Logs, Temp, UserSettings (generated files)

## Packages (selected)
- com.unity.render-pipelines.universal 17.4.0
- com.unity.inputsystem 1.19.0
- com.unity.ai.navigation 2.0.12
- com.unity.timeline 1.8.12
- com.unity.ugui 2.0.0

## Credits and Attribution
- Free Wood Door Pack (asset pack)
- TextMesh Pro (Unity package)

## Notes
- Unity Readme asset is located at Assets/Readme.asset.
- Update this README when scenes, scripts, or assets change.
