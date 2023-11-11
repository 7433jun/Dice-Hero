using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private int attack;
    private int addHealth;

    private void Start()
    {
        maxHealth = 100;
        currentHealth = 100;
    }
}
