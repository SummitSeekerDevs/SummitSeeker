using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller<GameInstaller>
{
    [SerializeField]
    private SceneLoader sceneLoaderPrefab;

    public override void InstallBindings()
    {
        Container
            .Bind<SceneLoader>()
            .FromComponentInNewPrefab(sceneLoaderPrefab)
            .AsSingle()
            .NonLazy();

        Container.BindInterfacesAndSelfTo<GameManager>().AsSingle().NonLazy();

        Container.Bind<IGameStateHandler>().To<MainMenuStateHandler>().AsSingle();
        Container.Bind<IGameStateHandler>().To<InGameStateHandler>().AsSingle();
    }
}
