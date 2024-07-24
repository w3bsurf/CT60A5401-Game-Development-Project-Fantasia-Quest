using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Modified by adding comments, setting up to load the characters, added equipped items list and removed unnecessary methods and properties.
 */

// Class for the player party
public class Party
{
    // List of PartyMembers in the game
    private List<PartyMember> partyMembers = new List<PartyMember>();
    // Party inventory
    private Inventory inventory = new Inventory();
    private List<Equipment> equippedItems = new List<Equipment>();
    public List<PartyMember> PartyMembers => partyMembers;
    public Inventory Inventory => inventory;
    public List<Equipment> EquippedItems => equippedItems;

    // Party builder
    public void CreateParty()
    {
        PartyMember Hero = ScriptableObject.Instantiate(Resources.Load<PartyMember>("ScriptableObjects/PartyMembers/Hero"));
        partyMembers.Add(Hero);
        inventory.Initialize();
    }

    // Add a new party member to the party
    public void AddPartyMember(PartyMember member)
    {
        if (partyMembers.Contains(member)) 
        {
            return;
        }
        partyMembers.Add(member);
    }
}
