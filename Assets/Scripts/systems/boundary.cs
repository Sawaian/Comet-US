using UnityEngine;

public class boundary : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("bullet"))
        {
            Debug.Log("collision detected: " + other.gameObject);
            Destroy(other.gameObject);
        }
      
    }
}
