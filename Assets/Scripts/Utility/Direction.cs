using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Modified by adding comments
 */

// A static class for handling the four main directions in the game world
public static class Direction
{
    public static readonly Vector2Int up = new Vector2Int(0, 1);
    public static readonly Vector2Int down = new Vector2Int(0, -1);
    public static readonly Vector2Int left = new Vector2Int(-1, 0);
    public static readonly Vector2Int right = new Vector2Int(1, 0);

    // Gets the center coordinate of a cell as a vector 2
    public static Vector2 Center2D(this Vector2Int cell)
    {
        Vector3Int threeDimensionCell = new Vector3Int(cell.x, cell.y, 0);
        return (Vector2)Game.Manager.Map.Grid.GetCellCenterWorld(threeDimensionCell);
    }
}
