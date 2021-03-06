// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Input.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Input : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Input()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Input"",
    ""maps"": [
        {
            ""name"": ""Play"",
            ""id"": ""208ad470-1c32-41f3-89f0-515e633a25fd"",
            ""actions"": [
                {
                    ""name"": ""Throw"",
                    ""type"": ""Button"",
                    ""id"": ""206da985-4acd-4f8d-9105-917e2c6293b4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""bc3713d0-2465-4f25-9fda-87795f5618b3"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""1db914f5-717b-4ea0-8df5-284866595961"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""fcf726df-dacb-418f-a917-18cd255dd010"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Work"",
                    ""type"": ""Button"",
                    ""id"": ""d7576793-655c-4d8a-b436-b9023a610b7f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TEST"",
                    ""type"": ""Button"",
                    ""id"": ""bf04176a-c3ae-4bb6-b627-bb4c5c8e54c7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""dd642a3d-f55d-44e8-9dc3-33402a92ff73"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Throw"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d26fd07e-56ce-4ee8-9c62-fecc5beceab4"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Throw"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""8d8693b1-2e1e-4f05-ac21-1802564cf9c5"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""721c256d-8c82-4a06-8b52-6cdb14bd045e"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""34f9d256-008b-4dc2-a15e-657aeaa30e61"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""28a925dd-c1be-4717-9885-5e496d28f611"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""a8cd8d31-b399-4de3-bb76-22b7440ddf38"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""9f425290-2242-4a2a-9e87-8cb07a770bd7"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""d5103dc0-3c3b-4fe8-b063-539713fe9bf7"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4ac4d560-c21a-4bed-945e-aa3c469a1c1b"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c951e77c-89fd-438f-9e41-bc0844b01a4d"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""21953ba7-98ce-488e-a7df-6c9f5d4eb650"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7ba26457-e58d-449f-96f6-5f3721a680f4"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Work"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""377a2509-26c9-4a38-832d-eb996ea60fe2"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Work"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3c969467-3fff-43ec-8f9a-a1d376444d8d"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TEST"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Play
        m_Play = asset.FindActionMap("Play", throwIfNotFound: true);
        m_Play_Throw = m_Play.FindAction("Throw", throwIfNotFound: true);
        m_Play_Movement = m_Play.FindAction("Movement", throwIfNotFound: true);
        m_Play_Interact = m_Play.FindAction("Interact", throwIfNotFound: true);
        m_Play_Jump = m_Play.FindAction("Jump", throwIfNotFound: true);
        m_Play_Work = m_Play.FindAction("Work", throwIfNotFound: true);
        m_Play_TEST = m_Play.FindAction("TEST", throwIfNotFound: true);
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

    // Play
    private readonly InputActionMap m_Play;
    private IPlayActions m_PlayActionsCallbackInterface;
    private readonly InputAction m_Play_Throw;
    private readonly InputAction m_Play_Movement;
    private readonly InputAction m_Play_Interact;
    private readonly InputAction m_Play_Jump;
    private readonly InputAction m_Play_Work;
    private readonly InputAction m_Play_TEST;
    public struct PlayActions
    {
        private @Input m_Wrapper;
        public PlayActions(@Input wrapper) { m_Wrapper = wrapper; }
        public InputAction @Throw => m_Wrapper.m_Play_Throw;
        public InputAction @Movement => m_Wrapper.m_Play_Movement;
        public InputAction @Interact => m_Wrapper.m_Play_Interact;
        public InputAction @Jump => m_Wrapper.m_Play_Jump;
        public InputAction @Work => m_Wrapper.m_Play_Work;
        public InputAction @TEST => m_Wrapper.m_Play_TEST;
        public InputActionMap Get() { return m_Wrapper.m_Play; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayActions set) { return set.Get(); }
        public void SetCallbacks(IPlayActions instance)
        {
            if (m_Wrapper.m_PlayActionsCallbackInterface != null)
            {
                @Throw.started -= m_Wrapper.m_PlayActionsCallbackInterface.OnThrow;
                @Throw.performed -= m_Wrapper.m_PlayActionsCallbackInterface.OnThrow;
                @Throw.canceled -= m_Wrapper.m_PlayActionsCallbackInterface.OnThrow;
                @Movement.started -= m_Wrapper.m_PlayActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayActionsCallbackInterface.OnMovement;
                @Interact.started -= m_Wrapper.m_PlayActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_PlayActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_PlayActionsCallbackInterface.OnInteract;
                @Jump.started -= m_Wrapper.m_PlayActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayActionsCallbackInterface.OnJump;
                @Work.started -= m_Wrapper.m_PlayActionsCallbackInterface.OnWork;
                @Work.performed -= m_Wrapper.m_PlayActionsCallbackInterface.OnWork;
                @Work.canceled -= m_Wrapper.m_PlayActionsCallbackInterface.OnWork;
                @TEST.started -= m_Wrapper.m_PlayActionsCallbackInterface.OnTEST;
                @TEST.performed -= m_Wrapper.m_PlayActionsCallbackInterface.OnTEST;
                @TEST.canceled -= m_Wrapper.m_PlayActionsCallbackInterface.OnTEST;
            }
            m_Wrapper.m_PlayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Throw.started += instance.OnThrow;
                @Throw.performed += instance.OnThrow;
                @Throw.canceled += instance.OnThrow;
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Work.started += instance.OnWork;
                @Work.performed += instance.OnWork;
                @Work.canceled += instance.OnWork;
                @TEST.started += instance.OnTEST;
                @TEST.performed += instance.OnTEST;
                @TEST.canceled += instance.OnTEST;
            }
        }
    }
    public PlayActions @Play => new PlayActions(this);
    public interface IPlayActions
    {
        void OnThrow(InputAction.CallbackContext context);
        void OnMovement(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnWork(InputAction.CallbackContext context);
        void OnTEST(InputAction.CallbackContext context);
    }
}
