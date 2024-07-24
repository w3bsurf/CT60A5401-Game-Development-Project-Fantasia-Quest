using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Modified by adding comments, changing methods and adding methods such as Co_ToggleBattleCommands and Update and properties such as enemyattributes, allow death, added checks to stop updates and action in certain cases
 */

// Class for enemy characters during battle
public class Enemy : Actor
{
    private EnemyAI enemyAI;

    public EnemyAttributes EnemyAttributes { get; set; }
    public event Action EnemyDefeated;
    public bool AllowDeath = true;

    protected override void Awake()
    {
        base.Awake();
        enemyAI = GetComponent<EnemyAI>();
        EnemyDefeated += OnEnemyKilled;
        sfx = menu.death;
    }

    protected void Update()
    {
        if (!AllowDeath)
        {
            return;
        }

        if (EnemyAttributes.Hp == 0)
        {
            EnemyDefeated?.Invoke();
        }
    }

    // starts the enemy character's turn
    public override void StartTurn(BattleMenu battleMenu)
    {
        if (!battle.BattleOver && EnemyAttributes.Hp != 0)
        {
            IsTakingTurn = true;
            StartCoroutine(Co_ToggleBattleCommands(battleMenu));
            StartCoroutine(Co_MoveForward());
        } 
    }

    // Moves enemy forward during battle when it is their turn
    private IEnumerator Co_MoveForward()
    {
        float elapsedTime = 0;
        battlePosition = startingPosition + new Vector2Int(-2, 0);

        Animator.Play("idle_battle");
        while ((Vector2)transform.position != battlePosition)
        {
            transform.position = Vector2.Lerp(startingPosition, battlePosition, elapsedTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Animator.Play("idle_battle");
        yield return new WaitForSeconds(.5f);

        StartCoroutine(Co_EnemyActionSelection());
    }

    // Toggles the battle command menu
    public IEnumerator Co_ToggleBattleCommands(BattleMenu battleMenu)
    {
        if (battleMenu.IsAnimating)
        {
            yield return null;
        }

        if (!battleMenu.IsShown)
        {
            yield return null;
        }
        else
        {
            battleMenu.HideBattleCommands();
        }
    }

    // Selects the enemy character's command
    private IEnumerator Co_EnemyActionSelection()
    {
        ICommand command;
        command = enemyAI.ChooseAction();
        StartCoroutine(command.Co_Execute(battle));
        while (!command.IsFinished)
        {
            yield return null;
        }
        StartCoroutine(Co_EndTurn());
    }

    // Ends turn and returns enemy to starting position
    private IEnumerator Co_EndTurn()
    {
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

    // Starts coroutine for enemy death when enemy is defeated
    private void OnEnemyKilled() => StartCoroutine(Co_Die());

    // Coroutine for handling enemy death
    private IEnumerator Co_Die()
    {
        battle.Dying = true;
        EnemyDefeated -= OnEnemyKilled;

        Animator.Play("death");
        sfx.Play();
        yield return null;
        while (Animator.IsAnimating())
        {
            yield return null;
        }
        Destroy(this.gameObject);
        battle.Dying = false;
    }
}
