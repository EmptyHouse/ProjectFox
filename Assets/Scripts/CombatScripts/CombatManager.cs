﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour {
    

    public Transform[] allEnemyCombatSpawnPoints;
    public Transform[] allPlayerPartyCombatSpawnPoints;
    public Transform activePlayerPosition;

    private List<CombatCharacter> orderOfCharacterBasedOnSpeed = new List<CombatCharacter>();
    private List<CombatCharacter> allEnemyCharacters = new List<CombatCharacter>();
    private List<CombatCharacter> allPlayerCharacters = new List<CombatCharacter>();

    private void Start()
    {
        SetupCombatScenario();
    }

    private void OnDrawGizmos()
    {
        float spherePointRadius = .5f;
        Color enemyColor = Color.red;
        Color playerColor = Color.blue;

        foreach (Transform t in allEnemyCombatSpawnPoints)
        {
            Gizmos.color = enemyColor;
            Gizmos.DrawSphere(t.position, spherePointRadius);
        }
        foreach (Transform t in allPlayerPartyCombatSpawnPoints)
        {
            Gizmos.color = playerColor;
            Gizmos.DrawSphere(t.position, spherePointRadius);
        }
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(activePlayerPosition.position, spherePointRadius);
    }

    
    public void SetupCombatScenario()
    {
        int numberOfEnemies = Random.Range(1, 4);
        int enemiesSpawnedSoFar = 0;
        CombatCharacter spawnCombatCharacter = null;
        while (enemiesSpawnedSoFar < numberOfEnemies)
        {
            int typeOfEnemy = Random.Range(0, 3);
            switch (typeOfEnemy)
            {
                case 0:
                    if (enemiesSpawnedSoFar > 0 || numberOfEnemies == 1)
                    {
                        spawnCombatCharacter = Instantiate<CombatCharacter>(GameOverseer.Instance.golemCombatPrefab);
                    }
                    else
                    {
                        continue;
                    }
                    break;
                case 1:
                    spawnCombatCharacter = Instantiate<CombatCharacter>(GameOverseer.Instance.mushroomCombatPrefab);
                    break;

                case 2:
                    spawnCombatCharacter = Instantiate<CombatCharacter>(GameOverseer.Instance.slimeCombatPrefab);
                    break;
            }
            
            spawnCombatCharacter.transform.position = allEnemyCombatSpawnPoints[enemiesSpawnedSoFar].position;
            allEnemyCharacters.Add(spawnCombatCharacter);
            enemiesSpawnedSoFar++;

            
        }
        allPlayerCharacters.Add(Instantiate<CombatCharacter>(GameOverseer.Instance.foxCombatPrefab));
        allPlayerCharacters.Add(Instantiate<CombatCharacter>(GameOverseer.Instance.bearCombatPrefab));
        allPlayerCharacters.Add(Instantiate<CombatCharacter>(GameOverseer.Instance.owlCombatPrefab));

        for (int i = 0; i < allPlayerCharacters.Count; i++)
        {
            allPlayerCharacters[i].transform.position = allPlayerPartyCombatSpawnPoints[i].position;
        }

    }

    
}
