using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelectableButton : SelectableUI {
    [Header("Button UI Elements")]
    public Image buttonBackgroundImage;
    public Text buttonText;

    [Header("Event")]
    public ButtonEvent buttonEvent;

    private Color defaultButtonBackgroundColor;
    private Color defaultButtonTextColor;

    #region monobehaviour methods
    private void Start()
    {
        if (buttonBackgroundImage != null)
        {
            defaultButtonTextColor = buttonBackgroundImage.color;
        }    
        if (buttonText != null)
        {
            defaultButtonTextColor = buttonText.color;
        }
    }

    private void Update()
    {
        if (GetSelectButtonDown())
        {
            buttonEvent.Invoke(this, false);
        }
    }
    #endregion monobehaivour methods

    #region override methods
    public override void DisplayNodeAsDisabled(bool isNodeDisabled)
    {
        if (isNodeDisabled)
        {
            if (buttonBackgroundImage != null)
                buttonBackgroundImage.color = DISABLED_COLOR;
            if (buttonText != null)
                buttonText.color = DISABLED_COLOR;
        }
        else
        {
            if (buttonBackgroundImage != null)
                buttonBackgroundImage.color = defaultButtonBackgroundColor;
            if (buttonText != null)
            {
                buttonText.color = defaultButtonTextColor;
            }
        }

    }

    public override void VisuallyUpdateNode()
    {
        buttonEvent.Invoke(this, true);
    }
    #endregion override methods
    [System.Serializable]
    public class ButtonEvent : UnityEvent<SelectableButton, bool>
    {

    }
}
