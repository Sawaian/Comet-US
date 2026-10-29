
using UnityEngine;

public class SnakeBullet : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float lifetime = 8f;

    private Vector3 direction = Vector3.up;

    public void Initialize(Vector3 newDirection)
    {
        direction = newDirection.normalized;
    }

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position +=
            direction * speed * Time.deltaTime;
    }
}
