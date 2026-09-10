using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] Canvas cv;
    [SerializeField] Canvas sm;

    void Awake()
    {
        sm.gameObject.SetActive(false);
    }
    public void ApagarMenu()
    {
        cv.gameObject.SetActive(false);
        sm.gameObject.SetActive(true);
    }
    public void EncenderMenu()
    {
        cv.gameObject.SetActive(true);
        sm.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ApagarMenu();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            EncenderMenu();
        }
    }
}
