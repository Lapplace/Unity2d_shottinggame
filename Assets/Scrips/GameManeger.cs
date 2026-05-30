using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManeger : MonoBehaviour
{
    private int currentEnergy;
    private const string KeyCoin = "coin";
    private int collectedEnergy;
    [Header("Gameplay")]
    //[SerializeField] private int maxEnergy = 3;
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject enemySpaner;
    [SerializeField] private Image energyBar;
    [SerializeField] private GameObject gameUI;

    [SerializeField] private TMP_Text gameUiEnergyText;
    [SerializeField] private TMP_Text gameOverEnergyText;
    [SerializeField] private TMP_Text winGameEnergyText;
    [SerializeField] private PlayerProgression progression;
    [Header("Menus")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject winGameMenu;

    [Header("Systems")]
    [SerializeField] private AudioManeger audioManeger;
    [SerializeField] private CinemachineCamera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool bossCalled;

    private void Start()
    {
        collectedEnergy = 0;
        bossCalled = false;
        BindProgression();
        UpdateEnergyBar();
        UpdateEnergyTexts();
        if (boss != null)
        {
            boss.SetActive(false);
        }
        StartGame();
        

        if (cam != null)
        {
            cam.Lens.OrthographicSize = 5f;
        }
    }
    private void OnEnable()
    {
        BindProgression();
    }

    private void OnDisable()
    {
        if (progression != null)
        {
            progression.OnExpChanged -= HandleExpChanged;
        }
    }

    private void BindProgression()
    {
        if (progression == null)
        {
            progression = FindFirstObjectByType<PlayerProgression>();
        }

        if (progression == null)
        {
            return;
        }

        progression.OnExpChanged -= HandleExpChanged;
        progression.OnExpChanged += HandleExpChanged;
        HandleExpChanged(progression.CurrentExp, progression.ExpToNextLevel);
    }
    public void AddEnergy()
    {
        collectedEnergy += 1;
        SaveCollectedEnergyToCoin(1);
        UpdateEnergyTexts();
    }

    private void HandleExpChanged(int currentExp, int maxExp)
    {
        UpdateEnergyBar();
        if (bossCalled || progression == null)
        {
            return;
        }

        if (progression.IsMaxLevel)
        {
            CallBoss();
        }
    }
    private void SaveCollectedEnergyToCoin(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        int currentCoin = PlayerPrefs.GetInt(KeyCoin, 0);
        PlayerPrefs.SetInt(KeyCoin, currentCoin + amount);
        PlayerPrefs.Save();
    }

    private void UpdateEnergyTexts()
    {
        string value = collectedEnergy.ToString();

        if (gameUiEnergyText != null)
        {
            gameUiEnergyText.text = value;
        }

        if (gameOverEnergyText != null)
        {
            gameOverEnergyText.text = value;
        }

        if (winGameEnergyText != null)
        {
            winGameEnergyText.text = value;
        }
    }

    
    private void CallBoss()
    {
        bossCalled = true;
        if (boss != null)
        {
            boss.SetActive(true);
        }

        if (enemySpaner != null)
        {
            enemySpaner.SetActive(false);
        }

        if (cam != null)
        {
            cam.Lens.OrthographicSize = 8f;
        }

        if (audioManeger != null)
        {
            audioManeger.PlayBossMusic();
        }
    }
    private void UpdateEnergyBar()
    {
        if (energyBar != null)
        {
            if(progression == null || progression.IsMaxLevel || progression.ExpToNextLevel <= 0)
            {
                energyBar.fillAmount = 1f;
                return;
            }

            float fillAmount = Mathf.Clamp01((float)progression.CurrentExp / progression.ExpToNextLevel);
            energyBar.fillAmount = fillAmount;
        }
    }
    public void PauseMenu()
    {
        //HideLegacyUiMain();

        if (pauseMenu != null) pauseMenu.SetActive(true);
        if (gameOverMenu != null) gameOverMenu.SetActive(false);
        if (winGameMenu != null) winGameMenu.SetActive(false);

        Time.timeScale = 0f;
    }
    public void GameOverMenu()
    {
        //HideLegacyUiMain();

        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (gameOverMenu != null) gameOverMenu.SetActive(true);
        if (winGameMenu != null) winGameMenu.SetActive(false);
        UpdateEnergyTexts();
        Time.timeScale = 0f;
    }
    public void WinGameMenu()
    {
        //HideLegacyUiMain();

        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (gameOverMenu != null) gameOverMenu.SetActive(false);
        if (winGameMenu != null) winGameMenu.SetActive(true);
        UpdateEnergyTexts();
        Time.timeScale = 0f;
    }
    public void StartGame()
    {
        //HideLegacyUiMain();

        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (gameOverMenu != null) gameOverMenu.SetActive(false);
        if (winGameMenu != null) winGameMenu.SetActive(false);

        Time.timeScale = 1f;
        UpdateEnergyTexts();
        if (audioManeger != null)
        {
            audioManeger.PlayDefaultMusic();
        }
    }
    public void ResumeGame()
    {
        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (gameOverMenu != null) gameOverMenu.SetActive(false);
        if (winGameMenu != null) winGameMenu.SetActive(false);

        Time.timeScale = 1f;
    }
}
