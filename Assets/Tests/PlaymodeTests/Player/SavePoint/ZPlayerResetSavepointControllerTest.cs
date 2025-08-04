using System.Collections;
using Moq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

public class ZPlayerResetSavepointControllerTest : ZenjectIntegrationTestFixture
{
    private Mock<SavePointState> savePointStateMock;
    private SignalBus signalBus;
    private Rigidbody fakePlayer_rb;

    void CommonInstall()
    {
        // Create fakePlayerObj with nessecary Components
        GameObject fakePlayer = GameObject.CreatePrimitive(PrimitiveType.Cube);
        fakePlayer_rb = fakePlayer.AddComponent<Rigidbody>();
        fakePlayer_rb.useGravity = false;
        fakePlayer_rb.position = new Vector3(10, 0, 0);
        PlayerResetSavePointController prsc_script =
            fakePlayer.AddComponent<PlayerResetSavePointController>();

        // Create and bind savePointState Mock
        savePointStateMock = new Mock<SavePointState>();
        Container.Bind<SavePointState>().FromInstance(savePointStateMock.Object).AsSingle();

        // Declare Signal and resolve signalbus
        Container.DeclareSignal<ResetToSavePointSignal>();
        signalBus = Container.Resolve<SignalBus>();

        // Inject Dependencies to script
        Container.Inject(prsc_script);
    }

    [UnityTest]
    public IEnumerator AvailableSavepointTest()
    {
        PreInstall();

        CommonInstall();

        PostInstall();

        // Create dummy obj to use dummyTransform
        Transform dummyTransform = new GameObject().transform;

        savePointStateMock.Setup(m => m.ConsumeSavePoint()).Returns(dummyTransform);

        signalBus.Fire<ResetToSavePointSignal>();

        yield return new WaitForFixedUpdate();

        savePointStateMock.Verify(m => m.ConsumeSavePoint(), Times.Once());

        Assert.AreEqual(
            dummyTransform.position,
            fakePlayer_rb.position,
            "dummy transform and fake player positions matching"
        );
    }

    [UnityTest]
    public IEnumerator SavepointNotAvailableTest()
    {
        PreInstall();

        CommonInstall();

        PostInstall();

        Vector3 rbStartingPosition = fakePlayer_rb.position;

        savePointStateMock.Setup(m => m.ConsumeSavePoint()).Returns((Transform)null);

        signalBus.Fire<ResetToSavePointSignal>();

        yield return new WaitForFixedUpdate();

        savePointStateMock.Verify(m => m.ConsumeSavePoint(), Times.Once());

        Assert.AreEqual(
            rbStartingPosition,
            fakePlayer_rb.position,
            "fake player not reset to a savepoint because it is not existing"
        );
    }
}
