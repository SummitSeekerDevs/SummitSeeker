public class MovementStateMachine
{
    // References
    public PlayerMovementController _playerMovementController { get; private set; }

    public IMovementState CurrentState { get; private set; }

    // States
    public StateWalking stateWalking { get; private set; }
    public StateSprinting stateSprinting { get; private set; }
    public StateJumping stateJumping { get; private set; }
    public StateCrouching stateCrouching { get; private set; }
    public StateAir stateAir { get; private set; }

    // Links
    public LinkWalking linkWalking { get; private set; }
    public LinkSprinting linkSprinting { get; private set; }
    public LinkJumping linkJumping { get; private set; }
    public LinkCrouching linkCrouching { get; private set; }
    public LinkAir linkAir { get; private set; }

    public MovementStateMachine(PlayerMovementController playerMovementController)
    {
        _playerMovementController = playerMovementController;
    }

    private void Initialize(IMovementState startingState)
    {
        CreateStates();
        CreateLinks();

        CurrentState = startingState;
        startingState.Enter();
    }

    private void CreateStates()
    {
        stateWalking = new StateWalking(this);
        stateSprinting = new StateSprinting(this);
        stateJumping = new StateJumping(this);
        stateCrouching = new StateCrouching(this);
        stateAir = new StateAir(this);
    }

    private void CreateLinks()
    {
        linkWalking = new LinkWalking(stateWalking);
        linkSprinting = new LinkSprinting(stateSprinting);
        linkJumping = new LinkJumping(stateJumping);
        linkCrouching = new LinkCrouching(stateCrouching);
        linkAir = new LinkAir(stateAir);
    }

    public void TransitionTo(IMovementState nextState)
    {
        CurrentState.Exit();
        CurrentState = nextState;
        nextState.Enter();
    }

    public void Update()
    {
        if (CurrentState != null)
        {
            CurrentState.Update();
        }
    }

    public void FixedUpdate()
    {
        if (CurrentState != null)
        {
            CurrentState.FixedUpdate();
        }
    }
}
