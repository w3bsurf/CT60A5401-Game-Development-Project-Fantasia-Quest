using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for managing the item list menu element in battle
public class ItemListBattle : MonoBehaviour
{
    [SerializeField] private ItemElementBattle itemElement;
    [SerializeField] private EmptyElementBattle emptyElement;

    void OnEnable()
    {

        if (Game.Manager.Party.Inventory.ItemList.Count > 0)
        {
            foreach (Item item in Game.Manager.Party.Inventory.ItemList)
            {
                ItemElementBattle itemE = Instantiate(itemElement, this.gameObject.transform.position, Quaternion.identity);
                itemE.transform.SetParent(this.transform);
            }
        }
        else
        {
            emptyElement.gameObject.SetActive(true);
        }
    }

    // Destroys the equipment elements when not used
    public void DestroyItemElements()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<ItemElementBattle>() != null)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
