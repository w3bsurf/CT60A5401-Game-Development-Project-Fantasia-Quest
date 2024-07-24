using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for the Double attack special skill
public class DoubleAttack : ICommand
{
    private Ally attacker;
    private List<Enemy> targets;
    private AudioSource sfx;

    public bool IsFinished { get; private set; } = false;

    public DoubleAttack(Ally actor, List<Enemy> targets, AudioSource sfx)
    {
        this.attacker = actor;
        this.targets= targets;
        this.sfx = sfx;
    }

    // Executes double attack command on targets, calculates damage, plays attack animation and sound
    public IEnumerator Co_Execute(Battle battle)
    {
        attacker.Animator.Play("attack");
        sfx.Play();

        for (int i  = 0; i < 2; i++)
        {
            Enemy target = targets[Random.Range(0, targets.Count)];
            target.AllowDeath = false;
            int damage = Calculate.DoubleAttackDamage(attacker, target);
            battle.PopupCreator.CreateDamagePopup(target, damage);
            target.Animator.Play("hurt");
        }

        yield return new WaitForEndOfFrame();

        while (attacker.Animator.IsAnimating() || targets[0].Animator.IsAnimating())
        {
            yield return null;
        }

        attacker.Animator.Play("walk_battle");

        foreach (var target in targets)
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
