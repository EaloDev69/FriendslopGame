using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public enum HazardAction
{
    Destruir,
    Recolectar
}

public class Hazard : MonoBehaviour
{
    public ClassType requiredClass;
    public HazardAction accion = HazardAction.Destruir;

    [Header("Jerarquia de limpieza")]
    [Tooltip("Orden en la jerarquia de limpieza. Menor numero = se limpia primero. Ej: Barrendero=0, Trapeador=1, Secador=2, Organizador=3")]
    public int orden;

    [Tooltip("Radio de deteccion para chequear si hay otro hazard de mayor prioridad cerca (o tocandose).")]
    [SerializeField] private float radioBloqueo = 1.5f;

    private void OnTriggerEnter(Collider other)
    {
        var interactor = other.GetComponentInParent<HazardInteractor>();
        if (interactor != null) interactor.RegistrarCercano(this);
    }

    private void OnTriggerExit(Collider other)
    {
        var interactor = other.GetComponentInParent<HazardInteractor>();
        if (interactor != null) interactor.QuitarCercano(this);
    }

    //True si hay otro hazard cercano (o tocandose) con mayor prioridad (orden menor) sin limpiar todavia
    public bool EstaBloqueado()
    {
        Collider[] cercanos = Physics.OverlapSphere(transform.position, radioBloqueo);
        return cercanos.Any(c =>
        {
            var otro = c.GetComponent<Hazard>();
            return otro != null && otro != this && otro.orden < orden;
        });
    }

    //Llamado por HazardInteractor cuando se completa el Hold de Act
    public void Interactuar(PlayerInput pi)
    {
        if (pi == null) return;

        // Si es el Organizador y ya tiene algo en mano, no puede intentar recolectar otro
        if (accion == HazardAction.Recolectar)
        {
            var carrierCheck = pi.GetComponentInChildren<Organizador>();
            if (carrierCheck != null && carrierCheck.TieneHazard)
            {
                Debug.Log("El Organizador ya tiene un hazard en mano, debe entregarlo primero.");
                return;
            }
        }

        if (EstaBloqueado())
        {
            Debug.Log("Hay que limpiar primero un hazard anterior en la jerarquia.");
            return;
        }

        var config = PlayerConfigurationManager.Instance
            .GetPlayerConfigs()
            .FirstOrDefault(p => p.PlayerIndex == pi.playerIndex);

        if (config == null || config.ClassType != requiredClass) return;

        if (accion == HazardAction.Destruir)
        {
            Destroy(gameObject);
        }
        else // Recolectar
        {
            var carrier = pi.GetComponentInChildren<Organizador>();
            if (carrier != null) carrier.TryPickup(gameObject);
        }
    }

    void Start()
    {
        LevelProgressManager.Instance?.RegisterHazard();
    }

    void OnDestroy()
    {
        LevelProgressManager.Instance?.HazardCleaned();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radioBloqueo);
    }
}