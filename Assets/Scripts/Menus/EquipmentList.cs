using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for managing the equipment list menu element
public class EquipmentList : MonoBehaviour
{
    [SerializeField] private EquipmentElement equipmentElement;

    void OnEnable()
    {
        foreach (Equipment item in PartyInfo.ListOfEquipment)
        {
            EquipmentElement equipmentE = Instantiate(equipmentElement, this.gameObject.transform.position, Quaternion.identity);
            equipmentE.transform.SetParent(this.transform);
        }
    }

    // Destroys the equipment elements when not used
    public void DestroyEquipmentElements()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<EquipmentElement>() != null)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
