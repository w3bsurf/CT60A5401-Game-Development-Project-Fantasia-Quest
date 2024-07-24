using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Based on the jRPG tutorial video series on youtube
 * Original author "Jeremy" jWohl1985 https://github.com/jWohl1985/Unity-Youtube
 * Added comments and checks and parameters for the different battleregions with different battle scenes, added checks for steps since battle
 */

[CreateAssetMenu(fileName = "New Battle Region", menuName = "New Battle Region")]

// A class for different battle regions based on the current map, different encounter rates, enemygroups and battlescenes
public class BattleRegion : ScriptableObject
{
    [SerializeField] private int encounterRate;
    [SerializeField] private int battleScene;
    [SerializeField] private List<EnemyGroup> enemyGroups;
    [SerializeField] private List<int> groupEncounterRate;
    private int stepsSinceBattle = 0;

    public int EncounterRate => encounterRate;
    public int BattleScene => battleScene;

    // Selects an enemy group for a battle based on encounter rates
    public EnemyGroup EncounterGroup()
    {
        int random = Random.Range(1, 101);

        for (int i = 0; i < enemyGroups.Count; i++)
        {
            if (random <= groupEncounterRate[i])
            {
                return enemyGroups[i];
            }
        }

        return enemyGroups[0];
    }

    // Checks whether an encounter will start based on the region encounter rate
    public void CheckForEncounter(Map map)
    {
        int random = Random.Range(1, 101);
        stepsSinceBattle++;

        if (random <= map.Region.EncounterRate && stepsSinceBattle >= Game.MIN_STEPS_BETWEEN_BATTLE)
        {
            stepsSinceBattle = 0;
            EnemyGroup enemyGroup = EncounterGroup();
            Game.Manager.StartCoroutine(Game.Manager.Co_StartBattle(enemyGroup, "BattleTransition", battleScene, false));
        }
    }
}
