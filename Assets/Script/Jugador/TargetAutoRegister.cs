using UnityEngine;
using UnityEngine.SceneManagement;

public class TargetAutoRegister : MonoBehaviour
{
    private MultiplayerTargetGroupCharacter manager;

    void OnEnable()
    {
        SceneManager.sceneLoaded += HandleSceneLoaded;
        RegisterToCurrentManager();
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;
        UnregisterFromCurrentManager();
    }

    void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UnregisterFromCurrentManager();
        RegisterToCurrentManager();
    }

    void RegisterToCurrentManager()
    {
        manager = FindFirstObjectByType<MultiplayerTargetGroupCharacter>();
        if (manager != null)
        {
            manager.RegisterTarget(transform);
        }
    }

    void UnregisterFromCurrentManager()
    {
        if (manager != null)
        {
            manager.UnRegisterTarget(transform);
        }
        manager = null;
    }

    void OnDestroy()
    {
        UnregisterFromCurrentManager();
    }
}