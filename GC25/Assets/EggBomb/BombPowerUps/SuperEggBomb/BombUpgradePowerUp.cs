using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombUpgradePowerUp : MonoBehaviour
{
    public BombStats upgradedStats;
    public float duration = 5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerBombController placer = other.GetComponent<PlayerBombController>();
        if (placer != null)
        {
            placer.ApplyBombUpgrade(upgradedStats, duration);
            Destroy(gameObject);
        }
    }
}