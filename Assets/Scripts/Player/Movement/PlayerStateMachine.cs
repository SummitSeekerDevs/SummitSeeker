using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("EditmodeTests")]

public class PlayerStateMachine
{
    public MovementState _currentState { get; internal set; }

    public enum MovementState
    {
        Walking,
        Sprinting,
        Crouching,
        Air,
    }

    public void UpdateMovementState(bool isGrounded, bool isCrouching, bool isSprinting)
    {
        // Mode - Air
        if (!isGrounded)
        {
            _currentState = MovementState.Air;
        }
        // Mode - Crouching
        else if (isGrounded && isCrouching)
        {
            _currentState = MovementState.Crouching;
        }
        // Mode - Sprinting
        else if (isGrounded && isSprinting && !isCrouching)
        {
            _currentState = MovementState.Sprinting;
        }
        // Mode - Walking
        else if (isGrounded && !isSprinting && !isCrouching)
        {
            _currentState = MovementState.Walking;
        }
    }
}
