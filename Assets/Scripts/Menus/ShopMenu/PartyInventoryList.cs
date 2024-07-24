using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for managing the party inventory list menu element when selling items
public class PartyInventoryList : MonoBehaviour
{
    [SerializeField] private ItemElementShop itemElementShop;
    [SerializeField] private EmptyElement emptyElement;

    void OnEnable()
    {
        if (Game.Manager.Party.Inventory.ItemList.Count > 0)
        {
            emptyElement.gameObject.SetActive(false);
            foreach (Item item in Game.Manager.Party.Inventory.ItemList)
            {
                ItemElementShop itemE = Instantiate(itemElementShop, this.gameObject.transform.position, Quaternion.identity);
                itemE.transform.SetParent(this.transform);
            }
        }
        else
        {
            emptyElement.gameObject.SetActive(true);
        }
    }

    // Destroys the item elements when not used
    public void DestroyItemElements()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<ItemElementShop>() != null)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
