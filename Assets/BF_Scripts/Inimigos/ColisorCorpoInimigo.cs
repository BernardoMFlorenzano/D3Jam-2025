using UnityEngine;

public class ColisorCorpoInimigo : MonoBehaviour
{
    public int dano;
    public bool knockback;
    public float forcaKnockback;
    public bool atingePelaBase;
    private SistemaVida sistemaVida;
    [SerializeField] private GameObject gameObjectPrincipal;
    [SerializeField] private SistemaVida sistemaVidaInimigo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        atingePelaBase = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CorpoPlayer") && atingePelaBase && !sistemaVidaInimigo.morreu && !sistemaVidaInimigo.recupDano)
        {
            // Da dano no player
            //Debug.Log("Player leva dano");
            sistemaVida = collision.GetComponentInParent<SistemaVida>();
            sistemaVida.LevaAtaquePlayer(dano, knockback, forcaKnockback, gameObjectPrincipal);
        }
    }

    // Tira getcomponent do triggerstay

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("CorpoPlayer") && atingePelaBase && !sistemaVidaInimigo.morreu && !sistemaVidaInimigo.recupDano)
        {
            // Da dano no player
            if(sistemaVida)
                sistemaVida.LevaAtaquePlayer(dano, knockback, forcaKnockback, gameObjectPrincipal);
        }
    }
}