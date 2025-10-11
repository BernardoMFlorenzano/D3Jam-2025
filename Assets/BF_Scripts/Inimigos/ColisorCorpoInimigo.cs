using UnityEngine;

public class ColisorCorpoInimigo : MonoBehaviour
{
    public int dano;
    public bool knockback;
    public float forcaKnockback;
    public bool atingePelaBase;
    private SistemaVida sistemaVida;
    [SerializeField] private GameObject gameObjectPrincipal;
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
        if (collision.CompareTag("CorpoPlayer") && atingePelaBase)
        {
            // Da dano no player
            Debug.Log("Player leva dano");
            sistemaVida = collision.GetComponentInParent<SistemaVida>();
            sistemaVida.LevaAtaquePlayer(dano, knockback, forcaKnockback, gameObjectPrincipal);
        }
    }
}