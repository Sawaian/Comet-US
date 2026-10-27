using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Health : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    public int health = 1;
    private int current_health;
    void Awake()
    {
        current_health = health;
        gameManager = FindAnyObjectByType<GameManager>();
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void hit(int damage)
    {
        current_health -= damage;
        if (current_health < 1)
        {
            Die();
        }
    }

    void allDead()
    {
        if(gameManager.howitzers.Count < 1)
        {
            gameManager.firstLevel = true;
        }
    }

    void Die()
    {
        Debug.Log("Die called");

        for(int i = 0; i < gameManager.howitzers.Count; i++)
        {
            if(this.gameObject == gameManager.howitzers[i])
            {
                Debug.Log("Object: " + this.gameObject);

                gameManager.howitzers.RemoveAt(i);
                Destroy(this.gameObject);
                allDead();

                return;
            }
        }
    }
}
