using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Class for item ui elements when choosing what to use
public class ItemElement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    private Item item;

    public Item Item => item;

    void Start()
    {
        int i = this.gameObject.transform.GetSiblingIndex() - 2;
        item = Game.Manager.Party.Inventory.ItemList[i];
        SetMenuText();
    }

    // Sets the equipment UI element text accordingly
    public void SetMenuText()
    {
        itemName.text = item.ItemName;
        itemDescription.text = item.ItemDescription;
    }
}
