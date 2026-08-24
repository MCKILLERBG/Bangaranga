using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<InventoryItem> items = new();
    public void AddItem(ItemData itemData, int amount)
    {
        if (itemData == null)
        {
            Debug.LogWarning("Cannot add a null item to the inventory");
            return;
        }

        if (amount <= 0)
        {
            Debug.LogWarning("Item amount must be greater than 0");
        }

        foreach (InventoryItem inventoryItem in items)
        {
            if (inventoryItem.itemData == itemData)
            {
                inventoryItem.quantity += amount;
                return;
            }
        }

        InventoryItem newInventoryItem = new()
        {
            itemData = itemData,
            quantity = amount
        };

        items.Add(newInventoryItem);
    }
    public bool HasItem(ItemData itemData, int amount = 1)
    {
        if (itemData == null || amount <= 0)
        {
            return false;
        }

        foreach (InventoryItem inventoryItem in items)
        {
            if ( inventoryItem.itemData == itemData)
            {
                return inventoryItem.quantity >= amount;
            }
        }

        return false;
    }
    public int GetItemQuantity(ItemData itemData)
    {
        if (itemData == null)
        {
            return 0;
        }

        foreach (InventoryItem inventoryItem in items)
        {
            if (inventoryItem.itemData == itemData)
            {
                return inventoryItem.quantity;
            }
        }

        return 0;
    }
    public bool RemoveItem(ItemData itemData, int amount = 1)
    {
        if (itemData == null || amount <= 0)
        {
            return false;
        }

        for (int i = 0; i < items.Count; i++)
        {
            InventoryItem inventoryItem = items[i];

            if ( inventoryItem.itemData != itemData)
            {
                continue;
            }

            if ( inventoryItem.quantity < amount)
            {
                return false;
            }

            inventoryItem.quantity -= amount;

            if ( inventoryItem.quantity == 0)
            {
                items.RemoveAt(i);
            }

            return true;
        }

        return false;
    }
}