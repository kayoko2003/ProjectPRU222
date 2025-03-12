using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public enum GameState
    {
        GamePlay,
        Paused,
        GameOver
    }

    public GameState currentState;

    public GameState previousState;

    [Header("Screens")]
    public GameObject pauseScreen;
    public GameObject resultsScreen;

    [Header("Current stats display")]
    public Text currentHealthDisplay;
    public Text currentRecoveryDisplay;
    public Text currentMoveSpeedDisplay;
    public Text currentMightDisplay;
    public Text currentProjectileSpeedDisplay;
    public Text currentMagnetDisplay;

    [Header("Results Screen Display")]
    public Image chosenCharacterImage;
    public Text chosenCharacterName;
    public Text levelReachedDisplay;
    public List<Image> chosenWeaponUI = new List<Image>(6);
    public List<Image> chosenPassiveItemUI = new List<Image>(6);

    [Header("Stopwatch")]
    public float timeLimit;
    float stopWatchTime;
    public Text stopwatchDisplay;

    public bool isGameOver = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DisableScreen();
    }

    void Update()
    {
        switch (currentState)
        {
            case GameState.GamePlay:
                CheckPauseAndResume();
                UpdateStopwatch();
                break;

            case GameState.Paused:
                CheckPauseAndResume();
                break;

            case GameState.GameOver:
                if (!isGameOver)
                {
                    isGameOver = true;
                    Time.timeScale = 0f;
                    DisplayResult();
                }
                break;

            default:
                break;
        }    
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }

    public void PauseGame()
    {
        if (currentState != GameState.Paused)
        {
            previousState = currentState;
            ChangeState(GameState.Paused);
            currentState = GameState.Paused;
            pauseScreen.SetActive(true);
            Time.timeScale = 0f;
        }     
    }

    public void ResumeGame()
    {
        if(currentState == GameState.Paused)
        {
            currentState = previousState;
            pauseScreen.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    void CheckPauseAndResume()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(currentState == GameState.Paused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void DisableScreen()
    {
        pauseScreen.SetActive(false);
        resultsScreen.SetActive(false);
    }

    public void GameOver()
    {
        ChangeState(GameState.GameOver);
    }

    void DisplayResult()
    {
        resultsScreen.SetActive(true);
    }

    public void AssignChosenCharacterUI(CharacterScriptableObject chosenCharacter)
    {
        chosenCharacterImage.sprite = chosenCharacter.Icon;
        chosenCharacterName.text = chosenCharacter.Name; 
    }

    public void AssignLevelReachedUI(int levelReachedData)
    {
        levelReachedDisplay.text = levelReachedData.ToString();
    }

    public void AssignchosenWeaponAndPassiveItemUI(List<Image> chosenWeaponData, List<Image> chosenPassiveItemData)
    {
        if (chosenWeaponData.Count != chosenWeaponUI.Count || chosenPassiveItemData.Count != chosenPassiveItemUI.Count)
        {
            return;
        }

        for (int i = 0; i < chosenWeaponUI.Count; i++)
        {
            if (chosenWeaponData[i].sprite)
            {
                chosenWeaponUI[i].enabled = true;
                chosenWeaponUI[i].sprite = chosenWeaponData[i].sprite;
            }
            else
            {
                chosenWeaponUI[i].enabled = false;
            }
        }

        for (int i = 0; i < chosenPassiveItemUI.Count; i++)
        {
            if (chosenPassiveItemData[i].sprite)
            {
                chosenPassiveItemUI[i].enabled = true;
                chosenPassiveItemUI[i].sprite = chosenPassiveItemData[i].sprite;
            }
            else
            {
                chosenPassiveItemUI[i].enabled = false;
            }
        }
    }

    void UpdateStopwatch()
    {
        stopWatchTime += Time.deltaTime;

        UpdateStopwatchDisplay();

        if (stopWatchTime >= timeLimit)
        {
            GameOver();
        }
    }

    void UpdateStopwatchDisplay()
    {
        int minutes = Mathf.FloorToInt(stopWatchTime / 60);
        int seconds = Mathf.FloorToInt(stopWatchTime % 60);

        stopwatchDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
