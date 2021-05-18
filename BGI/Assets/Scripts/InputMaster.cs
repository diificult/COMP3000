// GENERATED AUTOMATICALLY FROM 'Assets/InputMaster.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputMaster : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputMaster()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputMaster"",
    ""maps"": [
        {
            ""name"": ""Game Action Map"",
            ""id"": ""864961c8-1a8d-4c38-8997-f19125e2fc12"",
            ""actions"": [
                {
                    ""name"": ""Roll"",
                    ""type"": ""Value"",
                    ""id"": ""e0e7577e-6788-4071-bc54-764279bb4a8e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ea487e2d-99b5-4ffb-bed9-b9f86c1c8bd5"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""New control scheme"",
                    ""action"": ""Roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""New control scheme"",
            ""bindingGroup"": ""New control scheme"",
            ""devices"": []
        }
    ]
}");
        // Game Action Map
        m_GameActionMap = asset.FindActionMap("Game Action Map", throwIfNotFound: true);
        m_GameActionMap_Roll = m_GameActionMap.FindAction("Roll", throwIfNotFound: true);
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

    // Game Action Map
    private readonly InputActionMap m_GameActionMap;
    private IGameActionMapActions m_GameActionMapActionsCallbackInterface;
    private readonly InputAction m_GameActionMap_Roll;
    public struct GameActionMapActions
    {
        private @InputMaster m_Wrapper;
        public GameActionMapActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @Roll => m_Wrapper.m_GameActionMap_Roll;
        public InputActionMap Get() { return m_Wrapper.m_GameActionMap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameActionMapActions set) { return set.Get(); }
        public void SetCallbacks(IGameActionMapActions instance)
        {
            if (m_Wrapper.m_GameActionMapActionsCallbackInterface != null)
            {
                @Roll.started -= m_Wrapper.m_GameActionMapActionsCallbackInterface.OnRoll;
                @Roll.performed -= m_Wrapper.m_GameActionMapActionsCallbackInterface.OnRoll;
                @Roll.canceled -= m_Wrapper.m_GameActionMapActionsCallbackInterface.OnRoll;
            }
            m_Wrapper.m_GameActionMapActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Roll.started += instance.OnRoll;
                @Roll.performed += instance.OnRoll;
                @Roll.canceled += instance.OnRoll;
            }
        }
    }
    public GameActionMapActions @GameActionMap => new GameActionMapActions(this);
    private int m_NewcontrolschemeSchemeIndex = -1;
    public InputControlScheme NewcontrolschemeScheme
    {
        get
        {
            if (m_NewcontrolschemeSchemeIndex == -1) m_NewcontrolschemeSchemeIndex = asset.FindControlSchemeIndex("New control scheme");
            return asset.controlSchemes[m_NewcontrolschemeSchemeIndex];
        }
    }
    public interface IGameActionMapActions
    {
        void OnRoll(InputAction.CallbackContext context);
    }
}
