using Zenject;

public class KatapultInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<KatapultShooter>().AsSingle();
        Container.Bind<KatapultStunHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<KatapultManager>().AsSingle();

        // Signal
        Container.DeclareSignal<KatapultTriggerSignal>();
    }
}
