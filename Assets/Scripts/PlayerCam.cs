using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    private PlayerInput_Actions _playerInputActions;

    public float sensX, sensY;

    public Transform orientation;

    float xRotation, yRotation;

    private Vector2 mouseDelta;

    private void Awake() {
        setPlayerInputActions();
    }

    private void setPlayerInputActions() {
        if (GameManager.Instance == null) {
            Debug.LogError("GameManager.Instance is null. Ensure GameManager exists in the scene.");
            return;
        }
        _playerInputActions = GameManager.Instance.InputActions;
        if (_playerInputActions == null) {
            Debug.LogError("PlayerInput_Actions not initialized in GameManager");
        }
    }

    private void OnEnable() {
        _playerInputActions.Player.Point.performed += OnLook;
        _playerInputActions.Player.Point.canceled += OnLookCanceled;
    }

    private void OnDisable() {
        _playerInputActions.Player.Point.performed -= OnLook;
        _playerInputActions.Player.Point.canceled -= OnLookCanceled;
    }

    public void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnLook(InputAction.CallbackContext context) {
        mouseDelta = context.ReadValue<Vector2>();
    }

    private void OnLookCanceled(InputAction.CallbackContext context) {
        mouseDelta = Vector2.zero;
    }

    public void Update() {

        // get mouse input
        float mouseX = mouseDelta.x * Time.deltaTime * sensX;
        float mouseY = mouseDelta.y * Time.deltaTime * sensY;

        // Rotationen aktualisieren
        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // rotate cam and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
