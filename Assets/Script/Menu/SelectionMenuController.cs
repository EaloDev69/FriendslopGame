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

    float ignoreInputTime = 1.5f;
    bool inputEnabled;

    public void SetPlayerIndex(int pi)
    {
        playerIndex = pi;
        titleText.SetText("Jugador " + (pi + 1).ToString());

        ignoreInputTime = Time.time + ignoreInputTime;
    }

    void Update()
    {
        if(Time.time > ignoreInputTime)
        {
            inputEnabled = true;
        }
    }

    public void SetPrefab(GameObject JugadorPrefab)
    {
        if(!inputEnabled) {return; }

        PlayerConfigurationManager.Instance.SetPlayerClass(playerIndex, JugadorPrefab);
        ReadyPanel.SetActive(true);
        readyButton.Select();
        MenuPanel.SetActive(false);
    }

    public void ReadyPlayer()
    {
        if (!inputEnabled) {return; }

        PlayerConfigurationManager.Instance.ReadyPlayer(playerIndex);
        readyButton.gameObject.SetActive(false);
    }
}
