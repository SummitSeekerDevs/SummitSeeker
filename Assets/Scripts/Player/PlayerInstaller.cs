using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField]
    private PlayerMovementConfig playerMovementConfig;

    public override void InstallBindings()
    {
        Container.Bind<PlayerSlopeHandler>().AsSingle();
        Container.Bind<PlayerStateMachine>().AsSingle();
        Container.Bind<PlayerJumpHandler>().AsSingle();

        // Hier geht es nicht anders als die Komponente/Klasse für den Objectcontext zu binden, aufgrund der Codeaufteilung
        Container.Bind<Rigidbody>().FromComponentOnRoot().AsSingle();
        Container.Bind<PlayerMovementConfig>().FromInstance(playerMovementConfig).AsSingle();
    }
}
