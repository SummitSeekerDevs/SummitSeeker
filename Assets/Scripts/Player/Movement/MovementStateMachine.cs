using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MovementStateMachine
{
    public StateNode Current { get; private set; }
    private Dictionary<Type, StateNode> nodes = new();

    #region State and Link Vars
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

    #endregion

    private DiContainer _diContainer;
    private DelayInvoker _delayInvoker;

    public MovementStateMachine(
        MovementContext movementContext,
        PlayerMovementConfig movementConfig,
        DelayInvoker delayInvoker,
        DiContainer diContainer
    )
    {
        _diContainer = diContainer;
        _delayInvoker = delayInvoker;
        Initialize(movementContext, movementConfig);
    }

    public void SetMovementState(IMovementState state)
    {
        Current = nodes[state.GetType()];
        Current.State.Enter();
    }

    #region Init
    private void Initialize(MovementContext movementContext, PlayerMovementConfig movementConfig)
    {
        CreateStates(movementContext, movementConfig);
        CreateLinks(movementContext);
        RegisterStateTransitions();

        IMovementState startingState = stateWalking;
        SetMovementState(startingState);
    }

    private void CreateStates(MovementContext movementContext, PlayerMovementConfig movementConfig)
    {
        // Instantiation
        stateWalking = new StateWalking(movementContext, movementConfig);
        stateSprinting = new StateSprinting(movementContext, movementConfig);
        stateJumping = new StateJumping(movementContext, movementConfig, _delayInvoker);
        stateCrouching = new StateCrouching(movementContext, movementConfig);
        stateAir = new StateAir(movementContext, movementConfig);
    }

    private void CreateLinks(MovementContext movementContext)
    {
        // Instantiation
        linkWalking = new LinkWalking(stateWalking, movementContext);
        linkSprinting = new LinkSprinting(stateSprinting, movementContext);
        linkJumping = new LinkJumping(stateJumping, movementContext);
        linkCrouching = new LinkCrouching(stateCrouching, movementContext);
        linkAir = new LinkAir(stateAir, movementContext);

        // Injecting
        _diContainer.Inject(linkWalking);
        _diContainer.Inject(linkSprinting);
        _diContainer.Inject(linkJumping);
        _diContainer.Inject(linkCrouching);
    }

    private void RegisterStateTransitions()
    {
        SetStateTransitions(stateWalking, stateWalking.GetTransitionsList());
        SetStateTransitions(stateSprinting, stateSprinting.GetTransitionsList());
        SetStateTransitions(stateJumping, stateJumping.GetTransitionsList());
        SetStateTransitions(stateCrouching, stateCrouching.GetTransitionsList());
        SetStateTransitions(stateAir, stateAir.GetTransitionsList());
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
            if (transitionLink.ConditionMatching())
            {
                return transitionLink.GetLinkTo();
            }
        }

        return null;
    }

    private void SetStateTransitions(IMovementState state, List<TransitionLinkTypes> transitions)
    {
        foreach (var transition in transitions)
        {
            ITransitionLink transitionLink = null;

            switch (transition)
            {
                case TransitionLinkTypes.LinkAir:
                    transitionLink = linkAir;
                    break;
                case TransitionLinkTypes.LinkCrouching:
                    transitionLink = linkCrouching;
                    break;
                case TransitionLinkTypes.LinkJumping:
                    transitionLink = linkJumping;
                    break;
                case TransitionLinkTypes.LinkSprinting:
                    transitionLink = linkSprinting;
                    break;
                case TransitionLinkTypes.LinkWalking:
                    transitionLink = linkWalking;
                    break;
            }

            if (transitionLink != null)
            {
                AddTransition(state, transitionLink);
                continue;
            }

            throw new NullReferenceException($"No TransitionLink Found for {transition}");
        }
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

    public enum TransitionLinkTypes
    {
        LinkWalking,
        LinkSprinting,
        LinkJumping,
        LinkCrouching,
        LinkAir,
    }
}
