using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Modified by adding comments, transition gamestate, properties such as lists for holding gameobject ids, modified functions such as to set player facing when transitioning between areas, added constant, end map, game states added methods.
 */

public enum GameState
{
    World,
    Cutscene,
    Battle,
    Menu,
    ShopMenu,
    Transition,
    Dialogue,
}

// A class for the Game manager that handles much of the logic while the game is running.
public class Game : MonoBehaviour
{
    public static Game Manager {  get; private set; }
    public GameState State { get; private set; }
    public Map Map { get; set; }
    public Map EndMap { get; set; }
    public Player Player { get; private set; }
    public Party Party { get; private set; }
    public List<int> PlayedCutsceneIDs { get; private set; }
    public List<int> DestroyedCharacterIDs { get; private set; }
    public List<int> OpenedTreasureChestIDs { get; private set; }
    public const int MIN_STEPS_BETWEEN_BATTLE = 11;


    // SerializeFields can be assigned in the Unity inspector
    [SerializeField] private Map startingMap;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Vector2Int startingCell;
    [SerializeField] private DialogueWindow dialogueWindow;
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private ShopMenu shopMenu;
    [SerializeField] private GameObject mapTransition;
    [SerializeField] public Map EndingMap;
    [SerializeField] public Map BossMap;

    private GameState currentState;
   
    private void Awake()
    {
        // Create Game manager, destroy if already exists
        if (Manager != null && Manager != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Manager = this;
        }

        // Create playedcutsceneids list when starting
        if (PlayedCutsceneIDs == null)
        {
            PlayedCutsceneIDs = new List<int>();
        }

        // Create DestroyedCharacterIDs list when starting
        if (DestroyedCharacterIDs == null)
        {
            DestroyedCharacterIDs = new List<int>();
        }

        // Create OpenedTreasureChestIDs list when starting
        if (OpenedTreasureChestIDs == null)
        {
            OpenedTreasureChestIDs = new List<int>();
        }

        // Instantiate map when starting
        if (Map == null)
        {
            Map = Instantiate(startingMap);
        }

        // Instantiate player gameobject when starting
        if (Player == null)
        {
            GameObject gameObject = Instantiate(playerPrefab, startingCell.Center2D(), Quaternion.identity);
            Player = gameObject.GetComponent<Player>();
            if (!Player.gameObject.activeSelf)
            {
                Player.gameObject.SetActive(true);
            }
        }

        // Instantiate party when starting
        if (Party == null)
        {
            Party = new Party();
            Party.CreateParty();
        }

        DontDestroyOnLoad(this);
        DontDestroyOnLoad(Player);
    }

    // Toggles the main menu and sets gamestate accordingly
    public void ToggleMenu()
    {
        if (mainMenu.IsAnimating || State == GameState.Cutscene)
        {
            return;
        }
        else 
        {
            State = GameState.Menu;
            mainMenu.OpenMenu();
            mainMenu.StartCoroutine(Co_WaitForMenu());
        }
    }

    // Coroutine for waiting while the main menu is open
    private IEnumerator Co_WaitForMenu()
    {
        while (mainMenu.IsOpen)
        {
            yield return null;
        }
        State = GameState.World;
    }

    // Function for opening the shop menu
    public void OpenShopMenu()
    {
        StartCoroutine(Co_TryOpenShopMenu());
    }

    // Coroutine for opening the shop menu, waits until merchant dialogue is over
    public IEnumerator Co_TryOpenShopMenu()
    {
        while (State == GameState.Dialogue)
        {
            yield return null;
        }
        State = GameState.ShopMenu;
        shopMenu.OpenMenu();
        shopMenu.StartCoroutine(Co_WaitForShopMenu());
    }

    // Coroutine for waiting while the shop menu is open
    private IEnumerator Co_WaitForShopMenu()
    {
        while (shopMenu.IsOpen)
        {
            yield return null;
        }
        State = GameState.World;
    }

