using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = System.Object;

namespace Gameplay.Player
{
    public class CameraPlayer : MonoBehaviour
    {
        public static CameraPlayer Instance;

        [Header("Settings")]
        public Vector2 clampInDegrees = new(360, 180);
        
        [Space] [SerializeField] private Vector2 sensitivity = new(2, 2);
        [Space] public Vector2 smoothing = new(3, 3);
        private Vector2 _mouseAbsolute;
        private Vector2 _smoothMouse;

        private Vector2 _rawMouseInput;

        [HideInInspector]
        public bool scoped;

        private PlayerInput _playerInput;
        private InputAction _lookAction;
        private InputAction _escapeAction;
        private InputAction _primaryAction; 
        private bool _cursorLocked = true;
        //[SerializeField] private Transform _spine;
        [SerializeField] private Transform playerBody;   
        private float syncedSpineXRot;

        [SerializeField] private Transform cameraPivot;
        void Awake()
        {
           
            Instance = this;
            _playerInput = GetComponent<PlayerInput>();
            _lookAction = _playerInput.actions["Look"];
            _escapeAction = _playerInput.actions["Start"];
            _primaryAction = _playerInput.actions["Attack"]; 
        }

        void Start()
        {
            if (_cursorLocked)
                LockCursor();
        }

        public void LockCursor()
        {
            _cursorLocked = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void UnlockCursor()
        {
            _cursorLocked = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        void Update()
        {
            if (_escapeAction.triggered)
            {
                UnlockCursor();
            }

            if (!_cursorLocked && _primaryAction.triggered)
            {
                LockCursor();
            }

            if (_cursorLocked)
            {
                _rawMouseInput = _lookAction.ReadValue<Vector2>();

                Vector2 mouseDelta = Vector2.Scale(_rawMouseInput, new Vector2(sensitivity.x * smoothing.x, sensitivity.y * smoothing.y));

                _mouseAbsolute += mouseDelta;

                if (clampInDegrees.y < 360)
                    _mouseAbsolute.y = Mathf.Clamp(_mouseAbsolute.y, -clampInDegrees.y * 0.5f, clampInDegrees.y * 0.5f);

                if (clampInDegrees.x < 360)
                    _mouseAbsolute.x = Mathf.Clamp(_mouseAbsolute.x, -clampInDegrees.x * 0.5f, clampInDegrees.x * 0.5f);

                // Rotação vertical da câmera (pitch)
                cameraPivot.localRotation = Quaternion.Euler(-_mouseAbsolute.y, 0f, 0f);
                // Rotação horizontal do playerBody (yaw)
                playerBody.localRotation = Quaternion.Euler(0f, _mouseAbsolute.x, 0f);

                //_spine.localRotation = Quaternion.Euler(-_mouseAbsolute.y, 0f, 0f); 
            }
        }
        
        // private void LateUpdate()
        // {
        //     _spine.localRotation = Quaternion.Euler(-_mouseAbsolute.y, 0f, 0f);
        //     syncedSpineXRot = -_mouseAbsolute.y;
        // }
    }
}