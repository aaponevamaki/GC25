using UnityEngine;
using System.Collections.Generic;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Enemy : MonoBehaviour
{
    public enum EnemyType
    {
        Basic,
        Fast,
    }

    public EnemyType enemyType;

    private float _speed;
    public float _basicSpeed = 5f;
    public float _fastSpeed = 10f;

    private GameObject _target;

    private readonly float _nextWaypointDistance = 0.05f;

    private List<Node> _path;
    private int _currentWaypoint = 0;
    private readonly float _updateInterval = 0.5f;

    private int _damage = 1;
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
            HitPlayer();
        }
    }

    private void HitPlayer()
    {
        _hitCooldown = true;
        // TODO: Damage player
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

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
#endregion