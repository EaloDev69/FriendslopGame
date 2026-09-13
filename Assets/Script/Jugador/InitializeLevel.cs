using UnityEngine;

public class InitializeLevel : MonoBehaviour
{
    [Header("Insertar Players")]
    [SerializeField] private Transform[] playerSpawns;

    void Start()
    {
        var playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs();
        for (int i = 0; i < playerConfigs.Count; i++)
        {
            var pc = playerConfigs[i];

            // Usamos el objeto que YA existe y persistio desde el menu de seleccion
            // (el que tiene el PlayerInput real, ya emparejado con su dispositivo),
            // en vez de instanciar uno nuevo.
            var playerObject = pc.Input.gameObject;

            playerObject.transform.SetParent(gameObject.transform);
            playerObject.transform.position = playerSpawns[i].position;
            playerObject.transform.rotation = playerSpawns[i].rotation;

            playerObject.GetComponent<PlayerInputHandler>().InitializePlayer(pc);
        }
    }
}