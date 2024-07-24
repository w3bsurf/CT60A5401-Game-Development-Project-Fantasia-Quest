using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Modified by removing unwanted properties and renaming some to better suit use case, modified equipping function. Added properties for attributes with equipment bonuses. Added comments.
 */

[CreateAssetMenu(fileName = "New Party Member", menuName = "New Party Member")]

// A scriptable object class for party members. Contains serialized fields that can be set in the unity editor.
public class PartyMember: ScriptableObject
{
    [SerializeField] private string charName;
    [SerializeField] private GameObject battlePartyMember;
    [SerializeField] private CharacterAttributes characterAttributes;
    [SerializeField] private Sprite charSprite;

    private Weapon equippedWeapon;
    private Armor equippedArmor;

    public string Name => charName;
    public GameObject BattlePartyMember => battlePartyMember;
    public CharacterAttributes CharacterAttributes => characterAttributes;
    public Sprite CharSprite => charSprite;
    public Weapon EquippedWeapon => equippedWeapon ?? Resources.Load<Weapon>("ScriptableObjects/Weapons/Empty");
    public Armor EquippedArmor => equippedArmor ?? Resources.Load<Armor>("ScriptableObjects/Armor/Empty");

    public int Spd => characterAttributes.Spd + EquippedArmor.SpeedBonus;
    public int Def => characterAttributes.Def + EquippedArmor.DefenceBonus;
    public int Str => characterAttributes.Str + EquippedWeapon.StrengthBonus;
    public int Intelligence => characterAttributes.Intelligence + EquippedWeapon.IntellingenceBonus;

    // Method for equipping a piece of equipment to a party member
    public void EquipEquipment(Equipment equipment)
    {
        if (Game.Manager.Party.EquippedItems.Contains(equipment))
        {
            Game.Manager.Party.EquippedItems.Remove(equipment);
        }
        if (equipment is Armor)
        {
            Game.Manager.Party.EquippedItems.Remove(equippedArmor);
            equippedArmor = equipment as Armor;
            Game.Manager.Party.EquippedItems.Add(equipment);
        }
        else
        {
            Game.Manager.Party.EquippedItems.Remove(equippedWeapon);
            equippedWeapon = equipment as Weapon;
            Game.Manager.Party.EquippedItems.Add(equipment);
        }
    }
}
