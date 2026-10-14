using UnityEngine;

public class PlayerHealth3D : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;

    private int currentHealth;

    public bool IsDead { get; private set; }

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (IsDead || damage <= 0)
        {
            return;
        }

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log(
            $"{name} received {damage} damage. HP: {currentHealth}/{maxHealth}"
        );

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        IsDead = true;

        Debug.Log($"{name} was defeated!");
    }
}