using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Modified with added comments and adjusted variable names
 */

public class CharacterTurner
{
    private Character character;
    public Vector2Int Facing { get; private set; } = Direction.down;

    public CharacterTurner(Character character) 
    {
        this.character = character;
    }

    // Turn character towards a direction
    public void Turn(Vector2Int direction)
    {
        if (direction.IsBasic())
        {
            Facing = direction;
        }
    }

    // Turn character around
    public void TurnAround() => Facing = new Vector2Int(-Facing.x, -Facing.y);

    // Turn NPC to face player when interacting with them based on the player's position relative to NPC
    public void FacePlayer()
    {
        Player player = Game.Manager.Player;

        if (player.CurrentCell.x > character.CurrentCell.x)
        {
            Turn(Direction.right);
        }
        else if (player.CurrentCell.x < character.CurrentCell.x)
        {
            Turn(Direction.left);
        }
        else if (player.CurrentCell.y > character.CurrentCell.y)
        {
            Turn(Direction.up);
        }
        else if (player.CurrentCell.y < character.CurrentCell.y)
        {
            Turn(Direction.down);
        }
    }
}
