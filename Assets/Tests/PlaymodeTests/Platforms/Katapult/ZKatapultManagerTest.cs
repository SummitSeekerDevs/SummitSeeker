using System;
using System.Collections;
using Moq;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

public class ZKatapultManagerTest : ZenjectIntegrationTestFixture
{
    private SignalBus signalBus;
    private Mock<KatapultShooter> katapultshooterMock;
    private Mock<KatapultStunHandler> katapultstunhandlerMock;
    private Mock<DelayInvoker> delayInvokerMock;

    void CommonInstall()
    {
        // Mock all Dependencies for KatapultManager
        katapultshooterMock = new Mock<KatapultShooter>();
        katapultshooterMock.Setup(m => m.Shoot(It.IsAny<Rigidbody>(), It.IsAny<float>()));

        katapultstunhandlerMock = new Mock<KatapultStunHandler>();
        katapultstunhandlerMock.Setup(m =>
            m.ToggleFreezePlayerPosition(It.IsAny<bool>(), It.IsAny<Rigidbody>())
        );

        delayInvokerMock = new Mock<DelayInvoker>();
        delayInvokerMock
            .Setup(m => m.InvokeDelayed(It.IsAny<float>(), It.IsAny<Action>()))
            .Callback<float, Action>((delay, action) => action());

        // Bind all dependencies
        Container.Bind<KatapultShooter>().FromInstance(katapultshooterMock.Object).AsSingle();
        Container
            .Bind<KatapultStunHandler>()
            .FromInstance(katapultstunhandlerMock.Object)
            .AsSingle();
        Container.Bind<DelayInvoker>().FromInstance(delayInvokerMock.Object).AsSingle();

        // Bind and delare to be listened Signal
        Container.DeclareSignal<KatapultTriggerSignal>();

        // Resolve signalbus
        signalBus = Container.Resolve<SignalBus>();

        // Bind KatapultManager
        Container.BindInterfacesAndSelfTo<KatapultManager>().AsSingle();
    }

    [UnityTest]
    public IEnumerator KatapultManagerSignalTriggeredOKTest()
    {
        PreInstall();

        CommonInstall();

        PostInstall();

        Rigidbody rb = new Rigidbody();

        var signal = new KatapultTriggerSignal(rb, 0.5f, 10f);

        signalBus.Fire<KatapultTriggerSignal>(signal);

        yield return new WaitForFixedUpdate();

        katapultshooterMock.Verify(
            v => v.Shoot(It.IsAny<Rigidbody>(), It.IsAny<float>()),
            Times.Once()
        );

        katapultstunhandlerMock.Verify(v => v.ToggleFreezePlayerPosition(true, rb), Times.Once());
        katapultstunhandlerMock.Verify(v => v.ToggleFreezePlayerPosition(false, rb), Times.Once());

        delayInvokerMock.Verify(
            v => v.InvokeDelayed(It.IsAny<float>(), It.IsAny<Action>()),
            Times.Once()
        );
    }
}
