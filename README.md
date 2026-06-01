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

## Notes
- Unity Readme asset is located at Assets/Readme.asset.
- Update this README when scenes, scripts, or assets change.
