using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlatformMoverTest
{
    [Test]
    public void HasReachedTarget()
    {
        // Assign
        Vector3 currentPos1 = Vector3.zero;
        Vector3 targetPos1 = Vector3.one;
        Vector3 currentPos2 = Vector3.zero;
        Vector3 targetPos2 = new Vector3(0, 0, PlatformMover.THRESHOLD - 0.01f);

        // Not Reached
        bool reachedTarget1 = PlatformMover.HasReachedTarget(currentPos1, targetPos1);
        Assert.IsFalse(reachedTarget1);

        // Has Reached
        bool reachedTarget2 = PlatformMover.HasReachedTarget(currentPos2, targetPos2);
        Assert.IsTrue(reachedTarget2);
    }
}
