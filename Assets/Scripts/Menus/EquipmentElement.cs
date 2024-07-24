using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Class for equipment ui elements when choosing what to equip
public class EquipmentElement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI equipmentName;
    private Equipment equipment;

    public Equipment Equipment => equipment;

    void Start()
    {
        int i = this.gameObject.transform.GetSiblingIndex()-1;
        equipment = PartyInfo.ListOfEquipment[i];
        SetMenuText();
    }

    // Sets the equipment UI element text accordingly
    public void SetMenuText()
    {
        equipmentName.text = equipment.EquipmentName;
    }
}
