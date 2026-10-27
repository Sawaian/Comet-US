
using UnityEngine;

public class laser : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Health health = other.GetComponentInParent<Health>();

        if (health != null && !other.CompareTag("Player"))
        {
            health.hit(1);
            Destroy(gameObject);
        }
    }
}
