using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Class for the character equipment info UI element
public class CharEquipmentInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI charWeapon;
    [SerializeField] private TextMeshProUGUI charArmor;

    // Set the equipment info based on the character's actual equipment
    public void SetMenuText(PartyMember partyMember)
    {
        charWeapon.text = partyMember.EquippedWeapon.EquipmentName;
        charArmor.text = partyMember.EquippedArmor.EquipmentName;
    }
}
