using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

public class RaycastSuspension : MonoBehaviour
{
    [SerializeField, BoxGroup("Debug")] private bool DebugMode = false;
    [SerializeField, BoxGroup("Debug"), EnableIf("DebugMode")] Rigidbody carRigidbody;
    [SerializeField, BoxGroup("Debug"), EnableIf("DebugMode")] Transform tireTransform;
    [SerializeField, BoxGroup("Debug"), EnableIf("DebugMode")] Transform carTransform;
    
    [BoxGroup("Data Settings"), Label("Raycast Suspension Data")]
    [SerializeField] private SO_RaycastSuspension Data;
    
    private bool rayDidHit;
    private RaycastHit tireRay;
    
    private float SpeedFactor = 1.0f;
    private float CarTopSpeed = 100f;
    private AnimationCurve PowerCurve = null;
    
    public void SetUpSpeedFactor(float speedFactor, float carTopSpeed, AnimationCurve powerCurve)
    {
        SpeedFactor = speedFactor;
        CarTopSpeed = carTopSpeed;
        PowerCurve = powerCurve;
    }
    
    private void Awake()
    {
        Data = Resources.Load<SO_RaycastSuspension>("SO_RaycastSuspension");
        
        tireTransform = GetComponent<Transform>();
        carRigidbody = GetComponentInParent<Rigidbody>();
        carTransform = carRigidbody.gameObject.transform;
    }

    private void FixedUpdate()
    {
        if (Physics.Raycast(tireTransform.position, -tireTransform.up, out tireRay, Data.suspensionRestDist + 0.1f))  {
            rayDidHit = true;
        } else {
            rayDidHit = false;
        }
        
        SuspensionForce();
        StreeringForce();
        Accelaration();
    }
    
    private void SuspensionForce()
    {
        // Suspension spring force
        if (rayDidHit) {
            
            // world space direction of the spring force.
            Vector3 springDir = tireTransform.up;
            
            // World-space velocity of this tire.
            Vector3 tireWorldVel = carRigidbody.GetPointVelocity(tireTransform.position);
            
            // calculate offset from the raycast 
            float offset = Data.suspensionRestDist - tireRay.distance;
            
            // calculate velocity along the spring direction
            // note that springDir is a unit vector, so this returns the magnitude of tireWorldVel
            // as project onto springDir
            float vel = Vector3.Dot(springDir, tireWorldVel);
            
            // Calculate the magnitude of the dampened spring force !
            float force = (offset * Data.springStrength) - (vel * Data.springDamper);
            
            // apply the force at the location of this tire in the direction 
            // of the suspension 
            carRigidbody.AddForceAtPosition(springDir * force, tireTransform.position);
            
            if (DebugMode) 
            Debug.DrawRay(tireTransform.position, (springDir * force).normalized , Color.green);
        }   
    }

    private void StreeringForce()
    {
        // Steering force
        if (rayDidHit) {
            
            // world space direction of the spring force.
            Vector3 steeringDir = tireTransform.right;
            
            // World-space velocity of the suspension
            Vector3 tireWorldVel = carRigidbody.GetPointVelocity(tireTransform.position);
            
            // What it's the tire's velocity in the steering direction?
            // note that steeringDir is a unit vector, so this returns the magnitude of tireWorldVel
            // as project onto steeringDir
            float steeringVel = Vector3.Dot(steeringDir, tireWorldVel);
            
            // the change in velocity that we're looking for is -steeringVel * gripFactor
            float desiredChange = -steeringVel * Data.tireGripFactor;
            
            // turn change in velocity into a accelration (acceleration = change in vel / time)
            // this will produce the accelartion necessary to change the velocity by the desiredvelChange in 1 physics step
            float desiredAccel = desiredChange / Time.fixedDeltaTime;
            
            // Force = Mass * Acceleration, so multiply by the mass of the tire and apply as a force !
            carRigidbody.AddForceAtPosition(steeringDir * Data.tireMass * desiredAccel, tireTransform.position);
            
            if (DebugMode) 
            Debug.DrawRay(tireTransform.position, (steeringDir * Data.tireMass * desiredAccel).normalized , Color.red);
        }
    }

    private void Accelaration()
    {
        // Acceleration / breaking force
        if (rayDidHit) {
            
            Vector3 accelDir = tireTransform.forward;
            
            // Acceleration torque
            if (SpeedFactor > 0.0f) {
                
                // forward speed of the car (in the direction of driving)
                float carspeed = Vector3.Dot(carTransform.forward, carRigidbody.velocity);
                
                // normalized car speed
                float normalizedSpeed =  Mathf.Clamp01(Mathf.Abs((carspeed ) / CarTopSpeed));
                
                // Available torque 
                float availableTorque = (PowerCurve != null ? PowerCurve.Evaluate(normalizedSpeed) : CarTopSpeed) * SpeedFactor;
                
                carRigidbody.AddForceAtPosition(accelDir * availableTorque, tireTransform.position);
                
                if (DebugMode) 
                Debug.DrawRay(tireTransform.position, (accelDir * availableTorque).normalized , Color.blue);
            }
        }
    }
    
}
