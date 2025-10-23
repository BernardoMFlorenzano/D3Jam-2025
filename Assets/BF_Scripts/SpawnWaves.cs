using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using System;
using Unity.Cinemachine;
using Random = UnityEngine.Random;

public class SpawnWaves : MonoBehaviour
{
    [SerializeField] private CombateController combateController;
    public Combate combate; // Vai ser trocado externamente
    //private bool emCombate;
    public bool desligado;
    //private bool temWaves;
    private bool passaWave;
    private Coroutine corCombate;
    private Coroutine corTempoWave;
    [SerializeField] List<Transform> spawnPointsEsq;
    [SerializeField] List<Transform> spawnPointsDir;
    public int numInimigos;
    [SerializeField] CinemachinePositionComposer controleCamera;
    [SerializeField] private GameObject pickupCura;
    [SerializeField] private GameObject seta;
    [SerializeField] private float tempoSeta;
    private bool piscandoSeta = false;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        desligado = true;
        //emCombate = false;
        //temWaves = true;
        passaWave = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (desligado)
        {
            // Não faz nada
            Desligado();
        }
        else if (!desligado)
        {
            //emCombate = true;
            EmCombate();
        }
    }

    void Desligado()
    {
        if(!controleCamera.isActiveAndEnabled)
        {
            controleCamera.enabled = true;
        }
    }

    void EmCombate()
    {
        if (corCombate == null)
        {
            corCombate = StartCoroutine(CorCombate());
        }

        if(controleCamera.isActiveAndEnabled)
        {
            controleCamera.enabled = false;
        }
    }

    IEnumerator CorCombate()
    {
        foreach (Wave wave in combate.listaWaves)
        {
            foreach (GameObject inimigo in wave.inimigosWave)
            {
                //yield return new WaitUntil(() => combate.maxInimigos > numInimigos)
                SpawnarInimigo(inimigo, wave.lado);
                yield return new WaitForSeconds(wave.intervaloSpawn);
            }

            passaWave = false;
            corTempoWave = StartCoroutine(TempoWaveMax(combate.tempoMaxWave));

            yield return new WaitUntil(() => numInimigos <= combate.minInimigoPassaWave || passaWave && combate.maxInimigos > numInimigos);

            if (corTempoWave != null)
            {
                StopCoroutine(corTempoWave);
            }
            passaWave = false;

            yield return new WaitForSeconds(combate.delayEntreWaves);
        }

        yield return new WaitUntil(() => numInimigos <= 0);
        //temWaves = false;
        desligado = true;
        corCombate = null;
        //emCombate = false;

        if (!combateController.acabouCombates)
        {
            SpawnarCura();
            piscandoSeta = true;
            StartCoroutine(SetaGo());
        }

        Debug.Log("Acabou combate");
    }

    IEnumerator TempoWaveMax(float tempo)
    {
        yield return new WaitForSeconds(tempo);
        passaWave = true;
    }

    IEnumerator SetaGo()
    {
        StartCoroutine(TempoSetaGo());
        while (piscandoSeta)
        {
            seta.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            seta.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator TempoSetaGo()
    {
        yield return new WaitForSeconds(tempoSeta);
        seta.SetActive(false);
        piscandoSeta = false;
    }

    
    void SpawnarInimigo(GameObject inimigo, int lado)
    {
        int spawnPos;

        if (lado == 1)
        {
            spawnPos = Random.Range(0, spawnPointsEsq.Count);
            Instantiate(inimigo, new Vector3(spawnPointsEsq[spawnPos].position.x, spawnPointsEsq[spawnPos].position.y, 0), Quaternion.identity);
            numInimigos++;
        }
        else if (lado == 2)
        {
            spawnPos = Random.Range(0, spawnPointsDir.Count);
            Instantiate(inimigo, new Vector3(spawnPointsDir[spawnPos].position.x, spawnPointsDir[spawnPos].position.y, 0), Quaternion.identity);
            numInimigos++;
        }
        else if (lado == 3)
        {
            spawnPos = Random.Range(0, spawnPointsEsq.Count + spawnPointsDir.Count);
            if (spawnPos <= 2)
            {
                SpawnarInimigo(inimigo, 1);
            }
            else if (spawnPos > 2)
            {
                SpawnarInimigo(inimigo, 2);
            }
        }
    }
    
    void SpawnarCura()
    {
        int chance = Random.Range(0, 3);
        if (chance == 2)
        {
            int spawnPos = Random.Range(0, spawnPointsDir.Count);
            Instantiate(pickupCura, new Vector3(spawnPointsDir[spawnPos].position.x, spawnPointsDir[spawnPos].position.y, 0), Quaternion.identity);
        }
    }

}
