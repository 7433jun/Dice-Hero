using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Damage(Player player, Enemy enemy)
    {
        enemy.currentHealth -= player.attack;
        if(enemy.currentHealth <= 0)
        {
            enemy.currentHealth = 0;
            Debug.Log("you win");
        }
    }
}
