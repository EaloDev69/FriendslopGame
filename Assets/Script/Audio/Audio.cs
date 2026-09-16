using UnityEngine;

public class Audio : MonoBehaviour
{
    private AudioSource music;

    void Start()
    {
        music = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            music.Play();
        }
    }

    void OnTriggerExit(Collider other)
    {
       music.Stop();        
    }
}
