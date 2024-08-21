using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;


public class LuggageHandler : MonoBehaviour
{
    [SerializeField]
    [Tooltip("all the current luggage of the player")]
    private Luggagelibrary _luggageLibrary;
    
    [SerializeField][BoxGroup("Parameters")]
    private float _timeBeforeLosingBaggage = 1.0f;
    
    [SerializeField][BoxGroup("Parameters")]
    private int _nbrOfLuggageAtStart = 10;
    
    [SerializeField][BoxGroup("Parameters")][ReadOnly]
    private float _currentWeight = 0;
    
    // Timer key
    private string _timerUnstabilityKey;
    
    public float GetCurrentWeightInstead => _currentWeight;
    
    private void Awake()
    {
        
    }
    
    private void Start()
    {
        if (GameManager.Instance.NewGame) {
            _luggageLibrary = new Luggagelibrary(GameManager.Instance.LuggageCategories.GetAllLuggages());
            _luggageLibrary.AddLuggages(GameManager.Instance.LuggageCategories.PickLuggages(_nbrOfLuggageAtStart));
        } else {
            // TODO : load the luggage library from the save
        }
        LuggageIsUpdated();
    }

    // add specific luggage
    public void AddLuggage(Luggage luggage, int count)
    {
        _luggageLibrary.AddLuggage(luggage, count);
        LuggageIsUpdated();
    }
    
    // add random luggage
    public void AddRandomLuggage(int count)
    {
        _luggageLibrary.AddLuggages(GameManager.Instance.LuggageCategories.PickLuggages(count));
        LuggageIsUpdated();
    }
    
    // remove lowest stability luggage 
    public void RemoveLowestStabilityLuggages(int count)
    {
        _luggageLibrary.RemoveLowestStabilityLuggages(count);
        LuggageIsUpdated();
    }
    
    // remove lowest stability luggage in a specific category
    public void RemoveLuggageIn(string categoryName)
    {
        _luggageLibrary.RemoveLowestStabilityLuggageIn(categoryName);
        LuggageIsUpdated();
    }
    
    public void SetLuggage(int luggage)
    {
        LuggageIsUpdated();
    }
    
    private void UpdateCurrentWeight()
    {
        _currentWeight = _luggageLibrary?.GetTotalWeight() ?? -1;
    }
    
    private void LuggageIsUpdated()
    {
        UpdateCurrentWeight();
        GameManager.Instance.ActionManager.InvokeLuggageUpdate(_currentWeight);
    }
    
    public IEnumerator TimerUnstability()
    {
        while (true) {
            yield return new WaitForSeconds(_timeBeforeLosingBaggage);
            RemoveLowestStabilityLuggages(1);
        }
    }
    
    private void OnUnstableChange(bool unstable)
    {
        if (unstable) {
            if (!GameManager.Instance.TimerManager.IsTimerRunning(_timerUnstabilityKey)) {
                _timerUnstabilityKey = GameManager.Instance.TimerManager.StartTimer(TimerUnstability());
            } else {
                Debug.LogWarning("Timer already running, he should not be running", this);
            }
        } else {
            if (GameManager.Instance.TimerManager.IsTimerRunning(_timerUnstabilityKey)) {
                GameManager.Instance.TimerManager.StopTimer(_timerUnstabilityKey);
            } else {
                Debug.LogWarning("Timer not running, he should be running", this);
            }
        }
    }
    
    private void OnEnable()
    {
        GameManager.Instance.ActionManager.UnstableUpdate += OnUnstableChange;
    }

    private void OnDisable()
    {
        if (GameManager.Instance == null) return;
        GameManager.Instance.ActionManager.UnstableUpdate -= OnUnstableChange;
    }
}
