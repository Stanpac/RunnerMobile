using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using ScriptableObjects;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

// This script is responsible for managing the stability of the player.
public class Stability : MonoBehaviour
{
    [SerializeField]
    private SO_Stability _data;
    
    // Private Variables
    private Transform _carTransform;
    
    private float _stability = 0;
    private float _maxStability = 1;
    private float _minStability = -1;
    private float _previousStability = 0;
    
    private float _stabilityWeightMultiplicator = 1;
    
    private bool _regenarate = true;
    private bool _unstable = false;
    
    private Coroutine _stopRegenStabilityCoroutine;
    
    
    private void Awake()
    {
        _data = Resources.Load<SO_Stability>("SO_Stability");
        
        _carTransform = GetComponent<PlayerController>().transform;
        ResetStability();
    }

    private void Start()
    {
        
    }
    
    private void Update()
    {
        if (_regenarate) {
            RegenStability();
        }
        
        _stability = Mathf.Clamp(_stability, _minStability, _maxStability);
        CheckifUnstable();
        if (_previousStability != _stability) {
            GameManager._instance.actionManager.StabilityChange(_stability);
        }
        _previousStability = _stability;
    }
    
    private void CheckifUnstable()
    {
        if (_stability > _data.instabilityThreshold || _stability < -_data.instabilityThreshold) {
            _unstable = true;
        } else {
            _unstable = false;
        }
    }
    
    private void RegenStability()
    {
        float stabilityregen = _data.stabilityRegen * Time.deltaTime * 1/_stabilityWeightMultiplicator;
        if (_stability < 0) {
            AddStability(stabilityregen, true, true);
        } else if (_stability > 0) {
            RemoveStability(stabilityregen, true, true);
        }
    }
    
    public void ImpactStability(float value, EStabilityImpactSide side)
    {
        if (value > _data.stabilityForce) {
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
        _regenarate = false;
        yield return new WaitForSeconds(time);
        _regenarate = true;
    }
    
    private void StartTimerForRegenStability()
    {
        if (_stopRegenStabilityCoroutine != null) {
            StopCoroutine(_stopRegenStabilityCoroutine);
        }
        _stopRegenStabilityCoroutine = StartCoroutine(StopRegenStability(_data.stabilityRegenStopTime));
    }
}

public enum EStabilityImpactSide
{
    EIS_Left,
    EIS_Right,
}

