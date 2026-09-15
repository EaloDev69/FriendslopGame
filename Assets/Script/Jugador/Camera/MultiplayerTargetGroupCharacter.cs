using Unity.Cinemachine;
using UnityEngine;

public class MultiplayerTargetGroupCharacter : MonoBehaviour
{
    [SerializeField] CinemachineTargetGroup targetGroup;
    [SerializeField] float defaultWeight = 1f;
    [SerializeField] float defaultRadius = 2f;

    void Awake()
    {
        if(targetGroup == null)
        {
            targetGroup = FindFirstObjectByType<CinemachineTargetGroup>();
        }
    }

    public void RegisterTarget(Transform target)
    {
        if (target == null || targetGroup == null) return;

        targetGroup.AddMember(target, defaultWeight, defaultRadius);
    }
    
    public void UnRegisterTarget(Transform target)
    {
        if (target == null || targetGroup == null) return;

        targetGroup.RemoveMember(target);
    }
}
