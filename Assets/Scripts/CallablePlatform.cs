using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallablePlatform : MonoBehaviour
{
    public Transform targetPosition;
    public float moveSpeed = 2f;
    public bool moveToPosition = false;

    private void FixedUpdate(){
        if (moveToPosition)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition.position,
                moveSpeed * Time.fixedDeltaTime
            );

            if (Vector3.Distance(transform.position, targetPosition.position) < 0.1f)
            {
                moveToPosition = false;
            }
        }
    }
}
