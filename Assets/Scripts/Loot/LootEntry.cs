using UnityEngine;

[System.Serializable]
public class LootEntry
{
    public ItemData itemData;

    [Range(0f, 100f)]
    public float dropChance;

    public int minAmount = 1;
    public int maxAmount = 1;
}