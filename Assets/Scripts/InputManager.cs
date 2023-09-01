using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.EventSystems;
using TMPro;

[System.Serializable]
public class InputManagerData {
    public Vector2 velocityIS;
    public bool isJump;
    public bool isFire;
    public Vector3 characterForwardWS;
}

public class InputManager : MonoBehaviour {
    [SerializeField] private InputManagerData _inputData;
    public InputManagerData InputData => _inputData;

    private MobileControls inputActions;

    public TextMeshProUGUI text;

    private void Awake() {
        inputActions = new();
        inputActions.Enable();
        inputActions.Interaction.Touchscreen.Enable();
        inputActions.Locomotion.Joystick.started += OnJoystickStartAndPerform;
        inputActions.Locomotion.Joystick.performed += OnJoystickStartAndPerform;
        inputActions.Locomotion.Joystick.canceled += OnJoystickCancel;
    }
    private void OnJoystickStartAndPerform(InputAction.CallbackContext context) {
        _inputData.velocityIS = context.ReadValue<Vector2>();
    }
    private void OnJoystickCancel(InputAction.CallbackContext context) {
        _inputData.velocityIS.Set(0, 0);
    }

    private void OnDisable() {
        inputActions.Disable();
        inputActions.Locomotion.Joystick.started -= OnJoystickStartAndPerform;
        inputActions.Locomotion.Joystick.performed -= OnJoystickStartAndPerform;
        inputActions.Locomotion.Joystick.canceled -= OnJoystickCancel;
    }

    private void Update() {
        Vector2 touchData = GetTouchDelta();
        
        Debug.Log(touchData);
        text.text = touchData.ToString();

    }

    private Vector2 GetTouchDelta() {
        if (Touchscreen.current.touches.Count <= 0) return Vector2.zero;

        return Touchscreen.current.touches
            .Where(v => !EventSystem.current.IsPointerOverGameObject(v.touchId.ReadValue()) && v.isInProgress)
            .Select(v => v.delta.value).FirstOrDefault();
    }

    private Vector2 GetTouchDeltaInput() {
        if (Input.touchCount <= 0) return Vector2.zero;

        if (Input.GetTouch(0).phase == UnityEngine.TouchPhase.Moved) {
            return Input.GetTouch(0).deltaPosition;
        }
        return Vector2.zero;
    }
}
