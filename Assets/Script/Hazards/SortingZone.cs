using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class SortingZone : MonoBehaviour
{
    [Header("Restringir a una clase especifica (opcional)")]
    [SerializeField] private ClassType claseRequerida = ClassType.Organizador;

    private void OnTriggerEnter(Collider other)
    {
        var carrier = other.GetComponentInParent<Organizador>();
        if (carrier == null || !carrier.TieneHazard) return;

        if (claseRequerida != ClassType.Organizador)
        {
            var pi = other.GetComponentInParent<PlayerInput>();
            if (pi == null) return;

            var config = PlayerConfigurationManager.Instance
                .GetPlayerConfigs()
                .FirstOrDefault(p => p.PlayerIndex == pi.playerIndex);

            if (config == null || config.ClassType != claseRequerida) return;
        }

        carrier.EntregarHazard();
    }
}