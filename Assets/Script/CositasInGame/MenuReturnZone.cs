using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuReturnZone : MonoBehaviour
{
    [SerializeField] private float tiempoCuentaRegresiva = 5f;
    [SerializeField] private Collider zonaCollider;
    [SerializeField] private TextMeshPro SalirMenu;

    private readonly HashSet<int> jugadoresDentro = new HashSet<int>();
    private Coroutine cuentaRegresivaCoroutine;
    private bool activa = false;

    void Start()
    {
        if (zonaCollider != null) zonaCollider.enabled = false;

        if (SalirMenu != null) SalirMenu.gameObject.SetActive(false);

        if (LevelProgressManager.Instance != null)
        {
            LevelProgressManager.Instance.OnNivelCompletado += Activar;
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
        SalirMenu.gameObject.SetActive(true);
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
        Debug.Log("Cuenta regresiva iniciada, volviendo al menu.");
        yield return new WaitForSeconds(tiempoCuentaRegresiva);

        EscenarioManager.Instance.VolverAlMenuYReiniciar();
    }
}