using UnityEngine;
using UnityEngine.SceneManagement;

public class EscenarioManager : MonoBehaviour
{
    public static EscenarioManager Instance {get; private set;}

    public int NivelDesbloqueado {get; private set;} = 1; // Nivel 1 siempre disponible

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        NivelDesbloqueado = PlayerPrefs.GetInt("NivelDesbloqueado", 1);
    }

    //Llamado cuando se completa un nivel, con el build index del nivel SIGUIENTE
    public void DesbloquearNivel(int nivel)
    {
        if (nivel <= NivelDesbloqueado) return;

        NivelDesbloqueado = nivel;
        PlayerPrefs.SetInt("NivelDesbloqueado", NivelDesbloqueado);
        PlayerPrefs.Save();
    }

    public void Menu() => SceneManager.LoadScene(0);
    public void Niveluno() => SceneManager.LoadScene(1);
    public void Niveldos() => SceneManager.LoadScene(2);
    public void Creditos() => SceneManager.LoadScene(3);

    public void CargarNivel(int buildIndex) => SceneManager.LoadScene(buildIndex);

    public void Salir()
    {
        Application.Quit();
        Debug.Log("Saliste");
    }
}