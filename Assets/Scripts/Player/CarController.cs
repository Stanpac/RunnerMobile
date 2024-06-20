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
public class CarController : MonoBehaviour
{
    [SerializeField, BoxGroup("Data Settings"), Label("Player Controller Data")]
    private SO_CarController _data;
    
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
    
    private string _dataPath => "ScriptableObject/SO_PlayerController";
    
    public  bool GetFinerOnScreen => _fingerOnScreen;
    
    private void Reset()
    {
        if (_data == null)
            _data = Resources.Load<SO_CarController>(_dataPath);
    }

    private void Awake()
    {
        if (_data == null) 
            _data = Resources.Load<SO_CarController>(_dataPath);
        
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
        
        
        _wheelsToRotate.RotateWheels(rotation);
    }
    
    private float CalculateRotation()
    {
        float rotation = _rotationAngle;
       
        if (_fingerOnScreen) {
            if (_currentfinger.ScreenPosition.x > Screen.width / 2) {
                rotation =  Mathf.Clamp(rotation + Time.deltaTime / _data.timeForMaxRotation * _data.maxRotation, -_data.maxRotation, _data.maxRotation);
            } else {
                rotation =  Mathf.Clamp(rotation - Time.deltaTime / _data.timeForMaxRotation * _data.maxRotation, -_data.maxRotation, _data.maxRotation);
            }
        } else {
            if (rotation > 0)
                rotation = Mathf.Lerp(rotation, _data.rotationCenterTreshold * _data.maxRotation, Time.deltaTime / _data.timeForReachTreshold);
            else if (rotation < 0) {
                rotation = Mathf.Lerp(rotation, - _data.rotationCenterTreshold * _data.maxRotation, Time.deltaTime / _data.timeForReachTreshold);  
            }
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
        
        if (angle > _data.angleUpMaxRotation) {
            float diff = _data.angleUpMaxRotation - angle;
            var targetRotation = Quaternion.AngleAxis(diff, axis) * transform.rotation;
            
            if (_showDebug)
                Debug.DrawRay(transform.position, targetRotation * Vector3.up * _raylength, new Color(1f, 0.39f, 0.74f));
            
            var smoothRotation = Quaternion.Slerp(transform.rotation, targetRotation, _data.speedOfTheRotation * Time.deltaTime);
            
            transform.rotation = smoothRotation;
        } else {
            
        }
    }
    
    // TODO : there is the Same code in Stability.cs, find a way to factor this
    // Maybe all this need to be in the InputManager and the CarController need to be a listener of the InputManager ? and check Current finger in the InputManager
    private void OnFingerDown(LeanFinger finger)
    {
        if (finger.IsOverGui) return;
        
        _fingerOnScreen = true;
        if (_currentfinger == null || !_currentfinger.Set) {
            _currentfinger = finger;
        }
    }
    
    private void OnLastFingerUp(LeanFinger finger)
    {
        _fingerOnScreen = false;
        _currentfinger = null;
    }

    private void OnEnable()
    {
        GameManager.Instance.actionManager.OnFingerDown += OnFingerDown;
        GameManager.Instance.actionManager.OnFirstFingerDown += OnFingerDown;
        GameManager.Instance.actionManager.OnLastFingerUp += OnLastFingerUp;
    }

    private void OnDisable()
    {
        GameManager.Instance.actionManager.OnFingerDown -= OnFingerDown;
        GameManager.Instance.actionManager.OnFirstFingerDown -= OnFingerDown;
        GameManager.Instance.actionManager.OnLastFingerUp -= OnLastFingerUp;
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
