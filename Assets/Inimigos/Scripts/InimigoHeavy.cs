using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class InimigoHeavy : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float velInimigoHor;
    [SerializeField] private float velInimigoVer;
    [SerializeField] private Animator animatorHeavy;
    private Vector2 direcaoPlayer;
    private float distanciaPlayer;
    private Vector3 posicaoPlayerLida;
    private Vector2 direcao;
    private Vector2 velMov;
    private bool virado; // Controla flip do movimento
    private GameObject player;
    [SerializeField] private ColisorCorpoInimigo colisorCorpo;
    //private Vector2 posPlayer;
    [Header("Visão")]
    [SerializeField] private float visaoDetecta = 5f;

    [Header("Patrulha")]
    [SerializeField] private float rangeMinMovimentoPatrulha = 0.5f;
    [SerializeField] private float rangeMaxMovimentoPatrulha = 2f;
    [SerializeField] private float tempoParadoMaxPatrulha = 4f;
    [SerializeField] private float tempoParadoMinPatrulha = 2f;
    [SerializeField] private float velInimigoHorPatrulha;
    [SerializeField] private float velInimigoVerPatrulha;
    private bool patrulhando;
    private bool andandoEmPatrulha;
    private bool delayAndarEmPatrulha;
    private Coroutine corAndaEmPatrulha;
    private Coroutine corDelayAndaEmPatrulha;

    [Header("Em Combate")]
    [SerializeField] private float distanciaIdealX; // Distancia horizontal que o inimigo vai tentar manter
    [SerializeField] private float tempoPreparaAtaque;  // Pequeno delay até iniciar ataque
    [SerializeField] private float danoPassivo;  // Dano no player ao tocar o inimigo fora do modo ataque
    [SerializeField] private float tempoDeRespostaInimigo;  // Delay na atualização da posição e direção do player
    public bool podeTentarAtacar; // Colisor de range de ataque está vendo player ou não
    private bool podeAtacar; // Será falso se estiver em cooldown ou outro fator impeça ele de atacar
    private bool parado;    // verdade se estiver parado apos ataque
    private bool emCombate;
    private bool viuPlayer;
    private bool podeAtualizarInput;
    private bool preparandoAtaque;
    private Coroutine corPreparandoAtaque;

    [Header("Atacando")]
    [SerializeField] private int danoAtaque;  // Dano no player ao tocar o inimigo durante ataque
    [SerializeField] private bool knockBackAtaque;
    [SerializeField] private float forcaKnockbackAtaque;
    [SerializeField] private Vector2 offsetBoxDano;
    [SerializeField] private Vector2 sizeBoxDano;
    [SerializeField] private Vector2 offsetBoxRangeAtaque;
    [SerializeField] private Vector2 sizeBoxRangeAtaque;
    [SerializeField] private float cooldownAtaque;
    [SerializeField] private float cooldownAtaqueInterrompido;
    [SerializeField] private float tempoParadoPosAtaque;
    private bool atacando;
    private Coroutine corCooldownAtaque;

    [Header("Projetil")]
    [SerializeField] private Transform transformTiroPoint;
    [SerializeField] private float forcaLancaTiro;
    [SerializeField] private float forcaImpulsoTiro;
    [SerializeField] private GameObject projetilPrefab;
    private Vector2 direcaoTiro;
    private GameObject projetil;
    private Rigidbody2D rbProjetil;
    private ImpulsoCima impulsoProjetil;

    [Header("Sons")]
    [SerializeField] private AudioClip somProjetil;
    [SerializeField] private float somProjetilMult = 1f;
    private VolumeController volumeController;


    //[Header("Variaveis Externas")]
    private SistemaVida sistemaVida;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        sistemaVida = GetComponent<SistemaVida>();

        volumeController = GameObject.FindGameObjectWithTag("GameManager").GetComponentInChildren<VolumeController>();

        patrulhando = true;
        andandoEmPatrulha = false;
        delayAndarEmPatrulha = false;

        emCombate = false;
        viuPlayer = false;
        podeTentarAtacar = false;
        podeAtacar = true;
        parado = false;
        podeAtualizarInput = true;
        preparandoAtaque = false;

        atacando = false;

        virado = false;



        //volumeController.sfxSlider.onValueChanged.AddListener(delegate { SetVolume(); });
    }

    void Update()
    {
        if (patrulhando)
        {
            if (direcao.x < 0 && !virado)
                Flip();
            else if (direcao.x > 0 && virado)
                Flip();
        }
        else if (emCombate && !parado && !atacando && !preparandoAtaque && !sistemaVida.morreu)
        {
            if (player.transform.position.x < transform.position.x && !virado)
            {
                Flip();
            }
            else if (player.transform.position.x > transform.position.x && virado)
            {
                Flip();
            }
        }
    }

    void Flip()
    {
        virado = !virado;
        transform.Rotate(0, 180, 0); // roda em 180 para objeto flipar
    }

    void FixedUpdate()
    {


        if (podeAtualizarInput)
        {
            posicaoPlayerLida = player.transform.position;
            podeAtualizarInput = false;
            StartCoroutine(DelayRespostaInimigo());
        }

        direcaoPlayer = (transform.position - posicaoPlayerLida).normalized;
        distanciaPlayer = Vector2.Distance(transform.position, posicaoPlayerLida);

        if (!sistemaVida.morreu)
        {
            if (patrulhando)
            {
                Patrulha();
            }
            else if (emCombate)
            {
                EmCombate();
            }
            else if (atacando)
            {
                Atacando();
            }
        }
        else 
        {
            // Não faz nada
            //Morto();
        }

        // Animacoes
        if (animatorHeavy && !sistemaVida.morreu)
        {
            animatorHeavy.SetFloat("Vel", Mathf.Abs(rb.linearVelocityX) + Mathf.Abs(rb.linearVelocityY));
        }

    }

    IEnumerator DelayRespostaInimigo()
    {
        yield return new WaitForSeconds(tempoDeRespostaInimigo);
        podeAtualizarInput = true;
    }

    /* -------------------------------BLOCO PATRULHA INICIO------------------------------- */

    void Patrulha()
    {
        //Debug.Log("entra em Patrulha()");
        if (distanciaPlayer < visaoDetecta && !viuPlayer)
        {
            viuPlayer = true;
            emCombate = true;

            // Para toda lógica de patrulha
            patrulhando = false;
            if (corAndaEmPatrulha != null)
                StopCoroutine(corAndaEmPatrulha);
            if (corDelayAndaEmPatrulha != null)
                StopCoroutine(corDelayAndaEmPatrulha);

            //podeAtualizarInput = true; // teste

            andandoEmPatrulha = false;
            delayAndarEmPatrulha = false;
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            if (!andandoEmPatrulha && !delayAndarEmPatrulha)
            {
                andandoEmPatrulha = true;
                direcao = UnityEngine.Random.insideUnitCircle.normalized;
                velMov.x = direcao.x * velInimigoHorPatrulha;
                velMov.y = direcao.y * velInimigoVerPatrulha;
                corAndaEmPatrulha = StartCoroutine(AndaEmPatrulha());
            }
            else if (andandoEmPatrulha)
                rb.linearVelocity = velMov;
        }


    }

    IEnumerator AndaEmPatrulha()
    {
        Debug.Log("Anda");
        yield return new WaitForSeconds(UnityEngine.Random.Range(rangeMinMovimentoPatrulha, rangeMaxMovimentoPatrulha));
        rb.linearVelocity = Vector2.zero;
        andandoEmPatrulha = false;
        delayAndarEmPatrulha = true;
        Debug.Log("Para de andar");
        corDelayAndaEmPatrulha = StartCoroutine(DelayAndarEmPatrulha());
    }

    IEnumerator DelayAndarEmPatrulha()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(tempoParadoMinPatrulha, tempoParadoMaxPatrulha));
        delayAndarEmPatrulha = false;
    }

    /* -------------------------------BLOCO PATRULHA FIM------------------------------- */

    /* -------------------------------BLOCO EMCOMBATE INICIO------------------------------- */

    void EmCombate()
    {
        if ((sistemaVida.recupDano || sistemaVida.morreu) && preparandoAtaque)
        {
            if (corPreparandoAtaque != null)
            {
                StopCoroutine(corPreparandoAtaque);
                preparandoAtaque = false;
                podeAtacar = false;
                animatorHeavy.SetBool("Preparando", false);
                corCooldownAtaque = StartCoroutine(CooldownAtaqueInterrompido());
            }
        }
        if (podeTentarAtacar && podeAtacar)
        {
            if (!preparandoAtaque)
            {
                preparandoAtaque = true;
                corPreparandoAtaque = StartCoroutine(PreparaAtaque());
            }
        }

        else if (!sistemaVida.sofrendoKnockback && !sistemaVida.recupDano && !preparandoAtaque)
        {
            if (distanciaIdealX < MathF.Abs(transform.position.x - player.transform.position.x))
            {
                velMov.x = MathF.Sign(-direcaoPlayer.x);
            }
            else if (distanciaIdealX / 2 > MathF.Abs(transform.position.x - player.transform.position.x))
            {
                velMov.x = MathF.Sign(direcaoPlayer.x);
            }
            else
            {
                velMov.x = 0f;
            }

            if (distanciaIdealX / 2 < MathF.Abs(transform.position.x - player.transform.position.x) && MathF.Abs(transform.position.y - player.transform.position.y) > 0.2f)
            {
                velMov.y = MathF.Sign(-direcaoPlayer.y);
            }
            else
            {
                velMov.y = 0f;
            }
            rb.AddForce(new Vector2(velMov.x * 10 * velInimigoHor, velMov.y * 10 * velInimigoVer));
        }
        else if (sistemaVida.sofrendoKnockback)
        {
            if (rb.linearVelocity == Vector2.zero)
            {
                sistemaVida.sofrendoKnockback = false;
            }
        }
    }

    IEnumerator PreparaAtaque()
    {
        rb.linearVelocity = Vector2.zero;
        direcaoTiro = -direcaoPlayer;
        animatorHeavy.SetBool("Preparando", true);
        yield return new WaitForSeconds(tempoPreparaAtaque);
        animatorHeavy.SetBool("Preparando", false);

        animatorHeavy.SetTrigger("Atirou");
        //emCombate = false;
        //atacando = true;
        sistemaVida.agindo = true;
        preparandoAtaque = false;
        podeAtacar = false;

    

        if (!sistemaVida.morreu) // Depois tem que arrumar pra parar todo a logica do inimigo 
        {
            AudioManager.instance.PlaySFX(somProjetil, somProjetilMult);
        }

        AtiraProjetil(-direcaoPlayer);


        
        corCooldownAtaque = StartCoroutine(CooldownAtaque());
        //Debug.Log("Começa Ataque");
    }

    /* -------------------------------BLOCO EMCOMBATE FIM------------------------------- */

    /* -------------------------------BLOCO ATACANDO INICIO------------------------------- */

    void Atacando()
    {
        
    }

    void AtiraProjetil(Vector2 direcao)
    {
        projetil = Instantiate(projetilPrefab, new Vector2(transformTiroPoint.position.x, transform.position.y), Quaternion.identity);
        rbProjetil = projetil.GetComponent<Rigidbody2D>();
        impulsoProjetil = projetil.GetComponent<ImpulsoCima>();

        rbProjetil.AddForce(direcao * forcaLancaTiro,ForceMode2D.Impulse);
        impulsoProjetil.ForcaPraCima(forcaImpulsoTiro);
        impulsoProjetil.SetaPosicaoY(transformTiroPoint.localPosition.y);

        projetil.GetComponent<DestroiObjeto>().podeDestruir = true;
    }


    IEnumerator CooldownAtaque()
    {
        StartCoroutine(ParadoPosAtaque());
        yield return new WaitForSeconds(cooldownAtaque);
        podeAtacar = true;
        //Debug.Log("Pode Atacar de novo");
    }

    IEnumerator ParadoPosAtaque()
    {
        Debug.Log("Recuperando apos ataque");
        yield return new WaitForSeconds(tempoParadoPosAtaque);
        parado = false;
        //atacando = false;
        emCombate = true;
        sistemaVida.agindo = false;
    }

    IEnumerator CooldownAtaqueInterrompido()
    {
        yield return new WaitForSeconds(cooldownAtaqueInterrompido);
        podeAtacar = true;
        //Debug.Log("Pode Atacar de novo");
    }

    void SetaColisor(int dano, bool knockback, float forcaKnockback)
    {
        colisorCorpo.dano = dano;
        colisorCorpo.knockback = knockback;
        colisorCorpo.forcaKnockback = forcaKnockback;
    }
    
    /* -------------------------------BLOCO ATACANDO FIM------------------------------- */
}
