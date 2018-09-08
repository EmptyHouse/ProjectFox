using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreenMenu : MonoBehaviour {
    public SelectableUI initiallySelectedUINode;
    public SelectableUI[] selectableUIList;

    #region button events
    public void OnStartGameButtonPressed()
    {

    }

    public void OnOptionsButtonPresed()
    {

    }

    public void OnQuitGamePressed()
    {
        Application.Quit();
    }
    #endregion button events
}
