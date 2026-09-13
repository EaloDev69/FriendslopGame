using System;
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

    //Se dispara cada vez que un jugador elige (o le rechazan) una clase,
    //para que los menus de los demas jugadores refresquen que esta disponible
    public event Action OnPlayerClassChanged;

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

    //true si algun OTRO jugador (distinto a excludingPlayerIndex) ya tiene esta clase
    public bool IsClassTaken(GameObject prefabClass, int excludingPlayerIndex = -1)
    {
        return players.Any(p =>
            p.PlayerIndex != excludingPlayerIndex &&
            p.ClassPrefab == prefabClass);
    }

    //otorgamos la clase del jugador segun la que escoja
    //devuelve false si la clase ya estaba tomada por otro jugador
    public bool SetPlayerClass(int index, GameObject prefabClass)
    {
        var config = players.FirstOrDefault(p => p.PlayerIndex == index);
        if (config == null) return false;

        if (IsClassTaken(prefabClass, index))
        {
            return false;
        }

        config.ClassPrefab = prefabClass;
        OnPlayerClassChanged?.Invoke();
        return true;
    }

    //Confirmado para iniciar
    public void ReadyPlayer(int index)
    {
        var config = players.FirstOrDefault(p => p.PlayerIndex == index);
        if (config == null) return;

        config.IsReady = true;
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
            players.Add(new PlayerConfigurations(pi));
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