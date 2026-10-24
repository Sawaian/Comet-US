using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private enemy_spawner enemySpawner;
    [SerializeField] public List<GameObject> howitzers = new List<GameObject>();

    private int HowitzersAlive;
    private int gameScore;
    void Start()
    {
        StartWave();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartWave()
    {
        enemySpawner.SpawnEnemy(
            new Vector3(-9f, -18.5f, 0f)
            );
    }


    void AddHowitizersAlive(int addHowie)
    {
        HowitzersAlive += addHowie;
    }

    void SubHowitzersAlive(int subHowie)
    {
        HowitzersAlive -= subHowie;
    }
}
