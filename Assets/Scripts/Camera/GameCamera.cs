using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;


public class GameCamera : MonoBehaviour
{ 
    public Transform _target {get; set;}
    
    [SerializeField, BoxGroup("Offset")] Vector3 _moveOffset;
    [SerializeField, BoxGroup("Offset")] Vector3 _rotOffset;
    
    private void Update()
    {
        LookAtTarget();
    }
    
    private void LookAtTarget()
    {
        if (_target == null) return;
        Vector3 targetPos = _target.TransformPoint(_moveOffset);
        transform.position = targetPos;
        transform.LookAt(_target.position + _rotOffset);
    }
    
}
