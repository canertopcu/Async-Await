# Async-Await README
 
This repository contains three primary scripts and one interface for a Unity game:

1. **GameLogic.cs**: This script manages the game logic and player interactions.

2. **IPlayer.cs**: This interface is implemented by the `PlayerController` class and defines the contract for player-related actions.
 
3. **PlayerController.cs**: This script defines the behavior for the player character. And asyncronous programming is here

4. **TargetController.cs**: This script handles the target object's placement and activation. 

## IPlayer.cs (Interface)

- The `IPlayer` interface defines the contract for player characters.
- It includes a property for getting the player's transform and a method for setting the player's target position.

## GameLogic.cs

- The `GameLogic` script is responsible for controlling the game logic, including player interactions and target placements.
- It handles the creation of players and updates their target positions.
- The script utilizes Unity's MonoBehaviour and Unity's physics system for raycasting.
- Pressing the space key generates a player character.
- Left-clicking on the screen spawns a target object and allows the player to move it.
- When releasing the left mouse button, the target object's position is updated, affecting all player characters.

## PlayerController.cs

- The `PlayerController` script defines the behavior of the player character.
- It implements the `IPlayer` interface, providing a common contract for managing player actions.
- Player characters can rotate and move towards a target position asynchronously.
- The `SetTargetPosition` method starts the player's movement towards the given target.
- It uses asynchronous methods for smoother character movement.
- It supports cancellation of movement tasks.

## TargetController.cs

- The `TargetController` script manages the behavior of target objects.
- It allows placing target objects at specific positions and activating or deactivating them.
- It uses an event (`OnMove`) to notify other scripts when the target is moved.



## Usage

- Attach the `GameLogic` script to an empty GameObject in your Unity scene.
- Create prefabs for your player characters and targets and assign them to the respective fields in the `GameLogic` script.
- Run the game, and you can generate players by pressing the space key and move target objects by left-clicking.

## Dependencies

- This project is built with Unity, so you'll need Unity installed to run and modify the code.
- The code uses C# 7.0 and Unity's MonoBehaviour system.

## License

This project is provided under the [MIT License](LICENSE).

---

You can customize this README further by adding sections for installation instructions, troubleshooting, or any additional information that is relevant to your project. Remember to update the README as your project evolves and if you add or modify any features.
