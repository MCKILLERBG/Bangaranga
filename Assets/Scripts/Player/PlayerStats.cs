using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int level = 1;
    [SerializeField] private long currentXp = 0;
    [SerializeField] private long xpToNextLevel = 100;
    [SerializeField] private long gold = 0;

    public event Action OnStatsChanged;

    private PlayerHealth playerHealth;

    public int Level => level;
    public long CurrentXp => currentXp;
    public long XPToNextLevel => xpToNextLevel;
    public long Gold => gold;

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        xpToNextLevel = CalculateXpForLevel(level);
    }
    public void AddXp (long amount)
    {
        currentXp += amount;
        CheckLevelUp();
        OnStatsChanged?.Invoke();
    }
    public void AddGold (long amount)
    {
        gold += amount;
        OnStatsChanged?.Invoke();
    }
    private void CheckLevelUp()
    {
        if (xpToNextLevel <= 0)
        {
            Debug.LogError("XP to next level must be greater than 0");
            return;
        }

        while (currentXp >= xpToNextLevel)
        {
            currentXp -= xpToNextLevel;
            level++;

            playerHealth.IncreaseMaxHealth(50);

            xpToNextLevel = CalculateXpForLevel(level);
        }
    }
    private long CalculateXpForLevel(int playerLevel)
    {
        const long baseXp = 100;
        const double growthRate = 1.25;

        double calculatedXp = baseXp * Math.Pow(growthRate, playerLevel - 1);

        if (calculatedXp >= long.MaxValue)
        {
            return long.MaxValue;
        }

        return (long)Math.Round(calculatedXp);
    }
}
