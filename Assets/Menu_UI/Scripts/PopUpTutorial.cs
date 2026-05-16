using System.Collections;
using UnityEngine;

public class PopUpTutorial : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool comecou = false;
    [SerializeField] private bool comecaSozinho = false;
    [SerializeField] private float delayComeca = 0f;
    [SerializeField] private float tempoAteSumir = 4f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        if (comecaSozinho && !comecou)
            Comeca();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !comecou)
        {
            Comeca();
        }
    }

    public void Comeca()
    {
        comecou = true;
        StartCoroutine(ComecaAnim());
    }

    IEnumerator ComecaAnim()
    {  
        yield return new WaitForSeconds(delayComeca);
        animator.SetTrigger("Comeca");
        StartCoroutine(Some()); 
        spriteRenderer.enabled = true;
    }

    IEnumerator Some()
    {
        yield return new WaitForSeconds(tempoAteSumir);
        animator.SetTrigger("Some");
        //yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Default"));
        yield return new WaitForSeconds(1f);
        spriteRenderer.enabled = false;

    }
}
