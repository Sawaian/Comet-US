
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetSpawner : MonoBehaviour
{
    [SerializeField] private GameObject jetPrefab;
    [SerializeField] private Transform jetPool;

    [SerializeField] private float formationDelay = 0.8f;
    [SerializeField] private float waveDelay = 2f;

    private bool started = false;

    public void BeginJetWaves()
    {
        if (started) return;

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
            List<GameObject> activeJets =
                new List<GameObject>();

            for (int i = 0; i < wave; i++)
            {
                GameObject jet = Instantiate(
                    jetPrefab,
                    jetPool
                );

                activeJets.Add(jet);

                // Jets enter in sequence.
                yield return new WaitForSeconds(
                    formationDelay
                );
            }

            // Wait until every jet in this wave dies.
            yield return new WaitUntil(() =>
            {
                activeJets.RemoveAll(jet => jet == null);
                return activeJets.Count == 0;
            });

            yield return new WaitForSeconds(waveDelay);
        }

        Debug.Log("All jet waves completed!");
    }
}
