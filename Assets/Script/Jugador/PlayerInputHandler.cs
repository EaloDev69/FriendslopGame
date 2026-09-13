using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    PlayerConfigurations playerConfig;

    private GameObject classInstance;
    private PlayerInput playerInput;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    public void InitializePlayer(PlayerConfigurations pc)
    {
        playerConfig = pc;
        if (pc.ClassPrefab == null) return;

        classInstance = Instantiate(pc.ClassPrefab, transform);
        classInstance.transform.localPosition = Vector3.zero;
        classInstance.transform.localRotation = Quaternion.identity;

        var controller = classInstance.GetComponent<PlayerController>();
        if (controller != null && playerInput != null)
        {
            playerInput.actions["Gameplay/Move"].performed += controller.Move;
            playerInput.actions["Gameplay/Move"].canceled += controller.Move;
            playerInput.actions["Gameplay/Jump"].performed += controller.Jump;
            playerInput.actions["Gameplay/Jump"].canceled += controller.Jump;

            playerInput.SwitchCurrentActionMap("Gameplay"); // <- clave
        }
    }
}