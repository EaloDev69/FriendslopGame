using UnityEngine;

public class AudioListenerCentral : MonoBehaviour
{
    [SerializeField] string tagJugador = "Player";
    GameObject[] jugadores;
    float tiempoRefresco = 1f;
    float temporizador = 0f;

    void LateUpdate()
    {
        temporizador += Time.deltaTime;
        if (temporizador >= tiempoRefresco || jugadores == null || jugadores.Length == 0)
        {
            jugadores = GameObject.FindGameObjectsWithTag(tagJugador);
            temporizador = 0f;
        }

        if (jugadores == null || jugadores.Length == 0) return;

        Vector3 centro = Vector3.zero;
        int contador = 0;
        foreach (var j in jugadores)
        {
            if (j != null)
            {
                centro += j.transform.position;
                contador++;
            }
        }

        if (contador > 0)
            transform.position = centro / contador;
    }
}