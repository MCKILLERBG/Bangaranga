using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text xpText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text goldText;

    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerStats playerStats;

    private void Start()
    {
        playerStats.OnStatsChanged += UpdateUI;
        playerHealth.OnHealthChanged += UpdateUI;
        UpdateUI();
    }

    public void UpdateUI()
    {
        healthText.text = $"HP: {playerHealth.CurrentHealth} / {playerHealth.MaxHealth}";
        xpText.text = $"XP: {playerStats.CurrentXp} / {playerStats.XPToNextLevel}";
        levelText.text = $"Level: {playerStats.Level}";
        goldText.text = $"Gold: {playerStats.Gold}";
    }

    private void OnDestroy()
    {
        playerStats.OnStatsChanged -= UpdateUI;
        playerHealth.OnHealthChanged -= UpdateUI;
    }
}
