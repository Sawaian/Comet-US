using UnityEngine;

public class playerHealth : MonoBehaviour
{

    public int health = 3;
    GameObject projectile;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bullet"))
        {
            health--;
            healthCheck(health);
            Destroy(other.gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        health = health - damage;
        healthCheck(health);
    }


    void healthCheck(int health)
    {
        
        if (health < 1) 
        {
            //Debug.Log("ship destroyed" );
        }
    }
}
