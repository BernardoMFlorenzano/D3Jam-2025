using UnityEngine;

public class ObstaculoDano : MonoBehaviour
{
    [SerializeField] private int dano;
    private SistemaVida sistemaVida;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CorpoPlayer"))
        {
            sistemaVida = collision.GetComponentInParent<SistemaVida>();

            if (sistemaVida)
            {
                sistemaVida.LevaAtaquePlayer(dano, false, 0, gameObject);
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("CorpoPlayer"))
        {
            if (sistemaVida)
            {
                sistemaVida.LevaAtaquePlayer(dano, false, 0, gameObject);
            }
        }
    }

}
