using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCharacter : MonoBehaviour {
    public enum Alliance
    {
        Enemy,
        Player,
    }
    
    [Header("Default Stats")]
    [Tooltip("Alliance that our character is associated with. This will help so that we do not apply damage to friendly characters as well as don't heal enemy characters")]
    public Alliance characterAlliance;
    [Tooltip("The name of the character that will be displayed in a GUI element")]
    public string characterName;
    [Tooltip("The maximum HP that can be applied to this character")]
    public float maxHealthPoints = 20f;
    [Tooltip("Additional health that can be applied to this character with some special items or special actions that will give the player extra health that may go beyond maximum HP. NOTE: This health will be lost first if a player takes damage.")]
    public float temporaryHitPoints = 0;
    [Tooltip("The maximum special points that a player can possess. If a player applies a special that applies more than the maximum, the item will still be lost, but will only apply enough points to reach the maximum")]
    public float maxSpecialPoints = 15f;
    [Tooltip("The baseline armor class of the character, before any additionaly armor is taken into consideration")]
    public float armorClass = 5f;
    [Tooltip("The baseline attack power that a character possesses before additional stats from a weapon or other stat boosts are applied")]
    public float attackPower = 15f;

    [Tooltip("The baseline evasion rate that this character will possess before any other stat boosts are applied")]
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

    /// <summary>
    /// This method will determine if a character will be hit by a specific attack based on factors of this character
    /// as well as factors from the character that is attacking
    /// </summary>
    /// <param name="characterAttackingMe"></param>
    /// <param name="actionBeingPerformed"></param>
    /// <returns></returns>
    public bool CheckIfCombatCharacterLandsAttack(CombatCharacter characterAttackingMe, CombatAction actionBeingPerformed)
    {
        return false;
    }

    #region damage methods
    /// <summary>
    /// Whenever a character takes damage this method should be called
    /// </summary>
    /// <param name="characterThatDamagedMe"></param>
    /// <param name="damageToTake"></param>
    public virtual void TakeDamage(CombatCharacter characterThatDamagedMe, float damageToTake)
    {
        currentHealthPoints -= damageToTake;
        if (currentHealthPoints <= 0)
        {
            OnCharacterDied(characterThatDamagedMe);
        }
    }

    /// <summary>
    /// Method that will handle what will happen when our player 
    /// </summary>
    protected virtual void VisuallyDisplayTakingDamage()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public void DamagePlayerPhysically(CombatCharacter characterThatWillBeAttacked, CombatAction actionBeingApplied)
    {

    }

    /// <summary>
    /// 
    /// </summary>
    protected virtual void VisuallyDisplayApplyingDamage()
    {

    }
    #endregion damage methods

    #region healing methods
    /// <summary>
    /// 
    /// </summary>
    /// <param name="characterHealingMe"></param>
    /// <param name="healthToRegain"></param>
    public virtual void ReviveHealth(CombatCharacter characterHealingMe, float healthToRegain)
    {

    }
    #endregion healing methods

    public virtual void OnCharacterDied(CombatCharacter characterThatKilledMe)
    {
        Destroy(this.gameObject);
        CombatManager.Instance.CharacterDied(this);
    }
}
