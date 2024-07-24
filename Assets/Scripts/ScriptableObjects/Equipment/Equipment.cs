using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Abstract class for different types of equipment
public class Equipment : ScriptableObject
{
    [SerializeField] private string equipmentName;

    public string EquipmentName => equipmentName;
}
