using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
// Cutscene command for hiding a character during a cutscene
public class HideCharacterCommand : CutsceneCommand
{
    [SerializeField] private NPC npc;
    public bool IsFinished { get; private set; }

    // Coroutine to hide a specific character during a cutscene
    public IEnumerator Co_Execute()
    {
        npc.gameObject.SetActive(false);
        Game.Manager.Map.OccupiedCells.Remove(npc.CurrentCell);
        yield return null;
        IsFinished = true;
    }

    // Changes the to string return value to a more suitable string
    public override string ToString()
    {
        return "Hide Character";
    }
}
