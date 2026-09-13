using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Canvas cv;
    [SerializeField] PlayerInputManager pim;

    public void ApagarMenu()
    {
        cv.gameObject.SetActive(false);
        Debug.Log("Me debo apagar");

    
    }
}
