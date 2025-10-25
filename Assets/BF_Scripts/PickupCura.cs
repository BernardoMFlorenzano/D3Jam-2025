using UnityEngine;

public class PickupCura : MonoBehaviour
{
    [SerializeField] private int cura;
    [SerializeField] private AudioClip curaSom;
    private SistemaVida sistemaVida;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CorpoPlayer"))
        {
            sistemaVida = collision.GetComponentInParent<SistemaVida>();

            if (sistemaVida)
            {
                sistemaVida.Cura(cura);
                AudioManager.instance.PlaySFX(curaSom,1f);
            }
            Debug.Log("Curou");
            Destroy(gameObject);
        }
    }

}
