using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Modified with added comments.
 */

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]

// Abstract class for characters which is inherited by the NPC class
public abstract class Character : MonoBehaviour
{
    public CharacterMover Move {  get; private set; }
    public CharacterTurner Turn { get; private set; }
    public CharacterAnimator Animator { get; private set; }
    public bool IsMoving => Move.IsMoving;
    public Vector2Int Facing => Turn.Facing;
    public Vector2Int CurrentCell => Game.Manager.Map.Grid.GetCell2D(this.gameObject);

    protected virtual void Awake()
    {
        Move = new CharacterMover(this);
        Turn = new CharacterTurner(this);
        Animator = new CharacterAnimator(this);
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (Game.Manager != null)
        {
            Vector2Int currentCell = Game.Manager.Map.Grid.GetCell2D(this.gameObject);
            transform.position = Game.Manager.Map.Grid.GetCellCenter2D(currentCell);
            Game.Manager.Map.OccupiedCells.Add(currentCell, this);
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Animator.ChooseLayer();
        Animator.UpdateParameters();
    }
}
