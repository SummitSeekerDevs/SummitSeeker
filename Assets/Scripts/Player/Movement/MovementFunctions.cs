using UnityEngine;

public class MovementFunctions
{
    private RaycastHit _slopeHit;

    public bool OnSlope(Transform transform, float playerHeight, float maxSlopeAngle)
    {
        if (
            Physics.Raycast(
                transform.position,
                Vector3.down,
                out _slopeHit,
                playerHeight * 0.5f + 0.3f
            )
        )
        {
            float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 moveDirection)
    {
        return Vector3.ProjectOnPlane(moveDirection, _slopeHit.normal).normalized;
    }

    public void HandleDrag(Rigidbody rb, bool isGrounded, float groundDrag)
    {
        rb.linearDamping = isGrounded ? groundDrag : 0;
    }

    public void SpeedControl(Rigidbody rb, float moveSpeed, bool onSlope, bool exitingSlope)
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

    public bool IsGrounded(Rigidbody rb, float playerHeight, LayerMask whatIsGround)
    {
        return Physics.Raycast(
            rb.transform.position,
            Vector3.down,
            playerHeight * 0.5f + 0.2f,
            whatIsGround
        );
    }
}
