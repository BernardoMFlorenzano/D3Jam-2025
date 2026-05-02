using UnityEngine;

public class ObstaculoDano : MonoBehaviour
{
    [SerializeField] private int dano;
    public bool atingePelaBase = false;
    private SistemaVida sistemaVida;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CorpoPlayer") && atingePelaBase)
        {
            sistemaVida = collision.GetComponentInParent<SistemaVida>();

            if (sistemaVida)
            {
                sistemaVida.LevaAtaquePlayer(dano, false, 0, gameObject);
            }
        }
        else if (collision.CompareTag("CorpoPlayer"))
        {
            sistemaVida = collision.GetComponentInParent<SistemaVida>();
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("CorpoPlayer") && atingePelaBase)
        {
            if (sistemaVida)
            {
                sistemaVida.LevaAtaquePlayer(dano, false, 0, gameObject);
            }
        }
    }

}
