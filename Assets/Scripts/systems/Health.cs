
using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 1;
    [SerializeField] private float deathDelay = 0f;

    private int currentHealth;
    private bool isDead = false;

    public event Action<GameObject> OnDeath;
    public event Action OnDamaged;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void hit(int damage)
    {
        if (isDead || damage <= 0)
            return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            OnDamaged?.Invoke();
        }
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;

        OnDeath?.Invoke(gameObject);

        Destroy(gameObject, deathDelay);
    }
}
