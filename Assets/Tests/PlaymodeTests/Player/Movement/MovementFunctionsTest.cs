using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MovementFunctionsTest
{
    private MovementFunctions movementFunctions;
    private GameObject fakePlayer;
    private Rigidbody fakePlayerRb;

    [SetUp]
    public void Setup()
    {
        movementFunctions = new MovementFunctions();
        fakePlayer = GameObject.CreatePrimitive(PrimitiveType.Cube);
        fakePlayer.transform.position = Vector3.zero;
        fakePlayerRb = fakePlayer.AddComponent<Rigidbody>();

        fakePlayerRb.useGravity = false;
    }

    [UnityTest]
    public IEnumerator LimitSpeedOnSlopeTest()
    {
        fakePlayerRb.linearVelocity = new Vector3(30, 30, 30);
        var normalizedVector = fakePlayerRb.linearVelocity.normalized;

        float moveSpeed = 2;

        movementFunctions.SpeedControl(fakePlayerRb, moveSpeed, true, false);

        yield return new WaitForFixedUpdate();

        Assert.AreEqual(
            normalizedVector * moveSpeed,
            fakePlayerRb.linearVelocity,
            "Velocity should be adapted"
        );
    }

    [UnityTest]
    public IEnumerator LimitSpeedOnGroundAndAirTest()
    {
        Vector3 startVector = new Vector3(30, 30, 30);
        fakePlayerRb.linearVelocity = startVector;

        float moveSpeed = 2;

        movementFunctions.SpeedControl(fakePlayerRb, moveSpeed, false, true);

        yield return new WaitForFixedUpdate();

        Assert.Less(
            fakePlayerRb.linearVelocity.magnitude,
            startVector.magnitude,
            "Velocity should be smaller than before"
        );
    }

    [TearDown]
    public void TearDown()
    {
        GameObject.DestroyImmediate(fakePlayer);
    }
}
