using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SavePointStateTest
{
    [Test]
    public void SavePointStateTestSimplePasses()
    {
        SavePointState savePointState = new SavePointState();
        Transform dummyTransform = new GameObject().transform;
        dummyTransform.position = Vector3.one;

        savePointState.SetActiveSavePoint(dummyTransform);

        Assert.AreEqual(
            dummyTransform,
            savePointState._activeSavePoint,
            "Active Savepoint is dummyTransform"
        );

        Transform result = savePointState.ConsumeSavePoint();
        Assert.IsNull(savePointState._activeSavePoint);
        Assert.AreEqual(dummyTransform, result, "Consumed Savepoint is dummyTransform");
    }
}
