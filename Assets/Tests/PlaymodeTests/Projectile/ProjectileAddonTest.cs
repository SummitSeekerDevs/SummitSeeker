using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ProjectileAddonTest
{
    [UnityTest]
    public IEnumerator stickToTargetTest()
    {
        GameObject targetObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        targetObj.transform.position = new Vector3(3, 0, 0);
        targetObj.transform.localScale = Vector3.one;

        GameObject throwableObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        throwableObj.transform.position = new Vector3(0, 0, 2);
        throwableObj.transform.localScale = Vector3.one;
        Rigidbody rb = throwableObj.AddComponent<Rigidbody>();
        ProjectileAddon aoS = throwableObj.AddComponent<ProjectileAddon>();

        Assert.False(aoS.targetHit, "Target not hit yet");
        Assert.False(rb.isKinematic, "Rb is not kinetematic yet");
        Assert.Null(throwableObj.transform.parent, "No parent");

        rb.position = targetObj.transform.position;

        yield return new WaitForFixedUpdate();

        Assert.True(aoS.targetHit, "Target was hit");
        Assert.True(rb.isKinematic, "Rb is kinetematic");
        Assert.AreEqual(
            targetObj.transform,
            throwableObj.transform.parent,
            "Child of Obj stuck on"
        );
    }
}
