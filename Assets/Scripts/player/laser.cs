using UnityEngine;

public class laser : MonoBehaviour
{
    private bool hasHit = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;

        Health health = other.GetComponentInParent<Health>();

        if (health != null)
        {
            hasHit = true;
            health.hit(1);
            Destroy(gameObject);
        }
    }
}