using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class SpawnPlayerMenu : MonoBehaviour
{
    public GameObject PlayerSetupPanel;
    public PlayerInput Input;

    void Awake()
    {
        var rootMenu = GameObject.Find("Layout");
        if (rootMenu != null && MenuManager.Instance != null && MenuManager.Instance.Menuapagado)
        {
            var menu = Instantiate(PlayerSetupPanel, rootMenu.transform);
            Input.uiInputModule = menu.GetComponentInChildren<InputSystemUIInputModule>();
            menu.GetComponent<SelectionMenuController>().SetPlayerIndex(Input.playerIndex);
        }
    }
}