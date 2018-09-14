using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that handles the logic of our main camera
/// </summary>
public class CameraFollow : MonoBehaviour {

    [Header("Camera Settings")]
    [Tooltip("The speed at which our camera will move toward its goal position")]
    public float cameraSpeed = 100;
    [Tooltip("The rate at which our camera will rotate toward its goal rotation")]
    public float cameraRotationalSpeed = 100;
    [Tooltip("The maximum x rotation that we will allow our camera to reach")]
    public float maxXRotation = 89f;
    [Tooltip("The minimum x rotation that we will allow our camera to rotate at")]
    public float minXRotation = -60f;

    [Header("Camera Collision Detection")]
    [Tooltip("The distance between each layer of raycasts that are used to detect objects between the target and the camera point")]
    public float layerRadius = 1f;
    [Tooltip("The number of layers that we will cast")]
    public int numberOfLayers = 2;

    [Tooltip("Mark this true if you would like to inverse the controls along the y-axis for camera controls")]
    public bool invertYCameraControls;
    [Tooltip("Mark this true if you would like to inverse along the x-axis for camera controls")]
    public bool invertXCameraControls;

    private Camera cameraReference;
    private float distanceToTarget;
    private Vector3 cameraGoalPosition;

    private float horizontalInput;
    private float verticalInput;

    private void Start()
    {
        cameraReference = GetComponent<Camera>();

    }

    private void LateUpdate()
    {
        UpdateCameraPosition();
    }

    /// <summary>
    /// Updates he camera position based on controller input. Any time a player can control the camera in game,
    /// this method should be called
    /// </summary>
    /// <param name="horizontalInput"></param>
    /// <param name="verticalInput"></param>
    public void MoveCameraBasedOnInputs(float horizontalInput, float verticalInput)
    {
        this.horizontalInput = horizontalInput;
        this.verticalInput = verticalInput;
    }

    /// <summary>
    /// Updates the camera position based on the goal position of the camera
    /// </summary>
    private void UpdateCameraPosition()
    {
        
    }
}
