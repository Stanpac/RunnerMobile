using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

// This script is responsible for managing the stability of the player.

//TODO - Scriptable Object for the stability of the player
public class Stability : MonoBehaviour
{
    [SerializeField] 
    private float _stability;
    
    [SerializeField, HideInInspector]
    private float _maxStability = 1;
    
    [SerializeField, HideInInspector] 
    private float _minStability = -1;
    
    // to ADD
    // % at Wich the stability is outside the limit and the player is considered unstable 
    
    [SerializeField,Tooltip(" The rate in sec at which the stability of the player regenerates")]
    private float _stabilityRegen = 0.1f;
    
    [SerializeField]
    private float _StabilityMultiplicator = 1f;
    
    [SerializeField]
    private float _stabilityRegenStopTime = 1;
    
    [SerializeField] 
    private bool _CanRegen = true;
    

    // Private Variables
    private Transform _CarModel;
    
    private void Awake()
    {
        _CarModel = GetComponent<PlayerController>().CarModel;
    }

    void Start()
    {
        _stability = 0;
        _CanRegen = true;
    }
    
    void Update()
    {
        if (_CanRegen) {
            RegenStability();
        }
        
        _stability = Mathf.Clamp(_stability, _minStability, _maxStability);
        // Send Callbak to the UI of the _Stability of the player
    }
    
    void RegenStability()
    {
        float stabilityregen = _stabilityRegen * Time.deltaTime * _StabilityMultiplicator;
        if (_stability < 0) {
            AddStability(stabilityregen, true);
        } else if (_stability > 0) {
            RemoveStability(stabilityregen, true);
        }
    }
    
    private void AddStability(float value, bool ClampToZero)
    {
        _stability = _stability + value > 0 && ClampToZero ? 0 : _stability + value;
    }
    
    private void RemoveStability(float value, bool ClampToZero)
    {
        _stability = _stability - value < 0 && ClampToZero ? 0 : _stability - value;
    }
    
    void ResetStability()
    {
        _stability = 0;
    }
    
    IEnumerator StopRegenStability(float time)
    {
        _CanRegen = false;
        yield return new WaitForSeconds(time);
        _CanRegen = true;
    }
    
}
