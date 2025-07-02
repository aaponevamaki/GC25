using UnityEngine;

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
}
