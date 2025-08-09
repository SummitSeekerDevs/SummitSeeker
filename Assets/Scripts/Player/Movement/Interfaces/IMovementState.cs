using UnityEngine;

public interface IMovementState
{
    void Enter();
    void Update();
    void FixedUpdate(Vector3 moveDirection);
    void Exit();
}

public interface ILink
{
    bool ConditionMatching(PlayerMovementController playerMC);
    IMovementState GetLinkTo();
}
