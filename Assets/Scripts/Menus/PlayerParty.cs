using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for the player party UI element
public class PlayerParty : MonoBehaviour
{
    [SerializeField] private GameObject characterInfoBattlePrefab;

    void Start()
    {
        GenerateBattlePartyInfo();
    }

    // Creates a character info UI element in the player party window in battle for each party member
    private void GenerateBattlePartyInfo()
    {
        foreach (PartyMember member in Game.Manager.Party.PartyMembers)
        {
            Instantiate(characterInfoBattlePrefab, this.gameObject.transform);
        }
    }
}
