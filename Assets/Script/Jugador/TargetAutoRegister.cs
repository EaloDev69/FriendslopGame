using UnityEngine;

public class TargetAutoRegister : MonoBehaviour
{
    private MultiplayerTargetGroupCharacter manager;

    void Start()
    {
        manager = FindFirstObjectByType<MultiplayerTargetGroupCharacter>();

        if(manager != null)
        {
            manager.RegisterTarget(transform);
        }
    }

    void OnDestroy()
    {
        if (manager != null)
        {
            manager.UnRegisterTarget(transform);
        }
    }
}
