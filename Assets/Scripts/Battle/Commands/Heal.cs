using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for the heal battle command
public class Heal : ICommand
{
    private Actor healer;
    private List<Enemy> enemies;
    private List<Ally> allies;
    private AudioSource sfx;

    public bool IsFinished { get; private set; } = false;
    
    public Heal(Enemy actor, List<Enemy> targets, AudioSource sfx)
    {
        this.healer = actor;
        this.enemies = targets;
        this.sfx = sfx;
    }
    public Heal(Ally actor, List<Ally> targets, AudioSource sfx)
    {
        this.healer = actor;
        this.allies = targets;
        this.sfx = sfx;
    }

    // Executes heal command on targets, calculates amount healed, plays heal animation and sound
    public IEnumerator Co_Execute(Battle battle)
    {
        if (healer is Ally)
        {
            healer.Animator.Play("heal");
            sfx.Play();
            yield return new WaitForEndOfFrame();

            while (healer.Animator.IsAnimating())
            {
                yield return null;
            }

            foreach (var target in allies)
            {
                int healed = Calculate.MagicHeal(healer as Ally, target);
                battle.PopupCreator.CreateHealPopup(target, healed);
                target.Animator.Play("healed");
            }

            yield return new WaitForEndOfFrame();

            while (healer.Animator.IsAnimating() || allies[0].Animator.IsAnimating())
            {
                yield return null;
            }

            healer.Animator.Play("walk_battle");

            foreach (var target in allies)
            {
                target.Animator.Play("idle_battle");
            }
            
            IsFinished = true;
        }
        else
        {
            healer.Animator.Play("attack");
            sfx.Play();
            foreach (var target in enemies)
            {
                int damage = Calculate.EnemyMagicHeal(healer as Enemy, target);
                battle.PopupCreator.CreateDamagePopup(target, damage);
                target.Animator.Play("healed");
                
            }

            yield return new WaitForEndOfFrame();

            while (healer.Animator.IsAnimating() || enemies[0].Animator.IsAnimating())
            {
                yield return null;
            }

            healer.Animator.Play("idle_battle");

            foreach (var target in enemies)
            {
                target.Animator.Play("idle_battle");
            }
            
            IsFinished = true;
        } 
    }
}
