using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Added comments and new variables for the battle menu and audio, new methods for Getting targets
 */

// Abstract class for enemyai behaviour during battles
public abstract class EnemyAI : MonoBehaviour
{
    protected Enemy enemy;
    protected Battle battle;
    protected BattleMenu menu;
    protected AudioSource sfx;

    protected virtual void Awake()
    {
        battle = FindObjectOfType<Battle>();
        menu = FindObjectOfType<BattleMenu>();  
        enemy = GetComponent<Enemy>();
    }
    public abstract ICommand ChooseAction();
    
    // Chooses random target
    protected Actor GetRandomTarget()
    {
        return battle.Allies[Random.Range(0, battle.Allies.Count)];
    }

    // Get list of player party
    protected List<Ally> GetTargets()
    {
        return battle.Allies;
    }

    // Get list of enemy party
    protected List<Enemy> GetEnemyParty()
    {
        return battle.EnemyParty;
    }
}
