using System;
using UnityEngine;

public class DetectaBase : MonoBehaviour
{
    private SistemaVida sistemaVida;
    public int armaPlayer;  // Arma do player do colisor

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
        if (collision.CompareTag("Inimigo"))
        {
            sistemaVida = collision.GetComponent<SistemaVida>();

            if (sistemaVida)
            {
                if (armaPlayer == 0)
                    sistemaVida.atingivelBasePadrao = true;
                else if (armaPlayer == 1)
                    sistemaVida.atingivelBaseLanca = true;
            }
            Debug.Log("Player acerta pela base");
        }

        if (collision.CompareTag("Player"))
        {
            // Define o player como atingivel por esse inimigo
            Debug.Log("Inimigo acerta pela base");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Inimigo"))
        {
            sistemaVida = collision.GetComponent<SistemaVida>();
            if (sistemaVida)
            {
                if (armaPlayer == 0)
                    sistemaVida.atingivelBasePadrao = false;
                else if (armaPlayer == 1)
                    sistemaVida.atingivelBaseLanca = false;
            }
            Debug.Log("Player não acerta mais pela base");
        }

        if (collision.CompareTag("Player"))
        {
            // Define o player como não atingivel por esse inimigo
            Debug.Log("Inimigo não acerta mais pela base");
        }
    }

}
