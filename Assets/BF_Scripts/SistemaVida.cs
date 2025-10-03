using TMPro;
using UnityEngine;

public class SistemaVida : MonoBehaviour
{
    public bool atingivelBasePadrao;
    public bool atingivelBaseLanca;
    [SerializeField] private TMP_Text textoTeste;
    private Rigidbody2D rb;

    //private bool levandoAtaque;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (textoTeste)
            textoTeste.text = "10";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LevaAtaquePadrao(int tipo, GameObject atacante)
    {
        if (atingivelBasePadrao)
        {
            if (tipo == 1)
            {
                if (textoTeste)
                {
                    int num = int.Parse(textoTeste.text) - 1;
                    textoTeste.text = num.ToString();
                }
                Debug.Log("Acertou ataque no chao");
            }
            else if (tipo == 2)
            {
                if (textoTeste)
                {
                    int num = int.Parse(textoTeste.text) - 1;
                    textoTeste.text = num.ToString();
                }
                Debug.Log("Acertou ataque aereo");

                Vector2 direcao = (transform.position - atacante.transform.position).normalized;

                rb.AddForce(direcao * 200, ForceMode2D.Impulse);
            }
        }
    }
    
    public void LevaAtaqueLanca(int tipo, GameObject atacante)
    {
        if (atingivelBaseLanca)
        {
            if (tipo == 1)  
            {
                if (textoTeste)
                {
                    int num = int.Parse(textoTeste.text) - 1;
                    textoTeste.text = num.ToString();
                }
                Debug.Log("Acertou ataque no chao");
            }
            else if (tipo == 2)
            {
                if (textoTeste)
                {
                    int num = int.Parse(textoTeste.text) - 1;
                    textoTeste.text = num.ToString();
                }
                Debug.Log("Acertou ataque aereo");

                Vector2 direcao = (transform.position - atacante.transform.position).normalized;

                rb.AddForce(direcao*200, ForceMode2D.Impulse);
            }
        }
    }
}
