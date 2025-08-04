using System.Collections;
using System.Diagnostics;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

public class ZCallablePlatformTest : ZenjectIntegrationTestFixture
{
    CallablePlatform callablePlatformScript;
    GameObjectContext GoContext;

    void CommonInstall()
    {
        Container
            .Bind<CallablePlatform>()
            .FromComponentInNewPrefabResource("Prefabs/Platforms/CallablePlatform")
            .AsSingle();

        callablePlatformScript = Container.Resolve<CallablePlatform>();

        GoContext = callablePlatformScript.GetComponentInParent<GameObjectContext>();
    }

    [UnityTest]
    public IEnumerator CallablePlatformStartMovementTest()
    {
        PreInstall();

        CommonInstall();

        PostInstall();

        Assert.AreEqual(false, callablePlatformScript.moveToPosition, "Movement default is off");

        var signalBus = GoContext.Container.Resolve<SignalBus>();
        signalBus.Fire<CallablePlatformStartMovementSignal>();

        yield return new WaitForFixedUpdate();

        // Assertions
        Assert.AreEqual(true, callablePlatformScript.moveToPosition, "Signal triggered movement");
    }

    [UnityTest]
    public IEnumerator CallablePlatformMovingTest()
    {
        PreInstall();

        CommonInstall();

        PostInstall();

        // Assign
        var rb = callablePlatformScript.gameObject.GetComponent<Rigidbody>();
        var targetTransform = callablePlatformScript.targetPosition;

        var distanceBefore = Vector3.Distance(rb.position, targetTransform.position);

        // Let Platform move and wait for fixed update to allow update logic for CallablePlatform to run
        callablePlatformScript.moveToPosition = true;
        yield return new WaitForFixedUpdate();

        // Get end distance to target
        var distanceAfter = Vector3.Distance(rb.position, targetTransform.position);

        // Asssertions
        Assert.Less(distanceAfter, distanceBefore, "Distance to Target Postion");
    }

    [UnityTest]
    public IEnumerator CallablePlatformStopMovingOnCloseToTargetTest()
    {
        PreInstall();

        CommonInstall();

        PostInstall();

        // Assign
        var rb = callablePlatformScript.gameObject.GetComponent<Rigidbody>();
        var targetTransform = callablePlatformScript.targetPosition;

        // Let Platform move and wait for fixed update to allow update logic for CallablePlatform to run
        callablePlatformScript.moveToPosition = true;
        yield return new WaitForFixedUpdate();

        Vector3 velocity = rb.linearVelocity;

        Assert.AreEqual(true, callablePlatformScript.moveToPosition, "Platform is moving");
        Assert.Greater(
            Mathf.Abs(velocity.sqrMagnitude),
            Vector3.zero.sqrMagnitude,
            "Velocity shows movement"
        );

        rb.position = targetTransform.position;

        yield return new WaitForFixedUpdate();

        Vector3 velocityAfter = rb.linearVelocity;

        Assert.AreEqual(
            false,
            callablePlatformScript.moveToPosition,
            "Platform stopped moving on close to target"
        );

        Assert.AreEqual(
            Vector3.zero.sqrMagnitude,
            Mathf.Abs(velocityAfter.sqrMagnitude),
            "Platform velocity stopped"
        );
    }
}
