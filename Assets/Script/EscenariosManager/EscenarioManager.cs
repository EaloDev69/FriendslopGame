using UnityEngine;
using UnityEngine.SceneManagement;

public class EscenarioManager : MonoBehaviour
{
    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

    public void Niveluno()
    {
        SceneManager.LoadScene(1);
    }

    public void Niveldos()
    {
        SceneManager.LoadScene(2);
    }
    
    public void Creditos()
    {
        SceneManager.LoadScene(3);
    }

    public void Salir()
    {
        Application.Quit();
        Debug.Log("Saliste");
    }
    
}
