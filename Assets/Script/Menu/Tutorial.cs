using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [Header("Textos")]
    [SerializeField] Canvas Textostutorial;
    [SerializeField] TextMeshProUGUI Limpiador;
    [SerializeField] TextMeshProUGUI Organizador;
    [SerializeField] TextMeshProUGUI Trapeador;
    [SerializeField] TextMeshProUGUI Barrendero;
    [SerializeField] TextMeshProUGUI Controles;


    void Awake()
    {
        Textostutorial.gameObject.SetActive(false);
    }
    public void MostrarTutorial()
    {
        Textostutorial.gameObject.SetActive(true);
    }

    public void DesactivarTutorial()
    {
        Textostutorial.gameObject.SetActive(false);
    }

    public void showControles()
    {
        
    }
    
    public void showClaseL()
    {
        
    }

    public void showClaseB()
    {
        
    }
    public void showClaseO()
    {
        
    }
    public void showClaseT()
    {
        
    }
}
