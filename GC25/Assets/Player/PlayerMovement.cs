using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f;

    private Vector2 moveInput = Vector2.zero;
    private Controls controls;
    private bool hasStartedMoving = false;
    private bool gameStarted = false;

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
    }

    void Update()
    {
        if (!gameStarted)
            return;

        moveInput = controls.Player.Move.ReadValue<Vector2>();

        if (!hasStartedMoving && moveInput.sqrMagnitude > 0.01f)
        {
            hasStartedMoving = true;
        }

        if (!hasStartedMoving)
            return;

        if (controls.Player.Bomb.triggered)
        {
            PlaceBomb();
        }

        float horizontal = moveInput.x;

        if (Mathf.Abs(horizontal) > 0.1f)
        {
            float rotationAmount = -horizontal * rotationSpeed * Time.deltaTime;
            transform.Rotate(0f, 0f, rotationAmount);
        }

        transform.position += transform.up * moveSpeed * Time.deltaTime;
    }

    private void PlaceBomb()
    {
        Debug.Log("Bomb placed!");
        // TODO: Add bomb prefab
        // Call EggBomb script
    }
}