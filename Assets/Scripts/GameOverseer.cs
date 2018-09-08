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
    public GameState currentGameState { get; private set; }


    private void Awake()
    {
        instance = this;
    }


    public void SetGameState(GameState gameState)
    {
        currentGameState = gameState;
    }
}
