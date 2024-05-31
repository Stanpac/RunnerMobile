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
    private SO_PlayerController Data;
    
    [SerializeField, BoxGroup("Wheels")]
    WheelsToRotate _WheelsToRotate;
    
    private RaycastSuspension[] _Wheels;
    
    private float RotationAngle = 0;
    
    private bool FingerOnScreen = false;
    private LeanFinger _Currentfinger;
    
    private void Awake()
    {
        Data = Resources.Load<SO_PlayerController>("SO_PlayerController");
        
        // Event  
        GameManager.instance.actionManager.OnFingerDown += OnFingerDown;
        GameManager.instance.actionManager.OnFirstFingerDown += OnFingerDown;
        GameManager.instance.actionManager.OnLastFingerUp += OnLastFingerUp;
        
        RotationAngle = 0;
        
        _Wheels = GetComponentsInChildren<RaycastSuspension>();
        foreach (var wheel in _Wheels) {
            wheel.SetUpSpeedFactor(Data.SpeedFactor, Data.carTopSpeed, Data.powerCurve);
        }
    }

    private void Update()
    {
        float rotation = CalculateRotation();
        _WheelsToRotate.RotateWheels(rotation);
    }

    private float CalculateRotation()
    {
        float rotation = RotationAngle;
       
        if (FingerOnScreen) {
            if (_Currentfinger.ScreenPosition.x > Screen.width / 2) {
                rotation =  Mathf.Clamp(rotation + Time.deltaTime / Data.TimeForMaxRotation * Data.AngleMaxRotation, -Data.AngleMaxRotation, Data.AngleMaxRotation);
            } else {
                rotation =  Mathf.Clamp(rotation - Time.deltaTime / Data.TimeForMaxRotation * Data.AngleMaxRotation, -Data.AngleMaxRotation, Data.AngleMaxRotation);
            }
        } else {
           rotation = Mathf.Lerp(rotation, 0, Time.deltaTime / Data.TimeForMaxRotation);  
        }
       
        RotationAngle = rotation;
        return RotationAngle;
    }
    
    private void OnFingerDown(LeanFinger finger)
    {
        FingerOnScreen = true;
        if (_Currentfinger == null || !_Currentfinger.Set) {
            _Currentfinger = finger;
        }
    }
    
    private void OnLastFingerUp(LeanFinger obj)
    {
        FingerOnScreen = false;
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
