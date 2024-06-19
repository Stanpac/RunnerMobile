using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class _TestClampQuaternion : MonoBehaviour
{
    [SerializeField] private float _maxAngle = 45f;
    [SerializeField] private float _slerpStep = 5f;
    
    private void LateUpdate()
    {
        Vector3 axis = Vector3.Cross(Vector3.up, transform.up);
        float angle = Vector3.Angle(Vector3.up, transform.up);
        
        Debug.DrawRay(transform.position, Vector3.up, new Color(0.5f, 1f, 0.27f));
        Debug.DrawRay(transform.position, transform.up, new Color(1f, 0.84f, 0.24f));
        
        
        if (angle > _maxAngle)
        {
            float diff = _maxAngle - angle;
            var targetRotation = Quaternion.AngleAxis(diff, axis) * transform.rotation;
            Debug.DrawRay(transform.position, targetRotation * Vector3.up, new Color(1f, 0.39f, 0.74f));
            
            var smoothRotation = Quaternion.Slerp(transform.rotation, targetRotation, _slerpStep * Time.deltaTime);
            transform.rotation = smoothRotation;
        }
    }
}
