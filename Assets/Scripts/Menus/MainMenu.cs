using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Modified by adding comments and changing variable names to be more suitable to the game game, added many more methods, variables and states.
 */

// The main menu class. Mainly contains required properties and animations for menu opening and closing.
public class MainMenu : MonoBehaviour
{
    [SerializeField] private MenuSelector menuSelector;
    [SerializeField] private MenuSelector characterSelector;
    [SerializeField] private MenuSelector equipSelector;
    [SerializeField] private MenuSelector equipmentSelector;
    [SerializeField] private MenuSelector itemSelector;
    [SerializeField] private MenuSelectorHorizontal confirmSelector;
    [SerializeField] private RectTransform startMenuConfirmation;
    [SerializeField] private AudioSource menuAccept;
    [SerializeField] private AudioSource menuCancel;
    [SerializeField] private AudioSource itemUsed;
    [SerializeField] private AudioSource itemNotUsed;

    private Animator animator;
    private PartyInfo partyInfo;
    private MenuState menuState = MenuState.Closed;
    private enum MenuState
    {
        Closed,
        Main,
        EquipCharSelect,
        Equipment,
        EquipmentSelect,
        Inventory,
        Confirm,
    }

    public bool IsOpen {  get; private set; }
    public bool IsAnimating => (animator.IsAnimating());

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        
        if (Game.Manager.State == GameState.Menu && !IsAnimating)
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

    // Starts coroutine to open the menu
    public void OpenMenu()
    {
        StartCoroutine(Co_OpenMenu());
        partyInfo = GetComponentInChildren<PartyInfo>();
        partyInfo.ShowPartyInfo();
        menuSelector.Index = 0;
        menuSelector.IsActive = true;
        IsOpen = true;
    }

    // Plays the "menu_open" animation which displays the main menu to the player
    private IEnumerator Co_OpenMenu()
    {
        animator.Play("menu_open");
        yield return new WaitForEndOfFrame();
        while (IsAnimating)
        {
            yield return null;
        }
        menuState = MenuState.Main;
    }

    // Plays the "menu_close" animation which closes the main menu
    public void CloseMenu()
    {
        menuSelector.IsActive = false;
        IsOpen = false;
        animator.Play("menu_close");
        menuState = MenuState.Closed;
    }

    // Function to accept current menu selection and process it
    public void Accept()
    {
        switch (menuState)
        {
            case MenuState.Main:
                ProcessMainMenuSelection();
                break;
            case MenuState.EquipCharSelect:
                CharEquipInfo();
                break;
            case MenuState.Equipment:
                SelectEquipment();
                break;
            case MenuState.EquipmentSelect:
                EquipSelectedItem();
                break;
            case MenuState.Inventory:
                if (itemSelector.IsActive) UseSelectedItem();
                break;
            case MenuState.Confirm:
                GoToStartMenu();
                break;
        }
        menuAccept.Play();
    }

    // Function to cancel menu selection and return to previous state
    public void Cancel()
    {
        if (!IsAnimating)
        {
            switch (menuState)
            {
                case (MenuState.Main):
                    CloseMenu();
                    break;
                case (MenuState.EquipCharSelect):
                    characterSelector.Index = 0;
                    characterSelector.IsActive = false;
                    characterSelector.gameObject.SetActive(false);
                    menuSelector.IsActive = true;
                    menuState = MenuState.Main;
                    break;
                case (MenuState.Equipment):
                    partyInfo.ShowPartyInfo();
                    equipSelector.Index = 0;
                    equipSelector.IsActive = false;
                    characterSelector.IsActive = true;
                    characterSelector.gameObject.SetActive(true);
                    menuState = MenuState.EquipCharSelect;
                    break;
                case (MenuState.EquipmentSelect):
                    PartyMember selectedMember = Game.Manager.Party.PartyMembers[characterSelector.Index];
                    partyInfo.ShowEquipmentInfo(selectedMember);
                    equipmentSelector.IsActive = false;
                    equipmentSelector.gameObject.SetActive(false);
                    equipSelector.gameObject.SetActive(true);
                    equipSelector.IsActive = true;
                    menuState = MenuState.Equipment;
                    PartyInfo.ListOfEquipment.Clear();
                    break;
                case (MenuState.Inventory):
                    itemSelector.Index = 0;
                    itemSelector.IsActive = false;
                    itemSelector.gameObject.SetActive(false);
                    partyInfo.ShowPartyInfo();
                    menuSelector.IsActive = true;
                    menuState = MenuState.Main;
                    break;
                case (MenuState.Confirm):
                    startMenuConfirmation.gameObject.SetActive(false);
                    confirmSelector.Index = 0;
                    confirmSelector.IsActive = false;
                    confirmSelector.gameObject.SetActive(false);
                    menuSelector.IsActive = true;
                    menuState = MenuState.Main;
                    break;
            }
            menuCancel.Play();
        }     
    }

