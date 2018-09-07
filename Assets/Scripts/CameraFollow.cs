using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private PlayerStats associatePlayerStats;
    public float cameraSpeed = 100;

    private Camera cameraReference;
    private float distanceToTarget;

    private void Start()
    {
        cameraReference = GetComponent<Camera>();
        associatePlayerStats = GetComponent<PlayerStats>();

    }

    private void Update()
    {
        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        Transform targetCameraPosition = associatePlayerStats.cameraTargetPosition;


    }
}
