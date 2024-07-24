using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Modified by adding comments
 */

[System.Serializable]
// Cutscene command class for waiting a certain period of time during a cutscene
public class WaitCommand : CutsceneCommand
{
    [SerializeField] private float seconds = 0;
    public bool IsFinished { get; private set; }

    // Coroutine for having a wait period in a cutscene
    public IEnumerator Co_Execute()
    {
        yield return new WaitForSeconds(seconds);
        IsFinished = true;
    }

    // Overrides tostring to return a more suitable value
    public override string ToString()
    {
        return "Wait For Seconds";
    }
}
