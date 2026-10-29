using Unity.VisualScripting;
using UnityEngine;

public class shoot : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] public float fireDelay = 1f;
    public GameObject laser_gun;
    public GameObject laser_GO;
    public Vector3 pad = new Vector3(0f, -1.2f, 0f);
    public float initVelocity = 10f;
    public float nextFireTime = 0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Update is called once per frame
    void Update()
    {
        //Listens to button press. 
        if(Input.GetKeyDown(KeyCode.Space) && Time.time >= nextFireTime)
        {
            //turn into method. Could pass in different args for diff weapons. 
            fire();
            nextFireTime = Time.time + fireDelay;
        }

        //Is anchored for as long as button is pushed down. 
        //Moving angles the line. Too ambitious for now. Wishlist. 
    }
    void OnKeyboardInput()
    {

    }

    void fire()
    {

        if (laser_GO == null)
        {
            Debug.LogError("Missing projectile on: " + gameObject.name, gameObject);
            return;
        }
        GameObject laser = Instantiate(laser_GO, laser_gun.transform.position + pad, Quaternion.identity);
        laser.GetComponent<Rigidbody>().AddForce(Vector3.down * initVelocity, ForceMode.Impulse);
        audioSource.PlayOneShot(shootSound);
    }
}
