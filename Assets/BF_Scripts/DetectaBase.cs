using System;
using UnityEngine;

public class DetectaBase : MonoBehaviour
{
    private SistemaVida sistemaVida;
    [SerializeField] private int tipo; // Player ou inimigo
    [SerializeField] private ColisorCorpoInimigo colisorCorpo;
    //public int armaPlayer;  // Arma do player do colisor

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
        if (collision.CompareTag("Inimigo") || collision.CompareTag("InimigoVoador") && tipo == 1)
        {
            sistemaVida = collision.GetComponent<SistemaVida>();

            if (sistemaVida)
            {
                sistemaVida.atingivelBase = true;
            }
            Debug.Log("Player acerta pela base");
        }

        if (collision.CompareTag("Player") && tipo == 2)
        {
            // Define o player como atingivel por esse inimigo
            colisorCorpo.atingePelaBase = true;
            Debug.Log("Inimigo acerta pela base");
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Inimigo") || collision.CompareTag("InimigoVoador") && tipo == 1)
        {
            if (sistemaVida.atingivelBase == false)
            {
                sistemaVida = collision.GetComponent<SistemaVida>();
                if (sistemaVida)
                {
                    sistemaVida.atingivelBase = true;
                }
            }
            //Debug.Log("Player acerta pela base");
        }

        if (collision.CompareTag("Player") && tipo == 2)
        {
            // Define o player como atingivel por esse inimigo
            colisorCorpo.atingePelaBase = true;
            Debug.Log("Inimigo acerta pela base");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Inimigo") || collision.CompareTag("InimigoVoador") && tipo == 1)
        {
            sistemaVida = collision.GetComponent<SistemaVida>();
            if (sistemaVida)
            {
                sistemaVida.atingivelBase = false;
            }
            Debug.Log("Player não acerta mais pela base");
        }

        if (collision.CompareTag("Player") && tipo == 2)
        {
            // Define o player como não atingivel por esse inimigo
            colisorCorpo.atingePelaBase = false;
            Debug.Log("Inimigo não acerta mais pela base");
        }
    }

}
