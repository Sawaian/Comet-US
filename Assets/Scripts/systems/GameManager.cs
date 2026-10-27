using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private enemy_spawner enemySpawner;
    [SerializeField] public GameObject Wall;
    [SerializeField] public Vector3 wallPos = new Vector3(0.16f, 13.28f, 0);
    [SerializeField] public float wallSpacing = -15f;
    [SerializeField] public int numWall = 3;
    [SerializeField] public List<GameObject> walls = new List<GameObject>();
    [SerializeField] public List<GameObject> howitzers = new List<GameObject>();
    [SerializeField] private JetSpawner jetSpawner;

    private int HowitzersAlive;
    private int gameScore;
    public bool firstLevel = false;
    void Start()
    {
        StartWave();
        WallSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (firstLevel && walls.Count > 0)
        {
            GameObject wall = walls[0];

            walls.RemoveAt(0);
            Destroy(wall);

            jetSpawner.BeginJetWaves();

            firstLevel = false;
        }
    }

    void StartWave()
    {
        enemySpawner.SpawnEnemy(
            new Vector3(-9f, -18.5f, 0f)
            );
    }

    void WallSpawn()
    {
        for(int i = 0; i < numWall; i++)
        {

            Vector3 spawnPos = wallPos + new Vector3(0, i * wallSpacing, 0);

            GameObject wall = Instantiate(Wall, spawnPos, Quaternion.identity);
            walls.Add(wall);
        }
    }



    public void RegisterHowitzer(GameObject howitzer)
    {
        if (!howitzers.Contains(howitzer))
        {
            howitzers.Add(howitzer);

            Health health = howitzer.GetComponent<Health>();

            if (health != null)
            {
                health.OnDeath += OnHowitzerDeath;
            }
        }
    }

    private void OnHowitzerDeath(GameObject howitzer)
    {
        howitzers.Remove(howitzer);

        if (howitzers.Count == 0)
        {
            firstLevel = true;
        }
    }

}
