using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectEnemyUI : MonoBehaviour {
    public Transform pointerObjectToEnemy;
    public float pointerMovementSpeed = 100;

    private int enemyThatIsSelectedIndex = 0;
    private float previousHorizontalInput;
    private float previousVerticalinput;

    public CombatManager.CombatEvent combatEventType;

    private void Start()
    {
        
    }

    private void Update()
    {
        float verticalInput = SelectableUI.GetVertical();
        
        if (verticalInput > SelectableUI.JOYSTICK_ACTIVATION_THRESHOLD &&
            previousVerticalinput < SelectableUI.JOYSTICK_ACTIVATION_THRESHOLD)
        {
            SetNextSelectedEnemy(enemyThatIsSelectedIndex + 1);
        }

        if (verticalInput < -SelectableUI.JOYSTICK_ACTIVATION_THRESHOLD &&
            previousVerticalinput > -SelectableUI.JOYSTICK_ACTIVATION_THRESHOLD)
        {
            SetNextSelectedEnemy(enemyThatIsSelectedIndex - 1);
        }

        if (SelectableUI.GetCancelButtonDown())
        {
            ReturnToPlayerOptionSelection();
        }
        if (SelectableUI.GetSelectButtonDown())
        {
            EnemySelected();
        }
        previousVerticalinput = verticalInput;
        UpdatePointerPosition();
    }

    private void UpdatePointerPosition()
    {
        CombatCharacter enemyThatIsSelected = CombatManager.Instance.allEnemyCharacters[enemyThatIsSelectedIndex];
        Vector3 goalPosition = Camera.main.WorldToScreenPoint(enemyThatIsSelected.pointerPosition.position);
        pointerObjectToEnemy.position = Vector3.Lerp(pointerObjectToEnemy.position, goalPosition, Time.deltaTime * pointerMovementSpeed);
    }


    public void SetNextSelectedEnemy(int selectedEnemyIndex)
    {
        enemyThatIsSelectedIndex = SplashScreenMenu.CustomMod(selectedEnemyIndex, CombatManager.Instance.allEnemyCharacters.Count);
    }



    public void ReturnToPlayerOptionSelection()
    {
        CombatHUD.Instance.CloseSelectEnemyUI();
        CombatHUD.Instance.OpenPlayerSelectionMenu();
    }

    public void EnemySelected()
    {
        CombatHUD.Instance.CloseSelectEnemyUI();
        if (combatEventType == CombatManager.CombatEvent.Attack)
        {
            CombatManager.Instance.AttackCharacter(CombatManager.Instance.allEnemyCharacters[enemyThatIsSelectedIndex]);
        }

        if (combatEventType == CombatManager.CombatEvent.Special)
        {
            CombatManager.Instance.AttackCharacter(CombatManager.Instance.allEnemyCharacters[enemyThatIsSelectedIndex]);
        }
    }
}
