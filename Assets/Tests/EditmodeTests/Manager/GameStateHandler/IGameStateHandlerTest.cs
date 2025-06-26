using System.Collections;
using Moq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class IGameStateHandlerTest
{
    private bool loadSceneWasCalled = false;
    private Mock<SceneLoader> sceneLoaderMock;

    [SetUp]
    public void Setup()
    {
        sceneLoaderMock = new Mock<SceneLoader>();
        sceneLoaderMock
            .Setup(m => m.LoadSceneWhenReady(It.IsAny<string>(), It.IsAny<System.Action>()))
            .Callback(() => loadSceneWasCalled = true);
    }

    [UnityTest]
    public IEnumerator InGameStateHandlerOnEnterTest()
    {
        InGameStateHandler inGameStateHandler = new InGameStateHandler(sceneLoaderMock.Object);
        inGameStateHandler.OnEnter();

        yield return null;

        Assert.IsTrue(loadSceneWasCalled, "LoadScene got called");
    }

    [UnityTest]
    public IEnumerator MainMenuStateHandlerOnEnterTest()
    {
        MainMenuStateHandler mainMenuStateHandler = new MainMenuStateHandler(
            sceneLoaderMock.Object
        );
        mainMenuStateHandler.OnEnter();

        yield return null;

        Assert.IsTrue(loadSceneWasCalled, "LoadScene got called");

        Assert.AreEqual(30, Application.targetFrameRate, "TargetFramerate of main menu");
        Assert.AreEqual(0, QualitySettings.vSyncCount, "VSyncCount of main menu");
    }

    [TearDown]
    public void TearDown()
    {
        loadSceneWasCalled = false;
    }
}
