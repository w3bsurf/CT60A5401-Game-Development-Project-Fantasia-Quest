using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for handling the shop menu logic and window
public class ShopMenu : MonoBehaviour
{
    [SerializeField] private ShopMenuSelector menuSelector;
    [SerializeField] private ShopMenuSelector buySelector;
    [SerializeField] private ShopMenuSelector sellSelector;
    [SerializeField] private AudioSource menuAccept;
    [SerializeField] private AudioSource menuCancel;
    [SerializeField] private AudioSource confirmed;
    [SerializeField] private AudioSource declined;

    private Animator animator;
    private Shop shop;
    private MenuState menuState;
    private enum MenuState
    {
        Main,
        Buy,
        Sell,
    }

    public bool IsOpen { get; private set; }
    public bool IsAnimating => (animator.IsAnimating());

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Game.Manager.State == GameState.ShopMenu)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Accept();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cancel();
            }
        }
    }

    // Plays the "shop_menu_open" animation which displays the shop menu to the player
    public void OpenMenu()
    {
        shop = GetComponentInChildren<Shop>();
        menuState = MenuState.Main;
        menuSelector.Index = 0;
        menuSelector.IsActive = true;
        IsOpen = true;
        animator.Play("shop_menu_open");
    }

    // Plays the "shop_menu_close" animation which closes the shop menu
    public void CloseMenu()
    {
        menuSelector.IsActive = false;
        IsOpen = false;
        animator.Play("shop_menu_close");
    }

    // Function to accept current menu selection and process it
    public void Accept()
    {
        switch (menuState)
        {
            case MenuState.Main:
                ProcessMainMenuSelection();
                break;
            case MenuState.Buy:
                Buy();
                break;
            case MenuState.Sell:
                Sell();
                break;
        }
        menuAccept.Play();
    }

    // Function to cancel menu selection and return to previous state
    public void Cancel()
    {
        switch (menuState)
        {
            case MenuState.Main:
                CloseMenu();
                break;
            case MenuState.Buy:
                buySelector.IsActive = false;
                buySelector.Index = 0;
                buySelector.gameObject.SetActive(false);
                shop.HideShopInventory();
                menuSelector.IsActive = true;
                menuState = MenuState.Main;
                break;
            case MenuState.Sell:
                sellSelector.IsActive = false;
                sellSelector.Index = 0;
                sellSelector.gameObject.SetActive(true);
                shop.HidePartyInventory();
                menuSelector.IsActive = true;
                menuState = MenuState.Main;
                break;
        }
        menuCancel.Play();
    }

    // Function to process current menu selection
    private void ProcessMainMenuSelection()
    {
        switch (menuSelector.Index)
        {
            case 0:
                ShowShop();
                break;
            case 1:
                ShowPartyInventory();
                break;
        }
    }

    // Function for showing the shop inventory when buying
    private void ShowShop()
    {
        shop.ShowShopInventory();
        menuState = MenuState.Buy;
        menuSelector.IsActive = false;
        buySelector.gameObject.SetActive(true);
        buySelector.IsActive = true;
        buySelector.Index = 0;
    }

    //  Function for selecting an item to buy
    private void Buy()
    {
        switch (buySelector.Index)
        {
            case 0:
                if (Game.Manager.Party.Inventory.ItemList.Count < 8 && Game.Manager.Party.Inventory.Gold >= 5)
                {
                    if (Game.Manager.Party.Inventory.AddItem("Simple Potion"))
                    {
                        Game.Manager.Party.Inventory.DeductGold(5);
                        confirmed.Play();
                    }
                    else
                    {
                        declined.Play();
                    }
                }
                else
                {
                    declined.Play();
                }
                break;
            case 1:
                if (Game.Manager.Party.Inventory.ItemList.Count < 8 && Game.Manager.Party.Inventory.Gold >= 20)
                {
                    if (Game.Manager.Party.Inventory.AddItem("Greater Potion"))
                    {
                        Game.Manager.Party.Inventory.DeductGold(20);
                        confirmed.Play();
                    }
                    else
                    {
                        declined.Play();
                    }
                }
                else
                {
                    declined.Play();
                }
                break;
        }
    }

    // Function for showing the party inventory when selling
    private void ShowPartyInventory()
    {
        menuSelector.IsActive = false;
        shop.ShowPartyInventory();
        if (Game.Manager.Party.Inventory.ItemList.Count > 0)
        {
            sellSelector.gameObject.SetActive(true);
            sellSelector.IsActive = true;
            sellSelector.Index = 0;
        }
        menuState = MenuState.Sell;
    }

    // Function to sell selected item
    private void Sell()
    {
        if (Game.Manager.Party.Inventory.ItemList.Count > 0)
        {
            Item item = Game.Manager.Party.Inventory.ItemList[sellSelector.Index];
            if (item is Potion)
            {
                sellSelector.IsActive = false;
                sellSelector.gameObject.SetActive(false);
                item.SellItem();
                confirmed.Play();
            }
            shop.HidePartyInventory();
            menuSelector.IsActive = true;
            menuState = MenuState.Main;
        }
    }
}
