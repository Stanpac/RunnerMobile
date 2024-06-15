using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraFollow : MonoBehaviour
{
    [SerializeField, BoxGroup("Target")] 
    public Transform _target;
    
    [SerializeField, BoxGroup("Smoothness")] 
    float _moveSmoothness;
    
    [SerializeField, BoxGroup("Smoothness")] 
    float _rotSmoothness;

    [SerializeField, BoxGroup("Offset")] 
    Vector3 _moveOffset;
    
    [SerializeField, BoxGroup("Offset")]
    Vector3 _rotOffset;

    private void FixedUpdate()
    {
        FollowTarget();
    }

    private void FollowTarget()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        Vector3 targetPos = new Vector3();
        if (_target == null)  return;
        
        targetPos = _target.TransformPoint(_moveOffset);

        transform.position = Vector3.Lerp(transform.position, targetPos, _moveSmoothness * Time.deltaTime);
    }

    private void HandleRotation()
    {
        if (_target == null) return;
        var direction = _target.position - transform.position;
        var rotation = new Quaternion();

        rotation = Quaternion.LookRotation(direction + _rotOffset, Vector3.up);

        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, _rotSmoothness * Time.deltaTime);
    }
}
