﻿using System.Collections;
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
    public GameState currentGameState { get; private set; }

    [Header("Combat Character Prefabs")]
    public CombatCharacter foxCombatPrefab;
    public CombatCharacter bearCombatPrefab;
    public CombatCharacter owlCombatPrefab;
    [Space(3)]
    public CombatCharacter golemCombatPrefab;
    public CombatCharacter slimeCombatPrefab;
    public CombatCharacter mushroomCombatPrefab;


    private void Awake()
    {
        instance = this;
    }


    public void SetGameState(GameState gameState)
    {
        currentGameState = gameState;
    }
}
