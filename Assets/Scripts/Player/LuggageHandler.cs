using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;


public class LuggageHandler : MonoBehaviour
{
    public float _timeBeforeLosingBaggage = 1.0f;
    
    private string _timerUnstabilityKey;
    
    private int _luggage = 0;
    public int Luggage => _luggage;
    
    private void Awake()
    {
        SetLuggage(20);
    }

    public void AddLuggage(int luggage)
    {
        _luggage += luggage;
        GameManager.Instance.actionManager.LuggageChange(_luggage);
    }

    public void RemoveLuggage(int luggage)
    {
        _luggage -= luggage;
        if (_luggage <= 0)  {
            _luggage = 0;
            GameManager.Instance.actionManager.PlayerDeath();
        }
        GameManager.Instance.actionManager.LuggageChange(_luggage);
    }
    
    public void SetLuggage(int luggage)
    {
        _luggage = luggage;
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
