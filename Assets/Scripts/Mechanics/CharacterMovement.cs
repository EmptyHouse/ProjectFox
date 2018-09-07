using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {
    [Tooltip("The maximum speed of the character")]
    public float maxCharacterSpeed;

    public float movementAcceleration = 10;

    private Rigidbody rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="horizontalInput"></param>
    /// <param name="verticalInput"></param>
    public void UpdateMovement(float horizontalInput, float verticalInput)
    {
        float mag = Mathf.Min(new Vector2(horizontalInput, verticalInput).magnitude, 1);
        Vector3 currentVelocity = rigid.velocity;
        currentVelocity.y = 0;
        Vector3 goalVelocity = mag * (new Vector3(horizontalInput, 0, verticalInput)).normalized * maxCharacterSpeed;

        Vector3 velocity = Vector3.MoveTowards(currentVelocity, goalVelocity, Time.fixedDeltaTime * movementAcceleration);
        rigid.velocity = new Vector3(velocity.x, rigid.velocity.y, velocity.z);
    }
    
    private void Jump()
    {

    }
}
