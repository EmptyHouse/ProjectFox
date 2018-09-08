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
        if (buttonBackgroundImage)
        {
            defaultButtonTextColor = buttonBackgroundImage.color;
        }    
        if (buttonText)
        {
            defaultButtonTextColor = buttonText.color;
        }
    }

    private void Update()
    {
        
    }
    #endregion monobehaivour methods

    #region override methods
    public override void DisplayNodeAsDisabled(bool isNodeDisabled)
    {
        if (isNodeDisabled)
        {
            if (buttonBackgroundImage)
                buttonBackgroundImage.color = DISABLED_COLOR;
            if (buttonText)
                buttonText.color = DISABLED_COLOR;
        }
        else
        {
            if (buttonBackgroundImage)
                buttonBackgroundImage.color = defaultButtonBackgroundColor;
            if (buttonText)
            {
                buttonText.color = defaultButtonTextColor;
            }
        }

    }

    public override void UpdateNodeValue()
    {
        throw new System.NotImplementedException();
    }

    public override void VisuallyUpdateNode()
    {
        throw new System.NotImplementedException();
    }
    #endregion override methods
    [System.Serializable]
    public class ButtonEvent : UnityEvent<SelectableButton, bool>
    {

    }
}
