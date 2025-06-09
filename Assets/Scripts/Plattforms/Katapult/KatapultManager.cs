using System;
using System.Threading;
using Zenject;

public class KatapultManager : IInitializable, IDisposable
{
    private readonly SignalBus _signalBus;
    private readonly KatapultShooter _katapultShooter;
    private readonly KatapultStunHandler _katapultStunHandler;

    public KatapultManager(
        SignalBus signalBus,
        KatapultShooter katapultShooter,
        KatapultStunHandler katapultStunHandler
    )
    {
        _signalBus = signalBus;
        _katapultShooter = katapultShooter;
        _katapultStunHandler = katapultStunHandler;
    }

    public void Initialize()
    {
        _signalBus.Subscribe<KatapultTriggerSignal>(OnKatapultTriggered);
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<KatapultTriggerSignal>(OnKatapultTriggered);
    }

    private void OnKatapultTriggered(KatapultTriggerSignal signal)
    {
        _katapultStunHandler.ToggleFreezePlayerPosition(true, signal._rigidbody);

        // Monobehaviour klasse erstellen die dem gamemanager angehängt wird und durch dessen funktionsaufruf man invoke benutzen kann
    }
}
