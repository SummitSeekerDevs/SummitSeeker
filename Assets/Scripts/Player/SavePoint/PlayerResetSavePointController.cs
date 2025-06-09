using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerResetSavePointController : MonoBehaviour
{
    private SavePointState _savePointState;
    private PlayerInput_Actions _playerInputActions;

    [Inject]
    public void Construct(SavePointState savePointState, PlayerInput_Actions inputActions)
    {
        _savePointState = savePointState;
        _playerInputActions = inputActions;
    }

    private void OnEnable()
    {
        _playerInputActions.Player.ResetToSavePoint.performed += OnResetToSavePoint;
    }

    private void OnDisable()
    {
        _playerInputActions.Player.ResetToSavePoint.performed -= OnResetToSavePoint;
    }

    private void OnResetToSavePoint(InputAction.CallbackContext context)
    {
        Transform savePoint = _savePointState.ConsumeSavePoint();
        if (savePoint != null)
        {
            GetComponent<Rigidbody>().position = savePoint.position;
        }
    }
}
