using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class Screenshot : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputAction _attackAction;

    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _attackAction = _playerInput.actions["Attack"];
    }
    private void Update()
    {
        if (_attackAction.WasPressedThisFrame())
        {
            ScreenCapture.CaptureScreenshot("screenshot-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".png", 4);
        }
    }
}