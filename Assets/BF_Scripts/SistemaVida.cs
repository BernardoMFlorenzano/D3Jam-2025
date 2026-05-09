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
    [SerializeField] private float vidaMax;
    private float vidaAtual;
    public bool morreu;
    [Header("LifeSteal")]
    public bool lifeStealAtivo = false;
    [SerializeField] private float multCuraLifeSteal;
    [Header("Player")]
    [SerializeField] private float tempoInvenc; // tempo de invencibilidade
    [SerializeField] private float tempoInvencPos;  // tempo de invencibilidade apos animação de dano acabar
    public bool levandoDano;
    private bool podeLevarDano;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private AudioClip danoPlayer;
    [SerializeField] private PassaCena gameManager;
    private MovimentoPlayer movimentoPlayer;
    private Slider sliderVida;
    [Header("Inimigos")]
    [SerializeField] private float barraPoderFill = 0.05f;
    [SerializeField] private float multKnockback;
    [SerializeField] private RuntimeAnimatorController animatorMorto;
    [SerializeField] private float tempoEfeitoDano;
    [SerializeField] private Transform corpo;
    [SerializeField] private ParticleSystem particulasDano;
    [SerializeField] private float tempoMorto;
    [SerializeField] private int nivelAtaqueStun = 0;   // Com que ataques o inimigo leva stun (0 == todos, 1 == pesados) obs: ataques pesados são o que causam shake da camera
    private bool animDano = false;
    private Coroutine corEfeitoDano;
    private Coroutine corTimerEfeitoDano;
    private Coroutine corTimerEfeitoDanoPisca;
    [SerializeField] private AudioClip danoInimigo1;
    [SerializeField] private AudioClip danoInimigo2;
    [SerializeField] private AudioClip danoInimigo3;
    [SerializeField] private float volumeDanoMult;
    [SerializeField] private AudioSource somPassivo;
    private SistemaVida sistemaVidaPlayer;
    private ShakeCamera shakeCamera;
    private float qntEstocArTomadas = 0;

    [Header("Movimento Vertical")]
    [SerializeField] private float impulsoCimaVel = 0f; // Velocidade pra cima causado por ataques de knockback 
    private ImpulsoCima impulsoCima; 

    [Header("Spawner")]
    private SpawnWaves spawner;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        impulsoCima = GetComponent<ImpulsoCima>();
        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<SpawnWaves>();
        shakeCamera = GameObject.FindGameObjectWithTag("CineMachine").GetComponent<ShakeCamera>();
        vidaAtual = vidaMax;
        podeLevarDano = true;
        morreu = false;

        if (CompareTag("Player"))
        {
            movimentoPlayer = GetComponent<MovimentoPlayer>();
            sliderVida = GameObject.FindGameObjectWithTag("SliderVida").GetComponent<Slider>();
            sliderVida.value = 1f;
            gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PassaCena>();
        }
        else
        {
            StartCoroutine(ResetaKnockbackMod());
            sistemaVidaPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<SistemaVida>();
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

        StartCoroutine(DesapareceInimigo());
    }

    IEnumerator DesapareceInimigo()
    {
        yield return new WaitForSeconds(tempoMorto);
        Color corEfeito = new Color(1f, 1f, 1f, 1f);
        while(corEfeito.a > 0f)
        {
            corEfeito.a -= 0.1f;
            spriteRenderer.color = corEfeito;
            yield return new WaitForSeconds(0.05f);
        }
        Destroy(gameObject);
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

    public void LevaAtaqueInimigo(int condicaoAtaque, int modoAtaque, int dano, bool knockback, float forcaKnockback, bool shake, float forcaShake, float forcaImpulsoCorteAr, float forcaImpulsoEstocAr, GameObject atacante)
    {
        if (atingivelBase && !CompareTag("Player"))
        {
            Debug.Log("Acertou ataque");

            vidaAtual -= dano;

            if (nivelAtaqueStun == 0 || (dano > 1 && nivelAtaqueStun == 1))
                recupDano = true;

            if (knockback && (nivelAtaqueStun == 0 || (dano > 1 && nivelAtaqueStun == 1)))
            {
                Vector2 direcao = transform.position - atacante.transform.position;
                direcao.Normalize();

                sofrendoKnockback = true;
                rb.linearVelocity = Vector2.zero;
                rb.AddForce(new Vector2(1 * MathF.Sign(direcao.x) * forcaKnockback * multKnockback, 0), ForceMode2D.Impulse);
                
                impulsoCima.ForcaPraCima(impulsoCimaVel * forcaKnockback/300);
            }

            TocaSomHit();
            if (particulasDano)
                CriaParticulasDano();
                
            if (shake)
            {
                shakeCamera.Shake(forcaShake,0.25f);
            }

            if (vidaAtual <= 0 && !morreu)
            {
                morreu = true;
                MataInimigo();
            }
            else if (!morreu)
            {
                if (nivelAtaqueStun == 0 || (dano > 1 && nivelAtaqueStun == 1))
                {
                    if (CorRecupDano != null)
                        StopCoroutine(CorRecupDano);
                    CorRecupDano = StartCoroutine(DelayRecupDano());
                }
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

            // Impulso Ar player
            
            if (condicaoAtaque == 2)
            {
                if (modoAtaque == 1)
                    atacante.GetComponent<ImpulsoCima>().ForcaPraCima(forcaImpulsoCorteAr);
                else
                {
                    atacante.GetComponent<ImpulsoCima>().ForcaPraCima(forcaImpulsoEstocAr / (1 + qntEstocArTomadas/5));
                    Debug.Log(forcaImpulsoEstocAr / (1 + qntEstocArTomadas/10));
                    qntEstocArTomadas += 1;
                }  
            }

            sistemaVidaPlayer.LifeSteal(dano, barraPoderFill);
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

    IEnumerator ResetaKnockbackMod()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (qntEstocArTomadas > 0) 
                qntEstocArTomadas -= 1;
            else if (qntEstocArTomadas < 0)
                qntEstocArTomadas = 0;
            //Debug.Log(qntEstocArTomadas);
            yield return null;
        }
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
            sliderVida.value -= dano / vidaMax;

            if (knockback)
            {
                Vector2 direcao = transform.position - atacante.transform.position;
                direcao.Normalize();

                //sofrendoKnockback = true;
                rb.linearVelocity = Vector2.zero;
                rb.AddForce(new Vector2(MathF.Sign(direcao.x) * forcaKnockback * multKnockback, 0), ForceMode2D.Impulse);

                impulsoCima.ForcaPraCima(impulsoCimaVel);
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
            //Debug.Log(vetorShake);
            corpo.localPosition = new Vector2(vetorShake.x, corpo.localPosition.y);
            vetorShake.x -= 0.1f * MathF.Sign(vetorShake.x);
            yield return new WaitForSeconds(0.05f);
        }
        corpo.localPosition = new Vector2(vetorOriginal.x, corpo.localPosition.y);
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

    public void LifeSteal(float cura, float poderFill)
    {
        if (lifeStealAtivo)
        {
            Debug.Log(multCuraLifeSteal);
            Cura(cura * multCuraLifeSteal);
        }
        else
        {
            movimentoPlayer.FillBarraPoder(poderFill);
        }
    }

    public void Cura(float cura)
    {
        Debug.Log("Curou " + cura);
        vidaAtual += cura;
        sliderVida.value += cura / vidaMax;

        if(vidaAtual > vidaMax)
        {
            vidaAtual = vidaMax;
            sliderVida.value = 1f;
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
