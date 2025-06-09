using UnityEngine;
using Zenject;

public class InGameSceneInstaller : MonoInstaller
{
    [SerializeField]
    private GameObject _playerGameObject;

    public override void InstallBindings()
    {
        Container.Bind<GameObject>().FromInstance(_playerGameObject).AsSingle().NonLazy();
        Container.Bind<SavePointState>().AsSingle();
    }
}
