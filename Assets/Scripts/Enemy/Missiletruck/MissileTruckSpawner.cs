
using UnityEngine;

public class MissileTruckSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject truckPrefab;
    [SerializeField] private Transform pool;
    [SerializeField] private GameManager gameManager;

    [Header("Positions")]
    [SerializeField] private float spawnX = 18f;
    [SerializeField] private float stopX = 8f;
    [SerializeField] private float spawnY = -24f;

    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = FindAnyObjectByType<GameManager>();
        }
    }

    public void BeginWaves()
    {
        if (truckPrefab == null || gameManager == null)
        {
            Debug.LogError(
                "MissileTruckSpawner missing references!"
            );
            return;
        }

        // Left truck drives right.
        SpawnTruck(
            new Vector3(-spawnX, spawnY, 0f),
            new Vector3(-stopX, spawnY, 0f)
        );

        // Right truck drives left.
        SpawnTruck(
            new Vector3(spawnX, spawnY, 0f),
            new Vector3(stopX, spawnY, 0f)
        );
    }

    private void SpawnTruck(
        Vector3 spawn,
        Vector3 destination)
    {
        GameObject truck = Instantiate(
            truckPrefab,
            spawn,
            Quaternion.identity,
            pool
        );

        MissileTruck controller =
            truck.GetComponent<MissileTruck>();

        if (controller != null)
        {
            controller.Initialize(destination);
        }
        else
        {
            Debug.LogError(
                "Truck missing MissileTruck component!"
            );
        }

        gameManager.RegisterEnemy(truck);
    }
}
