//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Game/Input/PlayerInputMap.inputactions
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

public partial class @PlayerInputMap: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputMap()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputMap"",
    ""maps"": [
        {
            ""name"": ""Adventure"",
            ""id"": ""3ca0aa08-5015-448d-bca9-6343a88e2d91"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""39ac32cd-72da-4302-aacc-bcb9e6eab6aa"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Run"",
                    ""type"": ""Button"",
                    ""id"": ""d6d1a563-1680-45b6-b963-fd1dbbd80adf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Deside"",
                    ""type"": ""Button"",
                    ""id"": ""878a9bb2-3e13-4e3b-8433-284cb381ed13"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""3b75ae6e-1b8e-40ea-b490-256f9599e9cc"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""aea00706-988a-4b94-9a38-8e6fca5af005"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""86276f0b-2256-41ac-b8ba-6177c713703f"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""0fe11e70-22ca-4069-96a0-0e2fae9069c3"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""81a73a57-f443-4b57-9a8e-6e356fbc0c24"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""f436ede2-eed3-4b56-a2e1-0edac9c49054"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""93500d77-c9fe-46f1-b228-e62306986f92"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1e3df4c9-4dcd-4f03-846e-5089f1534c20"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""44034be8-5636-40fc-a336-4b7d22a1eec1"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Deside"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d43610d9-1615-4d86-8ca0-9bff85152b03"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Deside"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Adventure
        m_Adventure = asset.FindActionMap("Adventure", throwIfNotFound: true);
        m_Adventure_Move = m_Adventure.FindAction("Move", throwIfNotFound: true);
        m_Adventure_Run = m_Adventure.FindAction("Run", throwIfNotFound: true);
        m_Adventure_Deside = m_Adventure.FindAction("Deside", throwIfNotFound: true);
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

    // Adventure
    private readonly InputActionMap m_Adventure;
    private List<IAdventureActions> m_AdventureActionsCallbackInterfaces = new List<IAdventureActions>();
    private readonly InputAction m_Adventure_Move;
    private readonly InputAction m_Adventure_Run;
    private readonly InputAction m_Adventure_Deside;
    public struct AdventureActions
    {
        private @PlayerInputMap m_Wrapper;
        public AdventureActions(@PlayerInputMap wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Adventure_Move;
        public InputAction @Run => m_Wrapper.m_Adventure_Run;
        public InputAction @Deside => m_Wrapper.m_Adventure_Deside;
        public InputActionMap Get() { return m_Wrapper.m_Adventure; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(AdventureActions set) { return set.Get(); }
        public void AddCallbacks(IAdventureActions instance)
        {
            if (instance == null || m_Wrapper.m_AdventureActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_AdventureActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @Run.started += instance.OnRun;
            @Run.performed += instance.OnRun;
            @Run.canceled += instance.OnRun;
            @Deside.started += instance.OnDeside;
            @Deside.performed += instance.OnDeside;
            @Deside.canceled += instance.OnDeside;
        }

        private void UnregisterCallbacks(IAdventureActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @Run.started -= instance.OnRun;
            @Run.performed -= instance.OnRun;
            @Run.canceled -= instance.OnRun;
            @Deside.started -= instance.OnDeside;
            @Deside.performed -= instance.OnDeside;
            @Deside.canceled -= instance.OnDeside;
        }

        public void RemoveCallbacks(IAdventureActions instance)
        {
            if (m_Wrapper.m_AdventureActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IAdventureActions instance)
        {
            foreach (var item in m_Wrapper.m_AdventureActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_AdventureActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public AdventureActions @Adventure => new AdventureActions(this);
    public interface IAdventureActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnRun(InputAction.CallbackContext context);
        void OnDeside(InputAction.CallbackContext context);
    }
}