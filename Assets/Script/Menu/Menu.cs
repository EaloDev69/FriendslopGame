using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    [SerializeField] Canvas cv;
    [SerializeField] GameObject layout;
    [SerializeField] TextMeshProUGUI Instruccion;

    [Header("Seleccion de nivel")]
    [SerializeField] GameObject levelSelectionPanel; // nuevo panel, arrastrar en el Inspector

    public bool Menuapagado { get; private set; }

    void Awake()
    {
        Instance = this;

        if (UnityEngine.InputSystem.PlayerInputManager.instance != null)
        {
            UnityEngine.InputSystem.PlayerInputManager.instance.DisableJoining();
        }
        Instruccion.gameObject.SetActive(false);

        if (levelSelectionPanel != null) levelSelectionPanel.SetActive(false);
    }

    void Start()
    {
        if (PlayerConfigurationManager.Instance != null)
        {
            PlayerConfigurationManager.Instance.OnAllPlayersReady += MostrarSeleccionNivel;
        }
    }

    void OnDestroy()
    {
        if (PlayerConfigurationManager.Instance != null)
        {
            PlayerConfigurationManager.Instance.OnAllPlayersReady -= MostrarSeleccionNivel;
        }
    }

    public void ApagarMenu()
    {
        cv.gameObject.SetActive(false);
        Instruccion.gameObject.SetActive(true);

        layout.SetActive(true);
        Menuapagado = true;

        if (UnityEngine.InputSystem.PlayerInputManager.instance != null)
        {
            UnityEngine.InputSystem.PlayerInputManager.instance.EnableJoining();
        }
    }

    void MostrarSeleccionNivel()
    {
        Instruccion.gameObject.SetActive(false);

        if (levelSelectionPanel != null) levelSelectionPanel.SetActive(true);
    }
}