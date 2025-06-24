using System.Collections;
using System.Diagnostics;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

public class ZCallablePlatformTest : ZenjectIntegrationTestFixture
{
    [UnityTest]
    public IEnumerator CallablePlatformStartMovementTest()
    {
        PreInstall();

        Container
            .Bind<CallablePlatform>()
            .FromComponentInNewPrefabResource("Prefabs/Platforms/CallablePlatform")
            .AsSingle();

        var callablePlatformScript = Container.Resolve<CallablePlatform>();

        var context = callablePlatformScript.GetComponentInParent<GameObjectContext>();

        PostInstall();

        Assert.AreEqual(false, callablePlatformScript.moveToPosition, "Movement default is off");

        var signalBus = context.Container.Resolve<SignalBus>();
        signalBus.Fire<CallablePlatformStartMovementSignal>();

        yield return null;

        // Assertions
        Assert.AreEqual(true, callablePlatformScript.moveToPosition, "Signal triggered movement");
    }
}
