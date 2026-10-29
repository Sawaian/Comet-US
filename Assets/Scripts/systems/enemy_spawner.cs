
using UnityEngine;

public class enemy_spawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform pool;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject Howitzer;

    [Header("Spawn Settings")]
    [SerializeField] private float spawn_Padding = 9f;

    private void Awake()
    {
        if (gameManager == null)
            gameManager = FindAnyObjectByType<GameManager>();
    }

    public void SpawnEnemy(
        Vector3 spawnPos,
        int count = 3,
        float spacing = 9f)
    {
        if (Howitzer == null || gameManager == null)
        {
            Debug.LogError("Spawner missing references!");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            GameObject newHowitzer = Instantiate(
                Howitzer,
                spawnPos,
                Quaternion.identity,
                pool
            );

            gameManager.RegisterEnemy(newHowitzer);

            Debug.Log(
                "Spawned Howitzer " + (i + 1) +
                " at " + spawnPos
            );

            spawnPos.x += spacing;
        }
    }
}
