using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
// Class for moving both the player and an NPC at the same time during cutscenes
public class MoveBothCommand : CutsceneCommand
{
    private enum Dir
    {
        Up,
        Down,
        Left,
        Right,
    }

    [SerializeField] private Character character;
    [SerializeField] private float speed;
    [SerializeField] private List<Dir> route;
    
    public bool IsFinished { get; private set; }

    // moves the characters in the desired direction
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
            character.Move.TryMove(moveDirection);
            yield return null;

            while (Game.Manager.Player.IsMoving && character.IsMoving)
            {
                yield return null;
            }
        }

        IsFinished = true;
    }

    // Replaces the tostring return value with a more suitable string
    public override string ToString()
    {
        return "Move Both";
    }
}
