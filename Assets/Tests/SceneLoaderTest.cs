using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class SceneLoaderTest
{
    private GameObject loaderObject;
    private SceneLoader sceneLoaderScript;
    private bool sceneLoaded;

    [SetUp]
    public void SetUp()
    {
        loaderObject = new GameObject("SceneLoaderTestObj");
        sceneLoaderScript = loaderObject.AddComponent<SceneLoader>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(loaderObject);
    }

    [UnityTest]
    public IEnumerator LoadSceneAsync_LoadSceneSuccessfully()
    {
        sceneLoaded = false;

        yield return sceneLoaderScript.LoadSceneAsync("HomeMenuScene", () => sceneLoaded = true);

        Assert.IsTrue(sceneLoaded);
        Assert.AreEqual("HomeMenuScene", SceneManager.GetActiveScene().name);
    }
}
