using UnityEngine;
using Zenject;

public class PlayerResetSavePointController : MonoBehaviour
{
    private SavePointState _savePointState;
    private SignalBus _signalBus;

    [Inject]
    public void Construct(SavePointState savePointState, SignalBus signalBus)
    {
        _savePointState = savePointState;
        _signalBus = signalBus;

        // subscribe crouch signal
        _signalBus.Subscribe<ResetToSavePointSignal>(OnResetToSavePoint);
    }

    private void OnDisable()
    {
        _signalBus.Unsubscribe<ResetToSavePointSignal>(OnResetToSavePoint);
    }

    private void OnResetToSavePoint()
    {
        Transform savePoint = _savePointState.ConsumeSavePoint();
        if (savePoint != null)
        {
            GetComponent<Rigidbody>().position = savePoint.position;
        }
    }
}

public class ResetToSavePointSignal { }
