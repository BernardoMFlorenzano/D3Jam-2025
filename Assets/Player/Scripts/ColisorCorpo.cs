using UnityEngine;

public class ColisorCorpo : MonoBehaviour
{
    public int condicaoAtaque;  // Vai ser setado a condiçao do ataque assim que o collider for ativado; 1 = ataque chao, 2 = ataque aereo, 
    public int modoAtaque;  // modo que está sendo usada
    public int dano;
    public bool knockback;
    public float forcaKnockback;
    public bool shake;
    public float forcaShake;
    public float forcaImpulsoCorteAr;
    public float forcaImpulsoEstocAr;
    private SistemaVida sistemaVida;
    [SerializeField] private GameObject gameObjectPrincipal;
    private ImpulsoCima impulsoCima;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        impulsoCima = gameObjectPrincipal.GetComponent<ImpulsoCima>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CorpoInimigo"))
        {
            sistemaVida = collision.GetComponentInParent<SistemaVida>();
            sistemaVida.LevaAtaqueInimigo(condicaoAtaque, modoAtaque, dano, knockback, forcaKnockback, shake, forcaShake, forcaImpulsoCorteAr, forcaImpulsoEstocAr, gameObjectPrincipal);
        }
    }
}
