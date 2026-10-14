using UnityEngine;

public class PlayerCombat3D : MonoBehaviour
{
    [Header("Combat Settings")]
    [SerializeField] private int baseDamage = 20;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 1f;

    private float cooldownTimer;
    private bool autoAttack;
    private PlayerTarget3D playerTarget;

    private void Awake()
    {
        playerTarget = GetComponent<PlayerTarget3D>();
    }

    private void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartAutoAttack();
        }

        if (autoAttack)
        {
            TryAttack();
        }
    }

    private void StartAutoAttack()
    {
        if (autoAttack)
        {
            return;
        }

        autoAttack = true;
        Debug.Log("Auto attack enabled.");
    }

    public void StopAutoAttack()
    {
        if (!autoAttack)
        {
            return;
        }

        autoAttack = false;
        Debug.Log("Auto attack stopped because the player moved.");
    }

    private void TryAttack()
    {
        if (playerTarget == null)
        {
            return;
        }

        EnemyHealth3D target = playerTarget.CurrentTarget;

        if (target == null || target.IsDead)
        {
            return;
        }

        float distanceToTarget = Vector3.Distance(
            transform.position,
            target.transform.position
        );

        if (distanceToTarget > attackRange)
        {
            return;
        }

        if (cooldownTimer > 0f)
        {
            return;
        }

        BasicAttack(target);
        cooldownTimer = attackCooldown;
    }

    private void BasicAttack(EnemyHealth3D target)
    {
        int finalDamage = DamageCalculator.CalculateDamage(baseDamage);

        if (finalDamage <= 0)
        {
            return;
        }

        target.TakeDamage(finalDamage);

        Debug.Log(
            $"Basic attack dealt {finalDamage} damage to {target.name}."
        );
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}