using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CrowFactory : MonoBehaviour
{
    public float maxFlyDistance = 3;
    public GameObject crowPrefab;
    public Transform[] spawnPoints;
    public List<Crow> crows;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, maxFlyDistance);
    }

    void Start()
    {
        StartCoroutine(RunCrowSpawner());
        StartCoroutine(RunCrowCleanup());
    }

    IEnumerator RunCrowSpawner()
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
            GameObject c = Instantiate(crowPrefab, startPos, Quaternion.identity);
            crows.Add(c.GetComponent<Crow>());
        }
    }

    IEnumerator RunCrowCleanup()
    {
        while (true)
        {
            yield return new WaitForSeconds(3);
            CrowListCleanup();
        }
    }

    void CrowListCleanup()
    {
        Vector3 factoryCenter = transform.position;
        if (crows.Count == 0)
            return;

        for (int i = 0; i < crows.Count; i++)
        {
            Crow c = crows[i];
            if (c.distanceFrom(factoryCenter) > maxFlyDistance)
            {
                crows.Remove(c);
                c.Die();
            }
        }
    }

    public List<GameObject> getCrowList()
    {
        List<GameObject> gos = new List<GameObject>(crows.Count);
        for (int i = 0; i < crows.Count; i++)
        {
            gos[i] = crows[i].gameObject;
        }

        return gos;
    }

    Vector2 GetRandomSpawnPoint()
    {
        // Get random spawn point
        int randomIndex = Random.Range(0, spawnPoints.Length);
        return spawnPoints[randomIndex].position;
    }
}