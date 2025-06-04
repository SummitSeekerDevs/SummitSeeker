using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller<GameInstaller>
{
    [SerializeField]
    private SceneLoader _sceneLoaderPrefab;

    public override void InstallBindings()
    {
        Container
            .Bind<SceneLoader>()
            .FromComponentInNewPrefab(_sceneLoaderPrefab)
            .AsSingle()
            .NonLazy();

        var _playerInputActions = new PlayerInput_Actions();
        Container
            .Bind<PlayerInput_Actions>()
            .FromInstance(_playerInputActions)
            .AsSingle()
            .NonLazy();

        Container.BindInterfacesAndSelfTo<GameManager>().AsSingle().NonLazy();

        Container.Bind<IGameStateHandler>().To<MainMenuStateHandler>().AsSingle();
        Container.Bind<IGameStateHandler>().To<InGameStateHandler>().AsSingle();
    }
}
