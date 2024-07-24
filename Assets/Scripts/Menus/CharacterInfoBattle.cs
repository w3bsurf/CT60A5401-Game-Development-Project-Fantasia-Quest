using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Class for the party member info UI element in battles
public class CharacterInfoBattle : MonoBehaviour
{
    private PartyMember partyMember;
    private Ally ally;
    private Battle battle;

    [SerializeField] private TextMeshProUGUI charName;
    [SerializeField] private TextMeshProUGUI charHP;

    void Awake()
    {
        battle = GameObject.FindObjectOfType<Battle>();
        int i = this.gameObject.transform.GetSiblingIndex();
        partyMember = Game.Manager.Party.PartyMembers[i];
        ally = battle.Allies[i];
        ally.AllyDefeated += RemoveAllyInfoBattle;
    }

    // Set the party member info in the battle UI based on the character's actual stats
    public void Update()
    {
        charName.text = partyMember.Name;
        charHP.text = $"HP: {partyMember.CharacterAttributes.Hp} / {partyMember.CharacterAttributes.MaxHp}";
    }

    // Remove an enemy's ui element when they are defeated in battle
    private void RemoveAllyInfoBattle()
    {
        ally.AllyDefeated -= RemoveAllyInfoBattle;
        Destroy(this.gameObject);
    }
}
