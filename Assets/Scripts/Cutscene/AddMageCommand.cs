using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
// Cutscene command for adding the mage party member during a cutscene
public class AddMageCommand : CutsceneCommand
{
    [SerializeField] private NPC npc;
    public bool IsFinished { get; private set; }

    // Coroutine to add the mage character as a party member during a cutscene. Destroys mage npc in the game world.
    public IEnumerator Co_Execute()
    {
        PartyMember Mage = ScriptableObject.Instantiate(Resources.Load<PartyMember>("ScriptableObjects/PartyMembers/Mage"));
        Game.Manager.Party.AddPartyMember(Mage);
        npc.DestroyCharacter();
        yield return null;
        IsFinished = true;
    }

    // Changes the to string return value to a more suitable string
    public override string ToString()
    {
        return "Add Mage";
    }
}
