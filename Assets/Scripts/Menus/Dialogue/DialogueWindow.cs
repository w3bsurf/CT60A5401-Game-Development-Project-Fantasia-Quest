using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Added comments.
 */

// Class for the dialogue window UI element
public class DialogueWindow : MonoBehaviour
{
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private TextMeshProUGUI speaker;
    [SerializeField] private TextMeshProUGUI dialogueMessage;

    private Animator animator;
    private int currentDialogueIndex = 0;
    public bool IsOpen { get; private set; }
    public bool IsAnimating => (animator.IsAnimating());

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Advances the dialogue scene to the next dialogue line and closes the scene after the last line
    public void NextDialogueLine()
    {
        currentDialogueIndex++;
        if (currentDialogueIndex < dialogue.DialogueLines.Count)
        {
            DisplayDialogue(dialogue.DialogueLines[currentDialogueIndex]);
        }
        else
        {
            CloseDialogue();
        }
    }

    // Sets the dialogue speaker and line on the dialogue window UI element
    private void DisplayDialogue(DialogueLine dialogue)
    {
        speaker.text = dialogue.Speaker;
        dialogueMessage.text = dialogue.DialogueMessage;
    }

    // Playes the animation to display the dialogue window to the player
    public void OpenDialogue(Dialogue dialogueScene)
    {
        dialogue = dialogueScene;
        currentDialogueIndex = 0;
        DisplayDialogue(dialogue.DialogueLines[currentDialogueIndex]);
        animator.Play("dialogue_open");
        IsOpen = true;
    }

    // Playes the animation to close the dialogue window
    private void CloseDialogue()
    {
        animator.Play("dialogue_close");
        Game.Manager.EndDialogue();
        IsOpen = false;
    }
}
