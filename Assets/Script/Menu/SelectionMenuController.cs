using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectionMenuController : MonoBehaviour
{
    private int playerIndex;

    [Header("Elementos del menu")]
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] GameObject ReadyPanel;
    [SerializeField] GameObject MenuPanel;
    [SerializeField] GameObject LevelSelectPanel; // nuevo
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
        PlayerConfigurationManager.Instance.OnAllPlayersReady += MostrarSeleccionNivel;
        RefreshClassButtons();
    }

    void OnDisable()
    {
        if (PlayerConfigurationManager.Instance != null)
        {
            PlayerConfigurationManager.Instance.OnPlayerClassChanged -= RefreshClassButtons;
            PlayerConfigurationManager.Instance.OnAllPlayersReady -= MostrarSeleccionNivel;
        }
    }

    void Update()
    {
        if(Time.time > ignoreInputTime)
        {
            inputEnabled = true;
        }
    }

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
        if(!inputEnabled) { return; }

        ClassOption option = Array.Find(classOptions, o => o.prefab == JugadorPrefab);
        if (option == null)
        {
            Debug.LogWarning("No se encontro ClassOption para el prefab: " + JugadorPrefab.name);
            return;
        }

        bool assigned = PlayerConfigurationManager.Instance.SetPlayerClass(playerIndex, JugadorPrefab, option.classType);
        if (!assigned)
        {
            RefreshClassButtons();
            return;
        }

        ReadyPanel.SetActive(true);
        readyButton.Select();
        MenuPanel.SetActive(false);
    }

    public void ReadyPlayer()
    {
        if (!inputEnabled) { return; }

        PlayerConfigurationManager.Instance.ReadyPlayer(playerIndex);
        readyButton.gameObject.SetActive(false);
        cancelButton.gameObject.SetActive(false);
    }

    public void UnselectClass()
    {
        if (!inputEnabled) { return; }

        PlayerConfigurationManager.Instance.UnsetPlayerClass(playerIndex);
        ReadyPanel.SetActive(false);
        readyButton.gameObject.SetActive(true);
        MenuPanel.SetActive(true);

        SelectFirstAvailableClassButton();
    }

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

    //Se llama en TODOS los jugadores cuando el ultimo confirma su Ready
    void MostrarSeleccionNivel()
    {
        ReadyPanel.SetActive(false);

        if (LevelSelectPanel != null)
        {
            LevelSelectPanel.SetActive(true);

            // Solo el jugador 0 puede interactuar; los demas ven el panel pero no lo navegan
            var botones = LevelSelectPanel.GetComponentsInChildren<Button>();
            foreach (var boton in botones)
            {
                boton.interactable = (playerIndex == 0);
            }

            if (playerIndex == 0 && botones.Length > 0)
            {
                botones[0].Select();
            }
        }
    }
}