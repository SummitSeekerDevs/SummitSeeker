using UnityEngine;
using Zenject;

public class MovingPlatformInstaller : MonoInstaller<MovingPlatformInstaller>
{
    [SerializeField]
    private Transform[] _points;

    public override void InstallBindings()
    {
        Container.Bind<PlatformPath>().AsSingle().WithArguments(_points);
    }
}
