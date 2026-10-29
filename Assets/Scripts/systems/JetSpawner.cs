
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject jetPrefab;
    [SerializeField] private Transform jetPool;
    [SerializeField] private GameManager gameManager;

    [Header("Wave Settings")]
    [SerializeField] private float formationDelay = 0.8f;
    [SerializeField] private float waveDelay = 2f;

    [Header("Phase 3")]
    [SerializeField] private float phase3SpeedMultiplier = 2f;

    private bool started = false;

    private void Awake()
    {
        if (gameManager == null)
            gameManager = FindAnyObjectByType<GameManager>();
    }

    public void BeginJetWaves(int phase = 2)
    {
        if (started)
            return;

        if (jetPrefab == null || gameManager == null)
        {
            Debug.LogError("JetSpawner missing references!");
            return;
        }

        started = true;
        StartCoroutine(SpawnWaves(phase));
    }

    private IEnumerator SpawnWaves(int phase)
    {
        float multiplier = phase == 3
            ? Mathf.Max(1f, phase3SpeedMultiplier)
            : 1f;

        float currentFormationDelay =
            formationDelay / multiplier;

        float currentWaveDelay =
            waveDelay / multiplier;

        for (int wave = 1; wave <= 3; wave++)
        {
            Debug.Log(
                "Phase " + phase +
                " - Starting jet wave: " + wave
            );

            List<GameObject> activeJets = new();

            for (int i = 0; i < wave; i++)
            {
                GameObject jet = Instantiate(
                    jetPrefab,
                    jetPool
                );

                // Set actual flight speed.
                JetPlane controller =
                    jet.GetComponent<JetPlane>();

                if (controller != null)
                    controller.SetPhase(phase);

                // Register for phase completion.
                gameManager.RegisterEnemy(jet);

                activeJets.Add(jet);

                // Overlapping jet arrivals.
                if (i < wave - 1)
                {
                    yield return new WaitForSeconds(
                        currentFormationDelay
                    );
                }
            }

            // Wait until all jets are destroyed.
            yield return new WaitUntil(() =>
            {
                activeJets.RemoveAll(jet => jet == null);
                return activeJets.Count == 0;
            });

            Debug.Log(
                "Phase " + phase +
                " - Jet wave " + wave + " cleared"
            );

            if (wave < 3)
            {
                yield return new WaitForSeconds(
                    currentWaveDelay
                );
            }
        }

        Debug.Log(
            "All Phase " + phase +
            " jet waves completed!"
        );

        started = false;

        gameManager.PhaseSpawningFinished();
    }
}
