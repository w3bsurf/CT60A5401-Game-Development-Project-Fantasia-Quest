using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Modified by adding comments, added fields for end dialogue and boss dialogue
 */

[CreateAssetMenu(fileName = "New Dialogue", menuName = "New Dialogue")]
// A scriptable object class for dialogue
public class Dialogue : ScriptableObject 
{
    [SerializeField] private List<DialogueLine> dialogue;
    [SerializeField] public bool endDialogue;
    [SerializeField] public bool bossDialogue;

    public IReadOnlyList<DialogueLine> DialogueLines => dialogue;
}
