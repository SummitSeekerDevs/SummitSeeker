using UnityEngine;
using Zenject;

public class KatapultTrigger : MonoBehaviour
{
    [SerializeField]
    private float shootDelay = 4f;

    [SerializeField]
    private float shootUpForce = 50f;
    private SignalBus _signalBus;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            _signalBus.Fire<KatapultTriggerSignal>(
                new KatapultTriggerSignal(collision.rigidbody, shootDelay, shootUpForce)
            );
        }
    }
}

public class KatapultTriggerSignal
{
    public readonly Rigidbody _rigidbody;
    public readonly float _shootDelay;
    public readonly float _shootUpForce;

    public KatapultTriggerSignal(Rigidbody rigidbody, float shootDelay, float shootUpForce)
    {
        _rigidbody = rigidbody;
        _shootDelay = shootDelay;
        _shootUpForce = shootUpForce;
    }
}
