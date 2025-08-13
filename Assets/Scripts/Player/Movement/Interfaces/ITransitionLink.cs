public interface ITransitionLink
{
    bool ConditionMatching(PlayerMovementController playerMC);
    IMovementState GetLinkTo();
}
