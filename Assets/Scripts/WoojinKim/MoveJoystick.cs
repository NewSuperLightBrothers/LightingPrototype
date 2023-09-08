using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveJoystick : MonoBehaviour
{
    public RectTransform target;
    private RectTransform rectTransform;

    [SerializeField] private float distance;

    private void Start() {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update() {
        Vector3 position = target.localPosition;
        if (position.magnitude > distance) position = Vector2.ClampMagnitude(position, distance);

        rectTransform.localPosition = position;
    }

}
