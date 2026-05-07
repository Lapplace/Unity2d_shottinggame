using UnityEngine;
using UnityEngine.UI;
using Unity.Cinemachine;
public class GameManeger : MonoBehaviour
{
    private int currentEnergy;
    [SerializeField] private int maxEnergy = 3;
    [SerializeField] private GameObject boss;
    [SerializeField] private GameObject enemySpaner;
    private bool bossCalled = false;
    [SerializeField] private Image energyBar;
    [SerializeField] private GameObject gameUI;

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject winGameMenu;
    [SerializeField] private AudioManeger audioManeger;
    [SerializeField] private CinemachineCamera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentEnergy = 0;
        UpdateEnergyBar();
        boss.SetActive(false);
        //enemySpaner.SetActive(true);
        MainMenu();
        audioManeger.StopAudioGame(); // Stop all music at the start
        cam.Lens.OrthographicSize = 5f; // Set default camera size
    }
    public void AddEnergy()
    {
        currentEnergy += 1;
        UpdateEnergyBar();
        if (bossCalled) return;
        if (currentEnergy == maxEnergy)
        {
            CallBoss();
        }
    }
    private void CallBoss()
    {
        bossCalled = true;
        boss.SetActive(true);
        enemySpaner.SetActive(false);
        gameUI.SetActive(false);
        cam.Lens.OrthographicSize = 8f; // Zoom in the camera for boss fight
        audioManeger.PlayBossMusic(); // Play boss music
    }
    private void UpdateEnergyBar()
    {
        if (energyBar != null)
        {
            float fillAmount = Mathf.Clamp01((float)currentEnergy / (float)maxEnergy);
            energyBar.fillAmount = fillAmount;
        }
    }
    // menu 
    public void MainMenu()
    {
        mainMenu.SetActive(true);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        winGameMenu.SetActive(false);
        Time.timeScale = 0f; // Pause the game
    }
    public void PauseMenu()
    {
        mainMenu.SetActive(false);
        pauseMenu.SetActive(true);
        gameOverMenu.SetActive(false);
        winGameMenu.SetActive(false);
        Time.timeScale = 0f; // Pause the game
    }
    public void GameOverMenu()
    {
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(true);
        winGameMenu.SetActive(false);
        Time.timeScale = 0f; // Pause the game
    }
    public void WinGameMenu()
    {
        winGameMenu.SetActive(true);
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        Time.timeScale = 0f; // Pause the game
    }
    public void StartGame()
    {
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        winGameMenu.SetActive(false);
        Time.timeScale = 1f; // Resume the game
        audioManeger.PlayDefaultMusic(); // Play default music
    }
    public void ResumeGame()
    {
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        winGameMenu.SetActive(false);
        Time.timeScale = 1f; // Resume the game
    }
}
