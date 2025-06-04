using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();

        Container.Bind<IGameStateHandler>().To<MainMenuStateHandler>().AsSingle();
        Container.Bind<IGameStateHandler>().To<InGameStateHandler>().AsSingle();
    }
}
