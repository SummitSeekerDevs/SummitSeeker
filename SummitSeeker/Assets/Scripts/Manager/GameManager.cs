using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState State;
    public event Action<GameState> OnGameStateChanged;
    private bool gameIsPaused = false;

    // Input
    public PlayerInput_Actions _playerInputActions;

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        }
        else {
            Instance = this;
        }

        _playerInputActions = new PlayerInput_Actions();
    }

    void Start() {
        //UpdateGameState(GameState.InGame);
    }

    private void OnEnable() {
        _playerInputActions.Player.Enable();
    }

    private void OnDisable() {
        _playerInputActions.Player.Disable();
    }

    public void ButtonUpdateGameState(int newStateInt) {
        UpdateGameState((GameState)newStateInt);
    }

    public void UpdateGameState(GameState newState) {
        State = newState;

        switch (newState) {
            case GameState.MainMenu:
                break;
            case GameState.InGame:
                break;
            case GameState.Pause:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChanged?.Invoke(newState);
    }

    public void changePause() {
        if (!gameIsPaused) {
            Time.timeScale = 0;
            gameIsPaused = true;
            UpdateGameState(GameState.Pause);
        }
        else {
            Time.timeScale = 1;
            gameIsPaused = false;
            UpdateGameState(GameState.InGame);
        }
    }
    
}

public enum GameState {
    MainMenu,
    InGame,
    Pause
}