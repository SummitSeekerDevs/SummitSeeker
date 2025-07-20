using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class DelayInvokerTest
{
    [UnityTest]
    public IEnumerator InvokeDelayedTest()
    {
        GameObject delayInvokerObj = GameObject.Instantiate(
            Resources.Load<GameObject>("Prefabs/GameHelperFunctions")
        );

        bool calledAction = false;
        float delay = 1f;

        delayInvokerObj
            .GetComponent<DelayInvoker>()
            .InvokeDelayed(
                delay,
                () =>
                {
                    calledAction = true;
                }
            );

        yield return new WaitForSeconds(delay);

        Assert.True(calledAction, "DelayInvoker called Action");
    }
}
