using System;
using TMPro;
using UnityEngine;

public class SistemaVida : MonoBehaviour
{
    public bool atingivelBase;
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

    public void LevaAtaqueSoco(int tipo, GameObject atacante)
    {
        if (atingivelBase)
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

                Vector2 direcao = transform.position - atacante.transform.position;
                direcao.Normalize();

                rb.AddForce(new Vector2(direcao.x * 200, 0), ForceMode2D.Impulse);
            }
        }
    }

    // Ataques de armas estão separados para depois botar diferença de dano e outras caracteristicas

    public void LevaAtaqueCorte(int tipo, int dano, bool knockback, float forcaKnockback, GameObject atacante)
    {
        if (atingivelBase)
        {
            if (tipo == 1)
            {
                if (textoTeste)
                {
                    int num = int.Parse(textoTeste.text) - dano;
                    textoTeste.text = num.ToString();
                }
                Debug.Log("Acertou ataque no chao");

                if (knockback)
                {
                    Vector2 direcao = transform.position - atacante.transform.position;
                    direcao.Normalize();
                    rb.AddForce(new Vector2(1*MathF.Sign(direcao.x) * forcaKnockback, 0), ForceMode2D.Impulse);
                }
            }
            else if (tipo == 2)
            {
                if (textoTeste)
                {
                    int num = int.Parse(textoTeste.text) - dano;
                    textoTeste.text = num.ToString();
                }
                Debug.Log("Acertou ataque aereo");

                if (knockback)
                {
                    Vector2 direcao = transform.position - atacante.transform.position;
                    direcao.Normalize();
                    rb.AddForce(new Vector2(1*MathF.Sign(direcao.x) * forcaKnockback, 0), ForceMode2D.Impulse);
                }
            }
        }
    }

    public void LevaAtaqueEstocada(int tipo, int dano, bool knockback, float forcaKnockback, GameObject atacante)
    {
        if (atingivelBase)
        {
            if (tipo == 1)
            {
                if (textoTeste)
                {
                    int num = int.Parse(textoTeste.text) - dano;
                    textoTeste.text = num.ToString();
                }
                Debug.Log("Acertou ataque no chao");

                if (knockback)
                {
                    Vector2 direcao = transform.position - atacante.transform.position;
                    direcao.Normalize();
                    rb.AddForce(new Vector2(1*MathF.Sign(direcao.x) * forcaKnockback, 0), ForceMode2D.Impulse);
                }
            }
            else if (tipo == 2)
            {
                if (textoTeste)
                {
                    int num = int.Parse(textoTeste.text) - dano;
                    textoTeste.text = num.ToString();
                }
                Debug.Log("Acertou ataque aereo");

                if (knockback)
                {
                    Vector2 direcao = transform.position - atacante.transform.position;
                    direcao.Normalize();
                    rb.AddForce(new Vector2(1*MathF.Sign(direcao.x) * forcaKnockback, 0), ForceMode2D.Impulse);
                }
            }
        }
    }


}
