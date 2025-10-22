using System.Collections.Generic;
using UnityEngine;

public interface IMovementState
{
    List<MovementStateMachine.TransitionLinkTypes> GetTransitionsList();
    void Enter();
    void Update();
    void FixedUpdate(Vector3 moveDirection);
    void Exit();
}
