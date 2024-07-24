using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Modified by adding comments, transition gamestate, properties such as lists for holding gameobject ids, modified functions such as to set player facing when transitioning between areas.
 */

// A static class for various extension methods
public static class Extensions
{

    // Gets the cell as a vector2 int instead of vector 3
    public static Vector2Int GetCell2D(this Grid grid, GameObject gameobject)
    {
        Vector3 position = gameobject.transform.position;
        return (Vector2Int) grid.WorldToCell(position);
    }

    // Gets the cell center as vector 2
    public static Vector2 GetCellCenter2D(this Grid grid, Vector2Int cell)
    {
        Vector3Int threeDimensionCell = new Vector3Int(cell.x, cell.y, 0);
        return (Vector2) grid.GetCellCenterWorld(threeDimensionCell);
    }

    // Checks if animator is currently animating
    public static bool IsAnimating(this Animator animator)
    {
        if (animator != null)
        {
            bool isAnimating = (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1);
            return isAnimating;
        }
        return false;
    }

    // Checks if the given direction is one of the four basic directions
    public static bool IsBasic(this Vector2Int direction)
    {
        if (direction == Direction.up || direction == Direction.down || direction == Direction.left || direction == Direction.right)
        {
            return true;
        }
        return false;
    }
}
