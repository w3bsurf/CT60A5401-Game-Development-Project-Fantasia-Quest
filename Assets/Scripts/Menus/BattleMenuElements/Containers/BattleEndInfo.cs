using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Class for handling the BattleEndInfo ui element in battle
public class BattleEndInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gold;
    [SerializeField] private TextMeshProUGUI xp;
    [SerializeField] private TextMeshProUGUI item;
    [SerializeField] private RectTransform levelUp;

    // Sets the text at the end of battle based on gold, xp, item and levels received
    public void SetMenuText(int goldAmount, int xpAmount, string itemReceived, bool leveledUp)
    {
        gold.text = goldAmount.ToString();
        xp.text = xpAmount.ToString();
        item.text = itemReceived;

        if (leveledUp)
        {
            levelUp.gameObject.SetActive(true);
        }
    }
}
