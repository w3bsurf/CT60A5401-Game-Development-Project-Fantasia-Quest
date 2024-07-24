using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Modified with additional property intelligenceBonus and comments.
 */

[CreateAssetMenu(fileName = "New Weapon", menuName = "New Weapon")]

// Weapon class. Provides set bonuses to character strength and intelligence.
public class Weapon : Equipment
{
    [SerializeField] private int strengthBonus;
    [SerializeField] private int intellingenceBonus;

    public int StrengthBonus => strengthBonus;
    public int IntellingenceBonus => intellingenceBonus;
}
