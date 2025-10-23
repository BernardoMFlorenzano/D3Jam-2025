using UnityEngine;

public class ParaMusica : MonoBehaviour
{
    [SerializeField] private AudioClip musica;


    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            AudioManager.instance.PlayMusic(musica);
        }
    }
}
