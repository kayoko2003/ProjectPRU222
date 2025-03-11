using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Paused : MonoBehaviour
{
    public GameObject pauseMenu;
    private PlayerController playerController;
    public static bool isPaused;
    void Start()
    {
        pauseMenu.SetActive(false);
        playerController = FindObjectOfType<PlayerController>();
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }


    public void GoToMain()
    {
        Time.timeScale = 1f;

        // Kiểm tra nếu playerController tồn tại và disable PlayerControls trước khi chuyển scene
        if (playerController != null)
        {
            playerController.DisableControls();
        }

        SceneManager.LoadScene(0);
        isPaused = false;
    }

    public void GameOverRestart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }


}
