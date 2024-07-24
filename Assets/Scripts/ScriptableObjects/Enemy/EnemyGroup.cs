using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Modified by adding comments, added dropped items
 */

[CreateAssetMenu(fileName = "New Enemy Group", menuName = "New Enemy Group")]
// Class for making different enemy groups
public class EnemyGroup : ScriptableObject
{
    [SerializeField] private List<EnemyData> enemies;
    [SerializeField] private List<float> xCoordinates; // x coordinates for enemy spawns
    [SerializeField] private List<float> yCoordinates; // y coordinates for enemy spawns
    [SerializeField] private string droppedItem;

    public List<EnemyData> Enemies => enemies;
    public string DroppedItem => droppedItem;

    public IReadOnlyList<float> XCoordinates => xCoordinates;
    public IReadOnlyList<float > YCoordinates => yCoordinates;
}
