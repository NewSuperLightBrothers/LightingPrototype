using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAimingPosition : MonoBehaviour
{
    public float gunMaxDistance;
    public GameObject aimPosition;
    public GameObject aimStart;
    private new Rigidbody rigidbody;

    private void Awake() {
        rigidbody = aimPosition.GetComponent<Rigidbody>();
    }
    private void FixedUpdate() {
        if (Physics.Raycast(aimStart.transform.position, transform.forward, out RaycastHit hitInfo, gunMaxDistance)) {
            //hitPosition.transform.position = hitInfo.point;
            //rigidbody.MovePosition(hitInfo.point);
            aimPosition.transform.localPosition = hitInfo.distance * Vector3.forward;
        } else {
            //hitPosition.transform.position = transform.position + transform.forward * gunMaxDistance;
            aimPosition.transform.localPosition = gunMaxDistance * Vector3.forward;
        }
    }
}
