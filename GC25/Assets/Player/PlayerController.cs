using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Controls controls;
    private PlayerMovement movement;
    private PlayerBombController bomb;

    void Awake()
    {
        controls = new Controls();
        movement = GetComponent<PlayerMovement>();
        bomb = GetComponent<PlayerBombController>();

        controls.Player.Move.performed += ctx => movement.SetMoveInput(ctx.ReadValue<Vector2>());
        controls.Player.Move.canceled += ctx => movement.SetMoveInput(Vector2.zero);
        controls.Player.Bomb.performed += ctx => bomb.PlaceBomb();

        LoadPlayerData();
    }

    private void LoadPlayerData()
    {
        GameData gameData = SaveGame.LoadGameData();
        if (gameData == null) return;

        PlayerData playerData = gameData.playerData;
        if (playerData == null) return;

        transform.position = playerData.position;
        GetComponent<HealthManager>().SetHealth(playerData.health);
    }

    void OnEnable() => controls.Player.Enable();
    void OnDisable() => controls.Player.Disable();
}