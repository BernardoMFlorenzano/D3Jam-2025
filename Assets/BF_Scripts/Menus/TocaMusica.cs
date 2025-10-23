using UnityEngine;

public class TocaMusica : MonoBehaviour
{
    [SerializeField] private AudioClip musica;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.instance.PlayMusic(musica);
    }

}
