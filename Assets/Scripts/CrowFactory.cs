using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowFactory : MonoBehaviour
{
    public GameObject crowPrefab;
    public Transform[] spawnPoints;


    void Start()
    {
        StartCoroutine(SpawnCrows());
    }

    IEnumerator SpawnCrows()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2f, 10f));
            int numCrow = Random.Range(1, 3);
            SpawnCrow(numCrow);
        }
    }

    void SpawnCrow(int n)
    {
        for (int i = 0; i < n; i++)
        {
            Vector2 startPos = GetRandomSpawnPoint();
            Instantiate(crowPrefab, startPos, Quaternion.identity);
        }
    }


    Vector2 GetRandomSpawnPoint()
    {
        // Get random spawn point
        int randomIndex = Random.Range(0, spawnPoints.Length);
        return spawnPoints[randomIndex].position;
    }
}