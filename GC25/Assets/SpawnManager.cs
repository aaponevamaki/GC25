using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [Header("Enemy spawn settings")]
    [SerializeField] private GameObject[] _enemyPrefabs;
    [SerializeField] private int _poolSize = 20;
    [SerializeField] private float _enemySpawnRate = 5f;
    [SerializeField] private float _minSpawnDistance = 5f;

    [Header("Powerups")]
    [SerializeField] private GameObject[] _powerupPrefabs;

    private GridManager _gridManager;
    private GameObject _player;

    private List<GameObject> _enemyPool = new();

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }

        _gridManager = Pathfinding.Instance.GetComponent<GridManager>();
        _player = GameObject.FindGameObjectWithTag("Player");
        InitializeEnemyPool();
        LoadEnemyData();
    }

    private void InitializeEnemyPool()
    {
        GameObject enemyPool = new("EnemyPool");

        for (int i = 0; i < _poolSize; i++)
        {
            int prefabIndex = i % _enemyPrefabs.Length;
            GameObject enemy = Instantiate(_enemyPrefabs[prefabIndex]);
            enemy.transform.SetParent(enemyPool.transform);
            enemy.SetActive(false);
            enemy.GetComponent<Enemy>().id = i;
            _enemyPool.Add(enemy);
        }
    }

    private void LoadEnemyData()
    {
        GameData gameData = SaveGame.LoadGameData();
        if (gameData == null) return;

        List<EnemyData> enemyDataList = gameData.enemyDataList;
        if (enemyDataList == null) return;

        foreach (EnemyData enemy in enemyDataList)
        {
            _enemyPool[enemy.id].SetActive(true);
            _enemyPool[enemy.id].transform.position = enemy.position;
            _enemyPool[enemy.id].GetComponent<HealthManager>().SetHealth(enemy.health);
        }
    }

    public void StartSpawningEnemies()
    {
        InvokeRepeating(nameof(SpawnEnemyFromPool), 0f, _enemySpawnRate);
    }

    private void SpawnEnemyFromPool()
    {
        GameObject enemy = GetInactiveEnemyFromPool();

        if (enemy != null)
        {
            Vector3 spawnPos = GetRandomWorldPosition();
            enemy.transform.position = spawnPos;
            enemy.SetActive(true);
        }
    }

    private GameObject GetInactiveEnemyFromPool()
    {
        List<GameObject> inactiveEnemies = new();

        foreach (GameObject enemy in _enemyPool)
        {
            if (!enemy.activeInHierarchy) inactiveEnemies.Add(enemy);
        }

        if (inactiveEnemies.Count == 0) return null;

        int random = Random.Range(0, inactiveEnemies.Count);
        return inactiveEnemies[random];
    }

    private Vector3 GetRandomWorldPosition()
    {
        Vector3 randomPos;
        float distance;

        do
        {
            randomPos = new(Random.Range(-(_gridManager.gridWorldSize.x / 2), _gridManager.gridWorldSize.x / 2), Random.Range(-(_gridManager.gridWorldSize.y / 2), _gridManager.gridWorldSize.y / 2), 0f);
            distance = Vector3.Distance(randomPos, _player.transform.position);
        }
        while (!_gridManager.NodeFromWorldPoint(randomPos).walkable || distance < _minSpawnDistance);

        return randomPos;
    }

    public List<GameObject> GetActiveEnemies()
    {
        List<GameObject> enemies = new();

        foreach (GameObject enemy in _enemyPool)
        {
            if (enemy.activeInHierarchy) enemies.Add(enemy);
        }

        return enemies;
    }

    public void SpawnPowerup(Vector2 spawnPos)
    {
        float possibility = 0.33f;
        float randomFloat = Random.Range(0f, 1f);
        if (randomFloat > possibility) return;

        int randomInt = Random.Range(0, _powerupPrefabs.Length);
        GameObject powerup = _powerupPrefabs[randomInt];

        Instantiate(powerup, spawnPos, Quaternion.identity);
    }
}
