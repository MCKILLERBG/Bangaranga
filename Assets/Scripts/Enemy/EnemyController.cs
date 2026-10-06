using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float attackRange = 1.3f;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float triggerRange = 5f;
    [SerializeField] private float leashRange = 10f;

    private float nextAttackTime;
    private bool hasTarget;
    private bool returningToSpawn;
    private Vector2 spawnPosition;

    private Rigidbody2D rb;
    private Transform player;
    private PlayerHealth playerHealth;
    private EnemyHealth enemyHealth;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spawnPosition = transform.position;

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        enemyHealth = GetComponent<EnemyHealth>();

        if (playerObject != null)
        {
            player = playerObject.transform;
            playerHealth = playerObject.GetComponent<PlayerHealth>();
        }
    }
    private void FixedUpdate()
    {
        if (player == null)
        {
            return;
        }

        if (enemyHealth.IsDead)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (playerHealth.IsDead)
        {
            rb.linearVelocity = Vector2.zero;
            hasTarget = false;
            return;
        }

        float distanceToPlayer = Vector2.Distance(rb.position, player.position);
        float distanceFromSpawn = Vector2.Distance(rb.position, spawnPosition);

        if (returningToSpawn)
        {
            if (distanceFromSpawn > 0.1f)
            {
                Vector2 directionToSpawn =
                    (spawnPosition - rb.position).normalized;

                rb.MovePosition(
                    rb.position +
                    moveSpeed * Time.fixedDeltaTime * directionToSpawn
                );
            }
            else
            {
                rb.linearVelocity = Vector2.zero;

                returningToSpawn = false;
                hasTarget = false;

                enemyHealth.EndLeash();
            }

            return;
        }

        if (distanceFromSpawn > leashRange)
        {
            hasTarget = false;
            returningToSpawn = true;

            enemyHealth.StartLeash();

            return;
        }

        if (!hasTarget)
        {
            if (distanceToPlayer <= triggerRange)
            {
                hasTarget = true;
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }
        }

        if (distanceToPlayer > attackRange + 0.1f)
        {
            Vector2 direction =
                ((Vector2)player.position - rb.position).normalized;

            rb.MovePosition(
                rb.position +
                moveSpeed * Time.fixedDeltaTime * direction
            );

            return;
        }

        rb.linearVelocity = Vector2.zero;

        if (Time.time >= nextAttackTime)
        {
            BasicAttack();

            nextAttackTime = Time.time + attackCooldown;
        }
    }

    public void ResetEnemyState()
    {
        hasTarget = false;
        returningToSpawn = false;
        nextAttackTime = 0f;
    }

    private void BasicAttack()
    {
        playerHealth.TakeDamage(attackDamage);
    }
}
