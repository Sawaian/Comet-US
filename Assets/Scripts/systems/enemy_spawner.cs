using UnityEngine;
using System.Collections.Generic;

public class enemy_spawner : MonoBehaviour
{
    public GameObject Howitzer;
    public Vector3 spawnPos = new Vector3(-9f,-18.5f,0f);
    public float spawn_Padding = 9f;
    [SerializeField] private List<GameObject> howitzers = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < 3; i++)
        {

            GameObject howie = Instantiate(Howitzer, spawnPos, Quaternion.identity);
            AddObjectToList(howie);
            //Could use the spawnPos to update padding on the X by multiplying it by i.
            spawnPos.x = spawnPos.x + spawn_Padding;
        }
    }

    private void AddObjectToList(GameObject gameObject)
    {
        howitzers.Add(gameObject);
    }
}
