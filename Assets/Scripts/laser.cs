using UnityEngine;

public class laser : MonoBehaviour
{   

    private void OnTriggerEnter(Collider other)
    {
        Enemy_Health enemy = other.gameObject.GetComponent<Enemy_Health>();
        
        if(enemy != null)
        {
            enemy.hit(1);
            Destroy(gameObject);
        }
    }
}
