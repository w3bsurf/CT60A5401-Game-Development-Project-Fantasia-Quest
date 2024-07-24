using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Modified by adding comments adding distinction between enemies and allies during attacks, adjusted animations and variables, added audio
 */

// Class the attack command during battles. Inherits from ICommand class
public class Attack : ICommand
{
    private Actor attacker;
    private Actor target;
    private AudioSource sfx;

    public bool IsFinished { get; private set; } = false;

    public Attack(Actor actor, Actor target, AudioSource sfx)
    {
        this.attacker = actor;
        this.target = target;
        this.sfx = sfx;
    }

    // Executes attack command on target, calculates damage, plays attack animation
    public IEnumerator Co_Execute(Battle battle)
    {
        Vector2 targetPosition;

        if (attacker is Ally)
        {
            targetPosition = (Vector2)target.transform.position - new Vector2(1.5f, 0);
            attacker.Animator.Play("walk_battle");
        }
        else
        {
            targetPosition = (Vector2)target.transform.position + new Vector2(1.5f, 0);
            attacker.Animator.Play("idle_battle");
        }   

        while ((Vector2)attacker.transform.position != targetPosition)
        {
            attacker.transform.position = Vector2.MoveTowards(attacker.transform.position, targetPosition, 5f * Time.deltaTime);
            yield return null;
        }

        attacker.Animator.Play("attack");
        sfx.Play();
        if (attacker is Ally)
        {
            attacker.Animator.Play("walk_battle");
            int damage = Calculate.AttackDamage(attacker as Ally, target as Enemy);
            battle.PopupCreator.CreateDamagePopup(target, damage);
        }
        else
        {
            attacker.Animator.Play("idle_battle");
            int damage = Calculate.EnemyAttackDamage(attacker as Enemy, target as Ally);
            battle.PopupCreator.CreateDamagePopup(target, damage);
        }

        target.Animator.Play("hurt");
        yield return new WaitForEndOfFrame();
        while (attacker.Animator.IsAnimating() || target.Animator.IsAnimating())
        {
            yield return null;
        }

        if (target != null)
        {
            target.Animator.Play("idle_battle");
        }

        IsFinished = true; 
    }
}
