using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for the shop section of the shop menu
public class Shop : MonoBehaviour
{
    [SerializeField] private PartyInventoryList partyInventory;
    [SerializeField] private RectTransform shopInventory;

    // Shows the shop inventory when buying
    public void ShowShopInventory()
    {
        shopInventory.gameObject.SetActive(true);
    }

    // Hides the shop inventory
    public void HideShopInventory()
    {
        shopInventory.gameObject.SetActive(false);
    }

    // Shows the party inventory when selling
    public void ShowPartyInventory()
    {
        partyInventory.gameObject.SetActive(true);
    }

    // Hides the party inventory after selling
    public void HidePartyInventory()
    {
        partyInventory.gameObject.SetActive(false);
        partyInventory.DestroyItemElements();
    }
}
