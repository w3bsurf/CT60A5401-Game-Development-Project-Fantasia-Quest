using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

// battle command for using a healing potion
public class UsePotion : ICommand
{
    private List<Ally> allies;
    private Actor actor;
    private AudioSource sfx;
    private Item item;

    public bool IsFinished { get; private set; } = false;

    public UsePotion(Actor actor, List<Ally> allies, AudioSource sfx, Item item)
    {
        this.actor = actor;
        this.allies = allies;
        this.sfx = sfx;
        this.item = item;
    }

    // Executes use potion command on targets, calculates amount healed, removes item from item list and plays animation and sound
    public IEnumerator Co_Execute(Battle battle)
    {
        actor.Animator.Play("attack");
        sfx.Play();
        int healed = item.UseItemCombat(allies);

        yield return new WaitForEndOfFrame();

        while (actor.Animator.IsAnimating())
        {
            yield return null;
        }

        foreach (var target in allies)
        {
            battle.PopupCreator.CreateHealPopup(target, healed);
            target.Animator.Play("healed");
            
        }

        yield return new WaitForEndOfFrame();

        while (actor.Animator.IsAnimating() || allies[0].Animator.IsAnimating())
        {
            yield return null;
        }

        actor.Animator.Play("walk_battle");

        foreach (var target in allies)
        {
            target.Animator.Play("idle_battle");
        }
        
        IsFinished = true;
    }
}
