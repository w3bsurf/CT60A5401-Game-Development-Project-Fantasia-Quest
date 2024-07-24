using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class for imp enemy ai behaviour during battles
public class ImpAI : EnemyAI
{
    // Chooses action for the enemy to take during their turn in battle
    public override ICommand ChooseAction()
    {
        Actor target = GetRandomTarget();
        AudioSource sfx = menu.impAttack;
        return new Attack(enemy, target, sfx);
    }
}
