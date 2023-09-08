using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerControl : NetworkBehaviour
{
    [SerializeField] private float _walkSpeed = 5f;

    [SerializeField] private Vector2 defaultPositionRange = new Vector2(-4, 4);

    [SerializeField] private NetworkVariable<float> forwardBackPosition = new();
    
    [SerializeField] private NetworkVariable<float> leftRightPosition = new();

    // client caching
    private float _oldForwardBackPosition;
    private float _oldLeftRightPosition;

    private void Start()
    {
        transform.position = new Vector3(Random.Range(defaultPositionRange.x, defaultPositionRange.y), 0,
            Random.Range(defaultPositionRange.x, defaultPositionRange.y));
    }

    private void Update()
    {
        if (IsServer)
        {
            UpdateServer();   
        }
        
        if (IsClient && IsOwner)
        {
            UpdateClient();
        }
    }


    private void UpdateServer()
    {
        transform.position = new Vector3(transform.position.x + leftRightPosition.Value, transform.position.y,
            transform.position.z + forwardBackPosition.Value);
    }
    
    private void UpdateClient()
    {
        float forwardBackward = 0;
        float leftRight = 0;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            forwardBackward += _walkSpeed;
        }
        
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            forwardBackward -= _walkSpeed;
        }
        
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            leftRight -= _walkSpeed;
        }
        
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            leftRight += _walkSpeed;
        }

        if (_oldForwardBackPosition != forwardBackward || _oldLeftRightPosition != leftRight)
        {
            _oldForwardBackPosition = forwardBackward;
            _oldLeftRightPosition = leftRight;
            
            // Update server
            UpdateClientPositionServerRpc(forwardBackward, leftRight);
        }
    }

    [ServerRpc]
    private void UpdateClientPositionServerRpc(float forwardBackward, float leftRight)
    {
        forwardBackPosition.Value = forwardBackward;
        leftRightPosition.Value = leftRight;
    }
}
