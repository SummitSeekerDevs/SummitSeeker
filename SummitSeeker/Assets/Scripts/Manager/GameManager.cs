using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState State;
    public event Action<GameState> OnGameStateChanged;

    // Input
    private PlayerInput_Actions _playerInputActions;
    public PlayerInput_Actions InputActions => _playerInputActions;

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
            return;
        }
        else {
            Instance = this;
        }

        // Set PlayerInput Actions
        try {
            _playerInputActions = new PlayerInput_Actions();
        }
        catch (System.Exception e) {
            Debug.LogError($"Failed to initialize input actions: {e.Message}");
        }
        
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

    public void TogglePause() {
        if (State != GameState.Pause) {
            Time.timeScale = 0;
            UpdateGameState(GameState.Pause);
        }
        else {
            Time.timeScale = 1;
            UpdateGameState(GameState.InGame);
        }
    }
    
}

public enum GameState {
    MainMenu,
    InGame,
    Pause
}