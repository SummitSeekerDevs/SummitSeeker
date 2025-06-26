using UnityEngine;

public interface IGameStateHandler
{
    GameState State { get; }
    void OnEnter();
    void OnExit();
}
