using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Modified by adding comments and modifying with additional parameters, and additional methods such as methods for handling ally being defeated, added checks for allow death, and stopping turns when the battle is over
 */

// Class for party members aka "Allies" during battles
public class Ally : Actor
{
    public CharacterAttributes CharacterAttributes {  get; set; }
    public PartyMember member { get; set; }
    public bool AllowDeath = true;
    public event Action AllyDefeated;

    protected override void Awake()
    {
        base.Awake();
        sfx = menu.death;
        AllyDefeated += OnAllyKilled;
    }

    protected void Update()
    {
        if (!AllowDeath)
        {
            return;
        }

        if (CharacterAttributes.Hp == 0)
        {
            AllyDefeated?.Invoke();
        }
    }

    // Start the character's turn in battle
    public override void StartTurn(BattleMenu battleMenu)
    {
        if (!battle.BattleOver)
        {
            IsTakingTurn = true;
            StartCoroutine(Co_ToggleBattleCommands(battleMenu));
            StartCoroutine(Co_MoveForward(battleMenu));
        } 
    }

    // Move forward a few steps when it is the character's turn
    private IEnumerator Co_MoveForward(BattleMenu battleMenu) 
    {
        float elapsedTime = 0;
        battlePosition = startingPosition + new Vector2Int(2, 0);
        Animator.Play("walk_battle");

        while ((Vector2)transform.position != battlePosition)
        {
            transform.position = Vector2.Lerp(startingPosition, battlePosition, elapsedTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Animator.Play("idle_battle");

        StartCoroutine(Co_GetPlayerCommand(battleMenu));
    }

    // Toggles the battle command menu
    public IEnumerator Co_ToggleBattleCommands(BattleMenu battleMenu)
    {
        if (battleMenu.IsAnimating)
        {
            yield return null;
        }

        if (battleMenu.IsShown)
        {
            yield return null;
        }
        else
        {
            battleMenu.ShowBattleCommands();
        }
    }

    // Get player's command for character in battle
    private IEnumerator Co_GetPlayerCommand(BattleMenu battleMenu) {
        GetCommand getCommand = new GetCommand(this);
        StartCoroutine(getCommand.Co_GetCommand());

        while (getCommand.Command is null)
        {
            yield return null;
        }
        battleMenu.HideBattleCommands();
        StartCoroutine(getCommand.Command.Co_Execute(battle));

        while (!getCommand.Command.IsFinished)
        {
            yield return null; 
        }

        StartCoroutine(Co_EndTurn(battleMenu));
    }

    // End the party member turn
    private IEnumerator Co_EndTurn(BattleMenu battleMenu) {
        float elapsedTime = 0;
        Vector2 currentPosition = transform.position;

        while ((Vector2)transform.position != startingPosition)
        {
            transform.position = Vector2.Lerp(currentPosition, startingPosition, elapsedTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        Animator.Play("idle_battle");
        IsTakingTurn = false;    
    }

    // Starts coroutine for ally death when ally is defeated
    private void OnAllyKilled() => StartCoroutine(Co_Die());

    // Coroutine for handling ally death
    private IEnumerator Co_Die()
    {
        battle.Dying = true;
        AllyDefeated -= OnAllyKilled;
        Animator.Play("death");
        sfx.Play();
        while (Animator.IsAnimating())
        {
            yield return null;
        }
        Destroy(this.gameObject);
        battle.Dying = false;
    }
}
