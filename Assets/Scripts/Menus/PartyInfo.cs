using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for the party info section of the main menu
public class PartyInfo : MonoBehaviour
{
    [SerializeField] private GameObject characterInfoPrefab;
    [SerializeField] private CharEquipmentInfo charEquipmentInfo;
    [SerializeField] private EquipmentList equipmentList;
    [SerializeField] private ItemList itemList;

    public static List<Equipment> ListOfEquipment = new List<Equipment>();
    
    // Shows the party info by setting gameobjects active
    public void ShowPartyInfo()
    {
        itemList.DestroyItemElements();
        foreach (Transform child in transform)
        {
            if (child.GetComponent<CharacterInfo>() != null)
            {
                child.gameObject.SetActive(true);
                if (child.GetComponent<CharacterInfo>().PartyMember == null)
                {
                    child.gameObject.SetActive(false);
                }
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    // Shows the equipment info for a character
    public void ShowEquipmentInfo(PartyMember partyMember)
    {
        equipmentList.DestroyEquipmentElements();
        foreach (Transform child in transform)
        {
            if (child.GetComponent<CharacterInfo>() == null)
            {
                continue;
            }
            else if (child.GetComponent<CharacterInfo>().PartyMember != partyMember)
            {
                child.gameObject.SetActive(false);
            }
        }
        equipmentList.gameObject.SetActive(false);
        charEquipmentInfo.SetMenuText(partyMember);
        charEquipmentInfo.gameObject.SetActive(true);
        ListOfEquipment.Clear();
    }

    // Show the equipment list
    public void ShowEquipmentList(string equipmentType, PartyMember partyMember)
    {
        if (equipmentType == "Weapon")
        {
            foreach (Equipment equipment in Game.Manager.Party.Inventory.EquipmentList)
            {
                if (!(Game.Manager.Party.EquippedItems.Contains(equipment)) && equipment is Weapon)
                {
                    ListOfEquipment.Add(equipment);
                }
            }
            if (ListOfEquipment.Count == 0)
            {
                ListOfEquipment.Add(Resources.Load<Weapon>("ScriptableObjects/Weapons/Empty"));
            }
        }
        else
        {
            foreach (Equipment equipment in Game.Manager.Party.Inventory.EquipmentList)
            {
                if (!(Game.Manager.Party.EquippedItems.Contains(equipment)) && equipment is Armor)
                {
                    ListOfEquipment.Add(equipment);
                }
            }
            if (ListOfEquipment.Count == 0)
            {
                ListOfEquipment.Add(Resources.Load<Armor>("ScriptableObjects/Armor/Empty"));
            }
        }

        charEquipmentInfo.gameObject.SetActive(false);
        foreach (Transform child in transform)
        {
            if (child.GetComponent<CharacterInfo>() == null)
            {
                continue;
            }
            else if (child.GetComponent<CharacterInfo>().PartyMember != partyMember)
            {
                child.gameObject.SetActive(false);
            }
        }
        equipmentList.gameObject.SetActive(true);
    }

    // Shows the item list
    public bool ShowItems()
    {
        itemList.gameObject.SetActive(false);
        itemList.DestroyItemElements();
        foreach (Transform child in transform)
        {
            if (child.GetComponent<CharacterInfo>() != null)
            {
                child.gameObject.SetActive(false);
            }
        }

        itemList.gameObject.SetActive(true);
        return true;
    }
}
