using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Added comments and enum
 */

[System.Serializable]
// Class for moving the player during cutscenes
public class MovePlayerCommand : CutsceneCommand
{
    private enum Dir
    {
        Up,
        Down,
        Left,
        Right,
    }

    [SerializeField] private float speed;
    [SerializeField] private List<Dir> route;
    
    public bool IsFinished { get; private set; }

    // moves the player character in the desired direction
    public IEnumerator Co_Execute()
    {
        foreach (Dir direction in route)
        {

            Vector2Int moveDirection = direction switch
            {
                Dir.Left => Direction.left,
                Dir.Right => Direction.right,
                Dir.Up => Direction.up,
                Dir.Down => Direction.down,
                _ => new Vector2Int(0, 0)
            };

            Game.Manager.Player.Move.TryMove(moveDirection);
            yield return null;

            while (Game.Manager.Player.IsMoving)
            {
                yield return null;
            }
        }

        IsFinished = true;
    }

    // Replaces the tostring return value with a more suitable string
    public override string ToString()
    {
        return "Move Player";
    }
}
