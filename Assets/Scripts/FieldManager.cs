using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FieldManager : MonoBehaviour
{
    //singeleton
    public static FieldManager instance;

    public FieldTile[] fieldTiles;

    // dictionary of tiles
    private Dictionary<int, bool> tilePlantedDict = new Dictionary<int, bool>();


    private void Awake()
    {
        instance = this;
        fieldTiles = GetComponentsInChildren<FieldTile>();
        tilePlantedDict = new Dictionary<int, bool>(fieldTiles.Length);
        for (int i = 0; i < fieldTiles.Length; i++)
        {
            tilePlantedDict[i] = false;
        }
    }

    private void Start()
    {
        InitGameTiles();
        GameManager.instance.onNewDay += OnNewDay;
    }

    private void OnDisable()
    {
        GameManager.instance.onNewDay -= OnNewDay;
    }

    public void InitGameTiles()
    {
        TillAllTiles();
        WaterAllTiles();
    }


    private void OnNewDay()
    {
        WaterAllTiles();
        int numPlants = Random.Range(1, 4);
        PlantSeedOnRandomTiles(numPlants);
    }

    List<int> getIdx(bool val)
    {
        List<int> idx = new List<int>();
        foreach (KeyValuePair<int, bool> entry in tilePlantedDict)
        {
            if (entry.Value == val)
            {
                idx.Add(entry.Key);
            }
        }

        return idx;
    }

    public List<GameObject> getGrownPlantList()
    {
        List<GameObject> grownPlants = new List<GameObject>();
        List<int> plantIdxList = getIdx(true);

        foreach (int idx in plantIdxList)
        {
            Crop c = fieldTiles[idx].GetComponentInChildren<Crop>();
            if (c != null && c.CanHarvest())
            {
                grownPlants.Add(c.gameObject);
            }
        }

        return grownPlants;
    }

    void PlantSeedOnRandomTiles(int n = 1)
    {
        // Get all keys with "false" value from tilePlanetedDict
        List<int> unplantedIdx = getIdx(false);

        // get random value from trueIdx
        for (int i = 0; i < n; i++)
        {
            int randomIdx = Random.Range(0, unplantedIdx.Count);
            int randomTileIdx = unplantedIdx[randomIdx];
            fieldTiles[randomTileIdx].PlantSeed();
            unplantedIdx.RemoveAt(randomIdx);
            tilePlantedDict[randomIdx] = true;
        }
    }

    void WaterAllTiles()
    {
        foreach (FieldTile tile in fieldTiles)
        {
            tile.Water();
        }
    }

    void TillAllTiles()
    {
        foreach (FieldTile tile in fieldTiles)
        {
            tile.Till();
        }
    }
}