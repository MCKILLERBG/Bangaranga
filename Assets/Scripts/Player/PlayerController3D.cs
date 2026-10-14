using UnityEngine;

public class PlayerController3D : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    private Rigidbody rb;
    private Vector3 movement;
    private Animator animator;
    private PlayerCombat3D playerCombat;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        playerCombat = GetComponent<PlayerCombat3D>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        movement = new Vector3(horizontal, 0f, vertical).normalized;

        if (movement != Vector3.zero && playerCombat != null)
        {
            playerCombat.StopAutoAttack();
        }

        if (animator != null)
        {
            animator.SetFloat("Speed", movement.magnitude);
        }

        RotatePlayer();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(
            rb.position +
            speed * Time.fixedDeltaTime * movement
        );
    }

    private void RotatePlayer()
    {
        if (movement == Vector3.zero)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(movement);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}