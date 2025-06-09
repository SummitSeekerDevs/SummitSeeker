using UnityEngine;
using Zenject;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField]
    private float _sensX = 30f;

    [SerializeField]
    private float _sensY = 30f;

    [SerializeField]
    private Transform _orientation;

    private MouseInputProvider _mouseInputProvider;

    private float xRotation;
    private float yRotation;

    [Inject]
    public void Construct(MouseInputProvider mouseInputProvider)
    {
        _mouseInputProvider = mouseInputProvider;
    }

    private void Start()
    {
        SetCursorConfiguration();
    }

    private void Update()
    {
        MouseRotationLogic();
    }

    private void SetCursorConfiguration()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void MouseRotationLogic()
    {
        Vector2 delta = _mouseInputProvider._mouseDelta;

        // get mouse input
        float mouseX = delta.x * Time.deltaTime * _sensX;
        float mouseY = delta.y * Time.deltaTime * _sensY;

        // Rotationen aktualisieren
        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // rotate cam and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        _orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
