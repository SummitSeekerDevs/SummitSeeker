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
    private Vector3 target;

    private int nextIndex = 0;
    Rigidbody rb;

    private void Start()
    {
        InitiateVars();
    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            MovePlatform();
            TargetPointDistanceCheckSetNextTarget();
        }
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
        if (Vector3.Distance(rb.position, target) < 0.1f)
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

    /*private void OnTriggerEnter(Collider other) {
        
        if (other.transform.tag == "Player") {
            other.transform.parent = transform;
            Debug.Log(other.transform.parent);

        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.transform.tag == "Player") {
            other.transform.parent = null;
            Debug.Log(other.transform.parent);

        }
    }*/
}
