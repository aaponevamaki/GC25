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

        if (input == Vector2.zero)
        {
            GetComponent<Animator>().SetBool("Walking", false);
        }
        else
        {
            GetComponent<Animator>().SetBool("Walking", true);
        }

        FlipCharacter(input.x);
    }

    private void FlipCharacter(float moveX)
    {
        if (moveX > 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        else if (moveX < 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
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
