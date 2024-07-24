using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Abstract class for different types of items
public class Item : ScriptableObject
{
    [SerializeField] private string itemName;
    [SerializeField] private string itemDescription;
    [SerializeField] private int itemValue;

    public string ItemName => itemName;
    public string ItemDescription => itemDescription;
    public int ItemValue => itemValue;

    // Virtual function for using an item
    public virtual bool UseItem() {
        return false;
    }

    // Virtual function for selling an item
    public virtual void SellItem()
    {
    }

    // Virtual function for using an item in combat
    public virtual int UseItemCombat(List<Ally> allies) { 
        return 0;
    }
    
}
