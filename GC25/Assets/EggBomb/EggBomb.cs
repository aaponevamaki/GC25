using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EggBomb : MonoBehaviour
{
    [Header("FX")]
    public GameObject explosionEffectPrefab;

    [Header("Bomb Stats")]
    public BombStats stats;
    public BombStats upgradedBombStats;

    [Header("Layer Masks")]
    public LayerMask obstacleMask;

    void Start()
    {
        Invoke(nameof(Explode), stats.fuseTime);
        AudioManager.Instance.PlaySFXClip("BombDrop");
    }

    void Explode()
    {
        if (stats == null) return;

        AudioManager.Instance.PlaySFXClip("BombExplosion");

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
            Collider2D objCollider = obj.GetComponent<Collider2D>();
            if (objCollider != null)
            {
                Vector2 targetPoint = objCollider.ClosestPoint(transform.position);
                RaycastHit2D hit = Physics2D.Linecast(transform.position, targetPoint, obstacleMask);
                Debug.DrawLine(transform.position, targetPoint, Color.yellow, 1f);

                if (hit.collider != null)
                {
                    continue;
                }
            }

            HealthManager health = obj.GetComponent<HealthManager>();
            if (health != null)
            {
                health.Damage(stats.damage);
            }
        }

        // Trigger camera shake if upgraded
        if (CameraShake.Instance != null && stats == upgradedBombStats)
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