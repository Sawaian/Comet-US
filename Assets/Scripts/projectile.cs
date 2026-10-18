using UnityEngine;

public class projectile : MonoBehaviour
{

    Rigidbody m_Rigidbody;
    public float initVelocity = 2f;
    public int damage = 1;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //fetch rigidboy from GO this scrip attached
        m_Rigidbody = GetComponent<Rigidbody>();

        Debug.Log("Projectile Start running!");

        m_Rigidbody.AddForce(Vector3.up * initVelocity, ForceMode.Impulse);
    }

    // Update is called once per frame
}
