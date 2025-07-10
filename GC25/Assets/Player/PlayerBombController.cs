using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBombController : MonoBehaviour
{
    public GameObject bombPrefab;
    public BombStats bombStats;
    public BombStats defaultBombStats;

    public Slider powerUpSlider;

    private float lastBombTime = -Mathf.Infinity;

    private Coroutine upgradeCoroutine;

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

    public void ApplyBombUpgrade(BombStats upgradedStats, float duration)
    {
        if (upgradeCoroutine != null)
            StopCoroutine(upgradeCoroutine);

        upgradeCoroutine = StartCoroutine(TempBombUpgrade(upgradedStats, duration));
    }

    private IEnumerator TempBombUpgrade(BombStats upgradedStats, float duration)
    {
        bombStats = upgradedStats;

        if (powerUpSlider != null)
        {
            powerUpSlider.gameObject.SetActive(true);
        }

        float timeLeft = duration;
        while (timeLeft > 0f)
        {
            timeLeft -= Time.deltaTime;

            if (powerUpSlider != null)
                powerUpSlider.value = timeLeft / duration;

            yield return null;
        }

        bombStats = defaultBombStats;

        if (powerUpSlider != null)
        {
            powerUpSlider.gameObject.SetActive(false);
        }
    }
}