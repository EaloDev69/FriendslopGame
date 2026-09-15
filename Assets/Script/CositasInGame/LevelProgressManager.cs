using System;
using UnityEngine;

public class LevelProgressManager : MonoBehaviour
{
    public static LevelProgressManager Instance { get; private set; }

    [SerializeField] private Timer timer; // referencia al Timer de esta escena

    public int TotalHazards { get; private set; }
    public int HazardsRestantes { get; private set; }
    public bool NivelCompletado { get; private set; }

    //Para actualizar UI: (restantes, total)
    public event Action<int, int> OnHazardsChanged;
    public event Action OnNivelCompletado;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    //Llamado por cada Hazard al nacer
    public void RegisterHazard()
    {
        TotalHazards++;
        HazardsRestantes++;
        OnHazardsChanged?.Invoke(HazardsRestantes, TotalHazards);
    }

    //Llamado por cada Hazard al ser destruido (limpiado o entregado)
    public void HazardCleaned()
    {
        if (NivelCompletado) return;

        HazardsRestantes = Mathf.Max(0, HazardsRestantes - 1);
        Debug.Log($"Hazard limpiado. Restantes: {HazardsRestantes}/{TotalHazards}");
        OnHazardsChanged?.Invoke(HazardsRestantes, TotalHazards);

        VerificarCompletado();
    }

    void VerificarCompletado()
    {
        if (HazardsRestantes > 0) return;
        if (timer != null && timer.TiempoAgotado)
        {
            Debug.Log("Tiempo agotado, no se completa el nivel.");
            return;
        }

        NivelCompletado = true;
        Debug.Log("NIVEL COMPLETADO, disparando evento.");
        OnNivelCompletado?.Invoke();
    }
}