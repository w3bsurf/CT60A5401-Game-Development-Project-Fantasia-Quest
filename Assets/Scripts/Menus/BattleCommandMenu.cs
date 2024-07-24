using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for the BattleCommand section of the battle menu
public class BattleCommandMenu : MonoBehaviour
{
    [SerializeField] private SkillCommands skillCommands;
    [SerializeField] private ItemListBattle itemListBattle;

    // Shows the basic battle commands for a character in battle
    public void ShowCommands()
    {
        itemListBattle.DestroyItemElements();
        foreach (Transform child in transform)
        {
            if (child.GetComponent<CommandElement>() != null)
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
    }
    
    // Shows the skills for a character in battle
    public void ShowSkills(PartyMember partyMember)
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<CommandElement>() != null)
            {
                child.gameObject.SetActive(false);
            }
            else if (child.GetComponent<SkillCommands>() != null) 
            {
                child.gameObject.SetActive(true);
            }
        }
        skillCommands.ShowSkills(partyMember);
    }

    // Shows the item list in battle
    public void ShowItemList()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<CommandElement>() != null)
            {
                child.gameObject.SetActive(false);
            }
        }
        itemListBattle.gameObject.SetActive(true);
    }
}
