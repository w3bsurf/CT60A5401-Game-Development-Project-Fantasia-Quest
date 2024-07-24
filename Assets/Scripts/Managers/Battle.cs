using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Modified by adding comments, properties such as a list for enemydata, exp rewards, enemydying, bossbattle, music, modified methods, added checks, added methods.
 */

// Class for managing the battles in the game, handles turn order, spawning characters, etc.
public class Battle : MonoBehaviour
{
    [SerializeField] private AudioSource battleMusic;
    [SerializeField] private AudioSource victoryMusic;
    [SerializeField] private AudioSource defeatMusic;

    public static EnemyGroup EnemyGroup;
    private List<Actor> turnOrder = new List<Actor>();
    private List<Ally> allies = new List<Ally>();
    private List<Enemy> enemyParty = new List<Enemy>();
    private List<EnemyData> enemies = new List<EnemyData>();
    private int expReward = 0;
    private int goldReward = 0;
    private int turnNumber = 0;
    private bool setupComplete = false;

    public AudioSource BattleMusic => battleMusic;
    public AudioSource VictoryMusic => victoryMusic;
    public AudioSource DefeatMusic => defeatMusic;
    public IReadOnlyList<Actor> TurnOrder => turnOrder;
    public List<Ally> Allies => allies;
    public List<Enemy> EnemyParty => enemyParty;
    public IReadOnlyList<EnemyData> Enemies => enemies;
    public BattleMenu BattleMenu { get; private set; }
    public PopupCreator PopupCreator { get; private set; }
    public int TurnNumber => turnNumber;
    public bool Dying { get; set; } = false;
    public bool BossBattle = false;
    public bool BattleOver { get; set; } = false;

    private void Awake()
    {
        BattleMenu = FindObjectOfType<BattleMenu>();
        PopupCreator = FindObjectOfType<PopupCreator>();
        CreateParty();
        CreateEnemies();
    }

    private void Update()
    {
        if (!setupComplete)
        {
            foreach (Enemy enemy in enemyParty) 
            {
                enemy.EnemyDefeated += OnEnemyKilled;
            }
            foreach (Ally ally in allies)
            {
                ally.AllyDefeated += OnAllyKilled;
            }
            DetermineTurnOrder();
        }
        if (!Dying || !BattleOver)
        {
            if (turnOrder[turnNumber] == null)
            {
                CheckForBattleEnd();
                GoToNextTurn();
            }
            else if (turnOrder[turnNumber].IsTakingTurn)
            {
                return;
            }
            else
            {
                CheckForBattleEnd();
                GoToNextTurn();
            }
        } else
        {
            return;
        }  
    }
    
    // Creates the party members in the battle scene
    private void CreateParty()
    {
        Vector2 startPosition = new Vector2(-5, -1f);
        foreach (PartyMember member in Game.Manager.Party.PartyMembers)
        {
            GameObject partyMember = Instantiate(member.BattlePartyMember, startPosition, Quaternion.identity);
            Ally ally = partyMember.GetComponent<Ally>();
            ally.CharacterAttributes = member.CharacterAttributes;
            ally.member = member;
            ally.Initiave = ally.member.Spd + Random.Range(0, 2);
            turnOrder.Add(ally);
            allies.Add(ally);
            startPosition.y += 1.5f;
        }
    }

    // Creates the enemies in the battle scene
    private void CreateEnemies()
    {
        for (int i = 0; i < EnemyGroup.Enemies.Count; i++)
        {
            Vector2 startPosition = new Vector2(EnemyGroup.XCoordinates[i], EnemyGroup.YCoordinates[i]);
            EnemyData enemyData = Instantiate(EnemyGroup.Enemies[i]);
            GameObject enemyActor = Instantiate(enemyData.BattleEnemyMember, startPosition, Quaternion.identity);
            Enemy enemy = enemyActor.GetComponent<Enemy>();
            enemy.EnemyAttributes = enemyData.EnemyAttributes;
            enemy.Initiave = enemy.EnemyAttributes.Spd + Random.Range(0, 2);
            turnOrder.Add(enemy);
            enemyParty.Add(enemy);
            enemies.Add(enemyData);
            if (enemyData.Name == "Demon")
            {
                BossBattle = true;
            }
        }
    }

    // Sets the turn order in battle based on the characters' speed attributes
    private void DetermineTurnOrder()
    {
        turnOrder = turnOrder.OrderByDescending(actor => actor.Initiave).ToList();
        turnOrder[0].StartTurn(BattleMenu);
        setupComplete = true;
    }

    // Check when battle should end (enemies defeated), give party members exp and gold to inventory
    private void CheckForBattleEnd()
    {
        if (enemyParty.Count == 0)
        {
            BattleOver = true;
            bool leveledUp = false;
            List <bool> levelUps = new List<bool>();
            if (!BossBattle)
            {
                if (BattleMenu.BattleMenuState != BattleMenu.MenuState.EndOfBattle)
                {
                    foreach (PartyMember member in Game.Manager.Party.PartyMembers)
                    {
                        levelUps.Add(member.CharacterAttributes.GainExp(expReward));
                    }
                    foreach (bool levelUp in levelUps)
                    {
                        if (levelUp)
                        {
                            leveledUp = true;
                        }
                    }
                    Game.Manager.Party.Inventory.AddGold(goldReward);
                    BattleMenu.EndBattleInfo(goldReward, expReward, EnemyGroup.DroppedItem, leveledUp);
                }
            }
            else
            {
                if (BattleMenu.BattleMenuState != BattleMenu.MenuState.BossVictory)
                {
                    BattleMenu.BossVictory();
                }
            }  
        }

        if (allies.Count == 0)
        {
            BattleOver = true;
            if (BattleMenu.BattleMenuState != BattleMenu.MenuState.GameOver)
            {
                foreach (PartyMember member in Game.Manager.Party.PartyMembers)
                {
                    member.CharacterAttributes.Hp = member.CharacterAttributes.MaxHp;
                }

                BattleMenu.GameOverInfo();
            }
        }
    }

    // Coroutine to wait for the victory music after a boss battle
    private IEnumerator Co_BossBattleWin()
    {
        victoryMusic.Play();
        yield return new WaitForSeconds(4f);
        Game.Manager.EndBattle(BossBattle);
    }

    // Increase turn order to go to the next character turn
    private void GoToNextTurn()
    {
        turnNumber = (turnNumber + 1) % turnOrder.Count;
        if (turnOrder[turnNumber] != null)
        {
            turnOrder[turnNumber].StartTurn(BattleMenu);
        }
        else
        {
            return;
        }
    }

    // Remove enemy from battle when defeated
    private void OnEnemyKilled()
    {
        for (int i = enemyParty.Count - 1; i >= 0; i--)
        {
            if (enemyParty[i].EnemyAttributes.Hp == 0)
            {
                expReward += enemyParty[i].EnemyAttributes.ExpValue;
                goldReward += enemyParty[i].EnemyAttributes.GoldValue;
                enemyParty[i].EnemyDefeated -= OnEnemyKilled;
                //turnOrder.Remove(enemyParty[i]);
                enemyParty.Remove(enemyParty[i]);  
            }
        }
    }

    // Remove ally from battle when defeated
    private void OnAllyKilled()
    {
        for (int i = allies.Count - 1; i >= 0; i--)
        {
            if (allies[i].CharacterAttributes.Hp == 0)
            {
                allies[i].AllyDefeated -= OnAllyKilled;
                allies[i].CharacterAttributes.Hp = 1;
                allies.Remove(allies[i]);
            }
        }
    }
}
