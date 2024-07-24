using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Modified by adding comments, properties such as intelligence and expValue and goldValue, changing properties, changed to relate only to enemies.
 */

// Class for the enemy attributes
[System.Serializable]
public class EnemyAttributes
{
    private const int MAX_LVL = 99;
    private const int MAX_HP = 999;
    private const int MAX_STR = 99;
    private const int MAX_DEF = 99;
    private const int MAX_SPD = 99;
    private const int MAX_INT = 99;
    private const int MAX_EXP_VALUE = 100;
    private const int MAX_GOLD_VALUE = 100;


    [SerializeField] private int level;
    [SerializeField] private int hp;
    [SerializeField] private int maxhp;
    [SerializeField] private int str;
    [SerializeField] private int def;
    [SerializeField] private int spd;
    [SerializeField] private int intelligence;
    [SerializeField] private int expValue;
    [SerializeField] private int goldValue;

    public EnemyAttributes(int level, int hp, int maxhp, int str, int def, int spd, int intelligence, int expValue, int goldValue)
    {
        this.level = level;
        this.hp = hp;
        this.maxhp = maxhp;
        this.str = str;
        this.def = def;
        this.spd = spd;
        this.intelligence = intelligence;
        this.expValue = expValue;
        this.goldValue = goldValue;
    }

    public int Level
    {
        get => level;
        set
        {
            level = Mathf.Clamp(value, 1, MAX_LVL);
        }
    }
    public int Hp
    {
        get => hp;
        set
        {
            hp = Mathf.Clamp(value, 0, maxhp);
        }
    }

    public int MaxHp
    {
        get => maxhp;
        set
        {
            maxhp = Mathf.Clamp(value, 0, MAX_HP);
        }
    }

    public int Str
    {
        get => str;
        set
        {
            str = Mathf.Clamp(value, 0, MAX_STR);
        }
    }

    public int Def
    {
        get => def;
        set
        {
            def = Mathf.Clamp(value, 0, MAX_DEF);
        }
    }

    public int Spd
    {
        get => spd;
        set
        {
            spd = Mathf.Clamp(value, 0, MAX_SPD);
        }
    }

    public int Intelligence
    {
        get => intelligence;
        set
        {
            intelligence = Mathf.Clamp(value, 0, MAX_INT);
        }
    }

    public int ExpValue
    {
        get => expValue;
        set
        {
            expValue = Mathf.Clamp(value, 0, MAX_EXP_VALUE);
        }
    }

    public int GoldValue
    {
        get => goldValue;
        set
        {
            goldValue = Mathf.Clamp(value, 0, MAX_GOLD_VALUE);
        }
    }
}
