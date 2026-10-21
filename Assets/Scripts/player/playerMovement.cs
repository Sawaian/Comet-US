using UnityEngine;

public class playerMovement : MonoBehaviour
{
    [SerializeField] int speed = 10;
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

        movement = new Vector3(horizontal, vertical, 0f).normalized;
    }

    void FixedUpdate()
    {
        Vector3 direction = movement;

        // Only remove movement into a wall.
        if (blockedX)
            direction.x = 0f;

        if (blockedY)
            direction.y = 0f;

        rb.linearVelocity = direction.normalized * speed;

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

