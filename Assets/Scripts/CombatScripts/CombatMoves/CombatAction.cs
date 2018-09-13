using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAction : MonoBehaviour {
    [Tooltip("The name of the move that can be chosen")]
    public string combatMoveName;
    [Tooltip("The chance that this move will hit its target. The chance of landing move can be set between 0 and 1, with 0 meanint the move will never hit and 1 meaning the move will always hit")]
    [Range(0, 1)]
    public float chanceOfLandingMove;
    [Tooltip("The associated combat character that will perform this action")]
    public CombatCharacter associatedCombatCharacter;
    /// <summary>
    /// This variable should be marked true during the entire duration that this move is active. This will indicate that the move
    /// is currently performing the action
    /// </summary>
    public bool actionIsBusy { get; protected set; }

    #region action methods

    #endregion action methods

    #region helper methods
    /// <summary>
    /// Moves an object toward a position in the time that is provided. If you would like an object to ease into a position, set
    /// use lerp to true. Otherwise the object will move in a linear fasion
    /// </summary>
    /// <param name="objectToMove"></param>
    /// <param name="positionToMoveToward"></param>
    /// <param name="timeToReachDestination"></param>
    /// <param name="useLerp"></param>
    /// <returns></returns>
    public IEnumerator MoveTowardPosition(Transform objectToMove, Vector3 positionToMoveToward, float timeToReachDestination, bool useLerp)
    {
        Vector3 startPosition = objectToMove.position;
        Vector3 goalPosition = positionToMoveToward;
        Vector3 directionToGoal = (goalPosition - startPosition).normalized;
        float distanceToGoal = (goalPosition - startPosition).magnitude;

        float currentTimeThatHasPassed = 0;


        while (currentTimeThatHasPassed < timeToReachDestination)
        {
            if (useLerp)
            {
                objectToMove.position = Vector3.Lerp(startPosition, goalPosition, currentTimeThatHasPassed / timeToReachDestination);
            }
            else
            {
                objectToMove.position = startPosition + (directionToGoal * distanceToGoal * (currentTimeThatHasPassed / timeToReachDestination));
            }
            currentTimeThatHasPassed += Time.deltaTime;
            yield return null;
        }
        objectToMove.position = goalPosition;

    }
    #endregion helper methods
}
