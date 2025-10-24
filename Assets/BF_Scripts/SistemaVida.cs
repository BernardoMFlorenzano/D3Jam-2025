using System;
using System.Collections;
using System.Data.Common;

//using System.Numerics;
using TMPro;
using UnityEngine;
//using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
    public bool morreu;
    [Header("Player")]
    [SerializeField] private float tempoInvenc; // tempo de invencibilidade
    [SerializeField] private float tempoInvencPos;  // tempo de invencibilidade apos animação de dano acabar
    public bool levandoDano;
    private bool podeLevarDano;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private AudioClip danoPlayer;
    [SerializeField] private PassaCena gameManager;
    private Slider sliderVida;
    [Header("Inimigos")]
    [SerializeField] private float multKnockback;
    [SerializeField] private RuntimeAnimatorController animatorMorto;
    [SerializeField] private float tempoEfeitoDano;
    [SerializeField] private Transform corpo;
    [SerializeField] private ParticleSystem particulasDano;
    [SerializeField] private float tempoMorto;
    private bool animDano = false;
    private Coroutine corEfeitoDano;
    private Coroutine corTimerEfeitoDano;
    private Coroutine corTimerEfeitoDanoPisca;
    [SerializeField] private AudioClip danoInimigo1;
    [SerializeField] private AudioClip danoInimigo2;
    [SerializeField] private AudioClip danoInimigo3;
    [SerializeField] private float volumeDanoMult;
    [SerializeField] private AudioSource somPassivo;
    private ShakeCamera shakeCamera;

    [Header("Spawner")]
    private SpawnWaves spawner;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<SpawnWaves>();
        shakeCamera = GameObject.FindGameObjectWithTag("CineMachine").GetComponent<ShakeCamera>();
        vidaAtual = vidaMax;
        podeLevarDano = true;
        morreu = false;

        if (CompareTag("Player"))
        {
            sliderVida = GameObject.FindGameObjectWithTag("SliderVida").GetComponent<Slider>();
            sliderVida.value = 1f;
            gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PassaCena>();
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    void MataInimigo()
    {
        StopAllCoroutines();

        corpo.localPosition = new Vector2(0, corpo.localPosition.y);    // Resetar o que as corrotinas cuidariam
        spriteRenderer.enabled = true;
        if (somPassivo)
            somPassivo.enabled = false;

        if (animator)
        {
            animator.runtimeAnimatorController = animatorMorto;
        }
        Debug.Log("Inimigo Morreu");

        // Avisa spawner
        spawner.numInimigos--;

        Destroy(gameObject, tempoMorto);
    }

    void MataPlayer()
    {
        StopAllCoroutines();
        if (animator)
        {
            animator.runtimeAnimatorController = animatorMorto;
        }
        Debug.Log("Player Morreu");

        gameManager.TrocaCena(3);   // Morte
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

                if (knockback)
                {
                    Vector2 direcao = transform.position - atacante.transform.position;
                    direcao.Normalize();

                    sofrendoKnockback = true;
                    rb.linearVelocity = Vector2.zero;
                    rb.AddForce(new Vector2(1 * MathF.Sign(direcao.x) * forcaKnockback * multKnockback, 0), ForceMode2D.Impulse);
                }
            }
            else if (tipo == 2)
            {
                Debug.Log("Acertou ataque aereo");

                vidaAtual -= dano;

                recupDano = true;

                if (knockback)
                {
                    Vector2 direcao = transform.position - atacante.transform.position;
                    direcao.Normalize();

                    sofrendoKnockback = true;
                    rb.linearVelocity = Vector2.zero;
                    rb.AddForce(new Vector2(1 * MathF.Sign(direcao.x) * forcaKnockback * multKnockback, 0), ForceMode2D.Impulse);
                }
            }

            TocaSomHit();
            if (particulasDano)
                CriaParticulasDano();
            if (dano > 1)
            {
                shakeCamera.Shake(2f,0.25f);
            }

            if (vidaAtual <= 0 && !morreu)
            {
                morreu = true;
                MataInimigo();
            }
            else if (!morreu)
            {
                if (CorRecupDano != null)
                    StopCoroutine(CorRecupDano);
                CorRecupDano = StartCoroutine(DelayRecupDano());

                if (corEfeitoDano != null || corTimerEfeitoDano != null || corTimerEfeitoDanoPisca != null)
                {
                    StopCoroutine(corEfeitoDano);
                    StopCoroutine(corTimerEfeitoDano);
                    StopCoroutine(corTimerEfeitoDanoPisca);
                    corpo.localPosition = new Vector2(0, corpo.localPosition.y);
                    spriteRenderer.enabled = true;
                }
                animDano = true;
                corEfeitoDano = StartCoroutine(TimerDanoInimigo());
                corTimerEfeitoDano = StartCoroutine(EfeitoDanoInimigo());
                corTimerEfeitoDanoPisca = StartCoroutine(EfeitoDanoInimigoPisca());
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

                if (knockback)
                {
                    Vector2 direcao = transform.position - atacante.transform.position;
                    direcao.Normalize();

                    sofrendoKnockback = true;
                    rb.linearVelocity = Vector2.zero;
                    rb.AddForce(new Vector2(1 * MathF.Sign(direcao.x) * forcaKnockback * multKnockback, 0), ForceMode2D.Impulse);
                }
            }
            else if (tipo == 2)
            {
                Debug.Log("Acertou ataque aereo");

                vidaAtual -= dano;

                recupDano = true;

                if (knockback)
                {
                    Vector2 direcao = transform.position - atacante.transform.position;
                    direcao.Normalize();

                    sofrendoKnockback = true;
                    rb.linearVelocity = Vector2.zero;
                    rb.AddForce(new Vector2(1 * MathF.Sign(direcao.x) * forcaKnockback * multKnockback, 0), ForceMode2D.Impulse);
                }
            }

            TocaSomHit();
            if (particulasDano)
                CriaParticulasDano();
            if (dano > 1)
            {
                shakeCamera.Shake(2f,0.25f);
            }

            if (vidaAtual <= 0 && !morreu)
            {
                morreu = true;
                MataInimigo();
            }
            else if (!morreu)
            {
                if (CorRecupDano != null)
                    StopCoroutine(CorRecupDano);
                CorRecupDano = StartCoroutine(DelayRecupDano());

                if (corEfeitoDano != null || corTimerEfeitoDano != null || corTimerEfeitoDanoPisca != null)
                {
                    StopCoroutine(corEfeitoDano);
                    StopCoroutine(corTimerEfeitoDano);
                    StopCoroutine(corTimerEfeitoDanoPisca);
                    corpo.localPosition = new Vector2(0, corpo.localPosition.y);
                    spriteRenderer.enabled = true;
                }
                animDano = true;
                corEfeitoDano = StartCoroutine(TimerDanoInimigo());
                corTimerEfeitoDano = StartCoroutine(EfeitoDanoInimigo());
                corTimerEfeitoDanoPisca = StartCoroutine(EfeitoDanoInimigoPisca());
            }
        }
    }

    void TocaSomHit()
    {
        if (CompareTag("Player"))
        {
            AudioManager.instance.PlaySFX(danoPlayer, 1f);
        }
        else
        {
            int escolha = Random.Range(0, 3);
            if (escolha == 0)
                AudioManager.instance.PlaySFX(danoInimigo1, 1f);
            else if (escolha == 1)
                AudioManager.instance.PlaySFX(danoInimigo2, 1f);
            else
                AudioManager.instance.PlaySFX(danoInimigo3, 1f);
        }
    }

    void CriaParticulasDano()
    {
        particulasDano.Play();
    }

    IEnumerator DelayRecupDano()
    {
        if(somPassivo)
            somPassivo.enabled = false;
        if (animator && !morreu)
            animator.SetBool("Dano", true);
        yield return new WaitForSeconds(tempoRecupDano);
        if (animator && !morreu)
            animator.SetBool("Dano", false);
        if (somPassivo && !morreu)
            somPassivo.enabled = true;
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
            levandoDano = true;
            vidaAtual -= dano;
            sliderVida.value -= (float)dano / vidaMax;

            if (knockback)
            {
                Vector2 direcao = transform.position - atacante.transform.position;
                direcao.Normalize();

                //sofrendoKnockback = true;
                rb.linearVelocity = Vector2.zero;
                rb.AddForce(new Vector2(MathF.Sign(direcao.x) * forcaKnockback * multKnockback, 0), ForceMode2D.Impulse);
            }

            Debug.Log("Vida do player: " + vidaAtual);

            TocaSomHit();

            if (vidaAtual <= 0)
            {
                MataPlayer();
            }

            StartCoroutine(Invencibilidade());
        }
    }

    IEnumerator Invencibilidade()
    {
        StartCoroutine(EfeitoInvencibilidade());
        animator.SetBool("Dano", true);
        yield return new WaitForSeconds(tempoInvenc);
        animator.SetBool("Dano", false);
        levandoDano = false;
        yield return new WaitForSeconds(tempoInvencPos);
        podeLevarDano = true;
    }

    IEnumerator EfeitoInvencibilidade()
    {
        spriteRenderer.enabled = false;
        while (podeLevarDano == false)
        {
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.enabled = !spriteRenderer.enabled;
        }
        spriteRenderer.enabled = true;
    }

    IEnumerator EfeitoDanoInimigo()
    {
        Vector2 vetorOriginal = corpo.localPosition;
        Vector2 vetorShake = vetorOriginal;
        vetorShake.x -= 0.05f;
        while (animDano)
        {
            Debug.Log(vetorShake);
            corpo.localPosition = vetorShake;
            vetorShake.x -= 0.1f * MathF.Sign(vetorShake.x);
            yield return new WaitForSeconds(0.05f);
        }
        corpo.localPosition = vetorOriginal;
    }

    IEnumerator EfeitoDanoInimigoPisca()
    {
        spriteRenderer.enabled = false;
        while (animDano)
        {
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.enabled = !spriteRenderer.enabled;
        }
        spriteRenderer.enabled = true;
    }

    IEnumerator TimerDanoInimigo()
    {
        yield return new WaitForSeconds(tempoEfeitoDano);
        animDano = false;
    }

    public void Cura(int cura)
    {
        vidaAtual += cura;
        sliderVida.value += (float)cura / vidaMax;

        if(vidaAtual > vidaMax)
        {
            vidaAtual = vidaMax;
        }
        StartCoroutine(EfeitoCuraPlayer());
    }
    
    IEnumerator EfeitoCuraPlayer()
    {
        Color corEfeito = new Color(0f,1f,0f,1f);
        spriteRenderer.color = corEfeito;
        while (corEfeito.r < 1f)
        {
            yield return new WaitForSeconds(0.1f);
            corEfeito.r += 0.25f;
            corEfeito.b += 0.25f;
            spriteRenderer.color = corEfeito;
        }
        spriteRenderer.color = new Color(1f,1f,1f,1f);
    }
}
