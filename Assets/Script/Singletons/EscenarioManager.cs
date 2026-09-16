using UnityEngine;
using UnityEngine.SceneManagement;

public class EscenarioManager : MonoBehaviour
{
    public static EscenarioManager Instance {get; private set;}

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void CargarNivel(Nivel nivel) => SceneManager.LoadScene((int)nivel);

    public void Menu() => CargarNivel(Nivel.Menu);

    public void Salir()
    {
        Application.Quit();
        Debug.Log("Saliste");
    }
}