using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform follow = null;
    private Vector3 offset = Vector3.zero;
    void Start()
    {
        offset = this.transform.position - follow.position;
    }

    void LateUpdate()
    {
        if(follow != null)
        {
            transform.position = follow.position + offset;
        }
    }
}
