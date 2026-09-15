using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectionMenuController : MonoBehaviour
{
    private int  playerIndex;

    [Header("Elementos del menu")]
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] GameObject ReadyPanel;
    [SerializeField] GameObject MenuPanel;
    [SerializeField] Button readyButton;
    [SerializeField] Button cancelButton;

    [Serializable]
    public class ClassOption
    {
        public Button button;
        public GameObject prefab;
        public ClassType classType;
    }

    [Header("Clases seleccionables")]
    [SerializeField] ClassOption[] classOptions;

    float ignoreInputTime = 1.5f;
    bool inputEnabled;

    public void SetPlayerIndex(int pi)
    {
        playerIndex = pi;
        titleText.SetText("Jugador " + (pi + 1).ToString());

        ignoreInputTime = Time.time + ignoreInputTime;
    }

    void OnEnable()
    {
        PlayerConfigurationManager.Instance.OnPlayerClassChanged += RefreshClassButtons;
        RefreshClassButtons();
    }

    void OnDisable()
    {
        if (PlayerConfigurationManager.Instance != null)
        {
            PlayerConfigurationManager.Instance.OnPlayerClassChanged -= RefreshClassButtons;
        }
    }

    void Update()
    {
        if(Time.time > ignoreInputTime)
        {
            inputEnabled = true;
        }
    }

    //Deshabilita los botones de clases ya escogidas por otros jugadores
    void RefreshClassButtons()
    {
        if (classOptions == null) return;

        foreach (var option in classOptions)
        {
            if (option.button == null || option.prefab == null) continue;

            bool takenByOther = PlayerConfigurationManager.Instance.IsClassTaken(option.prefab, playerIndex);
            option.button.interactable = !takenByOther;
        }
    }

    public void SetPrefab(GameObject JugadorPrefab)
    {
        if(!inputEnabled) {return; }

        ClassOption option = Array.Find(classOptions, o => o.prefab == JugadorPrefab);
        if (option == null)
        {
            Debug.LogWarning("No se encontro ClassOption para el prefab: " + JugadorPrefab.name);
            return;
        }

        bool assigned = PlayerConfigurationManager.Instance.SetPlayerClass(playerIndex, JugadorPrefab, option.classType);
        if (!assigned)
        {
            // Otro jugador tomo esta clase justo antes; refrescamos por si acaso
            RefreshClassButtons();
            return;
        }

        ReadyPanel.SetActive(true);
        readyButton.Select();
        MenuPanel.SetActive(false);
    }

    public void ReadyPlayer()
    {
        if (!inputEnabled) {return; }

        PlayerConfigurationManager.Instance.ReadyPlayer(playerIndex);
        readyButton.gameObject.SetActive(false);
        cancelButton.gameObject.SetActive(false);
        
    }

    //Enganchar a un boton dentro del ReadyPanel (ej. "Cancelar")
    public void UnselectClass()
    {
        if (!inputEnabled) {return; }

        PlayerConfigurationManager.Instance.UnsetPlayerClass(playerIndex);
        ReadyPanel.SetActive(false);
        readyButton.gameObject.SetActive(true);
        MenuPanel.SetActive(true);

        SelectFirstAvailableClassButton();
    }

    //Selecciona el primer boton de clase habilitado, para que el menu
    //no quede sin foco (softlock) al volver desde el ReadyPanel
    void SelectFirstAvailableClassButton()
    {
        if (classOptions == null) return;

        foreach (var option in classOptions)
        {
            if (option.button != null && option.button.interactable)
            {
                option.button.Select();
                return;
            }
        }
    }
}