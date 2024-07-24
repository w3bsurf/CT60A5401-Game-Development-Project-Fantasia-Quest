using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Modified with additional property speedBonus and comments.
 */

[CreateAssetMenu(fileName = "New Armor", menuName = "New Armor")]

// Armor class. Provides set bonuses to character defence and speed
public class Armor : Equipment
{
    [SerializeField] private int defenceBonus;
    [SerializeField] private int speedBonus;

    public int DefenceBonus => defenceBonus;
    public int SpeedBonus => speedBonus;
}
