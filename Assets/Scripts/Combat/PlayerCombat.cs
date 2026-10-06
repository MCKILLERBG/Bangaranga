using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private int baseDamage = 20;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 1f;

    private float cooldownTimer;
    private bool autoAttack = false;

    private PlayerTarget playerTarget;

    private void Start()
    {
        playerTarget = GetComponent<PlayerTarget>();
    }
    private void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer < 0)
        {
            cooldownTimer = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            autoAttack = true;
        }

        if (autoAttack)
        {
            TryAttack();
        }

    }
    private void TryAttack()
    {
        if (playerTarget == null)
        {
            return;
        }

        EnemyHealth target = playerTarget.CurrentTarget;

        if (target == null || target.IsDead)
        {
            return;
        }

        if (target.IsUntargetable)
        {
            return;
        }

        float distanceToTarget = Vector2.Distance(transform.position, target.transform.position);

        if (distanceToTarget > attackRange)
        {
            return;
        }

        if (cooldownTimer > 0)
        {
            return;
        }

        BasicAttack(target);
        cooldownTimer = attackCooldown;
    }
    private void BasicAttack(EnemyHealth target)
    {
        int finalDamage = DamageCalculator.CalculateDamage(baseDamage);

        if (finalDamage <= 0)
        {
            return;
        }

        target.TakeDamage(finalDamage);
    }
    public void StopAutoAttack()
    {
        autoAttack = false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
