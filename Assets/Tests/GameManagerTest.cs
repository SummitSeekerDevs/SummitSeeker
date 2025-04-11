using System.Collections;
using NUnit.Framework;
using Unity.Properties;
using UnityEngine;
using UnityEngine.TestTools;

public class GameManagerTest
{
    private GameObject gameManager;
    private GameManager gameManagerScript;

    private int DEFAULT_FPS = 120,
        DEFAULT_VSYNC_MODE = 0;
    private string DONT_DESTROY_ON_LOAD_SCENE = "DontDestroyOnLoad";

    private void setGameManager()
    {
        if (GameManager.Instance != null)
        {
            gameManager = GameManager.Instance.gameObject;
        }
        else
        {
            gameManager = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/GameManager"));
        }
    }

    [SetUp]
    public void SetUp()
    {
        setGameManager();
    }

    [UnityTest]
    public IEnumerator GameManagerInitTest()
    {
        yield return null;

        Assert.AreEqual(DEFAULT_FPS, Application.targetFrameRate, "Framerate is set to default");
        Assert.AreEqual(
            DEFAULT_VSYNC_MODE,
            QualitySettings.vSyncCount,
            "Vsync mode is set to default"
        );

        Assert.AreEqual(
            DONT_DESTROY_ON_LOAD_SCENE,
            gameManager.scene.name,
            "GameManager is part of DontDestroyOnLoad scene"
        );

        Assert.NotNull(GameManager.Instance.InputActions, "Player InputActions are available");
        Assert.AreEqual(
            true,
            GameManager.Instance.InputActions.Player.enabled,
            "Player InputAction is enabled"
        );
    }

    [UnityTest]
    public IEnumerator CanUpdateGameStateWithButtonAllStatesTest()
    {
        // Main Menu
        GameManager.Instance.ButtonUpdateGameState(0);

        yield return null;

        Assert.AreEqual(GameState.MainMenu, GameManager.Instance.CurrentState);

        // In Game
        GameManager.Instance.ButtonUpdateGameState(1);

        yield return null;

        Assert.AreEqual(GameState.InGame, GameManager.Instance.CurrentState);

        // Pause
        GameManager.Instance.ButtonUpdateGameState(2);

        yield return null;

        Assert.AreEqual(GameState.Pause, GameManager.Instance.CurrentState);
    }

    [UnityTest]
    public IEnumerator TogglePauseTest()
    {
        GameManager.Instance.UpdateGameState(GameState.InGame);

        GameManager.Instance.TogglePause();

        yield return null;

        Assert.AreEqual(0, Time.timeScale, "Game time is paused");
        Assert.AreEqual(
            GameState.Pause,
            GameManager.Instance.CurrentState,
            "Gamestate toggled to pause"
        );

        GameManager.Instance.TogglePause();

        yield return null;

        Assert.AreEqual(1, Time.timeScale, "Game time is resumed");
        Assert.AreEqual(
            GameState.InGame,
            GameManager.Instance.CurrentState,
            "Gamestate toggled to inGame"
        );
    }
}
