using Moq;
using NUnit.Framework;
using Zenject;

[TestFixture]
public class ZTransitionLinksTest : ZenjectUnitTestFixture
{
    private Mock<PlayerMovementController> movementControllerMock;
    private Mock<PlayerInputProvider> inputProviderMock;

    public void CommonInstall()
    {
        var _playerInputActions = new PlayerInput_Actions();
        Container
            .Bind<PlayerInput_Actions>()
            .FromInstance(_playerInputActions)
            .AsSingle()
            .NonLazy();
        SignalBusInstaller.Install(Container);

        PlayerInput_Actions playerInputAction = Container.Resolve<PlayerInput_Actions>();
        SignalBus signalBus = Container.Resolve<SignalBus>();

        movementControllerMock = new Mock<PlayerMovementController>();
        inputProviderMock = new Mock<PlayerInputProvider>(playerInputAction, signalBus);
    }

    [Test]
    public void AirTransitionLinkTest()
    {
        CommonInstall();

        StateAir airState = new StateAir(new MovementStateMachine());
        LinkAir airLink = new LinkAir(airState);

        movementControllerMock.Setup(m => m._onGround).Returns(false);
        movementControllerMock.Setup(m => m._onSlope).Returns(false);

        bool result = airLink.ConditionMatching(movementControllerMock.Object);

        Assert.True(result, "Air condition matching");

        Assert.AreEqual(
            airState.GetHashCode(),
            airLink.GetLinkTo().GetHashCode(),
            "Returned states are equal"
        );
    }

    [Test]
    public void CrouchingTransitionLinkTest()
    {
        CommonInstall();

        StateCrouching crouchingState = new StateCrouching(new MovementStateMachine());
        LinkCrouching crouchingLink = new LinkCrouching(crouchingState);
        crouchingLink._inputProvider = inputProviderMock.Object;

        movementControllerMock.Setup(m => m._onGround).Returns(true);
        movementControllerMock.Setup(m => m._onSlope).Returns(false);
        inputProviderMock.Setup(m => m._crouchingIsPressed).Returns(true);

        bool result = crouchingLink.ConditionMatching(movementControllerMock.Object);

        Assert.True(result, "Crouching condition matching");

        Assert.AreEqual(
            crouchingState.GetHashCode(),
            crouchingLink.GetLinkTo().GetHashCode(),
            "Returned states are equal"
        );
    }

    [Test]
    public void JumpingTransitionLinkTest()
    {
        CommonInstall();

        StateJumping jumpingState = new StateJumping(new MovementStateMachine());
        LinkJumping jumpingLink = new LinkJumping(jumpingState);
        jumpingLink._inputProvider = inputProviderMock.Object;

        movementControllerMock.Setup(m => m._onGround).Returns(true);
        movementControllerMock.Setup(m => m._onSlope).Returns(true);
        movementControllerMock.Setup(m => m._readyToJump).Returns(true);
        inputProviderMock.Setup(m => m._jumpingIsPressed).Returns(true);

        bool result = jumpingLink.ConditionMatching(movementControllerMock.Object);

        Assert.True(result, "Jumping condition matching");

        Assert.AreEqual(
            jumpingState.GetHashCode(),
            jumpingLink.GetLinkTo().GetHashCode(),
            "Returned states are equal"
        );
    }

    [Test]
    public void NotJumpingInAirTransitionLinkTest()
    {
        CommonInstall();

        StateJumping jumpingState = new StateJumping(new MovementStateMachine());
        LinkJumping jumpingLink = new LinkJumping(jumpingState);
        jumpingLink._inputProvider = inputProviderMock.Object;

        movementControllerMock.Setup(m => m._onGround).Returns(false);
        movementControllerMock.Setup(m => m._onSlope).Returns(false);
        movementControllerMock.Setup(m => m._readyToJump).Returns(true);
        inputProviderMock.Setup(m => m._jumpingIsPressed).Returns(true);

        bool result = jumpingLink.ConditionMatching(movementControllerMock.Object);

        Assert.False(result, "Jumping condition not matching");
    }

    [Test]
    public void NotJumpingWhenNotReadyTransitionLinkTest()
    {
        CommonInstall();

        StateJumping jumpingState = new StateJumping(new MovementStateMachine());
        LinkJumping jumpingLink = new LinkJumping(jumpingState);
        jumpingLink._inputProvider = inputProviderMock.Object;

        movementControllerMock.Setup(m => m._onGround).Returns(true);
        movementControllerMock.Setup(m => m._onSlope).Returns(true);
        movementControllerMock.Setup(m => m._readyToJump).Returns(false);
        inputProviderMock.Setup(m => m._jumpingIsPressed).Returns(true);

        bool result = jumpingLink.ConditionMatching(movementControllerMock.Object);

        Assert.False(result, "Jumping condition not matching");
    }

    [Test]
    public void SprintingTransitionLinkTest()
    {
        CommonInstall();

        StateSprinting sprintingState = new StateSprinting(new MovementStateMachine());
        LinkSprinting sprintingLink = new LinkSprinting(sprintingState);
        sprintingLink._inputProvider = inputProviderMock.Object;

        movementControllerMock.Setup(m => m._onGround).Returns(false);
        movementControllerMock.Setup(m => m._onSlope).Returns(true);
        inputProviderMock.Setup(m => m._sprintingIsPressed).Returns(true);

        bool result = sprintingLink.ConditionMatching(movementControllerMock.Object);

        Assert.True(result, "Sprinting condition matching");

        Assert.AreEqual(
            sprintingState.GetHashCode(),
            sprintingLink.GetLinkTo().GetHashCode(),
            "Returned states are equal"
        );
    }

    [Test]
    public void WalkingTransitionLinkTest()
    {
        CommonInstall();

        StateWalking walkingState = new StateWalking(new MovementStateMachine());
        LinkWalking walkingLink = new LinkWalking(walkingState);
        walkingLink._inputProvider = inputProviderMock.Object;

        movementControllerMock.Setup(m => m._onGround).Returns(true);
        movementControllerMock.Setup(m => m._onSlope).Returns(true);
        inputProviderMock.Setup(m => m._sprintingIsPressed).Returns(false);
        inputProviderMock.Setup(m => m._jumpingIsPressed).Returns(false);
        inputProviderMock.Setup(m => m._crouchingIsPressed).Returns(false);

        bool result = walkingLink.ConditionMatching(movementControllerMock.Object);

        Assert.True(result, "Walking condition matching");

        Assert.AreEqual(
            walkingState.GetHashCode(),
            walkingLink.GetLinkTo().GetHashCode(),
            "Returned states are equal"
        );
    }
}
