using UnityEngine;
using System.Collections.Generic;

public class enemy_spawner : MonoBehaviour
{
    [SerializeField] Transform pool;
    [SerializeField] GameManager gameManager;
    public GameObject Howitzer;
    public Vector3 spawnPos = new Vector3(-9f,-18.5f,0f);
    public float spawn_Padding = 9f;
   

    public void SpawnEnemy(Vector3 spawnPos)
    {
        for (int i = 0; i < 3; i++)
        {

            GameObject howie = Instantiate(Howitzer, spawnPos, Quaternion.identity, pool);
            AddObjectToList(howie);
          
            //Could use the spawnPos to update padding on the X by multiplying it by i.
            spawnPos.x = spawnPos.x + spawn_Padding;
        }
    }

    private void AddObjectToList(GameObject gameObject)
    {
        gameManager.howitzers.Add(gameObject);
    }
}
