using UnityEngine;

public class InitializeLevel : MonoBehaviour
{
    [Header("Insertar Players")]
    [SerializeField] private Transform[] playerSpawns;

    [Header("Spawners de hazards de esta sala")]
    [SerializeField] private HazardSpawner[] hazardSpawners;

    void Start()
    {
        var playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs();
        for (int i = 0; i < playerConfigs.Count && i < playerSpawns.Length; i++)
        {
            var pc = playerConfigs[i];
            if (pc.Input == null) continue;

            var playerObject = pc.Input.gameObject;
            var playerController = playerObject.GetComponentInChildren<PlayerController>();

            if (playerController != null)
            {
                playerController.Teleport(playerSpawns[i].position, playerSpawns[i].rotation);
            }
            else
            {
                // Fallback: todavía no existe el classInstance (primera vez en el nivel)
                playerObject.transform.position = playerSpawns[i].position;
                playerObject.transform.rotation = playerSpawns[i].rotation;
            }

            playerObject.GetComponent<PlayerInputHandler>().InitializePlayer(pc);
        }

        foreach (var spawner in hazardSpawners)
        {
            if (spawner != null)
            {
                spawner.SpawnHazards();
            }
        }
    }
}