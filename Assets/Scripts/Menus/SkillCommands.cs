using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for the SkillCommands section of the battle menu
public class SkillCommands : MonoBehaviour
{

    // Shows the skill for a specific party member in battle
    public void ShowSkills(PartyMember partyMember)
    {
        if (partyMember.Name == "Hero")
        {
            foreach (Transform child in transform)
            {
                if (child.GetComponent<DoubleAttackSkillElement>() != null)
                {
                    child.gameObject.SetActive(true);
                }
                else
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            foreach (Transform child in transform)
            {
                if (child.GetComponent<MagicAttackSkillElement>() != null)
                {
                    child.gameObject.SetActive(true);
                }
                else if (child.GetComponent<HealMagicSkillElement>() != null) 
                {
                    child.gameObject.SetActive(true);
                }
                else
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }
}
