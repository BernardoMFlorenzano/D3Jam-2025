using UnityEngine;

public class ColisorCorpo : MonoBehaviour
{
    private SistemaVida sistemaVida;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CorpoInimigo"))
        {
            sistemaVida = collision.GetComponentInParent<SistemaVida>();
            sistemaVida.LevaAtaque();
        }
    }
}
