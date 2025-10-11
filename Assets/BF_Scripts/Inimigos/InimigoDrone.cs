using System;
using System.Collections;
using UnityEngine;

public class InimigoDrone : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float velInimigoHor;
    [SerializeField] private float velInimigoVer;
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
    [SerializeField] private float rangeMaxMovimentoPatrulha = 1f;
    [SerializeField] private float tempoParadoMaxPatrulha = 2f;
    [SerializeField] private float tempoParadoMinPatrulha = 1f;
    [SerializeField] private float velInimigoHorPatrulha;
    [SerializeField] private float velInimigoVerPatrulha;
    private bool patrulhando;
    private bool andandoEmPatrulha;
    private bool delayAndarEmPatrulha;
    private Coroutine corAndaEmPatrulha;
    private Coroutine corDelayAndaEmPatrulha;

    [Header("Em Combate")]
    [SerializeField] private float distanciaIdeal; // Distancia em que o drone vai tentar chegar perto
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
    [SerializeField] private float tempoParadoPosAtaque;
    [SerializeField] private float velInimigoHorRasante;
    [SerializeField] private float velInimigoVerRasante;
    private Vector2 direcaoRasante;
    [SerializeField] private float descidaRasante;  // Duração da descida
    [SerializeField] private float subidaRasante;   // Duração da subida
    [SerializeField] private float tempoChao;
    private ImpulsoCima impulsoRasante;
    private bool atacando;
    private bool acabouAtaque;
    public bool acabouRasante;
    private Coroutine corCooldownAtaque;
    private Coroutine corTerminaRasante;

    //[Header("Variaveis Externas")]
    private SistemaVida sistemaVida;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        sistemaVida = GetComponent<SistemaVida>();
        impulsoRasante = GetComponent<ImpulsoCima>();

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
        else if (emCombate && !parado)
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
        if (sistemaVida.recupDano && preparandoAtaque)
        {
            if (corPreparandoAtaque != null)
            {
                StopCoroutine(corPreparandoAtaque);
                preparandoAtaque = false;
                podeAtacar = false;
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
            if (distanciaIdeal < distanciaPlayer)
            {
                velMov.x = MathF.Sign(-direcaoPlayer.x);
                velMov.y = MathF.Sign(-direcaoPlayer.y);
            }
            else
            {
                velMov = Vector2.zero;
            }
            /*

            if (distanciaIdealX / 2 < MathF.Abs(transform.position.x - player.transform.position.x) && MathF.Abs(transform.position.y - player.transform.position.y) > 0.2f)
            {
                velMov.y = MathF.Sign(-direcaoPlayer.y);
            }
            else
            {
                velMov.y = 0f;
            }
            */

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
        yield return new WaitForSeconds(tempoPreparaAtaque);

        emCombate = false;
        atacando = true;
        sistemaVida.agindo = true;
        preparandoAtaque = false;
        direcaoRasante = -direcaoPlayer;

        SetaColisor(danoAtaque, knockBackAtaque, forcaKnockbackAtaque);
        StartCoroutine(impulsoRasante.ImpulsoDrone(descidaRasante, subidaRasante, tempoChao));
        acabouRasante = false;
        corTerminaRasante = StartCoroutine(TerminaRasante());
        Debug.Log("Começa Ataque");
    }

    /* -------------------------------BLOCO EMCOMBATE FIM------------------------------- */

    /* -------------------------------BLOCO ATACANDO INICIO------------------------------- */

    void Atacando()
    {
        if (acabouAtaque || sistemaVida.recupDano)
        {
            podeAtacar = false;
            acabouAtaque = false;
            //rb.linearVelocity = Vector2.zero;
            parado = true;

            StopCoroutine(TerminaRasante());    // Para logica de rasante
            SetaColisor(1, false, 0);   // Valor padrão do dano e knockback

            corCooldownAtaque = StartCoroutine(CooldownAtaque());
        }
        else if (!parado)
        {
            rb.linearVelocityX = direcaoRasante.x * velInimigoHorRasante;
            rb.linearVelocityY = direcaoRasante.y * velInimigoVerRasante;
        }
        else if (parado)
        {
            rb.linearVelocity = rb.linearVelocity * 0.95f;
        }
    }

    IEnumerator TerminaRasante()
    {
        yield return new WaitUntil(() => acabouRasante);
        acabouAtaque = true;
        SetaColisor(1, false, 0);   // Valor padrão do dano e knockback
        Debug.Log("Acaba Ataque");
    }

    IEnumerator CooldownAtaque()
    {
        StartCoroutine(ParadoPosRasante());
        yield return new WaitForSeconds(cooldownAtaque);
        podeAtacar = true;
        Debug.Log("Pode Atacar de novo");
    }

    IEnumerator ParadoPosRasante()
    {
        Debug.Log("Recuperando apos ataque");
        yield return new WaitForSeconds(tempoParadoPosAtaque);
        parado = false;
        atacando = false;
        emCombate = true;
        sistemaVida.agindo = false;
    }

    IEnumerator CooldownAtaqueInterrompido()
    {
        yield return new WaitForSeconds(cooldownAtaque);
        podeAtacar = true;
        Debug.Log("Pode Atacar de novo");
    }

    void SetaColisor(int dano, bool knockback, float forcaKnockback)
    {
        colisorCorpo.dano = dano;
        colisorCorpo.knockback = knockback;
        colisorCorpo.forcaKnockback = forcaKnockback;
    }
    
    /* -------------------------------BLOCO ATACANDO FIM------------------------------- */
}