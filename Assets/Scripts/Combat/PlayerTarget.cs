using UnityEngine;

public class PlayerTarget : MonoBehaviour
{
    [SerializeField] private GameObject targetIndicator;

    private EnemyHealth currentTarget;

    public EnemyHealth CurrentTarget => currentTarget;

    private void Start()
    {
        if (targetIndicator != null)
        {
            targetIndicator.SetActive(false);
        }
    }

    public void SetTarget(EnemyHealth newTarget)
    {
        if (newTarget == null || newTarget.IsDead)
        {
            return;
        }

        currentTarget = newTarget;

        if (targetIndicator != null)
        {
            targetIndicator.transform.SetParent(currentTarget.transform);

            targetIndicator.transform.localPosition = Vector3.zero;

            targetIndicator.SetActive(true);
        }
    }
    public void ClearTarget()
    {
        currentTarget = null;

        if (targetIndicator != null)
        {
            targetIndicator.transform.SetParent(null);
            targetIndicator.SetActive(false);
        }
    }
}
