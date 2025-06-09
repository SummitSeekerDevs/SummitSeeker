using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerSavePoint : MonoBehaviour
{
    public Transform activeSavePoint { get; private set; }

    private PlayerInput_Actions _playerInputActions;

    [Inject]
    public void Construct(PlayerInput_Actions playerInputAction)
    {
        _playerInputActions = playerInputAction;
    }

    private void OnEnable()
    {
        _playerInputActions.Player.ResetToSavePoint.performed += OnResetToSavePoint;
    }

    private void OnDisable()
    {
        _playerInputActions.Player.ResetToSavePoint.performed -= OnResetToSavePoint;
    }

    public void setActiveSavePoint(Transform savePoint)
    {
        activeSavePoint = savePoint;
    }

    private void OnResetToSavePoint(InputAction.CallbackContext context)
    {
        if (activeSavePoint != null)
        {
            GetComponent<Rigidbody>().position = activeSavePoint.position;
            activeSavePoint = null;
        }
    }
}
