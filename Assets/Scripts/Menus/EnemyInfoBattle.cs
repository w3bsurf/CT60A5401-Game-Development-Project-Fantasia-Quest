using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Class for the enemy character info element in battles
public class EnemyInfoBattle : MonoBehaviour
{
    private EnemyData enemyData;
    private Enemy enemy;
    private Battle battle;

    [SerializeField] private TextMeshProUGUI enemyName;
    [SerializeField] private TextMeshProUGUI enemyHP;

    void Start()
    {
        battle = GameObject.FindObjectOfType<Battle>();
        int i = this.gameObject.transform.GetSiblingIndex()-1;
        enemyData = battle.Enemies[i];
        enemy = battle.EnemyParty[i];
        enemy.EnemyDefeated += RemoveEnemyInfoBattle;
    }

    // Set the enemy group member info in the battle UI based on the enemy's actual stats
    public void Update()
    {
        enemyName.text = enemyData.Name;
        enemyHP.text = $"HP: {enemyData.EnemyAttributes.Hp} / {enemyData.EnemyAttributes.MaxHp}";
    }

    // Remove an enemy's ui element when they are defeated in battle
    private void RemoveEnemyInfoBattle()
    {
        enemy.EnemyDefeated -= RemoveEnemyInfoBattle;
        Destroy(this.gameObject);
    }
}
