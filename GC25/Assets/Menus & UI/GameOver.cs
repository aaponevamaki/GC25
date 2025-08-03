using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameOver : MonoBehaviour
{
    public TMP_Text gameOverText;
    public float gameOverDelay = 3f;

    public HealthManager playerHealth;
    private bool gameIsOver = false;

    private MainMenu mainMenuScript;

    void Awake()
    {
        mainMenuScript = GetComponent<MainMenu>();
    }

    public void OnPlayerDied()
    {
        if (gameIsOver)
            return;

        gameIsOver = true;
        StartCoroutine(GameOverSequence());
    }

    private IEnumerator GameOverSequence()
    {
        gameOverText.gameObject.SetActive(true);
        gameOverText.text = "GAME OVER";
        AudioManager.Instance.StopLoop("BackgroundMusic");
        AudioManager.Instance.PlaySFXClip("GameOver");
        yield return new WaitForSeconds(gameOverDelay);
        mainMenuScript.LoadMainMenu(saveProgress: false);
    }
}