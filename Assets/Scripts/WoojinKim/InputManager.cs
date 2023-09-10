using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class InputManagerData {
    public Vector2 velocityIS;
    public Vector2 swipeIS;
    public bool isJump;
    public bool isRun;
    public bool isFire;
    public Vector3 characterForwardWS;
}

public class InputManager : MonoBehaviour {
    [SerializeField] private InputManagerData _inputData;
    public InputManagerData InputData => _inputData;

    public MobileControls inputActions;
    public WeaponManager weaponManager;

    [SerializeField] private RectTransform _joystickButton;
    private Rect _rect;
    private Image _image;

    public float screenSensitivity;
    private Vector2 oldVelocityIS;

    private float pressTimer, releaseTimer;

    private void Awake() {
        inputActions = new();
        inputActions.Enable();
        inputActions.Interaction.Touchscreen.Enable();

        inputActions.Locomotion.Joystick.started -= OnJoystickStart;
        inputActions.Locomotion.Joystick.started += OnJoystickStart;
        inputActions.Locomotion.Joystick.performed -= OnJoystickStart;
        inputActions.Locomotion.Joystick.performed += OnJoystickStart;
        inputActions.Locomotion.Joystick.canceled -= OnJoystickCancel;
        inputActions.Locomotion.Joystick.canceled += OnJoystickCancel;

        inputActions.Interaction.SpaceKey.started -= OnSpaceKey;
        inputActions.Interaction.SpaceKey.started += OnSpaceKey;
        inputActions.Interaction.SpaceKey.performed -= OnSpaceKey;
        inputActions.Interaction.SpaceKey.performed += OnSpaceKey;
        inputActions.Interaction.SpaceKey.canceled -= OnSpaceKey;
        inputActions.Interaction.SpaceKey.canceled += OnSpaceKey;

        inputActions.Interaction.MouseLeft.started -= OnLeftClick;
        inputActions.Interaction.MouseLeft.started += OnLeftClick;
        inputActions.Interaction.MouseLeft.performed -= OnLeftClick;
        inputActions.Interaction.MouseLeft.performed += OnLeftClick;
        inputActions.Interaction.MouseLeft.canceled -= OnLeftClick;
        inputActions.Interaction.MouseLeft.canceled += OnLeftClick;
    }
    private void OnJoystickStart(InputAction.CallbackContext context) {
        _inputData.velocityIS = context.ReadValue<Vector2>();
    }
    private void OnJoystickCancel(InputAction.CallbackContext context) {
        _inputData.velocityIS.Set(0, 0);
    }

    private void OnSpaceKey(InputAction.CallbackContext context) {
        _inputData.isJump = context.ReadValue<float>() == 1;
    }

    private void OnLeftClick(InputAction.CallbackContext context) {
        _inputData.isFire = context.ReadValue<float>() == 1;
        weaponManager.OnLeftClick(_inputData.isFire);
    }

    private void OnDisable() {
        inputActions.Disable();
        inputActions.Interaction.Touchscreen.Disable();
        inputActions.Locomotion.Joystick.started -= OnJoystickStart;
        inputActions.Locomotion.Joystick.performed -= OnJoystickStart;
        inputActions.Locomotion.Joystick.canceled -= OnJoystickCancel;

        inputActions.Interaction.SpaceKey.started -= OnSpaceKey;
        inputActions.Interaction.SpaceKey.performed -= OnSpaceKey;
        inputActions.Interaction.SpaceKey.canceled -= OnSpaceKey;

        inputActions.Interaction.MouseLeft.started -= OnLeftClick;
        inputActions.Interaction.MouseLeft.performed -= OnLeftClick;
        inputActions.Interaction.MouseLeft.canceled -= OnLeftClick;
    }

    private void Start() {
        _rect = new Rect(_joystickButton.rect);
        _image = _joystickButton.gameObject.GetComponent<Image>();
        _rect.position = _joystickButton.parent.GetComponent<RectTransform>().anchoredPosition - (_rect.size /2) + new Vector2(_image.raycastPadding.x, _image.raycastPadding.y);
        _rect.size -= new Vector2(_image.raycastPadding.x, _image.raycastPadding.y) * 2;
        
    }
    private void Update() {
        Vector2 touchDelta = GetTouchDelta();
        _inputData.swipeIS = touchDelta * screenSensitivity;

    }

    private Vector2 GetTouchDelta() {
        if (Touchscreen.current == null) return Vector2.zero;
        if (Touchscreen.current.touches.Count <= 0) return Vector2.zero;

        return Touchscreen.current.touches
            .Where(v => !EventSystem.current.IsPointerOverGameObject(v.touchId.ReadValue()) && v.isInProgress && !_rect.Contains(v.startPosition.value))
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
