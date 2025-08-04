using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerInputProvider : IInitializable, IDisposable
{
    private readonly PlayerInput_Actions _playerInputActions;
    private SignalBus _signalBus;
    public Vector2 _mouseDelta { get; private set; }
    public Vector2 _moveInput { get; private set; }
    public bool _jumpingIsPressed { get; private set; }
    public bool _crouchingIsPressed { get; private set; }
    public bool _sprintingIsPressed { get; private set; }

    public PlayerInputProvider(PlayerInput_Actions playerInputActions, SignalBus signalBus)
    {
        _playerInputActions = playerInputActions;
        _signalBus = signalBus;
    }

    public void Initialize()
    {
        // Looking
        _playerInputActions.Player.Point.performed += OnLook;
        _playerInputActions.Player.Point.canceled += OnLookCanceled;

        // Throwing
        _playerInputActions.Player.Click.performed += OnThrow;

        // Moving
        _playerInputActions.Player.Move.performed += OnMove;
        _playerInputActions.Player.Move.canceled += OnMove;

        // Jumping
        _playerInputActions.Player.Jump.started += OnJump;
        _playerInputActions.Player.Jump.canceled += OnJumpCanceled;

        // Crouching
        _playerInputActions.Player.Crouch.started += OnCrouch;
        _playerInputActions.Player.Crouch.canceled += OnCrouchCanceled;

        // Sprinting
        _playerInputActions.Player.Sprint.started += OnSprint;
        _playerInputActions.Player.Sprint.canceled += OnSprintCanceled;

        // Savepoint
        _playerInputActions.Player.ResetToSavePoint.started += OnResetToSavePoint;
        _playerInputActions.Player.ResetToSavePoint.canceled += OnResetToSavePointCanceled;
    }

    public void Dispose()
    {
        // Looking
        _playerInputActions.Player.Point.performed -= OnLook;
        _playerInputActions.Player.Point.canceled -= OnLookCanceled;

        // Throwing
        _playerInputActions.Player.Click.performed -= OnThrow;

        // Moving
        _playerInputActions.Player.Move.performed -= OnMove;
        _playerInputActions.Player.Move.canceled -= OnMove;

        // Jumping
        _playerInputActions.Player.Jump.started -= OnJump;
        _playerInputActions.Player.Jump.canceled -= OnJumpCanceled;

        // Crouching
        _playerInputActions.Player.Crouch.started -= OnCrouch;
        _playerInputActions.Player.Crouch.canceled -= OnCrouchCanceled;

        // Sprinting
        _playerInputActions.Player.Sprint.started -= OnSprint;
        _playerInputActions.Player.Sprint.canceled -= OnSprintCanceled;

        // Savepoint
        _playerInputActions.Player.ResetToSavePoint.started -= OnResetToSavePoint;
        _playerInputActions.Player.ResetToSavePoint.canceled -= OnResetToSavePointCanceled;
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

    private void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        _jumpingIsPressed = true;
    }

    private void OnJumpCanceled(InputAction.CallbackContext context)
    {
        _jumpingIsPressed = false;
    }

    private void OnCrouch(InputAction.CallbackContext context)
    {
        _crouchingIsPressed = true;
        _signalBus.Fire<CrouchSignal>(new CrouchSignal(true));
    }

    private void OnCrouchCanceled(InputAction.CallbackContext context)
    {
        _signalBus.Fire<CrouchSignal>(new CrouchSignal(false));
        _crouchingIsPressed = false;
    }

    private void OnSprint(InputAction.CallbackContext context)
    {
        _sprintingIsPressed = true;
    }

    private void OnSprintCanceled(InputAction.CallbackContext context)
    {
        _sprintingIsPressed = false;
    }

    private void OnResetToSavePoint(InputAction.CallbackContext context)
    {
        _signalBus.Fire<ResetToSavePointSignal>(new ResetToSavePointSignal(true));
    }

    private void OnResetToSavePointCanceled(InputAction.CallbackContext context)
    {
        _signalBus.Fire<ResetToSavePointSignal>(new ResetToSavePointSignal(false));
    }
}
