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
    public int id;

    private float _speed;
    [SerializeField] private float _basicSpeed = 5f;
    [SerializeField] private float _fastSpeed = 10f;

    private GameObject _target;

    private readonly float _nextWaypointDistance = 0.05f;

    private readonly float _updateInterval = 0.5f;
    private List<Vector3> _smoothedPath = new();
    private int _smoothIndex = 0;

    private readonly float _waitTime = 1.5f;

    [SerializeField] private int _damage = 1;
    private float _hitInterval = 2f;
    private bool _hitCooldown = false;

    public bool _showGizmos = false;

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
        List<Node> path = Pathfinding.Instance.FindPath(transform.position, _target.transform.position);

        if (path != null && path.Count > 0)
        {
            SmoothPath(path);
        }
    }

    private Vector3 Bezier(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);
        return Vector3.Lerp(ab, bc, t);
    }

    private void SmoothPath(List<Node> nodePath)
    {
        _smoothedPath.Clear();

        if (nodePath == null || nodePath.Count == 0) return;

        _smoothedPath.Add(nodePath[0].worldPosition);

        for (int i = 1; i < nodePath.Count - 1; i++)
        {
            Node a = nodePath[i - 1];
            Node b = nodePath[i];
            Node c = nodePath[i + 1];

            Vector2 dir1 = new(b.gridX - a.gridX, b.gridY - a.gridY);
            Vector2 dir2 = new(c.gridX - b.gridX, c.gridY - b.gridY);

            if (dir1 != dir2)
            {
                for (float t = 0f; t <= 1f; t += 0.1f)
                {
                    Vector3 curvePoint = Bezier(a.worldPosition, b.worldPosition, c.worldPosition, t);
                    _smoothedPath.Add(curvePoint);
                }

                i++;
            }
            else
            {
                _smoothedPath.Add(b.worldPosition);
            }
        }

        if (_smoothedPath.Count == 0 || _smoothedPath[^1] != nodePath[^1].worldPosition)
        {
            _smoothedPath.Add(nodePath[^1].worldPosition);
        }

        _smoothIndex = 0;
    }

    private void Update()
    {
        if (_smoothedPath == null || _smoothIndex >= _smoothedPath.Count || !Timer.Instance.IsRunning()) return;

        Vector3 waypointPos = _smoothedPath[_smoothIndex];
        Vector3 dir = (waypointPos - transform.position).normalized;
        transform.position += _speed * Time.deltaTime * dir;

        if (Vector3.Distance(transform.position, waypointPos) < _nextWaypointDistance)
        {
            _smoothIndex++;
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

    private void OnDrawGizmos()
    {
        if (!_showGizmos || _smoothedPath == null) return;

        Gizmos.color = Color.green;
        for (int i = 0; i < _smoothedPath.Count - 1; i++)
        {
            Gizmos.DrawLine(_smoothedPath[i], _smoothedPath[i + 1]);
        }
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

        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
        EditorGUI.EndDisabledGroup();

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

        EditorGUILayout.PropertyField(serializedObject.FindProperty("_showGizmos"));

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
#endregion