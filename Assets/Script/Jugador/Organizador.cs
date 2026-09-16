using UnityEngine;

public class Organizador : MonoBehaviour
{
    [Header("Manos Organizador")]
    [SerializeField] Transform holdPoint;

    private GameObject hazardCargado;

    public bool TieneHazard => hazardCargado != null;

        public bool TryPickup(GameObject hazard)
    {
        if (hazardCargado != null) return false;

        hazardCargado = hazard;

        var col = hazard.GetComponent<Collider>();
        if (col != null) col.enabled = false; 
        var rb = hazard.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        hazard.transform.SetParent(holdPoint);
        hazard.transform.localPosition = Vector3.zero;
        hazard.transform.localRotation = Quaternion.identity;

        return true;
    }

    public void EntregarHazard()
    {
        if (hazardCargado == null) return;

        Destroy(hazardCargado);
        hazardCargado = null;
    }
}
