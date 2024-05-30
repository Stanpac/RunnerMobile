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
    
    [BoxGroup("Data Settings")]
    [SerializeField] private SO_RaycastSuspension raycastSuspensionData;
    
    // TODO : Speed need to be set by the player controller i think ? 
    
    private bool rayDidHit;
    private RaycastHit tireRay;

    private void Awake()
    {
        tireTransform = GetComponent<Transform>();
        carRigidbody = GetComponentInParent<Rigidbody>();
        carTransform = carRigidbody.gameObject.transform;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (Physics.Raycast(tireTransform.position, -tireTransform.up, out tireRay, raycastSuspensionData.suspensionRestDist + 0.1f))  {
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
            float offset = raycastSuspensionData.suspensionRestDist - tireRay.distance;
            
            // calculate velocity along the spring direction
            // note that springDir is a unit vector, so this returns the magnitude of tireWorldVel
            // as project onto springDir
            float vel = Vector3.Dot(springDir, tireWorldVel);
            
            // Calculate the magnitude of the dampened spring force !
            float force = (offset * raycastSuspensionData.springStrength) - (vel * raycastSuspensionData.springDamper);
            
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
            float desiredChange = -steeringVel * raycastSuspensionData.tireGripFactor;
            
            // turn change in velocity into a accelration (acceleration = change in vel / time)
            // this will produce the accelartion necessary to change the velocity by the desiredvelChange in 1 physics step
            float desiredAccel = desiredChange / Time.fixedDeltaTime;
            
            // Force = Mass * Acceleration, so multiply by the mass of the tire and apply as a force !
            carRigidbody.AddForceAtPosition(steeringDir * raycastSuspensionData.tireMass * desiredAccel, tireTransform.position);
            
            if (DebugMode) 
            Debug.DrawRay(tireTransform.position, (steeringDir * raycastSuspensionData.tireMass * desiredAccel).normalized , Color.red);
        }
    }

    private void Accelaration()
    {
        // Acceleration / breaking force
        if (rayDidHit) {
            
            Vector3 accelDir = tireTransform.forward;
            
            // Acceleration torque
            if (raycastSuspensionData.CarSpeed > 0.0f) {
                
                // forward speed of the car (in the direction of driving)
                float carspeed = Vector3.Dot(carTransform.forward, carRigidbody.velocity);
                
                // normlized car speed
                float normalizedSpeed =  Mathf.Clamp01(Mathf.Abs((carspeed ) / raycastSuspensionData.carTopSpeed));
                
                // Available torque 
                float availableTorque = raycastSuspensionData.powerCurve.Evaluate(normalizedSpeed) * raycastSuspensionData.CarSpeed;
                
                carRigidbody.AddForceAtPosition(accelDir * availableTorque, tireTransform.position);
                
                if (DebugMode) 
                Debug.DrawRay(tireTransform.position, (accelDir * availableTorque).normalized , Color.blue);
            }
        }
    }
    
}
