using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public enum Nivel
{
    Menu = 0,
    NivelUno = 1,
    NivelDos = 2,
    Creditos = 3
}

public class LevelExitZone : MonoBehaviour
{
    [SerializeField] private float tiempoCuentaRegresiva = 5f;
    [SerializeField] private Collider zonaCollider;

    private readonly HashSet<int> jugadoresDentro = new HashSet<int>();
    private Coroutine cuentaRegresivaCoroutine;
    private bool activa = false;

    void Start()
    {
        if (zonaCollider != null) zonaCollider.enabled = false;

        if (LevelProgressManager.Instance != null)
        {
            LevelProgressManager.Instance.OnNivelCompletado += Activar;
            Debug.Log("LevelExitZone suscrito a LevelProgressManager.");
        }
        else
        {
            Debug.LogWarning("LevelProgressManager.Instance es null al iniciar LevelExitZone.");
        }
    }

    void OnDestroy()
    {
        if (LevelProgressManager.Instance != null)
            LevelProgressManager.Instance.OnNivelCompletado -= Activar;
    }

    void Activar()
    {
        activa = true;
        if (zonaCollider != null) zonaCollider.enabled = true;
        Debug.Log("Zona de salida habilitada.");
    }

    void OnTriggerEnter(Collider other)
    {
        if (!activa) return;

        var pi = other.GetComponentInParent<PlayerInput>();
        if (pi == null) return;

        jugadoresDentro.Add(pi.playerIndex);
        ChequearJugadores();
    }

    void OnTriggerExit(Collider other)
    {
        var pi = other.GetComponentInParent<PlayerInput>();
        if (pi == null) return;

        jugadoresDentro.Remove(pi.playerIndex);

        if (cuentaRegresivaCoroutine != null)
        {
            StopCoroutine(cuentaRegresivaCoroutine);
            cuentaRegresivaCoroutine = null;
            Debug.Log("Cuenta regresiva cancelada, un jugador salio de la zona.");
        }
    }

    void ChequearJugadores()
    {
        int totalJugadores = PlayerConfigurationManager.Instance.GetPlayerConfigs().Count;
        if (jugadoresDentro.Count >= totalJugadores && cuentaRegresivaCoroutine == null)
        {
            cuentaRegresivaCoroutine = StartCoroutine(CuentaRegresiva());
        }
    }

    IEnumerator CuentaRegresiva()
    {
        Debug.Log("Cuenta regresiva iniciada.");
        yield return new WaitForSeconds(tiempoCuentaRegresiva);

        Nivel actual = (Nivel)SceneManager.GetActiveScene().buildIndex;
        Nivel otroNivel = (actual == Nivel.NivelUno) ? Nivel.NivelDos : Nivel.NivelUno;

        EscenarioManager.Instance.CargarNivel(otroNivel);
    }
}