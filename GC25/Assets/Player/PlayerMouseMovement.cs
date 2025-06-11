using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMouseMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f;

    private Controls controls;
    private bool hasStartedMoving = false;
    private bool initialMousePosSet = false;
    private bool gameStarted = false;
    private Vector2 initialMousePos;
    private const float movementThreshold = 5f;

    void Awake()
    {
        controls = new Controls();
    }

    void OnEnable()
    {
        controls.Player.Enable();
    }

    void OnDisable()
    {
        controls.Player.Disable();
    }

    public void OnCountdownFinished()
    {
        gameStarted = true;
        StartCoroutine(DelayInitialMouseRead());
    }

    private IEnumerator DelayInitialMouseRead()
    {
        yield return null;
        if (Mouse.current != null)
        {
            initialMousePos = Mouse.current.position.ReadValue();
            initialMousePosSet = true;
        }
    }

    void Update()
    {
        if (!gameStarted || !initialMousePosSet || Mouse.current == null)
            return;

        Vector2 currentMousePos = Mouse.current.position.ReadValue();

        if (!hasStartedMoving && Vector2.Distance(currentMousePos, initialMousePos) > movementThreshold)
        {
            hasStartedMoving = true;
        }

        if (hasStartedMoving)
        {
            transform.position += transform.up * moveSpeed * Time.deltaTime;
        }

        if (controls.Player.Bomb.triggered)
        {
            PlaceBomb();
        }

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(currentMousePos);
        mouseWorldPos.z = 0f;

        Vector3 direction = (mouseWorldPos - transform.position).normalized;

        if (direction.sqrMagnitude > 0.001f)
        {
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle - 90f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void PlaceBomb()
    {
        Debug.Log("Bomb placed!");
        // TODO: Add bomb prefab
        // Call EggBomb script
    }
}