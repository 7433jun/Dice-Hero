using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int code;
    public string characterName;
    public int maxHealth;
    public int currentHealth;
    private int attack;
    private int addHealth;

    private void Start()
    {
        maxHealth = 100;
        currentHealth = 100;
    }
}
