
using System;
using UnityEngine;

public class Enemy_Health : MonoBehaviour
{
    [SerializeField] private int health = 1;

    private int current_health;
    private bool isDead = false;

    public event Action<GameObject> OnDeath;

    private void Awake()
    {
        current_health = health;
    }

    public void hit(int damage)
    {
        if (isDead || damage <= 0)
            return;

        current_health -= damage;

        if (current_health <= 0)
        {
            Die();
        }
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
