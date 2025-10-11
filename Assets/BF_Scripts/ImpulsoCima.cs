using System.Collections;
using UnityEngine;

public class ImpulsoCima: MonoBehaviour
{
    private MovimentoPlayer movimentoPlayer;
    private InimigoDrone inimigoDrone;
    [SerializeField] private Transform transformCorpo;
    public bool subindo;
    public bool caindo;
    //[SerializeField] private string layerPlayerNome;
    //[SerializeField] private string layerInimigoNome;
    //private int layerPlayer;
    //private int layerInimigo;
    [SerializeField] private Animator animatorPlayer;
    [SerializeField] private Animator animatorDrone;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (CompareTag("Player"))
            movimentoPlayer = GetComponent<MovimentoPlayer>();
        else if (CompareTag("InimigoVoador"))
            inimigoDrone = GetComponent<InimigoDrone>();

        //layerPlayer = LayerMask.NameToLayer(layerPlayerNome);
        //layerInimigo = LayerMask.NameToLayer(layerInimigoNome);
        subindo = false;
        caindo = false;
    }

    public IEnumerator Impulso(float altura, float duracaoSubida, float duracaoDescida) // Logica de layer collision está desativada
    {
        //Debug.Log("Indo pra cima");
        if (CompareTag("Player"))
            movimentoPlayer.estaNoChao = false;

        //Physics2D.IgnoreLayerCollision(layerPlayer, layerInimigo, true);

        Vector2 posicaoInicial = transformCorpo.localPosition;
        Vector2 posicaoPico = new Vector2(posicaoInicial.x, posicaoInicial.y + altura);

        subindo = true;
        caindo = false;

        // Animacoes
        if (animatorPlayer)
        {
            animatorPlayer.SetBool("Pulando", subindo);
            animatorPlayer.SetBool("Caindo", caindo);
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
        if (animatorPlayer)
        {
            animatorPlayer.SetBool("Pulando", subindo);
            animatorPlayer.SetBool("Caindo", caindo);
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
        //Physics2D.IgnoreLayerCollision(layerPlayer, layerInimigo, false);
        //Debug.Log("Caiu");

        // Animacoes
        if (animatorPlayer)
        {
            animatorPlayer.SetBool("Pulando", subindo);
            animatorPlayer.SetBool("Caindo", caindo);
        }
    }
    
    public IEnumerator ImpulsoDrone(float duracaoDescida, float duracaoSubida)
    {
        // inimigoDrone.acabouRasante já é falso

        Vector2 posicaoInicial = transformCorpo.localPosition;
        Vector2 posicaoBase = Vector2.zero;

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

        yield return new WaitForSeconds(0.1f);

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
