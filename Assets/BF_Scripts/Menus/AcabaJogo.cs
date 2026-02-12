using UnityEngine;

public class AcabaJogo : MonoBehaviour
{
    private PassaCena gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PassaCena>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameManager.TrocaCena(2); // Acaba jogo
        }
    }
}
