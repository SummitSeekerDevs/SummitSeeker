using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

[assembly: InternalsVisibleTo("Tests")]

public class MovingPlatform : MonoBehaviour
{
    public float moveSpeed;

    internal Vector3 target;

    [SerializeField]
    private Rigidbody _rb; // Wird benötigt damit sich der Spieler beim draufstehen auch mitbewegt
    PlatformPath _platformPath;

    [Inject]
    public void Construct(PlatformPath platformPath)
    {
        _rb.isKinematic = true; // sollte in Rigidbody bereits richtig eingestellt sein

        _platformPath = platformPath;

        target = _platformPath.GetNextTarget();
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

    private void MovePlatform()
    {
        Vector3 nextPos = PlatformMover.GetNextPositionTowardsTarget(
            _rb.position,
            target,
            moveSpeed,
            Time.fixedDeltaTime
        );
        _rb.MovePosition(nextPos);
    }

    private void TargetPointDistanceCheckSetNextTarget()
    {
        if (PlatformMover.HasReachedTarget(_rb.position, target))
        {
            target = _platformPath.GetNextTarget();
        }
    }
}
