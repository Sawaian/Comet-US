
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject jetPrefab;
    [SerializeField] private Transform jetPool;

    [Header("Wave Settings")]
    [SerializeField] private float formationDelay = 0.8f;
    [SerializeField] private float waveDelay = 2f;

    private bool started = false;

    public void BeginJetWaves()
    {
        if (started)
            return;

        if (jetPrefab == null)
        {
            Debug.LogError("Jet prefab is missing!");
            return;
        }

        started = true;
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        // Wave 1: one jet
        // Wave 2: two jets
        // Wave 3: three jets

        for (int wave = 1; wave <= 3; wave++)
        {
            Debug.Log("Starting jet wave: " + wave);

            List<GameObject> activeJets =
                new List<GameObject>();

            for (int i = 0; i < wave; i++)
            {
                GameObject jet = Instantiate(
                    jetPrefab,
                    jetPool
                );

                activeJets.Add(jet);

                // Jets enter one after another.
                yield return new WaitForSeconds(
                    formationDelay
                );
            }

            // Wait until all jets in this wave die.
            yield return new WaitUntil(() =>
            {
                activeJets.RemoveAll(jet => jet == null);
                return activeJets.Count == 0;
            });

            Debug.Log("Jet wave " + wave + " cleared");

            if (wave < 3)
            {
                yield return new WaitForSeconds(
                    waveDelay
                );
            }
        }

        Debug.Log("All jet waves completed!");
    }
}
