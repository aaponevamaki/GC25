using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EggBomb : MonoBehaviour
{
    [Header("FX")]
    public GameObject explosionEffectPrefab;

    public BombStats stats;

    void Start()
    {
        Invoke(nameof(Explode), stats.fuseTime);
    }

    void Explode()
    {
        if (stats == null) return;

        // Show test explosion effect
        if (explosionEffectPrefab != null)
        {
            GameObject fx = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            ParticleSystem ps = fx.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                var shape = ps.shape;
                shape.radius = stats.explosionRadius;
            }

            Destroy(fx, 2f);
        }

        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, stats.explosionRadius);

        foreach (var obj in hitObjects)
        {
            HealthManager health = obj.GetComponent<HealthManager>();
            if (health != null)
            {
                health.Damage(stats.damage);
            }
        }

        // Trigger camera shake if upgraded
        // TODO: Change to better check for upgrade. Check if bomb has PowerUpBombStats.asset assigned
        if (CameraShake.Instance != null && stats.explosionRadius > 3f)
        {
            CameraShake.Instance.StartCoroutine(CameraShake.Instance.Shake(0.2f, 0.2f));
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stats != null ? stats.explosionRadius : 2f);
    }
}