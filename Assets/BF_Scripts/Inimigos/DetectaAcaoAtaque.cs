using UnityEngine;

public class DetectaAcaoAtaque : MonoBehaviour
{
    private SistemaVida sistemaVida;
    [SerializeField] private int tipoInimigo;
    [SerializeField] private InimigoSerra inimigoSerra;

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
        if (collision.CompareTag("Player"))
        {
            if (tipoInimigo == 1) // Serra
            {
                inimigoSerra.podeTentarAtacar = true;
                Debug.Log("Inimigo serra pode atacar");
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (tipoInimigo == 1)   // Serra
            {
                inimigoSerra.podeTentarAtacar = false;
                Debug.Log("Inimigo serra não pode atacar");
            }
            
        }
    }

}

