using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Added comments. Modified with the check for current cell method
 */

[System.Serializable]
// Class for dialogues during cutscenes
public class DialogueCommand : CutsceneCommand
{
    [SerializeField] private Dialogue dialogue;
    public bool IsFinished { get; private set; }

    // Function for starting a dialogue during cutscenes
    public IEnumerator Co_Execute()
    {
        Game.Manager.StartDialogue(dialogue);

        while (Game.Manager.State == GameState.Dialogue)
        {
            yield return null;
        }

        IsFinished = true;
    }

    // Overwrites the basic tostring method to return a more suitable name
    public override string ToString()
    {
        return "Dialogue";
    }
}
