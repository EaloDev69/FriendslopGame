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
        [Range(0f, 1f)] public float weight = 1f; // probabilidad relativa

        //Solo lectura, para mostrar info util en el Inspector sin duplicar datos
        [HideInInspector] public ClassType requiredClass;
        [HideInInspector] public HazardAction accion;
    }

    [Header("Area de spawneo (rectangular, centrada en este GameObject)")]
    [SerializeField] private Vector2 areaSize = new Vector2(10f, 10f); // x = ancho, y = largo
    [SerializeField] private bool dibujarGizmo = true;

    [Header("Hazards a spawnear")]
    [SerializeField] private HazardOption[] hazardOptions;

    [Header("Cantidad de hazards")]
    [SerializeField] private int cantidadMin = 5;
    [SerializeField] private int cantidadMax = 10;

    [Header("Variedad")]
    [Tooltip("Si esta activo, se asegura de spawnear al menos 1 de cada tipo de hazard antes de completar el resto al azar.")]
    [SerializeField] private bool garantizarVariedad = true;

    [Header("Configuracion adicional")]
    [SerializeField] private LayerMask capaObstaculos; // opcional

    private readonly List<GameObject> hazardsActivos = new List<GameObject>();

    void OnValidate()
    {
        //Sincroniza los datos de solo-lectura leyendo el componente Hazard del prefab,
        //asi el Inspector siempre refleja lo que realmente tiene el prefab asignado
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

    //Llamado externamente (ej. desde InitializeLevel) al armar el nivel
    public void SpawnHazards()
    {
        if (hazardOptions == null || hazardOptions.Length == 0) return;

        int cantidad = UnityEngine.Random.Range(cantidadMin, cantidadMax + 1); // max inclusivo
        var pendientes = new List<GameObject>();

        if (garantizarVariedad)
        {
            //Un ejemplar de cada tipo disponible primero, sin pasarnos del total pedido
            foreach (var option in hazardOptions)
            {
                if (option.prefab == null) continue;
                if (pendientes.Count >= cantidad) break;
                pendientes.Add(option.prefab);
            }
        }

        //Completa el resto de forma aleatoria segun el peso de cada opcion
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

            if (capaObstaculos.value == 0 || !Physics.CheckSphere(pos, 0.5f, capaObstaculos))
            {
                return pos;
            }
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