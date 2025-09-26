using UnityEngine;

public class teste : MonoBehaviour
{
    [SerializeField] private ImpulsoCima impulsoPulo;
    [SerializeField] private float duracaoPulo;
    [SerializeField] private float duracaoQueda;
    [SerializeField] private float alturaPulo;
    public bool estaNoChao;
    public bool pula;

    void Start()
    {
        if (pula)
            InvokeRepeating("Pula", 0f, 5f);
    }

    void Pula()
    {
        StartCoroutine(impulsoPulo.Impulso(alturaPulo, duracaoPulo, duracaoQueda));
    }
}