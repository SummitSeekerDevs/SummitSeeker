using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuStateHandler : IGameStateHandler
{
    private readonly SceneLoader _sceneLoader;
    private readonly string _mainMenuSceneName = "MainMenu";

    public GameState State => GameState.MainMenu;

    public MainMenuStateHandler(SceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader;
    }

    public void OnEnter()
    {
        Application.targetFrameRate = 30;
        QualitySettings.vSyncCount = 0;

        if (SceneManager.GetActiveScene().name != _mainMenuSceneName)
        {
            _sceneLoader.LoadSceneWhenReady(_mainMenuSceneName, null);
        }
    }

    public void OnExit()
    {
        // Cleanup etc.
    }
}
