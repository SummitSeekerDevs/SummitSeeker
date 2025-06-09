using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class MouseInputProvider : IInitializable, IDisposable
{
    private readonly PlayerInput_Actions _playerInputActions;
    public Vector2 _mouseDelta { get; private set; }

    public MouseInputProvider(PlayerInput_Actions playerInputActions)
    {
        _playerInputActions = playerInputActions;
    }

    public void Initialize()
    {
        _playerInputActions.Player.Point.performed += OnLook;
        _playerInputActions.Player.Point.canceled += OnLookCanceled;
    }

    public void Dispose()
    {
        _playerInputActions.Player.Point.performed -= OnLook;
        _playerInputActions.Player.Point.canceled -= OnLookCanceled;
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        _mouseDelta = context.ReadValue<Vector2>();
    }

    private void OnLookCanceled(InputAction.CallbackContext context)
    {
        _mouseDelta = Vector2.zero;
    }
}
