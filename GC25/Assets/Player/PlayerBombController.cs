using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBombController : MonoBehaviour
{
    public GameObject bombPrefab;
    public BombStats bombStats;

    private float lastBombTime = -Mathf.Infinity;

    public void PlaceBomb()
    {
        if (Time.time >= lastBombTime + bombStats.cooldown)
        {
            GameObject bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);

            EggBomb bombScript = bomb.GetComponent<EggBomb>();
            bombScript.stats = bombStats;

            lastBombTime = Time.time;

            //Debug.Log($"Bomb cooldown: {bombStats.cooldown}, radius: {bombStats.explosionRadius}, damage: {bombStats.damage}");
        }
        else
        {
            // Bomb on cooldown
            // TODO: Show feedback to player
        }
    }
}