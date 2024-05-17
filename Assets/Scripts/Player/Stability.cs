using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using ScriptableObjects;
using Unity.VisualScripting;
using UnityEngine;

// This script is responsible for managing the stability of the player.
public class Stability : MonoBehaviour
{
    [SerializeField]
    private SO_Stability _SO_Stability;
    
    // Private Variables
    private Transform _CarModel;
    
    private float _stability = 0;
    private float _maxStability = 1;
    private float _minStability = -1;
    private float _previousStability = 0;
    
    private float _StabilityWeightMultiplicator = 1;
    
    private bool _CanRegen = true;
    private bool _IsUnstable = false;
    
    private Coroutine _StopRegenStabilityCoroutine;
    
    private void Awake()
    {
        _CarModel = GetComponent<PlayerController>().CarModel;
        ResetStability();
    }

    private void Start()
    {
        
    }
    
    private void Update()
    {
        if (_CanRegen) {
            RegenStability();
        }
        
        _stability = Mathf.Clamp(_stability, _minStability, _maxStability);
        
        CheckifUnstable();
        if (_previousStability != _stability) {
            GameManager.instance.actionManager.StabilityChange(_stability);
        }
        _previousStability = _stability;
    }
    
    private void CheckifUnstable()
    {
        if (_stability > _SO_Stability._instabilityThreshold || _stability < -_SO_Stability._instabilityThreshold) {
            _IsUnstable = true;
        } else {
            _IsUnstable = false;
        }
    }
    
    private void RegenStability()
    {
        float stabilityregen = _SO_Stability._stabilityRegen * Time.deltaTime * 1/_StabilityWeightMultiplicator;
        if (_stability < 0) {
            AddStability(stabilityregen, true, true);
        } else if (_stability > 0) {
            RemoveStability(stabilityregen, true, true);
        }
    }
    
    public void ImpactStability(float value, EStabilityImpactSide side)
    {
        if (value > _SO_Stability._stabilityForce) {
            StartTimerForRegenStability();
        }
        
        if (side == EStabilityImpactSide.EIS_Left) {
            RemoveStability(value, false, true);
        } else if (side == EStabilityImpactSide.EIS_Right) {
            AddStability(value, false, true);
        } else {
            Debug.LogError("No Side to impact specified");
        }
    }
    
    private void AddStability(float value, bool ClampToZero, bool ClampToMax)
    {
        float NewStability = _stability + value > 0 && ClampToZero ? 0 : _stability + value;
        NewStability = _stability > _maxStability && ClampToMax ? _maxStability : _stability;
        _stability = NewStability;
    }
    
    private void RemoveStability(float value, bool ClampToZero, bool ClampToMin)
    {
        float NewStability = _stability - value < 0 && ClampToZero ? 0 : _stability - value;
        NewStability = _stability < _minStability && ClampToMin ? _minStability : _stability;
        _stability = NewStability;
    }
    
    private void ResetStability()
    {
        _stability = 0;
    }
    
    private IEnumerator StopRegenStability(float time)
    {
        _CanRegen = false;
        yield return new WaitForSeconds(time);
        _CanRegen = true;
    }
    
    private void StartTimerForRegenStability()
    {
        if (_StopRegenStabilityCoroutine != null) {
            StopCoroutine(_StopRegenStabilityCoroutine);
        }
        _StopRegenStabilityCoroutine = StartCoroutine(StopRegenStability(_SO_Stability._stabilityRegenStopTime));
    }
}

public enum EStabilityImpactSide
{
    EIS_Left,
    EIS_Right,
}

