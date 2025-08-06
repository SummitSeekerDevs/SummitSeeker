using System.Collections;
using Moq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

public class ZThrowingKnifeTest : ZenjectIntegrationTestFixture
{
    private ThrowingKnife throwingKnifeScript;
    private GameObject fakePlayer;
    private Transform cam;
    private SignalBus signalBus;

    private void CommonInstall()
    {
        cam = new GameObject().transform;
        Transform attackP = new GameObject().transform;

        fakePlayer = new GameObject();
        throwingKnifeScript = fakePlayer.AddComponent<ThrowingKnife>();

        // References in script
        throwingKnifeScript.cam = cam;
        throwingKnifeScript.attackPoint = attackP;
        throwingKnifeScript.objectToThrow = Resources.Load<GameObject>(
            "Prefabs/TacticalKnife_Gold"
        );

        Container.DeclareSignal<ThrowProjectileSignal>();
        Container.Inject(throwingKnifeScript);

        signalBus = Container.Resolve<SignalBus>();
    }

    [UnityTest]
    public IEnumerator CanThrowKnifeTest()
    {
        PreInstall();

        CommonInstall();

        PostInstall();

        int totalThrows = throwingKnifeScript.totalThrows;

        throwingKnifeScript.Throw();

        Assert.False(throwingKnifeScript.readyToThrow, "Should not be able to throw for a moment");

        yield return new WaitForFixedUpdate();

        // Check existing
        var projectile = GameObject.Find(throwingKnifeScript.objectToThrow.name + "(Clone)");
        Assert.NotNull(projectile, "Projectile should exist in scene");

        // Check Projectile velocity and direction
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        Assert.Greater(
            projectileRb.linearVelocity.magnitude,
            0.1f,
            "Projectile should have a velocity"
        );

        Vector3 expectedDirection = cam.transform.forward.normalized;
        float dot = Vector3.Dot(projectileRb.linearVelocity.normalized, expectedDirection);
        Assert.Greater(dot, 0.95f, "Projectile should fly to cam.forward");

        // Check Projectile Rotation exists
        Assert.Greater(projectileRb.angularVelocity.magnitude, 0.01f, "Projectile should rotate");

        // Check Totalthrows has decreased
        Assert.AreEqual(
            totalThrows - 1,
            throwingKnifeScript.totalThrows,
            "TotalThrows should have reduced"
        );
    }

    [UnityTest]
    public IEnumerator SignalTriggersOnThrowTest()
    {
        PreInstall();

        CommonInstall();

        PostInstall();

        signalBus.Fire<ThrowProjectileSignal>();

        yield return new WaitForFixedUpdate();

        // Check existing
        var projectile = GameObject.Find(throwingKnifeScript.objectToThrow.name + "(Clone)");
        Assert.NotNull(projectile, "Projectile should exist in scene");
    }

    [UnityTest]
    public IEnumerator NotReadyToThrowTest()
    {
        PreInstall();

        CommonInstall();

        PostInstall();

        throwingKnifeScript.readyToThrow = false;

        signalBus.Fire<ThrowProjectileSignal>();

        yield return new WaitForFixedUpdate();

        // Check existing
        var projectile = GameObject.Find(throwingKnifeScript.objectToThrow.name + "(Clone)");
        Assert.Null(projectile, "Projectile should not exist in scene");
    }

    [UnityTest]
    public IEnumerator NoTotalThrowsLeftTest()
    {
        PreInstall();

        CommonInstall();

        PostInstall();

        throwingKnifeScript.totalThrows = 0;

        signalBus.Fire<ThrowProjectileSignal>();

        yield return new WaitForFixedUpdate();

        // Check existing
        var projectile = GameObject.Find(throwingKnifeScript.objectToThrow.name + "(Clone)");
        Assert.Null(projectile, "Projectile should not exist in scene");
    }
}
