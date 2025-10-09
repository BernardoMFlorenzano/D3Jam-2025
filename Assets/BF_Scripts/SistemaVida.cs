using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class SistemaVida : MonoBehaviour
{
    public bool atingivelBase;
    [SerializeField] private TMP_Text textoTeste;
    private Rigidbody2D rb;
    [SerializeField] private float tempoRecupDano;
    private Coroutine CorRecupDano;
    public bool sofrendoKnockback;
    public bool recupDano;
    public bool agindo; // Se estiver agindo, ignora função de repelir

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
    /*

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

                sofrendoKnockback = true;
                rb.linearVelocity = Vector2.zero;
                rb.AddForce(new Vector2(direcao.x * 200, 0), ForceMode2D.Impulse);
            }
        }
    }
    */

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

                recupDano = true;
                if (CorRecupDano != null)
                    StopCoroutine(CorRecupDano);
                CorRecupDano = StartCoroutine(DelayRecupDano());

                if (knockback)
                {
                    Vector2 direcao = transform.position - atacante.transform.position;
                    direcao.Normalize();

                    sofrendoKnockback = true;
                    rb.linearVelocity = Vector2.zero;
                    rb.AddForce(new Vector2(1 * MathF.Sign(direcao.x) * forcaKnockback, 0), ForceMode2D.Impulse);
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

                recupDano = true;
                if (CorRecupDano != null)
                    StopCoroutine(CorRecupDano);
                CorRecupDano = StartCoroutine(DelayRecupDano());

                if (knockback)
                {
                    Vector2 direcao = transform.position - atacante.transform.position;
                    direcao.Normalize();

                    sofrendoKnockback = true;
                    rb.linearVelocity = Vector2.zero;
                    rb.AddForce(new Vector2(1 * MathF.Sign(direcao.x) * forcaKnockback, 0), ForceMode2D.Impulse);
                }
            }
        }
    }

    public void LevaAtaqueEstocada(int tipo, int dano, bool knockback, float forcaKnockback, GameObject atacante)
    {
        if (atingivelBase)
        {
            Debug.Log("Leva estocada");
            if (tipo == 1)
            {
                if (textoTeste)
                {
                    int num = int.Parse(textoTeste.text) - dano;
                    textoTeste.text = num.ToString();
                }
                Debug.Log("Acertou ataque no chao");

                recupDano = true;
                if (CorRecupDano != null)
                    StopCoroutine(CorRecupDano);
                CorRecupDano = StartCoroutine(DelayRecupDano());

                if (knockback)
                {
                    Vector2 direcao = transform.position - atacante.transform.position;
                    direcao.Normalize();

                    sofrendoKnockback = true;
                    rb.linearVelocity = Vector2.zero;
                    rb.AddForce(new Vector2(1 * MathF.Sign(direcao.x) * forcaKnockback, 0), ForceMode2D.Impulse);
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

                recupDano = true;
                if (CorRecupDano != null)
                    StopCoroutine(CorRecupDano);
                CorRecupDano = StartCoroutine(DelayRecupDano());

                if (knockback)
                {
                    Vector2 direcao = transform.position - atacante.transform.position;
                    direcao.Normalize();

                    sofrendoKnockback = true;
                    rb.linearVelocity = Vector2.zero;
                    rb.AddForce(new Vector2(1 * MathF.Sign(direcao.x) * forcaKnockback, 0), ForceMode2D.Impulse);
                }
            }
        }
    }

    IEnumerator DelayRecupDano()
    {
        yield return new WaitForSeconds(tempoRecupDano);
        recupDano = false;
    }

    public void RepeleObjeto(Vector2 direcao, float forca)
    {
        if (!sofrendoKnockback && !recupDano && !agindo)
            rb.AddForce(new Vector2(MathF.Sign(-direcao.x), MathF.Sign(-direcao.y)) * forca);
    }


}
