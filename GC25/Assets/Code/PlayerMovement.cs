using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 720f;

    private Rigidbody rb;
    private Vector3 moveDirection = Vector3.zero;

    private Controls controls;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controls = new Controls();

        // Test gravity
        rb.useGravity = false;
    }

    void OnEnable()
    {
        controls.Player.Enable();
    }

    void OnDisable()
    {
        controls.Player.Disable();
    }

    void FixedUpdate()
    {
        Vector2 moveInput = controls.Player.Move.ReadValue<Vector2>();

        if (moveInput != Vector2.zero)
        {
            moveDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
        }

        Vector3 targetVelocity = moveDirection * moveSpeed;
        targetVelocity.y = rb.velocity.y;

        rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, 0.1f);

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            rb.rotation = Quaternion.RotateTowards(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }
}