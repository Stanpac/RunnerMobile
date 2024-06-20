using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
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
    private CarController _carController;
    
    public float _stability = 0;
    private float _maxStability = 1;
    private float _minStability = -1;
    private float _previousStability = 0;
    
    private float _stabilityWeightMultiplicator = 1;
    private float _stabilitySideMultiplicator = 1;
    
    private bool _regenarate = true;
    private bool _unstable = false;
    
    private Coroutine _stopRegenStabilityCoroutine;
    
    
    private string _dataPath => "ScriptableObject/SO_Stability";
    
    private bool _fingerOnScreen = false;
    private float _timerFingerOnScreen = 0;
    private LeanFinger _currentfinger;
    
    private Coroutine _timerFingerOnScreenCoroutine;
    private Coroutine _timerFingerOffScreenCoroutine;
    
    public AnimationCurve _instabilityInputTimeCurve;
    
    // Min = 0
    public float _timeForReachMaxinstability = 3;
    
    private void Awake()
    {
        if (_data == null)
            _data = Resources.Load<SO_Stability>(_dataPath);
        
        _carTransform = GetComponent<CarController>().transform;
        _carController = GetComponent<CarController>();
        ResetStability();
    }
    
    private void Update()
    {
        if (_regenarate) {
            RegenStability();
        }
        
        _stability = CalculateRotationInstability() + CalculateEvents() + CalculateTerrain();
        _stability = Mathf.Clamp(_stability, _minStability, _maxStability);
        
        CheckifUnstable();
        if (_previousStability != _stability) {
            GameManager._instance.actionManager.StabilityChange(_stability);
        }
        _previousStability = _stability;
    }
    
    private float CalculateRotationInstability()
    {
        float normalizedTimer = Mathf.Clamp01(Mathf.Abs(_timerFingerOnScreen / _timeForReachMaxinstability));
        float stability = _instabilityInputTimeCurve.Evaluate(normalizedTimer);
        if (_fingerOnScreen) {
            if (_currentfinger.ScreenPosition.x > Screen.width / 2) {
                _stabilitySideMultiplicator = 1;
            } else {
                _stabilitySideMultiplicator = -1;
            }
        }
        stability *= _stabilitySideMultiplicator * _stabilityWeightMultiplicator;
        return normalizedTimer; 
    }
    
    private float CalculateEvents()
    {
        // TODO: Implement this with create trigger box for events
        return 0;
    }
    private float CalculateTerrain()
    {
        // TODO: implement this with raycast to check the terrain orientation ?
        return 0;
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
    
    private IEnumerator TimerFingerOnScreen()
    {
        while (_fingerOnScreen) {
            _timerFingerOnScreen += Time.deltaTime;
            _timerFingerOnScreen = Mathf.Clamp(_timerFingerOnScreen, 0 , _timeForReachMaxinstability); 
            yield return null;
        } 
    }  
    
    private IEnumerator TimerFingerOffScreen()
    {
        while (!_fingerOnScreen) {
            _timerFingerOnScreen -= Time.deltaTime;
            _timerFingerOnScreen = Mathf.Clamp(_timerFingerOnScreen, 0 , _timeForReachMaxinstability); 
            yield return null;
        } 
    }
    
    // TODO : there is the Same code in CarController.cs, find a way to factor this
    // Maybe all this need to be in the InputManager and the CarController need to be a listener of the InputManager ? and check Current finger in the InputManager
    private void OnFingerDown(LeanFinger finger)
    {
        if (finger.IsOverGui) return;
        
        _fingerOnScreen = true;
        
        if (_timerFingerOnScreenCoroutine == null) {
            if (_timerFingerOffScreenCoroutine != null) {
                StopCoroutine(_timerFingerOffScreenCoroutine);
                _timerFingerOffScreenCoroutine = null;
                Debug.Log("Stop Timer Finger Off Screen");
            }
            _timerFingerOnScreenCoroutine = StartCoroutine(TimerFingerOnScreen());
        }
        
        if (_currentfinger == null || !_currentfinger.Set) {
            _currentfinger = finger;
        }
    }
    
    private void OnLastFingerUp(LeanFinger finger)
    {
        _fingerOnScreen = false;
        _currentfinger = null;
        
        if (_timerFingerOffScreenCoroutine == null) {
            if (_timerFingerOnScreenCoroutine != null) {
                StopCoroutine(_timerFingerOnScreenCoroutine);
                _timerFingerOnScreenCoroutine = null;
                Debug.Log("Stop Timer Finger On Screen");
            }
            _timerFingerOffScreenCoroutine = StartCoroutine(TimerFingerOffScreen());
        }
    }

    private void OnEnable()
    {
        GameManager._instance.actionManager.OnFingerDown += OnFingerDown;
        GameManager._instance.actionManager.OnFirstFingerDown += OnFingerDown;
        GameManager._instance.actionManager.OnLastFingerUp += OnLastFingerUp;
    }
    
    private void OnDisable()
    {
        GameManager._instance.actionManager.OnFingerDown -= OnFingerDown;
        GameManager._instance.actionManager.OnFirstFingerDown -= OnFingerDown;
        GameManager._instance.actionManager.OnLastFingerUp -= OnLastFingerUp;
    }
}

public enum EStabilityImpactSide
{
    EIS_Left,
    EIS_Right,
}

