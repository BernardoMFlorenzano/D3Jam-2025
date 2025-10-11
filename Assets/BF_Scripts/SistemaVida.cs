using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class SistemaVida : MonoBehaviour
{
    public bool atingivelBase;
    private Rigidbody2D rb;
    [SerializeField] private float tempoRecupDano;
    private Coroutine CorRecupDano;
    public bool sofrendoKnockback;
    public bool recupDano;
    public bool agindo; // Se estiver agindo, ignora função de repelir

    [Header("Vida")]
    [SerializeField] private int vidaMax;
    private int vidaAtual;
    private bool morreu;
    [Header("Player")]
    [SerializeField] private float tempoInvenc; // tempo de invencibilidade
    private bool podeLevarDano;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        vidaAtual = vidaMax;
        podeLevarDano = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void MataInimigo()
    {
        Debug.Log("Inimigo Morreu");
        Destroy(gameObject, 0.5f);
    }

    public void LevaAtaqueCorte(int tipo, int dano, bool knockback, float forcaKnockback, GameObject atacante)
    {
        if (atingivelBase && !CompareTag("Player"))
        {
            if (tipo == 1)
            {
                Debug.Log("Acertou ataque no chao");

                vidaAtual -= dano;

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
                Debug.Log("Acertou ataque aereo");

                vidaAtual = vidaAtual - dano;
                
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

            if (vidaAtual <= 0)
            {
                MataInimigo();
            }
        }
    }

    public void LevaAtaqueEstocada(int tipo, int dano, bool knockback, float forcaKnockback, GameObject atacante)
    {
        if (atingivelBase && !CompareTag("Player"))
        {
            Debug.Log("Leva estocada");
            if (tipo == 1)
            {
                Debug.Log("Acertou ataque no chao");

                vidaAtual -= dano;

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
                Debug.Log("Acertou ataque aereo");
                
                vidaAtual -= dano;

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

            if (vidaAtual <= 0)
            {
                MataInimigo();
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

    // Player
    public void LevaAtaquePlayer(int dano, bool knockback, float forcaKnockback, GameObject atacante)   // Teoricamente já checou se acerta pela base
    {
        if (podeLevarDano && CompareTag("Player"))
        {
            podeLevarDano = false;
            vidaAtual -= dano;

            if (knockback)
            {
                Vector2 direcao = transform.position - atacante.transform.position;
                direcao.Normalize();

                //sofrendoKnockback = true;
                rb.linearVelocity = Vector2.zero;
                rb.AddForce(new Vector2(MathF.Sign(direcao.x) * forcaKnockback, 0), ForceMode2D.Impulse);
            }

            Debug.Log("Vida do player: " + vidaAtual);

            StartCoroutine(Invencibilidade());
        }
    }
    
    IEnumerator Invencibilidade()
    {
        // ativa animação de dano
        yield return new WaitForSeconds(tempoInvenc);
        // desativa animação de dano
        podeLevarDano = true;
    }


}
