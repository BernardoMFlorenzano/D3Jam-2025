using UnityEngine;

public class RepeleInimigo : MonoBehaviour
{
    [SerializeField] float forcaRepulsa;
    private SistemaVida sistemaVida;
    [SerializeField] private SistemaVida sistemaVidaSelf;
    private float distancia;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.CompareTag("Inimigo") && transform.parent.CompareTag("Inimigo") || collision.CompareTag("InimigoVoador") && transform.parent.CompareTag("InimigoVoador")) && !sistemaVidaSelf.morreu)
        {
            sistemaVida = collision.GetComponent<SistemaVida>();
            distancia = Vector2.Distance(transform.position, collision.transform.position);
            sistemaVida.RepeleObjeto((transform.position - collision.transform.position).normalized, forcaRepulsa/distancia);
        }
    }
}
