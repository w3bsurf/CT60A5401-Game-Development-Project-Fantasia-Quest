using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Added comments and enum
 */

[System.Serializable]
// Class for moving a character during cutscenes
public class MoveCharacterCommand : CutsceneCommand
{
    private enum Dir
    {
        Up,
        Down,
        Left,
        Right,
    }

    [SerializeField] private Character character;
    [SerializeField] private float movementSpeed;
    [SerializeField] private List<Dir> route = new List<Dir>();

    public bool IsFinished {  get; private set; }

    // Coroutine for moving a character along a set route during a cutscene
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

            character.Move.TryMove(moveDirection);
            yield return null;

            while (character.IsMoving)
            {
                yield return null;
            }
        }

        IsFinished = true;
    }

    // Overwrites the basic tostring method to return a more suitable name
    public override string ToString()
    {
        return "Move Character";
    }
}
