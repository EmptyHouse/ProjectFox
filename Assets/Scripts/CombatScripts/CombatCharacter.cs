using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCharacter : MonoBehaviour {
    

    [Header("Default Stats")]
    public float maxHealthPoints;
    public float maxSpecialPoints;
    public float armorClass;
    [Range(0f, 1f)]
    public float evasionRate;

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
