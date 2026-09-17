using System;
using UnityEngine;

public class LevelProgressManager : MonoBehaviour
{
    public static LevelProgressManager Instance { get; private set; }

    [SerializeField] private Timer timer;

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

    public void RegisterHazard()
    {
        TotalHazards++;
        HazardsRestantes++;
        OnHazardsChanged?.Invoke(HazardsRestantes, TotalHazards);
    }

    public void HazardCleaned()
    {
        if (NivelCompletado) return;

        HazardsRestantes = Mathf.Max(0, HazardsRestantes - 1);
        OnHazardsChanged?.Invoke(HazardsRestantes, TotalHazards);

        VerificarCompletado();
    }

    void VerificarCompletado()
    {
        if (HazardsRestantes > 0) return;
        if (timer != null && timer.TiempoAgotado)
        {
            return;
        }

        NivelCompletado = true;
        OnNivelCompletado?.Invoke();
    }
}