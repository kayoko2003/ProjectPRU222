using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverUI;

    // Hàm gọi khi game kết thúc
    public void ShowGameOver()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0f; // Dừng game
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Reset tốc độ game
        gameOverUI.SetActive(false); // Ẩn Game Over UI (nếu cần)

        // Load lại scene từ đầu
        SceneManager.LoadScene("Menu");
    }
    



}
