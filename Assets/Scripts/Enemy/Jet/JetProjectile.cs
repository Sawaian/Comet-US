
using UnityEngine;

public class JetProjectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 8f;
    [SerializeField] private int damage = 1;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void SetDirection(Vector3 direction, float speed)
    {
        rb.linearVelocity = direction.normalized * speed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit by jet projectile!");

            // Replace with your player health method.
            // other.GetComponent<PlayerHealth>()?.TakeDamage(damage);

            Destroy(gameObject);
        }
        else if (other.CompareTag("boundary"))
        {
            Destroy(gameObject);
        }
    }
}
