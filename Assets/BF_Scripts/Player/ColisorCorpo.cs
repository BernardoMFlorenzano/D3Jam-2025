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
    private SistemaVida sistemaVida;
    [SerializeField] private GameObject gameObjectPrincipal;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

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
            if (modoAtaque == 1)
            {
                sistemaVida.LevaAtaqueCorte(condicaoAtaque, dano, knockback, forcaKnockback, shake, forcaShake, gameObjectPrincipal);
            }
            else if (modoAtaque == 2)
            {
                //Debug.Log("Tenta dar dano de estocada");
                sistemaVida.LevaAtaqueEstocada(condicaoAtaque, dano, knockback, forcaKnockback, shake, forcaShake, gameObjectPrincipal);
            }
        }
    }
}
