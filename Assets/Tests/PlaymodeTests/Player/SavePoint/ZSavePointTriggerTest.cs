using System.Collections;
using Moq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

public class ZSavePointTriggerTest : ZenjectIntegrationTestFixture
{
    private Mock<SavePointState> savePointStateMock;
    private SavePointTrigger sptScript;
    private GameObject throwableObj,
        savePointObject;

    private void CommonInstall()
    {
        // Create Mock and bind to zenject
        savePointStateMock = new Mock<SavePointState>();
        savePointStateMock.Setup(m => m.SetActiveSavePoint(It.IsAny<Transform>()));
        Container.Bind<SavePointState>().FromInstance(savePointStateMock.Object).AsSingle();

        // Create Object with SavePointTrigger
        savePointObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        savePointObject.GetComponent<Collider>().isTrigger = true;
        sptScript = savePointObject.AddComponent<SavePointTrigger>();
        savePointObject.transform.position = new Vector3(3, 0, 0);
        savePointObject.transform.localScale = Vector3.one;

        // Inject Mock as Binding to SavePointTrigger
        Container.Inject(sptScript);

        // Create Transform as actual Savepoint location
        Transform dummyTransform = new GameObject().transform;
        dummyTransform.position = new Vector3(10, 0, 0);

        // Set SavePointTrigger script vars
        sptScript.savePoint = dummyTransform;
        sptScript.colliderTag = "Throwable";

        // Throwing Obj
        // tagged with Throwable
        throwableObj = GameObject.Instantiate(
            Resources.Load<GameObject>("Prefabs/TacticalKnife_Gold")
        );
    }

    [UnityTest]
    public IEnumerator ActivatingSavePointTest()
    {
        PreInstall();

        CommonInstall();

        PostInstall();

        Assert.False(sptScript._usedSavePoint, "Savepoint not activated yet");

        throwableObj.GetComponent<Rigidbody>().position = savePointObject.transform.position;

        yield return new WaitForFixedUpdate();

        Assert.True(sptScript._usedSavePoint, "Savepoint activated");

        savePointStateMock.Verify(m => m.SetActiveSavePoint(It.IsAny<Transform>()), Times.Once());

        yield return null;
    }

    [UnityTest]
    public IEnumerator SavePointAlreadyUsedTest()
    {
        PreInstall();

        CommonInstall();

        PostInstall();

        sptScript._usedSavePoint = true;

        throwableObj.GetComponent<Rigidbody>().position = savePointObject.transform.position;

        yield return new WaitForFixedUpdate();

        Assert.True(sptScript._usedSavePoint, "Savepoint already used");

        savePointStateMock.Verify(m => m.SetActiveSavePoint(It.IsAny<Transform>()), Times.Never());
    }

    [UnityTest]
    public IEnumerator OtherColliderTagTest()
    {
        PreInstall();

        CommonInstall();

        PostInstall();

        sptScript.colliderTag = "Finish";

        throwableObj.GetComponent<Rigidbody>().position = savePointObject.transform.position;

        yield return new WaitForFixedUpdate();

        savePointStateMock.Verify(m => m.SetActiveSavePoint(It.IsAny<Transform>()), Times.Never());
    }
}
