using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField] private GameObject _healthBar;
    [SerializeField] private Sprite _healthIcon;
    private List<GameObject> _healthIcons;

    private void Awake()
    {
        _currentHealth = _maxHealth;
        GenerateHealthIcons();
    }

    public void Heal(int healAmount)
    {
        _currentHealth += healAmount;
        DisplayHealth();
    }

    public void Damage(int damageAmount)
    {
        _currentHealth -= damageAmount;

        if (_currentHealth <= 0)
        {
            switch (objectType)
            {
                case ObjectType.Player:
                    {
                        GameOver gameOver = FindObjectOfType<GameOver>();
                        if (gameOver != null)
                        {
                            AudioManager.Instance.PlaySFXClip("PlayerDeath");
                            gameOver.OnPlayerDied();
                            Destroy(gameObject);
                        }
                        break;
                    }
                case ObjectType.Enemy:
                    {
                        gameObject.SetActive(false);
                        SetHealth(_maxHealth);
                        SpawnManager.Instance.SpawnPowerup(transform.position);
                        break;
                    }
            }
        }
        else
        {
            switch (objectType)
            {
                case ObjectType.Player:
                    {
                        AudioManager.Instance.PlaySFXClip("PlayerDamage");
                        break;
                    }
                case ObjectType.Enemy:
                    {
                        break;
                    }
            }
        }

        DisplayHealth();
    }

    public int GetHealth() => _currentHealth;
    public void SetHealth(int health)
    {
        _currentHealth = health;
        DisplayHealth();
    }

    private void GenerateHealthIcons()
    {
        if (_healthBar != null)
        {
            _healthIcons = new();

            for (int i = 0; i < _maxHealth; i++)
            {
                GameObject healthIcon = new("HealthIcon");
                healthIcon.transform.SetParent(_healthBar.transform);
                healthIcon.AddComponent<Image>();
                healthIcon.AddComponent<LayoutElement>();

                Image imageComponent = healthIcon.GetComponent<Image>();
                imageComponent.sprite = _healthIcon;
                imageComponent.preserveAspect = true;

                LayoutElement layoutElemenet = healthIcon.GetComponent<LayoutElement>();
                layoutElemenet.preferredHeight = 0.25f;
                layoutElemenet.preferredWidth = 0.25f;

                _healthIcons.Add(healthIcon);
            }
        }
    }

    private void DisplayHealth()
    {
        if (_healthBar != null)
        {
            for (int i = 0; i < _maxHealth; i++)
            {
                if (i < _currentHealth)
                    _healthIcons[i].SetActive(true);
                else
                    _healthIcons[i].SetActive(false);
            }
        }
    }
}