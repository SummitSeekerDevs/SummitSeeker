using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

[assembly: InternalsVisibleTo("Tests")]

public class MovingPlatform : MonoBehaviour
{
    public float moveSpeed;
    public Transform[] points;
    internal Vector3 target;

    private int nextIndex = 0;
    Rigidbody rb; // Wird benötigt damit sich der Spieler beim draufstehen auch mitbewegt
    internal float distanceToTarget = 0.1f;

    private void Start()
    {
        InitiateVars();
    }

    private void FixedUpdate()
    {
        MovePlatform();
        TargetPointDistanceCheckSetNextTarget();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Player")
        {
            other.transform.parent = transform;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.transform.tag == "Player")
        {
            other.transform.parent = null;
        }
    }

    private void InitiateVars()
    {
        target = points[nextIndex].position;

        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
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
            target,
            moveSpeed * Time.fixedDeltaTime
        );

        return nextPosition;
    }

    private void TargetPointDistanceCheckSetNextTarget()
    {
        if (Vector3.Distance(rb.position, target) < distanceToTarget)
        {
            nextIndex++;

            if (nextIndex >= points.Length)
            {
                nextIndex = 0;
                target = points[nextIndex].position;
            }
            else
            {
                target = points[nextIndex].position;
            }
        }
    }
}
