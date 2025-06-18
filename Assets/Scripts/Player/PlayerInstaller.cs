using Zenject;

public class PlayerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<PlayerSlopeHandler>().AsSingle();
        Container.Bind<PlayerStateMachine>().AsSingle();
    }
}
