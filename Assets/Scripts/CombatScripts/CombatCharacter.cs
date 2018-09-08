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
    public float maxHealthPoints = 20f;
    public float maxSpecialPoints = 15f;
    public float armorClass = 5f;
    [Range(0f, 1f)]
    public float evasionRate = .2f;
    public float speedInCombat = 1f;

    public float currentHealthPoints { get; private set; }
    public float currentSpecialPoints { get; private set; }

    private void Start()
    {
        currentHealthPoints = maxHealthPoints;
        currentSpecialPoints = maxSpecialPoints;
    }



    public virtual void TakeDamage(CombatCharacter characterThatDamagedMe, float damageToTake)
    {
        currentHealthPoints -= damageToTake;
    }

    public virtual void OnCharacterDied(CombatCharacter characterThatKilledMe)
    {

    }
}
