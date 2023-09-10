using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PlayerMovement : MonoBehaviour {
    public InputManager inputManager;
    public Transform Orientation;
    public CapsuleCollider skinCollider;
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
    private int jumpState;
    private float currentGravity;

    public float maxSlopeAngle;
    public float maxSlopeAngleDeg;

    void Start() {
        isJumpable = true;
        rigidbody = GetComponent<Rigidbody>();
        d_airVel__gCount = d_airVel__g.Count;

        float capsuleDownCenterHeightOS = 1f + skinCollider.gameObject.transform.position.y + (-skinCollider.height / 2 + skinCollider.radius);
        float capsuleRadius = skinCollider.radius;
        maxSlopeAngle = Mathf.PI / 2 - Mathf.Asin(capsuleRadius / capsuleDownCenterHeightOS);
        maxSlopeAngleDeg = maxSlopeAngle * Mathf.Rad2Deg;
    }

    void FixedUpdate() {
        RotateCamera();

        velocityOS = new Vector3(inputManager.InputData.velocityIS.x, 0, inputManager.InputData.velocityIS.y);
        if (velocityOS.magnitude >= 1) velocityOS.Normalize();
        velocityOS = Orientation.rotation * velocityOS;

        groundRay.OnUpdate();

        if (groundRay.isOnSlope) velocityWS = GetVelocityWSfromSlope(velocityOS);
        else if (!groundRay.distanceToGround.isNull) {
            velocityWS = velocityOS;
            velocityWS *= moveSpeedWS;
        } else {
            velocityWS.x = velocityOS.x * moveSpeedWS;
            velocityWS.z = velocityOS.z * moveSpeedWS;
        }

        displacementWS = velocityWS * Time.fixedDeltaTime;

        if (!groundRay.distanceToGround.isNull) {
            if (inputManager.InputData.isJump && isJumpable) {
                airVelocityWS = 10f;
                velocityWS.y = airVelocityWS;
                displacementWS += airVelocityWS * Vector3.up * Time.fixedDeltaTime;
                groundRay.distanceToGround.isNull = true;
            } else {
                airVelocityWS = 0;
                displacementWS -= groundRay.distanceToGround.value * Vector3.up;
                groundRay.rayMaxDistance = groundRay.rayDistance + 0.2f;
            }
            jumpState = 0;
        } else {
            while (jumpState < d_airVel__gCount && displacementWS.y / Time.fixedDeltaTime < d_airVel__g[jumpState].x) {
                currentGravity = d_airVel__g[jumpState].y;
                jumpState++;
            }
            

            velocityWS.y += currentGravity * Time.fixedDeltaTime;
            
            displacementWS = velocityWS * Time.fixedDeltaTime;
            groundRay.rayMaxDistance = groundRay.rayDistance - velocityWS.y * 1.1f * Time.fixedDeltaTime;
            
        }

        JumpCharge();

        ChangeSkinData();
        
        foreach (Vector3 normal in points) {
            if (Vector3.Dot(displacementWS, normal) > 0) continue;
            displacementWS = Vector3.ProjectOnPlane(displacementWS, normal);
        }


        rigidbody.MovePosition(this.transform.position + displacementWS);
    }

    private void RotateCamera() {
        rotationOS += inputManager.InputData.swipeIS;

        rotationOS.y = Mathf.Clamp(rotationOS.y, -90f, 90f);
        rotationOS.x %= 360f;

        crosshair.rotation = Quaternion.Euler(-rotationOS.y, rotationOS.x, 0);
    }


    private Vector3 GetVelocityWSfromSlope(Vector3 velocity) {
        return Vector3.ProjectOnPlane(velocity, groundRay.normal).normalized * velocity.magnitude;
    }

    private void JumpCharge() {
        if (inputManager.InputData.isJump && isJumpable && jumpState == 0) {
            isJumpable = false;
            StartCoroutine(IJumpCharge());
        }
    }

    private void ChangeSkinData() {
        float skinDelta = displacementWS.magnitude;
        //Debug.Log("skinDelta: " + skinDelta);
        //skinCollider.radius = initialSkinColliderRadius + skinDelta;
        //skinCollider.height = initialSkinColliderHeight + skinDelta * 2;
        skinCollider.center = displacementWS;
    }

    public List<Vector3> points;

    private IEnumerator IJumpCharge() {
        groundRay.enableRay = false;
        yield return new WaitForSeconds(0.1f);
        groundRay.enableRay = true;
        if (jumpCoolTime > 0.1f) {
            yield return new WaitForSeconds(jumpCoolTime - 0.1f);
        }
        isJumpable = true;
        yield return null;
    }

    private void OnDrawGizmos() {
        Gizmos.DrawSphere(transform.position + displacementWS, 0.02f);
        foreach (Vector3 point in points) {
            Gizmos.DrawSphere(point, 0.01f);
        }
    }
    /*
    private void OnCollisionStay(Collision other)
    {
        points = Enumerable.Range(0, other.contactCount)
        .Select(i => other.GetContact(i).normal)
        .ToList();
    }
    */
    private void OnCollisionEnter(Collision other) {
        int contactCount = other.contactCount;
        for (int c = 0; c < contactCount; c++) {
            if (points.Contains(other.GetContact(c).normal)) continue;
            points.Add(other.GetContact(c).normal);
        }
    }

    private void OnCollisionStay(Collision other) {
        int contactCount = other.contactCount;
        for (int c = 0; c < contactCount; c++) {
            if (points.Contains(other.GetContact(c).normal)) continue;
            points.Add(other.GetContact(c).normal);
        }
    }

    private void OnCollisionExit(Collision other) {
        points.Clear();
    }
}
