using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitModelActor : MonoBehaviour
{
    [Header("Unit Model Properties")]
    [SerializeField] private int maxHealth;
    public int MaxHealth => maxHealth;

    [SerializeField] private int currentHealth;
    public int CurrentHealth => currentHealth;

    public void SetMaxHealth(int health)
    {
        maxHealth = health;
        currentHealth = health;
    }

    public void SetDamage(int damage)
    {
        currentHealth -= damage;
    }
}
