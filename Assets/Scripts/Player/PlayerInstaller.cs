using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField]
    private PlayerMovementConfig playerMovementConfig;

    [SerializeField]
    private PlayerMovementController playerMovementController;

    public override void InstallBindings()
    {
        // Hier geht es nicht anders als die Komponente/Klasse für den Objectcontext zu binden, aufgrund der Codeaufteilung
        Container.Bind<Rigidbody>().FromComponentOnRoot().AsSingle();
        Container.Bind<PlayerMovementConfig>().FromInstance(playerMovementConfig).AsSingle();
    }
}
