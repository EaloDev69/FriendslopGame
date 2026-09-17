using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerConfigurationManager : MonoBehaviour
{
    List<PlayerConfigurations> players;

    [SerializeField] private int JugadoresMax = 1;

    public static PlayerConfigurationManager Instance {get; private set;}

    public event Action OnPlayerClassChanged;

    public event Action OnAllPlayersReady;

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

    public bool IsClassTaken(GameObject prefabClass, int excludingPlayerIndex = -1)
    {
        return players.Any(p =>
            p.PlayerIndex != excludingPlayerIndex &&
            p.ClassPrefab == prefabClass);
    }

    public bool SetPlayerClass(int index, GameObject prefabClass, ClassType classType)
    {
        var config = players.FirstOrDefault(p => p.PlayerIndex == index);
        if (config == null) return false;

        if (IsClassTaken(prefabClass, index)) return false;

        config.ClassPrefab = prefabClass;
        config.ClassType = classType;
        OnPlayerClassChanged?.Invoke();
        return true;
    }

    public bool UnsetPlayerClass(int index)
    {
        var config = players.FirstOrDefault(p => p.PlayerIndex == index);
        if (config == null) return false;

        config.ClassPrefab = null;
        config.IsReady = false;
        OnPlayerClassChanged?.Invoke();
        return true;
    }

    public void ReadyPlayer(int index)
    {
        var config = players.FirstOrDefault(p => p.PlayerIndex == index);
        if (config == null) return;

        config.IsReady = true;
        if(players.Count == JugadoresMax && players.All(p => p.IsReady == true))
        {
            OnAllPlayersReady?.Invoke();
        }
    }

    public void HandlePlayerJoin(PlayerInput pi)
    {
        Debug.Log("Se unio Jugador " + pi.playerIndex + 1);
        pi.transform.SetParent(transform);
        if(!players.Any(p => p.PlayerIndex == pi.playerIndex))
        {
            players.Add(new PlayerConfigurations(pi));
        }
    }

    public void ResetAll()
    {
        foreach (var pc in players)
        {
            if (pc.Input != null)
            {
                Destroy(pc.Input.gameObject);
            }
        }

        players.Clear();
        Instance = null;
        Destroy(gameObject);
    }
}

public enum ClassType
{
    Barrendero,
    Trapeador,
    Secador,
    Organizador
}

public class PlayerConfigurations
{
    public PlayerInput Input{get; set;}
    public int PlayerIndex {get; set;}
    public bool IsReady {get; set;}
    public GameObject ClassPrefab {get; set;}
    public ClassType ClassType {get; set;}

    public PlayerConfigurations(PlayerInput pi)
    {
        PlayerIndex = pi.playerIndex;
        Input = pi;
    }
}