using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Added comments. Modified with the check for current cell method
 */

// Class for the player character in the world game state
public class Player : Character
{
    private InputHandler InputHandler;

    protected override void Awake()
    {
        base.Awake();
        InputHandler = new InputHandler(this);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        InputHandler.CheckInput();
    }

    // Checks current cell for trigger such as exits and cutscenes or battles
    public void CheckCurrentCell()
    {
        if (Game.Manager.Map.Transfers.ContainsKey(CurrentCell))
        {
            Transfer transfer = Game.Manager.Map.Transfers[CurrentCell];
            transfer.TransportPlayer();
            return;
        }

        if (Game.Manager.Map.Cutscenes.ContainsKey(CurrentCell))
        {
            Cutscene cutscene = Game.Manager.Map.Cutscenes[CurrentCell];
            cutscene.Play();
            return;
        }

        if (Game.Manager.Map.Region != null) 
        {
            Game.Manager.Map.Region.CheckForEncounter(Game.Manager.Map);
        }  
    }
}
