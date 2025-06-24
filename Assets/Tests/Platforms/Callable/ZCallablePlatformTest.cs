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
        UnityEngine.Debug.Log("vor install");
        PreInstall();

        UnityEngine.Debug.Log("hallo");

        // Container
        //     .Bind<CallablePlatform>()
        //     .FromComponentInNewPrefabResource("Prefabs/Platforms/CallablePlatform")
        //     .AsSingle();

        //Container.DeclareSignal<CallablePlatformStartMovementSignal>();

        PostInstall();

        //var signalBus = Container.Resolve<SignalBus>();
        //signalBus.Fire<CallablePlatformStartMovementSignal>();
        yield break;
        //UnityEngine.Debug.Log(signalBus);

        //var callablePlatformScript = Container.Resolve<CallablePlatform>();88
        var testvar = false;
        // Assertions
        Assert.AreEqual(true, testvar, "Signal triggered movement");
    }
}
