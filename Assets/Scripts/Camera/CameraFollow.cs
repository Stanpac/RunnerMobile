using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraFollow : MonoBehaviour
{
    [SerializeField, BoxGroup("Target")] public Transform target;
    
    [SerializeField, BoxGroup("Smoothness")] float moveSmoothness;
    [SerializeField, BoxGroup("Smoothness")] float rotSmoothness;

    [SerializeField, BoxGroup("Offset")] Vector3 moveOffset;
    [SerializeField, BoxGroup("Offset")] Vector3 rotOffset;

    void FixedUpdate()
    {
        FollowTarget();
    }

    void FollowTarget()
    {
        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
    {
        Vector3 targetPos = new Vector3();
        if (target == null)  return;
        
        targetPos = target.TransformPoint(moveOffset);

        transform.position = Vector3.Lerp(transform.position, targetPos, moveSmoothness * Time.deltaTime);
    }

    void HandleRotation()
    {
        if (target == null) return;
        var direction = target.position - transform.position;
        var rotation = new Quaternion();

        rotation = Quaternion.LookRotation(direction + rotOffset, Vector3.up);

        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotSmoothness * Time.deltaTime);
    }
}
