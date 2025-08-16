using System.Collections;
using Moq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class StateAirTest
{
    private GameObject fakePlayer;
    private Rigidbody fakePlayerRB;
    private StateAir stateAir;
    private Mock<MovementStateMachine> movementStateMachineMock;

    [SetUp]
    public void Setup()
    {
        fakePlayer = GameObject.CreatePrimitive(PrimitiveType.Cube);
        fakePlayerRB = fakePlayer.AddComponent<Rigidbody>();
        fakePlayerRB.useGravity = false;

        movementStateMachineMock = new Mock<MovementStateMachine>();

        stateAir = new StateAir(movementStateMachineMock.Object);
    }

    [Test]
    public void ResetToSpawnPositionIfUnderMapTest()
    {
        movementStateMachineMock
            .Setup(m => m._playerMovementController.PLAYER_RB)
            .Returns(fakePlayerRB);
        Vector3 spawnPosition = new Vector3(10, 10, 10);

        fakePlayerRB.position = new Vector3(0, -20, 0);

        stateAir.ResetUnderMap(spawnPosition);

        Assert.AreEqual(spawnPosition, fakePlayerRB.position);
    }
}
