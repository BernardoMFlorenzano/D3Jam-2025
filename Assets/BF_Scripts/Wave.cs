using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Wave/Wave Normal")]
public class Wave : ScriptableObject
{
    public List<GameObject> inimigosWave;
    public float intervaloSpawn;
    public int lado;    // 1 == Esquerda, 2 == Direita ou 3 == Ambos
}
