using UnityEngine;
using Zenject;

public class LinkWalking : ITransitionLink
{
    internal PlayerInputProvider _inputProvider;
    private readonly StateWalking _linkToState;
    private readonly MovementContext _movementContext;

    [Inject]
    public void Injection(PlayerInputProvider inputProvider)
    {
        _inputProvider = inputProvider;
    }

    public LinkWalking(StateWalking linkToState, MovementContext movementContext)
    {
        _linkToState = linkToState;
        _movementContext = movementContext;
    }

    public bool ConditionMatching()
    {
        return (_movementContext.ONGROUND || _movementContext.ONSLOPE)
            && !_inputProvider._sprintingIsPressed
            && !_inputProvider._crouchingIsPressed
            && !_inputProvider._jumpingIsPressed;
    }

    public IMovementState GetLinkTo()
    {
        return _linkToState;
    }
}
