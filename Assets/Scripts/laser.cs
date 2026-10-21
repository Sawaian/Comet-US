using UnityEngine;

public class laser : MonoBehaviour
{   

    private void OnTriggerEnter(Collider other)
    {
        Enemy_Health enemy = other.GetComponent<Enemy_Health>();
        Debug.Log("Hit object: " + other.gameObject.name);
        Debug.Log("Parent object: " + other.transform.parent?.name);
        Debug.Log("Enemy component found: " + (enemy != null));

        if (enemy != null )
        {
            Debug.Log("Enemy detected: " + other.gameObject);
            enemy.hit(1); 
            Destroy(gameObject);
        }
       
    }
}
