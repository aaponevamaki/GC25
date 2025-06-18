using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public enum ObjectType
    {
        Player,
        Enemy,
    }

    public ObjectType objectType;
    [SerializeField] private int _health = 4;
    private int _currentHealth;

    private void OnEnable()
    {
        _currentHealth = _health;
    }

    public void Heal(int healAmount)
    {
        _currentHealth += healAmount;
    }

    public void Damage(int damageAmount)
    {
        _currentHealth -= damageAmount;

        if (_currentHealth <= 0)
        {
            switch (objectType)
            {
                case ObjectType.Player:
                    // Player died.
                    break;
                case ObjectType.Enemy:
                    gameObject.SetActive(false);
                    break;
            }
        }
    }
}
