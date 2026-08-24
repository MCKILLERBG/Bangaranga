using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private long xpReward = 1000;
    [SerializeField] private long goldReward = 1000;
    [SerializeField] private float respawnTime = 10f;

    private PlayerStats playerStats;
    private EnemyLoot enemyLoot;
    private PlayerTarget playerTarget;

    private Vector2 spawnPosition;
    private SpriteRenderer spriteRenderer;
    private Collider2D enemyCollider;
    private EnemyController enemyController;

    public bool IsDead { get; private set; }

    private int currentHealth;
    void Start()
    {
        currentHealth = maxHealth;

        playerTarget = FindFirstObjectByType<PlayerTarget>();
        playerStats = FindFirstObjectByType<PlayerStats>();
        enemyLoot = GetComponent<EnemyLoot>();

        spawnPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyCollider = GetComponent<Collider2D>();
        enemyController = GetComponent<EnemyController>();

    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            playerStats.AddXp(xpReward);
            playerStats.AddGold(goldReward);
            Die();
        }
    }
    private void Die()
    {
        if (IsDead)
        {
            return;
        }

        IsDead = true;

        if (playerTarget != null && playerTarget.CurrentTarget == this)
        {
            playerTarget.ClearTarget();
        }

        if (enemyLoot != null)
        {
            enemyLoot.RollLoot();
        }

        StartCoroutine(RespawnRoutine());
    }
    private void OnMouseDown()
    {
        PlayerTarget playerTarget = FindFirstObjectByType<PlayerTarget>();

        if (playerTarget == null)
        {
            return;
        }

        playerTarget.SetTarget(this);
    }
    private IEnumerator RespawnRoutine()
    {
        spriteRenderer.enabled = false;
        enemyCollider.enabled = false;
        enemyController.enabled = false;

        yield return new WaitForSeconds(respawnTime);

        transform.position = spawnPosition;
        currentHealth = maxHealth;
        IsDead = false;

        spriteRenderer.enabled = true;
        enemyCollider.enabled = true;
        enemyController.enabled = true;
    }
}