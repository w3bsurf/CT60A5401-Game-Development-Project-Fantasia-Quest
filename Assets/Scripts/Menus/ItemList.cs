using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for managing the item list menu element
public class ItemList : MonoBehaviour
{
    [SerializeField] private ItemElement itemElement;
    [SerializeField] private EmptyElement emptyElement;

    void OnEnable()
    {
        if (Game.Manager.Party.Inventory.ItemList.Count > 0) 
        {
            foreach (Item item in Game.Manager.Party.Inventory.ItemList)
            {
                ItemElement itemE = Instantiate(itemElement, this.gameObject.transform.position, Quaternion.identity);
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
            if (child.GetComponent<ItemElement>() != null)
            {
                Destroy(child.gameObject);
            }
        }

    }
}
