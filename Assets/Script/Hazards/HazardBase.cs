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

    //Llamado por HazardInteractor cuando se completa el Hold de Act
    public void Interactuar(PlayerInput pi)
    {
        if (pi == null) return;

        var config = PlayerConfigurationManager.Instance
            .GetPlayerConfigs()
            .FirstOrDefault(p => p.PlayerIndex == pi.playerIndex);

        Debug.Log("Clase jugador: " + config?.ClassType + " | Requerida: " + requiredClass);

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
}
