using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuPanel;
    private Countdown countdownScript;

    public void Awake()
    {
        countdownScript = FindObjectOfType<Countdown>();
    }

    public void PlayGame()
    {
        mainMenuPanel.SetActive(false);
        countdownScript.BeginCountdown();
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }

    public void LoadMainMenu(bool saveProgress = true)
    {
        if (!mainMenuPanel.activeSelf)
        {
            mainMenuPanel.SetActive(true);
            if (Timer.Instance != null) Timer.Instance.StopTimer();
            
            if (saveProgress)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                List<GameObject> enemies = SpawnManager.Instance.GetActiveEnemies();
                float time = Timer.Instance.GetCurrentTime();

                SaveGame.SaveGameData(player, enemies, time);
            }
            else
            {
                SaveGame.ClearGameData();
            }

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}