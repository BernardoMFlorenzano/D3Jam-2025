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

    [Header("Pulo")]
    [SerializeField] private Transform transformCorpo;
    [SerializeField] private float duracaoPulo;
    [SerializeField] private float duracaoQueda;
    [SerializeField] private float alturaPulo;
    private bool puloInput;
    private bool subindo;
    private bool caindo;
    private bool estaNoChao;
    [SerializeField] private string layerPlayerNome;
    [SerializeField] private string layerInimigoNome;
    private int layerPlayer;
    private int layerInimigo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        layerPlayer = LayerMask.NameToLayer(layerPlayerNome);
        layerInimigo = LayerMask.NameToLayer(layerInimigoNome);
        estaNoChao = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (estaNoChao)
        {
            direcaoInput.x = Input.GetAxisRaw("Horizontal");
            direcaoInput.y = Input.GetAxisRaw("Vertical");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            puloInput = true;
        }
    }

    void FixedUpdate()
    {
        direcaoInput.Normalize();

        velMovTarget.x = direcaoInput.x * velPlayerHor;
        velMovTarget.y = direcaoInput.y * velPlayerVer;

        rb.linearVelocity = velMovTarget;

        if (puloInput && estaNoChao)
        {
            //StartCoroutine(ImpulsoCima(alturaPulo, velPulo, velQueda));
            StartCoroutine(ImpulsoCima(alturaPulo, duracaoPulo, duracaoQueda));
            puloInput = false;
            estaNoChao = false;
        }
    }

    
    public IEnumerator ImpulsoCima(float altura, float duracaoSubida, float duracaoDescida)
    {
        Debug.Log("Indo pra cima");
        estaNoChao = false;
        Debug.Log(layerInimigo);
        Physics2D.IgnoreLayerCollision(layerPlayer, layerInimigo, true);

        Vector2 posicaoInicial = transformCorpo.localPosition;
        Vector2 posicaoPico = new Vector2(posicaoInicial.x, posicaoInicial.y + altura);
        float tempoDecorrido = 0f;

        while (tempoDecorrido < duracaoSubida)
        {
            float t = tempoDecorrido / duracaoSubida;

            float tSuavizado = Mathf.Sin(t * Mathf.PI * 0.5f);

            transformCorpo.localPosition = Vector2.Lerp(posicaoInicial, posicaoPico, tSuavizado);

            tempoDecorrido += Time.deltaTime;
            yield return null;
        }

        transformCorpo.localPosition = posicaoPico;
        
        yield return new WaitForSeconds(0.1f);

        tempoDecorrido = 0f; 
        while (tempoDecorrido < duracaoDescida)
        {
            float t = tempoDecorrido / duracaoDescida;

            float tSuavizado = 1 - Mathf.Cos(t * Mathf.PI * 0.5f);

            transformCorpo.localPosition = Vector2.Lerp(posicaoPico, posicaoInicial, tSuavizado);

            tempoDecorrido += Time.deltaTime;
            yield return null;
        }

        transformCorpo.localPosition = posicaoInicial;
        estaNoChao = true;
        Physics2D.IgnoreLayerCollision(layerPlayer, layerInimigo, false);
        Debug.Log("Caiu");
    }
}
