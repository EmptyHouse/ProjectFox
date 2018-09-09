using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectEnemyUI : MonoBehaviour {
    public Transform pointerObjectToEnemy;
    public float pointerMovementSpeed = 100;

    private int enemyThatIsSelected = 0;
    private float previousHorizontalInput;
    private float previousVerticalinput;

    private void Start()
    {
        
    }

    private void Update()
    {
        float verticalInput = SelectableUI.GetVertical();
        
        if (verticalInput > SelectableUI.JOYSTICK_ACTIVATION_THRESHOLD &&
            previousVerticalinput < SelectableUI.JOYSTICK_ACTIVATION_THRESHOLD)
        {
            SetNextSelectedEnemy(enemyThatIsSelected + 1);
        }

        if (verticalInput < -SelectableUI.JOYSTICK_ACTIVATION_THRESHOLD &&
            previousVerticalinput > -SelectableUI.JOYSTICK_ACTIVATION_THRESHOLD)
        {
            SetNextSelectedEnemy(enemyThatIsSelected - 1);
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
    }

    private void UpdatePointerPosition()
    {
        
    }


    public void SetNextSelectedEnemy(int selectedEnemyIndex)
    {
        enemyThatIsSelected = SplashScreenMenu.CustomMod(selectedEnemyIndex, CombatManager.Instance.allEnemyCharacters.Count);
    }



    public void ReturnToPlayerOptionSelection()
    {
        CombatHUD.Instance.CloseSelectEnemyUI();
        CombatHUD.Instance.OpenPlayerSelectionMenu();
    }

    public void EnemySelected()
    {
        CombatHUD.Instance.CloseSelectEnemyUI();
    }
}
