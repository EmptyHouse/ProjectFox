using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class PlayerController : MonoBehaviour {
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    #region main variables
    public bool blockContrllerInput = false;

    private CharacterMovement characterMovement;
    #endregion main variables

    #region monobehaiovur methods
    private void Start()
    {
        characterMovement = GetComponent<CharacterMovement>();
    }

    private void Update()
    {
        if (blockContrllerInput)
        {
            return;
        }
        float horizontalInput = Input.GetAxisRaw(HORIZONTAL);
        float verticalInput = Input.GetAxisRaw(VERTICAL);

        characterMovement.UpdateMovement(horizontalInput, verticalInput);

    }
    #endregion monobehaviour methods
}
