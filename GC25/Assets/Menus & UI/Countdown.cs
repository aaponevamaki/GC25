using System.Collections;
using TMPro;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    public TMP_Text countdownText;
    public float countdownDuration = 3f;

    public PlayerMovement playerMovement;

    public void BeginCountdown()
    {
        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        float timeLeft = countdownDuration;

        while (timeLeft > 0)
        {
            countdownText.text = Mathf.Ceil(timeLeft).ToString();
            AudioManager.Instance.PlaySFXClip("Beep");
            yield return new WaitForSeconds(1f);
            timeLeft--;
        }

        countdownText.text = "GO!";
        AudioManager.Instance.PlaySFXClip("Boop");
        AudioManager.Instance.StartLoop("BackgroundMusic", group: "Music");
        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);

        playerMovement.OnCountdownFinished();

        if (SpawnManager.Instance != null) SpawnManager.Instance.StartSpawningEnemies();
        if (Timer.Instance != null) Timer.Instance.StartTimer();
    }
}