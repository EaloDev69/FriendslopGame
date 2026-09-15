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
        for (int i = 0; i < playerConfigs.Count; i++)
        {
            var pc = playerConfigs[i];

            var playerObject = pc.Input.gameObject;

            playerObject.transform.SetParent(gameObject.transform);
            playerObject.transform.position = playerSpawns[i].position;
            playerObject.transform.rotation = playerSpawns[i].rotation;

            playerObject.GetComponent<PlayerInputHandler>().InitializePlayer(pc);
        }

        // Una vez que los jugadores ya estan ubicados, spawneamos los hazards de la sala
        foreach (var spawner in hazardSpawners)
        {
            if (spawner != null)
            {
                spawner.SpawnHazards();
            }
        }
    }
}