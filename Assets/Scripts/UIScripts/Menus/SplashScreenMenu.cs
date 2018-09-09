using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreenMenu : MonoBehaviour {

    public SelectableUI initiallySelectedUINode;
    public SelectableUI[] selectableUIList;
    public RectTransform optionPointer;
    public float timeToReachNextOption = .1f;
    public float timeBetweenAutoScrollOptions = .1f;


    private SelectableUI currentlySelectedNode;
    private float previousVerticalInput;

    private Vector3 pointerStartPosition;
    private Vector3 pointerGoalPosition;
    private bool isAutoScrolling;
    #region monobehaviour methods
    private void Start()
    {
        foreach (SelectableUI node in selectableUIList)
        {
            node.enabled = false;
        }
        for (int i = 0; i < selectableUIList.Length; i++)
        {
            selectableUIList[CustomMod(i - 1, selectableUIList.Length)].southUINode = selectableUIList[i];
            selectableUIList[i].northUINode = selectableUIList[CustomMod(i - 1, selectableUIList.Length)];
        }
        SetCurrentSelectableUI(initiallySelectedUINode);
        optionPointer.position = new Vector3(optionPointer.position.x, currentlySelectedNode.transform.position.y, optionPointer.position.z);
    }

    private void Update()
    {
        float verticalInput = SelectableUI.GetVertical();
        if (!isAutoScrolling)
        {
            if (verticalInput > SelectableUI.JOYSTICK_ACTIVATION_THRESHOLD)
            {
                StartCoroutine(AutoScroll(SelectableUI.GetVertical()));
            }
            if (verticalInput < -SelectableUI.JOYSTICK_ACTIVATION_THRESHOLD)
            {
                StartCoroutine(AutoScroll(SelectableUI.GetVertical()));
            }
        }
        
        UpdatePointerPosition();
        previousVerticalInput = verticalInput;
    }
    #endregion monobehaviour methods

    private void UpdatePointerPosition()
    {
        optionPointer.position = Vector3.Lerp(optionPointer.position, pointerGoalPosition, Time.deltaTime * timeToReachNextOption);
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

    public static int CustomMod(int value, int length)
    {
        int modValue = (value % length)  + length;
        return modValue % length;
    }

    #region button events
    public void OnStartGameButtonPressed(SelectableButton button, bool onlyUpdateVisually)
    {
        print("Start Game Pressed");
    }

    public void OnOptionsButtonPresed()
    {

    }

    public void OnQuitGamePressed()
    {
        Application.Quit();
    }
    #endregion button events

    private IEnumerator AutoScroll(float verticalInput)
    {
        this.isAutoScrolling = true;
        float direction = Mathf.Sign(verticalInput);
        SetCurrentSelectableUI(direction < 0 ? currentlySelectedNode.southUINode : currentlySelectedNode.northUINode);

        float timer = 0;
        while (timer < timeBetweenAutoScrollOptions * 3)
        {
            timer += Time.deltaTime;
            if (direction * SelectableUI.GetVertical() < SelectableUI.JOYSTICK_ACTIVATION_THRESHOLD)
            {
                this.isAutoScrolling = false;
                yield break;
            }
            yield return null;
        }
        timer = 0;
        SetCurrentSelectableUI(direction < 0 ? currentlySelectedNode.southUINode : currentlySelectedNode.northUINode);
        while (direction * SelectableUI.GetVertical() > SelectableUI.JOYSTICK_ACTIVATION_THRESHOLD)
        {
            if (timer > timeBetweenAutoScrollOptions)
            {
                timer = 0;
                SetCurrentSelectableUI(direction < 0 ? currentlySelectedNode.southUINode : currentlySelectedNode.northUINode);
            }
            timer += Time.deltaTime;
            yield return null;
        }
        this.isAutoScrolling = false;
    }
}
