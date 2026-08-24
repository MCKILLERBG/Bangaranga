using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float stoppingDistance = 1.3f;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackCooldown = 1f;
    private float nextAttackTime;
    [SerializeField] private float triggerRange = 5f;

    private Rigidbody2D rb;
    private Transform player;
    private PlayerHealth playerHealth;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

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

        if (playerHealth.IsDead)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float distanceToPlayer = Vector2.Distance(rb.position, player.position);

        if (distanceToPlayer > triggerRange)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (distanceToPlayer > stoppingDistance + 0.1f)
        {
            Vector2 direction = ((Vector2)player.position - rb.position).normalized;
            rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * direction);

        }
        else
        {
            rb.linearVelocity = Vector2.zero;

            if (Time.time >= nextAttackTime)
            {
                playerHealth.TakeDamage(attackDamage);
                nextAttackTime = Time.time + attackCooldown;
            }
        }

    }
}
