using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlatformPathTest
{
    GameObject gameObject1,
        gameObject2;
    PlatformPath platformPath;

    [SetUp]
    public void SetUp()
    {
        gameObject1 = new GameObject("GameObject1");
        gameObject1.transform.position = Vector3.zero;

        gameObject2 = new GameObject("GameObject2");
        gameObject2.transform.position = Vector3.one;

        Transform[] transforms = new Transform[2];
        transforms[0] = gameObject1.transform;
        transforms[1] = gameObject2.transform;

        platformPath = new PlatformPath(transforms);
    }

    [TearDown]
    public void TearDown()
    {
        GameObject.DestroyImmediate(gameObject1);
        GameObject.DestroyImmediate(gameObject2);
    }

    [Test]
    public void GetNextTargetTest()
    {
        Vector3 result = platformPath.GetNextTarget();

        Assert.AreEqual(gameObject2.transform.position, result, "Next target is second in array");
    }

    [Test]
    public void GetNextTargetEndArrayTest()
    {
        platformPath.currentIndex = 1;

        Vector3 result = platformPath.GetNextTarget();

        Assert.AreEqual(gameObject1.transform.position, result, "Next target is first in array");
    }
}
