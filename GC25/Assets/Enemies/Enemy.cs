using UnityEngine;
using System.Collections.Generic;
using System.Collections;
#region
#if UNITY_EDITOR
using UnityEditor;
#endif
#endregion

public class Enemy : MonoBehaviour
{
    public enum EnemyType
    {
        Basic,
        Fast,
    }

    public EnemyType enemyType;

    private float _speed;
    [SerializeField] private float _basicSpeed = 5f;
    [SerializeField] private float _fastSpeed = 10f;

    private GameObject _target;

    private readonly float _nextWaypointDistance = 0.05f;

    private List<Node> _path;
    private int _currentWaypoint = 0;
    private float _waitTime = 1.5f;
    private readonly float _updateInterval = 0.5f;

    [SerializeField] private int _damage = 1;
    private float _hitInterval = 2f;
    private bool _hitCooldown = false;

    private void OnEnable()
    {
        switch (enemyType)
        {
            case EnemyType.Basic:
                _speed = _basicSpeed;
                break;
            case EnemyType.Fast:
                _speed = _fastSpeed;
                break;
        }

        StartCoroutine(WaitBeforeMoving());
    }

    private IEnumerator WaitBeforeMoving()
    {
        yield return new WaitForSeconds(_waitTime);
        SetTarget(GameObject.FindGameObjectWithTag("Player"));
        InvokeRepeating(nameof(UpdatePath), 0f, _updateInterval);
    }

    private void SetTarget(GameObject targetObject = null)
    {
        _target = targetObject;
    }

    private void UpdatePath()
    {
        if (_target == null) return;
        _path = Pathfinding.Instance.FindPath(transform.position, _target.transform.position);
        _currentWaypoint = 0;
    }

    private void Update()
    {
        if (_path == null || _path.Count == 0) return;

        Vector3 waypointPos = _path[_currentWaypoint].worldPosition;
        Vector3 dir = (waypointPos - transform.position).normalized;
        transform.position += _speed * Time.deltaTime * dir;

        if (Vector3.Distance(transform.position, waypointPos) < _nextWaypointDistance)
        {
            _currentWaypoint++;
            if (_currentWaypoint >= _path.Count)
            {
                _path = null;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !_hitCooldown)
        {
            HitPlayer(collision.gameObject);
        }
    }

    private void HitPlayer(GameObject player)
    {
        _hitCooldown = true;

        if (player.TryGetComponent(out HealthManager healthManager))
            healthManager.Damage(_damage);

        StartCoroutine(HitCooldown());
    }

    private IEnumerator HitCooldown()
    {
        yield return new WaitForSeconds(_hitInterval);
        _hitCooldown = false;
    }
}
#region Editor script
#if UNITY_EDITOR
[CustomEditor(typeof(Enemy))]
public class EnemyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        Enemy enemy = (Enemy)target;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("enemyType"));

        switch (enemy.enemyType)
        {
            case Enemy.EnemyType.Basic:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_basicSpeed"));
                break;
            case Enemy.EnemyType.Fast:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_fastSpeed"));
                break;
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("_damage"));

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
#endregion