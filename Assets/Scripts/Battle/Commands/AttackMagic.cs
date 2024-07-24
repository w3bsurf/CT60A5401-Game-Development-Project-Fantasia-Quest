using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

// Class for magic attack commands
public class AttackMagic : ICommand
{
    private Actor attacker;
    private List<Ally> allies;
    private List<Enemy> enemies;
    private AudioSource sxf;


    public bool IsFinished { get; private set; } = false;

    public AttackMagic(Actor actor, List<Ally> targets, AudioSource sfx)
    {
        this.attacker = actor;
        this.allies = targets;
        this.sxf = sfx;
    }

    public AttackMagic(Actor actor, List<Enemy> targets, AudioSource sfx)
    {
        this.attacker = actor;
        this.enemies = targets;
        this.sxf = sfx;
    }

    // Executes magic attack command on targets, calculates damage, plays attack animation and sound
    public IEnumerator Co_Execute(Battle battle)
    {
        if (attacker is Ally)
        {
            attacker.Animator.Play("attack_magic");
            sxf.Play();
            foreach (var target in enemies)
            {
                target.AllowDeath = false;
                int damage = Calculate.MagicAttackDamage(attacker as Ally, target);
                battle.PopupCreator.CreateDamagePopup(target, damage);
            }
            

            yield return new WaitForEndOfFrame();

            foreach (var target in enemies)
            {
                target.Animator.Play("hurt");
            }
            
            while (attacker.Animator.IsAnimating() || enemies[0].Animator.IsAnimating())
            {
                yield return null;
            }

            attacker.Animator.Play("walk_battle");

            foreach (var target in enemies)
            {
                target.AllowDeath = true;
                if (target != null)
                {
                    target.Animator.Play("idle_battle");
                }
            }
            IsFinished = true;
        }
        else
        {
            attacker.Animator.Play("attack");
            sxf.Play();
            foreach (var target in allies)
            {
                target.AllowDeath = false;
                int damage = Calculate.EnemyMagicAttackDamage(attacker as Enemy, target);
                battle.PopupCreator.CreateDamagePopup(target, damage);
            }

            yield return new WaitForEndOfFrame();

            foreach (var target in allies)
            {
                target.Animator.Play("hurt");
            }

            while (attacker.Animator.IsAnimating() || allies[0].Animator.IsAnimating())
            {
                yield return null;
            }

            attacker.Animator.Play("idle_battle");

            foreach (var target in allies)
            {
                target.AllowDeath = true;
                if (target != null)
                {
                    target.Animator.Play("idle_battle");
                }
            }
            IsFinished = true;
        }  
    }
}
