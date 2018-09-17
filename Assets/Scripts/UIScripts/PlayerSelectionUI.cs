using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectionUI : MonoBehaviour {
    public SelectableUI initiallySelectedUINode;
    public SelectableUI[] selectableUIList;


    private SelectableUI currentlySelectedNode;
    public Transform optionPointer;

    private Vector3 pointerStartPosition;
    private Vector3 pointerGoalPosition;

    public float pointerMovementSpeed = 50f;
    private float previousVerticalInput;

    private void Start()
    {
        foreach (SelectableUI node in selectableUIList)
        {
            node.enabled = false;
        }
        for (int i = 0; i < selectableUIList.Length; i++)
        {
            selectableUIList[SplashScreenMenu.CustomMod(i - 1, selectableUIList.Length)].southUINode = selectableUIList[i];
            selectableUIList[i].northUINode = selectableUIList[SplashScreenMenu.CustomMod(i - 1, selectableUIList.Length)];
        }
        SetCurrentSelectableUI(initiallySelectedUINode);
        optionPointer.position = new Vector3(optionPointer.position.x, currentlySelectedNode.transform.position.y, optionPointer.position.z);
    }

    private void Update()
    {
        float verticalInput = SelectableUI.GetVertical();
        if (verticalInput < -SelectableUI.JOYSTICK_ACTIVATION_THRESHOLD && 
            previousVerticalInput > -SelectableUI.JOYSTICK_ACTIVATION_THRESHOLD)
        {
            SetCurrentSelectableUI(currentlySelectedNode.southUINode);
        }
        if (verticalInput > SelectableUI.JOYSTICK_ACTIVATION_THRESHOLD &&
            previousVerticalInput < SelectableUI.JOYSTICK_ACTIVATION_THRESHOLD)
        {
            SetCurrentSelectableUI(currentlySelectedNode.northUINode);
        }

        UpdatePointerPosition();
        previousVerticalInput = verticalInput;
    }



    private void UpdatePointerPosition()
    {
        optionPointer.position = Vector3.Lerp(optionPointer.position, pointerGoalPosition, Time.deltaTime * pointerMovementSpeed);
    }

    private void SetCurrentSelectableUI(SelectableUI nextSelectableNode)
    {
        if (currentlySelectedNode != null)
        {
            currentlySelectedNode.enabled = false;
        }
        currentlySelectedNode = nextSelectableNode;
        currentlySelectedNode.enabled = true;
        pointerStartPosition = optionPointer.position;
        pointerGoalPosition = new Vector3(optionPointer.position.x, currentlySelectedNode.transform.position.y, optionPointer.position.z);

    }

    #region button events
    public void OnAttackButtonPressed(SelectableButton button)
    {
        CombatHUD.Instance.ClosePlayerSelectionMenu();
        CombatHUD.Instance.selectEnemyUI.combatEventType = CombatManager.CombatEvent.Attack;
        CombatHUD.Instance.OpenSelectEnemyUI();
    }

    public void OnGuardButtonPressed(SelectableButton button)
    {
        CombatHUD.Instance.ClosePlayerSelectionMenu();
        CombatManager.Instance.SetNextActiveCharacter();
    }

    public void OnSpecialButtonPressed(SelectableButton button)
    {
        CombatHUD.Instance.ClosePlayerSelectionMenu();
        CombatHUD.Instance.selectEnemyUI.combatEventType = CombatManager.CombatEvent.Attack;
        CombatHUD.Instance.OpenSelectEnemyUI();
    }

    
    #endregion button events
}
