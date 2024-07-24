using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Modified by adding comments
 */

// Class for the character animator
public class CharacterAnimator
{
    private Character character;
    private Animator animator;

    private string horizontalParameter = "xDir";
    private string verticalParamenter = "yDir";
    private string walkingParameter = "isWalking";

    public CharacterAnimator(Character character)
    {
        this.character = character; 
        this.animator = character.GetComponent<Animator>();
    }

    // Sets animation layer
    public void ChooseLayer()
    {
        bool isWalking = character.IsMoving;
        animator.SetBool(walkingParameter, isWalking);
    }

    // Update animator parameters
    public void UpdateParameters()
    {
        animator.SetFloat(horizontalParameter, character.Facing.x);
        animator.SetFloat(verticalParamenter, character.Facing.y);
    }
}
