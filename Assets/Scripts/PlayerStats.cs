using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats {
    private static PlayerStats instance;

    public static PlayerStats Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<PlayerStats>();
            }
            return instance;
        }
    }

    public Transform cameraTargetPosition;

    private void Awake()
    {
        instance = this;
    }

    public Collider triggerBoxCollider;
}
