using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private GameState _currentState;
    public GameState CurrentState => _currentState;
    public event Action<GameState> OnGameStateChanged;

    // Input
    private PlayerInput_Actions _playerInputActions;
    public PlayerInput_Actions InputActions => _playerInputActions;

    // Referenced GameObjects
    public GameObject playerGO { get; private set; }

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogWarning("GameManager is null!");
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Debug.LogWarning("_instance of GameManager already exists. Destroying self.");
            Destroy(this);
            return;
        }

        //QualitySettings.vSyncCount = 1; 1 = VSync on 0 = off
        Application.targetFrameRate = 120;

        _instance = this;
        DontDestroyOnLoad(gameObject);

        setPlayerInputActions();
    }

    private void setPlayerInputActions()
    {
        // Set PlayerInput Actions
        try
        {
            _playerInputActions = new PlayerInput_Actions();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to initialize input actions: {e.Message}");
        }
    }

    public void setPlayerGameObject(GameObject player)
    {
        playerGO = player;
    }

    private void OnEnable()
    {
        _playerInputActions.Player.Enable();
    }

    private void OnDisable()
    {
        _playerInputActions.Player.Disable();
    }

    public void ButtonUpdateGameState(int newStateInt)
    {
        if (Enum.IsDefined(typeof(GameState), newStateInt))
        {
            UpdateGameState((GameState)newStateInt);
        }
        else
        {
            Debug.LogError($"Invalid game state value: {newStateInt}");
        }
    }

    public void UpdateGameState(GameState newState)
    {
        if (!IsValidStateTransition(_currentState, newState))
        {
            Debug.LogWarning($"Invalid state transition from {_currentState} to {newState}");
            return;
        }

        _currentState = newState;

        switch (newState)
        {
            case GameState.MainMenu:
                HandleMainMenuState();
                break;
            case GameState.InGame:
                HandleInGameState();
                break;
            case GameState.Pause:
                HandlePauseState();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChanged?.Invoke(newState);
    }

    private bool IsValidStateTransition(GameState from, GameState to)
    {
        // Add state transition validation logic here
        return true;
    }

    private void HandleMainMenuState()
    {
        // Add main menu state logic
    }

    private void HandleInGameState()
    {
        // Add In game state logic
    }

    private void HandlePauseState()
    {
        // Add pause state logic
    }

    public void TogglePause()
    {
        if (_currentState != GameState.Pause)
        {
            Time.timeScale = 0;
            UpdateGameState(GameState.Pause);
        }
        else
        {
            Time.timeScale = 1;
            UpdateGameState(GameState.InGame);
        }
    }
}

public enum GameState
{
    MainMenu,
    InGame,
    Pause,
}
