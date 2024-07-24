using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Cutscene command for starting the end credits
public class StartCreditsCommand : CutsceneCommand
{
    public bool IsFinished { get; private set; }

    // Coroutine to start the end credits sequence
    public IEnumerator Co_Execute()
    {
        yield return null;
        IsFinished = true;
        Game.Manager.StartCoroutine(SceneLoader.Co_StartCredits());
    }

    // Changes the to string return value to a more suitable string
    public override string ToString()
    {
        return "Start End Credits";
    }
}
