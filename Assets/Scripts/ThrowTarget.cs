using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowTarget : MonoBehaviour
{
    public string colliderTag;
    public CallablePlatform connectedPlatform;

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag(colliderTag))
        {
            connectedPlatform.moveToPosition = true;
            this.gameObject.SetActive(false);
        }
    }
}
