using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    public event Action OnHealthChanged;

    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;

    public bool IsDead { get; private set; }
    void Start()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            IsDead = true;

            GetComponent<PlayerController>().enabled = false;
        }

        OnHealthChanged?.Invoke();
    }
    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth = maxHealth;
    }
    public bool Heal(int amount)
    {
        if (amount <= 0)
        {
            return false;
        }

        if (IsDead)
        {
            return false;
        }

        if (currentHealth >= maxHealth)
        {
            return false;
        }

        currentHealth += amount;

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        OnHealthChanged?.Invoke();

        return true;
    }
}