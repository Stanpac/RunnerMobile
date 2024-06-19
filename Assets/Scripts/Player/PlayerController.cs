using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Lean.Touch;
using NaughtyAttributes;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

// This script is responsible for managing the player.
public class PlayerController : MonoBehaviour
{
    [SerializeField, BoxGroup("Data Settings"), Label("Player Controller Data")]
    private SO_PlayerController _data;
    
    [SerializeField, BoxGroup("Wheels")]
    WheelsToRotate _wheelsToRotate;
    
    [SerializeField, BoxGroup("Debug Settings")]
    private bool _showDebug = false;

    [SerializeField, BoxGroup("Debug Settings"), EnableIf("_showDebug")]
    private float _raylength = 2.0f;
    
    [SerializeField, BoxGroup("Debug Settings")]
    private bool _showWheelsDebug = false;
    
    
    private RaycastSuspension[] _wheels;
    
    private float _rotationAngle = 0;
    
    private bool _fingerOnScreen = false;
    private LeanFinger _currentfinger;
    
    public float _rotationFactorMultiplicator  = 10;
    private float _rotationFactor = 0;
    
    private string _dataPath => "ScriptableObject/SO_PlayerController";
    
    private void Reset()
    {
        if (_data == null)
            _data = Resources.Load<SO_PlayerController>(_dataPath);
    }

    private void Awake()
    {
        if (_data == null) 
            _data = Resources.Load<SO_PlayerController>(_dataPath);
        
        // Event  
        GameManager._instance.actionManager.OnFingerDown += OnFingerDown;
        GameManager._instance.actionManager.OnFirstFingerDown += OnFingerDown;
        GameManager._instance.actionManager.OnLastFingerUp += OnLastFingerUp;
        
        _rotationAngle = 0;
        
        _wheels = GetComponentsInChildren<RaycastSuspension>();
        foreach (var wheel in _wheels) {
            wheel.SetUpSpeedFactor(_data.speedFactor, _data.carTopSpeed, _data.powerCurve, _showWheelsDebug);
        }
    }

    private void Update()
    {
        float PreviousRotation = _rotationAngle;
        float rotation = CalculateRotation();
        
        //CalculateRotationFactorThisFrame(PreviousRotation, rotation);
        _wheelsToRotate.RotateWheels(rotation);
    }
    
    private float CalculateRotation()
    {
        float rotation = _rotationAngle;
       
        if (_fingerOnScreen) {
            if (_currentfinger.ScreenPosition.x > Screen.width / 2) {
                rotation =  Mathf.Clamp(rotation + Time.deltaTime / _data.timeForMaxRotation * _data.MaxRotation, -_data.MaxRotation, _data.MaxRotation);
            } else {
                rotation =  Mathf.Clamp(rotation - Time.deltaTime / _data.timeForMaxRotation * _data.MaxRotation, -_data.MaxRotation, _data.MaxRotation);
            }
        } else {
           rotation = Mathf.Lerp(rotation, 0, Time.deltaTime / _data.timeForMaxRotation);  
        }
        
        _rotationAngle = rotation;
        return _rotationAngle;
    }

    private void LateUpdate()
    {
        ClampRotation();
    }

    private void ClampRotation()
    {
        Vector3 axis = Vector3.Cross(Vector3.up, transform.up);
        float angle = Vector3.Angle(Vector3.up, transform.up);
        
        if (_showDebug) {
            Debug.DrawRay(transform.position, Vector3.up *_raylength, new Color(0.5f, 1f, 0.27f));
            Debug.DrawRay(transform.position, transform.up * _raylength, new Color(1f, 0.84f, 0.24f));
        }
        
        if (angle > _data.angleUpMaxRotation)
        {
            float diff = _data.angleUpMaxRotation - angle;
            var targetRotation = Quaternion.AngleAxis(diff, axis) * transform.rotation;
            
            if (_showDebug)
                Debug.DrawRay(transform.position, targetRotation * Vector3.up * _raylength, new Color(1f, 0.39f, 0.74f));
            
            var smoothRotation = Quaternion.Slerp(transform.rotation, targetRotation, _data.speedOfTheRotation * Time.deltaTime);
            
            transform.rotation = smoothRotation;
        } else {
            
        }
    }

    
    private void CalculateRotationFactorThisFrame(float PreviousRotation, float CurrentRotation)
    {
        if (_fingerOnScreen) {
            _rotationFactor = CurrentRotation - PreviousRotation;
        } else {
            _rotationFactor = Mathf.Lerp(_rotationFactor, 0, Time.deltaTime / _data.timeForMaxRotation);
        }
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
    
    public float GetRotationForStability()
    {
        return Mathf.Clamp(_rotationFactor * _rotationFactorMultiplicator, -1, 1);
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
