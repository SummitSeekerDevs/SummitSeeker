using UnityEngine;
using Zenject;

public class CallablePlatformInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);

        Container.DeclareSignal<CallablePlatformStartMovementSignal>();
    }
}
