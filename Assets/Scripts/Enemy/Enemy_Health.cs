using UnityEngine;

public class Enemy_Health : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    public int health = 1;
    private int current_health;
    void Awake()
    {
        current_health = health;     
    }

    // Update is called once per frame
    void Update()
    {
     if(current_health < 1)
        {
            Die();
        }
    }

    public void hit(int damage)
    {
        current_health -= damage;
    }

    void Die()
    {
        Destroy(this.gameObject);
    }
}
