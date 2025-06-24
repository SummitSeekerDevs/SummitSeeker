using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

[assembly: InternalsVisibleTo("Tests")]

public class CallablePlatform : MonoBehaviour
{
    [SerializeField]
    internal Transform targetPosition;
    internal bool moveToPosition = false;

    [SerializeField]
    private float moveSpeed = 2f;

    private Rigidbody _rb;
    private SignalBus _signalBus;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;

        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = true; // sollte in Rigidbody bereits richtig eingestellt sein

        _signalBus.Subscribe<CallablePlatformStartMovementSignal>(OnPlatformStartMovement);
    }

    public void OnDisable()
    {
        _signalBus.Unsubscribe<CallablePlatformStartMovementSignal>(OnPlatformStartMovement);
    }

    private void FixedUpdate()
    {
        if (moveToPosition)
        {
            MovePlatform();
            TargetPointDistanceCheck();
        }
    }

    private void OnPlatformStartMovement()
    {
        moveToPosition = true;
    }

    #region Movement
    private void MovePlatform()
    {
        Vector3 nextPos = PlatformMover.GetNextPositionTowardsTarget(
            _rb.position,
            targetPosition.position,
            moveSpeed,
            Time.fixedDeltaTime
        );
        _rb.MovePosition(nextPos);
    }

    private void TargetPointDistanceCheck()
    {
        if (PlatformMover.HasReachedTarget(_rb.position, targetPosition.position))
        {
            moveToPosition = false;
        }
    }
    #endregion
}

public class CallablePlatformStartMovementSignal { }
