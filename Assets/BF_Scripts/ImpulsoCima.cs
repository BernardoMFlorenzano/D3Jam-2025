using System.Collections;
using UnityEngine;

public class ImpulsoCima: MonoBehaviour
{
    private MovimentoPlayer movimentoPlayer;
    private InimigoDrone inimigoDrone;
    [SerializeField] private Transform transformCorpo;
    public bool subindo;
    public bool caindo;
    [SerializeField] private string layerPlayerNome;
    [SerializeField] private string layerObjetosNome;
    private int layerPlayer;
    private int layerObjetos;
    [SerializeField] private Animator animator;
    [SerializeField] private Animator animatorDrone;
    [SerializeField] private float posChaoDrone = 0.4f;

    private float posicaoYInicial;
    private float velocidadeYAtual = 0f;
    [SerializeField] private float gravidade = 20f;
    //[SerializeField] private float multGravCaindo = 1.33f; // Multiplicador da gravidade se estiver caindo
    [SerializeField] private float capVelocidadeY = 20f;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (CompareTag("Player"))
            movimentoPlayer = GetComponent<MovimentoPlayer>();
        else if (CompareTag("InimigoVoador"))
            inimigoDrone = GetComponent<InimigoDrone>();

        layerPlayer = LayerMask.NameToLayer(layerPlayerNome);
        layerObjetos = LayerMask.NameToLayer(layerObjetosNome);
        subindo = false;
        caindo = false;

        posicaoYInicial = transformCorpo.localPosition.y;
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        if (CompareTag("InimigoVoador"))
            return; // nada

