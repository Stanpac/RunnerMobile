using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastSuspension : MonoBehaviour
{
    [SerializeField] Rigidbody _rigidbody;
    
    public float travel = 0.1f;
    public float Stiffness = 1000;
    public float Damper = 200;
    
    public float wheelRadius = 0.5f;

    private float _springVelocity;
    private bool _isGrounded;
    private float _lastLength;
    
    RaycastHit _hit;

    private float SpringForce()
    {
        return Stiffness * (travel - (_hit.distance - wheelRadius));
    }
    
    float DamperForce()
    {
        return Damper * _springVelocity;
    }

    private void FixedUpdate()
    {
        ShootRays();
    }
    
    private void ShootRays()
    {
        _lastLength = travel - _hit.distance - wheelRadius;
        if (Physics.Raycast(transform.position, -transform.up, out _hit, travel + wheelRadius))  {
            _springVelocity = ((travel- _hit.distance - wheelRadius) - _lastLength) / Time.fixedDeltaTime;
            ApplyForce();
            _isGrounded = true;
            Debug.DrawRay(transform.position, -transform.up * _hit.distance, Color.red);
        } else {
            _isGrounded = false;
            Debug.DrawRay(transform.position, -transform.up * (travel + wheelRadius), Color.yellow);
        }
    }
    
    private void ApplyForce()
    {
        _rigidbody.AddForceAtPosition(transform.up * (SpringForce()  + DamperForce()), transform.position, ForceMode.Force);
    }
}
