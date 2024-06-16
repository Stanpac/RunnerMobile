using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using ScriptableObjects;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;


public class RaycastSuspension : MonoBehaviour
{
    [SerializeField, BoxGroup("Debug")] 
    private bool _debugMode = false;
    
    [SerializeField, BoxGroup("Debug"), EnableIf("DebugMode")] 
    Rigidbody _carRigidbody;
    
    [SerializeField, BoxGroup("Debug"), EnableIf("DebugMode")] 
    Transform _tireTransform;
    
    [SerializeField, BoxGroup("Debug"), EnableIf("DebugMode")] 
    Transform _carTransform;
    
    
    [BoxGroup("Data Settings"), Label("Raycast Suspension Data")]
    [SerializeField] private SO_RaycastSuspension _data;
    
    private bool _rayDidHit;
    private RaycastHit _tireRay;
    
    private float _speedFactor = 1.0f;
    private float _carTopSpeed = 100f;
    private AnimationCurve _powerCurve = null;
    
    public void SetUpSpeedFactor(float speedFactor, float carTopSpeed, AnimationCurve powerCurve)
    {
        _speedFactor = speedFactor;
        _carTopSpeed = carTopSpeed;
        _powerCurve = powerCurve;
    }

    private void Reset()
    {
        if (_data == null)
            _data = Resources.Load<SO_RaycastSuspension>("SO_RaycastSuspension");
    }

    private void Awake()
    {
        if (_data == null)
            _data = Resources.Load<SO_RaycastSuspension>("SO_RaycastSuspension");
        
        _tireTransform = GetComponent<Transform>();
        _carRigidbody = GetComponentInParent<Rigidbody>();
        _carTransform = _carRigidbody.gameObject.transform;
    }

    private void FixedUpdate()
    {
        if (Physics.Raycast(_tireTransform.position, -_tireTransform.up, out _tireRay, _data.suspensionRestDist + 0.1f))  {
            _rayDidHit = true;
        } else {
            _rayDidHit = false;
        }
        
        SuspensionForce();
        StreeringForce();
        Accelaration();
    }
    
    private void SuspensionForce()
    {
        // Suspension spring force
        if (_rayDidHit) {
            
            // world space direction of the spring force.
            Vector3 springDir = _tireTransform.up;
            
            // World-space velocity of this tire.
            Vector3 tireWorldVel = _carRigidbody.GetPointVelocity(_tireTransform.position);
            
            // calculate offset from the raycast 
            float offset = _data.suspensionRestDist - _tireRay.distance;
            
            // calculate velocity along the spring direction
            // note that springDir is a unit vector, so this returns the magnitude of tireWorldVel
            // as project onto springDir
            float vel = Vector3.Dot(springDir, tireWorldVel);
            
            // Calculate the magnitude of the dampened spring force !
            float force = (offset * _data.springStrength) - (vel * _data.springDamper);
            
            // apply the force at the location of this tire in the direction 
            // of the suspension 
            _carRigidbody.AddForceAtPosition(springDir * force, _tireTransform.position);
            
#if UNITY_EDITOR         
            if (_debugMode) 
                Debug.DrawRay(_tireTransform.position, (springDir * force).normalized , Color.green);
#endif
        }   
    }

    private void StreeringForce()
    {
        // Steering force
        if (_rayDidHit) {
            
            // world space direction of the spring force.
            Vector3 steeringDir = _tireTransform.right;
            
            // World-space velocity of the suspension
            Vector3 tireWorldVel = _carRigidbody.GetPointVelocity(_tireTransform.position);
            
            // What it's the tire's velocity in the steering direction?
            // note that steeringDir is a unit vector, so this returns the magnitude of tireWorldVel
            // as project onto steeringDir
            float steeringVel = Vector3.Dot(steeringDir, tireWorldVel);
            
            // the change in velocity that we're looking for is -steeringVel * gripFactor
            float desiredChange = -steeringVel * _data.tireGripFactor;
            
            // turn change in velocity into a accelration (acceleration = change in vel / time)
            // this will produce the accelartion necessary to change the velocity by the desiredvelChange in 1 physics step
            float desiredAccel = desiredChange / Time.fixedDeltaTime;
            
            // Force = Mass * Acceleration, so multiply by the mass of the tire and apply as a force !
            _carRigidbody.AddForceAtPosition(steeringDir * _data.tireMass * desiredAccel, _tireTransform.position);
            
#if UNITY_EDITOR             
            if (_debugMode) 
                Debug.DrawRay(_tireTransform.position, (steeringDir * _data.tireMass * desiredAccel).normalized , Color.red);
#endif
        }
    }

    private void Accelaration()
    {
        // Acceleration / breaking force
        if (_rayDidHit) {
            
            Vector3 accelDir = _tireTransform.forward;
            
            // Acceleration torque
            if (_speedFactor > 0.0f) {
                
                // forward speed of the car (in the direction of driving)
                float carspeed = Vector3.Dot(_carTransform.forward, _carRigidbody.velocity);
                
                // normalized car speed
                float normalizedSpeed =  Mathf.Clamp01(Mathf.Abs((carspeed ) / _carTopSpeed));
                
                // Available torque 
                float availableTorque = (_powerCurve != null ? _powerCurve.Evaluate(normalizedSpeed) : _carTopSpeed) * _speedFactor;
                
                _carRigidbody.AddForceAtPosition(accelDir * availableTorque, _tireTransform.position);

#if UNITY_EDITOR
                if (_debugMode) 
                    Debug.DrawRay(_tireTransform.position, (accelDir * availableTorque).normalized , Color.blue);
#endif
            }
        }
    }
    
}
