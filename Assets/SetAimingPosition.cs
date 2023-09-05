using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAimingPosition : MonoBehaviour
{
    public float gunMaxDistance;
    public GameObject hitPosition;

    private void Update() {
        if (Physics.Raycast(transform.position - transform.forward * transform.localPosition.z, transform.forward,out RaycastHit hitInfo, gunMaxDistance)) {
            hitPosition.transform.position = hitInfo.point;
        } else {
            hitPosition.transform.position = transform.position + transform.forward * gunMaxDistance;
        }
    }
}
