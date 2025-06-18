using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 8f;

    private Vector2 moveInput = Vector2.zero;
    private bool gameStarted = false;

    public void SetMoveInput(Vector2 input)
    {
        moveInput = input;
    }

    public void OnCountdownFinished()
    {
        gameStarted = true;
    }

    void Update()
    {
        if (!gameStarted) return;

        Vector3 movement = new Vector3(moveInput.x, moveInput.y, 0f);
        transform.position += movement * moveSpeed * Time.deltaTime;
    }
}