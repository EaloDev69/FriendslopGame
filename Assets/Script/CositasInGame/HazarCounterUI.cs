using TMPro;
using UnityEngine;

public class HazardCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI counterText;
    [SerializeField] private string formato = "Hazards: {0}/{1}";

    void OnEnable()
    {
        if (LevelProgressManager.Instance != null)
        {
            LevelProgressManager.Instance.OnHazardsChanged += ActualizarTexto;
            // Por si esto se activa despues de que ya se registraron hazards
            ActualizarTexto(LevelProgressManager.Instance.HazardsRestantes, LevelProgressManager.Instance.TotalHazards);
        }
    }

    void OnDisable()
    {
        if (LevelProgressManager.Instance != null)
        {
            LevelProgressManager.Instance.OnHazardsChanged -= ActualizarTexto;
        }
    }

    void ActualizarTexto(int restantes, int total)
    {
        counterText.SetText(string.Format(formato, restantes, total));
    }
}