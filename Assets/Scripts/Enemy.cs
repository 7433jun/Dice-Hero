using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int code;
    public string characterName;
    public int maxHealth;
    public int currentHealth;

    private void Start()
    {
        maxHealth = 100;
        currentHealth = 100;
    }

    public int Enemydamage()
    {
        float randomValue = Random.value;

        if (randomValue < 0.5f)
        {
            return 8;
        }
        else
        {
            return 10;
        }
    }
}
