using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerInputProvider : IInitializable, IDisposable
{
    private readonly PlayerInput_Actions _playerInputActions;
    private SignalBus _signalBus;
    public Vector2 _mouseDelta { get; private set; }

    public PlayerInputProvider(PlayerInput_Actions playerInputActions, SignalBus signalBus)
    {
        _playerInputActions = playerInputActions;
        _signalBus = signalBus;
    }

    public void Initialize()
    {
        _playerInputActions.Player.Point.performed += OnLook;
        _playerInputActions.Player.Point.canceled += OnLookCanceled;

        _playerInputActions.Player.Click.performed += OnThrow;
    }

    public void Dispose()
    {
        _playerInputActions.Player.Point.performed -= OnLook;
        _playerInputActions.Player.Point.canceled -= OnLookCanceled;

        _playerInputActions.Player.Click.performed -= OnThrow;
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        _mouseDelta = context.ReadValue<Vector2>();
    }

    private void OnLookCanceled(InputAction.CallbackContext context)
    {
        _mouseDelta = Vector2.zero;
    }

    private void OnThrow(InputAction.CallbackContext context)
    {
        _signalBus.Fire<ThrowProjectileSignal>();
    }
}
