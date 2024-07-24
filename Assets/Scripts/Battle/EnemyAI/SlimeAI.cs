using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class for slime enemy ai behaviour during battles
public class SlimeAI : EnemyAI
{
    // Chooses action for the enemy to take during their turn in battle
    public override ICommand ChooseAction()
    {
        Actor target = GetRandomTarget();
        AudioSource sfx = menu.slimeAttack;
        return new Attack(enemy, target, sfx);
    }
}
