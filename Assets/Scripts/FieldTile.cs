using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldTile : MonoBehaviour
{
    private Crop _curCrop;
    public GameObject cropPrefab;

    public SpriteRenderer sr;
    private bool _tilled;

    [Header("Sprites")] public Sprite grassSprite;
    public Sprite tilledSprite;
    public Sprite wateredTilledSprite;

    private void Awake()
    {
        //set the default grass sprite
        sr.sprite = grassSprite;
    }

    public void Interact()
    {
        if (!_tilled)
        {
            Till();
        }
        else if (!HasCrop() && GameManager.instance.CanPlantCrop())
        {
            PlantNewCrop(GameManager.instance.selectedCropToPlant);
        }
        else if (HasCrop() && _curCrop.CanHarvest())
        {
            _curCrop.Harvest();
        }
        else
        {
            Water();
        }
    }


    void PlantNewCrop(CropData crop)
    {
        if (!_tilled)
            return;

        _curCrop = Instantiate(cropPrefab, transform).GetComponent<Crop>();
        _curCrop.Plant(crop);

        GameManager.instance.onNewDay += OnNewDay;
    }

    public void PlantSeed()
    {
        if (HasCrop() || !_tilled)
            return;
        PlantNewCrop(GameManager.instance.selectedCropToPlant);
    }
    

    public void Till()
    {
        _tilled = true;
        sr.sprite = tilledSprite;
    }

    public void Water()
    {
        sr.sprite = wateredTilledSprite;

        if (HasCrop())
        {
            _curCrop.Water();
        }
    }

    void OnNewDay()
    {
        if (_curCrop == null)
        {
            _tilled = false;
            sr.sprite = grassSprite;

            GameManager.instance.onNewDay -= OnNewDay;
        }
        else if (_curCrop != null)
        {
            sr.sprite = tilledSprite;
            _curCrop.NewDayCheck();
        }
    }

    public bool HasCrop()
    {
        return _curCrop != null;
    }
}