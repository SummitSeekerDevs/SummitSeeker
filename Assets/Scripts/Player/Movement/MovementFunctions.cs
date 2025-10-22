using UnityEngine;

public static class MovementFunctions
{
    public static bool OnSlope(
        Transform transform,
        float playerHeight,
        float maxSlopeAngle,
        out RaycastHit slopeHit
    )
    {
        if (
            Physics.Raycast(
                transform.position,
                Vector3.down,
                out slopeHit,
                playerHeight * 0.5f + 0.3f
            )
        )
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    public static Vector3 GetSlopeMoveDirection(Vector3 moveDirection, RaycastHit slopeHit)
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    public static void HandleDrag(Rigidbody rb, bool isGrounded, float groundDrag)
    {
        rb.linearDamping = isGrounded ? groundDrag : 0;
    }

    public static void SpeedControl(Rigidbody rb, float moveSpeed, bool onSlope, bool exitingSlope)
    {
        // limiting speed on slope
        if (onSlope && !exitingSlope)
        {
            if (rb.linearVelocity.magnitude > moveSpeed)
                rb.linearVelocity = rb.linearVelocity.normalized * moveSpeed;
        }
        // limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
            }
        }
    }

    public static bool IsGrounded(Rigidbody rb, float playerHeight, LayerMask whatIsGround)
    {
        return Physics.Raycast(
            rb.transform.position,
            Vector3.down,
            playerHeight * 0.5f + 0.2f,
            whatIsGround
        );
    }
}
