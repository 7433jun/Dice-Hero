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
}
