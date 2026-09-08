using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [Header("Configurar tiempos")]
    //tiempo modificable segun necesidad
    [SerializeField] int tiempoTotal = 180; 
    //float para poder restar con el deltaTime
    float contador = 0f;

    //insertar la Slider aqui
    [SerializeField] Slider BarradeTiempo;

    //sonido eventualmente
    [SerializeField] AudioSource audioSource;

    //referencia para en caso de detenerlo por game over y demas razones
    bool detenido = false;

    void Start()
    {
        //Pasar de INT a Float
        contador = tiempoTotal;
        if(audioSource = null) return;
    }

    void Update()
    {
        actualizarContador();
    }

    public void Detener()
    {
        //en caso de querer usar un boton/comando desde otro codigo
        detenido = true;
    }

    void actualizarContador()
    {
        if (contador > 0f)
        {
            //restar el float
            contador -= Time.deltaTime;

            //damos un valor maximo de la barra de tiempo basado en el tiempo inicial
            BarradeTiempo.maxValue = tiempoTotal;
            //a medida que el deltaTime resta, se ve reflejado en la barra de tiempo
            BarradeTiempo.value = contador;
        }
    }
}
