using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CallablePlatform : MonoBehaviour
{
    public Transform targetPosition;
    public float moveSpeed = 2f;
    public bool moveToPosition = false;
    internal float distanceToTarget = 0.1f;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    private void FixedUpdate()
    {
        if (moveToPosition)
        {
            MovePlatform();
            TargetPointDistanceCheckSetNextTarget();
        }
    }

    private void MovePlatform()
    {
        Vector3 nextPos = getNextPositionTowardsTarget();
        rb.MovePosition(nextPos);
    }

    internal Vector3 getNextPositionTowardsTarget()
    {
        Vector3 currentPosition = rb.position;
        Vector3 nextPosition = Vector3.MoveTowards(
            currentPosition,
            targetPosition.position,
            moveSpeed * Time.fixedDeltaTime
        );

        return nextPosition;
    }

    private void TargetPointDistanceCheckSetNextTarget()
    {
        if (Vector3.Distance(rb.position, targetPosition.position) < distanceToTarget)
        {
            moveToPosition = false;
        }
    }
}
