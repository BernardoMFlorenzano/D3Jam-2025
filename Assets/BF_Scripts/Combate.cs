using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Combate/Combate Normal")]
public class Combate : ScriptableObject
{
    public List<Wave> listaWaves;
    public float delayEntreWaves;
    public float tempoMaxWave; // Tempo máximo pra proxima wave spawnar
    public int maxInimigos; // Quantidade máxima de inimigos na tela (Por enquanto não é um hardcap, apenas informa se wave pode começar ou não)
    public int minInimigoPassaWave;     // Quantidade de inimigos pra que proxima wave possa spawnar
}
