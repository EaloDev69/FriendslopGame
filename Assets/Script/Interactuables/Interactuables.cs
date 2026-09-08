using System;
using Unity.VisualScripting;
using UnityEngine;

public class Interactuables : MonoBehaviour
{
    public bool interactuar = false;
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Jugador entro");
        interactuar = true;

    }
    void OnTriggerExit(Collider other)
    {
        Debug.Log("Jugador fuera");
        interactuar = false;
    }


}
