using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class for demon enemy ai behaviour during battles
public class DemonAI : EnemyAI
{
    // Chooses action for the enemy to take during their turn in battle
    public override ICommand ChooseAction()
    {
        int i = Random.Range(0, 3);
        Actor target = GetRandomTarget();

        switch (i)
        {
            case 0:
                sfx = menu.demonAttack;
                return new Attack(enemy, target, sfx);
            case 1:
                List<Ally> targets = GetTargets();
                sfx = menu.magicEnemy;
                return new AttackMagic(enemy, targets, sfx);
            case 2:
                List<Enemy> enemyParty = GetEnemyParty();
                sfx = menu.heal;
                return new Heal(enemy, enemyParty, sfx);
            default:
                sfx = menu.demonAttack;
                return new Attack(enemy, target, sfx);
        }
    }
}
