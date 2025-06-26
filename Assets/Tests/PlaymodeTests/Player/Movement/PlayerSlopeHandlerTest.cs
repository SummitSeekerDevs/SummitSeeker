using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerSlopeHandlerTest
{
    private GameObject groundCube;
    private Transform transform;
    private PlayerSlopeHandler playerSlopeHandler;

    [SetUp]
    public void Setup()
    {
        groundCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        groundCube.transform.position = Vector3.zero;
        groundCube.transform.localScale = new Vector3(10, 1, 10);
        groundCube.transform.rotation = Quaternion.Euler(0, 0, 15);

        transform = new GameObject().transform;
        transform.position = new Vector3(-2, 1f, 0);

        playerSlopeHandler = new PlayerSlopeHandler();
    }

    [UnityTest]
    public IEnumerator IsOnSlopeTest()
    {
        yield return null;

        bool result = playerSlopeHandler.OnSlope(transform, 2, 20);
        Assert.True(result, "Cube is interpreted as slope");
    }

    [UnityTest]
    public IEnumerator OverMaxSlopeAngleIsNotOnSlopeTest()
    {
        yield return null;

        bool result = playerSlopeHandler.OnSlope(transform, 2, 1);
        Assert.False(result, "Cube is not interpreted as slope because of angle");
    }

    [UnityTest]
    public IEnumerator NotTouchingSlopeIsNotOnSlopeTest()
    {
        yield return null;

        bool result = playerSlopeHandler.OnSlope(transform, 0.5f, 20);
        Assert.False(result, "Not on Slope because not touching");
    }

    [TearDown]
    public void TearDown()
    {
        GameObject.DestroyImmediate(groundCube);
        GameObject.DestroyImmediate(transform.gameObject);
    }
}
