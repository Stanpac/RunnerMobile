using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

/*------------------- Struct / enum -------------------*/
/*------------------- End Struct / enum -------------------*/
/*
 * This script is responsible for managing the player.
 */
public class PlayerController : MonoBehaviour
{
    /*------------------- public / SerializeField variable -------------------*/
    
    [SerializeField, BoxGroup("PlayerSpeed")] private float _speed;
    [SerializeField, BoxGroup("Ground"), Tag] private String _groundTag;
    
    /*------------------- End public / SerializeField variable -------------------*/
    /*------------------- Private Variables -------------------*/
    
    private Vector3 _groundDirection;
    
    /*------------------- End Private Variables -------------------*/
    private void Awake()
    {
        
    }

    void Start()
    {
        
    }
    
    void Update()
    { 
        Vector3 direction = CalculateMovementDirection() * (_speed / 100);
    }

    private Vector3 CalculateMovementDirection()
    {
        Vector3 _direction = transform.forward * Time.deltaTime;
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up + _direction, out hit, 10f)) {
            if (hit.collider.gameObject.CompareTag(_groundTag))  {
                _direction = Vector3.ProjectOnPlane(_direction, hit.normal).normalized;
            } else {
                Debug.LogError("Ground not found", this);
            }
        }

        return - _direction;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, _groundDirection.normalized);
    }
}
