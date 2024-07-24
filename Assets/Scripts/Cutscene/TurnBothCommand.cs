using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
// Class for turning both the player and an NPC at the same time during cutscenes
public class TurnBothCommand : CutsceneCommand
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
    [SerializeField] private Dir npcFacing;
    [SerializeField] private Dir playerFacing;

    public bool IsFinished { get; private set; }

    // turns the characters in the desired direction
    public IEnumerator Co_Execute()
    {
        Vector2Int npcMoveDirection = npcFacing switch
        {
            Dir.Left => Direction.left,
            Dir.Right => Direction.right,
            Dir.Up => Direction.up,
            Dir.Down => Direction.down,
            _ => new Vector2Int(0, 0)
        };

        Vector2Int playerMoveDirection = playerFacing switch
        {
            Dir.Left => Direction.left,
            Dir.Right => Direction.right,
            Dir.Up => Direction.up,
            Dir.Down => Direction.down,
            _ => new Vector2Int(0, 0)
        };

        character.Turn.Turn(npcMoveDirection);
        Game.Manager.Player.Turn.Turn(playerMoveDirection);
        yield return null;

        IsFinished = true;
    }

    // Replaces the tostring return value with a more suitable string
    public override string ToString()
    {
        return "Turn Both";
    }
}
