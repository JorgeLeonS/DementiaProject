//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.2.0
//     from Assets/Scripts/SpectatorCamera/InputCameraController.inputactions
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

public partial class @InputCameraController : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputCameraController()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputCameraController"",
    ""maps"": [
        {
            ""name"": ""Drone"",
            ""id"": ""e4e260df-0586-4705-a23d-6deda51982d9"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""aa165cdb-f1b6-4548-9cd5-7bdb67a173a8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""321e1a8d-18bd-4eb1-b7e8-5a64fbe0a60c"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Reset Horizontal"",
                    ""type"": ""Button"",
                    ""id"": ""1e459904-968a-452b-a8b8-51168e778c5c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Fly"",
                    ""type"": ""Value"",
                    ""id"": ""34241c87-8308-41cb-9646-a4cc7ceaf98e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Arrows"",
                    ""id"": ""1899013a-c431-43a7-8cd5-4654767bbc58"",
                    ""path"": ""2DVector"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""8d7ffa86-52db-4214-96f8-0bc67c039db0"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""b3b0fdf4-b7f2-499f-a224-7832b81ecdae"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""94a85289-559d-47b0-99ed-783a01f20373"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""de08a130-607c-4909-af2e-1490953efb50"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""9c78cb46-1ed6-4f30-923c-7c4a6f2f905a"",
                    ""path"": ""2DVector"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""7156e77b-b0cf-4a69-91b0-f988005955fb"",
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
                    ""id"": ""44b4eea2-1aa2-4b2d-a050-7a9b57ae7255"",
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
                    ""id"": ""0f440f1e-19df-4562-b8af-ac0dfb779107"",
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
                    ""id"": ""7a78acef-c945-440a-8397-d794b9e76f65"",
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
                    ""id"": ""e6d9204d-3387-445b-89f7-c3e48c7f524e"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Reset Horizontal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""IK"",
                    ""id"": ""fdc7baad-9829-49d8-b120-27959b3e9474"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fly"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""67bda888-0dce-448b-bc3f-d564247ffb69"",
                    ""path"": ""<Keyboard>/numpad8"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fly"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""fbbd6e5a-e5ea-4b0f-892f-2dce7cadaee6"",
                    ""path"": ""<Keyboard>/numpad2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fly"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Drone
        m_Drone = asset.FindActionMap("Drone", throwIfNotFound: true);
        m_Drone_Move = m_Drone.FindAction("Move", throwIfNotFound: true);
        m_Drone_Look = m_Drone.FindAction("Look", throwIfNotFound: true);
        m_Drone_ResetHorizontal = m_Drone.FindAction("Reset Horizontal", throwIfNotFound: true);
        m_Drone_Fly = m_Drone.FindAction("Fly", throwIfNotFound: true);
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

    // Drone
    private readonly InputActionMap m_Drone;
    private IDroneActions m_DroneActionsCallbackInterface;
    private readonly InputAction m_Drone_Move;
    private readonly InputAction m_Drone_Look;
    private readonly InputAction m_Drone_ResetHorizontal;
    private readonly InputAction m_Drone_Fly;
    public struct DroneActions
    {
        private @InputCameraController m_Wrapper;
        public DroneActions(@InputCameraController wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Drone_Move;
        public InputAction @Look => m_Wrapper.m_Drone_Look;
        public InputAction @ResetHorizontal => m_Wrapper.m_Drone_ResetHorizontal;
        public InputAction @Fly => m_Wrapper.m_Drone_Fly;
        public InputActionMap Get() { return m_Wrapper.m_Drone; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DroneActions set) { return set.Get(); }
        public void SetCallbacks(IDroneActions instance)
        {
            if (m_Wrapper.m_DroneActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_DroneActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_DroneActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_DroneActionsCallbackInterface.OnMove;
                @Look.started -= m_Wrapper.m_DroneActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_DroneActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_DroneActionsCallbackInterface.OnLook;
                @ResetHorizontal.started -= m_Wrapper.m_DroneActionsCallbackInterface.OnResetHorizontal;
                @ResetHorizontal.performed -= m_Wrapper.m_DroneActionsCallbackInterface.OnResetHorizontal;
                @ResetHorizontal.canceled -= m_Wrapper.m_DroneActionsCallbackInterface.OnResetHorizontal;
                @Fly.started -= m_Wrapper.m_DroneActionsCallbackInterface.OnFly;
                @Fly.performed -= m_Wrapper.m_DroneActionsCallbackInterface.OnFly;
                @Fly.canceled -= m_Wrapper.m_DroneActionsCallbackInterface.OnFly;
            }
            m_Wrapper.m_DroneActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @ResetHorizontal.started += instance.OnResetHorizontal;
                @ResetHorizontal.performed += instance.OnResetHorizontal;
                @ResetHorizontal.canceled += instance.OnResetHorizontal;
                @Fly.started += instance.OnFly;
                @Fly.performed += instance.OnFly;
                @Fly.canceled += instance.OnFly;
            }
        }
    }
    public DroneActions @Drone => new DroneActions(this);
    public interface IDroneActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnResetHorizontal(InputAction.CallbackContext context);
        void OnFly(InputAction.CallbackContext context);
    }
}
