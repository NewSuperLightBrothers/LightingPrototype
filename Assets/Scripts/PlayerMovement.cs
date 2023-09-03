using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour {
    public InputManager inputManager;
    public Transform Orientation;
    public Transform crosshair;

    public GroundRay groundRay;
    public float moveSpeedWS;

    private new Rigidbody rigidbody;
    private Vector2 rotationOS;
    private Vector3 velocityOS; 
    private Vector3 velocityWS;
    private Vector3 displacementWS;

    private float airVelocityWS;

    void Start() {
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        RotateCamera();

        velocityOS = new Vector3(inputManager.InputData.velocityIS.x, 0, inputManager.InputData.velocityIS.y);
        if (velocityOS.magnitude >= 1) velocityOS.Normalize();
        velocityOS = Orientation.rotation * velocityOS;

        groundRay.OnUpdate();

        if (groundRay.isOnSlope) velocityWS = GetVelocityWSfromSlope(velocityOS);
        else velocityWS = velocityOS;

        velocityWS *= moveSpeedWS;

        displacementWS = velocityWS * Time.fixedDeltaTime;

        if (!groundRay.distanceToGround.isNull) {
            if (inputManager.InputData.isJump) {
                airVelocityWS = 10f;
                displacementWS += airVelocityWS * Vector3.up * Time.fixedDeltaTime;
                groundRay.distanceToGround.isNull = true;
            } else {
                airVelocityWS = 0;
                displacementWS -= groundRay.distanceToGround.value * Vector3.up;
                groundRay.rayMaxDistance = groundRay.rayDistance + 0.2f;
            }
        } else {
            airVelocityWS += -9.8f * Time.fixedDeltaTime;
            displacementWS += airVelocityWS * Vector3.up * Time.fixedDeltaTime;
            groundRay.rayMaxDistance = groundRay.rayDistance - airVelocityWS * 1.1f * Time.fixedDeltaTime;
        }

        rigidbody.MovePosition(this.transform.position + displacementWS);
    }

    private void RotateCamera() {
        rotationOS += inputManager.InputData.swipeIS * 0.1f;

        rotationOS.y = Mathf.Clamp(rotationOS.y, -90f, 90f);
        rotationOS.x %= 360f;

        crosshair.rotation = Quaternion.Euler(rotationOS.y, -rotationOS.x, 0);
    }
    
    private Vector3 GetVelocityWSfromSlope(Vector3 velocity) {
        return Vector3.ProjectOnPlane(velocity, groundRay.normal).normalized * velocity.magnitude;
    }


}
