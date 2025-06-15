using System;
using Zenject;

public class KatapultManager : IInitializable, IDisposable
{
    private readonly SignalBus _signalBus;
    private readonly KatapultShooter _katapultShooter;
    private readonly KatapultStunHandler _katapultStunHandler;
    private readonly DelayInvoker _delayInvoker;

    public KatapultManager(
        SignalBus signalBus,
        KatapultShooter katapultShooter,
        KatapultStunHandler katapultStunHandler,
        DelayInvoker delayInvoker
    )
    {
        _signalBus = signalBus;
        _katapultShooter = katapultShooter;
        _katapultStunHandler = katapultStunHandler;
        _delayInvoker = delayInvoker;
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

        // Monobehaviour hilfklasse zur Unterstützung
        _delayInvoker.InvokeDelayed(
            signal._shootDelay,
            () =>
            {
                _katapultStunHandler.ToggleFreezePlayerPosition(false, signal._rigidbody);
                _katapultShooter.Shoot(signal._rigidbody, signal._shootUpForce);
            }
        );
    }
}
