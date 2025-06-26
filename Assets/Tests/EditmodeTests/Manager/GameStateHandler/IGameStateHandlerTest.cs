using System.Collections;
using Moq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class IGameStateHandlerTest
{
    [UnityTest]
    public IEnumerator InGameStateHandlerTest()
    {
        bool wasCalled = false;
        var sceneLoaderMock = new Mock<SceneLoader>();
        sceneLoaderMock
            .Setup(m => m.LoadSceneWhenReady(It.IsAny<string>(), It.IsAny<System.Action>()))
            .Callback(() => wasCalled = true);

        sceneLoaderMock.Object.LoadSceneWhenReady("DummySceneName", null);

        yield return null;

        Assert.IsTrue(wasCalled, "LoadScene got called");
    }
}
