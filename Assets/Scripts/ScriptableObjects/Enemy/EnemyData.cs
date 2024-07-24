using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "New Enemy Data")]

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Modified by adding comments, changed to use enemyattributes
 */

// Class for enemydata scriptable objects. Allows for setting values in Unity editor
public class EnemyData : ScriptableObject
{
    // Serialized variables for name, scriptable enemy member object and attributes. Allows for setting values in editor.
    [SerializeField] private string enemyName;
    [SerializeField] private GameObject battleEnemyMember;
    [SerializeField] private EnemyAttributes enemyAttributes;

    public string Name => enemyName;
    public GameObject BattleEnemyMember => battleEnemyMember;
    public EnemyAttributes EnemyAttributes => enemyAttributes;
}
