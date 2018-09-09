using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// General purpose overseer that will manage the current gamestate and any important objects in the game
/// </summary>
public class GameOverseer : MonoBehaviour {
    private static GameOverseer instance;
    public static GameOverseer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<GameOverseer>();
            }

            return instance;
        }
    }

    public enum GameState
    {
        GAME_PLAYING,
        MENU_OPEN,
    }

    public Vector3 playerPosition { get; set; }
    public bool[] enemiesDeadList;

    public GameState currentGameState { get; private set; }
    public EnemyNPCEncounter[] allEnemyNPCs = new EnemyNPCEncounter[10];

    [Header("Combat Character Prefabs")]
    public CombatCharacter foxCombatPrefab;
    public CombatCharacter bearCombatPrefab;
    public CombatCharacter owlCombatPrefab;
    [Space(3)]
    public CombatCharacter golemCombatPrefab;
    public CombatCharacter slimeCombatPrefab;
    public CombatCharacter mushroomCombatPrefab;
#if UNITY_EDITOR
    public UnityEditor.SceneAsset encounterScene;
#endif

    private void Start()
    {
        if (instance != null)
        {
            instance.allEnemyNPCs = this.allEnemyNPCs;
            SetUpForReloadScene();
            Destroy(this.gameObject);
            PlayerStats.Instance.triggerBoxCollider.enabled = true;
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        enemiesDeadList = new bool[allEnemyNPCs.Length];
        
    }

    public void SetUpForReloadScene()
    {
        int countEnemiesDead = 0;
        PlayerStats.Instance.transform.position = instance.playerPosition;
        for (int i = 0; i < instance.enemiesDeadList.Length; i++)
        {
            if (instance.enemiesDeadList[i])
            {
                countEnemiesDead++;
                allEnemyNPCs[i].gameObject.SetActive(false);
            }
        }
        if (countEnemiesDead >= allEnemyNPCs.Length)
        {
            OnPlayerWonGame();
        }
    }

    private void OnPlayerWonGame()
    {

    }


    public void SetGameState(GameState gameState)
    {
        currentGameState = gameState;
    }
}
