using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetWallCollider : MonoBehaviour
{
    public CapsuleCollider capsuleCollider;
    public ContactPoint[] l_contactPoint;
    public List<Vector3> l_point;
    private void OnCollisionEnter(Collision other) {
        l_contactPoint = other.contacts;
        l_point.Clear();
        foreach (ContactPoint p in other.contacts) {
            l_point.Add(p.normal);
        }
    }
    private void OnCollisionStay(Collision other) {
        l_contactPoint = other.contacts;
        l_point.Clear();
        foreach (ContactPoint p in other.contacts) {
            l_point.Add(p.normal);
        }
    }
    private void OnCollisionExit(Collision other) {
        l_contactPoint = other.contacts;
        l_point.Clear();
        foreach (ContactPoint p in other.contacts) {
            l_point.Add(p.normal);
        }
    }
}
