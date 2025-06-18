using Zenject;

public class CallablePlatformInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.DeclareSignal<CallablePlatformStartMovementSignal>();
    }
}
