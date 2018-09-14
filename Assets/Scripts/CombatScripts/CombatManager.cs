using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Manager class handles all the logic when a player has entered combat
/// </summary>
public class CombatManager : MonoBehaviour {
    /// <summary>
    /// In any given scene there should only be one instance of the combat manager.
    /// </summary>
    protected static CombatManager instance;

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

    /// <summary>
    /// The current state of the combat session. This will help in ensuring that the player or AI do not make any actions
    /// when they are not meant to
    /// </summary>
    private CombatState currentCombatState;

    public float timeForPlayerCharacterToREachFront = .1f;

    public CombatCharacter.Alliance currentActiveAlliance;
    public CombatCharacter currentlyActiveCharacter;
    public int currentlyActivateCharacter;

    public Transform[] allEnemyCombatSpawnPoints;
    public Transform[] allPlayerPartyCombatSpawnPoints;
    public Transform activePlayerPosition;

    private List<CombatCharacter> orderOfCharacterBasedOnSpeed = new List<CombatCharacter>();
    public List<CombatCharacter> allEnemyCharacters = new List<CombatCharacter>();
    public List<CombatCharacter> allPlayerCharacters = new List<CombatCharacter>();


    public string sceneToLoadOnVictory;
    public string sceneToLoadOnDefeat;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        currentCombatState = CombatState.CombatInSession;
        SetupCombatScenario();
    }

    /// <summary>
    /// Draws the spawn points of where our combat characters will appear
    /// </summary>
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

    /// <summary>
    /// Script that will set up the combat manager
    /// </summary>
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
        currentlyActivateCharacter = -1;
        SetNextActiveCharacter();
        CombatHUD.Instance.FadeBlackBackground();
    }

    /// <summary>
    /// Sets the next active character in the combat order. The order will be created upon setup, typically based on speed. There may be certain scenarios
    /// that set the order of the players based on other conditions
    /// </summary>
    public void SetNextActiveCharacter()
    {
        
        int nextCharacterIndex = currentlyActivateCharacter + 1;
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
        currentlyActivateCharacter = nextCharacterIndex;
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
        PerformCombatAction(allPlayerCharacters[Random.Range(0, allPlayerCharacters.Count)]);
    }

    /// <summary>
    /// When a player has selected an action, this method should be called to perform that action. The components should
    /// include the combat action that was selectd and the character that will be on the receiving end of the action
    /// </summary>
    /// <param name="characterToAttack"></param>
    public void PerformCombatAction(CombatCharacter characterToAttack)
    {
        StartCoroutine(AttackCharacter(orderOfCharacterBasedOnSpeed[currentlyActivateCharacter], characterToAttack));
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
        hitText.SetUpHitText(attackingCharacter.attackPower);
        characterBeingAttacked.TakeDamage(attackingCharacter, attackingCharacter.attackPower);
        StartCoroutine(MoveCombatCharacterToPosition(attackingCharacter, timeToReturn, previousPosition));
        yield return new WaitForSeconds(timeToReturn + .5f);
        SetNextActiveCharacter();
        
    }
    
    private IEnumerator LoadNewScene(string sceneToLoad)
    {
        StartCoroutine(CombatHUD.Instance.FadeInBlackBackground(1));
        yield return new WaitForSeconds(2);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
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
