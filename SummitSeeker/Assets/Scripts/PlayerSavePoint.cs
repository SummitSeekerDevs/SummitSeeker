using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSavePoint : MonoBehaviour
{
    public Transform activeSavePoint { get; private set; }

    private PlayerInput_Actions _playerInputActions;

    private void Awake()
    {
        setPlayerInputActions();
    }

    private void setPlayerInputActions()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance is null. Ensure GameManager exists in the scene.");
            return;
        }
        _playerInputActions = GameManager.Instance.InputActions;
        if (_playerInputActions == null)
        {
            Debug.LogError("PlayerInput_Actions not initialized in GameManager");
        }
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
