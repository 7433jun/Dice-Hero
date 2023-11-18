using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
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
