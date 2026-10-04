using UnityEngine;

public class playerMovement : MonoBehaviour
{
    [SerializeField] int speed = 10;

    GameObject GO;

    Transform pos;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        Input.GetKeyDown(KeyCode.W);
        
        Input.GetKeyDown(KeyCode.S);   
        
    }

    private void FixedUpdate()
    {
        
    }
}
