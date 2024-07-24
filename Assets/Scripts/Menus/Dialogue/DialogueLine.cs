using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Modified by adding comments
 */

[System.Serializable]
// Class for a single line of dialogue in a dialogue scene
public class DialogueLine
{
    [SerializeField] private string speaker;
    [SerializeField] [TextArea(3, 5)] private string dialogue;

    public string Speaker => speaker;
    public string DialogueMessage => dialogue;
}
