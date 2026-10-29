using UnityEngine;

public class playerHealth : MonoBehaviour
{
    public int health = 3;

    [Header("Debug")]
    [SerializeField] private bool invulnerable = false;

    private bool isDead = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bullet"))
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead || invulnerable) return;

        health -= damage;
        healthCheck();
    }

    void healthCheck()
    {
        if (health <= 0)
        {
            isDead = true;
            GameManager.Instance.LoseGame();
            Destroy(gameObject);
        }
    }
}