using UnityEngine;

public class InitializeLevel : MonoBehaviour
{
    [Header("Insertar Players y prefabs")]
    [SerializeField] private Transform[] playerSpawns;
    [SerializeField] private GameObject[] playerPrefab;

    void Start()
    {
        var playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray();
        for (int i = 0; i < playerConfigs.Length; i++)
        {
            var player = Instantiate(playerPrefab[i], playerSpawns[i].position, playerSpawns[i].rotation, gameObject.transform);
            player.GetComponent<PlayerInputHandler>().InitializePlayer(playerConfigs[i]);

        }

    }
}
