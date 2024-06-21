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
    
    // Need to be Move 
    private float _timerFingerOnScreen = 0;
    private LeanFinger _currentfinger;
    
    // Reference to Car
    private CarController _carController;
    
    // Instability Variables
    public float _stability = 0;
    private float _previousStability = 0;
    private float _maxStability = 1;
    private float _minStability = -1;
    private bool _unstable = false;
    
    // Multiplicator for the stability
    private float _stabilityWeightMultiplicator = 1;
    private float _stabilitySideMultiplicator = 1;
    
    // Timer keys
    private string _timerFingerOnScreenKey;
    private string _timerFingerOffScreenKey;
    
    // Path to the Data
    private string _dataPath => "ScriptableObject/SO_Stability";
    
    private void Awake()
    {
        if (_data == null)
            _data = Resources.Load<SO_Stability>(_dataPath);
        
        _carController = GetComponent<CarController>();
        ResetStability();
    }
    
    private void Update()
    {
        _stability = CalculateRotationInstability() + CalculateEvents() + CalculateTerrain();
        _stability = Mathf.Clamp(_stability, _minStability, _maxStability);
        
        CheckifUnstable();
        if (_previousStability != _stability) {
            GameManager.Instance.actionManager.StabilityChange(_stability);
        }
        _previousStability = _stability;
    }
    
    private float CalculateRotationInstability()
    {
        float normalizedTimer = Mathf.Clamp01(Mathf.Abs(_timerFingerOnScreen / _data.timeForReachMaxinstability));
        float stability =_data.instabilityInputTimeCurve.Evaluate(normalizedTimer);
        if (GameManager.Instance.inputManager.IsFingerOnScreen() && _currentfinger != null){
            if (_currentfinger.ScreenPosition.x > Screen.width / 2) {
                _stabilitySideMultiplicator = 1;
            } else {
                _stabilitySideMultiplicator = -1;
            }
        }
        
        stability *= _stabilitySideMultiplicator * _stabilityWeightMultiplicator;
        return stability; 
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
        bool CheckUpdate = _unstable;
        if (_stability > _data.instabilityThreshold || _stability < -_data.instabilityThreshold) {
            _unstable = true;
        } else {
            _unstable = false;
        }
        
        if (CheckUpdate != _unstable) {
            GameManager.Instance.actionManager.UnstableChange(_unstable);
        }
    }
    
    public void ImpactStability(float value, EStabilityImpactSide side)
    {
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
        GameManager.Instance.actionManager.StabilityChange(_stability);
    }
    
    private IEnumerator TimerFingerOnScreen()
    {
        while (GameManager.Instance.inputManager.IsFingerOnScreen()) {
            _timerFingerOnScreen += Time.deltaTime;
            _timerFingerOnScreen = Mathf.Clamp(_timerFingerOnScreen, 0 , _data.timeForReachMaxinstability);
            yield return new WaitForEndOfFrame();
        } 
    }  
    
    private IEnumerator TimerFingerOffScreen()
    {
        while (!GameManager.Instance.inputManager.IsFingerOnScreen()) {
            _timerFingerOnScreen -= Time.deltaTime;
            _timerFingerOnScreen = Mathf.Clamp(_timerFingerOnScreen, 0 , _data.timeForReachMaxinstability); 
            yield return new WaitForEndOfFrame();
        } 
    }
    
    private void OnFingerDown(LeanFinger finger)
    {
        if (finger.IsOverGui) return;

        if (!GameManager.Instance.timerManager.IsTimerRunning(_timerFingerOnScreenKey))  {
            if (GameManager.Instance.timerManager.IsTimerRunning(_timerFingerOffScreenKey)) {
                GameManager.Instance.timerManager.StopTimer(_timerFingerOffScreenKey);
            }
            _timerFingerOnScreenKey = GameManager.Instance.timerManager.StartTimer(TimerFingerOnScreen());
        }
        
        if (_currentfinger == null || !_currentfinger.Set) {
            _currentfinger = finger;
        }
    }
    
    private void OnLastFingerUp(LeanFinger finger)
    {
        _currentfinger = null;
        if (!GameManager.Instance.timerManager.IsTimerRunning(_timerFingerOffScreenKey))  {
            if (GameManager.Instance.timerManager.IsTimerRunning(_timerFingerOnScreenKey)) {
                GameManager.Instance.timerManager.StopTimer(_timerFingerOnScreenKey);
            }
            _timerFingerOffScreenKey = GameManager.Instance.timerManager.StartTimer(TimerFingerOffScreen());
        }
    }

    private void OnEnable()
    {
        GameManager.Instance.actionManager.OnFingerDown += OnFingerDown;
        GameManager.Instance.actionManager.OnFirstFingerDown += OnFingerDown;
        GameManager.Instance.actionManager.OnLastFingerUp += OnLastFingerUp;
    }
    
    private void OnDisable()
    {
        GameManager.Instance.actionManager.OnFingerDown -= OnFingerDown;
        GameManager.Instance.actionManager.OnFirstFingerDown -= OnFingerDown;
        GameManager.Instance.actionManager.OnLastFingerUp -= OnLastFingerUp;
    }
}

public enum EStabilityImpactSide
{
    EIS_Left,
    EIS_Right,
}

