using UnityEngine;

public class SetaEfeitoAtaque : MonoBehaviour
{
    private Animator animatorEfeito;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animatorEfeito = GetComponent<Animator>();
    }

    public void SetarEfeitoPosEscala(Vector2 pos, Vector2 escala)
    {
        transform.localPosition = pos;
        transform.localScale = escala;
    }
}
