using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Lean.Touch;
using NaughtyAttributes;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

// This script is responsible for managing the player.
public class PlayerController : MonoBehaviour
{
    [SerializeField, BoxGroup("Data Settings"), Label("Player Controller Data")]
    private SO_PlayerController _data;
    
    [FormerlySerializedAs("_WheelsToRotate")] [SerializeField, BoxGroup("Wheels")]
    WheelsToRotate _wheelsToRotate;
    
    private RaycastSuspension[] _wheels;
    
    private float _rotationAngle = 0;
    
    private bool _fingerOnScreen = false;
    private LeanFinger _currentfinger;
    
    
    private void Reset()
    {
        if (_data == null)
            _data = Resources.Load<SO_PlayerController>("SO_PlayerController");
    }

    private void Awake()
    {
        if (_data == null) 
            _data = Resources.Load<SO_PlayerController>("SO_PlayerController");
        
        // Event  
        GameManager._instance.actionManager.OnFingerDown += OnFingerDown;
        GameManager._instance.actionManager.OnFirstFingerDown += OnFingerDown;
        GameManager._instance.actionManager.OnLastFingerUp += OnLastFingerUp;
        
        _rotationAngle = 0;
        
        _wheels = GetComponentsInChildren<RaycastSuspension>();
        foreach (var wheel in _wheels) {
            wheel.SetUpSpeedFactor(_data.speedFactor, _data.carTopSpeed, _data.powerCurve);
        }
    }

    private void Update()
    {
        float rotation = CalculateRotation();
        _wheelsToRotate.RotateWheels(rotation);
    }

    private float CalculateRotation()
    {
        float rotation = _rotationAngle;
       
        if (_fingerOnScreen) {
            if (_currentfinger.ScreenPosition.x > Screen.width / 2) {
                rotation =  Mathf.Clamp(rotation + Time.deltaTime / _data.timeForMaxRotation * _data.angleMaxRotation, -_data.angleMaxRotation, _data.angleMaxRotation);
            } else {
                rotation =  Mathf.Clamp(rotation - Time.deltaTime / _data.timeForMaxRotation * _data.angleMaxRotation, -_data.angleMaxRotation, _data.angleMaxRotation);
            }
        } else {
           rotation = Mathf.Lerp(rotation, 0, Time.deltaTime / _data.timeForMaxRotation);  
        }
       
        _rotationAngle = rotation;
        return _rotationAngle;
    }
    
    private void OnFingerDown(LeanFinger finger)
    {
        _fingerOnScreen = true;
        if (_currentfinger == null || !_currentfinger.Set) {
            _currentfinger = finger;
        }
    }
    
    private void OnLastFingerUp(LeanFinger obj)
    {
        _fingerOnScreen = false;
        _currentfinger = null;
    }
}

[Serializable]
public struct WheelsToRotate
{
    [SerializeField] private List<RaycastSuspension> Wheels;
    
    public void RotateWheels(float rotation)
    {
        foreach (var wheel in Wheels) {
            wheel.transform.rotation = Quaternion.Euler(0, rotation, 0);
        }
    }
}
