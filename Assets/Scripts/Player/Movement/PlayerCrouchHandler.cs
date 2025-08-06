using System;
using UnityEngine;
using Zenject;

public class PlayerCrouchHandler
{
    private readonly Rigidbody _rb;
    private readonly PlayerMovementConfig _playerMovementConfig;
    private readonly SignalBus _signalBus;

    private float _startYScale;

    public PlayerCrouchHandler(
        Rigidbody rb,
        PlayerMovementConfig playerMovementConfig,
        SignalBus signalBus
    )
    {
        _rb = rb;
        _playerMovementConfig = playerMovementConfig;
        _signalBus = signalBus;

        // Set default scale
        _startYScale = _rb.transform.localScale.y;
    }

    public void OnGameObjectEnabled()
    {
        // subscribe crouch signal
        _signalBus.Subscribe<CrouchSignal>(OnCrouch);
    }

    public void OnGameObjectDisabled()
    {
        _signalBus.Unsubscribe<CrouchSignal>(OnCrouch);
    }

    private void OnCrouch(CrouchSignal crouchSignal)
    {
        if (crouchSignal._crouchKeyPressed)
            Crouch();
        else
            ResetCrouch();
    }

    private void Crouch()
    {
        _rb.transform.localScale = new Vector3(
            _rb.transform.localScale.x,
            _playerMovementConfig.crouchYScale,
            _rb.transform.localScale.z
        );

        _rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
    }

    private void ResetCrouch()
    {
        _rb.transform.localScale = new Vector3(
            _rb.transform.localScale.x,
            _startYScale,
            _rb.transform.localScale.z
        );
    }
}
