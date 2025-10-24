using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;

public class CombateController : MonoBehaviour
{
    [SerializeField] List<Combate> combates;
    [SerializeField] List<int> xPosCombates;
    [SerializeField] GameObject parede;
    private SpawnWaves spawner;
    private Transform cameraPos;
    private int contCombate = 0;
    private Coroutine corAtivaCombate;
    private PassaCena gameManager;
    public bool acabouCombates;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraPos = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<SpawnWaves>();
        corAtivaCombate = StartCoroutine(AtivaCombate());
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PassaCena>();
    }


    void FixedUpdate()
    {

    }

    IEnumerator AtivaCombate()
    {
        foreach (Combate combate in combates)
        {
            yield return new WaitUntil(() => cameraPos.position.x >= xPosCombates[contCombate]);

            DesativaBackTrack(xPosCombates[contCombate]);

            spawner.combate = combate;
            spawner.desligado = false;
            contCombate++;
        }

        acabouCombates = true;

        //yield return new WaitUntil(() => spawner.desligado == false);
        // Agora espera acabar o ultimo combate
        //yield return new WaitUntil(() => spawner.desligado == true);
        //gameManager.TrocaCena(2); // Acaba jogo
    }
    
    void DesativaBackTrack(float x)
    {
        x -= 10f;
        parede.transform.position = new Vector3(x,0f,0f);
    }
}
