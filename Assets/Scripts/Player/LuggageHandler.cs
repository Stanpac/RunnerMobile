using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;


public class LuggageHandler : MonoBehaviour
{
    public float _timeBeforeLosingBaggage = 1.0f;
    public int _startluggage = 20;
    
    
    private string _timerUnstabilityKey;
    private int _luggage = 0;
    
    private void Awake()
    {
        SetLuggage(_startluggage);
    }

    public void AddLuggage(int luggage)
    {
        _luggage += luggage;
        LuggageIsUpdated();
    }

    public void RemoveLuggage(int luggage)
    {
        _luggage -= luggage;
        if (_luggage <= 0)  {
            _luggage = 0;
            GameManager.Instance.actionManager.PlayerDeath();
        }
        LuggageIsUpdated();
    }
    
    public void SetLuggage(int luggage)
    {
        _luggage = luggage;
        LuggageIsUpdated();
    }
    
    private void LuggageIsUpdated()
    {
        GameManager.Instance.actionManager.LuggageChange(_luggage);
    }
    
    public IEnumerator TimerUnstability()
    {
        while (true) {
            yield return new WaitForSeconds(_timeBeforeLosingBaggage);
            RemoveLuggage(1);
        }
    }
    
    private void OnUnstableChange(bool unstable)
    {
        if (unstable) {
            if (!GameManager.Instance.timerManager.IsTimerRunning(_timerUnstabilityKey)) {
                _timerUnstabilityKey = GameManager.Instance.timerManager.StartTimer(TimerUnstability());
            } else {
                Debug.LogWarning("Timer already running, he should not be running", this);
            }
        } else {
            if (GameManager.Instance.timerManager.IsTimerRunning(_timerUnstabilityKey)) {
                GameManager.Instance.timerManager.StopTimer(_timerUnstabilityKey);
            } else {
                Debug.LogWarning("Timer not running, he should be running", this);
            }
        }
    }
    
    private void OnEnable()
    {
        GameManager.Instance.actionManager.OnUnstableChange += OnUnstableChange;
    }
    
    private void OnDisable()
    {
        GameManager.Instance.actionManager.OnUnstableChange -= OnUnstableChange;
    }
}

public struct SLugagge
{
    private string _name;
    private SLuggageType _type;
    private float _weight;
    
    SLugagge(string name, SLuggageType type, float weight)
    {
        _name = name;
        _type = type;
        _weight = weight;
    }
    
    public string Name => _name;
    public SLuggageType Type => _type;
    public float Weight => _weight;
}

public enum SLuggageType
{
    Fragile,
    Heavy,
    Light,
    Normal
}
