using System;
using System.Collections;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

public class ZGameManagerTest : ZenjectIntegrationTestFixture
{
    private GameManager gameManager;
    private Mock<IGameStateHandler> inGameHandlerMock;
    private Mock<IGameStateHandler> mainMenuHandlerMock;

    void CommonInstall()
    {
        // Mock and bind playerInput
        Mock<PlayerInput_Actions> inputAction = new Mock<PlayerInput_Actions>();
        Container.Bind<PlayerInput_Actions>().FromInstance(inputAction.Object).AsSingle().NonLazy();

        // Mock 2 stateHandler Interfaces
        inGameHandlerMock = new Mock<IGameStateHandler>();
        inGameHandlerMock.SetupGet(h => h.State).Returns(GameState.InGame);
        inGameHandlerMock.Setup(m => m.OnEnter());
        inGameHandlerMock.Setup(m => m.OnExit());

        mainMenuHandlerMock = new Mock<IGameStateHandler>();
        mainMenuHandlerMock.SetupGet(h => h.State).Returns(GameState.MainMenu);
        mainMenuHandlerMock.Setup(m => m.OnEnter());
        mainMenuHandlerMock.Setup(m => m.OnExit());

        // Bind both interfaces
        Container
            .Bind<List<IGameStateHandler>>()
            .FromInstance(
                new List<IGameStateHandler> { inGameHandlerMock.Object, mainMenuHandlerMock.Object }
            )
            .AsSingle();

        // Bind GameManager
        Container.BindInterfacesAndSelfTo<GameManager>().AsSingle().NonLazy();

        gameManager = Container.Resolve<GameManager>();
    }

    [UnityTest]
    public IEnumerator UpdateGameStateOKTest()
    {
        PreInstall();

        CommonInstall();

        PostInstall();

        GameState? receivedState = null;
        gameManager.OnGameStateChanged += state => receivedState = state;

        gameManager.UpdateGameState(GameState.MainMenu);

        yield return new WaitForFixedUpdate();

        // GameManager sets initial GameState to inGame on Initialize
        inGameHandlerMock.Verify(m => m.OnExit(), Times.Once);

        Assert.AreEqual(
            mainMenuHandlerMock.Object,
            gameManager._currentHandler,
            "GameManagers current handler is mainMenuHandler"
        );

        mainMenuHandlerMock.Verify(m => m.OnEnter(), Times.Once());

        Assert.AreEqual(
            GameState.MainMenu,
            receivedState,
            "Received State from Action is mainMenu"
        );
    }

    [UnityTest]
    public IEnumerator UpdateGameStateNoHandlerExceptionTest()
    {
        PreInstall();

        CommonInstall();

        PostInstall();

        yield return new WaitForFixedUpdate();

        GameState changeTo = GameState.QuitGame;

        var ex = Assert.Throws<Exception>(() => gameManager.UpdateGameState(changeTo));

        Assert.AreEqual($"No handler found for GameState: {changeTo}", ex.Message);
    }
}
