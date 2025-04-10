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
    public IEnumerator CanUpdateGameStateWithButtonTest()
    {
        GameManager.Instance.ButtonUpdateGameState(1); // 1 = InGame state

        yield return null;

        Assert.AreEqual(GameState.InGame, GameManager.Instance.CurrentState);
    }
}
