using UnityEngine;

public class ItemUseManager : MonoBehaviour
{
    [SerializeField] private ConsumableData healthPotion;

    private Inventory inventory;
    private PlayerHealth playerHealth;
    private void Start()
    {
        inventory = GetComponent<Inventory>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            UseHealthPotion();
        }
    }
    private void UseHealthPotion()
    {
        if (healthPotion == null)
        {
            Debug.LogWarning("Health Potion is not assigned!");
            return;
        }

        if (inventory == null || playerHealth == null)
        {
            Debug.LogWarning("Inventory or PlayerHealth was not found!");
            return;
        }

        if (!inventory.HasItem(healthPotion))
        {
            Debug.Log("There are no health potions in the inventory!");
            return;
        }

        bool wasHealed = playerHealth.Heal(healthPotion.healAmount);

        if (!wasHealed)
        {
            Debug.Log("The Health Potion was not used!");
            return;
        }

        inventory.RemoveItem(healthPotion);

        Debug.Log($"Used 1 x {healthPotion.itemName}!");
    }
}
