using UnityEngine;
using Zenject;

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

    private DiContainer _diContainer;

    [Inject]
    public void Construct(
        PlayerMovementController playerMovementController,
        DiContainer diContainer
    )
    {
        _playerMovementController = playerMovementController;
        Debug.Log(_playerMovementController);
        _diContainer = diContainer;
        Initialize();
    }

    private void Initialize()
    {
        CreateStates();
        CreateLinks();
        InitializeStates();

        IMovementState startingState = stateWalking;

        CurrentState = startingState;
        startingState.Enter();
    }

    private void CreateStates()
    {
        // Instantiation
        stateWalking = new StateWalking(this);
        stateSprinting = new StateSprinting(this);
        stateJumping = new StateJumping(this);
        stateCrouching = new StateCrouching(this);
        stateAir = new StateAir(this);

        // Injecting
        _diContainer.Inject(stateJumping);
    }

    private void CreateLinks()
    {
        // Instantiation
        linkWalking = new LinkWalking(stateWalking);
        linkSprinting = new LinkSprinting(stateSprinting);
        linkJumping = new LinkJumping(stateJumping);
        linkCrouching = new LinkCrouching(stateCrouching);
        linkAir = new LinkAir(stateAir);

        // Injecting
        _diContainer.Inject(linkWalking);
        _diContainer.Inject(linkSprinting);
        _diContainer.Inject(linkJumping);
        _diContainer.Inject(linkCrouching);
    }

    private void InitializeStates()
    {
        stateWalking.Initialize();
        stateSprinting.Initialize();
        stateJumping.Initialize();
        stateCrouching.Initialize();
        stateAir.Initialize();
    }

    public void TransitionTo(IMovementState nextState)
    {
        CurrentState.Exit();
        CurrentState = nextState;
        nextState.Enter();
    }

    public void Update()
    {
        CurrentState?.Update();
    }

    public void FixedUpdate(Vector3 moveDirection)
    {
        CurrentState?.FixedUpdate(moveDirection);
    }
}
