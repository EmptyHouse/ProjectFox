using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCharacter : MonoBehaviour {
    public enum Alliance
    {
        Enemy,
        Player,
    }
    public Alliance characterAlliance;
    [Header("Default Stats")]
    public string characterName;
    public float maxHealthPoints = 20f;
    public float maxSpecialPoints = 15f;
    public float armorClass = 5f;
    
    public float power = 15f;
    [Range(0f, 1f)]
    public float evasionRate = .2f;
    public float speedInCombat = 1f;

    public Transform pointerPosition;

    public float currentHealthPoints { get; private set; }
    public float currentSpecialPoints { get; private set; }

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        currentHealthPoints = maxHealthPoints;
        currentSpecialPoints = maxSpecialPoints;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        spriteRenderer.transform.LookAt(Camera.main.transform.position);
    }

    public virtual void TakeDamage(CombatCharacter characterThatDamagedMe, float damageToTake)
    {
        currentHealthPoints -= damageToTake;
        if (currentHealthPoints <= 0)
        {
            OnCharacterDied(characterThatDamagedMe);
        }
    }

    public virtual void OnCharacterDied(CombatCharacter characterThatKilledMe)
    {
        Destroy(this.gameObject);
        CombatManager.Instance.CharacterDied(this);
    }
}
