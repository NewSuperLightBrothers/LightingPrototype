using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicCharacterController : MonoBehaviour
{
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private Transform _orientation;
    [SerializeField] private Transform _cameraRotationTarget;
    [SerializeField] private CapsuleCollider _mainCollider;
    
    [SerializeField] private GroundRay _groundRay;
    [SerializeField] private float _moveSpeedWS;

    private Rigidbody _rigidbody;
    private Vector2 _rotationOS;
    private Vector3 _velocityOS; 
    private Vector3 _velocityWS;

    [SerializeField] private float _jumpCoolTime;
    public bool isJumpable;

    [SerializeField] private List<Vector2> _d_airVel__g;
    private float _d_airVel__gCount;
    private int _jumpState;
    private float _currentGravity;
    private float _inverseFixedDeltaTime;

    private float _maxSlopeAngle;
    private float _maxSlopeAngleDeg;

    public List<Vector3> normals;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start() {
        isJumpable = true;
        _d_airVel__gCount = _d_airVel__g.Count;
        float capsuleDownCenterHeightOS = 1f + _mainCollider.gameObject.transform.position.y + (-_mainCollider.height / 2 + _mainCollider.radius);
        float capsuleRadius = _mainCollider.radius;
        _maxSlopeAngle = Mathf.PI / 2 - Mathf.Asin(capsuleRadius / capsuleDownCenterHeightOS);
        _maxSlopeAngleDeg = _maxSlopeAngle * Mathf.Rad2Deg;
    }

    private void FixedUpdate() {
        _inverseFixedDeltaTime = 1 / Time.fixedDeltaTime;
        RotateCamera();

        _velocityOS = Vector3XZ(_inputManager.InputData.velocityIS);
        _velocityOS = _orientation.rotation * _velocityOS;

        _groundRay.OnUpdate();

        PlayerMovement();

        JumpCharge();

        foreach (Vector3 normal in normals) {
            if (Vector3.Dot(_velocityWS, normal) > 0) continue;
            _velocityWS = Vector3.ProjectOnPlane(_velocityWS, normal);
        }

        _rigidbody.MovePosition(transform.position + _velocityWS * Time.fixedDeltaTime);
    }

    private void RotateCamera() {
        _rotationOS += _inputManager.InputData.swipeIS;

        _rotationOS.y = Mathf.Clamp(_rotationOS.y, -90f, 90f);
        _rotationOS.x %= 360f;

        _cameraRotationTarget.rotation = Quaternion.Euler(-_rotationOS.y, _rotationOS.x, 0);
    }

    private Vector3 Vector3XZ(Vector2 value) {
        return value.x * Vector3.right + value.y * Vector3.forward;
    }

    private void PlayerMovement() {
        // If player is in midair
        if (_groundRay.distanceToGround.isNull) {
            // Cycle through gravities
            while (_jumpState < _d_airVel__gCount && _velocityWS.y < _d_airVel__g[_jumpState].x) {
                _currentGravity = _d_airVel__g[_jumpState].y;
                _jumpState++;
            }
            _velocityWS.x = _velocityOS.x * _moveSpeedWS;
            _velocityWS.z = _velocityOS.z * _moveSpeedWS;
            _velocityWS.y += _currentGravity * Time.fixedDeltaTime;

            // Multiplied 1.1f for safety
            _groundRay.rayMaxDistance = _groundRay.rayDistance - _velocityWS.y * 1.1f * Time.fixedDeltaTime;
            return;
        }

        // Init jump state
        _jumpState = 0;

        // If player is able to jump
        if (_inputManager.InputData.isJump && isJumpable) {
            _velocityWS = _velocityOS * _moveSpeedWS;
            _velocityWS.y = 10f;
            _groundRay.distanceToGround.isNull = true;
            return;
        }

        // Added 0.2 for safety
        _groundRay.rayMaxDistance = _groundRay.rayDistance + 0.2f;

        if (_groundRay.isOnSlope) {
            // If player is in slope
            _velocityWS = GetVelocityWSfromSlope(_velocityOS) * _moveSpeedWS;
        } else {
            // If player is on flat surface
            _velocityWS = _velocityOS * _moveSpeedWS;
        }

        // Snap player to the ground
        _velocityWS -= _groundRay.distanceToGround.value * Vector3.up / Time.fixedDeltaTime;
    }

    private Vector3 GetVelocityWSfromSlope(Vector3 velocity) {
        return Vector3.ProjectOnPlane(velocity, _groundRay.normal).normalized * velocity.magnitude;
    }

    private void JumpCharge() {
        if (_inputManager.InputData.isJump && isJumpable && _jumpState == 0) {
            isJumpable = false;
            StartCoroutine(IJumpCharge());
        }
    }

    private IEnumerator IJumpCharge() {
        _groundRay.enableRay = false;
        yield return new WaitForSeconds(0.1f);
        _groundRay.enableRay = true;
        if (_jumpCoolTime > 0.1f) {
            yield return new WaitForSeconds(_jumpCoolTime - 0.1f);
        }
        isJumpable = true;
        yield return null;
    }

    private void OnCollisionEnter(Collision other) {
        int contactCount = other.contactCount;
        for (int c = 0; c < contactCount; c++) {
            if (normals.Contains(other.GetContact(c).normal)) continue;
            normals.Add(other.GetContact(c).normal);
        }
    }

    private void OnCollisionStay(Collision other) {
        int contactCount = other.contactCount;
        for (int c = 0; c < contactCount; c++) {
            if (normals.Contains(other.GetContact(c).normal)) continue;
            normals.Add(other.GetContact(c).normal);
        }
    }

    private void OnCollisionExit(Collision other) {
        normals.Clear();
    }
}
