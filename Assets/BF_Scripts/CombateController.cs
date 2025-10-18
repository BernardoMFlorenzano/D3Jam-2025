using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombateController : MonoBehaviour
{
    [SerializeField] List<Combate> combates;
    [SerializeField] List<int> xPosCombates;
    private SpawnWaves spawner;
    private Transform cameraPos;
    private int contCombate = 0;
    private Coroutine corAtivaCombate;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraPos = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<SpawnWaves>();
        corAtivaCombate = StartCoroutine(AtivaCombate());
    }


    void FixedUpdate()
    {

    }
    
    IEnumerator AtivaCombate()
    {
        foreach(Combate combate in combates)
        {
            yield return new WaitUntil(() => cameraPos.position.x >= xPosCombates[contCombate]);
            spawner.combate = combate;
            spawner.desligado = false;
            contCombate++;
        }
    }
}
