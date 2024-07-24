using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
// Cutscene command for showing a character during a cutscene
public class ShowCharacterCommand : CutsceneCommand
{
    [SerializeField] private NPC npc;
    public bool IsFinished { get; private set; }

    // Coroutine to show a specific character during a cutscene
    public IEnumerator Co_Execute()
    {
        npc.gameObject.SetActive(true);
        yield return null;
        IsFinished = true;
    }

    // Changes the to string return value to a more suitable string
    public override string ToString()
    {
        return "Show Character";
    }
}