    // Advances the dialogue to the next line if the dialogue window is open and not animating
    public void AdvanceDialogue()
    {
        if(!dialogueWindow.IsOpen || State != GameState.Dialogue || dialogueWindow.IsAnimating)
        {
            return;
        }
        dialogueWindow.NextDialogueLine();
    }

    // Open dialogue and change game state to cutscene
    public void StartDialogue(Dialogue dialogue)
    {
        currentState = State;
        State = GameState.Dialogue;
        dialogueWindow.OpenDialogue(dialogue);
    }

    // End the dialogue by setting the gamestate to "World"
    public void EndDialogue()
    {
        State = currentState;
    }

    // Coroutine to start battles, sets gamestate to "Battle" and load battle scene
    public IEnumerator Co_StartBattle(EnemyGroup enemyGroup, string battleTransition, int battleScene, bool bossBattle) 
    {
        State = GameState.Battle;
        Battle.EnemyGroup = enemyGroup;
        GameObject transition = Instantiate(Resources.Load<GameObject>(battleTransition), Player.transform.position, Quaternion.identity);
        Animator animator = transition.GetComponent<Animator>();
        while (animator.IsAnimating())
        {
            yield return null;
        }
        SceneLoader.LoadBattleScene(battleScene, bossBattle);
    }

    // End battle and reload previous scene while setting the gamestate back to "World"
    public void EndBattle(bool bossBattle) {
        SceneLoader.ReloadSavedSceneAfterBattle(bossBattle);
        State = GameState.World;
    }

    // Ends the boss battle after a defeat and reloads the previous scene while setting the gamestate back to "World"
    public void EndBossBattle()
    {
        SceneLoader.ReloadSavedSceneAfterBossBattle();
        State = GameState.World;
    }

    // Loads the target map when moving between areas
    public void LoadMap(Map targetMap, int targetId, string facing)
    {
        StartCoroutine(Game.Manager.Co_LoadMap(targetMap, targetId, facing));
    }

    // Coroutine for loading a new map when travelling between areas
    private IEnumerator Co_LoadMap(Map targetMap, int destinationId, string facing)
    {
        State = GameState.Transition;
        GameObject transition = Instantiate(mapTransition, Player.transform.position, Quaternion.identity);
        Animator animator = transition.GetComponent<Animator>();
        animator.Play("map_transition");
        yield return null;
        while (animator.IsAnimating())
        {
            yield return null;
        }
        Map currentMap = Map;
        Map = Instantiate(targetMap);
        Destroy(currentMap.gameObject);

        Transfer[] transfers = FindObjectsOfType<Transfer>();
        Transfer transfer = transfers.Where(transfer => transfer.Id == destinationId).ToList().FirstOrDefault();

        Player.transform.position = (transfer.Cell + transfer.TargetOffset).Center2D();
        Vector2Int direction = new Vector2Int(0, 0);

        if (facing == "left")
        {
            direction = Direction.left;
        }   
        else if (facing == "right")
        {
            direction = Direction.right;
        } 
        else if (facing == "up") {
            direction = Direction.up;
        }  
        else
        {
            direction = Direction.down;
        }
        Player.Turn.Turn(direction);

        Destroy(animator.gameObject);
        State = GameState.World;
    }

    // Tries to play cutscene and returns boolean based on success
    public bool TryPlayCutscene(Cutscene cutscene)
    {
        if (State != GameState.World)
        {
            return false;
        }

        State = GameState.Cutscene;
        StartCoroutine(Co_PlayCutscene(cutscene));
        return true;
    }

    // Coroutine for playing cutscene and each of the commands associated with it
    private IEnumerator Co_PlayCutscene(Cutscene cutscene) 
    {
        bool isBossBattle = false;
        foreach (CutsceneCommand command in cutscene.Commands)
        {
            StartCoroutine(command.Co_Execute());
            while (command.IsFinished != true) 
            {
                yield return null;
            }
            if (command is StartBossBattleCommand)
            {
                isBossBattle = true;
            }
        }

        cutscene.IsFinished = true;
        State = GameState.World;
        if (isBossBattle) 
        {
            cutscene.IsFinished = false;
            cutscene.StartBossBattle();
        }
    }
}
