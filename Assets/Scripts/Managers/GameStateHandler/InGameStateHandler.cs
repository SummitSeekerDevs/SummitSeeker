using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameStateHandler : IGameStateHandler
{
    private readonly SceneLoader _sceneLoader;
    private readonly string _inGameSceneName = "SampleScene";

    public GameState State => GameState.InGame;

    public InGameStateHandler(SceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader;
    }

    public void OnEnter()
    {
        if (SceneManager.GetActiveScene().name != _inGameSceneName)
        {
            _sceneLoader.LoadSceneWhenReady(_inGameSceneName, null);
        }
    }

    public void OnExit()
    {
        // Cleanup etc.
    }
}
