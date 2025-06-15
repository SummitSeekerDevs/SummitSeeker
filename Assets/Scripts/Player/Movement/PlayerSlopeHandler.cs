using UnityEngine;

public class PlayerSlopeHandler
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
}
