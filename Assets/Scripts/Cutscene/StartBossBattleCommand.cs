using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
// Cutscene command for starting a boss battle
public class StartBossBattleCommand : CutsceneCommand
{
    public bool IsFinished { get; private set; }
    
    // Coroutine to start the final boss battle
    public IEnumerator Co_Execute()
    {
        yield return null;
        IsFinished = true;
    }

    // Changes the to string return value to a more suitable string
    public override string ToString()
    {
        return "Start Boss Battle";
    }
}
