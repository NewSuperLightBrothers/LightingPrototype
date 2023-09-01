using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public InputManager inputManager;
    private new Rigidbody rigidbody;
    private Vector2 velocityOS; 
    private Vector3 velocityWS;
    

    void Start() {
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        velocityWS = new Vector3(inputManager.InputData.velocityIS.x, 0, inputManager.InputData.velocityIS.y);
        rigidbody.MovePosition(this.transform.position + velocityWS * Time.deltaTime);
    }
}
