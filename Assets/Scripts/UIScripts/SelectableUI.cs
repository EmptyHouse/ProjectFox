using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for selectable UI objects, such as sliders, buttons, etc
/// </summary>
public abstract class SelectableUI : MonoBehaviour {
    #region const variables
    protected const string SELECT_BUTTON = "Select";
    protected const string CANCEL_BUTTON = "Cancel";
    protected const string HORIZONTAL_INPUT = "Horizontal";
    protected const string VERTICAL_INPUT = "Vertical";
    #endregion const variables

    public enum Directions
    {
        North,
        South,
        East,
        West
    }

    private const float JOYSTICK_ACTIVATION_THRESHOLD = .7f;
    protected Color DISABLED_COLOR
    {
        get
        {
            return new Color(.3f, .3f, .3f);
        }
    }

    public SelectableUI northUINode;
    public SelectableUI southUINode;
    public SelectableUI eastUINode;
    public SelectableUI westUINode;

    public bool isNodeDisabled { get; private set; }

    public SelectableUI GetNodeInDirection(Directions directionToRetrieveNode)
    {
        switch (directionToRetrieveNode)
        {
            case Directions.North:
                return northUINode;
            case Directions.South:
                return southUINode;
            case Directions.East:
                return eastUINode;
            case Directions.West:
                return westUINode;
        }

        return null;
    }

    public abstract void DisplayNodeAsDisabled(bool isNodeDisabled);

    public abstract void VisuallyUpdateNode();
    #region input methods
    public bool GetSelectButtonDown()
    {
        return Input.GetButtonDown(SELECT_BUTTON);
    }

    public bool GetCancelButtonDown()
    {
        return Input.GetButtonDown(CANCEL_BUTTON);
    }

    public float GetHorizontal()
    {
        return Input.GetAxisRaw(HORIZONTAL_INPUT);
    }

    public float GetVertical()
    {
        return Input.GetAxisRaw(VERTICAL_INPUT);
    }
    #endregion input methods
}
