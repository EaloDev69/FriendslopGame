using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerConfigurationManager : MonoBehaviour
{
    //Listado de jugadores
    List<PlayerConfigurations> players;
    
    //Limitamos los jugadores a los necesarios para seguir (4 por regla)
    //la dejo como serialized para modificar en unit para debugueo :3
    [SerializeField] private int JugadoresMax = 1;

    //instancia del singlelton
    public static PlayerConfigurationManager Instance {get; private set;}
    
    //Confirmante invocable desde el juego para jugar el segundo nivel cuando
    //se cumpla la condicion de terminar el nivel 1
    public bool NivelUnoPassed {get; private set;}

    //iniciar singlelton
    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            players = new List<PlayerConfigurations>();
        }
    }

    public List<PlayerConfigurations> GetPlayerConfigs()
    {
        return players;
    }

    //otorgamos la clase del jugador segun la que escoja
    public void SetPlayerClass(int index, GameObject prefabClass)
    {

        
        players[index].ClassPrefab = prefabClass;
    }

    //Confirmado para iniciar
    public void ReadyPlayer(int index)
    {
        players[index].IsReady = true;
        if(players.Count == JugadoresMax && players.All(p => p.IsReady == true))
        {
            EscenarioManager.Instance.Niveluno();
        }
    }

    //Se añade al jugador que ingresa al listado de jugadores
    public void HandlePlayerJoin(PlayerInput pi)
    {
        Debug.Log("Se unio Jugador " + pi.playerIndex + 1);
        pi.transform.SetParent(transform);
        if(!players.Any(p => p.PlayerIndex == pi.playerIndex))
        {
            players.Add(new PlayerConfigurations(pi))            ;
        }
    }
}

public class PlayerConfigurations
{
    //datos  elementos necesarios para que el jugador inicie correctamente
    public PlayerInput Input{get; set;}
    public int PlayerIndex {get; set;}
    public bool IsReady {get; set;}

    public GameObject ClassPrefab {get; set;}

    public PlayerConfigurations(PlayerInput pi)
    {
        PlayerIndex = pi.playerIndex;
        Input = pi;
    }

}