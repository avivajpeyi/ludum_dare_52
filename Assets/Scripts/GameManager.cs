using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int curDay = 0;
    public int money = 0;
    public int cropInventory = 100;


    public CropData selectedCropToPlant;
    public TextMeshProUGUI statsText;

    public TextMeshProUGUI healthText;
    public GameObject restartPanel;

    [SerializeField] private float dayDuration = 2.0f;

    public event UnityAction onNewDay;

    //singeleton
    public static GameManager instance;

    private bool _gameOver = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void Start()
    {
        StartGame();
    }

    public void Update()
    {
    }


    private void StartGame()
    {
        StartCoroutine(DayTimer());
        SetNextDay();
    }


    private IEnumerator DayTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(dayDuration);
            curDay++;
            onNewDay?.Invoke();
        }
    }

    private void OnEnable()
    {
        Crop.onPlantCrop += OnPlantCrop;
        Crop.onHarvestCrop += OnHarvestCrop;
    }

    private void OnDisable()
    {
        Crop.onPlantCrop -= OnPlantCrop;
        Crop.onHarvestCrop -= OnHarvestCrop;
    }

    public void SetNextDay()
    {
        curDay++;
        onNewDay?.Invoke();
        UpdateStatsText();
    }

    public void OnPlantCrop(CropData crop)
    {
        cropInventory--;
        UpdateStatsText();
    }

    public void OnHarvestCrop(CropData crop)
    {
        money += crop.sellPrice;
        UpdateStatsText();
    }

    public void PurchaseCrop(CropData crop)
    {
        money -= crop.purchasePrice;
        cropInventory++;
        UpdateStatsText();
    }

    public bool CanPlantCrop()
    {
        return cropInventory > 0;
    }

    public void OnBuyCropButton(CropData crop)
    {
        if (money >= crop.purchasePrice)
        {
            PurchaseCrop(crop);
        }
    }

    void UpdateStatsText()
    {
        statsText.text = $"${money}";
    }

    public void UpdateHealthText(int health)
    {
        char heart = '\u2665';
        string heartTxt = new String(heart, health);
        string emptyTxt = new String(heart, 3 - health);
        healthText.text = "<color=red>" + heartTxt + "<color=black>" + emptyTxt;
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
        restartPanel.SetActive(true);
        StartCoroutine(RestartScreenTimer());
        _gameOver = true;
    }

    IEnumerator RestartScreenTimer()
    {
        TextMeshProUGUI txt = restartPanel.GetComponentInChildren<TextMeshProUGUI>();
        float time = 3;
        while (time >= 0)
        {
            txt.text = $"Restarting in\n{time}...";
            yield return new WaitForSeconds(0.7f);
            time--;
        }

        RestartGame();
    }


    public void RestartGame()
    {
        if (_gameOver)
        {
            Debug.Log("Reload scene");
            SceneManager.LoadSceneAsync(
                SceneManager.GetActiveScene().buildIndex);
        }
    }
}