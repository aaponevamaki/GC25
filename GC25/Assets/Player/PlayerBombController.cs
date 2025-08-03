using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBombController : MonoBehaviour
{
    public GameObject bombPrefab;
    public BombStats bombStats;
    public BombStats defaultBombStats;

    public BombStats upgradedBombStats;
    public BombStats rapidBombStats;


    public Color rapidColor = new Color(1f, 0f, 0.63f);
    public Color nukeColor = new Color(1f, 0.16f, 0f);

    public Slider powerUpSlider;

    private float lastBombTime = -Mathf.Infinity;

    private Coroutine upgradeCoroutine;

    public void PlaceBomb(bool canDrop)
    {
        if (Time.time >= lastBombTime + bombStats.cooldown && canDrop)
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
            powerUpSlider.fillRect.GetComponent<Image>().color =
                (bombStats == upgradedBombStats) ? nukeColor : rapidColor;

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