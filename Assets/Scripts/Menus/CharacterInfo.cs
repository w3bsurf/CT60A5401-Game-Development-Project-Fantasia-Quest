using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Modified by adding comments, setting approriate character attribute fields, added Update method to keep the info in the menu consistent with the game state.
 */

// Class for the character info UI elements in the main menu.
public class CharacterInfo : MonoBehaviour
{
    private PartyMember partyMember;

    [SerializeField] private Image charSprite;
    [SerializeField] private TextMeshProUGUI charName;
    [SerializeField] private TextMeshProUGUI charLevel;
    [SerializeField] private TextMeshProUGUI charHP;
    [SerializeField] private TextMeshProUGUI charAttributesSTR;
    [SerializeField] private TextMeshProUGUI charAttributesINT;
    [SerializeField] private TextMeshProUGUI charAttributesDEF;
    [SerializeField] private TextMeshProUGUI charAttributesSPD;

    public PartyMember PartyMember => partyMember;

    void OnEnable()
    {
        int i = this.gameObject.transform.GetSiblingIndex();
        if (i < Game.Manager.Party.PartyMembers.Count)
        {
            partyMember = Game.Manager.Party.PartyMembers[i];
            SetMenuAttributes();
        }
    }

    // Makes sure that the character info that is displayed in the main menu is consistent with the actual character stats
    private void Update()
    {
        charLevel.text = $"Level: {partyMember.CharacterAttributes.Level}";
        charHP.text = $"HP: {partyMember.CharacterAttributes.Hp}/{partyMember.CharacterAttributes.MaxHp}";
        charAttributesSTR.text = $"STR: {partyMember.Str}";
        charAttributesINT.text = $"INT: {partyMember.Intelligence}";
        charAttributesDEF.text = $"DEF: {partyMember.Def}";
        charAttributesSPD.text = $"SPD: {partyMember.Spd}";
    }

    // Set the party member info based on the character's actual stats
    public void SetMenuAttributes()
    {
        charSprite.sprite = partyMember.CharSprite;
        charName.text = partyMember.Name;
        charLevel.text = $"Level: {partyMember.CharacterAttributes.Level}";
        charHP.text = $"HP: {partyMember.CharacterAttributes.Hp}/{partyMember.CharacterAttributes.MaxHp}";
        charAttributesSTR.text = $"STR: {partyMember.Str}";
        charAttributesINT.text = $"INT: {partyMember.Intelligence}";
        charAttributesDEF.text = $"DEF: {partyMember.Def}";
        charAttributesSPD.text = $"SPD: {partyMember.Spd}";
    }
}
