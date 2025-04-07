using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("Tests")]

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
