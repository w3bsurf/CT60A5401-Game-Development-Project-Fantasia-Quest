using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class for the enemy party in battle
public class EnemyParty : MonoBehaviour
{
    [SerializeField] private GameObject enemyInfoBattlePrefab;

    private Battle battle;

    void Start()
    {
        battle = GameObject.FindObjectOfType<Battle>();
        GenerateEnemyPartyInfo();
    }

    // Creates a character info UI element in the enemy group window in battle for each enemy
    private void GenerateEnemyPartyInfo()
    {
        foreach (EnemyData enemy in battle.Enemies)
        {
            Instantiate(enemyInfoBattlePrefab, this.gameObject.transform);
        }
    }
}
