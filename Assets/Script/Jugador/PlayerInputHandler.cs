using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    PlayerConfigurations playerConfig;

    private PlayerController controls;

    private GameObject classInstance;

    void Awake()
    {
        controls = new PlayerController();
    }

    public void InitializePlayer(PlayerConfigurations pc)
    {
        playerConfig = pc;
        if(pc.ClassPrefab != null)
        {
            classInstance = Instantiate(pc.ClassPrefab, transform.position, transform.rotation);
        }
    }
}
