using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSavePoint : MonoBehaviour
{
    public Transform activeSavePoint { get; private set;}
    public KeyCode resetToActiveSavePointKey = KeyCode.R;

    private PlayerInput_Actions _playerInputActions;

    private void Awake() {
        _playerInputActions = new PlayerInput_Actions();
    }

    private void OnEnable() {
        _playerInputActions.Player.Enable();
        _playerInputActions.Player.ResetToSavePoint.performed += OnResetToSavePoint;
    }

    private void OnDisable() {
        _playerInputActions.Player.ResetToSavePoint.performed -= OnResetToSavePoint;
        _playerInputActions.Player.Disable();
    }

    public void setActiveSavePoint(Transform savePoint) {
        activeSavePoint = savePoint;
    }

    private void OnResetToSavePoint(InputAction.CallbackContext context) {
        if (activeSavePoint != null) {
            GetComponent<Rigidbody>().position = activeSavePoint.position;
            activeSavePoint = null;
        }
    }
}
