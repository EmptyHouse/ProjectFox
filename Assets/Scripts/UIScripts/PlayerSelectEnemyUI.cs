using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectEnemyUI : MonoBehaviour {
    public Transform pointerObjectToEnemy;

    private int enemyThatIsSelected = 0;
    private float previousHorizontalInput;
    private float previousVerticalinput;

    private void Start()
    {
        
    }

    private void Update()
    {
        float verticalInput = SelectableUI.GetVertical();
        float horizontalInput = SelectableUI.GetHorizontal();
    }


}
