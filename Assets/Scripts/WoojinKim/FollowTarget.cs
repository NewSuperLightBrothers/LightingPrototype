using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    private Vector3 latePosition;
    private Vector3 velocity = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        //Vector3 velocity = (transform.position - latePosition) / Time.deltaTime;
        transform.position = Vector3.SmoothDamp(transform.position, target.position,ref velocity, 0.03f);
        transform.rotation = target.rotation;
        latePosition = transform.position;
    }
}
