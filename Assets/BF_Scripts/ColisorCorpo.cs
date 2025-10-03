using UnityEngine;

public class ColisorCorpo : MonoBehaviour
{
    public int tipoAtaque;  // Vai ser setado o tipo do ataque assim que o collider for ativado; 1 = ataque chao, 2 = ataque aereo, 
    public int armaAtaque;  // Arma que está sendo usada
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
            if (armaAtaque == 0)
            {
                sistemaVida.LevaAtaquePadrao(tipoAtaque, gameObjectPrincipal);
            }
            else if (armaAtaque == 1)
            {
                sistemaVida.LevaAtaqueLanca(tipoAtaque, gameObjectPrincipal);
            }
        }
    }
}
