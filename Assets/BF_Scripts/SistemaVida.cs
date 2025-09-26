using TMPro;
using UnityEngine;

public class SistemaVida : MonoBehaviour
{
    public bool atingivelBase;
    [SerializeField] private TMP_Text textoTeste;
    //private bool levandoAtaque;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(textoTeste)
            textoTeste.text = "10";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LevaAtaque()
    {
        if (atingivelBase)
        {
            if (textoTeste)
            {
                int num = int.Parse(textoTeste.text) - 1;
                textoTeste.text = num.ToString();
            }
                
            Debug.Log("Acertou ataque");
        }
    }
}
