using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Lean.Touch;
using UnityEngine;

/*------------------- Struct / enum -------------------*/
public enum Axel
{
    front,
    rear
}
    
[Serializable]
public struct Wheel
{
    public GameObject WheelModel;
    public WheelCollider wheelCollider;
    public Axel axel;
}

/*------------------- End Struct / enum -------------------*/
public class CarController : MonoBehaviour
{
    /*------------------- public / SerializeField variable -------------------*/
    
    [SerializeField, BoxGroup("Acceleration")] private float _maxAcceleration = 30.0f;
    
    [SerializeField, BoxGroup("Rotate Param")] private float turnSensitivity = 1.0f;
    [SerializeField, BoxGroup("Rotate Param")] private float maxSteerAngle = 30.0f;
    
    [SerializeField, BoxGroup("Wheels")] private List<Wheel> wheels;
    
    /*------------------- End public / SerializeField variable -------------------*/
    /*------------------- Private Variables -------------------*/
    
    private float moveInput;
    private float steerInput;
    
    private Rigidbody _carRigidbody;
    
    [NonSerialized]
    private List<LeanFinger> fingers = new List<LeanFinger>();
    
    /*------------------- End Private Variables -------------------*/
    private void Awake()
    {
        _carRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        moveInput = 1;
    }
    

    private void Update()
    {
        Move();
        Steer();
    }
    
    void Move()
    {
        foreach(var wheel in wheels)  {
            wheel.wheelCollider.motorTorque = moveInput * 600 * _maxAcceleration * Time.deltaTime;
        }
    }
    
    void Steer()
    {
        foreach(var wheel in wheels)
        {
            if (wheel.axel == Axel.front)
            {
                var _steerAngle = steerInput * turnSensitivity * maxSteerAngle;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, _steerAngle, 0.6f);
            }
        }
    }
    
    public void HandleSteer(float value)
    {
        steerInput = value;
    }
    
    [Button]
    private void StartMove()
    {
        moveInput = 1;
    }
    
    [Button]
    private void StopMove()
    {
        moveInput = 0;
    }
    
    // TODO Move all the Check for the Touch Input In a SubSystem for Touch Event
    private void HandleFingerUpdate(LeanFinger finger)
    {
        if (finger.IsOverGui) return;
        
    }
    private void HandleFingerDown(LeanFinger finger)
    {
        if (finger.IsOverGui) return;
        fingers.Add(finger);
    }

    private void HandleFingerUp(LeanFinger finger)
    {
        fingers.Remove(finger);
        
        if (fingers.Count == 0) {
            //Last Finger Up
        }
    }
    
    private void OnEnable()
    {
        LeanTouch.OnFingerUpdate += HandleFingerUpdate;
        LeanTouch.OnFingerDown += HandleFingerDown;
        LeanTouch.OnFingerUp += HandleFingerUp;
    }


    private void OnDisable()
    {
        LeanTouch.OnFingerUpdate -= HandleFingerUpdate;
        LeanTouch.OnFingerDown += HandleFingerDown;
        LeanTouch.OnFingerUp -= HandleFingerUp;
    }

}
