using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovimentoPlayer : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float velPlayerHor;
    [SerializeField] private float velPlayerVer;
    private Vector2 direcaoInput;
    //private Vector2 velMovReal;
    private Vector2 velMovTarget;
    private bool virado; // Controla flip do movimento
    private SistemaVida sistemaVida;
    [Header("Animações")]
    [SerializeField] private Animator animatorPlayer;
    [SerializeField] private Animator animatorEfeitoAtaque;
    [SerializeField] private SetaEfeitoAtaque efeitoAtaque;

    [Header("Pulo")]
    [SerializeField] private float duracaoPulo;
    [SerializeField] private float duracaoQueda;
    [SerializeField] private float alturaPulo;
    [SerializeField] private float delayPosPulo;
    private ImpulsoCima impulsoPulo;
    private bool puloInput;
    public bool estaNoChao;
    private Vector2 direcaoPulo;

    [Header("Ataque")]
    [SerializeField] private GameObject rangeCorpo;
    [SerializeField] private GameObject rangeBase;
    [SerializeField] private int armaAtual; // Define qual arma o player está usando atualmente
    private ColisorCorpo colisorCorpo;
    private BoxCollider2D boxCorpo;
    private BoxCollider2D boxBase;
    public bool agindo;
    private bool podeAtacar;
    [SerializeField] private float delayAtaquePadrao;
    private bool ataqueInput1;
    private bool ataqueInput2;
    [Header("Lança")]
    [SerializeField] private Vector2 offsetBoxEstocada;
    [SerializeField] private Vector2 sizeBoxEstocada;
    [SerializeField] private int indexEstocada = 2;
    [Header("Espada")]
    [SerializeField] private Vector2 offsetBoxCorte;
    [SerializeField] private Vector2 sizeBoxCorte;
    [SerializeField] private int indexCorte = 1;
    [Header("Corte Ar")]
    [SerializeField] private Vector2 offsetBoxCorteAr;
    [SerializeField] private Vector2 sizeBoxCorteAr;
    [Header("Estocada Ar")]
    [SerializeField] private Vector2 offsetBoxEstocadaAr;
    [SerializeField] private Vector2 sizeBoxEstocadaAr;
    [SerializeField] private Vector2 offsetBoxEstocArChao;
    [SerializeField] private Vector2 sizeBoxEstocArChao;

    [Header("Combos")]
    [SerializeField] private bool podeEntrarCombo;
    [SerializeField] private float delayCancelaCombo = 2f;
    [SerializeField] private Ataque cortePadrao;
    [SerializeField] private Ataque estocPadrao;
    [SerializeField] private Ataque corteArPadrao;
    [SerializeField] private Ataque estocArPadrao;
    [SerializeField] private List<Ataque> combo1;
    [SerializeField] private List<Ataque> combo2;   // Quando adicionar combos vai ter que atualizar o código, não consegui deixar isso dinamico
    [SerializeField] private List<Ataque> combo3;
    [SerializeField] private List<Ataque> combo4;
    private List<int> comboEfetuado = new List<int>();
    private bool estaEmCombo;
    //private bool comboFinalizado = false;
    //private bool acabouCombo;
    private int comboCount;
    private Coroutine comboTimerCorrotina;
    private Coroutine delayAtaqueCorrotina;
    //[SerializeField] private float delayTrocaArma;
    //private bool podeTrocarArma = true;
    [Header("Sons")]
    [SerializeField] private AudioClip grama1;
    [SerializeField] private AudioClip grama2;
    [SerializeField] private AudioClip grama3;
    [SerializeField] private float tempoSomGrama = 1f;
    [SerializeField] private float volumeSomGrama = 1f;
    private bool delaySomGrama = false;
    [Header("Pausa")]
    [SerializeField] private Pausa pauseManager;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        impulsoPulo = GetComponent<ImpulsoCima>();
        estaNoChao = true;
        virado = false;
        rangeCorpo.SetActive(false);
        colisorCorpo = rangeCorpo.GetComponent<ColisorCorpo>();

        boxCorpo = rangeCorpo.GetComponent<BoxCollider2D>();
        boxBase = rangeBase.GetComponent<BoxCollider2D>();

        sistemaVida = GetComponent<SistemaVida>();

        agindo = false;
        podeAtacar = true;
        podeEntrarCombo = true;
        estaEmCombo = false;
        //acabouCombo = false;
        comboCount = 0;

        //SetarHitBox();

    }

    public void OnJump()
    {
        puloInput = true;
    }

    public void OnCorte()
    {
        ataqueInput1 = true;
        ataqueInput2 = false;
    }

    public void OnEstocada()
    {
        ataqueInput2 = true;
        ataqueInput1 = false;
    }

    public void OnPausa()
    {
        pauseManager.PausaJogo();
    }

    // Update is called once per frame
    void Update()
    {
        if (agindo)   // Não deixa player dar input
            return;

    
        direcaoInput.x = Input.GetAxisRaw("Horizontal");
        direcaoInput.y = Input.GetAxisRaw("Vertical");
        if (direcaoInput.x < 0 && !virado)
            Flip();
        else if (direcaoInput.x > 0 && virado)
            Flip();

        if (estaEmCombo && direcaoInput.magnitude > 0.1f)
        {
            ResetarCombo();
        }
    }

    void FixedUpdate()
    {
        if (ataqueInput1)
        {
            AtaqueNormal(indexCorte);
        }
        if (ataqueInput2)
        {
            AtaqueNormal(indexEstocada);
        }

        if (agindo && estaNoChao || sistemaVida.levandoDano)
        {
            if (direcaoInput != Vector2.zero && !sistemaVida.levandoDano)
                rb.linearVelocity = Vector2.zero;
            return;
        }

        direcaoInput.Normalize();

        if (estaNoChao)
        {
            velMovTarget.x = direcaoInput.x * velPlayerHor;
            velMovTarget.y = direcaoInput.y * velPlayerVer;

            rb.linearVelocity = velMovTarget;
        }
        else
        {
            velMovTarget.x = direcaoPulo.x * velPlayerHor;
            velMovTarget.y = direcaoPulo.y * velPlayerVer;

            rb.linearVelocity = velMovTarget;
        }
        
        if (direcaoInput.magnitude > 0.1f && rb.linearVelocity.magnitude > 0.1f && estaNoChao)
        {
            SomGrama();
        }

        if (puloInput && estaNoChao)
        {
            direcaoPulo = direcaoInput;
            StartCoroutine(impulsoPulo.Impulso(alturaPulo, duracaoPulo, duracaoQueda));
            puloInput = false;
            estaNoChao = false;
            animatorPlayer.SetBool("Pulando", true);
        }

        // Animacoes
        if (animatorPlayer)
        {
            //animatorPlayer.SetFloat("Vel", Mathf.Abs(rb.linearVelocityX) + Mathf.Abs(rb.linearVelocityY));
            animatorPlayer.SetFloat("Vel", rb.linearVelocity.magnitude);
            animatorPlayer.SetBool("NoChao", estaNoChao);
        }

    }

    void SomGrama()
    {
        if (!delaySomGrama)
        {
            delaySomGrama = true;
            int escolha = Random.Range(0, 3);
            if (escolha == 0)
                AudioManager.instance.PlaySFX(grama1, volumeSomGrama);
            else if (escolha == 1)
                AudioManager.instance.PlaySFX(grama2, volumeSomGrama);
            else
                AudioManager.instance.PlaySFX(grama3, volumeSomGrama);
            StartCoroutine(DelaySomGrama());
        }
    }
    
    IEnumerator DelaySomGrama()
    {
        yield return new WaitForSeconds(tempoSomGrama);
        delaySomGrama = false;
    }

    void Flip()
    {
        virado = !virado;
        transform.Rotate(0, 180, 0); // roda em 180 para objeto flipar
    }

    void AtaqueNormal(int ataqueModo)
    {
        if (podeAtacar)
        {
            if (ataqueInput1)
            {
                ataqueInput1 = false;
            }
            if (ataqueInput2)
            {
                ataqueInput2 = false;
            }
            Debug.Log("Atacou");
            podeAtacar = false;
            agindo = true;

            if (estaNoChao)
            {
                StartCoroutine(CorrotinaAtaqueChao(ataqueModo));
                if (delayAtaqueCorrotina != null)
                    StopCoroutine(delayAtaqueCorrotina);
                delayAtaqueCorrotina = StartCoroutine(CorrotinaDelayAtaque(delayAtaquePadrao));
            }
            else
            {
                if (ataqueModo == 1 || !impulsoPulo.subindo)
                    StartCoroutine(CorrotinaAtaqueAr(ataqueModo));
                else
                {
                    podeAtacar = true;
                    agindo = false;
                }
            }
        }
    }

    IEnumerator CorrotinaDelayAtaque(float delayAtaque)
    {
        yield return new WaitForSeconds(delayAtaque);
        agindo = false;
        podeAtacar = true;
    }

    IEnumerator CorrotinaAtaqueChao(int ataqueModo)
    {
        if (!estaEmCombo)
        {
            if (ataqueModo == 1)
            {
                SetarColisorVars(1, ataqueModo, cortePadrao.dano, cortePadrao.knockback, cortePadrao.forcaKnockback, cortePadrao.shake, cortePadrao.forcaShake);
                SetarHitBox(sizeBoxCorte, offsetBoxCorte);
                animatorPlayer.SetTrigger("Corte");
                animatorEfeitoAtaque.SetTrigger("Corte");
                efeitoAtaque.SetarEfeitoPosEscala(cortePadrao.posEfeito, cortePadrao.escalaEfeito, cortePadrao.invertido, Vector3.zero);
                AudioManager.instance.PlaySFX(cortePadrao.efeitoSlash, 1f);
            }
            else if (ataqueModo == 2)
            {
                SetarColisorVars(1, ataqueModo, estocPadrao.dano, estocPadrao.knockback, estocPadrao.forcaKnockback, estocPadrao.shake, estocPadrao.forcaShake);
                SetarHitBox(sizeBoxEstocada, offsetBoxEstocada);
                animatorPlayer.SetTrigger("Estocada");
                animatorEfeitoAtaque.SetTrigger("Estocada");
                efeitoAtaque.SetarEfeitoPosEscala(estocPadrao.posEfeito, estocPadrao.escalaEfeito, estocPadrao.invertido, Vector3.zero);
                AudioManager.instance.PlaySFX(estocPadrao.efeitoSlash, 1f);
            }

            if (podeEntrarCombo)
            {
                comboEfetuado.Clear();
                comboCount = 1;
                estaEmCombo = true;
                comboEfetuado.Add(ataqueModo);
                //acabouCombo = false;
            }

            rangeCorpo.SetActive(false);
            rangeCorpo.SetActive(true);


            yield return new WaitForSeconds(0.1f);
            rangeCorpo.SetActive(false);
            //agindo = false;

            if (podeEntrarCombo)
            {
                podeEntrarCombo = false;
                if(comboTimerCorrotina != null)
                    StopCoroutine(comboTimerCorrotina);
                comboTimerCorrotina = StartCoroutine(TimerCombo());
            }
        }
        else if (estaEmCombo)
        {
            if (comboTimerCorrotina != null)
            {
                StopCoroutine(comboTimerCorrotina);
            }

            comboEfetuado.Add(ataqueModo);
            bool continuouCombo = false; // Pra saber se um novo timer deve ser iniciado
            bool finalizouCombo = false;    

            // Ordem dos combos vai servir como a prioridade: menor = maior prioridade

            // Combo 1
            if (comboCount < combo1.Count && comboEfetuado[comboCount] == combo1[comboCount].modoAtaque && comboEfetuado[comboCount - 1] == combo1[comboCount - 1].modoAtaque)
            {
                SetarColisorVars(1, ataqueModo, combo1[comboCount].dano, combo1[comboCount].knockback, combo1[comboCount].forcaKnockback, combo1[comboCount].shake, combo1[comboCount].forcaShake);
                SetarHitBox(combo1[comboCount].boxSize, combo1[comboCount].boxOffset);
                efeitoAtaque.SetarEfeitoPosEscala(combo1[comboCount].posEfeito, combo1[comboCount].escalaEfeito, combo1[comboCount].invertido, Vector3.zero);
                AudioManager.instance.PlaySFX(combo1[comboCount].efeitoSlash, 1f);
                comboCount++;
                continuouCombo = true;
                if (comboCount >= combo1.Count)
                {
                    finalizouCombo = true;
                    StartCoroutine(FinalizarCombo(0.5f)); // finaliza o combo
                }
            }
            // Combo 2
            else if (comboCount < combo2.Count && comboEfetuado[comboCount] == combo2[comboCount].modoAtaque && comboEfetuado[comboCount - 1] == combo2[comboCount - 1].modoAtaque)
            {
                SetarColisorVars(1, ataqueModo, combo2[comboCount].dano, combo2[comboCount].knockback, combo2[comboCount].forcaKnockback, combo2[comboCount].shake, combo2[comboCount].forcaShake);
                SetarHitBox(combo2[comboCount].boxSize, combo2[comboCount].boxOffset);
                efeitoAtaque.SetarEfeitoPosEscala(combo2[comboCount].posEfeito, combo2[comboCount].escalaEfeito, combo2[comboCount].invertido, Vector3.zero);
                AudioManager.instance.PlaySFX(combo2[comboCount].efeitoSlash, 1f);
                comboCount++;
                continuouCombo = true;
                if (comboCount >= combo2.Count)
                {
                    finalizouCombo = true;
                    StartCoroutine(FinalizarCombo(0.5f)); // finaliza o combo
                }
            }
            // Combo 3
            else if (comboCount < combo3.Count && comboEfetuado[comboCount] == combo3[comboCount].modoAtaque && comboEfetuado[comboCount - 1] == combo3[comboCount - 1].modoAtaque)
            {
                SetarColisorVars(1, ataqueModo, combo3[comboCount].dano, combo3[comboCount].knockback, combo3[comboCount].forcaKnockback, combo3[comboCount].shake, combo3[comboCount].forcaShake);
                SetarHitBox(combo3[comboCount].boxSize, combo3[comboCount].boxOffset);
                efeitoAtaque.SetarEfeitoPosEscala(combo3[comboCount].posEfeito, combo3[comboCount].escalaEfeito, combo3[comboCount].invertido, Vector3.zero);
                AudioManager.instance.PlaySFX(combo3[comboCount].efeitoSlash, 1f);
                comboCount++;
                continuouCombo = true;
                if (comboCount >= combo3.Count)
                {
                    finalizouCombo = true;
                    StartCoroutine(FinalizarCombo(0.5f)); // finaliza o combo
                }
            }
            // Combo 4
            else if (comboCount < combo4.Count && comboEfetuado[comboCount] == combo4[comboCount].modoAtaque && comboEfetuado[comboCount - 1] == combo4[comboCount - 1].modoAtaque)
            {
                SetarColisorVars(1, ataqueModo, combo4[comboCount].dano, combo4[comboCount].knockback, combo4[comboCount].forcaKnockback, combo4[comboCount].shake, combo4[comboCount].forcaShake);
                SetarHitBox(combo4[comboCount].boxSize, combo4[comboCount].boxOffset);
                efeitoAtaque.SetarEfeitoPosEscala(combo4[comboCount].posEfeito, combo4[comboCount].escalaEfeito, combo4[comboCount].invertido, Vector3.zero);
                AudioManager.instance.PlaySFX(combo4[comboCount].efeitoSlash, 1f);
                comboCount++;
                continuouCombo = true;
                if (comboCount >= combo4.Count)
                {
                    finalizouCombo = true;
                    StartCoroutine(FinalizarCombo(0.5f)); // finaliza o combo
                }
            }

            // Mais combos adicionar aqui, acho que deve funcionar só mudando aqui mesmo eu espero


            // Não achou combo, então ataca normalmente e acaba combo
            else
            {
                SetarColisorVars(1, ataqueModo, 1, false, 0, false, 0);
                if (ataqueModo == 1)
                {
                    SetarHitBox(sizeBoxCorte, offsetBoxCorte);   // Hitbox padrão
                    efeitoAtaque.SetarEfeitoPosEscala(cortePadrao.posEfeito, cortePadrao.escalaEfeito, cortePadrao.invertido, Vector3.zero);
                    AudioManager.instance.PlaySFX(cortePadrao.efeitoSlash, 1f);
                }
                else if (ataqueModo == 2)
                {
                    SetarHitBox(sizeBoxEstocada, offsetBoxEstocada);
                    efeitoAtaque.SetarEfeitoPosEscala(estocPadrao.posEfeito, estocPadrao.escalaEfeito, cortePadrao.invertido, Vector3.zero);
                    AudioManager.instance.PlaySFX(estocPadrao.efeitoSlash, 1f);
                }
                ResetarCombo(); // Quebrou o combo
            }

            // Executa para todos os ataques dentro do combo
            yield return new WaitForSeconds(0.05f);
            rangeCorpo.SetActive(false);
            rangeCorpo.SetActive(true);
            if (ataqueModo == 1)
            {
                animatorPlayer.SetTrigger("Corte");
                animatorEfeitoAtaque.SetTrigger("Corte");
            }
            else if (ataqueModo == 2)
            {
                animatorPlayer.SetTrigger("Estocada");
                animatorEfeitoAtaque.SetTrigger("Estocada");
            }

            yield return new WaitForSeconds(0.1f);
            rangeCorpo.SetActive(false);
            agindo = false;

            // Se o combo continuou reiniciar o timer
            if (continuouCombo && !finalizouCombo)
            {
                comboTimerCorrotina = StartCoroutine(TimerCombo());
            }

        }
    }

    IEnumerator TimerCombo()
    {
        yield return new WaitForSeconds(delayCancelaCombo);
        if (!agindo && podeAtacar)
        {
            Debug.Log("Combo feito: " + string.Join(", ", comboEfetuado));
            ResetarCombo();
        }
    }

    IEnumerator FinalizarCombo(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("Combo feito: " + string.Join(", ", comboEfetuado));
        ResetarCombo();
    }

    void ResetarCombo()
    {
        estaEmCombo = false;
        podeEntrarCombo = true;
        //acabouCombo = false;
        comboEfetuado.Clear();
        comboCount = 0;

        if (comboTimerCorrotina != null)
        {
            StopCoroutine(comboTimerCorrotina);
            comboTimerCorrotina = null; // Limpa a referência
        }
    }

    IEnumerator CorrotinaAtaqueAr(int ataqueModo)
    {
        //rangeBase.enabled = true;
        
        if (ataqueModo == 1)
        {
            SetarHitBox(sizeBoxCorteAr, offsetBoxCorteAr); 
            SetarColisorVars(2, ataqueModo, corteArPadrao.dano, corteArPadrao.knockback, corteArPadrao.forcaKnockback, corteArPadrao.shake, corteArPadrao.forcaShake); // ataque no ar
            //animatorPlayer.SetTrigger("Corte");
            animatorEfeitoAtaque.SetTrigger("Corte");
            efeitoAtaque.SetarEfeitoPosEscala(corteArPadrao.posEfeito, corteArPadrao.escalaEfeito, corteArPadrao.invertido, Vector3.zero);
            AudioManager.instance.PlaySFX(corteArPadrao.efeitoSlash, 1f);
        }
        else if (ataqueModo == 2)
        {
            SetarHitBox(sizeBoxEstocadaAr, offsetBoxEstocadaAr);
            SetarColisorVars(2, ataqueModo, estocArPadrao.dano, estocArPadrao.knockback, estocArPadrao.forcaKnockback, estocArPadrao.shake, estocArPadrao.forcaShake); // ataque no ar

            // Box do chao é diferente
            boxBase.size = sizeBoxEstocArChao;
            boxBase.offset = offsetBoxEstocArChao;

            //animatorPlayer.SetTrigger("Corte");
            animatorEfeitoAtaque.SetTrigger("Estocada");
            efeitoAtaque.SetarEfeitoPosEscala(estocArPadrao.posEfeito, estocArPadrao.escalaEfeito, estocArPadrao.invertido, new Vector3(0, 0, -90f));
            AudioManager.instance.PlaySFX(estocArPadrao.efeitoSlash, 1f);
        }
        rangeCorpo.SetActive(false);
        rangeCorpo.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        rangeCorpo.SetActive(false);

        yield return new WaitUntil(() => estaNoChao || !agindo || sistemaVida.levandoDano);
        yield return new WaitForSeconds(delayPosPulo);
        agindo = false;
        podeAtacar = true;
    }

    public void SetarHitBox(Vector2 size, Vector2 offset)
    {
        boxBase.size = size;
        boxCorpo.size = size;
        boxBase.offset = offset;
        boxCorpo.offset = offset;
        
    }

    public void SetarColisorVars(int condicaoAtaque, int ataqueModo, int dano, bool knockback, float forcaKnockback, bool shake, float forcaShake)
    {
        colisorCorpo.condicaoAtaque = condicaoAtaque;    // ataque no chao
        colisorCorpo.modoAtaque = ataqueModo;   // corte ou estocada
        colisorCorpo.dano = dano;
        colisorCorpo.knockback = knockback;
        colisorCorpo.forcaKnockback = forcaKnockback;
        colisorCorpo.shake = shake;
        colisorCorpo.forcaShake = forcaShake;
    }
}
