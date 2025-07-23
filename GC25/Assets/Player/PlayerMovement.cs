using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 8f;

    private Vector2 moveInput = Vector2.zero;
    private bool gameStarted = false;

    private Coroutine speedBoostCoroutine;

    public void SetMoveInput(Vector2 input)
    {
        moveInput = input;
    }

    public void OnCountdownFinished()
    {
        gameStarted = true;
    }

    void Update()
    {
        if (!gameStarted) return;

        Vector3 movement = new Vector3(moveInput.x, moveInput.y, 0f);
        transform.position += movement * moveSpeed * Time.deltaTime;
    }

    public void ApplySpeedBoost(float multiplier, float duration, Slider slider, Color sliderColor)
    {
        if (speedBoostCoroutine != null)
            StopCoroutine(speedBoostCoroutine);

        speedBoostCoroutine = StartCoroutine(SpeedBoostCoroutine(multiplier, duration, slider, sliderColor));
    }

    private IEnumerator SpeedBoostCoroutine(float multiplier, float duration, Slider slider, Color sliderColor)
    {
        float originalSpeed = moveSpeed;
        moveSpeed *= multiplier;

        if (slider != null)
        {
            slider.gameObject.SetActive(true);
            slider.fillRect.GetComponent<Image>().color = sliderColor;
            slider.value = 1f;
        }

        float timeLeft = duration;
        while (timeLeft > 0f)
        {
            timeLeft -= Time.deltaTime;

            if (slider != null)
                slider.value = timeLeft / duration;

            yield return null;
        }

        moveSpeed = originalSpeed;

        if (slider != null)
            slider.gameObject.SetActive(false);
    }
}
