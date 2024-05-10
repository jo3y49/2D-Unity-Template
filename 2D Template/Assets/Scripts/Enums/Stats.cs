using UnityEngine;
using System;

[Serializable]
public class Stats
{
    private int MaxHealth;
    [SerializeField]
    private int Health;
    [SerializeField]
    private int Attack;
    [SerializeField]
    private int Defense;
    [SerializeField]
    private int Speed;
    
    public Stats(int health, int attack, int defense, int speed)
    {
        Health = health;
        MaxHealth = health;
        Attack = attack;
        Defense = defense;
        Speed = speed;
    }

    public void SetMaxHealth(int value) => MaxHealth = value;
    public void SetHealth(int value) => Health = value;
    public void SetAttack(int value) => Attack = value;
    public void SetDefense(int value) => Defense = value;
    public void SetSpeed(int value) => Speed = value;

    public void AddMaxHealth(int value, bool reset = true)
    {
        MaxHealth += value;
        if (reset) ResetHealth();
    }
    public void AddAttack(int value) => Attack += value;
    public void AddDefense(int value) => Defense += value;
    public void AddSpeed(int value) => Speed += value;

    public int GetMaxHealth() => MaxHealth;
    public int GetHealth() => Health;
    public int GetAttack() => Attack;
    public int GetDefense() => Defense;
    public int GetSpeed() => Speed;

    public void Heal(int health)
    {
        Health += health;

        if (Health > MaxHealth)
        {
            Health = MaxHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            Health = 0;
        }
    }

    public void ResetHealth() => Health = MaxHealth;
}