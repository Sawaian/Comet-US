using UnityEngine;

public class WinBoundary : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<playerHealth>() != null)
        {
            GameManager.Instance.WinGame();
        }
    }
}