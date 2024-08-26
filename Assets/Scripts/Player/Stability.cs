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
    
    // Debug Settings
    [SerializeField, BoxGroup("Debug Settings")]
    private bool _stabilityInput = true;
    [SerializeField, BoxGroup("Debug Settings")]
    private bool _stabilityRotation = true;
    [SerializeField, BoxGroup("Debug Settings")]
    private bool _stabilityEvent = true;
    
    // temp
    public float _maxRotationZ = 20.0f;
    public float _multiplicator = 1;
    
    // Need to be Move 
    private float _timerFingerOnScreen = 0;
    private LeanFinger _currentfinger;

    private float _timeForReachMaxInputInstability;
    
    // Reference to Car
    private CarController _carController;
    
    // Instability Variables
    public float _stability = 0;
    private float _previousStability = 0;
    private float _maxStability = 1;
    private float _minStability = -1;
    private bool _unstable = false;
    
    // Stability rotation 
    private float _stabilityWeightMultiplicator = 1;
    private float _stabilityInputMultiplicator = 1;
    
    // Stability Terrain
    private float _stabilityTerrainMultiplicator = 1;
    
    // Stability Event
    private float _stabilityEventMultiplicator = 1;
    
    // Timer keys
    private string _timerFingerOnScreenKey;
    private string _timerFingerOffScreenKey;
    
    // Path to the Data
    private string DataPath => "ScriptableObject/SO_Stability";
    
    private void Awake()
    {
        if (_data == null)
            _data = Resources.Load<SO_Stability>(DataPath);
        
        _carController = GetComponent<CarController>();
        ResetStability();
    }
    
    private void Update()
    {
        _stability = CalculateInputInstability() + CalculateRotationStability() + CalculateEvents();
        _stability = Mathf.Clamp(_stability, _minStability, _maxStability);
        
        CheckifUnstable();
        if (_previousStability != _stability) {
            GameManager.Instance.ActionManager.InvokeStabilityUpdate(_stability);
        }
        _previousStability = _stability;
    }
    
    private float CalculateInputInstability()
    {
        if (!_stabilityInput) return 0;
        
        float normalizedTimer = Mathf.Clamp01(Mathf.Abs(_timerFingerOnScreen / _data.timeForReachMaxInputInstability));
        float stability =_data.instabilityInputTimeCurve.Evaluate(normalizedTimer);
        
        if (GameManager.Instance.InputManager.IsFingerOnScreen() && _currentfinger != null){
            if (_currentfinger.ScreenPosition.x > Screen.width / 2) {
                _stabilityInputMultiplicator = 1;
            } else {
                _stabilityInputMultiplicator = -1;
            }
        }
        
        stability *= _stabilityInputMultiplicator * _stabilityWeightMultiplicator;
        return stability; 
    }
    
    private float CalculateRotationStability()
    {
        if (!_stabilityRotation) return 0;
        
        if (_carController == null) {
            Debug.LogError("No Car Controller found");
            return 0;
        }
        
        // Difference entre la value Z de rotation max et la rotation actuelle Z
        float rotation = _carController.transform.rotation.z;
        float normalizedRotation = Mathf.Clamp01(Mathf.Abs(rotation / _maxRotationZ));
        
        
        if (rotation > 0) {
            return normalizedRotation * 1 * _multiplicator;
        } else {
            return normalizedRotation * -1 * _multiplicator;
        }
    }
    
    private float CalculateEvents()
    {
        if (!_stabilityEvent) return 0;
        
        // TODO: Implement this with create trigger box for events
        return 0;
    }
    
    private void CheckifUnstable()
    {
        bool checkUpdate = _unstable;
        if (_stability > _data.instabilityThreshold || _stability < -_data.instabilityThreshold) {
            _unstable = true;
        } else {
            _unstable = false;
        }
        
        if (checkUpdate != _unstable) {
            GameManager.Instance.ActionManager.InvokeUnstableUpdate(_unstable);
        }
    }
    
    private void ResetStability()
    {
        _stability = 0;
        GameManager.Instance.ActionManager.InvokeStabilityUpdate(_stability);
    }
    
    public void ImpactStability(float value, EStabilityImpactSide side)
    {
        if (side == EStabilityImpactSide.Left) {
            RemoveStability(value, false, true);
        } else if (side == EStabilityImpactSide.Right) {
            AddStability(value, false, true);
        } else {
            Debug.LogError("No Side to impact specified");
        }
    }
    
    private void AddStability(float value, bool clampToZero, bool clampToMax)
    {
        float newStability = _stability + value > 0 && clampToZero ? 0 : _stability + value;
        newStability = _stability > _maxStability && clampToMax ? _maxStability : _stability;
        _stability = newStability;
    }
    
    private void RemoveStability(float value, bool clampToZero, bool clampToMin)
    {
        float newStability = _stability - value < 0 && clampToZero ? 0 : _stability - value;
        newStability = _stability < _minStability && clampToMin ? _minStability : _stability;
        _stability = newStability;
    }
    
    private IEnumerator TimerFingerOnScreen()
    {
        while (GameManager.Instance.InputManager.IsFingerOnScreen()) {
            _timerFingerOnScreen += Time.deltaTime;
            _timerFingerOnScreen = Mathf.Clamp(_timerFingerOnScreen, 0 , _data.timeForReachMaxInputInstability);
            yield return new WaitForEndOfFrame();
        } 
    }  
    
    private IEnumerator TimerFingerOffScreen()
    {
        while (!GameManager.Instance.InputManager.IsFingerOnScreen()) {
            _timerFingerOnScreen -= Time.deltaTime;
            _timerFingerOnScreen = Mathf.Clamp(_timerFingerOnScreen, 0 , _data.timeForReachMaxInputInstability); 
            yield return new WaitForEndOfFrame();
        } 
    }
    
    private void OnFingerDown(LeanFinger finger)
    {
        if (finger.IsOverGui) return;

        if (!GameManager.Instance.TimerManager.IsTimerRunning(_timerFingerOnScreenKey))  {
            if (GameManager.Instance.TimerManager.IsTimerRunning(_timerFingerOffScreenKey)) {
                GameManager.Instance.TimerManager.StopTimer(_timerFingerOffScreenKey);
            }
            _timerFingerOnScreenKey = GameManager.Instance.TimerManager.StartTimer(TimerFingerOnScreen());
        }
        
        if (_currentfinger == null || !_currentfinger.Set) {
            _currentfinger = finger;
        }
    }
    
    private void OnLastFingerUp(LeanFinger finger)
    {
        _currentfinger = null;
        if (!GameManager.Instance.TimerManager.IsTimerRunning(_timerFingerOffScreenKey))  {
            if (GameManager.Instance.TimerManager.IsTimerRunning(_timerFingerOnScreenKey)) {
                GameManager.Instance.TimerManager.StopTimer(_timerFingerOnScreenKey);
            }
            _timerFingerOffScreenKey = GameManager.Instance.TimerManager.StartTimer(TimerFingerOffScreen());
        }
    }

    private void OnEnable()
    {
        GameManager.Instance.ActionManager.FingerDown += OnFingerDown;
        GameManager.Instance.ActionManager.FirstFingerDown += OnFingerDown;
        GameManager.Instance.ActionManager.LastFingerUp += OnLastFingerUp;
    }
    
    private void OnDisable()
    {
        if (GameManager.Instance == null) return;
        GameManager.Instance.ActionManager.FingerDown -= OnFingerDown;
        GameManager.Instance.ActionManager.FirstFingerDown -= OnFingerDown;
        GameManager.Instance.ActionManager.LastFingerUp -= OnLastFingerUp;
    }
}

public enum EStabilityImpactSide
{
    Left,
    Right,
    Forward,
    Backward,
}

