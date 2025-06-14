using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovingPlatform : MonoBehaviour
{
    public float moveSpeed;
    public Transform[] points;
    private Vector3 target;

    private int nextIndex = 0;
    Rigidbody rb;

    private void Start()
    {
        target = points[nextIndex].position;

        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            Vector3 currentPosition = rb.position;
            Vector3 nextPosition = Vector3.MoveTowards(
                currentPosition,
                target,
                moveSpeed * Time.fixedDeltaTime
            );

            rb.MovePosition(nextPosition);

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
}
