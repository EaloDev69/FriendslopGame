using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] int tiempoTotal = 180;
    [SerializeField] Image barraDeTiempo; // Image con Type = Filled

    float contador;
    bool detenido = false;
    bool eventoDisparado = false;

    public float TiempoRestante => contador;
    public bool TiempoAgotado => contador <= 0f;
    public event System.Action OnTiempoAgotado;

    void Start()
    {
        contador = tiempoTotal;
    }

    void Update()
    {
        if (detenido || contador <= 0f)
        {
            if (contador <= 0f && !eventoDisparado)
            {
                eventoDisparado = true;
                OnTiempoAgotado?.Invoke();
            }
            return;
        }

        contador -= Time.deltaTime;
        contador = Mathf.Max(0f, contador);

        if (barraDeTiempo != null)
            barraDeTiempo.fillAmount = contador / tiempoTotal;
    }

    public void Detener() => detenido = true;
    public void Reanudar() => detenido = false;
    public void Reiniciar()
    {
        contador = tiempoTotal;
        detenido = false;
        eventoDisparado = false;
    }
}