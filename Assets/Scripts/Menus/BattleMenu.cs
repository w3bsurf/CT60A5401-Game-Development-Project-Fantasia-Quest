using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for handling battle menu
public class BattleMenu : MonoBehaviour
{
    [SerializeField] private BattleMenuSelector commandSelector;
    [SerializeField] private BattleMenuSelector skillSelector;
    [SerializeField] private BattleMenuSelector targetSelector;
    [SerializeField] private BattleMenuSelector itemSelector;
    [SerializeField] private BattleMenuSelector defeatSelector;
    [SerializeField] private BattleCommandMenu commandMenu;
    [SerializeField] private BattleEndInfo battleEndInfo;
    [SerializeField] private RectTransform gameOverPanel;
    [SerializeField] private AudioSource menuAccept;
    [SerializeField] private AudioSource menuCancel;

    [SerializeField] public AudioSource heal;
    [SerializeField] public AudioSource attackMage;
    [SerializeField] public AudioSource attackHero;
    [SerializeField] public AudioSource magicAlly;
    [SerializeField] public AudioSource magicEnemy;
    [SerializeField] public AudioSource death;
    [SerializeField] public AudioSource slimeAttack;
    [SerializeField] public AudioSource impAttack;
    [SerializeField] public AudioSource wizardAttack;
    [SerializeField] public AudioSource demonAttack;
    [SerializeField] public AudioSource usePotion;

    private Animator animator;
    private MenuState menuState;
    private BattleCommand command;
    private Battle battle;

    public ICommand Command;

    public enum MenuState
    {
        Inactive,
        BattleCommandSelect,
        SkillSelect,
        TargetSelect,
        ItemSelect,
        EndOfBattle,
        BossVictory,
        GameOver,
    }

    private enum BattleCommand
    {
        Attack,
        DoubleAttack,
        Heal,
        Magic,
        Item,
    }

    public MenuState BattleMenuState => menuState;

    public bool IsShown { get; private set; }
    public bool IsAnimating => (animator.IsAnimating());

    private void Awake()
    {
        animator = GetComponent<Animator>();
        battle = GetComponentInParent<Battle>();
    }

