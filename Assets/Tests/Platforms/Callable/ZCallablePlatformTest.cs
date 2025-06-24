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

        yield return null;

        // Assertions
        Assert.AreEqual(true, callablePlatformScript.moveToPosition, "Signal triggered movement");
    }
}
