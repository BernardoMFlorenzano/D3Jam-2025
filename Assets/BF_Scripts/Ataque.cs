using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[CreateAssetMenu(menuName = "Ataques/Ataque Normal")]
public class Ataque : ScriptableObject
{
    //public AnimatorOverrideController animatorOV;
    public int modoAtaque; // corte ou estocada 
    public Vector2 boxOffset;   // Controla hitbox
    public Vector2 boxSize;     // Controla hitbox
    public int dano;
    public bool knockback;
    public float forcaKnockback;
}
