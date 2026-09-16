using UnityEngine;

public class SelectLevelButton : MonoBehaviour
{
    [SerializeField] private Nivel nivel;

    public void Seleccionar()
    {
        EscenarioManager.Instance.CargarNivel(nivel);
    }
}