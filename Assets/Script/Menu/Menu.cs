using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    [SerializeField] Canvas cv;
    [SerializeField] GameObject layout; // objeto Layout, arrastrar en el Inspector

    public bool Menuapagado { get; private set; }

    void Awake()
    {
        Instance = this;

        // Nadie puede unirse mientras se ve la portada
        if (UnityEngine.InputSystem.PlayerInputManager.instance != null)
        {
            UnityEngine.InputSystem.PlayerInputManager.instance.DisableJoining();
        }
    }

    public void ApagarMenu()
    {
        cv.gameObject.SetActive(false);
        Debug.Log("Me debo apagar");

        layout.SetActive(true); // 1. se activa Layout
        Menuapagado = true;

        // 2. solo ahora se permite el Join
        if (UnityEngine.InputSystem.PlayerInputManager.instance != null)
        {
            UnityEngine.InputSystem.PlayerInputManager.instance.EnableJoining();
        }

        Debug.Log("Layout activo, Join habilitado");
    }
}