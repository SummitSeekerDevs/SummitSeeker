using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerStateMachineTest
{
    private PlayerStateMachine playerStateMachine;

    [SetUp]
    public void Setup()
    {
        playerStateMachine = new PlayerStateMachine();
        playerStateMachine._currentState = PlayerStateMachine.MovementState.Walking;
    }

    [Test]
    public void MovementStateIsCrouchingTest()
    {
        playerStateMachine.UpdateMovementState(true, true, true);
        Assert.AreEqual(
            PlayerStateMachine.MovementState.Crouching,
            playerStateMachine._currentState,
            "MovementState is crouching"
        );
    }

    [Test]
    public void MovementStateIsSprintingTest()
    {
        playerStateMachine.UpdateMovementState(true, false, true);
        Assert.AreEqual(
            PlayerStateMachine.MovementState.Sprinting,
            playerStateMachine._currentState,
            "MovementState is sprinting"
        );
    }

    [Test]
    public void MovementStateIsWalkingTest()
    {
        playerStateMachine.UpdateMovementState(true, false, false);
        Assert.AreEqual(
            PlayerStateMachine.MovementState.Walking,
            playerStateMachine._currentState,
            "MovementState is walking"
        );
    }

    [Test]
    public void MovementStateIsAirTest()
    {
        playerStateMachine.UpdateMovementState(false, true, true);
        Assert.AreEqual(
            PlayerStateMachine.MovementState.Air,
            playerStateMachine._currentState,
            "MovementState is air"
        );
    }
}
