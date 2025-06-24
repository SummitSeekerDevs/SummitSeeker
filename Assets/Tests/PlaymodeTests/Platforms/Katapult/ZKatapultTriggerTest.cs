using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

public class ZKatapultTriggerTest : ZenjectIntegrationTestFixture
{
    [UnityTest]
    public IEnumerator KatapultCollisionFiresSignalTest()
    {
        // Player Gameobject
        GameObject Player = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Player.transform.position = new Vector3(3, 0, 0);
        Player.transform.localScale = Vector3.one;
        Player.AddComponent<BoxCollider>();
        Player.tag = "Player";
        Rigidbody PlayerRb = Player.AddComponent<Rigidbody>();
        PlayerRb.useGravity = false;

        GameObject Katapult = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Katapult.transform.position = new Vector3(10, 0, 0);
        Katapult.transform.localScale = Vector3.one;
        Katapult.AddComponent<BoxCollider>();

        // Add KatapultTrigger script
        KatapultTrigger katapultTriggerScript = Katapult.AddComponent<KatapultTrigger>();

        var signalWasFired = false;

        Rigidbody katapultTriggerRigidbody = new Rigidbody();
        float katapultTriggerShootDelay = 0f;
        float katapultTriggerShootUpForce = 0f;

        PreInstall();

        Container.DeclareSignal<KatapultTriggerSignal>();
        Container
            .BindSignal<KatapultTriggerSignal>()
            .ToMethod(signal =>
            {
                signalWasFired = true;
                katapultTriggerRigidbody = signal._rigidbody;
                katapultTriggerShootDelay = signal._shootDelay;
                katapultTriggerShootUpForce = signal._shootUpForce;
            });

        PostInstall();

        PlayerRb.position = Katapult.gameObject.transform.position;

        yield return null;

        Assert.IsTrue(signalWasFired);
        Assert.AreEqual(
            PlayerRb,
            katapultTriggerRigidbody,
            "Transmitted Rigidbody was the same from collision"
        );
        Assert.AreEqual(
            katapultTriggerScript.shootDelay,
            katapultTriggerShootDelay,
            "Signal transmitted right value for shoot delay"
        );
        Assert.AreEqual(
            katapultTriggerScript.shootUpForce,
            katapultTriggerShootUpForce,
            "Signal transmitted right value for shoot up force"
        );
    }
}
