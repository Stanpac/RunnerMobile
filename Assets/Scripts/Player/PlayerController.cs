using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using NaughtyAttributes;
using UnityEngine;

// This script is responsible for managing the player.
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public Transform CarModel;
    
    public float AngleMaxrotation = 45;
    public float TimeForMaxRotation = 1f;
    
    private float RotationAngle = 0;
    
    private bool FingerOnScreen = false;
    private LeanFinger _Currentfinger;
    
    public void SetRotation(Quaternion rotation)
    {
        CarModel.rotation = Quaternion.Lerp(CarModel.rotation, rotation, Time.deltaTime * 10f);
    }

    private void Awake()
    {
        // Event  
        GameManager.instance.actionManager.OnFingerDown += OnFingerDown;
        GameManager.instance.actionManager.OnFirstFingerDown += OnFingerDown;
        GameManager.instance.actionManager.OnLastFingerUp += OnLastFingerUp;
        
        RotationAngle = 0;
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
