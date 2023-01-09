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
    public List<bool> hasCrop = new List<bool>();
    public List<int> unplantedTiles = new List<int>();
    
    private void Awake()
    {
        instance = this;
        fieldTiles = GetComponentsInChildren<FieldTile>();
        hasCrop = new List<bool>(new bool[fieldTiles.Length]);
        
    }

    private void Start()
    {
        InitGameTiles();
        GameManager.instance.onNewDay += OnNewDay;
    }

    private void UpdateCropList()
    {
        
        unplantedTiles.Clear();
        for (int i = 0; i < fieldTiles.Length; i++)
        {
            hasCrop[i] = fieldTiles[i].HasCrop();;
            if (!hasCrop[i])
            {
                unplantedTiles.Add(i);
            }   
        }

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
        UpdateCropList();
        int numPlants = Random.Range(1, 4);
        PlantSeedOnRandomTiles(numPlants);
    }




    void PlantSeedOnRandomTiles(int n = 1)
    {
        
        
        // get unplantedIdx
        List<int> unplantedIdx = new List<int>();
        for (int i = 0; i < hasCrop.Count; i++)
        {
            if (!hasCrop[i])
            {
                unplantedIdx.Add(i);
            }
        }
        
        // get random value from trueIdx
        for (int i = 0; i < n; i++)
        {
            int randomIdx = Random.Range(0, unplantedIdx.Count);
            int randomTileIdx = unplantedIdx[randomIdx];
            fieldTiles[randomTileIdx].PlantSeed();
            unplantedIdx.RemoveAt(randomIdx);
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