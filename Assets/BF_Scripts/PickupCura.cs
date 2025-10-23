using UnityEngine;

public class PickupCura : MonoBehaviour
{
    [SerializeField] private int cura;
    private SistemaVida sistemaVida;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CorpoPlayer"))
        {
            sistemaVida = collision.GetComponentInParent<SistemaVida>();

            if (sistemaVida)
            {
                sistemaVida.Cura(cura);
            }
            Debug.Log("Curou");
            Destroy(gameObject);
        }
    }

}
