using UnityEngine;
using UnityEngine.UI;
using Unity.Cinemachine;
public class GameManeger : MonoBehaviour
{
    private int currentEnergy;
    [Header("Gameplay")]
    [SerializeField] private int maxEnergy = 3;
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject enemySpaner;
    [SerializeField] private Image energyBar;
    [SerializeField] private GameObject gameUI;
    [Header("Menus")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject winGameMenu;
    //[Header("Optional legacy menu root (Ui_Main)")]
    //[SerializeField] private GameObject uiMainRoot;

    [Header("Systems")]
    [SerializeField] private AudioManeger audioManeger;
    [SerializeField] private CinemachineCamera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool bossCalled;

    private void Start()
    {
        currentEnergy = 0;
        bossCalled = false;
        UpdateEnergyBar();
        if (boss != null)
        {
            boss.SetActive(false);
        }
        StartGame();
        //if (audioManeger != null)
        //{
        //    audioManeger.StopAudioGame();
        //}

        if (cam != null)
        {
            cam.Lens.OrthographicSize = 5f;
        }
    }
    //private void HideLegacyUiMain()
    //{
    //    if (uiMainRoot != null)
    //    {
    //        uiMainRoot.SetActive(false);
    //    }
    //}
    public void AddEnergy()
    {
        currentEnergy += 1;
        UpdateEnergyBar();
        if (bossCalled)
        {
            return;
        }

        if (currentEnergy >= maxEnergy)
        {
            CallBoss();
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

        if (gameUI != null)
        {
            gameUI.SetActive(false);
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
            float fillAmount = Mathf.Clamp01((float)currentEnergy /maxEnergy);
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

        Time.timeScale = 0f;
    }
    public void WinGameMenu()
    {
        //HideLegacyUiMain();

        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (gameOverMenu != null) gameOverMenu.SetActive(false);
        if (winGameMenu != null) winGameMenu.SetActive(true);

        Time.timeScale = 0f;
    }
    public void StartGame()
    {
        //HideLegacyUiMain();

        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (gameOverMenu != null) gameOverMenu.SetActive(false);
        if (winGameMenu != null) winGameMenu.SetActive(false);

        Time.timeScale = 1f;

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