    // Function to process current menu selection
    private void ProcessMainMenuSelection()
    {
        switch (menuSelector.Index)
        {
            case 0:
                Items();
                break;
            case 1:
                Equip();
                break;
            default:
                ConfirmSelection();
                break;
        }
    }

    // Function for selecting a character to equip for
    private void CharEquipInfo()
    {
        PartyMember selectedMember = Game.Manager.Party.PartyMembers[characterSelector.Index];
        partyInfo.ShowEquipmentInfo(selectedMember);
        menuState = MenuState.Equipment;
        characterSelector.IsActive = false;
        characterSelector.gameObject.SetActive(false);
        equipSelector.gameObject.SetActive(true);
        equipSelector.Index = 0;
        equipSelector.IsActive = true;
    }

    //  Function for selecting a piece of equipment to change
    private void SelectEquipment()
    {
        PartyMember selectedMember = Game.Manager.Party.PartyMembers[characterSelector.Index];
        if (equipSelector.Index == 0)
        {
            string selectedEquipment = "Weapon";
            partyInfo.ShowEquipmentList(selectedEquipment, selectedMember);
        }
        else
        {
            string selectedEquipment = "Armor";
            partyInfo.ShowEquipmentList(selectedEquipment, selectedMember);
        }
        menuState = MenuState.EquipmentSelect;
        equipSelector.IsActive = false;
        equipSelector.gameObject.SetActive(false);
        equipmentSelector.gameObject.SetActive(true);
        equipmentSelector.Index = 0;
        equipmentSelector.IsActive = true;
    }

    // Function to equip selected item
    private void EquipSelectedItem()
    {
        PartyMember selectedMember = Game.Manager.Party.PartyMembers[characterSelector.Index];
        Equipment equipment = PartyInfo.ListOfEquipment[equipmentSelector.Index];
        selectedMember.EquipEquipment(equipment);
        partyInfo.ShowEquipmentInfo(selectedMember);
        equipmentSelector.IsActive = false;
        equipmentSelector.gameObject.SetActive(false);
        equipSelector.gameObject.SetActive(true);
        equipSelector.Index = 0;
        equipSelector.IsActive = true;
        menuState = MenuState.Equipment;
        PartyInfo.ListOfEquipment.Clear();
    }

    // Function to use selected item
    private void UseSelectedItem()
    {
        if (Game.Manager.Party.Inventory.ItemList.Count == 1)
        {
            Item item = Game.Manager.Party.Inventory.ItemList[itemSelector.Index];
            if (item is Potion)
            {
                itemSelector.IsActive = false;
                itemSelector.gameObject.SetActive(false);
                if(item.UseItem())
                {
                    itemUsed.Play();
                }
                else
                {
                    itemNotUsed.Play();
                }
            }
            partyInfo.ShowPartyInfo();
            menuSelector.IsActive = true;
            menuState = MenuState.Main;
        }
        else
        {
            Item item = Game.Manager.Party.Inventory.ItemList[itemSelector.Index];
            if (item is Potion)
            {
                itemSelector.IsActive = false;
                itemSelector.gameObject.SetActive(false);
                if (item.UseItem())
                {
                    itemUsed.Play();
                }
                else
                {
                    itemNotUsed.Play();
                }
            }
            partyInfo.ShowPartyInfo();
            menuSelector.IsActive = true;
            menuState = MenuState.Main;
        }
    }

    // Function for the start menu option
    private void ConfirmSelection()
    {
        menuState = MenuState.Confirm;
        menuSelector.IsActive = false;
        startMenuConfirmation.gameObject.SetActive(true);
        confirmSelector.gameObject.SetActive(true);
        confirmSelector.Index = 0;
        confirmSelector.IsActive = true;
    }

    // Function for the equipment menu option
    private void Equip()
    {
        menuState = MenuState.EquipCharSelect;
        menuSelector.IsActive = false;
        characterSelector.gameObject.SetActive(true);
        characterSelector.Index = 0;
        characterSelector.IsActive = true;
    }

    // Function to show items in inventory
    private void Items()
    {
        menuState = MenuState.Inventory;
        menuSelector.IsActive = false;
        partyInfo.ShowItems();
        if (Game.Manager.Party.Inventory.ItemList.Count > 0)
        {
            itemSelector.IsActive = true;
            itemSelector.gameObject.SetActive(true);
            itemSelector.Index = 0;
        }
    }

    // Function to go to the start menu
    private void GoToStartMenu()
    {
        switch (confirmSelector.Index)
        {
            case 0:
                SceneLoader.LoadStartMenu();
                break;
            case 1:
                startMenuConfirmation.gameObject.SetActive(false);
                confirmSelector.Index = 0;
                confirmSelector.IsActive = false;
                confirmSelector.gameObject.SetActive(false);
                menuSelector.IsActive = true;
                menuState = MenuState.Main;
                break;
        }
    }
}
