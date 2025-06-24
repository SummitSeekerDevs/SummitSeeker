using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class KatapultStunHandlerTest
{
    [Test]
    public void KatapultStunHandlerToggleFreezePlayerPostionTest()
    {
        // Assign
        GameObject go = new GameObject("GameObject");
        Rigidbody rb = go.AddComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        RigidbodyConstraints defaultConstraints = rb.constraints;
        KatapultStunHandler katapultStunHandler = new KatapultStunHandler();

        // Freeze
        // Do
        katapultStunHandler.ToggleFreezePlayerPosition(true, rb);

        // Assert
        Assert.AreEqual(
            RigidbodyConstraints.FreezeAll,
            rb.constraints,
            "Rigidbody constraints are all freezed"
        );

        // Unfreeze
        // Do
        katapultStunHandler.ToggleFreezePlayerPosition(false, rb);

        // Assert
        Assert.AreEqual(
            defaultConstraints,
            rb.constraints,
            "Rigidbody constraints are reset to default"
        );
    }
}
