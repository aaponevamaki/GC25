using UnityEngine;

public class RapidFirePowerUp : MonoBehaviour
{
    public BombStats rapidFireStats;
    public float duration = 3f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerBombController placer = other.GetComponent<PlayerBombController>();
        if (placer != null)
        {
            placer.ApplyBombUpgrade(rapidFireStats, duration);
            Destroy(gameObject);
        }
    }
}