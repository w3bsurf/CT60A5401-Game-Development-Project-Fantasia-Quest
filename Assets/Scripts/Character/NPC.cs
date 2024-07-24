using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Modified by adding comments, additional properties such as charID and method for destroying instances of the class.
 */


// A class for NPCs which inherits from the Character and IInteractable classes
public class NPC : Character, IInteractable
{
    private enum Dir
    {
        Up,
        Down,
        Left,
        Right,
    }

    [SerializeField] private ScriptableObject interaction;
    [SerializeField] private List<Dir> moveRoute = new List<Dir>();
    [SerializeField] private float delay = 2f;
    [SerializeField] private bool stationary = false;
    [SerializeField] private bool randomMovement = false;
    [SerializeField] private bool loopRoute = false;
    [SerializeField] private int charID;

    private int currentMoveRouteIndex = 0;
    private float timeElapsed = 0;

    public int CharID => charID;
    public ScriptableObject Interaction => interaction;

    // Function for interacting with an NPC. Check interaction type and runs approriate interaction.
    public void Interact()
    {
        if (interaction is Dialogue dialogue)
        {
            Turn.FacePlayer();
            Game.Manager.StartDialogue(dialogue);
            Game.Manager.OpenShopMenu();
        }
    }

    protected override void Update()
    {
        base.Update();

        if (Game.Manager != null)
        {
            // Stop updating NPCs if the gamestate is not "World"
            if (Game.Manager.State != GameState.World)
            {
                return;
            }
        }
        
        // Return if NPC is set as stationary or is moving at this time
        if (stationary || IsMoving)
        {
            return;
        }

        timeElapsed += Time.deltaTime;

        if (timeElapsed < delay)
        {
            return;
        }

        timeElapsed = 0;

        if (randomMovement)
        {
            MoveInRandomDirection();
            return;
        }

        FollowMoveRoute();
    }

    // move NPC in a random direction
    private void MoveInRandomDirection()
    {
        int random = Random.Range(0, 4);
        Vector2Int moveDirection = random switch
        {
            0 => Direction.left,
            1 => Direction.right,
            2 => Direction.up,
            3 => Direction.down,
            _ => new Vector2Int(0, 0)
        };

        Move.TryMove(moveDirection);
    }

    // Have NPC follow a set route
    private void FollowMoveRoute()
    {
        if (currentMoveRouteIndex >= moveRoute.Count)
        {
            return;
        }

        Dir direction = moveRoute[currentMoveRouteIndex];
        Vector2Int moveDirection = direction switch
        {
            Dir.Left => Direction.left,
            Dir.Right => Direction.right,
            Dir.Up => Direction.up,
            Dir.Down => Direction.down,
            _ => new Vector2Int(0, 0)
        };

        Move.TryMove(moveDirection);
        currentMoveRouteIndex++;

        if (loopRoute)
        {
            currentMoveRouteIndex = currentMoveRouteIndex % moveRoute.Count;
        }
    }

    // Have character disappear from the scene and added into list of destroyed characters so they won't be loaded into maps
    public void DestroyCharacter()
    {
        Game.Manager.Map.OccupiedCells.Remove(this.CurrentCell);
        if (!Game.Manager.DestroyedCharacterIDs.Contains(this.CharID)) {
            Game.Manager.DestroyedCharacterIDs.Add(this.CharID);
        }
        GameObject.Destroy(this.gameObject);
    }
}
