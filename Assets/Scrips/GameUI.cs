using UnityEngine;
using UnityEngine.SceneManagement;
public class GameUI : MonoBehaviour
{
    [SerializeField] private GameManeger gameManeger;
    public void StartGame()
    {
        gameManeger.StartGame();
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Tải lại cảnh hiện tại để bắt đầu lại trò chơi
    }
    public void ExitGame()
    {
        Application.Quit(); // Thoát khỏi trò chơi
    }
    public void ContineuGame()
    {
        gameManeger.ResumeGame();
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
