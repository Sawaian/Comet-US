
using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int health = 1;

    private int currentHealth;
    private bool isDead = false;

    public event Action<GameObject> OnDeath;
    public event Action OnDamaged;

    private void Awake()
    {
        currentHealth = health;
    }

    public void hit(int damage)
    {
        if (isDead || damage <= 0)
            return;

        currentHealth -= damage;

        Debug.Log(gameObject.name + " HP remaining: " + currentHealth);

        OnDamaged?.Invoke();

        if (currentHealth <= 0)
            Die();
    }

    public void TakeDamage(int damage)
    {
        hit(damage);
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;

        Debug.Log("Enemy destroyed: " + gameObject.name);

        OnDeath?.Invoke(gameObject);
        Destroy(gameObject);
    }
}
