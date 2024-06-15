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
    
    [SerializeField, BoxGroup("Wheels")]
    WheelsToRotate _WheelsToRotate;
    
    private RaycastSuspension[] _Wheels;
    
    private float _RotationAngle = 0;
    
    private bool _FingerOnScreen = false;
    private LeanFinger _Currentfinger;
    
    private void Awake()
    {
        _data = Resources.Load<SO_PlayerController>("SO_PlayerController");
        
        // Event  
        GameManager._instance.actionManager.OnFingerDown += OnFingerDown;
        GameManager._instance.actionManager.OnFirstFingerDown += OnFingerDown;
        GameManager._instance.actionManager.OnLastFingerUp += OnLastFingerUp;
        
        _RotationAngle = 0;
        
        _Wheels = GetComponentsInChildren<RaycastSuspension>();
        foreach (var wheel in _Wheels) {
            wheel.SetUpSpeedFactor(_data.speedFactor, _data.carTopSpeed, _data.powerCurve);
        }
    }

    private void Update()
    {
        float rotation = CalculateRotation();
        _WheelsToRotate.RotateWheels(rotation);
    }

    private float CalculateRotation()
    {
        float rotation = _RotationAngle;
       
        if (_FingerOnScreen) {
            if (_Currentfinger.ScreenPosition.x > Screen.width / 2) {
                rotation =  Mathf.Clamp(rotation + Time.deltaTime / _data.timeForMaxRotation * _data.angleMaxRotation, -_data.angleMaxRotation, _data.angleMaxRotation);
            } else {
                rotation =  Mathf.Clamp(rotation - Time.deltaTime / _data.timeForMaxRotation * _data.angleMaxRotation, -_data.angleMaxRotation, _data.angleMaxRotation);
            }
        } else {
           rotation = Mathf.Lerp(rotation, 0, Time.deltaTime / _data.timeForMaxRotation);  
        }
       
        _RotationAngle = rotation;
        return _RotationAngle;
    }
    
    private void OnFingerDown(LeanFinger finger)
    {
        _FingerOnScreen = true;
        if (_Currentfinger == null || !_Currentfinger.Set) {
            _Currentfinger = finger;
        }
    }
    
    private void OnLastFingerUp(LeanFinger obj)
    {
        _FingerOnScreen = false;
        _Currentfinger = null;
        
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
