using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

[assembly: InternalsVisibleTo("Tests")]

public class ThrowTarget : MonoBehaviour
{
    public string colliderTag;
    public CallablePlatform connectedPlatform;

    private SignalBus _signalBus;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag(colliderTag))
        {
            _signalBus.Fire<CallablePlatformStartMovementSignal>();
            //connectedPlatform.moveToPosition = true;
            this.gameObject.SetActive(false);
        }
    }
}
