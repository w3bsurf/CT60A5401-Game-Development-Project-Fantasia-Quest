using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Modified with added comments, altered structure, changed commands, added checks for when input is possible
 */

public class InputHandler
{
    private Player player;
    private Command command;

    private enum Command
    {
        None,
        MoveLeft,
        MoveRight,
        MoveUp,
        MoveDown,
        Interact,
        ToggleMenu,
        AdvanceDialogue,
    }

    public InputHandler(Player player)
    {
        this.player = player;
    }

    // Check player input for different commands
    public void CheckInput()
    {
        command = Command.None;

        if (Game.Manager.State == GameState.Transition) 
        {
            return;
        }

        if (Game.Manager.State == GameState.Dialogue && (Input.GetKeyDown(KeyCode.Space) || (Input.GetKeyDown(KeyCode.Return)))) { 
            command = Command.AdvanceDialogue;
            HandleCommand(command);
            return;
        }

        if (Game.Manager.State != GameState.World)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            command = Command.ToggleMenu;
            HandleCommand(command);
            return;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            command = Command.MoveLeft;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            command = Command.MoveRight;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            command = Command.MoveUp;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            command = Command.MoveDown;
        }
        else if (Input.GetKeyDown(KeyCode.Space) || (Input.GetKeyDown(KeyCode.Return)))
        {
            command = Command.Interact;
        }

        if (command != Command.None)
        {
            HandleCommand(command);
        }
    }

    // Checks type of command issued by player and calls appropriate function
    private void HandleCommand(Command command)
    {
        switch (command)
        {
            case Command.MoveLeft:
            case Command.MoveRight:
            case Command.MoveUp:
            case Command.MoveDown:
                ProcessMovement(command);
                break;
            case Command.Interact:
                ProcessInteract();
                break;
            case Command.ToggleMenu:
                ProcessToggleMenu();
                break;
            case Command.AdvanceDialogue:
                ProcessAdvanceDialogue();
                break;
        }
    }

    // Takes in the command issued and moves the player
    private void ProcessMovement(Command command)
    {
        Vector2Int direction = new Vector2Int(0, 0);

        switch (command)
        {
            case Command.MoveLeft:
                direction = Direction.left;
                break;
            case Command.MoveRight:
                direction = Direction.right;
                break;
            case Command.MoveUp:
                direction = Direction.up;
                break;
            case Command.MoveDown:
                direction = Direction.down;
                break;
        }

        player.Move.TryMove(direction);
    }

    // Checks for interactables when receiving interact command and starts interaction
    private void ProcessInteract()
    {
        Vector2Int direction = player.Facing;
        Vector2Int currentCell = Game.Manager.Map.Grid.GetCell2D(player.gameObject);

        if (!Game.Manager.Map.OccupiedCells.ContainsKey(currentCell + direction))
        {
            return;
        }

        if (Game.Manager.Map.OccupiedCells[currentCell + direction] is IInteractable interactable)
        {
            interactable.Interact();
        }
    }

    // Toggles the game menu open or closed
    private void ProcessToggleMenu() 
    {
        Game.Manager.ToggleMenu();
    }

    // Advances the dialogue to the next line
    private void ProcessAdvanceDialogue()
    {
        Game.Manager.AdvanceDialogue();
    }
}
