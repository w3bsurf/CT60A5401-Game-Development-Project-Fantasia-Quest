using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.TextCore.Text;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Modified with added comments and adjusted speed and variable and function names.
 */

// Class for moving NPCs and player character. 
public class CharacterMover
{
    private Character character;
    private Transform transform;
    private const float time_to_move_one_square = .250f;
    public bool IsMoving { get; private set; } = false;

    public CharacterMover(Character character)
    {
        this.character = character;
        this.transform = character.transform;
    }

    // Attempts to move into the target cell.
    public void TryMove(Vector2Int direction)
    {
        if (IsMoving || !direction.IsBasic()) return;
 
        character.Turn.Turn(direction);
        Vector2Int targetCell = character.CurrentCell + direction;

        if (CanMoveIntoCell(targetCell, direction))
        {
            Game.Manager.Map.OccupiedCells.Add(targetCell, character);
            Game.Manager.Map.OccupiedCells.Remove(character.CurrentCell);
            character.StartCoroutine(Co_Move(direction));
        }  
    }

    // Checks for objects such as characters in the target cell.
    private bool CanMoveIntoCell(Vector2Int targetCell, Vector2Int direction)
    {
        if (IsCellOccupied(targetCell))
        {
            return false;
        }

        Ray2D ray = new Ray2D(character.CurrentCell.Center2D(), direction);
        RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.distance < Game.Manager.Map.Grid.cellSize.x) { 
                return false;
            }
         }
        return true;
    }

    // Checks if a cell is already occupied.
    private bool IsCellOccupied(Vector2Int cell) 
    { 
        return Game.Manager.Map.OccupiedCells.ContainsKey(cell);
    }


    // Coroutine for moving character or player in specified direction from one cell to the target cell.
    private IEnumerator Co_Move(Vector2Int direction)
    {
        IsMoving = true;

        Vector2 startingPosition = character.CurrentCell.Center2D(); 
        Vector2 endingPosition = (character.CurrentCell + direction).Center2D();

        float elapsedTime = 0;
        
        while ((Vector2)transform.position != endingPosition)
        {
            transform.position = Vector2.Lerp(startingPosition, endingPosition, elapsedTime / time_to_move_one_square);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        transform.position = endingPosition;
        IsMoving = false;

        if (character is Player player)
        {
            player.CheckCurrentCell();
        }
    }
}
