
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Spawners")]
    [SerializeField] private enemy_spawner enemySpawner;
    [SerializeField] private JetSpawner jetSpawner;

    [Header("Walls")]
    [SerializeField] private GameObject Wall;
    [SerializeField]
    private Vector3 wallPos =
        new Vector3(0.16f, 13.28f, 0f);
    [SerializeField] private float wallSpacing = -15f;
    [SerializeField] private int numWall = 3;

    [SerializeField]
    public List<GameObject> walls =
        new List<GameObject>();

    [SerializeField]
    public List<GameObject> howitzers =
        new List<GameObject>();

    private int gameScore;
    private int currentPhase = 1;

    public bool firstLevel = false;

    void Start()
    {
        WallSpawn();
        StartWave();
    }

    void Update()
    {
        if (firstLevel && currentPhase == 1)
        {
            firstLevel = false;

            // Destroy first defensive wall.
            if (walls.Count > 0)
            {
                GameObject wall = walls[0];
                walls.RemoveAt(0);
                Destroy(wall);
            }

            StartPhase2();
        }
    }

    void StartWave()
    {
        enemySpawner.SpawnEnemy(
            new Vector3(-9f, -18.5f, 0f),
            3
        );
    }

    void WallSpawn()
    {
        for (int i = 0; i < numWall; i++)
        {
            Vector3 spawnPos = wallPos +
                new Vector3(0, i * wallSpacing, 0);

            GameObject wall = Instantiate(
                Wall,
                spawnPos,
                Quaternion.identity
            );

            walls.Add(wall);
        }
    }

    public void RegisterHowitzer(GameObject howitzer)
    {
        if (howitzer == null)
            return;

        Health health = howitzer.GetComponent<Health>();

        if (health == null)
        {
            Debug.LogError(
                "Howitzer missing Health component: " +
                howitzer.name
            );
            return;
        }

        if (howitzers.Contains(howitzer))
            return;

        howitzers.Add(howitzer);
        health.OnDeath += OnHowitzerDeath;
    }

    private void OnHowitzerDeath(GameObject howitzer)
    {
        howitzers.Remove(howitzer);

        Debug.Log(
            "Howitzers remaining: " + howitzers.Count
        );

        if (howitzers.Count == 0 && currentPhase == 1)
        {
            firstLevel = true;
        }
    }


    public void StartPhase2()
    {
        if (currentPhase != 1)
            return;

        currentPhase = 2;
        firstLevel = false;

        Debug.Log("STARTING PHASE 2");

        // Five Howitzers in a compact formation.
        enemySpawner.SpawnEnemy(
            new Vector3(-6f, -18.5f, 0f),
            5,
            3f
        );

        // Jets begin at the same time.
        jetSpawner.BeginJetWaves();

        Debug.Log("Phase 2 Howitzers: " + howitzers.Count);
    }

}
