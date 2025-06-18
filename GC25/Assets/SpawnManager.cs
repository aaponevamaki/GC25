using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    [Header("Enemy spawn settings")]
    [SerializeField] private GameObject[] _enemyPrefabs;
    [SerializeField] private int _poolSize = 20;
    [SerializeField] private float _enemySpawnRate = 5f;
    [SerializeField] private float _minSpawnDistance = 5f;

    private GridManager _gridManager;
    private GameObject _player;

    private List<GameObject> _enemyPool = new();

    private void Start()
    {
        _gridManager = Pathfinding.Instance.GetComponent<GridManager>();
        _player = GameObject.FindGameObjectWithTag("Player");
        InitializeEnemyPool();
        StartSpawningEnemies();
    }

    private void InitializeEnemyPool()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            int prefabIndex = i % _enemyPrefabs.Length;
            GameObject enemy = Instantiate(_enemyPrefabs[prefabIndex]);
            enemy.SetActive(false);
            _enemyPool.Add(enemy);
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
}