    private void Update()
    {
        if (menuState != MenuState.Inactive || menuState != MenuState.EndOfBattle || menuState != MenuState.BossVictory)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Accept();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cancel();
            }
        }
        if (menuState == MenuState.EndOfBattle)
        {
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    EndBattle();
                }
                else if (Input.GetKeyDown(KeyCode.Escape))
                {
                    EndBattle();
                }
            }
        }
    }

    // Plays the "battlecommand_appear" animation which displays the battlecommand menu to the player
    public void ShowBattleCommands()
    {
        if (menuState != MenuState.EndOfBattle)
        {
            menuState = MenuState.BattleCommandSelect;
            commandSelector.Index = 0;
            commandSelector.IsActive = true;
            commandSelector.gameObject.SetActive(true);
            IsShown = true;
            animator.Play("battlecommand_appear");
        }  
    }

    // Plays the "battlecommand_disappear" animation which hides the battlecommand menu
    public void HideBattleCommands()
    {
        menuState = MenuState.Inactive;
        commandSelector.IsActive = false;
        commandSelector.gameObject.SetActive(false);
        IsShown = false;
        animator.Play("battlecommand_disappear");
        Command = null;
    }

    // Function to accept current menu selection and process it
    public void Accept()
    {
        switch (menuState)
        {
            case MenuState.Inactive:
                return;
            case MenuState.BattleCommandSelect:
                ChooseBattleCommand();
                break;
            case MenuState.SkillSelect:
                SelectSkill();
                break;
            case MenuState.TargetSelect:
                SelectTarget();
                break;
            case MenuState.ItemSelect:
                if (itemSelector.IsActive) SelectItem();
                break;
            case MenuState.EndOfBattle:
                break;
            case MenuState.GameOver:
                SelectGameOver();
                break;
        }
        menuAccept.Play();
    }

    // Function to cancel menu selection and return to previous state
    public void Cancel()
    {
        switch (menuState)
        {
            case MenuState.Inactive:
                break;
            case (MenuState.SkillSelect):
                skillSelector.IsActive = false;
                skillSelector.gameObject.SetActive(false);
                commandMenu.ShowCommands();
                commandSelector.IsActive = true;
                commandSelector.gameObject.SetActive(true);
                menuState = MenuState.BattleCommandSelect;
                break;
            case (MenuState.TargetSelect):
                targetSelector.IsActive = false;
                targetSelector.gameObject.SetActive(false);
                commandSelector.IsActive = true;
                commandSelector.gameObject.SetActive(true);
                menuState = MenuState.BattleCommandSelect;
                break;
            case (MenuState.ItemSelect):
                itemSelector.IsActive = false;
                itemSelector.gameObject.SetActive(false);
                commandMenu.ShowCommands();
                commandSelector.IsActive = true;
                commandSelector.gameObject.SetActive(true);
                menuState = MenuState.BattleCommandSelect;
                break;
        }
        menuCancel.Play();
    }

    // Function to end battle after defeating the boss
    public void BossVictory()
    {
        battle.BattleMusic.Stop();
        battle.VictoryMusic.Play();
        menuState = MenuState.BossVictory;
        StartCoroutine(Co_BossVictory());
    }

    // Coroutine to end battle after defeating the boss
    public IEnumerator Co_BossVictory()
    {
        yield return new WaitForSeconds(4);
        Game.Manager.EndBattle(battle.BossBattle);
    }

    // Function to end the battle and return to the previous scene
    private void EndBattle()
    {
        menuAccept.Play();
        Game.Manager.EndBattle(battle.BossBattle);
    }

    // Function to end a boss battle after defeat
    private void EndBossBattle()
    {
        menuAccept.Play();
        Game.Manager.EndBossBattle();
    }

    // Function for choosing what to do at a game over
    private void SelectGameOver()
    {
        if (!battle.BossBattle)
        {
            switch (defeatSelector.Index)
            {
                case 0:
                    EndBattle();
                    break;
                case 1:
                    menuAccept.Play();
                    SceneLoader.LoadStartMenuFromBattle();
                    break;
            }
        }
        else
        {
            switch (defeatSelector.Index)
            {
                case 0:
                    EndBossBattle();
                    break;
                case 1:
                    menuAccept.Play();
                    SceneLoader.LoadStartMenuFromBattle();
                    break;
            }
        }
    }

    // Function to show the game over screen
    public void GameOverInfo()
    {
        battle.BattleMusic.Stop();
        battle.DefeatMusic.Play();
        menuState = MenuState.GameOver;
        itemSelector.IsActive = false;
        itemSelector.gameObject.SetActive(false);
        commandSelector.IsActive = false;
        commandSelector.gameObject.SetActive(false);
        targetSelector.IsActive = false;
        targetSelector.gameObject.SetActive(false);
        skillSelector.IsActive = false;
        skillSelector.gameObject.SetActive(false);
        defeatSelector.IsActive = true;
        defeatSelector.gameObject.SetActive(true);
        gameOverPanel.gameObject.SetActive(true);
    }

    // Function to show info at the end of battle
    public void EndBattleInfo(int goldAmount, int xpAmount, string droppedItem, bool leveledUp)
    {
        battle.BattleMusic.Stop();
        battle.VictoryMusic.Play();
        menuState = MenuState.EndOfBattle;
        itemSelector.IsActive = false;
        itemSelector.gameObject.SetActive(false);
        commandSelector.IsActive = false;
        commandSelector.gameObject.SetActive(false);
        targetSelector.IsActive = false;
        targetSelector.gameObject.SetActive(false);
        skillSelector.IsActive = false;
        skillSelector.gameObject.SetActive(false);
        Game.Manager.Party.Inventory.AddItem(droppedItem);
        battleEndInfo.gameObject.SetActive(true);
        battleEndInfo.SetMenuText(goldAmount, xpAmount, droppedItem, leveledUp);
    }

    // Function to choose a basic command in battle
    private void ChooseBattleCommand()
    {
        switch (commandSelector.Index)
        {
            case 0:
                AttackCommand();
                break;
            case 1:
                GoToSkillSelect();
                break;
            case 2:
                GoToItemSelect();
                break;
        }
    }

    // Function for moving from having chosen the attack command to target selection
    private void AttackCommand()
    {
        menuState = MenuState.TargetSelect;
        command = BattleCommand.Attack;
        commandSelector.IsActive = false;
        commandSelector.gameObject.SetActive(false);
        targetSelector.gameObject.SetActive(true);
        targetSelector.IsActive = true;
        targetSelector.Index = 0;
    }

    // Function for showing the character's skills after choosing the Skill command
    private void GoToSkillSelect()
    {
        menuState = MenuState.SkillSelect;
        commandSelector.IsActive = false;
        commandSelector.gameObject.SetActive(false);
        Ally ally = (Ally)battle.TurnOrder[battle.TurnNumber];
        PartyMember partyMember = ally.member;
        commandMenu.ShowSkills(partyMember);
        skillSelector.gameObject.SetActive(true);
        skillSelector.IsActive = true;
        skillSelector.Index = 0;
    }

    // Function for showing the item list after choosing the Item command
    private void GoToItemSelect()
    {
        menuState = MenuState.ItemSelect;
        commandSelector.IsActive = false;
        commandSelector.gameObject.SetActive(false);
        commandMenu.ShowItemList();
        if (Game.Manager.Party.Inventory.ItemList.Count > 0)
        {
            itemSelector.gameObject.SetActive(true);
            itemSelector.IsActive = true;
            itemSelector.Index = 0;
        }
    }

    // Function for handling the choosing of different skills
    private void SelectSkill()
    {
        Ally ally = (Ally)battle.TurnOrder[battle.TurnNumber];
        PartyMember partyMember = ally.member;
        AudioSource sfx = new AudioSource();

        if (partyMember.Name == "Hero")
        {
            command = BattleCommand.DoubleAttack;
        }
        else
        {
            switch (skillSelector.Index)
            {
                case 0:
                    command = BattleCommand.Magic;
                    break;
                case 1:
                    command = BattleCommand.Heal;
                    break;
            }
        }

        switch (command)
        {
            case BattleCommand.DoubleAttack:
                sfx = attackHero;
                Command = new DoubleAttack(ally, battle.EnemyParty, sfx);
                commandMenu.ShowCommands();
                break;
            case BattleCommand.Magic:
                sfx = magicAlly;
                Command = new AttackMagic(battle.TurnOrder[battle.TurnNumber], battle.EnemyParty, sfx);
                commandMenu.ShowCommands();
                break;
            case BattleCommand.Heal:
                sfx = heal;
                Command = new Heal(ally, battle.Allies, heal);
                commandMenu.ShowCommands();
                break;
        }
    }

    // Function for choosing an item in battle
    private void SelectItem()
    {
        Ally ally = (Ally)battle.TurnOrder[battle.TurnNumber];
        AudioSource sfx = usePotion;
        Item item = Game.Manager.Party.Inventory.ItemList[itemSelector.Index];
        Command = new UsePotion(ally, battle.Allies, sfx, item);
        itemSelector.gameObject.SetActive(false);
        itemSelector.IsActive = false;
        itemSelector.Index = 0;
        commandMenu.ShowCommands(); 
    }

    // Function for choosing a target in battle
    private void SelectTarget()
    {
        if (battle.EnemyParty.Count > 0)
        {
            switch (command)
            {
                case BattleCommand.Attack:
                    Ally ally = (Ally)battle.TurnOrder[battle.TurnNumber];
                    PartyMember partyMember = ally.member;
                    AudioSource sfx = new AudioSource();

                    if (partyMember.Name == "Hero")
                    {
                        sfx = attackHero;
                    }
                    else
                    {
                        sfx = attackMage;
                    }

                    Command = new Attack(battle.TurnOrder[battle.TurnNumber], battle.EnemyParty[targetSelector.Index], sfx);
                    targetSelector.gameObject.SetActive(false);
                    targetSelector.IsActive = false;
                    targetSelector.Index = 0;
                    return;
            }
        } 
        else
        {
            return;
        }
        
    }
}
