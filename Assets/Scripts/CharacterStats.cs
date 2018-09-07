using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class CharacterStats : MonoBehaviour {
    public float maximumHealth = 100;
    public float currentHealth;

    //[SerializeField]
    private SpriteRenderer spriteRenderer;


    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
        currentHealth = maximumHealth;
    }
}
