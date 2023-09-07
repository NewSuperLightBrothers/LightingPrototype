using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAimingPosition : MonoBehaviour
{
    public float gunMaxDistance;
    public GameObject hitPosition;
    private new Rigidbody rigidbody;

    private void Awake() {
        rigidbody = hitPosition.GetComponent<Rigidbody>();
    }
    private void FixedUpdate() {
        if (Physics.Raycast(transform.position - transform.forward * (transform.localPosition.z - 0.5f), transform.forward,out RaycastHit hitInfo, gunMaxDistance)) {
            //hitPosition.transform.position = hitInfo.point;
            rigidbody.MovePosition(hitInfo.point);
        } else {
            //hitPosition.transform.position = transform.position + transform.forward * gunMaxDistance;
            rigidbody.MovePosition(transform.position + transform.forward * gunMaxDistance);
        }
    }
}
