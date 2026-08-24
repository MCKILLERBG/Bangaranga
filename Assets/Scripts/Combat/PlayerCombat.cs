using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private int baseDamage = 20;
    [SerializeField] private float attackRange = 1.5f;

    private PlayerTarget playerTarget;

    private void Start()
    {
        playerTarget = GetComponent<PlayerTarget>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
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

        float distanceToTarget = Vector2.Distance(transform.position, target.transform.position);

        if (distanceToTarget > attackRange)
        {
            return;
        }

        int finalDamage = DamageCalculator.CalculateDamage(baseDamage);

        if (finalDamage <= 0)
        {
            return;
        }

        target.TakeDamage(finalDamage);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
