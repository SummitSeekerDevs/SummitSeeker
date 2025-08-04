using System.Collections;
using Moq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

public class ZMovingPlatformTest : ZenjectIntegrationTestFixture
{
    private MovingPlatform movingPlatformScript;
    private GameObjectContext GoContext;
    private Mock<PlatformPath> platformPathMock;
    private Rigidbody platRb;

    void CommonInstall()
    {
        // Dependencies für MovingPlatform script mocken und dann binden
        platformPathMock = new Mock<PlatformPath>((Transform[])null);
        platformPathMock.Setup(m => m.GetNextTarget()).Returns(new Vector3(10, 0, 0));
        Container.Bind<PlatformPath>().FromInstance(platformPathMock.Object).AsSingle();

        Container
            .Bind<MovingPlatform>()
            .FromMethod(ctx =>
            {
                var prefab = Resources.Load<GameObject>("Prefabs/Platforms/MovingPlatContainer");
                var instance = ctx.Container.InstantiatePrefab(prefab);
                var comp = instance.GetComponentInChildren<MovingPlatform>();
                ctx.Container.Inject(comp);
                return comp;
            })
            .AsSingle();

        movingPlatformScript = Container.Resolve<MovingPlatform>();

        GoContext = movingPlatformScript.GetComponent<GameObjectContext>();

        platRb = movingPlatformScript.GetComponent<Rigidbody>();
    }

    [UnityTest]
    public IEnumerator PlatformStartsMovingImmediatlyTest()
    {
        PreInstall();

        CommonInstall();

        PostInstall();

        Vector3 startingPos = platRb.position;

        yield return new WaitForFixedUpdate();

        float traveledDist = Vector3.Distance(startingPos, platRb.position);

        Assert.Greater(traveledDist, 0, "Platform hat sich bewegt");
    }

    [UnityTest]
    public IEnumerator PlatformGetsNextTargetOnCloseToCurrentTargetTest()
    {
        PreInstall();

        CommonInstall();

        PostInstall();

        yield return new WaitForFixedUpdate();

        platformPathMock.Verify(m => m.GetNextTarget(), Times.Once());

        Vector3 newTargetPosition = new Vector3(-10, 0, 0);
        platformPathMock.Setup(m => m.GetNextTarget()).Returns(newTargetPosition);

        platRb.position = movingPlatformScript.target;

        yield return new WaitForFixedUpdate();

        Assert.AreEqual(
            newTargetPosition,
            movingPlatformScript.target,
            "New targetposition was returned"
        );

        // Exactly 2 times (1 on init, second on close to target)
        platformPathMock.Verify(m => m.GetNextTarget(), Times.Exactly(2));
    }

    [UnityTest]
    public IEnumerator PlatformTogglesParentingOfPlayerTaggedObjTest()
    {
        PreInstall();

        CommonInstall();

        PostInstall();

        GameObject fakePlayer = GameObject.CreatePrimitive(PrimitiveType.Cube);
        fakePlayer.tag = "Player";
        fakePlayer.transform.localScale = Vector3.one;

        Assert.Null(fakePlayer.transform.parent, "fake player has no parent");

        movingPlatformScript.TogglePlayerParenting(true, fakePlayer.transform);

        yield return new WaitForFixedUpdate();

        Assert.AreEqual(
            movingPlatformScript.transform,
            fakePlayer.transform.parent,
            "MovingPlat is parent of fake player"
        );
    }

    [UnityTest]
    public IEnumerator PlatformTogglesNoParentingOfPlayerTaggedObjTest()
    {
        PreInstall();

        CommonInstall();

        PostInstall();

        GameObject fakePlayer = GameObject.CreatePrimitive(PrimitiveType.Cube);
        fakePlayer.tag = "Player";
        fakePlayer.transform.localScale = Vector3.one;
        fakePlayer.transform.parent = movingPlatformScript.transform;

        Assert.AreEqual(
            movingPlatformScript.transform,
            fakePlayer.transform.parent,
            "MovingPlat is parent of fake player"
        );

        movingPlatformScript.TogglePlayerParenting(false, fakePlayer.transform);

        yield return new WaitForFixedUpdate();

        Assert.Null(fakePlayer.transform.parent, "fake player has no parent");
    }
}
