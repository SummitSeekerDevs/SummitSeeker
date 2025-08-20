using System.Collections;
using Moq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class StateSprintingTest
{
    private GameObject fakePlayer;
    private Rigidbody fakePlayerRB;
    private StateSprinting stateSprinting;
    private PlayerMovementController playerMovementController;

    [SetUp]
    public void Setup()
    {
        fakePlayer = GameObject.CreatePrimitive(PrimitiveType.Cube);
        fakePlayerRB = fakePlayer.AddComponent<Rigidbody>();
        fakePlayer.transform.localScale = Vector3.one;
        fakePlayerRB.useGravity = false;
        playerMovementController = fakePlayer.AddComponent<PlayerMovementController>();
    }

    [UnityTest]
    public IEnumerator PlayerCanSprintOnGroundTest()
    {
        Vector3 startPosition = fakePlayerRB.position;

        Mock<MovementStateMachine> movementStateMachineMock = new Mock<MovementStateMachine>();
        movementStateMachineMock
            .Setup(m => m._playerMovementController)
            .Returns(playerMovementController);
        playerMovementController._onGround = true;
        playerMovementController._onSlope = false;
        playerMovementController._moveSpeed = 10;
        playerMovementController._exitingSlope = false;

        Vector3 moveDirection = new Vector3(1, 0, 0);

        stateSprinting.FixedUpdate(moveDirection);

        yield return new WaitForFixedUpdate();

        Assert.Greater(Vector3.Distance(startPosition, fakePlayerRB.position), 0f);
    }
}
