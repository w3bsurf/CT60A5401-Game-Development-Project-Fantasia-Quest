using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Modified by adding comments, array for NPCs and checks for whether an NPC should be destroyed on load.
 */

// Class for each different map, keeps list of NPCs, cutscenes, occupied cells and transfers in each map 
public class Map : MonoBehaviour
{
    [SerializeField] private BattleRegion region;
    public Dictionary<Vector2Int, MonoBehaviour> OccupiedCells { get; private set; } = new Dictionary<Vector2Int, MonoBehaviour>();
    public Dictionary<Vector2Int, Transfer> Transfers { get; private set; } = new Dictionary<Vector2Int, Transfer>();
    public Dictionary<Vector2Int, Cutscene> Cutscenes { get; private set; } = new Dictionary<Vector2Int, Cutscene>();
    public NPC[] NpcArray { get; private set; }  

    public Grid Grid { get; private set; }
    public BattleRegion Region => region;

    private void Awake()
    {
        Grid = GetComponent<Grid>();
        NpcArray= FindObjectsOfType<NPC>();

        // Removes any NPC that are supposed to be destroyed when loading a map
        if (NpcArray.Length > 0 )
        {
            foreach (NPC npc in NpcArray)
            {
                if (Game.Manager.DestroyedCharacterIDs.Contains(npc.CharID))
                {
                    npc.DestroyCharacter();
                }
                else
                {
                    continue;
                }
            }
        }
        
        OccupiedCells.Clear();
    }
}
