
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] int speed = 10;

    [Header("Screen Boundaries")]
    [SerializeField] float negXClamp = -11.12401f;
    [SerializeField] float posXClamp = 11.27597f;
    [SerializeField] float posYClamp = 22.15563f;
    [SerializeField] float negYClamp = -18f;

    [Header("HUD")]
    [SerializeField] float livesAreaPadding = 2f;

    private Rigidbody rb;
    private Vector3 movement;

    private bool blockedX;
    private bool blockedY;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        movement = new Vector3(
            horizontal,
            vertical,
            0f
        ).normalized;
    }

    void FixedUpdate()
    {
        Vector3 direction = movement;

        // Preserve wall collision behavior.
        if (blockedX)
            direction.x = 0f;

        if (blockedY)
            direction.y = 0f;

        // Calculate proposed movement.
        Vector3 nextPosition =
            rb.position +
            direction.normalized * speed * Time.fixedDeltaTime;

        // Clamp horizontal position.
        nextPosition.x = Mathf.Clamp(
            nextPosition.x,
            negXClamp,
            posXClamp
        );

        // Only restrict upward movement.
        // Allow player to reach the ground to win.
        nextPosition.y = Mathf.Min(
            nextPosition.y,
            posYClamp - livesAreaPadding
        );

        // Keep Z position unchanged.
        nextPosition.z = rb.position.z;

        rb.MovePosition(nextPosition);

        blockedX = false;
        blockedY = false;
    }

    void OnCollisionStay(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Wall"))
            return;

        foreach (ContactPoint contact in collision.contacts)
        {
            Vector3 normal = contact.normal;

            if (Mathf.Abs(normal.x) > 0.9f &&
                movement.x * normal.x < 0f)
            {
                blockedX = true;
            }

            if (Mathf.Abs(normal.y) > 0.9f &&
                movement.y * normal.y < 0f)
            {
                blockedY = true;
            }
        }
    }
}
