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

    public void LoadMainMenu()
    {
        if (!mainMenuPanel.activeSelf)
        {
            mainMenuPanel.SetActive(true);
            if (Timer.Instance != null) Timer.Instance.StopTimer();
            // TODO: Work with game saving/loading
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}