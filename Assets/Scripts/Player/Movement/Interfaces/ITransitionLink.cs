public interface ITransitionLink
{
    bool ConditionMatching();
    IMovementState GetLinkTo();
}
