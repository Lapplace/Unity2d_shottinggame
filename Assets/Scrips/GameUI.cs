using UnityEngine;
using UnityEngine.SceneManagement;
public class GameUI : MonoBehaviour
{
    [SerializeField] private GameManeger gameManeger;
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    private void Awake()
    {
        if (gameManeger == null)
        {
            gameManeger = FindFirstObjectByType<GameManeger>();
        }
    }

    private bool HasManager()
    {
        if (gameManeger != null)
        {
            return true;
        }

        Debug.LogError("GameUI: Missing GameManeger reference. Please assign it in Inspector.");
        return false;
    }
    public void StartGame()
    {
        if (!HasManager()) return;
        gameManeger.StartGame();
    }
    public void PauseGame()
    {
        if (!HasManager()) return;
        gameManeger.PauseMenu();
    }
    public void ContinueGame()
    {
        if (!HasManager()) return;
        gameManeger.ResumeGame();
    }
    public void OpenGameOverPanel()
    {
        if (!HasManager()) return;
        gameManeger.GameOverMenu();
    }

    public void OpenWinPanel()
    {
        if (!HasManager()) return;
        gameManeger.WinGameMenu();
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
