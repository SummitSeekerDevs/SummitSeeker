using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

[assembly: InternalsVisibleTo("Tests")]

public class ThrowTarget : MonoBehaviour
{
    [SerializeField]
    private string colliderTag;

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
            this.gameObject.SetActive(false);
        }
    }
}
