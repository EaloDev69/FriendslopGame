using UnityEngine;

public class SelectLevelButton : MonoBehaviour
{
    [SerializeField] private int buildIndexNivel;

    public void Seleccionar()
    {
        if (buildIndexNivel > EscenarioManager.Instance.NivelDesbloqueado)
        {
            Debug.Log("Este nivel todavia no esta desbloqueado.");
            return;
        }

        EscenarioManager.Instance.CargarNivel(buildIndexNivel);
    }
}