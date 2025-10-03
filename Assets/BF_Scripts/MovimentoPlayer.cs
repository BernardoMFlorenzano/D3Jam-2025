using System.Collections;
using UnityEngine;

public class MovimentoPlayer : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float velPlayerHor;
    [SerializeField] private float velPlayerVer;
    private Vector2 direcaoInput;
    private Vector2 velMovReal;
    private Vector2 velMovTarget;
    private bool virado; // Controla flip do movimento

    [Header("Pulo")]
    [SerializeField] private float duracaoPulo;
    [SerializeField] private float duracaoQueda;
    [SerializeField] private float alturaPulo;
    private ImpulsoCima impulsoPulo;
    private bool puloInput;
    public bool estaNoChao;

    [Header("Ataque")]
    [SerializeField] private GameObject rangeCorpoPadrao;
    [SerializeField] private GameObject rangeCorpoLanca;
    //[SerializeField] private GameObject rangeBase;
    [SerializeField] private int armaAtual; // Define qual arma o player está usando atualmente
    private ColisorCorpo colisorCorpoPadrao;
    private ColisorCorpo colisorCorpoLanca;
    public bool agindo;
    private bool podeAtacar;
    [SerializeField] private float delayAtaque;
    private bool ataqueInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        impulsoPulo = GetComponent<ImpulsoCima>();
        estaNoChao = true;
        virado = false;
        rangeCorpoPadrao.SetActive(false);
        rangeCorpoLanca.SetActive(false);
        colisorCorpoPadrao = rangeCorpoPadrao.GetComponent<ColisorCorpo>();
        colisorCorpoLanca = rangeCorpoLanca.GetComponent<ColisorCorpo>();
        agindo = false;
        podeAtacar = true;
        //rangeBase.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (agindo)   // Não deixa player dar input
            return;

        if (estaNoChao)
        {
            direcaoInput.x = Input.GetAxisRaw("Horizontal");
            direcaoInput.y = Input.GetAxisRaw("Vertical");
            if (direcaoInput.x < 0 && !virado)
                Flip();
            else if (direcaoInput.x > 0 && virado)
                Flip();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            puloInput = true;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            ataqueInput = true;
        }
    }

    void FixedUpdate()
    {
        if (ataqueInput)
        {
            ataqueInput = false;
            AtaqueNormal();
        }

        if (agindo && estaNoChao)
        {
            if (direcaoInput != Vector2.zero)
                rb.linearVelocity = Vector2.zero;
            return;
        }

        direcaoInput.Normalize();

        velMovTarget.x = direcaoInput.x * velPlayerHor;
        velMovTarget.y = direcaoInput.y * velPlayerVer;

        rb.linearVelocity = velMovTarget;

        if (puloInput && estaNoChao)
        {
            StartCoroutine(impulsoPulo.Impulso(alturaPulo, duracaoPulo, duracaoQueda));
            puloInput = false;
            estaNoChao = false;
        }
    }

    void Flip()
    {
        virado = !virado;
        transform.Rotate(0, 180, 0); // roda em 180 para objeto flipar
    }

    void AtaqueNormal()
    {
        if (podeAtacar)
        {
            Debug.Log("Atacou");
            podeAtacar = false;
            agindo = true;
            StartCoroutine(CorrotinaDelayAtaque());
            if (estaNoChao)
                StartCoroutine(CorrotinaAtaqueChao());
            else
                StartCoroutine(CorrotinaAtaqueAr());
        }
    }

    IEnumerator CorrotinaDelayAtaque()
    {
        yield return new WaitForSeconds(delayAtaque);
        podeAtacar = true;
    }

    IEnumerator CorrotinaAtaqueChao()
    {
        //rangeBase.enabled = true;
        if (armaAtual == 0)
        {
            colisorCorpoPadrao.tipoAtaque = 1;
            rangeCorpoPadrao.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            rangeCorpoPadrao.SetActive(false);
        }
        else if (armaAtual == 1)
        {
            colisorCorpoLanca.tipoAtaque = 1;
            rangeCorpoLanca.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            rangeCorpoLanca.SetActive(false);
        }


        
        //yield return new WaitForSeconds(0.4f);
        agindo = false;
    }

    IEnumerator CorrotinaAtaqueAr()
    {
        //rangeBase.enabled = true;
        if (armaAtual == 0)
        {
            colisorCorpoPadrao.tipoAtaque = 2;
            rangeCorpoPadrao.SetActive(true);
        }
        else if (armaAtual == 1)
        {
            colisorCorpoLanca.tipoAtaque = 2;
            rangeCorpoLanca.SetActive(true);
        }

        /*
        while (!estaNoChao)
        {
            yield return new WaitForSeconds(0.05f);
        }
        */

        yield return new WaitUntil(() => estaNoChao || !agindo);

        rangeCorpoPadrao.SetActive(false);
        rangeCorpoLanca.SetActive(false);

        yield return new WaitForSeconds(0.5f);
        agindo = false;
    }
}
