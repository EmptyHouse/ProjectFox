﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour {
    private static CombatManager instance;

    public static CombatManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<CombatManager>();
            }
            return instance;
        }
    }

    public enum CombatEvent
    {
        Attack,
        Special,
        Guard,
    }

    public enum CombatState
    {
        CombatInSession,
        PlayerWon,
        PlayerLost,
    }

    private CombatState currentCombatState;

    public float timeForPlayerCharacterToREachFront = .1f;

    public CombatCharacter.Alliance currentActiveAlliance;
    public CombatCharacter currentlyActiveCharacter;
    public int currentlActiveCharacterIndex;

    public Transform[] allEnemyCombatSpawnPoints;
    public Transform[] allPlayerPartyCombatSpawnPoints;
    public Transform activePlayerPosition;

    private List<CombatCharacter> orderOfCharacterBasedOnSpeed = new List<CombatCharacter>();
    public List<CombatCharacter> allEnemyCharacters = new List<CombatCharacter>();
    public List<CombatCharacter> allPlayerCharacters = new List<CombatCharacter>();

    public UnityEditor.SceneAsset sceneToLoadOnVictory;
    public UnityEditor.SceneAsset sceneToLoadOnDefeat;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        currentCombatState = CombatState.CombatInSession;
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
        foreach (CombatCharacter c in allPlayerCharacters)
        {
            orderOfCharacterBasedOnSpeed.Add(c);
        }

        foreach (CombatCharacter c in allEnemyCharacters)
        {
            orderOfCharacterBasedOnSpeed.Add(c);
        }
        currentlActiveCharacterIndex = -1;
        SetNextActiveCharacter();
        CombatHUD.Instance.FadeBlackBackground();
    }

    public void SetNextActiveCharacter()
    {
        
        int nextCharacterIndex = currentlActiveCharacterIndex + 1;
        nextCharacterIndex = SplashScreenMenu.CustomMod(nextCharacterIndex, orderOfCharacterBasedOnSpeed.Count);
        CombatCharacter characterToMoveNext = orderOfCharacterBasedOnSpeed[nextCharacterIndex];
        if (currentlyActiveCharacter != null && currentlyActiveCharacter.characterAlliance == CombatCharacter.Alliance.Player)
        {
            for (int i = 0; i < allPlayerCharacters.Count; i++)
            {
                if (currentlyActiveCharacter == allPlayerCharacters[i])
                {
                    StartCoroutine(MoveCombatCharacterToPosition(currentlyActiveCharacter, timeForPlayerCharacterToREachFront, allPlayerPartyCombatSpawnPoints[i].position));
                    break;
                }
            }
        }

        if (currentCombatState != CombatState.CombatInSession)
        {
            return;
        }

        currentActiveAlliance = characterToMoveNext.characterAlliance;
        
        currentlyActiveCharacter = characterToMoveNext;
        currentlActiveCharacterIndex = nextCharacterIndex;
        if (characterToMoveNext.characterAlliance == CombatCharacter.Alliance.Player)
        {
            StartCoroutine(MoveCombatCharacterToPosition(characterToMoveNext, timeForPlayerCharacterToREachFront, activePlayerPosition.position));
            CombatHUD.Instance.OpenPlayerSelectionMenu();
        }
        else
        {
            CombatHUD.Instance.ClosePlayerSelectionMenu();
            SelectAIAction();
        }

    }

    public void SelectAIAction()
    {
        AttackCharacter(allPlayerCharacters[Random.Range(0, allPlayerCharacters.Count)]);
    }

    public void AttackCharacter(CombatCharacter characterToAttack)
    {
        StartCoroutine(AttackCharacter(orderOfCharacterBasedOnSpeed[currentlActiveCharacterIndex], characterToAttack));
    }

    public void DefendCharacter()
    {

    }

    public void SpecialAttackCharacter()
    {

    }


    public void CharacterDied(CombatCharacter characterThatDied)
    {
        orderOfCharacterBasedOnSpeed.Remove(characterThatDied);
        switch (characterThatDied.characterAlliance)
        {
            case CombatCharacter.Alliance.Enemy:
                allEnemyCharacters.Remove(characterThatDied);
                if (allEnemyCharacters.Count == 0)
                {
                    OnPlayerWonCombat();
                }
                break;
            case CombatCharacter.Alliance.Player:
                allPlayerCharacters.Remove(characterThatDied);
                if (allPlayerCharacters.Count == 0)
                {
                    OnPlayerLostCombat();
                }
                break;
        }
    }

    private void OnPlayerWonCombat()
    {
        currentCombatState = CombatState.PlayerWon;
        CombatHUD.Instance.victoryText.gameObject.SetActive(true);
        StartCoroutine(LoadNewScene(sceneToLoadOnVictory));
    }

    private void OnPlayerLostCombat()
    {
        currentCombatState = CombatState.PlayerLost;
        CombatHUD.Instance.defeatText.gameObject.SetActive(true);
        StartCoroutine(LoadNewScene(sceneToLoadOnDefeat));
    }

    private IEnumerator AttackCharacter(CombatCharacter attackingCharacter, CombatCharacter characterBeingAttacked)
    {
        Vector3 previousPosition = attackingCharacter.transform.position;
        float timeToReachCharacter = .2f;
        float timeToReturn = .4f;
        StartCoroutine(MoveCombatCharacterToPosition(attackingCharacter, timeToReachCharacter, characterBeingAttacked.transform.position));
        yield return new WaitForSeconds(timeToReachCharacter);
        HitText hitText = Instantiate<HitText>(CombatHUD.Instance.hitTextPrefab);
        hitText.transform.parent = CombatHUD.Instance.transform;
        hitText.transform.position = Camera.main.WorldToScreenPoint(characterBeingAttacked.pointerPosition.position);
        hitText.SetUpHitText(attackingCharacter.power);
        characterBeingAttacked.TakeDamage(attackingCharacter, attackingCharacter.power);
        StartCoroutine(MoveCombatCharacterToPosition(attackingCharacter, timeToReturn, previousPosition));
        yield return new WaitForSeconds(timeToReturn + .5f);
        SetNextActiveCharacter();
        
    }
    
    private IEnumerator LoadNewScene(UnityEditor.SceneAsset sceneToLoad)
    {
        StartCoroutine(CombatHUD.Instance.FadeInBlackBackground(1));
        yield return new WaitForSeconds(2);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad.name);
    }

    private IEnumerator CharacterGuard(CombatCharacter charachterThatIsGuarding)
    {
        yield break;
    }

    private IEnumerator SpecialAttackCharacter(CombatCharacter characterAttackign, CombatCharacter characterBeingAttacked)
    {
        yield break;
    }

    private IEnumerator MoveCombatCharacterToPosition(CombatCharacter character, float timeToReachPosition, Vector3 goalPosition)
    {
        Vector3 startPosition = character.transform.position;
        float timer = 0;
        while (timer < timeToReachPosition)
        {
            timer += Time.deltaTime;
            character.transform.position = Vector3.Lerp(startPosition, goalPosition, timer / timeToReachPosition);
            yield return null;
        }
        character.transform.position = goalPosition;

    }

    
}
