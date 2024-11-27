//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.11.2
//     from Assets/InputSystem/PlayerInput_Actions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInput_Actions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInput_Actions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInput_Actions"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""184ec929-7e22-494c-b3ca-7c83a8748080"",
            ""actions"": [
                {
                    ""name"": ""ResetToSavePoint"",
                    ""type"": ""Button"",
                    ""id"": ""55034195-5083-4124-bb8c-9da5ea7b1c6c"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""e6683626-4911-46b4-8380-5cb643f716be"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ResetToSavePoint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_ResetToSavePoint = m_Player.FindAction("ResetToSavePoint", throwIfNotFound: true);
    }

    ~@PlayerInput_Actions()
    {
        UnityEngine.Debug.Assert(!m_Player.enabled, "This will cause a leak and performance issues, PlayerInput_Actions.Player.Disable() has not been called.");
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player
    private readonly InputActionMap m_Player;
    private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
    private readonly InputAction m_Player_ResetToSavePoint;
    public struct PlayerActions
    {
        private @PlayerInput_Actions m_Wrapper;
        public PlayerActions(@PlayerInput_Actions wrapper) { m_Wrapper = wrapper; }
        public InputAction @ResetToSavePoint => m_Wrapper.m_Player_ResetToSavePoint;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @ResetToSavePoint.started += instance.OnResetToSavePoint;
            @ResetToSavePoint.performed += instance.OnResetToSavePoint;
            @ResetToSavePoint.canceled += instance.OnResetToSavePoint;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @ResetToSavePoint.started -= instance.OnResetToSavePoint;
            @ResetToSavePoint.performed -= instance.OnResetToSavePoint;
            @ResetToSavePoint.canceled -= instance.OnResetToSavePoint;
        }

        public void RemoveCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnResetToSavePoint(InputAction.CallbackContext context);
    }
}
