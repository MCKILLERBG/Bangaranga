using UnityEngine;

public class CameraFollow3D : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private Vector3 offset = new Vector3(0f, 10f, -11f);

    private void Start()
    {
        transform.rotation = Quaternion.Euler(50f, 0f, 0f);
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        transform.position = target.position + offset;
    }
}