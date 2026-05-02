using UnityEngine;

public class DestroiObjeto : MonoBehaviour
{
    private ImpulsoCima impulsoCima;
    public bool podeDestruir = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        impulsoCima = GetComponent<ImpulsoCima>();


    }

    // Update is called once per frame
    void Update()
    {
        if (impulsoCima.caiu && podeDestruir)
        {
            Destroy(gameObject, 0.5f);
        }
    }
}
