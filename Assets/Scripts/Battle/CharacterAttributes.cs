using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Modified by adding comments, properties such as intelligence, exp changing properties and adding methods for gaining exp and leveling up.
 */

[System.Serializable]
// Class for the attributes of player characters
public class CharacterAttributes
{
    private const int MAX_LVL = 99;
    private const int MAX_HP = 999;
    private const int MAX_STR = 99;
    private const int MAX_DEF = 99;
    private const int MAX_SPD = 99;
    private const int MAX_INT = 99;
    private const int BASE_EXP_REQUIRED = 10;

    [SerializeField] private int level;
    [SerializeField] private int hp;
    [SerializeField] private int maxhp;
    [SerializeField] private int str;
    [SerializeField] private int def;
    [SerializeField] private int spd;
    [SerializeField] private int intelligence;
    [SerializeField] private string charClass;

    private int exp;
    private int expRequired;

    public CharacterAttributes(int level, int hp, int maxhp, int str, int def, int spd, int intelligence)
    {
        this.level = level;
        this.hp = hp;
        this.maxhp = maxhp;
        this.str = str;
        this.def = def;
        this.spd = spd;
        this.intelligence = intelligence;
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
        set {
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

    public int Exp => exp;

    public int ExpRequired => expRequired; 

    // Function for adding gained experience to player character and leveling up if required exp reached.
    public bool GainExp(int expGained)
    {
        expRequired = Mathf.RoundToInt((BASE_EXP_REQUIRED * level) + (3 * Mathf.Pow(level, 2)));
        exp += expGained;

        if (exp >= expRequired)
        {
            levelUp();
            return true;
        }
        else
        {
            return false;
        }
    }

    // Function for leveling up and increasing character attributes based on character class
    public void levelUp()
    {
        level++;
        expRequired = Mathf.RoundToInt((BASE_EXP_REQUIRED * level) + Mathf.Pow(level, 2));

        if (charClass == "hero")
        {
            maxhp += 4;
            hp = maxhp;
            str += 1;
            def += 2;
            spd += 1;
            intelligence += 1;
        }
        else if (charClass == "mage")
        {
            maxhp += 3;
            hp = maxhp;
            str += 1;
            def += 2;
            spd += 2;
            intelligence += 1;
        }
        exp = 0;
    }
}
