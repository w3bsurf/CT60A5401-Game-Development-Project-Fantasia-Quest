using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//Class for the UI element showing current gold amount
public class CurrencyInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gold;

    void OnAwake()
    {
        SetMenuText();
    }

    // Makes sure that the info that is displayed in the main menu is consistent with the actual gold amount
    private void Update()
    {
        gold.text = $"Gold: {Game.Manager.Party.Inventory.Gold}";
    }

    // Set the currency info based on the party's actual gold
    public void SetMenuText()
    {
        gold.text = $"Gold: {Game.Manager.Party.Inventory.Gold}";
    }
}
