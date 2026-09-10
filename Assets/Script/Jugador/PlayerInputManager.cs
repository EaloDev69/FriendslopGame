using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform[] spawnPoints;

    private bool wasdJoined = false;
    private readonly HashSet<Gamepad> joinedGamepads = new HashSet<Gamepad>();
    private int nextSpawnIndex = 0;

    void Update()
    {
        if (Keyboard.current != null &&
            !wasdJoined &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            SpawnPlayer(playerPrefab, "WASD", Keyboard.current);
            wasdJoined = true;
        }

        foreach (var gamepad in Gamepad.all)
        {
            if (gamepad.buttonSouth.wasPressedThisFrame && !joinedGamepads.Contains(gamepad))
            {
                SpawnPlayer(playerPrefab, "Gamepad", gamepad);
                joinedGamepads.Add(gamepad);
            }
        }
    }

    void SpawnPlayer(GameObject prefab, string controlScheme, InputDevice device)
    {
        var player = PlayerInput.Instantiate(prefab, controlScheme: controlScheme, pairWithDevice: device);

        if (spawnPoints.Length > 0)
        {
            player.transform.position = spawnPoints[nextSpawnIndex % spawnPoints.Length].position;
            nextSpawnIndex++;
        }
    }

}
