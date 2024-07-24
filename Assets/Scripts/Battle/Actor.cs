using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Modified by adding comments removing unnecessary methods and properties and adding properties such as initiative
 */

// Base class for both player characters and enemies in battle
public abstract class Actor : MonoBehaviour
{
    protected Vector2 startingPosition;
    protected Vector2 battlePosition;
    protected Battle battle;
    protected BattleMenu menu;
    protected AudioSource sfx;

    public Animator Animator {  get; protected set; }
    public bool IsTakingTurn { get; protected set; } = false;

    public int Initiave { get; set; }

    protected virtual void Awake()
    {
        Animator = GetComponent<Animator>();
        battle = FindObjectOfType<Battle>();
        menu = FindObjectOfType<BattleMenu>();
    }
    
    protected virtual void Start()
    {
        startingPosition = transform.position;
    }

    // Start the actor turn in battle
    public abstract void StartTurn(BattleMenu battleMenu);
}
