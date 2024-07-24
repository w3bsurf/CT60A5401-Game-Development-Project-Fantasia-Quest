using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Added comments. Modified with approriate equipment and variable names added list for items, added methods for adding items, removing, adding gold, adding equipment, variables for gold and constants.
 */

// Class for handling the player party invetory
public class Inventory
{
    private const int MAX_ITEMS = 8;
    private const int MAX_GOLD = 999;

    private Potion simplePotion = Resources.Load<Potion>("ScriptableObjects/Items/SimplePotion");
    private Potion greaterPotion = Resources.Load<Potion>("ScriptableObjects/Items/GreaterPotion");

    private List<Equipment> equipmentList = new List<Equipment>();
    private List<Item> itemList = new List<Item>();
    private int gold;

    public IReadOnlyList<Equipment> EquipmentList => equipmentList;
    public IReadOnlyList<Item> ItemList => itemList;
    public int Gold
    {
        get => gold;
        set
        {
            gold = Mathf.Clamp(value, 0, MAX_GOLD);
        }
    }

    // Initialize starting equipment for hero
    public void Initialize()
    {
        itemList.Add(GameObject.Instantiate(simplePotion));
        itemList.Add(GameObject.Instantiate(simplePotion));
        gold = 10;
    }

    // Remove item from inventory list
    public void RemoveItem(Item item)
    {
        if (itemList.Contains(item))
        {
            itemList.Remove(item);
            GameObject.Destroy(item);
        }
    }

    // Add item to inventory
    public bool AddItem(string item)
    {
        if (item != null && itemList.Count < MAX_ITEMS)
        {
            if (item == "Simple Potion")
            {
                itemList.Add(GameObject.Instantiate(simplePotion));
            }
            else if (item == "Greater Potion")
            {
                itemList.Add(GameObject.Instantiate(greaterPotion));
            }
            return true;
        } 
        else
        {
            return false; 
        }
    }

    // Add gold to inventory
    public void AddGold(int goldAmount)
    {
        if (goldAmount < MAX_GOLD)
        {
            gold += goldAmount;
        }
    }

    // Remove gold from inventory
    public void DeductGold(int goldAmount)
    {
        gold -= goldAmount;
    }

    // Add equipment to inventory
    public void AddEquipment(Equipment equipment)
    {
        if (equipment != null)
        {
            equipmentList.Add(equipment);
        }
    }
}
