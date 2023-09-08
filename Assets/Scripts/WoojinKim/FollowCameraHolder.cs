using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCameraHolder : MonoBehaviour
{
    public Transform cameraHolder;

    private void LateUpdate() {
        transform.rotation = Quaternion.Euler(new Vector3(0, cameraHolder.rotation.eulerAngles.y, 0));
    }
}
