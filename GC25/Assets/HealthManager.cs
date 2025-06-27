using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public enum ObjectType
    {
        Player,
        Enemy,
    }

    public ObjectType objectType;
    [SerializeField] private int _maxHealth = 4;
    private int _currentHealth;

    private void Awake()
    {
        _currentHealth = _maxHealth;
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
                    SetHealth(_maxHealth);
                    break;
            }
        }
    }

    public int GetHealth() => _currentHealth;
    public void SetHealth(int health) => _currentHealth = health;
}
