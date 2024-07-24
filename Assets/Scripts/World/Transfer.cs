using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Modified by adding comments and changing parameter names to be more suitable to game.
 */

// Class for handling transfers between different maps.
public class Transfer : MonoBehaviour
{
    private Map currentMap;
    private Vector2Int exitCell;

    [SerializeField] private int id;
    [SerializeField] private Map targetMap;
    [SerializeField] private string facing;
    [SerializeField] private int targetId;
    [SerializeField] private Vector2Int targetOffset;

    public int Id => id;
    public Vector2Int Cell => exitCell;
    public Vector2Int TargetOffset => targetOffset;


    private void Awake()
    {
        currentMap = FindObjectOfType<Map>();
        exitCell = currentMap.Grid.GetCell2D(this.gameObject);
    }

    private void Start()
    {
        currentMap.Transfers.Add(exitCell, this);
    }

    // Transports the player to the target map of the transfer instance
    public void TransportPlayer()
    {
        Game.Manager.LoadMap(targetMap, targetId, facing);
    }
}
