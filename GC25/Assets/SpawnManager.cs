using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Enemy spawn settings")]
    [SerializeField] private GameObject[] _enemies;
    [SerializeField] private float _enemySpawnRate = 5f;

    private GridManager _gridManager;

    private GameObject _player;
    [SerializeField] private float _minSpawnDistance = 5f;

    private void Start()
    {
        _gridManager = Pathfinding.Instance.GetComponent<GridManager>();
        _player = GameObject.FindGameObjectWithTag("Player");
        StartSpawningEnemies();
    }

    public void StartSpawningEnemies()
    {
        InvokeRepeating(nameof(SpawnEnemies), 0f, _enemySpawnRate);
    }

    private void SpawnEnemies()
    {
        int i = Random.Range(0, _enemies.Length);
        Vector3 spawnPos = GetRandomWorldPosition();

        Instantiate(_enemies[i], spawnPos, Quaternion.identity);
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
