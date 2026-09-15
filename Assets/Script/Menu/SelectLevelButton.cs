using UnityEngine;

public class SelectLevelButton : MonoBehaviour
{
    [SerializeField] private int buildIndexNivel;

    public void Seleccionar()
    {
        EscenarioManager.Instance.CargarNivel(buildIndexNivel);
    }
}