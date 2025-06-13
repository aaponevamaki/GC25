using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private int _health = 4;

    public void Heal(int healAmount)
    {
        _health += healAmount;
    }

    public void Damage(int damageAmount)
    {
        _health -= damageAmount;

        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
