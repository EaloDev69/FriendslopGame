using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [Header("Configurar tiempos")]
    [SerializeField] int tiempoTotal = 180; 
    float contador = 0f;

    [SerializeField] Slider BarradeTiempo;

    bool detenido = false;

    public float TiempoRestante => contador;
    public bool TiempoAgotado => contador <= 0f;

    void Start()
    {
        contador = tiempoTotal;
    }

    void Update()
    {
        actualizarContador();
    }

    public void Detener()
    {
        detenido = true;
    }

    void actualizarContador()
    {
        if (detenido) return;

        if (contador > 0f)
        {
            contador -= Time.deltaTime;
            BarradeTiempo.maxValue = tiempoTotal;
            BarradeTiempo.value = contador;
        }
        else
        {
            ResetLevel();
        }
    }

    private void ResetLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
    }
}