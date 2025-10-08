using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class InimigoSerra : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float velInimigoHor;
    [SerializeField] private float velInimigoVer;
    private Vector2 direcaoPlayer;
    private float distanciaPlayer;
    private Vector2 direcao;
    private Vector2 velMov;
    private bool virado; // Controla flip do movimento
    private GameObject player;
    //private Vector2 posPlayer;
    private bool patrulhando;
    private bool andandoEmPatrulha;
    private bool delayAndarEmPatrulha;
    private bool emCombate;
    private bool viuPlayer;
    private bool atacando;
    [Header("Visão")]
    [SerializeField] private float visaoDetecta = 5f;
    [Header("Patrulha")]
    [SerializeField] private float rangeMinMovimentoPatrulha = 0.5f;
    [SerializeField] private float rangeMaxMovimentoPatrulha = 2f;
    [SerializeField] private float tempoParadoMaxPatrulha = 4f;
    [SerializeField] private float tempoParadoMinPatrulha = 2f;
    [SerializeField] private float velInimigoHorPatrulha;
    [SerializeField] private float velInimigoVerPatrulha;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>(); 

        patrulhando = true;
        andandoEmPatrulha = false;
        delayAndarEmPatrulha = false;

        emCombate = false;
        viuPlayer = false;

        atacando = false;

        virado = false;
    }

    void Update()
    {
        if (direcao.x < 0 && !virado)
            Flip();
        else if (direcao.x > 0 && virado)
            Flip();
    }

    void Flip()
    {
        virado = !virado;
        transform.Rotate(0, 180, 0); // roda em 180 para objeto flipar
    }

    void FixedUpdate()
    {
        direcaoPlayer = (transform.position - player.transform.position).normalized;
        distanciaPlayer = Vector2.Distance(transform.position, player.transform.position);

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

    void Patrulha()
    {
        if (distanciaPlayer < visaoDetecta && !viuPlayer)
        {
            viuPlayer = true;
            emCombate = true;

            // Para toda lógica de patrulha
            patrulhando = false;
            StopAllCoroutines();
            andandoEmPatrulha = false;
            delayAndarEmPatrulha = false;
        }
        else
        {
            if (!andandoEmPatrulha && !delayAndarEmPatrulha)
            {
                andandoEmPatrulha = true;
                direcao = Random.insideUnitCircle.normalized;
                velMov.x = direcao.x * velInimigoHorPatrulha;
                velMov.y = direcao.y * velInimigoVerPatrulha;
                StartCoroutine(AndaEmPatrulha());
            }
            else if (andandoEmPatrulha)
                rb.linearVelocity = velMov;
        }


    }

    IEnumerator AndaEmPatrulha()
    {
        Debug.Log("Anda");
        yield return new WaitForSeconds(Random.Range(rangeMinMovimentoPatrulha, rangeMaxMovimentoPatrulha));
        rb.linearVelocity = Vector2.zero;
        andandoEmPatrulha = false;
        delayAndarEmPatrulha = true;
        Debug.Log("Para de andar");
        StartCoroutine(DelayAndarEmPatrulha());
    }

    IEnumerator DelayAndarEmPatrulha()
    {
        yield return new WaitForSeconds(Random.Range(tempoParadoMinPatrulha, tempoParadoMaxPatrulha));
        delayAndarEmPatrulha = false;
    }

    void EmCombate()
    {

    }

    void Atacando()
    {
        
    }
}
