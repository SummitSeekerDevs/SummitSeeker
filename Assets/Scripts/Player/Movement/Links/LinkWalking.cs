using UnityEngine;
using Zenject;

public class LinkWalking : ITransitionLink
{
    internal PlayerInputProvider _inputProvider;
    private readonly StateWalking _linkToState;

    [Inject]
    public void Injection(PlayerInputProvider inputProvider)
    {
        _inputProvider = inputProvider;
    }

    public LinkWalking(StateWalking linkToState)
    {
        _linkToState = linkToState;
    }

    public bool ConditionMatching(PlayerMovementController playerMC)
    {
        return (playerMC.MOVEMENTCONTEXT.ONGROUND || playerMC.MOVEMENTCONTEXT.ONSLOPE)
            && !_inputProvider._sprintingIsPressed
            && !_inputProvider._crouchingIsPressed
            && !_inputProvider._jumpingIsPressed;
    }

    public IMovementState GetLinkTo()
    {
        return _linkToState;
    }
}
