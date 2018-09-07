using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {
    [Tooltip("The maximum speed of the character")]
    public float maxCharacterSpeed;

    private Rigidbody rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Updates the movement speed of the character
    /// </summary>
    private void UpdateMovementSpeed()
    {

    }
    
    private void Jump()
    {

    }
}
