using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for calculating values for different actions in battle
public static class Calculate
{
    // Calculates damage done by player character attack on enemy target
    public static int AttackDamage(Ally attacker, Enemy target)
    {
        int damage = Mathf.Max(0, ((attacker.member.Str - (target.EnemyAttributes.Def/4)) + Random.Range(1, 5)));
        target.EnemyAttributes.Hp -= damage;
        return damage;
    }

    // Calculates damage done by player character double attack skill on enemy target
    public static int DoubleAttackDamage(Ally attacker, Enemy target)
    {
        int damage = Mathf.Max(0, ((attacker.member.Str - (target.EnemyAttributes.Def/3)) + Random.Range(0, 2)));
        target.EnemyAttributes.Hp -= damage;
        return damage;
    }

    // Calculates damage done by enemy character attack on player character
    public static int EnemyAttackDamage(Enemy attacker, Ally target)
    {
        int damage = new int();
        if (attacker.EnemyAttributes.Str > 10)
        {
            damage = Mathf.Max(0, ((attacker.EnemyAttributes.Str - (target.member.Def/4)) + Random.Range(0, 4)));
        }
        else
        {
            damage = Mathf.Max(0, ((attacker.EnemyAttributes.Str - (target.member.Def/4)) + Random.Range(0, 2)));
        }
        target.CharacterAttributes.Hp -= damage;
        return damage;
    }

    // Calculates damage done by player character magic attack on enemy target
    public static int MagicAttackDamage(Ally attacker, Enemy target)
    {
        int damage = Mathf.Max(0, ((attacker.member.Intelligence - (target.EnemyAttributes.Def/4)) + Random.Range(0, 3)));
        target.EnemyAttributes.Hp -= damage;
        return damage;
    }

    // Calculates damage done by enemy character magic attack on player character
    public static int EnemyMagicAttackDamage(Enemy attacker, Ally target)
    {
        int damage = new int();
        if (attacker.EnemyAttributes.Intelligence >= 12 )
        {
            damage = Mathf.Max(0, ((attacker.EnemyAttributes.Intelligence - (target.member.Def/4)) + Random.Range(3, 6)));
        } else
        {
            damage = Mathf.Max(0, ((attacker.EnemyAttributes.Intelligence - (target.member.Def/4)) + Random.Range(0, 3)));
        }
        target.CharacterAttributes.Hp -= damage;
        return damage;
    }

    // Calculates amount of damage healed when casting a heal magic on party
    public static int MagicHeal(Ally attacker, Ally target)
    {
        int heal = Mathf.Max(0, (attacker.member.Intelligence + Random.Range(3, 6)));
        target.CharacterAttributes.Hp += heal;
        return heal;
    }

    // Calculates amount of damage healed when enemy heals themselves
    public static int EnemyMagicHeal(Enemy attacker, Enemy target)
    {
        int heal = Mathf.Max(0, (attacker.EnemyAttributes.Intelligence));
        target.EnemyAttributes.Hp += heal;
        return heal;
    }
}
