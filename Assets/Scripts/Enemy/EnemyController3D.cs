using UnityEngine;

public class EnemyController3D : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float stoppingDistance = 1.5f;

    [Header("Detection Settings")]
    [SerializeField] private float triggerRange = 5f;
    [SerializeField] private float leashRange = 10f;

    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float firstAttackDelay = 0.5f;
    [SerializeField] private float attackRecovery = 0.5f;
    [SerializeField] private float attackWindup = 0.5f;

    private float AttackCooldown =>
    attackRecovery + attackWindup;
    private enum AttackPhase
    {
        None,
        FirstDelay,
        Recovery,
        Windup
    }

    private Rigidbody rb;
    private Transform player;
    private PlayerHealth3D playerHealth;
    private EnemyHealth3D enemyHealth;

    private Vector3 spawnPosition;

    private bool hasTarget;
    private bool returningToSpawn;
    private bool hasAttackedSinceApproach;

    private AttackPhase attackPhase = AttackPhase.None;
    private float attackPhaseTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        enemyHealth = GetComponent<EnemyHealth3D>();
        spawnPosition = transform.position;
    }
    private void Start()
    {
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
            playerHealth = playerObject.GetComponent<PlayerHealth3D>();

            if (playerHealth == null)
            {
                Debug.LogWarning(
                    "EnemyController3D: PlayerHealth3D was not found."
                );
            }
        }
        else
        {
            Debug.LogWarning(
                "EnemyController3D: Player was not found."
            );
        }
    }
    private void FixedUpdate()
    {
        if (player == null || rb == null)
        {
            StopMovement();
            return;
        }

        if (enemyHealth != null && enemyHealth.IsDead)
        {
            StopMovement();
            StopPreparingAttack();
            return;
        }

        if (playerHealth != null && playerHealth.IsDead)
        {
            StopMovement();
            StopPreparingAttack();
            hasTarget = false;
            return;
        }

        float distanceToPlayer =
            Vector3.Distance(rb.position, player.position);

        float distanceFromSpawn =
            Vector3.Distance(rb.position, spawnPosition);

        if (returningToSpawn)
        {
            ReturnToSpawn();
            return;
        }

        if (distanceFromSpawn > leashRange)
        {
            StopPreparingAttack();

            hasTarget = false;
            returningToSpawn = true;

            if (enemyHealth != null)
            {
                enemyHealth.StartReturningToSpawn();
            }

            Debug.Log($"{name} is returning to spawn.");
            return;
        }

        if (!hasTarget)
        {
            if (distanceToPlayer <= triggerRange)
            {
                hasTarget = true;
                Debug.Log($"{name} detected the player.");
            }
            else
            {
                StopMovement();
                StopPreparingAttack();
                return;
            }
        }

        if (distanceToPlayer > stoppingDistance)
        {
            StopPreparingAttack();
            MoveTowards(player.position);
            return;
        }

        StopMovement();

        if (distanceToPlayer <= attackRange)
        {
            TryAttackPlayer();
        }
        else
        {
            StopPreparingAttack();
        }
    }
    private void TryAttackPlayer()
    {
        if (playerHealth == null || playerHealth.IsDead)
        {
            StopPreparingAttack();
            return;
        }

        if (!hasAttackedSinceApproach)
        {
            if (attackPhase == AttackPhase.None)
            {
                attackPhase = AttackPhase.FirstDelay;
                attackPhaseTimer = firstAttackDelay;

                Debug.Log($"{name} is preparing its first attack.");
                return;
            }

            if (attackPhase == AttackPhase.FirstDelay)
            {
                attackPhaseTimer -= Time.fixedDeltaTime;

                if (attackPhaseTimer <= 0f)
                {
                    DealAttackDamage();

                    hasAttackedSinceApproach = true;
                    attackPhase = AttackPhase.Recovery;
                    attackPhaseTimer = attackRecovery;
                }
            }

            return;
        }
        
        attackPhaseTimer -= Time.fixedDeltaTime;

        if (attackPhaseTimer > 0f)
        {
            return;
        }

        if (attackPhase == AttackPhase.Recovery)
        {
            attackPhase = AttackPhase.Windup;
            attackPhaseTimer = attackWindup;
        }
        else if (attackPhase == AttackPhase.Windup)
        {
            DealAttackDamage();

            attackPhase = AttackPhase.Recovery;
            attackPhaseTimer = attackRecovery;
        }
    }
    private void DealAttackDamage()
    {
        if (playerHealth == null || playerHealth.IsDead)
        {
            StopPreparingAttack();
            return;
        }

        playerHealth.TakeDamage(attackDamage);

        Debug.Log(
            $"{name} attacked the player for {attackDamage} damage."
        );
    }
    private void StopPreparingAttack()
    {
        attackPhase = AttackPhase.None;
        attackPhaseTimer = 0f;
        hasAttackedSinceApproach = false;
    }
    private void MoveTowards(Vector3 destination)
    {
        Vector3 direction = destination - rb.position;
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.001f)
        {
            StopMovement();
            return;
        }

        direction.Normalize();

        Vector3 nextPosition =
            rb.position + moveSpeed * Time.fixedDeltaTime * direction;

        rb.MovePosition(nextPosition);

        transform.rotation = Quaternion.LookRotation(direction);
    }
    private void ReturnToSpawn()
    {
        float distanceFromSpawn =
            Vector3.Distance(rb.position, spawnPosition);

        if (distanceFromSpawn <= 0.1f)
        {
            rb.position = spawnPosition;
            StopMovement();

            returningToSpawn = false;
            hasTarget = false;

            StopPreparingAttack();

            if (enemyHealth != null)
            {
                enemyHealth.FinishReturningToSpawn();
            }

            Debug.Log($"{name} returned to spawn.");
            return;
        }

        MoveTowards(spawnPosition);
    }
    private void StopMovement()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
        }
    }
}