using UnityEngine;

public class PlayerTarget3D : MonoBehaviour
{
    [SerializeField] private GameObject targetIndicator;

    private EnemyHealth3D currentTarget;
    private Camera mainCamera;

    public EnemyHealth3D CurrentTarget => currentTarget;

    private void Awake()
    {
        mainCamera = Camera.main;

        if (targetIndicator != null)
        {
            targetIndicator.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectTarget();
        }
    }

    private void SelectTarget()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (mainCamera == null)
        {
            Debug.LogWarning("PlayerTarget3D: Main Camera was not found.");
            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            EnemyHealth3D enemy =
                hit.collider.GetComponentInParent<EnemyHealth3D>();

            if (enemy != null && !enemy.IsDead)
            {
                SetTarget(enemy);
            }
        }
    }

    public void SetTarget(EnemyHealth3D newTarget)
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

        Debug.Log("Target selected: " + currentTarget.name);
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