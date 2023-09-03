using System.Collections;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Realtime;

[System.Serializable]
public class Nullable<T> {
    public T value;
    public bool isNull;
}

public class GroundRay : MonoBehaviour
{
    public Nullable<Vector3> point;
    public Vector3 normal;
    public Nullable<float> distanceToGround;

    public bool isOnSlope;

    private LineRenderer _lineRenderer;
    public float rayDistance;
    public float rayMaxDistance;

    private void Start() {
        _lineRenderer = GetComponent<LineRenderer>();
        rayDistance = Vector3.Distance(_lineRenderer.GetPosition(0), _lineRenderer.GetPosition(1));
        rayMaxDistance = rayDistance + 0.2f;
    }

    public void OnUpdate() {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, rayMaxDistance)) {
            point.isNull = false;
            point.value = hitInfo.point;

            normal = hitInfo.normal;

            distanceToGround.isNull = false;
            distanceToGround.value = hitInfo.distance - rayDistance;
        } else {
            point.isNull = true;
            normal = Vector3.up;
            distanceToGround.isNull = true;
        }
        
        isOnSlope = !(point.isNull || normal == Vector3.up);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, rayMaxDistance * Vector3.down);
    }
}