        else 
        {
            if (transformCorpo.localPosition.y > posicaoYInicial)
            {
                velocidadeYAtual += -gravidade * Time.deltaTime;

                if (velocidadeYAtual > 0 && !subindo)
                {
                    subindo = true;
                    caindo = false;

                    if (CompareTag("Player"))
                    {
                        movimentoPlayer.estaNoChao = false;
                        Physics2D.IgnoreLayerCollision(layerPlayer, layerObjetos, true);
                    }
                    
                }
                else if (velocidadeYAtual < 0 && !caindo) 
                {
                    subindo = false;
                    caindo = true;
                }
            }
            else if (transformCorpo.localPosition.y < posicaoYInicial)
            {
                velocidadeYAtual = 0;
                transformCorpo.localPosition = new Vector2(transformCorpo.localPosition.x, posicaoYInicial);

                subindo = false;
                caindo = false;

                if (CompareTag("Player"))
                {
                    movimentoPlayer.estaNoChao = true;
                    Physics2D.IgnoreLayerCollision(layerPlayer, layerObjetos, false);
                }
            }

            
            if (Mathf.Abs(velocidadeYAtual) > 0)
            {
                if (Mathf.Abs(velocidadeYAtual) > capVelocidadeY) // Limite de velocidade
                    velocidadeYAtual = capVelocidadeY * Mathf.Sign(velocidadeYAtual);

                transformCorpo.localPosition = new Vector2(transformCorpo.localPosition.x, transformCorpo.localPosition.y + velocidadeYAtual * Time.deltaTime);
            }

            if (animator)
            {
                animator.SetBool("Pulando", subindo);
                animator.SetBool("Caindo", caindo);
            }
        }
    }

    public void ForcaPraCima(float velocidade)
    {
        if (!CompareTag("InimigoVoador"))
            velocidadeYAtual = velocidade;
    }


    public IEnumerator Impulso(float altura, float duracaoSubida, float duracaoDescida) // Logica de layer collision está desativada
    {
        //Debug.Log("Indo pra cima");
        if (CompareTag("Player"))
            movimentoPlayer.estaNoChao = false;

        Physics2D.IgnoreLayerCollision(layerPlayer, layerObjetos, true);

        Vector2 posicaoInicial = transformCorpo.localPosition;
        Vector2 posicaoPico = new Vector2(posicaoInicial.x, posicaoInicial.y + altura);

        subindo = true;
        caindo = false;

        // Animacoes
        if (animator)
        {
            animator.SetBool("Pulando", subindo);
            animator.SetBool("Caindo", caindo);
        }


        float tempoDecorrido = 0f;
        while (tempoDecorrido < duracaoSubida)
        {
            //Physics2D.IgnoreLayerCollision(layerPlayer, layerInimigo, true);    // Gambiarra pra corrotinas não influenciarem uma outra
            float t = tempoDecorrido / duracaoSubida;

            float tSuavizado = Mathf.Sin(t * Mathf.PI * 0.5f);

            transformCorpo.localPosition = Vector2.Lerp(posicaoInicial, posicaoPico, tSuavizado);

            tempoDecorrido += Time.deltaTime;
            yield return null;
        }

        transformCorpo.localPosition = posicaoPico;

        yield return new WaitForSeconds(0.1f);

        subindo = false;
        caindo = true;

        // Animacoes
        if (animator)
        {
            animator.SetBool("Pulando", subindo);
            animator.SetBool("Caindo", caindo);
        }

        tempoDecorrido = 0f;
        while (tempoDecorrido < duracaoDescida)
        {
            //Physics2D.IgnoreLayerCollision(layerPlayer, layerInimigo, true);    // Gambiarra pra corrotinas não influenciarem uma outra
            float t = tempoDecorrido / duracaoDescida;

            float tSuavizado = 1 - Mathf.Cos(t * Mathf.PI * 0.5f);

            transformCorpo.localPosition = Vector2.Lerp(posicaoPico, posicaoInicial, tSuavizado);

            tempoDecorrido += Time.deltaTime;
            yield return null;
        }

        transformCorpo.localPosition = posicaoInicial;

        if (CompareTag("Player"))
            movimentoPlayer.estaNoChao = true;

        subindo = false;
        caindo = false;
        Physics2D.IgnoreLayerCollision(layerPlayer, layerObjetos, false);
        //Debug.Log("Caiu");

        // Animacoes
        if (animator)
        {
            animator.SetBool("Pulando", subindo);
            animator.SetBool("Caindo", caindo);
        }
    }
    
    public IEnumerator ImpulsoDrone(float duracaoDescida, float duracaoSubida, float tempoChao)
    {
        // inimigoDrone.acabouRasante já é falso

        Vector2 posicaoInicial = transformCorpo.localPosition;
        Vector2 posicaoBase = new Vector2(0, posChaoDrone);

        subindo = false;
        caindo = true;

        // Animacoes
        /*
        if (animatorDrone)
        {
            animatorPlayer.SetBool("Pulando", subindo);
            animatorPlayer.SetBool("Caindo", caindo);
        }
        */

        float tempoDecorrido = 0f;
        while (tempoDecorrido < duracaoDescida)
        {
            float t = tempoDecorrido / duracaoDescida;

            float tSuavizado = 1 - Mathf.Cos(t * Mathf.PI * 0.5f);

            transformCorpo.localPosition = Vector2.Lerp(posicaoInicial, posicaoBase, tSuavizado);

            tempoDecorrido += Time.deltaTime;
            yield return null;
        }

        transformCorpo.localPosition = posicaoBase;

        yield return new WaitForSeconds(tempoChao);

        subindo = true;
        caindo = false;

        // Animacoes
        /*
        if (animatorDrone)
        {
            animatorPlayer.SetBool("Pulando", subindo);
            animatorPlayer.SetBool("Caindo", caindo);
        }
        */

        tempoDecorrido = 0f;
        while (tempoDecorrido < duracaoSubida)
        {
            float t = tempoDecorrido / duracaoSubida;

            float tSuavizado = Mathf.Sin(t * Mathf.PI * 0.5f);

            transformCorpo.localPosition = Vector2.Lerp(posicaoBase, posicaoInicial, tSuavizado);

            tempoDecorrido += Time.deltaTime;
            yield return null;
        }

        transformCorpo.localPosition = posicaoInicial;

        subindo = false;
        caindo = false;

        // Animacoes
        /*
        if (animatorDrone)
        {
            animatorPlayer.SetBool("Pulando", subindo);
            animatorPlayer.SetBool("Caindo", caindo);
        }
        */

        inimigoDrone.acabouRasante = true;
    }
}
