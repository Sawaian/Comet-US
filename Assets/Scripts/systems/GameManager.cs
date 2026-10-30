
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject winPanel;

    [Header("HUD")]
    [SerializeField] private GameObject playerLives;

    [Header("Spawners")]
    [SerializeField] private enemy_spawner enemySpawner;
    [SerializeField] private JetSpawner jetSpawner;
    [SerializeField] private MissileTruckSpawner missileTruckSpawner;

    [Header("Shields")]
    [SerializeField] private GameObject Wall;
    [SerializeField]
    private Vector3 wallPos =
        new Vector3(0.16f, 13.28f, 0f);
    [SerializeField] private float wallSpacing = -15f;
    [SerializeField] private int numWall = 3;

    [SerializeField] private List<GameObject> walls = new();

    [Header("Shield Audio")]
    [SerializeField] private AudioSource shieldAudioSource;
    [SerializeField] private AudioClip shieldDestroySound;

    [Header("Victory Audio")]
    [SerializeField] private AudioSource victoryAudioSource;
    [SerializeField] private AudioClip victorySound;

    [Header("Enemy Tracking")]
    [SerializeField] private List<GameObject> activeEnemies = new();

    [Header("Game State")]
    [SerializeField] private int currentPhase = 1;

    private bool gameOver = false;
    private bool phaseSpawningComplete = false;
    private bool phaseCleared = false;
    private bool gameStarted = false;
    private bool allPhasesCleared = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        WallSpawn();

        startPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
        playerLives.SetActive(false);

        Time.timeScale = 0f;
    }

    public void PlayGame()
    {
        if (gameOver || gameStarted) return;

        gameStarted = true;

        startPanel.SetActive(false);
        playerLives.SetActive(true);

        Time.timeScale = 1f;

        StartPhase1();
    }

    public void WinGame()
    {
        if (gameOver || !allPhasesCleared) return;

        gameOver = true;

        Debug.Log("YOU WIN!");

        playerLives.SetActive(false);
        winPanel.SetActive(true);

        // Play victory sound.
        if (victoryAudioSource != null &&
            victorySound != null)
        {
            victoryAudioSource.PlayOneShot(victorySound);
        }

        Time.timeScale = 0f;
    }

    public void LoseGame()
    {
        if (gameOver) return;

        gameOver = true;

        Debug.Log("GAME OVER!");

        playerLives.SetActive(false);
        gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    private void WallSpawn()
    {
        walls.Clear();

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

    private void DestroyNextShield()
    {
        if (walls.Count == 0)
            return;

        GameObject wall = walls[0];
        walls.RemoveAt(0);

        if (wall != null)
        {
            Destroy(wall);

            if (shieldAudioSource != null &&
                shieldDestroySound != null)
            {
                shieldAudioSource.PlayOneShot(
                    shieldDestroySound
                );
            }
        }

        Debug.Log("Shield destroyed!");
    }

    public void RegisterEnemy(GameObject enemy)
    {
        if (enemy == null || gameOver || phaseCleared)
            return;

        Health health = enemy.GetComponent<Health>();

        if (health == null)
        {
            Debug.LogError(
                "Enemy missing Health: " + enemy.name
            );
            return;
        }

        if (activeEnemies.Contains(enemy))
            return;

        activeEnemies.Add(enemy);

        health.OnDeath += OnEnemyDeath;
    }

    public void RegisterHowitzer(GameObject howitzer)
    {
        RegisterEnemy(howitzer);
    }

    private void OnEnemyDeath(GameObject enemy)
    {
        if (gameOver) return;

        if (!activeEnemies.Remove(enemy))
            return;

        Debug.Log(
            "Enemies remaining: " + activeEnemies.Count
        );

        CheckPhaseClear();
    }

    public void PhaseSpawningFinished()
    {
        if (gameOver || phaseCleared)
            return;

        phaseSpawningComplete = true;

        CheckPhaseClear();
    }

    private void CheckPhaseClear()
    {
        if (gameOver || phaseCleared)
            return;

        if (!phaseSpawningComplete ||
            activeEnemies.Count > 0)
            return;

        phaseCleared = true;

        Debug.Log(
            "PHASE " + currentPhase + " CLEARED!"
        );

        DestroyNextShield();

        switch (currentPhase)
        {
            case 1:
                StartPhase2();
                break;

            case 2:
                StartPhase3();
                break;

            case 3:
                allPhasesCleared = true;
                Debug.Log(
                    "ALL PHASES CLEARED! REACH THE GROUND!"
                );
                break;
        }
    }

    private void PreparePhase(int phase)
    {
        currentPhase = phase;

        activeEnemies.Clear();
        phaseSpawningComplete = false;
        phaseCleared = false;
    }

    private void StartPhase1()
    {
        PreparePhase(1);

        Debug.Log("STARTING PHASE 1");

        enemySpawner.SpawnEnemy(
            new Vector3(-9f, -18.5f, 0f),
            3
        );

        PhaseSpawningFinished();
    }

    public void StartPhase2()
    {
        if (gameOver || currentPhase != 1)
            return;

        PreparePhase(2);

        Debug.Log("STARTING PHASE 2");

        enemySpawner.SpawnEnemy(
            new Vector3(-6f, -18.5f, 0f),
            5,
            3f
        );

        jetSpawner.BeginJetWaves(2);
    }

    public void StartPhase3()
    {
        if (gameOver || currentPhase != 2)
            return;

        PreparePhase(3);

        Debug.Log("STARTING PHASE 3");

        // Five Howitzers.
        enemySpawner.SpawnEnemy(
            new Vector3(-6f, -18.5f, 0f),
            5,
            3f
        );

        // Two Missile Trucks.
        missileTruckSpawner.BeginWaves();

        // Faster jet waves.
        jetSpawner.BeginJetWaves(3);
    }
}
