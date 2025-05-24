using UnityEngine;
using UnityEngine.InputSystem;

public class ThrowingKnife : MonoBehaviour
{
    private PlayerInput_Actions _playerInputActions;

    [Header("References")]
    public Transform cam, attackPoint;
    public GameObject objectToThrow;

    [Header("Settings")]
    public int totalThrows;
    public float throwCooldown;

    [Header("Throwing")]
    public float throwForce, throwUpwardForce;

    bool readyToThrow;

    private void Awake() {
        SetPlayerInputActions();
    }

    private void SetPlayerInputActions() {
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
        _playerInputActions.Player.Click.performed += OnThrow;
    }

    private void OnDisable() {
        _playerInputActions.Player.Click.performed -= OnThrow;
    }

    private void Start() {
        readyToThrow = true;
    }

    private void OnThrow(InputAction.CallbackContext context) {
        if(readyToThrow && totalThrows > 0) {
            Throw();
        }
    }

    private void Throw() {
        readyToThrow = false;

        // instantiate object to throw
        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, cam.rotation);

        // get rigidbody component
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        projectileRb.angularDamping = 0f;

        // calculate direction
        Vector3 forceDirection = cam.transform.forward;

        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, 500f)) {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        // add force
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;
        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        // Drehung
        Vector3 torque = attackPoint.transform.right * 1000 * Time.deltaTime;
        projectileRb.AddTorque(torque, ForceMode.Impulse);

        totalThrows--;

        // implement throwCooldown
        Invoke(nameof(ResetThrow), throwCooldown);
    }

    private void ResetThrow() {
        readyToThrow = true;
    }
}
