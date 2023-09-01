using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.EventSystems;
using TMPro;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine.UI;

[System.Serializable]
public class InputManagerData {
    public Vector2 velocityIS;

    public Vector2 swipeIS;
    public bool isJump;
    public bool isFire;
    public Vector3 characterForwardWS;
}

public class InputManager : MonoBehaviour {
    [SerializeField] private InputManagerData _inputData;
    public InputManagerData InputData => _inputData;

    private MobileControls inputActions;

    [SerializeField] private RectTransform _joystickButton;
    private Rect _rect;
    private Image _image;

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

    private void Start() {
        _rect = new Rect(_joystickButton.rect);
        _image = _joystickButton.gameObject.GetComponent<Image>();
        _rect.position = _joystickButton.parent.Get<RectTransform>().anchoredPosition - (_rect.size /2) + new Vector2(_image.raycastPadding.x, _image.raycastPadding.y);
        _rect.size -= new Vector2(_image.raycastPadding.x, _image.raycastPadding.y) * 2;
    }
    private void Update() {
        Vector2 touchDelta = GetTouchDelta();
        _inputData.swipeIS = touchDelta;
        
        text.text = touchDelta.ToString() + "\n" + Touchscreen.current.primaryTouch.startPosition.value;
    }

    private Vector2 GetTouchDelta() {
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
