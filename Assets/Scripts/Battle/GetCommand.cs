using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Modified by adding comments.
 */

// Class for getting a command from a character in battle
public class GetCommand
{
    private Actor actor;
    private Battle battle;
    public ICommand Command {  get; private set; }

    // Constructor for GetCommand class
    public GetCommand(Actor actor)
    {
        this.actor = actor;
        battle = GameObject.FindObjectOfType<Battle>();
    }

    // Coroutine for waiting for a command
    public IEnumerator Co_GetCommand()
    {
        while (battle.BattleMenu.Command == null) 
        {
            yield return null;
        }
        Command = battle.BattleMenu.Command;
    }
    
}
