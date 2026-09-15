using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AutoBoxCollider : MonoBehaviour
{
    void Start()
    {
        UpdateCollider();
    }

    public void UpdateCollider()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
            return;

        Bounds bounds = renderers[0].bounds;

        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }

        BoxCollider box = GetComponent<BoxCollider>();

        // Convertir de coordenadas globales a locales
        box.center = transform.InverseTransformPoint(bounds.center);

        Vector3 localSize = transform.InverseTransformVector(bounds.size);
        box.size = new Vector3(
            Mathf.Abs(localSize.x),
            Mathf.Abs(localSize.y),
            Mathf.Abs(localSize.z)
        );
    }
}
