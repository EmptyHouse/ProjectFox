using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for selectable UI objects, such as sliders, buttons, etc
/// </summary>
public abstract class SelectableUI : MonoBehaviour {
    public enum Directions
    {
        North,
        South,
        East,
        West
    }

    private const float JOYSTICK_ACTIVATION_THRESHOLD = .7f;

    public SelectableUI northUINode;
    public SelectableUI southUINode;
    public SelectableUI eastUINode;
    public SelectableUI westUINode;

    public bool isNodeDisabled = false;

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

    public abstract void DisplayNodeAsDisabled();

    public abstract void VisuallyUpdateNode();

    public abstract void UpdateNodeValue();
}
