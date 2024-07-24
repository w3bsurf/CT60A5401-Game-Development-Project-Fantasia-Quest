using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Modified by adding comments, properties such as sceneID and modified methods to prevent cutscenes from triggering if they have already been played. Added method for starting a boss battle.
 */

// Class for cutscenes that can be played during the game
public class Cutscene : MonoBehaviour
{
    [SerializeField] bool autoPlay = false;
    [SerializeField] private int sceneID;
    [SerializeField] private EnemyGroup enemyGroup;

    [SerializeReference] private List<CutsceneCommand> commands = new List<CutsceneCommand>(); 
    private bool isRunning = false;

    private Map currentMap;
    private Vector2Int cutsceneTriggerCell;

    public IReadOnlyList<CutsceneCommand> Commands => commands;
    public Vector2Int TriggerCell => cutsceneTriggerCell;

    // Boolean for the whether the cutscene is finished or not
    public bool IsFinished { get; set; } = false;
   
    private void Start()
    {
        if (Game.Manager.State != GameState.Battle)
        {
            currentMap = FindObjectOfType<Map>();
            cutsceneTriggerCell = currentMap.Grid.GetCell2D(this.gameObject);
            // Check if the cutscene has already been played before adding the cutscene trigger to a list
            if (!Game.Manager.PlayedCutsceneIDs.Contains(this.sceneID))
            {
                currentMap.Cutscenes.Add(TriggerCell, this);
            }
        }
    }

    private void Update()
    {
        if (autoPlay && !isRunning)
        {
            Play();
        }
    }

    // Calls the TryPlayCutscene function in the Game class and sets cutscene as running if it starts successfully. Only plays a cutscene if not already played.
    public void Play()
    {
        if (IsFinished)
        {
            return;
        }
        if (Game.Manager.TryPlayCutscene(this))
        {
            isRunning = true;
            if (this.sceneID != 3)
            {
                Game.Manager.PlayedCutsceneIDs.Add(this.sceneID);
            }
        }
    }

    // starts a boss battle if the cutscene is a boss cutscene
    public void StartBossBattle() {
        Game.Manager.StartCoroutine(Game.Manager.Co_StartBattle(enemyGroup, "BossBattleTransition", 5, true));
    }

    public void InsertCommand(int index, CutsceneCommand command) => commands.Insert(index, command);
    public void RemoveAt(int i) => commands.RemoveAt(i);
    public void SwapCommands(int i, int j)
    {
        (commands[i], commands[j]) = (commands[j], commands[i]);
    }
}
