using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

public class ZThrowTargetTest : ZenjectIntegrationTestFixture
{
    [UnityTest]
    public IEnumerator ThrowTargetCollisionFiresSignalTest()
    {
        // Throw Target Obj
        GameObject throwTargetObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        throwTargetObj.transform.position = new Vector3(3, 0, 0);
        throwTargetObj.transform.localScale = Vector3.one;
        throwTargetObj.AddComponent<MeshCollider>();

        // Add ThrowTarget script
        ThrowTarget throwTarget = throwTargetObj.AddComponent<ThrowTarget>();
        throwTarget.colliderTag = "Throwable";

        // Throwing Obj
        GameObject throwableObj = GameObject.Instantiate(
            Resources.Load<GameObject>("Prefabs/TacticalKnife_Gold")
        );

        var signalWasFired = false;

        PreInstall();

        Container.DeclareSignal<CallablePlatformStartMovementSignal>();
        Container
            .BindSignal<CallablePlatformStartMovementSignal>()
            .ToMethod(() => signalWasFired = true);

        PostInstall();

        // Set ThrowableObj Rigidbody to position of ThrowTarget to trigger collision
        Rigidbody throwableObjRB = throwableObj.GetComponent<Rigidbody>();
        throwableObjRB.position = throwTargetObj.gameObject.transform.position;
        throwableObjRB.useGravity = false;

        yield return new WaitForFixedUpdate();

        Assert.AreEqual(true, signalWasFired, "Signal was fired");
    }
}
