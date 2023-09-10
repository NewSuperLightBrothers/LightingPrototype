using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlayerControl : NetworkBehaviour
{
    public enum PlayerState
    {
        Idle,
        Walk,
        ReverseWalk,
    }

    [SerializeField] private float _speed = 5f;
    
    [SerializeField] private float _rotationSpeed = 1.5f;

    [SerializeField] private Vector2 defaultInitialPlanePosition = new Vector2(-4, 4);

    [SerializeField] private NetworkVariable<Vector3> networkPositionDirection = new();

    [SerializeField] private NetworkVariable<Vector3> networkRotationRotation = new();

    [SerializeField] private NetworkVariable<PlayerState> networkPlayerState = new();

    private CharacterController _characterController;

    private Animator _animator;
    
    // client cache position
    private Vector3 _oldInputPosition;
    private Vector3 _oldInputRotation;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        
    }

    private void Start()
    {
        if (IsClient && IsOwner)
        {
             transform.position = new Vector3(Random.Range(defaultInitialPlanePosition.x, defaultInitialPlanePosition.y), 0,
                Random.Range(defaultInitialPlanePosition.x, defaultInitialPlanePosition.y));
        }
    }

    private void Update()
    {
        if (IsClient && IsOwner)
        {
            ClientInput();
        }

        ClientMoveAndRotate();

        ClientVisuals();
    }

    private void ClientInput()
    {
        // Player position and rotation input
        // Rotation
        Vector3 inputRotation = new Vector3(0, Input.GetAxis("Horizontal"), 0);
        
        // Position
        Vector3 direction = transform.TransformDirection(Vector3.forward);
        float forwardInput = Input.GetAxis("Vertical");
        Vector3 inputPosition = direction * forwardInput;
        
        if (_oldInputPosition != inputPosition || _oldInputRotation != inputRotation)
        {
            _oldInputPosition = inputPosition;
            _oldInputRotation = inputRotation;
            UpdateClientPositionAndRotationServerRpc(inputPosition * _speed, inputRotation * _rotationSpeed);
        }

        // Player state changes based on input
        if (forwardInput > 0)
        {
            UpdatePlayerStateServerRpc(PlayerState.Walk);
        }
        else if (forwardInput < 0)
        {
            UpdatePlayerStateServerRpc(PlayerState.ReverseWalk);
        }
        else
        {
            UpdatePlayerStateServerRpc(PlayerState.Idle);
        }
    }

    private void ClientMoveAndRotate()
    {
        if (networkPositionDirection.Value != Vector3.zero)
        {
            _characterController.SimpleMove(networkPositionDirection.Value);
        }
        if(networkRotationRotation.Value != Vector3.zero)
        {
            transform.Rotate(networkRotationRotation.Value, Space.World); 
        }
    }

    private void ClientVisuals()
    {
        if (networkPlayerState.Value == PlayerState.Walk)
        {
            _animator.SetFloat("Walk", 1);
        }
        else if (networkPlayerState.Value == PlayerState.ReverseWalk)
        {
            _animator.SetFloat("Walk", -1);
        }
        else
        {
            _animator.SetFloat("Walk", 0);
        }
    }

    [ServerRpc]
    private void UpdateClientPositionAndRotationServerRpc(Vector3 newPositionDirection, Vector3 newRotationDirection)
    {
        networkPositionDirection.Value = newPositionDirection;
        networkRotationRotation.Value = newRotationDirection;
    }

    [ServerRpc]
    public void UpdatePlayerStateServerRpc(PlayerState newState)
    {
        networkPlayerState.Value = newState;
    }
}
