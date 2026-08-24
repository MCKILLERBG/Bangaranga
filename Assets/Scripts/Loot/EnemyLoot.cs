using System.Collections.Generic;
using UnityEngine;

public class EnemyLoot : MonoBehaviour
{
    [SerializeField] private List<LootEntry> lootTable = new();

    private Inventory playerInventory;

    private void Start()
    {
        playerInventory = FindFirstObjectByType<Inventory>();
    }

    public void RollLoot()
    {
        if (playerInventory == null)
        {
            Debug.LogWarning("Player inventory was not found");
            return;
        }

        foreach (LootEntry lootEntry in lootTable)
        {
            if (lootEntry.itemData == null)
            {
                continue;
            }

            float randomRoll = Random.Range(0f, 100f);

            if (randomRoll <= lootEntry.dropChance)
            {
                int minimumAmount = Mathf.Max(1, lootEntry.minAmount);
                int maximumAmount = Mathf.Max(1, lootEntry.maxAmount);

                int droppedAmount = Random.Range(minimumAmount, maximumAmount + 1);

                playerInventory.AddItem(lootEntry.itemData, droppedAmount);

                Debug.Log($"Loot dropped: {droppedAmount} x " +
                          $"{lootEntry.itemData.itemName}. ");
            }
        }
    }
}
