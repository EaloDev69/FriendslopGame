using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HazardSpawner : MonoBehaviour
{
    [Serializable]
    public class HazardOption
    {
        public GameObject prefab;
        [Range(0f, 1f)] public float weight = 1f;
        [HideInInspector] public ClassType requiredClass;
        [HideInInspector] public HazardAction accion;
    }

    [Header("Area de spawneo")]
    [SerializeField] private Vector2 areaSize = new Vector2(10f, 10f); 
    [SerializeField] private bool dibujarGizmo = true;

    [Header("Hazards spawneables")]
    [SerializeField] private HazardOption[] hazardOptions;

    [Header("Cantidad de hazards")]
    [SerializeField] private int cantidadMin = 5;
    [SerializeField] private int cantidadMax = 10;

    [Header("Variedad")]
    [SerializeField] private bool garantizarVariedad = true;

    private readonly List<GameObject> hazardsActivos = new List<GameObject>();

    void OnValidate()
    {
        if (hazardOptions == null) return;

        foreach (var option in hazardOptions)
        {
            if (option.prefab == null) continue;

            var hazardComp = option.prefab.GetComponent<Hazard>();
            if (hazardComp == null) continue;

            option.requiredClass = hazardComp.requiredClass;
            option.accion = hazardComp.accion;
        }
    }

    public void SpawnHazards()
    {
        if (hazardOptions == null || hazardOptions.Length == 0) return;

        int cantidad = UnityEngine.Random.Range(cantidadMin, cantidadMax + 1);
        var pendientes = new List<GameObject>();

        if (garantizarVariedad)
        {
            foreach (var option in hazardOptions)
            {
                if (option.prefab == null) continue;
                if (pendientes.Count >= cantidad) break;
                pendientes.Add(option.prefab);
            }
        }

        while (pendientes.Count < cantidad)
        {
            var prefab = ElegirHazardAleatorio();
            if (prefab == null) break;
            pendientes.Add(prefab);
        }

        foreach (var prefab in pendientes)
        {
            SpawnHazard(prefab);
        }
    }

    void SpawnHazard(GameObject prefab)
    {
        if (prefab == null) return;

        Vector3 pos = ObtenerPosicionAleatoria();
        GameObject hazard = Instantiate(prefab, pos, Quaternion.identity, transform);
        hazardsActivos.Add(hazard);
    }

    GameObject ElegirHazardAleatorio()
    {
        if (hazardOptions == null || hazardOptions.Length == 0) return null;

        float totalPeso = hazardOptions.Sum(o => o.weight);

        if (totalPeso <= 0f)
            return hazardOptions[UnityEngine.Random.Range(0, hazardOptions.Length)].prefab;

        float rand = UnityEngine.Random.Range(0f, totalPeso);
        float acumulado = 0f;
        foreach (var opt in hazardOptions)
        {
            acumulado += opt.weight;
            if (rand <= acumulado) return opt.prefab;
        }

        return hazardOptions[hazardOptions.Length - 1].prefab;
    }

    Vector3 ObtenerPosicionAleatoria()
    {
        float halfX = areaSize.x / 2f;
        float halfZ = areaSize.y / 2f;
        Vector3 centro = transform.position;
        Vector3 pos = centro;
        int intentos = 10;

        while (intentos > 0)
        {
            float x = UnityEngine.Random.Range(-halfX, halfX);
            float z = UnityEngine.Random.Range(-halfZ, halfZ);
            pos = centro + new Vector3(x, 0f, z);
            intentos--;
        }

        return pos;
    }

    void OnDrawGizmosSelected()
    {
        if (!dibujarGizmo) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(areaSize.x, 0.1f, areaSize.y));
    }
}