using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Zenject;

[assembly: InternalsVisibleTo("Tests")]

public class GameManager : IInitializable, System.IDisposable
{
    private PlayerInput_Actions _playerInputActions;

    private readonly List<IGameStateHandler> _handlers;
    internal IGameStateHandler _currentHandler;

    public event Action<GameState> OnGameStateChanged;

    public GameManager(List<IGameStateHandler> handlers, PlayerInput_Actions playerInputAction)
    {
        _handlers = handlers;
        _playerInputActions = playerInputAction;
    }

    public void Initialize()
    {
        _playerInputActions.Player.Enable();

        UpdateGameState(GameState.InGame);
    }

    public void Dispose()
    {
        _playerInputActions.Player.Disable();
    }

    public void UpdateGameState(GameState newState)
    {
        if (_currentHandler?.State == newState)
            return;

        _currentHandler?.OnExit();

        _currentHandler = _handlers.FirstOrDefault(h => h.State == newState);

        if (_currentHandler == null)
        {
            throw new Exception($"No handler found for GameState: {newState}");
        }

        _currentHandler.OnEnter();

        OnGameStateChanged?.Invoke(newState);
    }
}

public enum GameState
{
    MainMenu = 0,
    InGame = 1,
    Pause = 2,
    QuitGame = 3,
}
