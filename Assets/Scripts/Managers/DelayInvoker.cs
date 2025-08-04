using System.Collections;
using UnityEngine;

public class DelayInvoker : MonoBehaviour
{
    public virtual void InvokeDelayed(float delay, System.Action action)
    {
        StartCoroutine(InvokeCoroutine(delay, action));
    }

    private IEnumerator InvokeCoroutine(float delay, System.Action action)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
    }
}
