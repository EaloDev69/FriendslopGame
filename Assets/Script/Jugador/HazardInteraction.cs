using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class HazardInteractor : MonoBehaviour
{
    private readonly List<Hazard> hazardsCercanos = new List<Hazard>();
    private PlayerInput playerInput;

    void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
    }

    public void Act(InputAction.CallbackContext context)
    {
        Debug.Log("Act invocado, performed: " + context.performed);
        if (!context.performed) return;

        var hazard = hazardsCercanos.FirstOrDefault(h => h != null);
        Debug.Log("Hazards cercanos: " + hazardsCercanos.Count);
        if (hazard == null) return;

        hazard.Interactuar(playerInput);
    }

    public void RegistrarCercano(Hazard hazard)
    {
        if (!hazardsCercanos.Contains(hazard))
            hazardsCercanos.Add(hazard);
    }

    public void QuitarCercano(Hazard hazard)
    {
        hazardsCercanos.Remove(hazard);
    }
}