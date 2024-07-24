using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Potion", menuName = "New Potion")]

// Potion item class. Heals the characters for a certain amount of health
public class Potion : Item
{
    [SerializeField] private int healAmount;

    private bool usable = false;

    public int HealAmount => healAmount;

    // Sells item and removes from inventory
    public override void SellItem()
    {
        Game.Manager.Party.Inventory.AddGold(this.ItemValue);
        Game.Manager.Party.Inventory.RemoveItem(this);
    }

    // use item from main menu and remove from inventory
    public override bool UseItem() 
    {
        foreach (PartyMember member in Game.Manager.Party.PartyMembers)
        {
            if (member.CharacterAttributes.Hp < member.CharacterAttributes.MaxHp)
            {
                usable = true;
                member.CharacterAttributes.Hp += healAmount;
            }
        }

        if (usable)
        {
            Game.Manager.Party.Inventory.RemoveItem(this);
            return usable;
        }
        else
        {
            return usable;
        }

    }

    // use item in combat and remove from inventory
    public override int UseItemCombat(List<Ally> allies) 
    {
        foreach (Ally ally in allies)
        {
            ally.CharacterAttributes.Hp += healAmount;
        }

        Game.Manager.Party.Inventory.RemoveItem(this);

        return healAmount;
    }   
}
