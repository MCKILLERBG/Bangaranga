using UnityEngine;

public class EnemyHealth3D : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;

    private int currentHealth;

    public bool IsDead {get; private set;}
    public bool IsReturningToSpawn {get; private set;}

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (IsDead || IsReturningToSpawn || damage <= 0)
        {
            return;
        }

        currentHealth -= damage;

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

        gameObject.SetActive(false);
    }

    public void StartReturningToSpawn()
    {
        IsReturningToSpawn = true;
    }

    public void FinishReturningToSpawn()
    {
        currentHealth = maxHealth;
        IsReturningToSpawn = false;

        Debug.Log($"{name} returned to spawn. Health restored.");
    }
}