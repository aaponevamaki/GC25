using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombUpgradePowerUp : MonoBehaviour
{
    public BombStats upgradedStats;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerBombController placer = other.GetComponent<PlayerBombController>();
        if (placer != null)
        {
            placer.bombStats = upgradedStats;

            Debug.Log("Bomb upgraded!");

            Destroy(gameObject);
        }
    }
}