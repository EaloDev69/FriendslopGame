using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioFuente3D : MonoBehaviour
{
    [Header("Configuración de distancia")]
    [SerializeField] float distanciaMinima = 1f;
    [SerializeField] float distanciaMaxima = 15f;
    [SerializeField] AnimationCurve curvaAtenuacion = AnimationCurve.Linear(0, 1, 1, 0);

    AudioSource fuente;

    void Awake()
    {
        fuente = GetComponent<AudioSource>();
        ConfigurarAudio3D();
    }

    void ConfigurarAudio3D()
    {
        fuente.spatialBlend = 1f; // 0 = 2D, 1 = 3D completo
        fuente.rolloffMode = AudioRolloffMode.Custom;
        fuente.minDistance = distanciaMinima;
        fuente.maxDistance = distanciaMaxima;
        fuente.SetCustomCurve(AudioSourceCurveType.CustomRolloff, curvaAtenuacion);
        fuente.spread = 0f; // 0 = paneo direccional preciso
        fuente.dopplerLevel = 0f; // normalmente no quieres doppler en top-down
    }
}
