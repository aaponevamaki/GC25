using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedPowerUp : MonoBehaviour
{
    public float speedMultiplier = 1.5f;
    public float duration = 5f;
    public Slider speedSlider;
    public Color speedColor = new Color(1f, 0.98f, 0f);


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement movement = other.GetComponent<PlayerMovement>();
            if (movement != null)
            {
                movement.ApplySpeedBoost(speedMultiplier, duration, speedSlider, speedColor);
            }

            Destroy(gameObject);
        }
    }
}