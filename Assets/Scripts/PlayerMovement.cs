using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public InputManager inputManager;
    public Transform crosshair;
    private new Rigidbody rigidbody;
    private Vector3 velocityOS; 
    private Vector3 velocityWS;

    private Vector2 rotationOS;
    private RaycastHit _hitInfo;

    private Vector3 horizontalVelocityWS = Vector3.zero;

    public bool isDetectFloor = true;
    public bool isGrounded;
    public bool isFalling = false;

    void Start() {
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        velocityOS = new Vector3(inputManager.InputData.velocityIS.x, 0, inputManager.InputData.velocityIS.y);
        if (velocityOS.magnitude >= 1) velocityOS.Normalize();

        GetMoveDirection();

        GetAirborneMovement();

        rigidbody.MovePosition(this.transform.position + velocityWS * Time.deltaTime);

        RotateCamera();
    }

    private void RotateCamera() {
        rotationOS += inputManager.InputData.swipeIS * 0.1f;
        rotationOS.y = Mathf.Clamp(rotationOS.y, -90f, 90f);
        rotationOS.x %= 360f;

        crosshair.rotation = Quaternion.Euler(rotationOS.y, -rotationOS.x, 0);
    }

    private void GetMoveDirection() {
        if (Physics.Raycast(transform.position, Vector3.down, out _hitInfo, 1.1f)) {
            isDetectFloor = true;
            if (!isFalling) {
                isGrounded = true;
                float velocityOSsize = velocityOS.magnitude;

                velocityWS = Vector3.ProjectOnPlane(velocityOS, _hitInfo.normal).normalized * velocityOSsize;
                velocityWS += (1f - _hitInfo.distance) * Vector3.up / Time.deltaTime;
            }
        } else {
            isDetectFloor = false;
            velocityWS = velocityOS;
        }
    }

    private void GetAirborneMovement() {
        if (!isDetectFloor && !isFalling) {
            horizontalVelocityWS = Vector3.zero;
            isGrounded = false;
            isFalling = true;
        }
        if (!isGrounded) {
            velocityWS += horizontalVelocityWS;
            horizontalVelocityWS += Time.fixedDeltaTime * Physics.gravity;
            if (isDetectFloor && _hitInfo.distance + horizontalVelocityWS.y < 1f) {
                isFalling = false;
                horizontalVelocityWS = Vector3.zero;
            }
        } else isFalling = false;
    }

}
