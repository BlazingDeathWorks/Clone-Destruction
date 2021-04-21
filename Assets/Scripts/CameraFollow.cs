using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    Transform target = null;

    private void LateUpdate()
    {
        if (target == null) return;
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
    }
}
