using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public float jumpCoolTime;
    public bool isJumpable;

    /// <summary>
    /// key 이상의 속도는 value의 gravity를 가진다
    /// </summary>
    public List<Vector2> d_airVel__g;
    private float d_airVel__gCount;
    public int jumpState;
    public float currentGravity;

    void Start() {
        isJumpable = true;
        rigidbody = GetComponent<Rigidbody>();
        d_airVel__gCount = d_airVel__g.Count;
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
            if (inputManager.InputData.isJump && isJumpable) {
                airVelocityWS = 10f;
                displacementWS += airVelocityWS * Vector3.up * Time.fixedDeltaTime;
                groundRay.distanceToGround.isNull = true;
            } else {
                airVelocityWS = 0;
                displacementWS -= groundRay.distanceToGround.value * Vector3.up;
                groundRay.rayMaxDistance = groundRay.rayDistance + 0.2f;
            }
            jumpState = 0;
        } else {
            while (jumpState < d_airVel__gCount && airVelocityWS < d_airVel__g[jumpState].x) {
                currentGravity = d_airVel__g[jumpState].y;
                jumpState++;
            }

            airVelocityWS += currentGravity * Time.fixedDeltaTime;
            displacementWS += airVelocityWS * Vector3.up * Time.fixedDeltaTime;
            groundRay.rayMaxDistance = groundRay.rayDistance - airVelocityWS * 1.1f * Time.fixedDeltaTime;
            
        }

        if (inputManager.InputData.isJump && isJumpable) {
            isJumpable = false;
            StartCoroutine(IJumpCharge());
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

    private IEnumerator IJumpCharge() {
        groundRay.enableRay = false;
        yield return new WaitForSeconds(0.1f);
        groundRay.enableRay = true;
        yield return new WaitForSeconds(jumpCoolTime - 0.1f);
        isJumpable = true;
        yield return null;
    }
}
