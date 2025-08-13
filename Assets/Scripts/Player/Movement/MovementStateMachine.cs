using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MovementStateMachine
{
    // References
    public PlayerMovementController _playerMovementController { get; private set; }

    public StateNode Current { get; private set; }
    private Dictionary<Type, StateNode> nodes = new();

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

    public void SetMovementState(IMovementState state)
    {
        Current = nodes[state.GetType()];
        Current.State.Enter();
    }

    #region Init
    private void Initialize()
    {
        CreateStates();
        CreateLinks();
        InitializeStates();

        IMovementState startingState = stateWalking;

        SetMovementState(startingState);
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

    #endregion

    #region Gameloop updates
    public void Update()
    {
        var nextState = GetNextState();
        if (nextState != null)
            TransitionTo(nextState);

        Current.State?.Update();
    }

    public void FixedUpdate(Vector3 moveDirection)
    {
        Current.State?.FixedUpdate(moveDirection);
    }

    #endregion

    #region Transition
    public void AddTransition(IMovementState fromState, ITransitionLink transitionLink)
    {
        GetOrAddNode(fromState).AddTransitionLink(transitionLink);
    }

    public void TransitionTo(IMovementState nextState)
    {
        Current.State.Exit();
        Current = nodes[nextState.GetType()];
        nextState.Enter();
    }

    private IMovementState GetNextState()
    {
        // Check transitions
        foreach (var transitionLink in Current.TransitionLinks)
        {
            if (transitionLink.ConditionMatching(_playerMovementController)) // Wsl effizienter wenn einmal zugewiesen
            {
                return transitionLink.GetLinkTo();
            }
        }

        return null;
    }

    #endregion

    #region StateNode
    private StateNode GetOrAddNode(IMovementState state)
    {
        var node = nodes.GetValueOrDefault(state.GetType());

        if (node == null)
        {
            node = new StateNode(state);
            nodes.Add(state.GetType(), node);
        }

        return node;
    }

    public class StateNode
    {
        public IMovementState State { get; }
        public HashSet<ITransitionLink> TransitionLinks { get; }

        public StateNode(IMovementState state)
        {
            State = state;
            TransitionLinks = new HashSet<ITransitionLink>();
        }

        public void AddTransitionLink(ITransitionLink link)
        {
            TransitionLinks.Add(link);
        }
    }

    #endregion
}
