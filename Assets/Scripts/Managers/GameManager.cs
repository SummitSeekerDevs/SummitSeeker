using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Zenject;

public class GameManager : IInitializable
{
    private readonly List<IGameStateHandler> _handlers;
    private IGameStateHandler _currentHandler;
    private GameState _currentState;

    public event Action<GameState> OnGameStateChanged;

    public GameManager(List<IGameStateHandler> handlers)
    {
        _handlers = handlers;
    }

    public void Initialize()
    {
        UpdateGameState(GameState.InGame);
    }

    public void UpdateGameState(GameState newState)
    {
        if (_currentState == newState)
            return;

        _currentHandler?.OnExit();

        _currentState = newState;
        _currentHandler = _handlers.FirstOrDefault(h => h.State == newState);

        if (_currentHandler == null)
        {
            throw new Exception($"No handler found for GameState: {newState}");
        }

        _currentHandler.OnEnter();

        OnGameStateChanged?.Invoke(newState);
    }
}

public interface IGameStateHandler
{
    GameState State { get; }
    void OnEnter();
    void OnExit();
}

public enum GameState
{
    MainMenu = 0,
    InGame = 1,
    Pause = 2,
    QuitGame = 3,
}
